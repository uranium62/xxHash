// ReSharper disable InconsistentNaming

using System.Runtime.CompilerServices;

namespace Standart.Hash.xxHash
{
    public static partial class xxHash3
    {
        private const ulong XXH_PRIME64_1 = 11400714785074694791UL;
        private const ulong XXH_PRIME64_2 = 14029467366897019727UL;
        private const ulong XXH_PRIME64_3 = 1609587929392839161UL;
        private const ulong XXH_PRIME64_4 = 9650029242287828579UL;
        private const ulong XXH_PRIME64_5 = 2870177450012600261UL;

        private const uint XXH_PRIME32_1 = 2654435761U;
        private const uint XXH_PRIME32_2 = 2246822519U;
        private const uint XXH_PRIME32_3 = 3266489917U;
        private const uint XXH_PRIME32_4 = 668265263U;
        private const uint XXH_PRIME32_5 = 374761393U;

        private const int XXH_STRIPE_LEN = 64;
        private const int XXH_ACC_NB = XXH_STRIPE_LEN / 8;
        private const int XXH_SECRET_CONSUME_RATE = 8;
        private const int XXH_SECRET_DEFAULT_SIZE = 192;
        private const int XXH_SECRET_MERGEACCS_START = 11;
        private const int XXH_SECRET_LASTACC_START = 7;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static unsafe ulong XXH_readLE64(byte* ptr)
        {
            return *(ulong*) ptr;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static unsafe uint XXH_readLE32(byte* ptr)
        {
            return *(uint*) ptr;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static ulong XXH_swap64(ulong x)
        {
            return ((x << 56) & 0xff00000000000000UL) |
                   ((x << 40) & 0x00ff000000000000UL) |
                   ((x << 24) & 0x0000ff0000000000UL) |
                   ((x << 8) & 0x000000ff00000000UL) |
                   ((x >> 8) & 0x00000000ff000000UL) |
                   ((x >> 24) & 0x0000000000ff0000UL) |
                   ((x >> 40) & 0x000000000000ff00UL) |
                   ((x >> 56) & 0x00000000000000ffUL);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static ulong XXH_mult32to64(ulong x, ulong y)
        {
            return (ulong) (uint) (x) * (ulong) (uint) (y);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static ulong XXH_xorshift64(ulong v64, int shift)
        {
            return v64 ^ (v64 >> shift);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static uint XXH_swap32(uint x)
        {
            return ((x << 24) & 0xff000000) |
                   ((x << 8) & 0x00ff0000) |
                   ((x >> 8) & 0x0000ff00) |
                   ((x >> 24) & 0x000000ff);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static ulong XXH_rotl64(ulong x, int r)
        {
            return (x << r) | (x >> (64 - r));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static unsafe void XXH_writeLE64(byte* dst, ulong v64)
        {
            *(ulong*) dst = v64;
        }
    }
}