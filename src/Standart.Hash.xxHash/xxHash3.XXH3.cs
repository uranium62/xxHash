// ReSharper disable InconsistentNaming

using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;

namespace Standart.Hash.xxHash
{
    public static partial class xxHash3
    {
        private static byte[] XXH3_SECRET =
        {
            0xb8, 0xfe, 0x6c, 0x39, 0x23, 0xa4, 0x4b, 0xbe, 0x7c, 0x01, 0x81, 0x2c, 0xf7, 0x21, 0xad, 0x1c,
            0xde, 0xd4, 0x6d, 0xe9, 0x83, 0x90, 0x97, 0xdb, 0x72, 0x40, 0xa4, 0xa4, 0xb7, 0xb3, 0x67, 0x1f,
            0xcb, 0x79, 0xe6, 0x4e, 0xcc, 0xc0, 0xe5, 0x78, 0x82, 0x5a, 0xd0, 0x7d, 0xcc, 0xff, 0x72, 0x21,
            0xb8, 0x08, 0x46, 0x74, 0xf7, 0x43, 0x24, 0x8e, 0xe0, 0x35, 0x90, 0xe6, 0x81, 0x3a, 0x26, 0x4c,
            0x3c, 0x28, 0x52, 0xbb, 0x91, 0xc3, 0x00, 0xcb, 0x88, 0xd0, 0x65, 0x8b, 0x1b, 0x53, 0x2e, 0xa3,
            0x71, 0x64, 0x48, 0x97, 0xa2, 0x0d, 0xf9, 0x4e, 0x38, 0x19, 0xef, 0x46, 0xa9, 0xde, 0xac, 0xd8,
            0xa8, 0xfa, 0x76, 0x3f, 0xe3, 0x9c, 0x34, 0x3f, 0xf9, 0xdc, 0xbb, 0xc7, 0xc7, 0x0b, 0x4f, 0x1d,
            0x8a, 0x51, 0xe0, 0x4b, 0xcd, 0xb4, 0x59, 0x31, 0xc8, 0x9f, 0x7e, 0xc9, 0xd9, 0x78, 0x73, 0x64,
            0xea, 0xc5, 0xac, 0x83, 0x34, 0xd3, 0xeb, 0xc3, 0xc5, 0x81, 0xa0, 0xff, 0xfa, 0x13, 0x63, 0xeb,
            0x17, 0x0d, 0xdd, 0x51, 0xb7, 0xf0, 0xda, 0x49, 0xd3, 0x16, 0x55, 0x26, 0x29, 0xd4, 0x68, 0x9e,
            0x2b, 0x16, 0xbe, 0x58, 0x7d, 0x47, 0xa1, 0xfc, 0x8f, 0xf8, 0xb8, 0xd1, 0x7a, 0xd0, 0x31, 0xce,
            0x45, 0xcb, 0x3a, 0x8f, 0x95, 0x16, 0x04, 0x28, 0xaf, 0xd7, 0xfb, 0xca, 0xbb, 0x4b, 0x40, 0x7e,
        };

        private static ulong[] XXH3_INIT_ACC =
        {
            XXH_PRIME32_3, XXH_PRIME64_1, XXH_PRIME64_2, XXH_PRIME64_3,
            XXH_PRIME64_4, XXH_PRIME32_2, XXH_PRIME64_5, XXH_PRIME32_1
        };

