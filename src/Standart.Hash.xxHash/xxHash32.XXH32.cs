// ReSharper disable InconsistentNaming

namespace Standart.Hash.xxHash
{
    using System.Runtime.CompilerServices;

    public static partial class xxHash32
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static unsafe uint XXH32_internal(byte* input, int len, uint seed)
        {
            byte* end = input + len;
            uint h32;

            if (len >= 16)
            {
                byte* limit = end - 16;

                uint v1 = seed + XXH_PRIME32_1 + XXH_PRIME32_2;
                uint v2 = seed + XXH_PRIME32_2;
                uint v3 = seed + 0;
                uint v4 = seed - XXH_PRIME32_1;

                do
                {
                    // XXH32_round
                    v1 += *((uint*)input) * XXH_PRIME32_2;
                    v1 = XXH_rotl32(v1, 13);
                    v1 *= XXH_PRIME32_1;
                    input += 4;

                    // XXH32_round
                    v2 += *((uint*)input) * XXH_PRIME32_2;
                    v2 = XXH_rotl32(v2, 13);
                    v2 *= XXH_PRIME32_1;
                    input += 4;

                    // XXH32_round
                    v3 += *((uint*)input) * XXH_PRIME32_2;
                    v3 = XXH_rotl32(v3, 13);
                    v3 *= XXH_PRIME32_1;
                    input += 4;

                    // XXH32_round
                    v4 += *((uint*)input) * XXH_PRIME32_2;
                    v4 = XXH_rotl32(v4, 13); 
                    v4 *= XXH_PRIME32_1;
                    input += 4;

                } while (input <= limit);

                h32 = XXH_rotl32(v1, 1) + 
                      XXH_rotl32(v2, 7) +
                      XXH_rotl32(v3, 12) +
                      XXH_rotl32(v4, 18);
            }
            else
            {
                h32 = seed + XXH_PRIME32_5;
            }

            h32 += (uint)len;

            // XXH32_finalize
            while (input <= end - 4)
            {
                h32 += *((uint*)input) * XXH_PRIME32_3;
                h32 = XXH_rotl32(h32, 17) * XXH_PRIME32_4;
                input += 4;
            }

            while (input < end)
            {
                h32 += *((byte*)input) * XXH_PRIME32_5;
                h32 = XXH_rotl32(h32, 11) * XXH_PRIME32_1;
                input += 1;
            }

            // XXH32_avalanche
            h32 ^= h32 >> 15;
            h32 *= XXH_PRIME32_2;
            h32 ^= h32 >> 13;
            h32 *= XXH_PRIME32_3;
            h32 ^= h32 >> 16;

            return h32;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static unsafe void __XXH32_stream_align(byte[] input, int len, ref uint v1, ref uint v2, ref uint v3, ref uint v4)
        {
            fixed (byte* pData = &input[0])
            {
                byte* ptr = pData;
                byte* limit = ptr + len;

                do
                {
                    // XXH32_round
                    v1 += *((uint*)ptr) * XXH_PRIME32_2;
                    v1 = XXH_rotl32(v1, 13);
                    v1 *= XXH_PRIME32_1;
                    ptr += 4;

                    // XXH32_round
                    v2 += *((uint*)ptr) * XXH_PRIME32_2;
                    v2 = XXH_rotl32(v2, 13);
                    v2 *= XXH_PRIME32_1;
                    ptr += 4;

                    // XXH32_round
                    v3 += *((uint*)ptr) * XXH_PRIME32_2;
                    v3 = XXH_rotl32(v3, 13);
                    v3 *= XXH_PRIME32_1;
                    ptr += 4;

                    // XXH32_round
                    v4 += *((uint*)ptr) * XXH_PRIME32_2;
                    v4 = XXH_rotl32(v4, 13);
                    v4 *= XXH_PRIME32_1;
                    ptr += 4;

                } while (ptr < limit);
            }
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static unsafe uint __XXH32_stream_finalize(byte[] input, int len, ref uint v1, ref uint v2, ref uint v3, ref uint v4, long length, uint seed)
        {
            fixed (byte* pData = &input[0])
            {
                byte* ptr = pData;
                byte* end = pData + len;
                uint h32;

                if (length >= 16)
                {
                    h32 = XXH_rotl32(v1, 1) +  
                          XXH_rotl32(v2, 7) +
                          XXH_rotl32(v3, 12) +
                          XXH_rotl32(v4, 18);
                }
                else
                {
                    h32 = seed + XXH_PRIME32_5;
                }

                h32 += (uint)length;

                // XXH32_finalize
                while (ptr <= end - 4)
                {
                    h32 += *((uint*)ptr) * XXH_PRIME32_3;
                    h32 = XXH_rotl32(h32, 17) * XXH_PRIME32_4;
                    ptr += 4;
                }

                while (ptr < end)
                {
                    h32 += *((byte*)ptr) * XXH_PRIME32_5;
                    h32 = XXH_rotl32(h32, 11) * XXH_PRIME32_1;
                    ptr += 1;
                }

                // XXH32_avalanche
                h32 ^= h32 >> 15;
                h32 *= XXH_PRIME32_2;
                h32 ^= h32 >> 13;
                h32 *= XXH_PRIME32_3;
                h32 ^= h32 >> 16;

                return h32;
            }
        }
    }
}