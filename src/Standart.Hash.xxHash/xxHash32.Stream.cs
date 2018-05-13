namespace Standart.Hash.xxHash
{
    using System.Buffers;
    using System.IO;
    using System.Threading.Tasks;

    public static partial class xxHash32
    {
        private const int min32 = 256;
        private const int div16 = 0x7FFFFFF0;
        
        /// <summary>
        /// Compute xxHash for the stream
        /// </summary>
        /// <param name="stream">The stream of data</param>
        /// <param name="bufferSize">The buffer size</param>
        /// <param name="seed">The seed number</param>
        /// <returns>The hash</returns>
        public static uint ComputeHash(Stream stream, int bufferSize = 4096, uint seed = 0)
        {
            // Go to the beginning of the stream
            stream.Seek(0, SeekOrigin.Begin);
            
            // Get length of the stream
            long length = stream.Length;

            // The buffer size can't be less than 256 bytes
            if (bufferSize < min32) bufferSize = min32;
            else bufferSize &= div16;

            // Calculate the number of chunks and the remain
            int chunks = (int) length / bufferSize;
            int remain = (int) length % bufferSize;
            int offset = bufferSize;

            // Calculate the offset
            if (remain != 0) chunks++;
            if (remain != 0 && remain < 16) offset -= 16;
            
            // Optimizing memory allocation
            byte[] buffer = ArrayPool<byte>.Shared.Rent(bufferSize);

            try
            {
                // Prepare the seed vector
                uint v1 = seed + p1 + p2;
                uint v2 = seed + p2;
                uint v3 = seed + 0;
                uint v4 = seed - p1;
                
                // Process chunks
                // Skip the last chunk. It will processed a little bit later
                for (int i = 2; i <= chunks; i++)
                { 
                    // Change bufferSize for the last read
                    if (i == chunks) bufferSize = offset;
                    
                    // Read the next chunk
                    stream.Read(buffer, 0, bufferSize);
                    
                    unsafe
                    {
                        fixed (byte* pData = &buffer[0])
                        {
                            byte* ptr = pData;
                            byte* end = pData + bufferSize;
                            
                            do
                            {
                                v1 += *((uint*)ptr) * p2;
                                v1 = (v1 << 13) | (v1 >> (32 - 13)); // rotl 13
                                v1 *= p1;
                                ptr += 4;
    
                                v2 += *((uint*)ptr) * p2;
                                v2 = (v2 << 13) | (v2 >> (32 - 13)); // rotl 13
                                v2 *= p1;
                                ptr += 4;
    
                                v3 += *((uint*)ptr) * p2;
                                v3 = (v3 << 13) | (v3 >> (32 - 13)); // rotl 13
                                v3 *= p1;
                                ptr += 4;
    
                                v4 += *((uint*)ptr) * p2;
                                v4 = (v4 << 13) | (v4 >> (32 - 13)); // rotl 13
                                v4 *= p1;
                                ptr += 4;
    
                            } while (ptr < end);
                        }
                    }
                }
                
                // Read the last chunk
                offset = stream.Read(buffer, 0, bufferSize);
                
                // Process the last chunk
                unsafe
                {
                    fixed (byte* pData = &buffer[0])
                    {
                        byte* ptr = pData;
                        byte* end = pData + offset;
                        uint h32;
    
                        if (length >= 16)
                        {
                            byte* limit = end - 16;
    
                            do
                            {
                                v1 += *((uint*) ptr) * p2;
                                v1 = (v1 << 13) | (v1 >> (32 - 13)); // rotl 13
                                v1 *= p1;
                                ptr += 4;
    
                                v2 += *((uint*) ptr) * p2;
                                v2 = (v2 << 13) | (v2 >> (32 - 13)); // rotl 13
                                v2 *= p1;
                                ptr += 4;
    
                                v3 += *((uint*) ptr) * p2;
                                v3 = (v3 << 13) | (v3 >> (32 - 13)); // rotl 13
                                v3 *= p1;
                                ptr += 4;
    
                                v4 += *((uint*) ptr) * p2;
                                v4 = (v4 << 13) | (v4 >> (32 - 13)); // rotl 13
                                v4 *= p1;
                                ptr += 4;
    
                            } while (ptr <= limit);
    
                            h32 = ((v1 << 1) | (v1 >> (32 - 1))) +   // rotl 1
                                  ((v2 << 7) | (v2 >> (32 - 7))) +   // rotl 7
                                  ((v3 << 12) | (v3 >> (32 - 12))) + // rotl 12
                                  ((v4 << 18) | (v4 >> (32 - 18)));  // rotl 18
                        }
                        else
                        {
                            h32 = seed + p5;
                        }
    
                        h32 += (uint) length;
    
                        while (ptr <= end - 4)
                        {
                            h32 += *((uint*) ptr) * p3;
                            h32 = ((h32 << 17) | (h32 >> (32 - 17))) * p4; // (rotl 17) * p4
                            ptr += 4;
                        }
    
                        while (ptr < end)
                        {
                            h32 += *((byte*) ptr) * p5;
                            h32 = ((h32 << 11) | (h32 >> (32 - 11))) * p1; // (rotl 11) * p1
                            ptr += 1;
                        }
    
                        h32 ^= h32 >> 15;
                        h32 *= p2;
                        h32 ^= h32 >> 13;
                        h32 *= p3;
                        h32 ^= h32 >> 16;
    
                        return h32;
                    }
                }
            }
            finally 
            {
                // Free memory
                ArrayPool<byte>.Shared.Return(buffer);
            }
        }
        
        /// <summary>
        /// Compute xxHash for the async stream
        /// </summary>
        /// <param name="stream">The stream of data</param>
        /// <param name="bufferSize">The buffer size</param>
        /// <param name="seed">The seed number</param>
        /// <returns>The hash</returns>
        public static async Task<uint> ComputeHashAsync(Stream stream, int bufferSize = 4096, uint seed = 0)
        {
            // Go to the beginning of the stream
            stream.Seek(0, SeekOrigin.Begin);
            
            // Get length of the stream
            long length = stream.Length;

            // The buffer size can't be less than 256 bytes
            if (bufferSize < min32) bufferSize = min32;
            else bufferSize &= div16;

            // Calculate the number of chunks and the remain
            int chunks = (int) length / bufferSize;
            int remain = (int) length % bufferSize;
            int offset = bufferSize;

            // Calculate the offset
            if (remain != 0) chunks++;
            if (remain != 0 && remain < 16) offset -= 16;
           
            // Optimizing memory allocation
            byte[] buffer = ArrayPool<byte>.Shared.Rent(bufferSize);

            try
            {
                // Prepare the seed vector
                uint v1 = seed + p1 + p2;
                uint v2 = seed + p2;
                uint v3 = seed + 0;
                uint v4 = seed - p1;
                
                // Process chunks
                // Skip the last chunk. It will processed a little bit later
                for (int i = 2; i <= chunks; i++)
                { 
                    // Change bufferSize for the last read
                    if (i == chunks) bufferSize = offset;
                    
                    // Read the next chunk
                    await stream.ReadAsync(buffer, 0, bufferSize).ConfigureAwait(false);;
                    
                    unsafe
                    {
                        fixed (byte* pData = &buffer[0])
                        {
                            byte* ptr = pData;
                            byte* end = pData + bufferSize;
                            
                            do
                            {
                                v1 += *((uint*)ptr) * p2;
                                v1 = (v1 << 13) | (v1 >> (32 - 13)); // rotl 13
                                v1 *= p1;
                                ptr += 4;
    
                                v2 += *((uint*)ptr) * p2;
                                v2 = (v2 << 13) | (v2 >> (32 - 13)); // rotl 13
                                v2 *= p1;
                                ptr += 4;
    
                                v3 += *((uint*)ptr) * p2;
                                v3 = (v3 << 13) | (v3 >> (32 - 13)); // rotl 13
                                v3 *= p1;
                                ptr += 4;
    
                                v4 += *((uint*)ptr) * p2;
                                v4 = (v4 << 13) | (v4 >> (32 - 13)); // rotl 13
                                v4 *= p1;
                                ptr += 4;
    
                            } while (ptr < end);
                        }
                    }   
                }
                
                // Read the last chunk
                offset = await stream.ReadAsync(buffer, 0, bufferSize).ConfigureAwait(false);
                
                // Process the last chunk
                unsafe
                {
                    fixed (byte* pData = &buffer[0])
                    {
                        byte* ptr = pData;
                        byte* end = pData + offset;
                        uint h32;
    
                        if (length >= 16)
                        {
                            byte* limit = end - 16;
    
                            do
                            {
                                v1 += *((uint*) ptr) * p2;
                                v1 = (v1 << 13) | (v1 >> (32 - 13)); // rotl 13
                                v1 *= p1;
                                ptr += 4;
    
                                v2 += *((uint*) ptr) * p2;
                                v2 = (v2 << 13) | (v2 >> (32 - 13)); // rotl 13
                                v2 *= p1;
                                ptr += 4;
    
                                v3 += *((uint*) ptr) * p2;
                                v3 = (v3 << 13) | (v3 >> (32 - 13)); // rotl 13
                                v3 *= p1;
                                ptr += 4;
    
                                v4 += *((uint*) ptr) * p2;
                                v4 = (v4 << 13) | (v4 >> (32 - 13)); // rotl 13
                                v4 *= p1;
                                ptr += 4;
    
                            } while (ptr <= limit);
    
                            h32 = ((v1 << 1) | (v1 >> (32 - 1))) +   // rotl 1
                                  ((v2 << 7) | (v2 >> (32 - 7))) +   // rotl 7
                                  ((v3 << 12) | (v3 >> (32 - 12))) + // rotl 12
                                  ((v4 << 18) | (v4 >> (32 - 18)));  // rotl 18
                        }
                        else
                        {
                            h32 = seed + p5;
                        }
    
                        h32 += (uint) length;
    
                        while (ptr <= end - 4)
                        {
                            h32 += *((uint*) ptr) * p3;
                            h32 = ((h32 << 17) | (h32 >> (32 - 17))) * p4; // (rotl 17) * p4
                            ptr += 4;
                        }
    
                        while (ptr < end)
                        {
                            h32 += *((byte*) ptr) * p5;
                            h32 = ((h32 << 11) | (h32 >> (32 - 11))) * p1; // (rotl 11) * p1
                            ptr += 1;
                        }
    
                        h32 ^= h32 >> 15;
                        h32 *= p2;
                        h32 ^= h32 >> 13;
                        h32 *= p3;
                        h32 ^= h32 >> 16;
    
                        return h32;
                    }
                }
            }
            finally
            {
                // Free memory
                ArrayPool<byte>.Shared.Return(buffer);
            }
        }
    }
}