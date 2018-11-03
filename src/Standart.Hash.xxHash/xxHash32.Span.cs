using System;
using System.Diagnostics;

namespace Standart.Hash.xxHash
{
    using System.Runtime.InteropServices;

    public static partial class xxHash32
    {
        /// <summary>
        /// Compute xxHash for the data byte span 
        /// </summary>
        /// <param name="data">The source of data</param>
        /// <param name="length">The length of the data for hashing</param>
        /// <param name="seed">The seed number</param>
        /// <returns>hash</returns>
        public static unsafe uint ComputeHash(Span<byte> data, int length, uint seed = 0)
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
        public static unsafe uint ComputeHash(ReadOnlySpan<byte> data, int length, uint seed = 0)
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
