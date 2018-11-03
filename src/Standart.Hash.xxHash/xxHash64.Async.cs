namespace Standart.Hash.xxHash
{
    using System.Buffers;
    using System.Diagnostics;
    using System.IO;
    using System.Threading;
    using System.Threading.Tasks;

    public static partial class xxHash64
    {
        /// <summary>
        /// Compute xxHash for the async stream
        /// </summary>
        /// <param name="stream">The stream of data</param>
        /// <param name="bufferSize">The buffer size</param>
        /// <param name="seed">The seed number</param>
        /// <returns>The hash</returns>
        public static async ValueTask<ulong> ComputeHashAsync(Stream stream, int bufferSize = 8192, ulong seed = 0)
        {
            return await ComputeHashAsync(stream, bufferSize, seed, CancellationToken.None);
        }
        
        /// <summary>
        /// Compute xxHash for the async stream
        /// </summary>
        /// <param name="stream">The stream of data</param>
        /// <param name="bufferSize">The buffer size</param>
        /// <param name="seed">The seed number</param>
        /// <param name="cancellationToken">The cancelation token</param>
        /// <returns>The hash</returns>
        public static async ValueTask<ulong> ComputeHashAsync(Stream stream, int bufferSize, ulong seed, CancellationToken cancellationToken)
        {
            Debug.Assert(stream != null);
            Debug.Assert(bufferSize > 32);

            // Optimizing memory allocation
            byte[] buffer = ArrayPool<byte>.Shared.Rent(bufferSize + 32);

            int readBytes;
            int offset = 0;
            long length = 0;

            // Prepare the seed vector
            ulong v1 = seed + p1 + p2;
            ulong v2 = seed + p2;
            ulong v3 = seed + 0;
            ulong v4 = seed - p1;

            try
            {
                // Read flow of bytes
                while ((readBytes = await stream.ReadAsync(buffer, offset, bufferSize, cancellationToken).ConfigureAwait(false)) > 0)
                {   
                    // Exit if the operation is canceled
                    if (cancellationToken.IsCancellationRequested)
                    {
                        return await Task.FromCanceled<ulong>(cancellationToken);
                    }
                    
                    length = length + readBytes;
                    offset = offset + readBytes;

                    if (offset < 32) continue;

                    int r = offset % 32; // remain
                    int l = offset - r;  // length

                    // Process the next chunk 
                    UnsafeAlign(buffer, l, ref v1, ref v2, ref v3, ref v4);

                    // Put remaining bytes to buffer
                    UnsafeBuffer.BlockCopy(buffer, l, buffer, 0, r);
                    offset = r;
                }

                // Process the final chunk
                ulong h64 = UnsafeFinal(buffer, offset, ref v1, ref v2, ref v3, ref v4, length, seed);

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
