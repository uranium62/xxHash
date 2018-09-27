namespace Standart.Hash.xxHash
{
    using System;
    using System.Buffers;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Threading.Tasks;


    public static partial class xxHash64
    {      
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static unsafe void Shift(byte[] data, int l, ref ulong v1, ref ulong v2, ref ulong v3, ref ulong v4)
        {
            fixed (byte* pData = &data[0])
            {
                byte* ptr = pData;
                byte* limit = ptr + l;

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

                } while (ptr < limit);
            }
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static unsafe ulong Final(byte[] data, int l, ref ulong v1, ref ulong v2, ref ulong v3, ref ulong v4, long length, ulong seed)
        {
            fixed (byte* pData = &data[0])
            {
                byte* ptr = pData;
                byte* end = pData + l;
                ulong h64;

                if (length >= 16)
                {
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
        
        /// <summary>
        /// Compute xxHash for the stream
        /// </summary>
        /// <param name="stream">The stream of data</param>
        /// <param name="bufferSize">The buffer size</param>
        /// <param name="seed">The seed number</param>
        /// <returns>The hash</returns>
        public static ulong ComputeHash(Stream stream, int bufferSize = 8192, ulong seed = 0)
        {
            // Optimizing memory allocation
            byte[] buffer = ArrayPool<byte>.Shared.Rent(bufferSize + 32);

            int  readBytes;
            int  offset = 0;
            long length = 0;

            // Prepare the seed vector
            ulong v1 = seed + p1 + p2;
            ulong v2 = seed + p2;
            ulong v3 = seed + 0;
            ulong v4 = seed - p1;
            
            try
            {
                // Read flow of bytes
                while ((readBytes = stream.Read(buffer, offset, bufferSize)) > 0)
                {
                    length = length + readBytes;
                    offset = offset + readBytes;

                    if (offset < 32) continue;

                    int r = offset % 32; // remain
                    int l = offset - r;  // length

                    // Process the next chunk 
                    Shift(buffer, l, ref v1, ref v2, ref v3, ref v4);

                    // Put remaining bytes to buffer
                    Array.Copy(buffer, l, buffer, 0, r);
                    offset = r;
                }

                // Process the final chunk
                ulong h64 = Final(buffer, offset, ref v1, ref v2, ref v3, ref v4, length, seed);

                return h64;
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
            // Optimizing memory allocation
            byte[] buffer = ArrayPool<byte>.Shared.Rent(bufferSize + 32);

            int  readBytes;
            int  offset = 0;
            long length = 0;

            // Prepare the seed vector
            ulong v1 = seed + p1 + p2;
            ulong v2 = seed + p2;
            ulong v3 = seed + 0;
            ulong v4 = seed - p1;
            
            try
            {
                // Read flow of bytes
                while ((readBytes = await stream.ReadAsync(buffer, offset, bufferSize)) > 0)
                {
                    length = length + readBytes;
                    offset = offset + readBytes;

                    if (offset < 32) continue;

                    int r = offset % 32; // remain
                    int l = offset - r;  // length

                    // Process the next chunk 
                    Shift(buffer, l, ref v1, ref v2, ref v3, ref v4);

                    // Put remaining bytes to buffer
                    Array.Copy(buffer, l, buffer, 0, r);
                    offset = r;
                }

                // Process the final chunk
                ulong h64 = Final(buffer, offset, ref v1, ref v2, ref v3, ref v4, length, seed);

                return h64;
            }
            finally
            {
                // Free memory
                ArrayPool<byte>.Shared.Return(buffer);
            }    
        }
    }
}