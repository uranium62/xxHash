namespace Standart.Hash.xxHash
{
    using System.Buffers;
    using System.Diagnostics;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Threading.Tasks;

    public static partial class xxHash32
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static unsafe void Shift(byte[] data, int l, ref uint v1, ref uint v2, ref uint v3, ref uint v4)
        {
            fixed (byte* pData = &data[0])
            {
                byte* ptr = pData;
                byte* limit = ptr + l;

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

                } while (ptr < limit);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static unsafe uint Final(byte[] data, int l, ref uint v1, ref uint v2, ref uint v3, ref uint v4, long length, uint seed)
        {
            fixed (byte* pData = &data[0])
            {
                byte* ptr = pData;
                byte* end = pData + l;
                uint h32;

                if (length >= 16)
                {
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

                // finalize
                while (ptr <= end - 4)
                {
                    h32 += *((uint*)ptr) * p3;
                    h32 = ((h32 << 17) | (h32 >> (32 - 17))) * p4; // (rotl 17) * p4
                    ptr += 4;
                }

                while (ptr < end)
                {
                    h32 += *((byte*)ptr) * p5;
                    h32 = ((h32 << 11) | (h32 >> (32 - 11))) * p1; // (rotl 11) * p1
                    ptr += 1;
                }

                // avalanche
                h32 ^= h32 >> 15;
                h32 *= p2;
                h32 ^= h32 >> 13;
                h32 *= p3;
                h32 ^= h32 >> 16;

                return h32;
            }
        }
        

        /// <summary>
        /// Compute xxHash for the stream
        /// </summary>
        /// <param name="stream">The stream of data</param>
        /// <param name="bufferSize">The buffer size</param>
        /// <param name="seed">The seed number</param>
        /// <returns>The hash</returns>
        public static uint ComputeHash(Stream stream, int bufferSize = 4096, uint seed = 0)
        {
            Debug.Assert(stream != null);
            Debug.Assert(bufferSize > 16);
            
            // Optimizing memory allocation
            byte[] buffer = ArrayPool<byte>.Shared.Rent(bufferSize + 16);

            int readBytes;
            int offset = 0;
            long length = 0;

            // Prepare the seed vector
            uint v1 = seed + p1 + p2;
            uint v2 = seed + p2;
            uint v3 = seed + 0;
            uint v4 = seed - p1;

            try
            {
                // Read flow of bytes
                while ((readBytes = stream.Read(buffer, offset, bufferSize)) > 0)
                {
                    length = length + readBytes;
                    offset = offset + readBytes;

                    if (offset < 16) continue;

                    int r = offset % 16; // remain
                    int l = offset - r;  // length

                    // Process the next chunk 
                    Shift(buffer, l, ref v1, ref v2, ref v3, ref v4);

                    // Put remaining bytes to buffer
                    UnsafeBuffer.BlockCopy(buffer, l, buffer, 0, r);
                    offset = r;
                }

                // Process the final chunk
                uint h32 = Final(buffer, offset, ref v1, ref v2, ref v3, ref v4, length, seed);

                return h32;
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
        public static async ValueTask<uint> ComputeHashAsync(Stream stream, int bufferSize = 4096, uint seed = 0)
        {
            Debug.Assert(stream != null);
            Debug.Assert(bufferSize > 16);
            
            // Optimizing memory allocation
            byte[] buffer = ArrayPool<byte>.Shared.Rent(bufferSize + 16);

            int readBytes;
            int offset = 0;
            long length = 0;

            // Prepare the seed vector
            uint v1 = seed + p1 + p2;
            uint v2 = seed + p2;
            uint v3 = seed + 0;
            uint v4 = seed - p1;

            try
            {
                // Read flow of bytes
                while ((readBytes = await stream.ReadAsync(buffer, offset, bufferSize)) > 0)
                {
                    length = length + readBytes;
                    offset = offset + readBytes;

                    if (offset < 16) continue;

                    int r = offset % 16; // remain
                    int l = offset - r;  // length

                    // Process the next chunk 
                    Shift(buffer, l, ref v1, ref v2, ref v3, ref v4);

                    // Put remaining bytes to buffer
                    UnsafeBuffer.BlockCopy(buffer,l, buffer, 0, r);
                    offset = r;
                }

                // Process the final chunk
                uint h32 = Final(buffer, offset, ref v1, ref v2, ref v3, ref v4, length, seed);

                return h32;
            }
            finally
            {
                // Free memory
                ArrayPool<byte>.Shared.Return(buffer);
            }
        }
    }
}