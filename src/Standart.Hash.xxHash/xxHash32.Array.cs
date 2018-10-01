namespace Standart.Hash.xxHash
{
    using System.Diagnostics;

    public static partial class xxHash32
    {
        /// <summary>
        /// Compute xxHash for the data byte array
        /// </summary>
        /// <param name="data">The source of data</param>
        /// <param name="length">The length of the data for hashing</param>
        /// <param name="seed">The seed number</param>
        /// <returns>hash</returns>
        public static unsafe uint ComputeHash(byte[] data, int length, uint seed = 0)
        {
            Debug.Assert(data != null);
            Debug.Assert(length >= 0);
            Debug.Assert(length <= data.Length);
            
            fixed (byte* pData = &data[0])
            {
                return UnsafeComputeHash(pData, length, seed);
            }
        }
    }
}