        private const int XXH3_MIDSIZE_MAX = 240;
        private const int XXH3_MIDSIZE_STARTOFFSET = 3;
        private const int XXH3_MIDSIZE_LASTOFFSET = 17;
        private const int XXH3_SECRET_SIZE_MIN = 136;
        private const int XXH3_SECRET_DEFAULT_SIZE = 192;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static unsafe ulong XXH3_64bits_internal(byte* input, int len, ulong seed64, byte* secret,
            int secretLen)
        {
            if (len <= 16)
                return XXH3_len_0to16_64b(input, len, secret, seed64);
            if (len <= 128)
                return XXH3_len_17to128_64b(input, len, secret, secretLen, seed64);
            if (len <= XXH3_MIDSIZE_MAX)
                return XXH3_len_129to240_64b(input, len, secret, secretLen, seed64);

            return XXH3_hashLong_64b_withSeed(input, len, seed64, secret, secretLen);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static unsafe ulong XXH3_len_0to16_64b(byte* input, int len, byte* secret, ulong seed)
        {
            if (len > 8)
                return XXH3_len_9to16_64b(input, len, secret, seed);
            if (len >= 4)
                return XXH3_len_4to8_64b(input, len, secret, seed);
            if (len != 0)
                return XXH3_len_1to3_64b(input, len, secret, seed);

            return XXH64_avalanche(seed ^ (XXH_readLE64(secret + 56) ^ XXH_readLE64(secret + 64)));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static unsafe ulong XXH3_len_9to16_64b(byte* input, int len, byte* secret, ulong seed)
        {
            ulong bitflip1 = (XXH_readLE64(secret + 24) ^ XXH_readLE64(secret + 32)) + seed;
            ulong bitflip2 = (XXH_readLE64(secret + 40) ^ XXH_readLE64(secret + 48)) - seed;
            ulong input_lo = XXH_readLE64(input) ^ bitflip1;
            ulong input_hi = XXH_readLE64(input + len - 8) ^ bitflip2;
            ulong acc = ((ulong) len)
                        + XXH_swap64(input_lo) + input_hi
                        + XXH3_mul128_fold64(input_lo, input_hi);
            return XXH3_avalanche(acc);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static ulong XXH3_mul128_fold64(ulong lhs, ulong rhs)
        {
            uint128 product = XXH_mult64to128(lhs, rhs);
            return product.low64 ^ product.high64;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static ulong XXH3_avalanche(ulong h64)
        {
            h64 = XXH_xorshift64(h64, 37);
            h64 *= 0x165667919E3779F9UL;
            h64 = XXH_xorshift64(h64, 32);
            return h64;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static unsafe ulong XXH3_len_4to8_64b(byte* input, int len, byte* secret, ulong seed)
        {
            seed ^= (ulong) XXH_swap32((uint) seed) << 32;
            {
                uint input1 = XXH_readLE32(input);
                uint input2 = XXH_readLE32(input + len - 4);
                ulong bitflip = (XXH_readLE64(secret + 8) ^ XXH_readLE64(secret + 16)) - seed;
                ulong input64 = input2 + (((ulong) input1) << 32);
                ulong keyed = input64 ^ bitflip;
                return XXH3_rrmxmx(keyed, len);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static ulong XXH3_rrmxmx(ulong h64, int len)
        {
            h64 ^= XXH_rotl64(h64, 49) ^ XXH_rotl64(h64, 24);
            h64 *= 0x9FB21C651E98DF25UL;
            h64 ^= (h64 >> 35) + (ulong) len;
            h64 *= 0x9FB21C651E98DF25UL;
            return XXH_xorshift64(h64, 28);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static unsafe ulong XXH3_len_1to3_64b(byte* input, int len, byte* secret, ulong seed)
        {
            byte c1 = input[0];
            byte c2 = input[len >> 1];
            byte c3 = input[len - 1];
            uint combined = ((uint) c1 << 16) |
                            ((uint) c2 << 24) |
                            ((uint) c3 << 0) |
                            ((uint) len << 8);

            ulong bitflip = (XXH_readLE32(secret) ^
                             XXH_readLE32(secret + 4)) + seed;

            ulong keyed = (ulong)combined ^ bitflip;
            return XXH64_avalanche(keyed);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static unsafe ulong XXH3_len_17to128_64b(byte* input, int len, byte* secret, int secretSize, ulong seed)
        {
            ulong acc = ((ulong)len) * XXH_PRIME64_1;

            if (len > 32)
            {
                if (len > 64)
                {
                    if (len > 96)
                    {
                        acc += XXH3_mix16B(input + 48, secret + 96, seed);
                        acc += XXH3_mix16B(input + len - 64, secret + 112, seed);
                    }
                    acc += XXH3_mix16B(input + 32, secret + 64, seed);
                    acc += XXH3_mix16B(input + len - 48, secret + 80, seed);
                }
                acc += XXH3_mix16B(input + 16, secret + 32, seed);
                acc += XXH3_mix16B(input + len - 32, secret + 48, seed);
            }

            acc += XXH3_mix16B(input + 0, secret + 0, seed);
            acc += XXH3_mix16B(input + len - 16, secret + 16, seed);
            return XXH3_avalanche(acc);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static unsafe ulong XXH3_mix16B(byte* input, byte* secret, ulong seed64)
        {
            ulong input_lo = XXH_readLE64(input);
            ulong input_hi = XXH_readLE64(input + 8);

            return XXH3_mul128_fold64(
                input_lo ^ (XXH_readLE64(secret) + seed64),
                input_hi ^ (XXH_readLE64(secret + 8) - seed64)
            );
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static unsafe ulong XXH3_len_129to240_64b(byte* input, int len, byte* secret, int secretSize,
            ulong seed)
        {
            ulong acc = ((ulong) len) * XXH_PRIME64_1;
            int nbRounds = len / 16;

            for (int i = 0; i < 8; i++)
            {
                acc += XXH3_mix16B(input + (16 * i), secret + (16 * i), seed);
            }
            acc = XXH3_avalanche(acc);

            for (int i = 8; i < nbRounds; i++)
            {
                acc += XXH3_mix16B(input + (16 * i), secret + (16 * (i - 8)) + XXH3_MIDSIZE_STARTOFFSET, seed);
            }

            acc += XXH3_mix16B(input + len - 16, secret + XXH3_SECRET_SIZE_MIN - XXH3_MIDSIZE_LASTOFFSET, seed);
            return XXH3_avalanche(acc);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static unsafe ulong XXH3_hashLong_64b_withSeed(byte* input, int len, ulong seed, byte* secret,
            int secretSize)
        {
            if (seed == 0)
                return XXH3_hashLong_64b_internal(input, len, secret, secretSize);

            int customSecretSize = XXH3_SECRET_DEFAULT_SIZE;
            byte* customSecret = stackalloc byte[customSecretSize];

            fixed (byte* ptr = &XXH3_SECRET[0])
            {
                for (int i = 0; i < customSecretSize; i += 8)
                {
                    customSecret[i] = ptr[i];
                    customSecret[i + 1] = ptr[i + 1];
                    customSecret[i + 2] = ptr[i + 2];
                    customSecret[i + 3] = ptr[i + 3];
                    customSecret[i + 4] = ptr[i + 4];
                    customSecret[i + 5] = ptr[i + 5];
                    customSecret[i + 6] = ptr[i + 6];
                    customSecret[i + 7] = ptr[i + 7];
                }
            }

            XXH3_initCustomSecret(customSecret, seed);

            return XXH3_hashLong_64b_internal(input, len, customSecret, customSecretSize);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static unsafe void XXH3_initCustomSecret(byte* customSecret, ulong seed)
        {
            if (Avx2.IsSupported)
                XXH3_initCustomSecret_avx2(customSecret, seed);
            else if (Sse2.IsSupported)
                XXH3_initCustomSecret_sse2(customSecret, seed);
            else
                XXH3_initCustomSecret_scalar(customSecret, seed);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static unsafe void XXH3_initCustomSecret_avx2(byte* customSecret, ulong seed64)
        {
            const int m256i_size = 32;

            var seed = Vector256.Create(seed64, 0U - seed64, seed64, 0U - seed64);

            fixed (byte* secret = &XXH3_SECRET[0])
            {
                for (int i = 0; i < XXH_SECRET_DEFAULT_SIZE / m256i_size; i++)
                {
                    int uint64_offset = i * 4;

                    var src32 = Avx2.LoadVector256(((ulong*) secret) + uint64_offset);
                    var dst32 = Avx2.Add(src32, seed);
                                Avx2.Store((ulong*) customSecret + uint64_offset, dst32);
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static unsafe void XXH3_initCustomSecret_sse2(byte* customSecret, ulong seed64)
        {
            const int m128i_size = 16;

            var seed = Vector128.Create((long) seed64, (long) (0U - seed64));

            fixed (byte* secret = &XXH3_SECRET[0])
            {
                for (int i = 0; i < XXH_SECRET_DEFAULT_SIZE / m128i_size; i++)
                {
                    int uint64_offset = i * 2;

                    var src16 = Sse2.LoadVector128(((long*) secret) + uint64_offset);
                    var dst16 = Sse2.Add(src16, seed);
                                Sse2.Store((long*) customSecret + uint64_offset, dst16);
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static unsafe void XXH3_initCustomSecret_scalar(byte* customSecret, ulong seed)
        {
            fixed (byte* kSecretPtr = &XXH3_SECRET[0])
            {
                int nbRounds = XXH_SECRET_DEFAULT_SIZE / 16;

                for (int i = 0; i < nbRounds; i++)
                {
                    ulong lo = XXH_readLE64(kSecretPtr + 16 * i) + seed;
                    ulong hi = XXH_readLE64(kSecretPtr + 16 * i + 8) - seed;
                    XXH_writeLE64((byte*) customSecret + 16 * i, lo);
                    XXH_writeLE64((byte*) customSecret + 16 * i + 8, hi);
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static unsafe ulong XXH3_hashLong_64b_internal(byte* input, int len, byte* secret, int secretSize)
        {
            ulong* acc = stackalloc ulong[8];

            fixed (ulong* ptr = &XXH3_INIT_ACC[0])
            {
                acc[0] = ptr[0];
                acc[1] = ptr[1];
                acc[2] = ptr[2];
                acc[3] = ptr[3];
                acc[4] = ptr[4];
                acc[5] = ptr[5];
                acc[6] = ptr[6];
                acc[7] = ptr[7];
            }

            XXH3_hashLong_internal_loop(acc, input, len, secret, secretSize);

            return XXH3_mergeAccs(acc, secret + XXH_SECRET_MERGEACCS_START, ((ulong) len) * XXH_PRIME64_1);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static unsafe ulong XXH3_mergeAccs(ulong* acc, byte* secret, ulong start)
        {
            ulong result64 = start;

            for (int i = 0; i < 4; i++)
                result64 += XXH3_mix2Accs(acc + 2 * i, secret + 16 * i);

            return XXH3_avalanche(result64);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static unsafe ulong XXH3_mix2Accs(ulong* acc, byte* secret)
        {
            return XXH3_mul128_fold64(
                acc[0] ^ XXH_readLE64(secret),
                acc[1] ^ XXH_readLE64(secret + 8));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static unsafe void XXH3_hashLong_internal_loop(ulong* acc, byte* input, int len, byte* secret,
            int secretSize)
        {
            int nbStripesPerBlock = (secretSize - XXH_STRIPE_LEN) / XXH_SECRET_CONSUME_RATE;
            int block_len = XXH_STRIPE_LEN * nbStripesPerBlock;
            int nb_blocks = (len - 1) / block_len;

            for (int n = 0; n < nb_blocks; n++)
            {
                XXH3_accumulate(acc, input + n * block_len, secret, nbStripesPerBlock);
                XXH3_scrambleAcc(acc, secret + secretSize - XXH_STRIPE_LEN);
            }

            int nbStripes = ((len - 1) - (block_len * nb_blocks)) / XXH_STRIPE_LEN;
            XXH3_accumulate(acc, input + nb_blocks * block_len, secret, nbStripes);

            byte* p = input + len - XXH_STRIPE_LEN;
            XXH3_accumulate_512(acc, p, secret + secretSize - XXH_STRIPE_LEN - XXH_SECRET_LASTACC_START);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static unsafe void XXH3_accumulate(ulong* acc, byte* input, byte* secret, int nbStripes)
        {
            for (int n = 0; n < nbStripes; n++)
            {
                byte* inp = input + n * XXH_STRIPE_LEN;
                XXH3_accumulate_512(acc, inp, secret + n * XXH_SECRET_CONSUME_RATE);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static unsafe void XXH3_accumulate_512(ulong* acc, byte* input, byte* secret)
        {
            if (Avx2.IsSupported)
                XXH3_accumulate_512_avx2(acc, input, secret);
            else if (Sse2.IsSupported)
                XXH3_accumulate_512_sse2(acc, input, secret);
            else
                XXH3_accumulate_512_scalar(acc, input, secret);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static unsafe void XXH3_accumulate_512_avx2(ulong* acc, byte* input, byte* secret)
        {
            const int m256i_size = 32;
            const byte _MM_SHUFFLE_0_3_0_1 = 0b0011_0001;
            const byte _MM_SHUFFLE_1_0_3_2 = 0b0100_1110;

            for (int i = 0; i < XXH_STRIPE_LEN / m256i_size; i++)
            {
                int uint32_offset = i * 8;
                int uint64_offset = i * 4;

                var acc_vec = Avx2.LoadVector256(acc + uint64_offset);
                var data_vec = Avx2.LoadVector256((uint*) input + uint32_offset);
                var key_vec = Avx2.LoadVector256((uint*) secret + uint32_offset);
                var data_key = Avx2.Xor(data_vec, key_vec);
                var data_key_lo = Avx2.Shuffle(data_key, _MM_SHUFFLE_0_3_0_1);
                var product = Avx2.Multiply(data_key, data_key_lo);
                var data_swap = Avx2.Shuffle(data_vec, _MM_SHUFFLE_1_0_3_2).AsUInt64();
                var sum = Avx2.Add(acc_vec, data_swap);
                var result = Avx2.Add(product, sum);
                Avx2.Store(acc + uint64_offset, result);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static unsafe void XXH3_accumulate_512_sse2(ulong* acc, byte* input, byte* secret)
        {
            const int m128i_size = 16;
            const byte _MM_SHUFFLE_0_3_0_1 = 0b0011_0001;
            const byte _MM_SHUFFLE_1_0_3_2 = 0b0100_1110;

            for (int i = 0; i < XXH_STRIPE_LEN / m128i_size; i++)
            {
                int uint32_offset = i * 4;
                int uint64_offset = i * 2;

                var acc_vec = Sse2.LoadVector128(acc + uint64_offset);
                var data_vec = Sse2.LoadVector128((uint*) input + uint32_offset);
                var key_vec = Sse2.LoadVector128((uint*) secret + uint32_offset);
                var data_key = Sse2.Xor(data_vec, key_vec);
                var data_key_lo = Sse2.Shuffle(data_key, _MM_SHUFFLE_0_3_0_1);
                var product = Sse2.Multiply(data_key, data_key_lo);
                var data_swap = Sse2.Shuffle(data_vec, _MM_SHUFFLE_1_0_3_2).AsUInt64();
                var sum = Sse2.Add(acc_vec, data_swap);
                var result = Sse2.Add(product, sum);
                Sse2.Store(acc + uint64_offset, result);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static unsafe void XXH3_accumulate_512_scalar(ulong* acc, byte* input, byte* secret)
        {
            for (int i = 0; i < XXH_ACC_NB; i++)
                XXH3_scalarRound(acc, input, secret, i);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static unsafe void XXH3_scalarRound(ulong* acc, byte* input, byte* secret, int lane)
        {
            ulong* xacc = acc;
            byte* xinput = input;
            byte* xsecret = secret;

            ulong data_val = XXH_readLE64(xinput + lane * 8);
            ulong data_key = data_val ^ XXH_readLE64(xsecret + lane * 8);
            xacc[lane ^ 1] += data_val;
            xacc[lane] += XXH_mult32to64(data_key & 0xFFFFFFFF, data_key >> 32);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static unsafe void XXH3_scrambleAcc(ulong* acc, byte* secret)
        {
            if (Avx2.IsSupported)
                XXH3_scrambleAcc_avx2(acc, secret);
            else if (Sse2.IsSupported)
                XXH3_scrambleAcc_sse2(acc, secret);
            else
                XXH3_scrambleAcc_scalar(acc, secret);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static unsafe void XXH3_scrambleAcc_avx2(ulong* acc, byte* secret)
        {
            const int m256i_size = 32;
            const byte _MM_SHUFFLE_0_3_0_1 = 0b0011_0001;

            var prime32 = Vector256.Create(XXH_PRIME32_1);

            for (int i = 0; i < XXH_STRIPE_LEN / m256i_size; i++)
            {
                int uint64_offset = i * 4;

                var acc_vec = Avx2.LoadVector256(acc + uint64_offset);
                var shifted = Avx2.ShiftRightLogical(acc_vec, 47);
                var data_vec = Avx2.Xor(acc_vec, shifted);
                var key_vec = Avx2.LoadVector256((ulong*) secret + uint64_offset);
                var data_key = Avx2.Xor(data_vec, key_vec).AsUInt32();
                var data_key_hi = Avx2.Shuffle(data_key, _MM_SHUFFLE_0_3_0_1);
                var prod_lo = Avx2.Multiply(data_key, prime32);
                var prod_hi = Avx2.Multiply(data_key_hi, prime32);
                var result = Avx2.Add(prod_lo, Avx2.ShiftLeftLogical(prod_hi, 32));
                Avx2.Store(acc + uint64_offset, result);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static unsafe void XXH3_scrambleAcc_sse2(ulong* acc, byte* secret)
        {
            const int m128i_size = 16;
            const byte _MM_SHUFFLE_0_3_0_1 = 0b0011_0001;

            var prime32 = Vector128.Create(XXH_PRIME32_1);

            for (int i = 0; i < XXH_STRIPE_LEN / m128i_size; i++)
            {
                int uint32_offset = i * 4;
                int uint64_offset = i * 2;

                var acc_vec = Sse2.LoadVector128(acc + uint64_offset).AsUInt32();
                var shifted = Sse2.ShiftRightLogical(acc_vec, 47);
                var data_vec = Sse2.Xor(acc_vec, shifted);
                var key_vec = Sse2.LoadVector128((uint*) secret + uint32_offset);
                var data_key = Sse2.Xor(data_vec, key_vec);
                var data_key_hi = Sse2.Shuffle(data_key.AsUInt32(), _MM_SHUFFLE_0_3_0_1);
                var prod_lo = Sse2.Multiply(data_key, prime32);
                var prod_hi = Sse2.Multiply(data_key_hi, prime32);
                var result = Sse2.Add(prod_lo, Sse2.ShiftLeftLogical(prod_hi, 32));
                Sse2.Store(acc + uint64_offset, result);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static unsafe void XXH3_scrambleAcc_scalar(ulong* acc, byte* secret)
        {
            for (int i = 0; i < XXH_ACC_NB; i++)
                XXH3_scalarScrambleRound(acc, secret, i);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static unsafe void XXH3_scalarScrambleRound(ulong* acc, byte* secret, int lane)
        {
            ulong* xacc = acc;
            byte* xsecret = secret;

            ulong key64 = XXH_readLE64(xsecret + lane * 8);
            ulong acc64 = xacc[lane];
            acc64 = XXH_xorshift64(acc64, 47);
            acc64 ^= key64;
            acc64 *= XXH_PRIME32_1;
            xacc[lane] = acc64;
        }
    }
}