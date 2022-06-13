/*
* This is the auto generated code.
* All function calls are inlined in XXH3_128bits_internal
* Please don't try to analyze it.
*/

using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;

namespace Standart.Hash.xxHash
{
    public static partial class xxHash3
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static unsafe ulong __inline__XXH3_64bits_internal(byte* input, int len, ulong seed64, byte* secret, int secretLen)
        {
            if (len <= 16)
            {
                if (len > 8)
                {
                    byte* ptr = secret + 24;
                    byte* ptr1 = secret + 32;
                    ulong bitflip1 = (*(ulong*) ptr ^ *(ulong*) ptr1) + seed64;
                    byte* ptr2 = secret + 40;
                    byte* ptr3 = secret + 48;
                    ulong bitflip2 = (*(ulong*) ptr2 ^ *(ulong*) ptr3) - seed64;
                    ulong input_lo = *(ulong*) input ^ bitflip1;
                    byte* ptr4 = input + len - 8;
                    ulong input_hi = *(ulong*) ptr4 ^ bitflip2;
                    uint128 ret;
                    if (Bmi2.IsSupported)
                    {
                        ulong product_low;
                        ulong product_high = Bmi2.X64.MultiplyNoFlags(input_lo, input_hi, &product_low);
                        uint128 r128;
                        r128.low64 = product_low;
                        r128.high64 = product_high;
                        ret = r128;
                    }
                    else
                    {
                        ulong y = input_hi & 0xFFFFFFFF;
                        ulong lo_lo = (ulong) (uint) (input_lo & 0xFFFFFFFF) * (ulong) (uint) (y);
                        ulong y1 = input_hi & 0xFFFFFFFF;
                        ulong hi_lo = (ulong) (uint) (input_lo >> 32) * (ulong) (uint) (y1);
                        ulong y2 = input_hi >> 32;
                        ulong lo_hi = (ulong) (uint) (input_lo & 0xFFFFFFFF) * (ulong) (uint) (y2);
                        ulong y3 = input_hi >> 32;
                        ulong hi_hi = (ulong) (uint) (input_lo >> 32) * (ulong) (uint) (y3);

                        ulong cross = (lo_lo >> 32) + (hi_lo & 0xFFFFFFFF) + lo_hi;
                        ulong upper = (hi_lo >> 32) + (cross >> 32) + hi_hi;
                        ulong lower = (cross << 32) | (lo_lo & 0xFFFFFFFF);

                        uint128 r128;
                        r128.low64 = lower;
                        r128.high64 = upper;
                        ret = r128;
                    }

                    uint128 product = ret;
                    ulong acc = ((ulong) len)
                                + (((input_lo << 56) & 0xff00000000000000UL) |
                                   ((input_lo << 40) & 0x00ff000000000000UL) |
                                   ((input_lo << 24) & 0x0000ff0000000000UL) |
                                   ((input_lo << 8) & 0x000000ff00000000UL) |
                                   ((input_lo >> 8) & 0x00000000ff000000UL) |
                                   ((input_lo >> 24) & 0x0000000000ff0000UL) |
                                   ((input_lo >> 40) & 0x000000000000ff00UL) |
                                   ((input_lo >> 56) & 0x00000000000000ffUL)) + input_hi
                                + (product.low64 ^ product.high64);
                    ulong h64 = acc;
                    h64 = h64 ^ (h64 >> 37);
                    h64 *= 0x165667919E3779F9UL;
                    h64 = h64 ^ (h64 >> 32);
                    return h64;
                }

                if (len >= 4)
                {
                    ulong seed = seed64;
                    uint x = (uint) seed;
                    seed ^= (ulong) (((x << 24) & 0xff000000) |
                                     ((x << 8) & 0x00ff0000) |
                                     ((x >> 8) & 0x0000ff00) |
                                     ((x >> 24) & 0x000000ff)) << 32;
                    {
                        uint input1 = *(uint*) input;
                        byte* ptr2 = input + len - 4;
                        uint input2 = *(uint*) ptr2;
                        byte* ptr = secret + 8;
                        byte* ptr1 = secret + 16;
                        ulong bitflip = (*(ulong*) ptr ^ *(ulong*) ptr1) - seed;
                        ulong input64 = input2 + (((ulong) input1) << 32);
                        ulong keyed = input64 ^ bitflip;
                        ulong h64 = keyed;
                        h64 ^= ((h64 << 49) | (h64 >> (64 - 49))) ^ ((h64 << 24) | (h64 >> (64 - 24)));
                        h64 *= 0x9FB21C651E98DF25UL;
                        h64 ^= (h64 >> 35) + (ulong) len;
                        h64 *= 0x9FB21C651E98DF25UL;
                        return h64 ^ (h64 >> 28);
                    }
                }

                if (len != 0)
                {
                    byte c1 = input[0];
                    byte c2 = input[len >> 1];
                    byte c3 = input[len - 1];
                    uint combined = ((uint) c1 << 16) |
                                    ((uint) c2 << 24) |
                                    ((uint) c3 << 0) |
                                    ((uint) len << 8);

                    byte* ptr = secret + 4;
                    ulong bitflip = (*(uint*) secret ^
                                     *(uint*) ptr) + seed64;

                    ulong keyed = (ulong)combined ^ bitflip;
                    ulong hash = keyed;
                    hash ^= hash >> 33;
                    hash *= XXH_PRIME64_2;
                    hash ^= hash >> 29;
                    hash *= XXH_PRIME64_3;
                    hash ^= hash >> 32;
                    return hash;
                }

                byte* ptr5 = secret + 56;
                byte* ptr6 = secret + 64;
                ulong hash1 = seed64 ^ (*(ulong*) ptr5 ^ *(ulong*) ptr6);
                hash1 ^= hash1 >> 33;
                hash1 *= XXH_PRIME64_2;
                hash1 ^= hash1 >> 29;
                hash1 *= XXH_PRIME64_3;
                hash1 ^= hash1 >> 32;
                return hash1;
            }

            if (len <= 128)
            {
                ulong acc = ((ulong)len) * XXH_PRIME64_1;

                if (len > 32)
                {
                    if (len > 64)
                    {
                        if (len > 96)
                        {
                            byte* input1 = input + 48;
                            byte* secret1 = secret + 96;
                            ulong input_lo = *(ulong*) input1;
                            byte* ptr = input1 + 8;
                            ulong input_hi = *(ulong*) ptr;

                            byte* ptr1 = secret1 + 8;
                            ulong rhs = input_hi ^ (*(ulong*) ptr1 - seed64);
                            ulong lhs = input_lo ^ (*(ulong*) secret1 + seed64);
                            uint128 ret;
                            if (Bmi2.IsSupported)
                            {
                                ulong product_low;
                                ulong product_high = Bmi2.X64.MultiplyNoFlags(lhs, rhs, &product_low);
                                uint128 r128;
                                r128.low64 = product_low;
                                r128.high64 = product_high;
                                ret = r128;
                            }
                            else
                            {
                                ulong y = rhs & 0xFFFFFFFF;
                                ulong lo_lo = (ulong) (uint) (lhs & 0xFFFFFFFF) * (ulong) (uint) (y);
                                ulong y1 = rhs & 0xFFFFFFFF;
                                ulong hi_lo = (ulong) (uint) (lhs >> 32) * (ulong) (uint) (y1);
                                ulong y2 = rhs >> 32;
                                ulong lo_hi = (ulong) (uint) (lhs & 0xFFFFFFFF) * (ulong) (uint) (y2);
                                ulong y3 = rhs >> 32;
                                ulong hi_hi = (ulong) (uint) (lhs >> 32) * (ulong) (uint) (y3);

                                ulong cross = (lo_lo >> 32) + (hi_lo & 0xFFFFFFFF) + lo_hi;
                                ulong upper = (hi_lo >> 32) + (cross >> 32) + hi_hi;
                                ulong lower = (cross << 32) | (lo_lo & 0xFFFFFFFF);

                                uint128 r128;
                                r128.low64 = lower;
                                r128.high64 = upper;
                                ret = r128;
                            }

                            uint128 product = ret;
                            acc += product.low64 ^ product.high64;
                            byte* input2 = input + len - 64;
                            byte* secret2 = secret + 112;
                            ulong inputLo = *(ulong*) input2;
                            byte* ptr2 = input2 + 8;
                            ulong inputHi = *(ulong*) ptr2;

                            byte* ptr3 = secret2 + 8;
                            ulong rhs1 = inputHi ^ (*(ulong*) ptr3 - seed64);
                            ulong lhs1 = inputLo ^ (*(ulong*) secret2 + seed64);
                            uint128 ret1;
                            if (Bmi2.IsSupported)
                            {
                                ulong productLow;
                                ulong productHigh = Bmi2.X64.MultiplyNoFlags(lhs1, rhs1, &productLow);
                                uint128 r129;
                                r129.low64 = productLow;
                                r129.high64 = productHigh;
                                ret1 = r129;
                            }
                            else
                            {
                                ulong y4 = rhs1 & 0xFFFFFFFF;
                                ulong loLo = (ulong) (uint) (lhs1 & 0xFFFFFFFF) * (ulong) (uint) (y4);
                                ulong y5 = rhs1 & 0xFFFFFFFF;
                                ulong hiLo = (ulong) (uint) (lhs1 >> 32) * (ulong) (uint) (y5);
                                ulong y6 = rhs1 >> 32;
                                ulong loHi = (ulong) (uint) (lhs1 & 0xFFFFFFFF) * (ulong) (uint) (y6);
                                ulong y7 = rhs1 >> 32;
                                ulong hiHi = (ulong) (uint) (lhs1 >> 32) * (ulong) (uint) (y7);

                                ulong cross1 = (loLo >> 32) + (hiLo & 0xFFFFFFFF) + loHi;
                                ulong upper1 = (hiLo >> 32) + (cross1 >> 32) + hiHi;
                                ulong lower1 = (cross1 << 32) | (loLo & 0xFFFFFFFF);

                                uint128 r1210;
                                r1210.low64 = lower1;
                                r1210.high64 = upper1;
                                ret1 = r1210;
                            }

                            uint128 product1 = ret1;
                            acc += product1.low64 ^ product1.high64;
                        }

                        byte* input3 = input + 32;
                        byte* secret3 = secret + 64;
                        ulong inputLo1 = *(ulong*) input3;
                        byte* ptr4 = input3 + 8;
                        ulong inputHi1 = *(ulong*) ptr4;

                        byte* ptr5 = secret3 + 8;
                        ulong rhs2 = inputHi1 ^ (*(ulong*) ptr5 - seed64);
                        ulong lhs2 = inputLo1 ^ (*(ulong*) secret3 + seed64);
                        uint128 ret2;
                        if (Bmi2.IsSupported)
                        {
                            ulong productLow1;
                            ulong productHigh1 = Bmi2.X64.MultiplyNoFlags(lhs2, rhs2, &productLow1);
                            uint128 r1211;
                            r1211.low64 = productLow1;
                            r1211.high64 = productHigh1;
                            ret2 = r1211;
                        }
                        else
                        {
                            ulong y8 = rhs2 & 0xFFFFFFFF;
                            ulong loLo1 = (ulong) (uint) (lhs2 & 0xFFFFFFFF) * (ulong) (uint) (y8);
                            ulong y9 = rhs2 & 0xFFFFFFFF;
                            ulong hiLo1 = (ulong) (uint) (lhs2 >> 32) * (ulong) (uint) (y9);
                            ulong y10 = rhs2 >> 32;
                            ulong loHi1 = (ulong) (uint) (lhs2 & 0xFFFFFFFF) * (ulong) (uint) (y10);
                            ulong y11 = rhs2 >> 32;
                            ulong hiHi1 = (ulong) (uint) (lhs2 >> 32) * (ulong) (uint) (y11);

                            ulong cross2 = (loLo1 >> 32) + (hiLo1 & 0xFFFFFFFF) + loHi1;
                            ulong upper2 = (hiLo1 >> 32) + (cross2 >> 32) + hiHi1;
                            ulong lower2 = (cross2 << 32) | (loLo1 & 0xFFFFFFFF);

                            uint128 r1212;
                            r1212.low64 = lower2;
                            r1212.high64 = upper2;
                            ret2 = r1212;
                        }

                        uint128 product2 = ret2;
                        acc += product2.low64 ^ product2.high64;
                        byte* input4 = input + len - 48;
                        byte* secret4 = secret + 80;
                        ulong inputLo2 = *(ulong*) input4;
                        byte* ptr6 = input4 + 8;
                        ulong inputHi2 = *(ulong*) ptr6;

                        byte* ptr7 = secret4 + 8;
                        ulong rhs3 = inputHi2 ^ (*(ulong*) ptr7 - seed64);
                        ulong lhs3 = inputLo2 ^ (*(ulong*) secret4 + seed64);
                        uint128 ret3;
                        if (Bmi2.IsSupported)
                        {
                            ulong productLow2;
                            ulong productHigh2 = Bmi2.X64.MultiplyNoFlags(lhs3, rhs3, &productLow2);
                            uint128 r1213;
                            r1213.low64 = productLow2;
                            r1213.high64 = productHigh2;
                            ret3 = r1213;
                        }
                        else
                        {
                            ulong y12 = rhs3 & 0xFFFFFFFF;
                            ulong loLo2 = (ulong) (uint) (lhs3 & 0xFFFFFFFF) * (ulong) (uint) (y12);
                            ulong y13 = rhs3 & 0xFFFFFFFF;
                            ulong hiLo2 = (ulong) (uint) (lhs3 >> 32) * (ulong) (uint) (y13);
                            ulong y14 = rhs3 >> 32;
                            ulong loHi2 = (ulong) (uint) (lhs3 & 0xFFFFFFFF) * (ulong) (uint) (y14);
                            ulong y15 = rhs3 >> 32;
                            ulong hiHi2 = (ulong) (uint) (lhs3 >> 32) * (ulong) (uint) (y15);

                            ulong cross3 = (loLo2 >> 32) + (hiLo2 & 0xFFFFFFFF) + loHi2;
                            ulong upper3 = (hiLo2 >> 32) + (cross3 >> 32) + hiHi2;
                            ulong lower3 = (cross3 << 32) | (loLo2 & 0xFFFFFFFF);

                            uint128 r1214;
                            r1214.low64 = lower3;
                            r1214.high64 = upper3;
                            ret3 = r1214;
                        }

                        uint128 product3 = ret3;
                        acc += product3.low64 ^ product3.high64;
                    }

                    byte* input5 = input + 16;
                    byte* secret5 = secret + 32;
                    ulong inputLo3 = *(ulong*) input5;
                    byte* ptr8 = input5 + 8;
                    ulong inputHi3 = *(ulong*) ptr8;

                    byte* ptr9 = secret5 + 8;
                    ulong rhs4 = inputHi3 ^ (*(ulong*) ptr9 - seed64);
                    ulong lhs4 = inputLo3 ^ (*(ulong*) secret5 + seed64);
                    uint128 ret4;
                    if (Bmi2.IsSupported)
                    {
                        ulong productLow3;
                        ulong productHigh3 = Bmi2.X64.MultiplyNoFlags(lhs4, rhs4, &productLow3);
                        uint128 r1215;
                        r1215.low64 = productLow3;
                        r1215.high64 = productHigh3;
                        ret4 = r1215;
                    }
                    else
                    {
                        ulong y16 = rhs4 & 0xFFFFFFFF;
                        ulong loLo3 = (ulong) (uint) (lhs4 & 0xFFFFFFFF) * (ulong) (uint) (y16);
                        ulong y17 = rhs4 & 0xFFFFFFFF;
                        ulong hiLo3 = (ulong) (uint) (lhs4 >> 32) * (ulong) (uint) (y17);
                        ulong y18 = rhs4 >> 32;
                        ulong loHi3 = (ulong) (uint) (lhs4 & 0xFFFFFFFF) * (ulong) (uint) (y18);
                        ulong y19 = rhs4 >> 32;
                        ulong hiHi3 = (ulong) (uint) (lhs4 >> 32) * (ulong) (uint) (y19);

                        ulong cross4 = (loLo3 >> 32) + (hiLo3 & 0xFFFFFFFF) + loHi3;
                        ulong upper4 = (hiLo3 >> 32) + (cross4 >> 32) + hiHi3;
                        ulong lower4 = (cross4 << 32) | (loLo3 & 0xFFFFFFFF);

                        uint128 r1216;
                        r1216.low64 = lower4;
                        r1216.high64 = upper4;
                        ret4 = r1216;
                    }

                    uint128 product4 = ret4;
                    acc += product4.low64 ^ product4.high64;
                    byte* input6 = input + len - 32;
                    byte* secret6 = secret + 48;
                    ulong inputLo4 = *(ulong*) input6;
                    byte* ptr10 = input6 + 8;
                    ulong inputHi4 = *(ulong*) ptr10;

                    byte* ptr11 = secret6 + 8;
                    ulong rhs5 = inputHi4 ^ (*(ulong*) ptr11 - seed64);
                    ulong lhs5 = inputLo4 ^ (*(ulong*) secret6 + seed64);
                    uint128 ret5;
                    if (Bmi2.IsSupported)
                    {
                        ulong productLow4;
                        ulong productHigh4 = Bmi2.X64.MultiplyNoFlags(lhs5, rhs5, &productLow4);
                        uint128 r1217;
                        r1217.low64 = productLow4;
                        r1217.high64 = productHigh4;
                        ret5 = r1217;
                    }
                    else
                    {
                        ulong y20 = rhs5 & 0xFFFFFFFF;
                        ulong loLo4 = (ulong) (uint) (lhs5 & 0xFFFFFFFF) * (ulong) (uint) (y20);
                        ulong y21 = rhs5 & 0xFFFFFFFF;
                        ulong hiLo4 = (ulong) (uint) (lhs5 >> 32) * (ulong) (uint) (y21);
                        ulong y22 = rhs5 >> 32;
                        ulong loHi4 = (ulong) (uint) (lhs5 & 0xFFFFFFFF) * (ulong) (uint) (y22);
                        ulong y23 = rhs5 >> 32;
                        ulong hiHi4 = (ulong) (uint) (lhs5 >> 32) * (ulong) (uint) (y23);

                        ulong cross5 = (loLo4 >> 32) + (hiLo4 & 0xFFFFFFFF) + loHi4;
                        ulong upper5 = (hiLo4 >> 32) + (cross5 >> 32) + hiHi4;
                        ulong lower5 = (cross5 << 32) | (loLo4 & 0xFFFFFFFF);

                        uint128 r1218;
                        r1218.low64 = lower5;
                        r1218.high64 = upper5;
                        ret5 = r1218;
                    }

                    uint128 product5 = ret5;
                    acc += product5.low64 ^ product5.high64;
                }

                byte* input7 = input + 0;
                byte* secret7 = secret + 0;
                ulong inputLo5 = *(ulong*) input7;
                byte* ptr12 = input7 + 8;
                ulong inputHi5 = *(ulong*) ptr12;

                byte* ptr13 = secret7 + 8;
                ulong rhs6 = inputHi5 ^ (*(ulong*) ptr13 - seed64);
                ulong lhs6 = inputLo5 ^ (*(ulong*) secret7 + seed64);
                uint128 ret6;
                if (Bmi2.IsSupported)
                {
                    ulong productLow5;
                    ulong productHigh5 = Bmi2.X64.MultiplyNoFlags(lhs6, rhs6, &productLow5);
                    uint128 r1219;
                    r1219.low64 = productLow5;
                    r1219.high64 = productHigh5;
                    ret6 = r1219;
                }
                else
                {
                    ulong y24 = rhs6 & 0xFFFFFFFF;
                    ulong loLo5 = (ulong) (uint) (lhs6 & 0xFFFFFFFF) * (ulong) (uint) (y24);
                    ulong y25 = rhs6 & 0xFFFFFFFF;
                    ulong hiLo5 = (ulong) (uint) (lhs6 >> 32) * (ulong) (uint) (y25);
                    ulong y26 = rhs6 >> 32;
                    ulong loHi5 = (ulong) (uint) (lhs6 & 0xFFFFFFFF) * (ulong) (uint) (y26);
                    ulong y27 = rhs6 >> 32;
                    ulong hiHi5 = (ulong) (uint) (lhs6 >> 32) * (ulong) (uint) (y27);

                    ulong cross6 = (loLo5 >> 32) + (hiLo5 & 0xFFFFFFFF) + loHi5;
                    ulong upper6 = (hiLo5 >> 32) + (cross6 >> 32) + hiHi5;
                    ulong lower6 = (cross6 << 32) | (loLo5 & 0xFFFFFFFF);

                    uint128 r1220;
                    r1220.low64 = lower6;
                    r1220.high64 = upper6;
                    ret6 = r1220;
                }

                uint128 product6 = ret6;
                acc += product6.low64 ^ product6.high64;
                byte* input8 = input + len - 16;
                byte* secret8 = secret + 16;
                ulong inputLo6 = *(ulong*) input8;
                byte* ptr14 = input8 + 8;
                ulong inputHi6 = *(ulong*) ptr14;

                byte* ptr15 = secret8 + 8;
                ulong rhs7 = inputHi6 ^ (*(ulong*) ptr15 - seed64);
                ulong lhs7 = inputLo6 ^ (*(ulong*) secret8 + seed64);
                uint128 ret7;
                if (Bmi2.IsSupported)
                {
                    ulong productLow6;
                    ulong productHigh6 = Bmi2.X64.MultiplyNoFlags(lhs7, rhs7, &productLow6);
                    uint128 r1221;
                    r1221.low64 = productLow6;
                    r1221.high64 = productHigh6;
                    ret7 = r1221;
                }
                else
                {
                    ulong y28 = rhs7 & 0xFFFFFFFF;
                    ulong loLo6 = (ulong) (uint) (lhs7 & 0xFFFFFFFF) * (ulong) (uint) (y28);
                    ulong y29 = rhs7 & 0xFFFFFFFF;
                    ulong hiLo6 = (ulong) (uint) (lhs7 >> 32) * (ulong) (uint) (y29);
                    ulong y30 = rhs7 >> 32;
                    ulong loHi6 = (ulong) (uint) (lhs7 & 0xFFFFFFFF) * (ulong) (uint) (y30);
                    ulong y31 = rhs7 >> 32;
                    ulong hiHi6 = (ulong) (uint) (lhs7 >> 32) * (ulong) (uint) (y31);

                    ulong cross7 = (loLo6 >> 32) + (hiLo6 & 0xFFFFFFFF) + loHi6;
                    ulong upper7 = (hiLo6 >> 32) + (cross7 >> 32) + hiHi6;
                    ulong lower7 = (cross7 << 32) | (loLo6 & 0xFFFFFFFF);

                    uint128 r1222;
                    r1222.low64 = lower7;
                    r1222.high64 = upper7;
                    ret7 = r1222;
                }

                uint128 product7 = ret7;
                acc += product7.low64 ^ product7.high64;
                ulong h64 = acc;
                h64 = h64 ^ (h64 >> 37);
                h64 *= 0x165667919E3779F9UL;
                h64 = h64 ^ (h64 >> 32);
                return h64;
            }

            if (len <= XXH3_MIDSIZE_MAX)
            {
                ulong acc = ((ulong) len) * XXH_PRIME64_1;
                int nbRounds = len / 16;

                for (int i = 0; i < 8; i++)
                {
                    byte* input1 = input + (16 * i);
                    byte* secret1 = secret + (16 * i);
                    ulong input_lo = *(ulong*) input1;
                    byte* ptr = input1 + 8;
                    ulong input_hi = *(ulong*) ptr;

                    byte* ptr1 = secret1 + 8;
                    ulong rhs = input_hi ^ (*(ulong*) ptr1 - seed64);
                    ulong lhs = input_lo ^ (*(ulong*) secret1 + seed64);
                    uint128 ret;
                    if (Bmi2.IsSupported)
                    {
                        ulong product_low;
                        ulong product_high = Bmi2.X64.MultiplyNoFlags(lhs, rhs, &product_low);
                        uint128 r128;
                        r128.low64 = product_low;
                        r128.high64 = product_high;
                        ret = r128;
                    }
                    else
                    {
                        ulong y = rhs & 0xFFFFFFFF;
                        ulong lo_lo = (ulong) (uint) (lhs & 0xFFFFFFFF) * (ulong) (uint) (y);
                        ulong y1 = rhs & 0xFFFFFFFF;
                        ulong hi_lo = (ulong) (uint) (lhs >> 32) * (ulong) (uint) (y1);
                        ulong y2 = rhs >> 32;
                        ulong lo_hi = (ulong) (uint) (lhs & 0xFFFFFFFF) * (ulong) (uint) (y2);
                        ulong y3 = rhs >> 32;
                        ulong hi_hi = (ulong) (uint) (lhs >> 32) * (ulong) (uint) (y3);

                        ulong cross = (lo_lo >> 32) + (hi_lo & 0xFFFFFFFF) + lo_hi;
                        ulong upper = (hi_lo >> 32) + (cross >> 32) + hi_hi;
                        ulong lower = (cross << 32) | (lo_lo & 0xFFFFFFFF);

                        uint128 r128;
                        r128.low64 = lower;
                        r128.high64 = upper;
                        ret = r128;
                    }

                    uint128 product = ret;
                    acc += product.low64 ^ product.high64;
                }

                ulong h64 = acc;
                h64 = h64 ^ (h64 >> 37);
                h64 *= 0x165667919E3779F9UL;
                h64 = h64 ^ (h64 >> 32);
                acc = h64;

                for (int i = 8; i < nbRounds; i++)
                {
                    byte* input1 = input + (16 * i);
                    byte* secret1 = secret + (16 * (i - 8)) + XXH3_MIDSIZE_STARTOFFSET;
                    ulong input_lo = *(ulong*) input1;
                    byte* ptr = input1 + 8;
                    ulong input_hi = *(ulong*) ptr;

                    byte* ptr1 = secret1 + 8;
                    ulong rhs = input_hi ^ (*(ulong*) ptr1 - seed64);
                    ulong lhs = input_lo ^ (*(ulong*) secret1 + seed64);
                    uint128 ret;
                    if (Bmi2.IsSupported)
                    {
                        ulong product_low;
                        ulong product_high = Bmi2.X64.MultiplyNoFlags(lhs, rhs, &product_low);
                        uint128 r128;
                        r128.low64 = product_low;
                        r128.high64 = product_high;
                        ret = r128;
                    }
                    else
                    {
                        ulong y = rhs & 0xFFFFFFFF;
                        ulong lo_lo = (ulong) (uint) (lhs & 0xFFFFFFFF) * (ulong) (uint) (y);
                        ulong y1 = rhs & 0xFFFFFFFF;
                        ulong hi_lo = (ulong) (uint) (lhs >> 32) * (ulong) (uint) (y1);
                        ulong y2 = rhs >> 32;
                        ulong lo_hi = (ulong) (uint) (lhs & 0xFFFFFFFF) * (ulong) (uint) (y2);
                        ulong y3 = rhs >> 32;
                        ulong hi_hi = (ulong) (uint) (lhs >> 32) * (ulong) (uint) (y3);

                        ulong cross = (lo_lo >> 32) + (hi_lo & 0xFFFFFFFF) + lo_hi;
                        ulong upper = (hi_lo >> 32) + (cross >> 32) + hi_hi;
                        ulong lower = (cross << 32) | (lo_lo & 0xFFFFFFFF);

                        uint128 r128;
                        r128.low64 = lower;
                        r128.high64 = upper;
                        ret = r128;
                    }

                    uint128 product = ret;
                    acc += product.low64 ^ product.high64;
                }

                byte* input2 = input + len - 16;
                byte* secret2 = secret + XXH3_SECRET_SIZE_MIN - XXH3_MIDSIZE_LASTOFFSET;
                ulong inputLo = *(ulong*) input2;
                byte* ptr2 = input2 + 8;
                ulong inputHi = *(ulong*) ptr2;

                byte* ptr3 = secret2 + 8;
                ulong rhs1 = inputHi ^ (*(ulong*) ptr3 - seed64);
                ulong lhs1 = inputLo ^ (*(ulong*) secret2 + seed64);
                uint128 ret1;
                if (Bmi2.IsSupported)
                {
                    ulong productLow;
                    ulong productHigh = Bmi2.X64.MultiplyNoFlags(lhs1, rhs1, &productLow);
                    uint128 r129;
                    r129.low64 = productLow;
                    r129.high64 = productHigh;
                    ret1 = r129;
                }
                else
                {
                    ulong y4 = rhs1 & 0xFFFFFFFF;
                    ulong loLo = (ulong) (uint) (lhs1 & 0xFFFFFFFF) * (ulong) (uint) (y4);
                    ulong y5 = rhs1 & 0xFFFFFFFF;
                    ulong hiLo = (ulong) (uint) (lhs1 >> 32) * (ulong) (uint) (y5);
                    ulong y6 = rhs1 >> 32;
                    ulong loHi = (ulong) (uint) (lhs1 & 0xFFFFFFFF) * (ulong) (uint) (y6);
                    ulong y7 = rhs1 >> 32;
                    ulong hiHi = (ulong) (uint) (lhs1 >> 32) * (ulong) (uint) (y7);

                    ulong cross1 = (loLo >> 32) + (hiLo & 0xFFFFFFFF) + loHi;
                    ulong upper1 = (hiLo >> 32) + (cross1 >> 32) + hiHi;
                    ulong lower1 = (cross1 << 32) | (loLo & 0xFFFFFFFF);

                    uint128 r1210;
                    r1210.low64 = lower1;
                    r1210.high64 = upper1;
                    ret1 = r1210;
                }

                uint128 product1 = ret1;
                acc += product1.low64 ^ product1.high64;
                ulong h65 = acc;
                h65 = h65 ^ (h65 >> 37);
                h65 *= 0x165667919E3779F9UL;
                h65 = h65 ^ (h65 >> 32);
                return h65;
            }

            if (seed64 == 0)
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

                int nbStripesPerBlock = (secretLen - XXH_STRIPE_LEN) / XXH_SECRET_CONSUME_RATE;
                int block_len = XXH_STRIPE_LEN * nbStripesPerBlock;
                int nb_blocks = (len - 1) / block_len;

                for (int n = 0; n < nb_blocks; n++)
                {
                    byte* input1 = input + n * block_len;
                    for (int n1 = 0; n1 < nbStripesPerBlock; n1++)
                    {
                        byte* inp = input1 + n1 * XXH_STRIPE_LEN;
                        byte* secret1 = secret + n1 * XXH_SECRET_CONSUME_RATE;
                        if (Avx2.IsSupported)
                        {
                            const int m256i_size = 32;
                            const byte _MM_SHUFFLE_0_3_0_1 = 0b0011_0001;
                            const byte _MM_SHUFFLE_1_0_3_2 = 0b0100_1110;

                            for (int i = 0; i < XXH_STRIPE_LEN / m256i_size; i++)
                            {
                                int uint32_offset = i * 8;
                                int uint64_offset = i * 4;

                                var acc_vec = Avx2.LoadVector256(acc + uint64_offset);
                                var data_vec = Avx2.LoadVector256((uint*) inp + uint32_offset);
                                var key_vec = Avx2.LoadVector256((uint*) secret1 + uint32_offset);
                                var data_key = Avx2.Xor(data_vec, key_vec);
                                var data_key_lo = Avx2.Shuffle(data_key, _MM_SHUFFLE_0_3_0_1);
                                var product = Avx2.Multiply(data_key, data_key_lo);
                                var data_swap = Avx2.Shuffle(data_vec, _MM_SHUFFLE_1_0_3_2).AsUInt64();
                                var sum = Avx2.Add(acc_vec, data_swap);
                                var result = Avx2.Add(product, sum);
                                Avx2.Store(acc + uint64_offset, result);
                            }
                        }
                        else if (Sse2.IsSupported)
                        {
                            const int m128i_size = 16;
                            const byte _MM_SHUFFLE_0_3_0_1 = 0b0011_0001;
                            const byte _MM_SHUFFLE_1_0_3_2 = 0b0100_1110;

                            for (int i = 0; i < XXH_STRIPE_LEN / m128i_size; i++)
                            {
                                int uint32_offset = i * 4;
                                int uint64_offset = i * 2;

                                var acc_vec = Sse2.LoadVector128(acc + uint64_offset);
                                var data_vec = Sse2.LoadVector128((uint*) inp + uint32_offset);
                                var key_vec = Sse2.LoadVector128((uint*) secret1 + uint32_offset);
                                var data_key = Sse2.Xor(data_vec, key_vec);
                                var data_key_lo = Sse2.Shuffle(data_key, _MM_SHUFFLE_0_3_0_1);
                                var product = Sse2.Multiply(data_key, data_key_lo);
                                var data_swap = Sse2.Shuffle(data_vec, _MM_SHUFFLE_1_0_3_2).AsUInt64();
                                var sum = Sse2.Add(acc_vec, data_swap);
                                var result = Sse2.Add(product, sum);
                                Sse2.Store(acc + uint64_offset, result);
                            }
                        }
                        else
                        {
                            for (int i = 0; i < XXH_ACC_NB; i++)
                            {
                                ulong* xacc = acc;
                                byte* xinput = inp;
                                byte* xsecret = secret1;

                                byte* ptr = xinput + i * 8;
                                ulong data_val = *(ulong*) ptr;
                                byte* ptr1 = xsecret + i * 8;
                                ulong data_key = data_val ^ *(ulong*) ptr1;
                                xacc[i ^ 1] += data_val;
                                ulong y = data_key >> 32;
                                xacc[i] += (ulong) (uint) (data_key & 0xFFFFFFFF) * (ulong) (uint) (y);
                            }
                        }
                    }

                    byte* secret3 = secret + secretLen - XXH_STRIPE_LEN;
                    if (Avx2.IsSupported)
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
                            var key_vec = Avx2.LoadVector256((ulong*) secret3 + uint64_offset);
                            var data_key = Avx2.Xor(data_vec, key_vec).AsUInt32();
                            var data_key_hi = Avx2.Shuffle(data_key, _MM_SHUFFLE_0_3_0_1);
                            var prod_lo = Avx2.Multiply(data_key, prime32);
                            var prod_hi = Avx2.Multiply(data_key_hi, prime32);
                            var result = Avx2.Add(prod_lo, Avx2.ShiftLeftLogical(prod_hi, 32));
                            Avx2.Store(acc + uint64_offset, result);
                        }
                    }
                    else if (Sse2.IsSupported)
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
                            var key_vec = Sse2.LoadVector128((uint*) secret3 + uint32_offset);
                            var data_key = Sse2.Xor(data_vec, key_vec);
                            var data_key_hi = Sse2.Shuffle(data_key.AsUInt32(), _MM_SHUFFLE_0_3_0_1);
                            var prod_lo = Sse2.Multiply(data_key, prime32);
                            var prod_hi = Sse2.Multiply(data_key_hi, prime32);
                            var result = Sse2.Add(prod_lo, Sse2.ShiftLeftLogical(prod_hi, 32));
                            Sse2.Store(acc + uint64_offset, result);
                        }
                    }
                    else
                    {
                        for (int i = 0; i < XXH_ACC_NB; i++)
                        {
                            ulong* xacc = acc;
                            byte* xsecret = secret3;

                            byte* ptr = xsecret + i * 8;
                            ulong key64 = *(ulong*) ptr;
                            ulong acc64 = xacc[i];
                            acc64 = acc64 ^ (acc64 >> 47);
                            acc64 ^= key64;
                            acc64 *= XXH_PRIME32_1;
                            xacc[i] = acc64;
                        }
                    }
                }

                int nbStripes = ((len - 1) - (block_len * nb_blocks)) / XXH_STRIPE_LEN;
                byte* input2 = input + nb_blocks * block_len;
                for (int n2 = 0; n2 < nbStripes; n2++)
                {
                    byte* inp1 = input2 + n2 * XXH_STRIPE_LEN;
                    byte* secret1 = secret + n2 * XXH_SECRET_CONSUME_RATE;
                    if (Avx2.IsSupported)
                    {
                        const int m256i_size = 32;
                        const byte _MM_SHUFFLE_0_3_0_1 = 0b0011_0001;
                        const byte _MM_SHUFFLE_1_0_3_2 = 0b0100_1110;

                        for (int i = 0; i < XXH_STRIPE_LEN / m256i_size; i++)
                        {
                            int uint32_offset = i * 8;
                            int uint64_offset = i * 4;

                            var acc_vec = Avx2.LoadVector256(acc + uint64_offset);
                            var data_vec = Avx2.LoadVector256((uint*) inp1 + uint32_offset);
                            var key_vec = Avx2.LoadVector256((uint*) secret1 + uint32_offset);
                            var data_key = Avx2.Xor(data_vec, key_vec);
                            var data_key_lo = Avx2.Shuffle(data_key, _MM_SHUFFLE_0_3_0_1);
                            var product = Avx2.Multiply(data_key, data_key_lo);
                            var data_swap = Avx2.Shuffle(data_vec, _MM_SHUFFLE_1_0_3_2).AsUInt64();
                            var sum = Avx2.Add(acc_vec, data_swap);
                            var result = Avx2.Add(product, sum);
                            Avx2.Store(acc + uint64_offset, result);
                        }
                    }
                    else if (Sse2.IsSupported)
                    {
                        const int m128i_size = 16;
                        const byte _MM_SHUFFLE_0_3_0_1 = 0b0011_0001;
                        const byte _MM_SHUFFLE_1_0_3_2 = 0b0100_1110;

                        for (int i = 0; i < XXH_STRIPE_LEN / m128i_size; i++)
                        {
                            int uint32_offset = i * 4;
                            int uint64_offset = i * 2;

                            var acc_vec = Sse2.LoadVector128(acc + uint64_offset);
                            var data_vec = Sse2.LoadVector128((uint*) inp1 + uint32_offset);
                            var key_vec = Sse2.LoadVector128((uint*) secret1 + uint32_offset);
                            var data_key = Sse2.Xor(data_vec, key_vec);
                            var data_key_lo = Sse2.Shuffle(data_key, _MM_SHUFFLE_0_3_0_1);
                            var product = Sse2.Multiply(data_key, data_key_lo);
                            var data_swap = Sse2.Shuffle(data_vec, _MM_SHUFFLE_1_0_3_2).AsUInt64();
                            var sum = Sse2.Add(acc_vec, data_swap);
                            var result = Sse2.Add(product, sum);
                            Sse2.Store(acc + uint64_offset, result);
                        }
                    }
                    else
                    {
                        for (int i = 0; i < XXH_ACC_NB; i++)
                        {
                            ulong* xacc = acc;
                            byte* xinput = inp1;
                            byte* xsecret = secret1;

                            byte* ptr = xinput + i * 8;
                            ulong data_val = *(ulong*) ptr;
                            byte* ptr1 = xsecret + i * 8;
                            ulong data_key = data_val ^ *(ulong*) ptr1;
                            xacc[i ^ 1] += data_val;
                            ulong y = data_key >> 32;
                            xacc[i] += (ulong) (uint) (data_key & 0xFFFFFFFF) * (ulong) (uint) (y);
                        }
                    }
                }

                byte* p = input + len - XXH_STRIPE_LEN;
                byte* secret2 = secret + secretLen - XXH_STRIPE_LEN - XXH_SECRET_LASTACC_START;
                if (Avx2.IsSupported)
                {
                    const int m256i_size = 32;
                    const byte _MM_SHUFFLE_0_3_0_1 = 0b0011_0001;
                    const byte _MM_SHUFFLE_1_0_3_2 = 0b0100_1110;

                    for (int i = 0; i < XXH_STRIPE_LEN / m256i_size; i++)
                    {
                        int uint32_offset = i * 8;
                        int uint64_offset = i * 4;

                        var acc_vec = Avx2.LoadVector256(acc + uint64_offset);
                        var data_vec = Avx2.LoadVector256((uint*) p + uint32_offset);
                        var key_vec = Avx2.LoadVector256((uint*) secret2 + uint32_offset);
                        var data_key = Avx2.Xor(data_vec, key_vec);
                        var data_key_lo = Avx2.Shuffle(data_key, _MM_SHUFFLE_0_3_0_1);
                        var product = Avx2.Multiply(data_key, data_key_lo);
                        var data_swap = Avx2.Shuffle(data_vec, _MM_SHUFFLE_1_0_3_2).AsUInt64();
                        var sum = Avx2.Add(acc_vec, data_swap);
                        var result = Avx2.Add(product, sum);
                        Avx2.Store(acc + uint64_offset, result);
                    }
                }
                else if (Sse2.IsSupported)
                {
                    const int m128i_size = 16;
                    const byte _MM_SHUFFLE_0_3_0_1 = 0b0011_0001;
                    const byte _MM_SHUFFLE_1_0_3_2 = 0b0100_1110;

                    for (int i = 0; i < XXH_STRIPE_LEN / m128i_size; i++)
                    {
                        int uint32_offset = i * 4;
                        int uint64_offset = i * 2;

                        var acc_vec = Sse2.LoadVector128(acc + uint64_offset);
                        var data_vec = Sse2.LoadVector128((uint*) p + uint32_offset);
                        var key_vec = Sse2.LoadVector128((uint*) secret2 + uint32_offset);
                        var data_key = Sse2.Xor(data_vec, key_vec);
                        var data_key_lo = Sse2.Shuffle(data_key, _MM_SHUFFLE_0_3_0_1);
                        var product = Sse2.Multiply(data_key, data_key_lo);
                        var data_swap = Sse2.Shuffle(data_vec, _MM_SHUFFLE_1_0_3_2).AsUInt64();
                        var sum = Sse2.Add(acc_vec, data_swap);
                        var result = Sse2.Add(product, sum);
                        Sse2.Store(acc + uint64_offset, result);
                    }
                }
                else
                {
                    for (int i = 0; i < XXH_ACC_NB; i++)
                    {
                        ulong* xacc = acc;
                        byte* xinput = p;
                        byte* xsecret = secret2;

                        byte* ptr = xinput + i * 8;
                        ulong data_val = *(ulong*) ptr;
                        byte* ptr1 = xsecret + i * 8;
                        ulong data_key = data_val ^ *(ulong*) ptr1;
                        xacc[i ^ 1] += data_val;
                        ulong y = data_key >> 32;
                        xacc[i] += (ulong) (uint) (data_key & 0xFFFFFFFF) * (ulong) (uint) (y);
                    }
                }

                byte* secret4 = secret + XXH_SECRET_MERGEACCS_START;
                ulong result64 = ((ulong) len) * XXH_PRIME64_1;

                for (int i1 = 0; i1 < 4; i1++)
                {
                    ulong* acc2 = acc + 2 * i1;
                    byte* secret1 = secret4 + 16 * i1;
                    byte* ptr = secret1 + 8;
                    ulong rhs = acc2[1] ^ *(ulong*) ptr;
                    ulong lhs = acc2[0] ^ *(ulong*) secret1;
                    uint128 ret;
                    if (Bmi2.IsSupported)
                    {
                        ulong product_low;
                        ulong product_high = Bmi2.X64.MultiplyNoFlags(lhs, rhs, &product_low);
                        uint128 r128;
                        r128.low64 = product_low;
                        r128.high64 = product_high;
                        ret = r128;
                    }
                    else
                    {
                        ulong y = rhs & 0xFFFFFFFF;
                        ulong lo_lo = (ulong) (uint) (lhs & 0xFFFFFFFF) * (ulong) (uint) (y);
                        ulong y1 = rhs & 0xFFFFFFFF;
                        ulong hi_lo = (ulong) (uint) (lhs >> 32) * (ulong) (uint) (y1);
                        ulong y2 = rhs >> 32;
                        ulong lo_hi = (ulong) (uint) (lhs & 0xFFFFFFFF) * (ulong) (uint) (y2);
                        ulong y3 = rhs >> 32;
                        ulong hi_hi = (ulong) (uint) (lhs >> 32) * (ulong) (uint) (y3);

                        ulong cross = (lo_lo >> 32) + (hi_lo & 0xFFFFFFFF) + lo_hi;
                        ulong upper = (hi_lo >> 32) + (cross >> 32) + hi_hi;
                        ulong lower = (cross << 32) | (lo_lo & 0xFFFFFFFF);

                        uint128 r128;
                        r128.low64 = lower;
                        r128.high64 = upper;
                        ret = r128;
                    }

                    uint128 product = ret;
                    result64 += product.low64 ^ product.high64;
                }

                ulong h64 = result64;
                h64 = h64 ^ (h64 >> 37);
                h64 *= 0x165667919E3779F9UL;
                h64 = h64 ^ (h64 >> 32);
                return h64;
            }

            int customSecretSize = XXH3_SECRET_DEFAULT_SIZE;
            byte* customSecret = stackalloc byte[customSecretSize];

            fixed (byte* ptr = &XXH3_SECRET[0])
            {
                for (int i1 = 0; i1 < customSecretSize; i1 += 8)
                {
                    customSecret[i1] = ptr[i1];
                    customSecret[i1 + 1] = ptr[i1 + 1];
                    customSecret[i1 + 2] = ptr[i1 + 2];
                    customSecret[i1 + 3] = ptr[i1 + 3];
                    customSecret[i1 + 4] = ptr[i1 + 4];
                    customSecret[i1 + 5] = ptr[i1 + 5];
                    customSecret[i1 + 6] = ptr[i1 + 6];
                    customSecret[i1 + 7] = ptr[i1 + 7];
                }
            }

            if (Avx2.IsSupported)
            {
                const int m256i_size = 32;

                var seed = Vector256.Create(seed64, 0U - seed64, seed64, 0U - seed64);

                fixed (byte* secret1 = &XXH3_SECRET[0])
                {
                    for (int i = 0; i < XXH_SECRET_DEFAULT_SIZE / m256i_size; i++)
                    {
                        int uint64_offset = i * 4;

                        var src32 = Avx2.LoadVector256(((ulong*) secret1) + uint64_offset);
                        var dst32 = Avx2.Add(src32, seed);
                        Avx2.Store((ulong*) customSecret + uint64_offset, dst32);
                    }
                }
            }
            else if (Sse2.IsSupported)
            {
                const int m128i_size = 16;

                var seed = Vector128.Create((long) seed64, (long) (0U - seed64));

                fixed (byte* secret1 = &XXH3_SECRET[0])
                {
                    for (int i = 0; i < XXH_SECRET_DEFAULT_SIZE / m128i_size; i++)
                    {
                        int uint64_offset = i * 2;

                        var src16 = Sse2.LoadVector128(((long*) secret1) + uint64_offset);
                        var dst16 = Sse2.Add(src16, seed);
                        Sse2.Store((long*) customSecret + uint64_offset, dst16);
                    }
                }
            }
            else
            {
                fixed (byte* kSecretPtr = &XXH3_SECRET[0])
                {
                    int nbRounds = XXH_SECRET_DEFAULT_SIZE / 16;

                    for (int i = 0; i < nbRounds; i++)
                    {
                        byte* ptr = kSecretPtr + 16 * i;
                        ulong lo = *(ulong*) ptr + seed64;
                        byte* ptr1 = kSecretPtr + 16 * i + 8;
                        ulong hi = *(ulong*) ptr1 - seed64;
                        byte* dst = (byte*) customSecret + 16 * i;
                        *(ulong*) dst = lo;
                        byte* dst1 = (byte*) customSecret + 16 * i + 8;
                        *(ulong*) dst1 = hi;
                    }
                }
            }

            ulong* acc1 = stackalloc ulong[8];

            fixed (ulong* ptr16 = &XXH3_INIT_ACC[0])
            {
                acc1[0] = ptr16[0];
                acc1[1] = ptr16[1];
                acc1[2] = ptr16[2];
                acc1[3] = ptr16[3];
                acc1[4] = ptr16[4];
                acc1[5] = ptr16[5];
                acc1[6] = ptr16[6];
                acc1[7] = ptr16[7];
            }

            int nbStripesPerBlock1 = (customSecretSize - XXH_STRIPE_LEN) / XXH_SECRET_CONSUME_RATE;
            int blockLen = XXH_STRIPE_LEN * nbStripesPerBlock1;
            int nbBlocks = (len - 1) / blockLen;

            for (int n1 = 0; n1 < nbBlocks; n1++)
            {
                byte* input1 = input + n1 * blockLen;
                for (int n = 0; n < nbStripesPerBlock1; n++)
                {
                    byte* inp = input1 + n * XXH_STRIPE_LEN;
                    byte* secret1 = customSecret + n * XXH_SECRET_CONSUME_RATE;
                    if (Avx2.IsSupported)
                    {
                        const int m256i_size = 32;
                        const byte _MM_SHUFFLE_0_3_0_1 = 0b0011_0001;
                        const byte _MM_SHUFFLE_1_0_3_2 = 0b0100_1110;

                        for (int i = 0; i < XXH_STRIPE_LEN / m256i_size; i++)
                        {
                            int uint32_offset = i * 8;
                            int uint64_offset = i * 4;

                            var acc_vec = Avx2.LoadVector256(acc1 + uint64_offset);
                            var data_vec = Avx2.LoadVector256((uint*) inp + uint32_offset);
                            var key_vec = Avx2.LoadVector256((uint*) secret1 + uint32_offset);
                            var data_key = Avx2.Xor(data_vec, key_vec);
                            var data_key_lo = Avx2.Shuffle(data_key, _MM_SHUFFLE_0_3_0_1);
                            var product = Avx2.Multiply(data_key, data_key_lo);
                            var data_swap = Avx2.Shuffle(data_vec, _MM_SHUFFLE_1_0_3_2).AsUInt64();
                            var sum = Avx2.Add(acc_vec, data_swap);
                            var result = Avx2.Add(product, sum);
                            Avx2.Store(acc1 + uint64_offset, result);
                        }
                    }
                    else if (Sse2.IsSupported)
                    {
                        const int m128i_size = 16;
                        const byte _MM_SHUFFLE_0_3_0_1 = 0b0011_0001;
                        const byte _MM_SHUFFLE_1_0_3_2 = 0b0100_1110;

                        for (int i = 0; i < XXH_STRIPE_LEN / m128i_size; i++)
                        {
                            int uint32_offset = i * 4;
                            int uint64_offset = i * 2;

                            var acc_vec = Sse2.LoadVector128(acc1 + uint64_offset);
                            var data_vec = Sse2.LoadVector128((uint*) inp + uint32_offset);
                            var key_vec = Sse2.LoadVector128((uint*) secret1 + uint32_offset);
                            var data_key = Sse2.Xor(data_vec, key_vec);
                            var data_key_lo = Sse2.Shuffle(data_key, _MM_SHUFFLE_0_3_0_1);
                            var product = Sse2.Multiply(data_key, data_key_lo);
                            var data_swap = Sse2.Shuffle(data_vec, _MM_SHUFFLE_1_0_3_2).AsUInt64();
                            var sum = Sse2.Add(acc_vec, data_swap);
                            var result = Sse2.Add(product, sum);
                            Sse2.Store(acc1 + uint64_offset, result);
                        }
                    }
                    else
                    {
                        for (int i = 0; i < XXH_ACC_NB; i++)
                        {
                            ulong* xacc = acc1;
                            byte* xinput = inp;
                            byte* xsecret = secret1;

                            byte* ptr = xinput + i * 8;
                            ulong data_val = *(ulong*) ptr;
                            byte* ptr1 = xsecret + i * 8;
                            ulong data_key = data_val ^ *(ulong*) ptr1;
                            xacc[i ^ 1] += data_val;
                            ulong y = data_key >> 32;
                            xacc[i] += (ulong) (uint) (data_key & 0xFFFFFFFF) * (ulong) (uint) (y);
                        }
                    }
                }

                byte* secret2 = customSecret + customSecretSize - XXH_STRIPE_LEN;
                if (Avx2.IsSupported)
                {
                    const int m256i_size = 32;
                    const byte _MM_SHUFFLE_0_3_0_1 = 0b0011_0001;

                    var prime32 = Vector256.Create(XXH_PRIME32_1);

                    for (int i = 0; i < XXH_STRIPE_LEN / m256i_size; i++)
                    {
                        int uint64_offset = i * 4;

                        var acc_vec = Avx2.LoadVector256(acc1 + uint64_offset);
                        var shifted = Avx2.ShiftRightLogical(acc_vec, 47);
                        var data_vec = Avx2.Xor(acc_vec, shifted);
                        var key_vec = Avx2.LoadVector256((ulong*) secret2 + uint64_offset);
                        var data_key = Avx2.Xor(data_vec, key_vec).AsUInt32();
                        var data_key_hi = Avx2.Shuffle(data_key, _MM_SHUFFLE_0_3_0_1);
                        var prod_lo = Avx2.Multiply(data_key, prime32);
                        var prod_hi = Avx2.Multiply(data_key_hi, prime32);
                        var result = Avx2.Add(prod_lo, Avx2.ShiftLeftLogical(prod_hi, 32));
                        Avx2.Store(acc1 + uint64_offset, result);
                    }
                }
                else if (Sse2.IsSupported)
                {
                    const int m128i_size = 16;
                    const byte _MM_SHUFFLE_0_3_0_1 = 0b0011_0001;

                    var prime32 = Vector128.Create(XXH_PRIME32_1);

                    for (int i = 0; i < XXH_STRIPE_LEN / m128i_size; i++)
                    {
                        int uint32_offset = i * 4;
                        int uint64_offset = i * 2;

                        var acc_vec = Sse2.LoadVector128(acc1 + uint64_offset).AsUInt32();
                        var shifted = Sse2.ShiftRightLogical(acc_vec, 47);
                        var data_vec = Sse2.Xor(acc_vec, shifted);
                        var key_vec = Sse2.LoadVector128((uint*) secret2 + uint32_offset);
                        var data_key = Sse2.Xor(data_vec, key_vec);
                        var data_key_hi = Sse2.Shuffle(data_key.AsUInt32(), _MM_SHUFFLE_0_3_0_1);
                        var prod_lo = Sse2.Multiply(data_key, prime32);
                        var prod_hi = Sse2.Multiply(data_key_hi, prime32);
                        var result = Sse2.Add(prod_lo, Sse2.ShiftLeftLogical(prod_hi, 32));
                        Sse2.Store(acc1 + uint64_offset, result);
                    }
                }
                else
                {
                    for (int i = 0; i < XXH_ACC_NB; i++)
                    {
                        ulong* xacc = acc1;
                        byte* xsecret = secret2;

                        byte* ptr = xsecret + i * 8;
                        ulong key64 = *(ulong*) ptr;
                        ulong acc64 = xacc[i];
                        acc64 = acc64 ^ (acc64 >> 47);
                        acc64 ^= key64;
                        acc64 *= XXH_PRIME32_1;
                        xacc[i] = acc64;
                    }
                }
            }

            int nbStripes1 = ((len - 1) - (blockLen * nbBlocks)) / XXH_STRIPE_LEN;
            byte* input9 = input + nbBlocks * blockLen;
            for (int n3 = 0; n3 < nbStripes1; n3++)
            {
                byte* inp2 = input9 + n3 * XXH_STRIPE_LEN;
                byte* secret1 = customSecret + n3 * XXH_SECRET_CONSUME_RATE;
                if (Avx2.IsSupported)
                {
                    const int m256i_size = 32;
                    const byte _MM_SHUFFLE_0_3_0_1 = 0b0011_0001;
                    const byte _MM_SHUFFLE_1_0_3_2 = 0b0100_1110;

                    for (int i = 0; i < XXH_STRIPE_LEN / m256i_size; i++)
                    {
                        int uint32_offset = i * 8;
                        int uint64_offset = i * 4;

                        var acc_vec = Avx2.LoadVector256(acc1 + uint64_offset);
                        var data_vec = Avx2.LoadVector256((uint*) inp2 + uint32_offset);
                        var key_vec = Avx2.LoadVector256((uint*) secret1 + uint32_offset);
                        var data_key = Avx2.Xor(data_vec, key_vec);
                        var data_key_lo = Avx2.Shuffle(data_key, _MM_SHUFFLE_0_3_0_1);
                        var product = Avx2.Multiply(data_key, data_key_lo);
                        var data_swap = Avx2.Shuffle(data_vec, _MM_SHUFFLE_1_0_3_2).AsUInt64();
                        var sum = Avx2.Add(acc_vec, data_swap);
                        var result = Avx2.Add(product, sum);
                        Avx2.Store(acc1 + uint64_offset, result);
                    }
                }
                else if (Sse2.IsSupported)
                {
                    const int m128i_size = 16;
                    const byte _MM_SHUFFLE_0_3_0_1 = 0b0011_0001;
                    const byte _MM_SHUFFLE_1_0_3_2 = 0b0100_1110;

                    for (int i = 0; i < XXH_STRIPE_LEN / m128i_size; i++)
                    {
                        int uint32_offset = i * 4;
                        int uint64_offset = i * 2;

                        var acc_vec = Sse2.LoadVector128(acc1 + uint64_offset);
                        var data_vec = Sse2.LoadVector128((uint*) inp2 + uint32_offset);
                        var key_vec = Sse2.LoadVector128((uint*) secret1 + uint32_offset);
                        var data_key = Sse2.Xor(data_vec, key_vec);
                        var data_key_lo = Sse2.Shuffle(data_key, _MM_SHUFFLE_0_3_0_1);
                        var product = Sse2.Multiply(data_key, data_key_lo);
                        var data_swap = Sse2.Shuffle(data_vec, _MM_SHUFFLE_1_0_3_2).AsUInt64();
                        var sum = Sse2.Add(acc_vec, data_swap);
                        var result = Sse2.Add(product, sum);
                        Sse2.Store(acc1 + uint64_offset, result);
                    }
                }
                else
                {
                    for (int i = 0; i < XXH_ACC_NB; i++)
                    {
                        ulong* xacc = acc1;
                        byte* xinput = inp2;
                        byte* xsecret = secret1;

                        byte* ptr = xinput + i * 8;
                        ulong data_val = *(ulong*) ptr;
                        byte* ptr1 = xsecret + i * 8;
                        ulong data_key = data_val ^ *(ulong*) ptr1;
                        xacc[i ^ 1] += data_val;
                        ulong y = data_key >> 32;
                        xacc[i] += (ulong) (uint) (data_key & 0xFFFFFFFF) * (ulong) (uint) (y);
                    }
                }
            }

            byte* p1 = input + len - XXH_STRIPE_LEN;
            byte* secret9 = customSecret + customSecretSize - XXH_STRIPE_LEN - XXH_SECRET_LASTACC_START;
            if (Avx2.IsSupported)
            {
                const int m256i_size = 32;
                const byte _MM_SHUFFLE_0_3_0_1 = 0b0011_0001;
                const byte _MM_SHUFFLE_1_0_3_2 = 0b0100_1110;

                for (int i = 0; i < XXH_STRIPE_LEN / m256i_size; i++)
                {
                    int uint32_offset = i * 8;
                    int uint64_offset = i * 4;

                    var acc_vec = Avx2.LoadVector256(acc1 + uint64_offset);
                    var data_vec = Avx2.LoadVector256((uint*) p1 + uint32_offset);
                    var key_vec = Avx2.LoadVector256((uint*) secret9 + uint32_offset);
                    var data_key = Avx2.Xor(data_vec, key_vec);
                    var data_key_lo = Avx2.Shuffle(data_key, _MM_SHUFFLE_0_3_0_1);
                    var product = Avx2.Multiply(data_key, data_key_lo);
                    var data_swap = Avx2.Shuffle(data_vec, _MM_SHUFFLE_1_0_3_2).AsUInt64();
                    var sum = Avx2.Add(acc_vec, data_swap);
                    var result = Avx2.Add(product, sum);
                    Avx2.Store(acc1 + uint64_offset, result);
                }
            }
            else if (Sse2.IsSupported)
            {
                const int m128i_size = 16;
                const byte _MM_SHUFFLE_0_3_0_1 = 0b0011_0001;
                const byte _MM_SHUFFLE_1_0_3_2 = 0b0100_1110;

                for (int i = 0; i < XXH_STRIPE_LEN / m128i_size; i++)
                {
                    int uint32_offset = i * 4;
                    int uint64_offset = i * 2;

                    var acc_vec = Sse2.LoadVector128(acc1 + uint64_offset);
                    var data_vec = Sse2.LoadVector128((uint*) p1 + uint32_offset);
                    var key_vec = Sse2.LoadVector128((uint*) secret9 + uint32_offset);
                    var data_key = Sse2.Xor(data_vec, key_vec);
                    var data_key_lo = Sse2.Shuffle(data_key, _MM_SHUFFLE_0_3_0_1);
                    var product = Sse2.Multiply(data_key, data_key_lo);
                    var data_swap = Sse2.Shuffle(data_vec, _MM_SHUFFLE_1_0_3_2).AsUInt64();
                    var sum = Sse2.Add(acc_vec, data_swap);
                    var result = Sse2.Add(product, sum);
                    Sse2.Store(acc1 + uint64_offset, result);
                }
            }
            else
            {
                for (int i = 0; i < XXH_ACC_NB; i++)
                {
                    ulong* xacc = acc1;
                    byte* xinput = p1;
                    byte* xsecret = secret9;

                    byte* ptr = xinput + i * 8;
                    ulong data_val = *(ulong*) ptr;
                    byte* ptr1 = xsecret + i * 8;
                    ulong data_key = data_val ^ *(ulong*) ptr1;
                    xacc[i ^ 1] += data_val;
                    ulong y = data_key >> 32;
                    xacc[i] += (ulong) (uint) (data_key & 0xFFFFFFFF) * (ulong) (uint) (y);
                }
            }

            byte* secret10 = customSecret + XXH_SECRET_MERGEACCS_START;
            ulong result65 = ((ulong) len) * XXH_PRIME64_1;

            for (int i2 = 0; i2 < 4; i2++)
            {
                ulong* acc = acc1 + 2 * i2;
                byte* secret1 = secret10 + 16 * i2;
                byte* ptr = secret1 + 8;
                ulong rhs = acc[1] ^ *(ulong*) ptr;
                ulong lhs = acc[0] ^ *(ulong*) secret1;
                uint128 ret;
                if (Bmi2.IsSupported)
                {
                    ulong product_low;
                    ulong product_high = Bmi2.X64.MultiplyNoFlags(lhs, rhs, &product_low);
                    uint128 r128;
                    r128.low64 = product_low;
                    r128.high64 = product_high;
                    ret = r128;
                }
                else
                {
                    ulong y = rhs & 0xFFFFFFFF;
                    ulong lo_lo = (ulong) (uint) (lhs & 0xFFFFFFFF) * (ulong) (uint) (y);
                    ulong y1 = rhs & 0xFFFFFFFF;
                    ulong hi_lo = (ulong) (uint) (lhs >> 32) * (ulong) (uint) (y1);
                    ulong y2 = rhs >> 32;
                    ulong lo_hi = (ulong) (uint) (lhs & 0xFFFFFFFF) * (ulong) (uint) (y2);
                    ulong y3 = rhs >> 32;
                    ulong hi_hi = (ulong) (uint) (lhs >> 32) * (ulong) (uint) (y3);

                    ulong cross = (lo_lo >> 32) + (hi_lo & 0xFFFFFFFF) + lo_hi;
                    ulong upper = (hi_lo >> 32) + (cross >> 32) + hi_hi;
                    ulong lower = (cross << 32) | (lo_lo & 0xFFFFFFFF);

                    uint128 r128;
                    r128.low64 = lower;
                    r128.high64 = upper;
                    ret = r128;
                }

                uint128 product = ret;
                result65 += product.low64 ^ product.high64;
            }

            ulong h66 = result65;
            h66 = h66 ^ (h66 >> 37);
            h66 *= 0x165667919E3779F9UL;
            h66 = h66 ^ (h66 >> 32);
            return h66;
        }
    }
}

