namespace Standart.Hash.xxHash
{
    using System;
    using System.Diagnostics;
    using System.Runtime.InteropServices;

    public static partial class xxHash64
    {
        /// <summary>
        /// Compute xxHash for the data byte span
        /// </summary>
        /// <param name="data">The source of data</param>
        /// <param name="length">The length of the data for hashing</param>
        /// <param name="seed">The seed number</param>
        /// <returns>hash</returns>
        public static unsafe ulong ComputeHash(Span<byte> data, int length, ulong seed = 0)
        {
            Debug.Assert(data != null);
            Debug.Assert(length >= 0);
            Debug.Assert(length <= data.Length);

            fixed (byte* pData = &MemoryMarshal.GetReference(data))
            {
                return UnsafeComputeHash(pData, length, seed);
            }
        }
        
        /// <summary>
        /// Compute xxHash for the data byte span
        /// </summary>
        /// <param name="data">The source of data</param>
        /// <param name="length">The length of the data for hashing</param>
        /// <param name="seed">The seed number</param>
        /// <returns>hash</returns>
        public static unsafe ulong ComputeHash(ReadOnlySpan<byte> data, int length, ulong seed = 0)
        {
            Debug.Assert(data != null);
            Debug.Assert(length >= 0);
            Debug.Assert(length <= data.Length);

            fixed (byte* pData = &MemoryMarshal.GetReference(data))
            {
                return UnsafeComputeHash(pData, length, seed);
            }
        }
    }
}
