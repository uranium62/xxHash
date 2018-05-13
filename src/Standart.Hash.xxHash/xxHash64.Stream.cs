namespace Standart.Hash.xxHash
{
    using System.Buffers;
    using System.IO;
    using System.Threading.Tasks;

    public static partial class xxHash64
    {
        private const int min64 = 1024;
        private const int div32 = 0x7FFFFFE0;

        /// <summary>
        /// Compute xxHash for the stream
        /// </summary>
        /// <param name="stream">The stream of data</param>
        /// <param name="bufferSize">The buffer size</param>
        /// <param name="seed">The seed number</param>
        /// <returns>The hash</returns>
        public static ulong ComputeHash(Stream stream, int bufferSize = 8192, ulong seed = 0)
        {
            // Go to the beginning of the stream
            stream.Seek(0, SeekOrigin.Begin);
            
            // Get length of the stream
            long length = stream.Length;
            
            // The buffer can't be less than 1024 bytes
            if (bufferSize < min64) bufferSize = min64;
            else bufferSize &= div32;
            
            // Calculate the number of chunks and the remain
            int chunks = (int) length / bufferSize;
            int remain = (int) length % bufferSize;
            int offset = bufferSize;
            
            // Calculate the offset
            if (remain != 0) chunks++;
            if (remain != 0 && remain < 32) offset -= 32;
            
            // Optimizing memory allocation
            byte[] buffer = ArrayPool<byte>.Shared.Rent(bufferSize);

            try
            {
                // Prepare the seed vector
                ulong v1 = seed + p1 + p2;
                ulong v2 = seed + p2;
                ulong v3 = seed + 0;
                ulong v4 = seed - p1;
                
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
                                v1 += *((ulong*) ptr) * p2;
                                v1 = (v1 << 31) | (v1 >> (64 - 31)); // rotl 31
                                v1 *= p1;
                                ptr += 8;
    
                                v2 += *((ulong*) ptr) * p2;
                                v2 = (v2 << 31) | (v2 >> (64 - 31)); // rotl 31
                                v2 *= p1;
                                ptr += 8;
    
                                v3 += *((ulong*) ptr) * p2;
                                v3 = (v3 << 31) | (v3 >> (64 - 31)); // rotl 31
                                v3 *= p1;
                                ptr += 8;
    
                                v4 += *((ulong*) ptr) * p2;
                                v4 = (v4 << 31) | (v4 >> (64 - 31)); // rotl 31
                                v4 *= p1;
                                ptr += 8;
    
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
                        ulong h64;
        
                        if (length >= 32)
                        {
                            byte* limit = end - 32;
        
                            do
                            {
                                v1 += *((ulong*)ptr) * p2;
                                v1 = (v1 << 31) | (v1 >> (64 - 31)); // rotl 31
                                v1 *= p1;
                                ptr += 8;
        
                                v2 += *((ulong*)ptr) * p2;
                                v2 = (v2 << 31) | (v2 >> (64 - 31)); // rotl 31
                                v2 *= p1;
                                ptr += 8;
        
                                v3 += *((ulong*)ptr) * p2;
                                v3 = (v3 << 31) | (v3 >> (64 - 31)); // rotl 31
                                v3 *= p1;
                                ptr += 8;
        
                                v4 += *((ulong*)ptr) * p2;
                                v4 = (v4 << 31) | (v4 >> (64 - 31)); // rotl 31
                                v4 *= p1;
                                ptr += 8;
        
                            } while (ptr <= limit);
        
                            h64 = ((v1 << 1) | (v1 >> (64 - 1))) +   // rotl 1
                                  ((v2 << 7) | (v2 >> (64 - 7))) +   // rotl 7
                                  ((v3 << 12) | (v3 >> (64 - 12))) + // rotl 12
                                  ((v4 << 18) | (v4 >> (64 - 18)));  // rotl 18
        
                            // merge round
                            v1 *= p2;
                            v1 = (v1 << 31) | (v1 >> (64 - 31)); // rotl 31
                            v1 *= p1;
                            h64 ^= v1;
                            h64 = h64 * p1 + p4;
        
                            // merge round
                            v2 *= p2;
                            v2 = (v2 << 31) | (v2 >> (64 - 31)); // rotl 31
                            v2 *= p1;
                            h64 ^= v2;
                            h64 = h64 * p1 + p4;
        
                            // merge round
                            v3 *= p2;
                            v3 = (v3 << 31) | (v3 >> (64 - 31)); // rotl 31
                            v3 *= p1;
                            h64 ^= v3;
                            h64 = h64 * p1 + p4;
        
                            // merge round
                            v4 *= p2;
                            v4 = (v4 << 31) | (v4 >> (64 - 31)); // rotl 31
                            v4 *= p1;
                            h64 ^= v4;
                            h64 = h64 * p1 + p4;
                        }
                        else
                        {
                            h64 = seed + p5;
                        }
        
                        h64 += (ulong) length;
        
                        // finalize
                        while (ptr <= end - 8)
                        {
                            ulong t1 = *((ulong*)ptr) * p2;
                            t1 = (t1 << 31) | (t1 >> (64 - 31)); // rotl 31
                            t1 *= p1;
                            h64 ^= t1;
                            h64 = ((h64 << 27) | (h64 >> (64 - 27))) * p1 + p4; // (rotl 27) * p1 + p4
                            ptr += 8;
                        }
                        
                        if (ptr <= end - 4)
                        {
                            h64 ^= *((uint*)ptr) * p1;
                            h64 = ((h64 << 23) | (h64 >> (64 - 23))) * p2 + p3; // (rotl 27) * p2 + p3
                            ptr += 4;
                        }
        
                        while (ptr < end)
                        {
                            h64 ^= *((byte*)ptr) * p5;
                            h64 = ((h64 << 11) | (h64 >> (64 - 11))) * p1; // (rotl 11) * p1
                            ptr += 1;
                        }
        
                        // avalanche
                        h64 ^= h64 >> 33;
                        h64 *= p2;
                        h64 ^= h64 >> 29;
                        h64 *= p3;
                        h64 ^= h64 >> 32;
        
                        return h64;
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
        public static async Task<ulong> ComputeHashAsync(Stream stream, int bufferSize = 8192, ulong seed = 0)
        {
            // Go to the beginning of the stream
            stream.Seek(0, SeekOrigin.Begin);

            // Get length of the stream
            long length = stream.Length;

            // The buffer can't be less than 1024 bytes
            if (bufferSize < min64) bufferSize = min64;
            else bufferSize &= div32;

            // Calculate the number of chunks and the remain
            int chunks = (int) length / bufferSize;
            int remain = (int) length % bufferSize;
            int offset = bufferSize;

            // Calculate the offset
            if (remain != 0) chunks++;
            if (remain != 0 && remain < 32) offset -= 32;

            // Optimizing memory allocation
            byte[] buffer = ArrayPool<byte>.Shared.Rent(bufferSize);

            try
            {
                // Prepare the seed vector
                ulong v1 = seed + p1 + p2;
                ulong v2 = seed + p2;
                ulong v3 = seed + 0;
                ulong v4 = seed - p1;
    
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
                                v1 += *((ulong*) ptr) * p2;
                                v1 = (v1 << 31) | (v1 >> (64 - 31)); // rotl 31
                                v1 *= p1;
                                ptr += 8;
    
                                v2 += *((ulong*) ptr) * p2;
                                v2 = (v2 << 31) | (v2 >> (64 - 31)); // rotl 31
                                v2 *= p1;
                                ptr += 8;
    
                                v3 += *((ulong*) ptr) * p2;
                                v3 = (v3 << 31) | (v3 >> (64 - 31)); // rotl 31
                                v3 *= p1;
                                ptr += 8;
    
                                v4 += *((ulong*) ptr) * p2;
                                v4 = (v4 << 31) | (v4 >> (64 - 31)); // rotl 31
                                v4 *= p1;
                                ptr += 8;
    
                            } while (ptr < end);
                        }
                    }
                }
    
                // Read the last chunk
                offset = await stream.ReadAsync(buffer, 0, bufferSize).ConfigureAwait(false);;
    
                // Process the last chunk
                unsafe
                {
                    fixed (byte* pData = &buffer[0])
                    {
                        byte* ptr = pData;
                        byte* end = pData + offset;
                        ulong h64;
    
                        if (length >= 32)
                        {
                            byte* limit = end - 32;
    
                            do
                            {
                                v1 += *((ulong*) ptr) * p2;
                                v1 = (v1 << 31) | (v1 >> (64 - 31)); // rotl 31
                                v1 *= p1;
                                ptr += 8;
    
                                v2 += *((ulong*) ptr) * p2;
                                v2 = (v2 << 31) | (v2 >> (64 - 31)); // rotl 31
                                v2 *= p1;
                                ptr += 8;
    
                                v3 += *((ulong*) ptr) * p2;
                                v3 = (v3 << 31) | (v3 >> (64 - 31)); // rotl 31
                                v3 *= p1;
                                ptr += 8;
    
                                v4 += *((ulong*) ptr) * p2;
                                v4 = (v4 << 31) | (v4 >> (64 - 31)); // rotl 31
                                v4 *= p1;
                                ptr += 8;
    
                            } while (ptr <= limit);
    
                            h64 = ((v1 << 1) | (v1 >> (64 - 1))) + // rotl 1
                                  ((v2 << 7) | (v2 >> (64 - 7))) + // rotl 7
                                  ((v3 << 12) | (v3 >> (64 - 12))) + // rotl 12
                                  ((v4 << 18) | (v4 >> (64 - 18))); // rotl 18
    
                            // merge round
                            v1 *= p2;
                            v1 = (v1 << 31) | (v1 >> (64 - 31)); // rotl 31
                            v1 *= p1;
                            h64 ^= v1;
                            h64 = h64 * p1 + p4;
    
                            // merge round
                            v2 *= p2;
                            v2 = (v2 << 31) | (v2 >> (64 - 31)); // rotl 31
                            v2 *= p1;
                            h64 ^= v2;
                            h64 = h64 * p1 + p4;
    
                            // merge round
                            v3 *= p2;
                            v3 = (v3 << 31) | (v3 >> (64 - 31)); // rotl 31
                            v3 *= p1;
                            h64 ^= v3;
                            h64 = h64 * p1 + p4;
    
                            // merge round
                            v4 *= p2;
                            v4 = (v4 << 31) | (v4 >> (64 - 31)); // rotl 31
                            v4 *= p1;
                            h64 ^= v4;
                            h64 = h64 * p1 + p4;
                        }
                        else
                        {
                            h64 = seed + p5;
                        }
    
                        h64 += (ulong) length;
    
                        // finalize
                        while (ptr <= end - 8)
                        {
                            ulong t1 = *((ulong*) ptr) * p2;
                            t1 = (t1 << 31) | (t1 >> (64 - 31)); // rotl 31
                            t1 *= p1;
                            h64 ^= t1;
                            h64 = ((h64 << 27) | (h64 >> (64 - 27))) * p1 + p4; // (rotl 27) * p1 + p4
                            ptr += 8;
                        }
    
                        if (ptr <= end - 4)
                        {
                            h64 ^= *((uint*) ptr) * p1;
                            h64 = ((h64 << 23) | (h64 >> (64 - 23))) * p2 + p3; // (rotl 27) * p2 + p3
                            ptr += 4;
                        }
    
                        while (ptr < end)
                        {
                            h64 ^= *((byte*) ptr) * p5;
                            h64 = ((h64 << 11) | (h64 >> (64 - 11))) * p1; // (rotl 11) * p1
                            ptr += 1;
                        }
    
                        // avalanche
                        h64 ^= h64 >> 33;
                        h64 *= p2;
                        h64 ^= h64 >> 29;
                        h64 *= p3;
                        h64 ^= h64 >> 32;
    
                        return h64;
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