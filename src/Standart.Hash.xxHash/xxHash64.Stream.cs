namespace Standart.Hash.xxHash
{
    using System.Buffers;
    using System.Diagnostics;
    using System.IO;

    public static partial class xxHash64
    {      
        /// <summary>
        /// Compute xxHash for the stream
        /// </summary>
        /// <param name="stream">The stream of data</param>
        /// <param name="bufferSize">The buffer size</param>
        /// <param name="seed">The seed number</param>
        /// <returns>The hash</returns>
        public static ulong ComputeHash(Stream stream, int bufferSize = 8192, ulong seed = 0)
        {
            Debug.Assert(stream != null);
            Debug.Assert(bufferSize > 32);
            
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