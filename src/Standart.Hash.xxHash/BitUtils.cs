using System.Runtime.CompilerServices;

namespace Standart.Hash.xxHash
{
    internal static class BitUtils
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint RotateLeft(uint value, int offset)
        {
#if FCL_BITOPS
            return System.Numerics.BitOperations.RotateLeft(value, offset);
#else
            return (value << offset) | (value >> (32 - offset));
#endif
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ulong RotateLeft(ulong value, int offset) // Taken help from: https://stackoverflow.com/a/48580489/5592276
        {
#if FCL_BITOPS
            return System.Numerics.BitOperations.RotateLeft(value, offset);
#else
            return (value << offset) | (value >> (64 - offset));
#endif
        }
    }
}
