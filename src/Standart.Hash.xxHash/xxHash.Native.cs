namespace Standart.Hash.xxHash
{
    using System.Runtime.InteropServices;
    using System.Security;

    public static class xxHashNative
    {
        [DllImport("Standart.Hash.xxHash.Native.dll")]
        [SuppressUnmanagedCodeSecurity]
        static extern unsafe uint XXH32(byte* input, ulong len, uint seed);

        [DllImport("Standart.Hash.xxHash.Native.dll")]
        [SuppressUnmanagedCodeSecurity]
        static extern unsafe ulong XXH64(byte* input, ulong len, ulong seed);

        public static unsafe uint ComputeXXH32(byte[] data, ulong length, uint seed = 0)
        {
            fixed (byte* pData = &data[0])
            {
                return XXH32(pData, length, seed);
            }
        }

        public static unsafe ulong ComputeXXH64(byte[] data, ulong length, ulong seed = 0)
        {
            fixed (byte* pData = &data[0])
            {
                return XXH64(pData, length, seed);
            }
        }

    }
}
