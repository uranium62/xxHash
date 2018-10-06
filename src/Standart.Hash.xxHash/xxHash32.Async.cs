namespace Standart.Hash.xxHash
{
    using System.Buffers;
    using System.Diagnostics;
    using System.IO;
    using System.Threading.Tasks;

    public static partial class xxHash32
    {
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
                while ((readBytes = await stream.ReadAsync(buffer, offset, bufferSize).ConfigureAwait(false)) > 0)
                {
                    length = length + readBytes;
                    offset = offset + readBytes;

                    if (offset < 16) continue;

                    int r = offset % 16; // remain
                    int l = offset - r;  // length

                    // Process the next chunk 
                    UnsafeAlign(buffer, l, ref v1, ref v2, ref v3, ref v4);

                    // Put remaining bytes to buffer
                    UnsafeBuffer.BlockCopy(buffer, l, buffer, 0, r);
                    offset = r;
                }

                // Process the final chunk
                uint h32 = UnsafeFinal(buffer, offset, ref v1, ref v2, ref v3, ref v4, length, seed);

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
