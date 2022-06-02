// ReSharper disable InconsistentNaming

namespace Standart.Hash.xxHash
{
    using System.Runtime.CompilerServices;

    public static partial class xxHash64
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static unsafe ulong XXH64_internal(byte* input, int len, ulong seed)
        {
            byte* end = input + len;
            ulong h64;

            if (len >= 32)

            {
                byte* limit = end - 32;

                ulong v1 = seed + XXH_PRIME64_1 + XXH_PRIME64_2;
                ulong v2 = seed + XXH_PRIME64_2;
                ulong v3 = seed + 0;
                ulong v4 = seed - XXH_PRIME64_1;

                do
                {
                    // XXH64_round
                    v1 += *((ulong*)input) * XXH_PRIME64_2;
                    v1 = XXH_rotl64(v1, 31);
                    v1 *= XXH_PRIME64_1;
                    input += 8;

                    // XXH64_round
                    v2 += *((ulong*)input) * XXH_PRIME64_2;
                    v2 = XXH_rotl64(v2, 31);
                    v2 *= XXH_PRIME64_1;
                    input += 8;

                    // XXH64_round
                    v3 += *((ulong*)input) * XXH_PRIME64_2;
                    v3 = XXH_rotl64(v3, 31);
                    v3 *= XXH_PRIME64_1;
                    input += 8;

                    // XXH64_round
                    v4 += *((ulong*)input) * XXH_PRIME64_2;
                    v4 = XXH_rotl64(v4, 31);
                    v4 *= XXH_PRIME64_1;
                    input += 8;

                } while (input <= limit);

                h64 = XXH_rotl64(v1, 1) +
                      XXH_rotl64(v2, 7) +
                      XXH_rotl64(v3, 12) +
                      XXH_rotl64(v4, 18);  

                // XXH64_mergeRound
                v1 *= XXH_PRIME64_2;
                v1 = XXH_rotl64(v1, 31);
                v1 *= XXH_PRIME64_1;
                h64 ^= v1;
                h64 = h64 * XXH_PRIME64_1 + XXH_PRIME64_4;

                // XXH64_mergeRound
                v2 *= XXH_PRIME64_2;
                v2 = XXH_rotl64(v2, 31);
                v2 *= XXH_PRIME64_1;
                h64 ^= v2;
                h64 = h64 * XXH_PRIME64_1 + XXH_PRIME64_4;

                // XXH64_mergeRound
                v3 *= XXH_PRIME64_2;
                v3 = XXH_rotl64(v3, 31);
                v3 *= XXH_PRIME64_1;
                h64 ^= v3;
                h64 = h64 * XXH_PRIME64_1 + XXH_PRIME64_4;

                // XXH64_mergeRound
                v4 *= XXH_PRIME64_2;
                v4 = XXH_rotl64(v4, 31);
                v4 *= XXH_PRIME64_1;
                h64 ^= v4;
                h64 = h64 * XXH_PRIME64_1 + XXH_PRIME64_4;
            }
            else
            {
                h64 = seed + XXH_PRIME64_5;
            }

            h64 += (ulong)len;

            // XXH64_finalize
            while (input <= end - 8)
            {
                ulong t1 = *((ulong*)input) * XXH_PRIME64_2;
                t1 = XXH_rotl64(t1, 31); 
                t1 *= XXH_PRIME64_1;
                h64 ^= t1;
                h64 = XXH_rotl64(h64, 27) * XXH_PRIME64_1 + XXH_PRIME64_4;
                input += 8;
            }

            if (input <= end - 4)
            {
                h64 ^= *((uint*)input) * XXH_PRIME64_1;
                h64 = XXH_rotl64(h64, 23) * XXH_PRIME64_2 + XXH_PRIME64_3;
                input += 4;
            }

            while (input < end)
            {
                h64 ^= *((byte*)input) * XXH_PRIME64_5;
                h64 = XXH_rotl64(h64, 11) * XXH_PRIME64_1;
                input += 1;
            }

            // XXH64_avalanche
            h64 ^= h64 >> 33;
            h64 *= XXH_PRIME64_2;
            h64 ^= h64 >> 29;
            h64 *= XXH_PRIME64_3;
            h64 ^= h64 >> 32;

            return h64;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static unsafe void __XXH64_stream_align(byte[] input, int len, ref ulong v1, ref ulong v2, ref ulong v3, ref ulong v4)
        {
            fixed (byte* pData = &input[0])
            {
                byte* ptr = pData;
                byte* limit = ptr + len;

                do
                {
                    // XXH64_round
                    v1 += *((ulong*)ptr) * XXH_PRIME64_2;
                    v1 = XXH_rotl64(v1, 31);
                    v1 *= XXH_PRIME64_1;
                    ptr += 8;

                    // XXH64_round
                    v2 += *((ulong*)ptr) * XXH_PRIME64_2;
                    v2 = XXH_rotl64(v2, 31);
                    v2 *= XXH_PRIME64_1;
                    ptr += 8;

                    // XXH64_round
                    v3 += *((ulong*)ptr) * XXH_PRIME64_2;
                    v3 = XXH_rotl64(v3, 31);
                    v3 *= XXH_PRIME64_1;
                    ptr += 8;

                    // XXH64_round
                    v4 += *((ulong*)ptr) * XXH_PRIME64_2;
                    v4 = XXH_rotl64(v4, 31);
                    v4 *= XXH_PRIME64_1;
                    ptr += 8;

                } while (ptr < limit);
            }
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static unsafe ulong __XXH64_stream_finalize(byte[] input, int len, ref ulong v1, ref ulong v2, ref ulong v3, ref ulong v4, long length, ulong seed)
        {
            fixed (byte* pData = &input[0])
            {
                byte* ptr = pData;
                byte* end = pData + len;
                ulong h64;

                if (length >= 32)
                {
                    h64 = XXH_rotl64(v1, 1) +
                          XXH_rotl64(v2, 7) +
                          XXH_rotl64(v3, 12) +
                          XXH_rotl64(v4, 18);

                    // XXH64_mergeRound
                    v1 *= XXH_PRIME64_2;
                    v1 = XXH_rotl64(v1, 31);
                    v1 *= XXH_PRIME64_1;
                    h64 ^= v1;
                    h64 = h64 * XXH_PRIME64_1 + XXH_PRIME64_4;

                    // XXH64_mergeRound
                    v2 *= XXH_PRIME64_2;
                    v2 = XXH_rotl64(v2, 31);
                    v2 *= XXH_PRIME64_1;
                    h64 ^= v2;
                    h64 = h64 * XXH_PRIME64_1 + XXH_PRIME64_4;

                    // XXH64_mergeRound
                    v3 *= XXH_PRIME64_2;
                    v3 = XXH_rotl64(v3, 31);
                    v3 *= XXH_PRIME64_1;
                    h64 ^= v3;
                    h64 = h64 * XXH_PRIME64_1 + XXH_PRIME64_4;

                    // XXH64_mergeRound
                    v4 *= XXH_PRIME64_2;
                    v4 = XXH_rotl64(v4, 31);
                    v4 *= XXH_PRIME64_1;
                    h64 ^= v4;
                    h64 = h64 * XXH_PRIME64_1 + XXH_PRIME64_4;
                }
                else
                {
                    h64 = seed + XXH_PRIME64_5;
                }

                h64 += (ulong)length;

                // XXH64_finalize
                while (ptr <= end - 8)
                {
                    ulong t1 = *((ulong*)ptr) * XXH_PRIME64_2;
                    t1 = XXH_rotl64(t1, 31);
                    t1 *= XXH_PRIME64_1;
                    h64 ^= t1;
                    h64 = XXH_rotl64(h64, 27) * XXH_PRIME64_1 + XXH_PRIME64_4;
                    ptr += 8;
                }

                if (ptr <= end - 4)
                {
                    h64 ^= *((uint*)ptr) * XXH_PRIME64_1;
                    h64 = XXH_rotl64(h64, 23) * XXH_PRIME64_2 + XXH_PRIME64_3;
                    ptr += 4;
                }

                while (ptr < end)
                {
                    h64 ^= *((byte*)ptr) * XXH_PRIME64_5;
                    h64 = XXH_rotl64(h64, 11) * XXH_PRIME64_1;
                    ptr += 1;
                }

                // XXH64_avalanche
                h64 ^= h64 >> 33;
                h64 *= XXH_PRIME64_2;
                h64 ^= h64 >> 29;
                h64 *= XXH_PRIME64_3;
                h64 ^= h64 >> 32;

                return h64;
            }
        }
    }
}
