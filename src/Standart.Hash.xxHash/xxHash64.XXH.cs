// ReSharper disable InconsistentNaming

using System.Runtime.CompilerServices;

namespace Standart.Hash.xxHash;

public static partial class xxHash64
{
    private const ulong XXH_PRIME64_1 = 11400714785074694791UL;
    private const ulong XXH_PRIME64_2 = 14029467366897019727UL;
    private const ulong XXH_PRIME64_3 = 1609587929392839161UL;
    private const ulong XXH_PRIME64_4 = 9650029242287828579UL;
    private const ulong XXH_PRIME64_5 = 2870177450012600261UL;
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static ulong XXH_rotl64(ulong x, int r)
    {
        return (x << r) | (x >> (64 - r));
    }
}