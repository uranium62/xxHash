namespace Standart.Hash.xxHash
{
    using System.Runtime.InteropServices;
    using System.Security;

    public static class xxHashNative
    {
        [DllImport("xxHash.dll")]
        [SuppressUnmanagedCodeSecurity]
        static extern unsafe uint XXH32(byte* input, ulong len, uint seed);

        [DllImport("xxHash.dll")]
        [SuppressUnmanagedCodeSecurity]
        static extern unsafe ulong XXH64(byte* input, ulong len, ulong seed);

        [DllImport("xxHash.dll")]
        [SuppressUnmanagedCodeSecurity]
        static extern unsafe ulong XXH3_64bits_withSeed(byte* input, ulong len, ulong seed);

        [DllImport("xxHash.dll")]
        [SuppressUnmanagedCodeSecurity]
        static extern unsafe XXH128_hash_t XXH3_128bits_withSeed(byte* input, ulong len, ulong seed);

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

        public static unsafe ulong ComputeXXH3(byte[] data, ulong length, ulong seed = 0)
        {
            fixed (byte* pData = &data[0])
            {
                return XXH3_64bits_withSeed(pData, length, seed);
            }
        }

        public static unsafe XXH128_hash_t ComputeXXH128(byte[] data, ulong length, ulong seed = 0)
        {
            fixed (byte* pData = &data[0])
            {
                return XXH3_128bits_withSeed(pData, length, seed);
            }
        }
    }

    [StructLayout(LayoutKind.Explicit, Size = 64)]
    public struct XXH128_hash_t
    {
        [FieldOffset(0)]
        public ulong low64;
        [FieldOffset(8)]
        public ulong high64;
    }
}
