/*
* This is the auto generated code.
* All function calls are inlined in XXH3_128bits_internal
* Please don't try to analyze it.
*/

using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;

namespace Standart.Hash.xxHash;

public static partial class xxHash128
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static unsafe uint128 __XXH3_128bits_internal(byte* input, int len, ulong seed, byte* secret, int secretLen)
    {
        if (len <= 16)
        {
            if (len > 8)
            {
                byte* ptr = secret + 32;
                byte* ptr1 = secret + 40;
                ulong bitflipl1 = (*(ulong*) ptr ^ *(ulong*) ptr1) - seed;
                byte* ptr2 = secret + 48;
                byte* ptr3 = secret + 56;
                ulong bitfliph1 = (*(ulong*) ptr2 ^ *(ulong*) ptr3) + seed;
                ulong input_lo = *(ulong*) input;
                byte* ptr4 = input + len - 8;
                ulong input_hi = *(ulong*) ptr4;
                ulong lhs = input_lo ^ input_hi ^ bitflipl1;
                uint128 ret;
                if (Bmi2.IsSupported)
                {
                    ulong product_low;
                    ulong product_high = Bmi2.X64.MultiplyNoFlags(lhs, XXH_PRIME64_1, &product_low);
                    uint128 r128;
                    r128.low64  = product_low;
                    r128.high64 = product_high;
                    ret = r128;
                }
                else
                {
                    ulong lo_lo = (ulong)(uint)(lhs & 0xFFFFFFFF) * (ulong)(uint)(XXH_PRIME64_1 & 0xFFFFFFFF);
                    ulong hi_lo = (ulong)(uint)(lhs >> 32) * (ulong)(uint)(XXH_PRIME64_1 & 0xFFFFFFFF);
                    ulong lo_hi = (ulong)(uint)(lhs & 0xFFFFFFFF) * (ulong)(uint)(XXH_PRIME64_1 >> 32);
                    ulong hi_hi = (ulong)(uint)(lhs >> 32) * (ulong)(uint)(XXH_PRIME64_1 >> 32);
            
                    ulong cross = (lo_lo >> 32) + (hi_lo & 0xFFFFFFFF) + lo_hi;
                    ulong upper = (hi_lo >> 32) + (cross >> 32)        + hi_hi;
                    ulong lower = (cross << 32) | (lo_lo & 0xFFFFFFFF);

                    uint128 r128;
                    r128.low64  = lower;
                    r128.high64 = upper;
                    ret = r128;
                }

                uint128 m128 = ret;

                m128.low64 += (ulong) (len - 1) << 54;
                input_hi ^= bitfliph1;

                m128.high64 += input_hi + (ulong)(uint)(uint) input_hi * (ulong)(uint)(XXH_PRIME32_2 - 1);
                m128.low64 ^= ((m128.high64 << 56) & 0xff00000000000000UL) | 
                              ((m128.high64 << 40) & 0x00ff000000000000UL) | 
                              ((m128.high64 << 24) & 0x0000ff0000000000UL) | 
                              ((m128.high64 << 8)  & 0x000000ff00000000UL) | 
                              ((m128.high64 >> 8)  & 0x00000000ff000000UL) | 
                              ((m128.high64 >> 24) & 0x0000000000ff0000UL) | 
                              ((m128.high64 >> 40) & 0x000000000000ff00UL) | 
                              ((m128.high64 >> 56) & 0x00000000000000ffUL);

                uint128 ret1;
                if (Bmi2.IsSupported)
                {
                    ulong product_low;
                    ulong product_high = Bmi2.X64.MultiplyNoFlags(m128.low64, XXH_PRIME64_2, &product_low);
                    uint128 r128;
                    r128.low64  = product_low;
                    r128.high64 = product_high;
                    ret1 = r128;
                }
                else
                {
                    ulong lo_lo = (ulong)(uint)(m128.low64 & 0xFFFFFFFF) * (ulong)(uint)(XXH_PRIME64_2 & 0xFFFFFFFF);
                    ulong hi_lo = (ulong)(uint)(m128.low64 >> 32) * (ulong)(uint)(XXH_PRIME64_2 & 0xFFFFFFFF);
                    ulong lo_hi = (ulong)(uint)(m128.low64 & 0xFFFFFFFF) * (ulong)(uint)(XXH_PRIME64_2 >> 32);
                    ulong hi_hi = (ulong)(uint)(m128.low64 >> 32) * (ulong)(uint)(XXH_PRIME64_2 >> 32);
            
                    ulong cross = (lo_lo >> 32) + (hi_lo & 0xFFFFFFFF) + lo_hi;
                    ulong upper = (hi_lo >> 32) + (cross >> 32)        + hi_hi;
                    ulong lower = (cross << 32) | (lo_lo & 0xFFFFFFFF);

                    uint128 r128;
                    r128.low64  = lower;
                    r128.high64 = upper;
                    ret1 = r128;
                }

                uint128 h129 = ret1;
                h129.high64 += m128.high64 * XXH_PRIME64_2;

                ulong h64 = h129.low64;
                h64 = h64 ^ (h64 >> 37);
                h64 *= 0x165667919E3779F9UL;
                h64 = h64 ^ (h64 >> 32);
                h129.low64 = h64;
                ulong h65 = h129.high64;
                h65 = h65 ^ (h65 >> 37);
                h65 *= 0x165667919E3779F9UL;
                h65 = h65 ^ (h65 >> 32);
                h129.high64 = h65;
                return h129;
            }

            if (len >= 4)
            {
                ulong seed1 = seed;

                uint x = (uint) seed1;
                seed1 ^= (ulong) (((x << 24) & 0xff000000 ) | 
                                  ((x <<  8) & 0x00ff0000 ) | 
                                  ((x >>  8) & 0x0000ff00 ) | 
                                  ((x >> 24) & 0x000000ff )) << 32;

                uint input_lo = *(uint*) input;
                byte* ptr2 = input + len - 4;
                uint input_hi = *(uint*) ptr2;
                ulong input_64 = input_lo + ((ulong) input_hi << 32);
                byte* ptr = secret + 16;
                byte* ptr1 = secret + 24;
                ulong bitflip = (*(ulong*) ptr ^  *(ulong*) ptr1) + seed1;
                ulong keyed = input_64 ^ bitflip;

                ulong rhs = XXH_PRIME64_1 + ((ulong) len << 2);
                uint128 ret;
                if (Bmi2.IsSupported)
                {
                    ulong product_low;
                    ulong product_high = Bmi2.X64.MultiplyNoFlags(keyed, rhs, &product_low);
                    uint128 r128;
                    r128.low64  = product_low;
                    r128.high64 = product_high;
                    ret = r128;
                }
                else
                {
                    ulong y = rhs & 0xFFFFFFFF;
                    ulong lo_lo = (ulong)(uint)(keyed & 0xFFFFFFFF) * (ulong)(uint)(y);
                    ulong y1 = rhs & 0xFFFFFFFF;
                    ulong hi_lo = (ulong)(uint)(keyed >> 32) * (ulong)(uint)(y1);
                    ulong y2 = rhs >> 32;
                    ulong lo_hi = (ulong)(uint)(keyed & 0xFFFFFFFF) * (ulong)(uint)(y2);
                    ulong y3 = rhs >> 32;
                    ulong hi_hi = (ulong)(uint)(keyed >> 32) * (ulong)(uint)(y3);
            
                    ulong cross = (lo_lo >> 32) + (hi_lo & 0xFFFFFFFF) + lo_hi;
                    ulong upper = (hi_lo >> 32) + (cross >> 32)        + hi_hi;
                    ulong lower = (cross << 32) | (lo_lo & 0xFFFFFFFF);

                    uint128 r128;
                    r128.low64  = lower;
                    r128.high64 = upper;
                    ret = r128;
                }

                uint128 m128 = ret;

                m128.high64 += (m128.low64 << 1);
                m128.low64 ^= (m128.high64 >> 3);

                m128.low64 =  m128.low64 ^ (m128.low64 >> 35);
                m128.low64 *= 0x9FB21C651E98DF25UL;
                m128.low64 =  m128.low64 ^ (m128.low64 >> 28);
                ulong h64 = m128.high64;
                h64 = h64 ^ (h64 >> 37);
                h64 *= 0x165667919E3779F9UL;
                h64 = h64 ^ (h64 >> 32);
                m128.high64 = h64;

                return m128;
            }

            if (len != 0)
            {
                byte c1 = input[0];
                byte c2 = input[len >> 1];
                byte c3 = input[len - 1];

                uint combinedl = ((uint) c1 << 16) |
                                 ((uint) c2 << 24) |
                                 ((uint) c3 << 0) |
                                 ((uint) len << 8);
                uint x = ((combinedl << 24) & 0xff000000 ) | 
                         ((combinedl <<  8) & 0x00ff0000 ) | 
                         ((combinedl >>  8) & 0x0000ff00 ) | 
                         ((combinedl >> 24) & 0x000000ff );
                uint combinedh = (x << 13) | (x >> (32 - 13));

                byte* ptr = secret + 4;
                ulong bitflipl1 = (*(uint*) secret ^ *(uint*) ptr) + seed;
                byte* ptr1 = secret + 8;
                byte* ptr2 = secret + 12;
                ulong bitfliph1 = (*(uint*) ptr1 ^ *(uint*) ptr2) - seed;
                ulong keyed_lo = (ulong) combinedl ^ bitflipl1;
                ulong keyed_hi = (ulong) combinedh ^ bitfliph1;

                uint128 h129;
                ulong hash = keyed_lo;
                hash ^= hash >> 33;
                hash *= XXH_PRIME64_2;
                hash ^= hash >> 29;
                hash *= XXH_PRIME64_3;
                hash ^= hash >> 32;
                h129.low64 = hash;
                ulong hash1 = keyed_hi;
                hash1 ^= hash1 >> 33;
                hash1 *= XXH_PRIME64_2;
                hash1 ^= hash1 >> 29;
                hash1 *= XXH_PRIME64_3;
                hash1 ^= hash1 >> 32;
                h129.high64 = hash1;

                return h129;
            }

            uint128 h128;
            byte* ptr5 = secret + 64;
            byte* ptr6 = secret + 72;
            ulong bitflipl = *(ulong*) ptr5 ^ *(ulong*) ptr6;
            byte* ptr7 = secret + 80;
            byte* ptr8 = secret + 88;
            ulong bitfliph = *(ulong*) ptr7 ^ *(ulong*) ptr8;
            ulong hash2 = seed ^ bitflipl;
            hash2 ^= hash2 >> 33;
            hash2 *= XXH_PRIME64_2;
            hash2 ^= hash2 >> 29;
            hash2 *= XXH_PRIME64_3;
            hash2 ^= hash2 >> 32;
            h128.low64 = hash2;
            ulong hash3 = seed ^ bitfliph;
            hash3 ^= hash3 >> 33;
            hash3 *= XXH_PRIME64_2;
            hash3 ^= hash3 >> 29;
            hash3 *= XXH_PRIME64_3;
            hash3 ^= hash3 >> 32;
            h128.high64 = hash3;
            return h128;
        }

        if (len <= 128)
        {
            uint128 acc;
            acc.low64 = (ulong) len * XXH_PRIME64_1;
            acc.high64 = 0;
            
            if (len > 32) {
                if (len > 64) {
                    if (len > 96)
                    {
                        uint128 acc1 = acc;
                        byte* input1 = input+48;
                        byte* input2 = input+len-64;
                        byte* secret1 = secret+96;
                        byte* secret4 = secret1 + 0;
                        ulong input_lo = *(ulong*) input1;
                        byte* ptr8 = input1 + 8;
                        ulong input_hi = *(ulong*) ptr8;

                        byte* ptr9 = secret4 + 8;
                        ulong lhs = input_lo ^ (*(ulong*) secret4 + seed);
                        ulong rhs = input_hi ^ (*(ulong*) ptr9 - seed);
                        uint128 ret;
                        if (Bmi2.IsSupported)
                        {
                            ulong product_low;
                            ulong product_high = Bmi2.X64.MultiplyNoFlags(lhs, rhs, &product_low);
                            uint128 r128;
                            r128.low64  = product_low;
                            r128.high64 = product_high;
                            ret = r128;
                        }
                        else
                        {
                            ulong y = rhs & 0xFFFFFFFF;
                            ulong lo_lo = (ulong)(uint)(lhs & 0xFFFFFFFF) * (ulong)(uint)(y);
                            ulong y1 = rhs & 0xFFFFFFFF;
                            ulong hi_lo = (ulong)(uint)(lhs >> 32) * (ulong)(uint)(y1);
                            ulong y2 = rhs >> 32;
                            ulong lo_hi = (ulong)(uint)(lhs & 0xFFFFFFFF) * (ulong)(uint)(y2);
                            ulong y3 = rhs >> 32;
                            ulong hi_hi = (ulong)(uint)(lhs >> 32) * (ulong)(uint)(y3);
            
                            ulong cross = (lo_lo >> 32) + (hi_lo & 0xFFFFFFFF) + lo_hi;
                            ulong upper = (hi_lo >> 32) + (cross >> 32)        + hi_hi;
                            ulong lower = (cross << 32) | (lo_lo & 0xFFFFFFFF);

                            uint128 r128;
                            r128.low64  = lower;
                            r128.high64 = upper;
                            ret = r128;
                        }

                        uint128 product = ret;
                        acc1.low64 += product.low64 ^ product.high64;
                        byte* ptr = input2 + 8;
                        acc1.low64 ^= *(ulong*) input2 + *(ulong*) ptr;
                        byte* secret5 = secret1 + 16;
                        ulong inputLo = *(ulong*) input2;
                        byte* ptr10 = input2 + 8;
                        ulong inputHi = *(ulong*) ptr10;

                        byte* ptr11 = secret5 + 8;
                        ulong lhs1 = inputLo ^ (*(ulong*) secret5 + seed);
                        ulong rhs1 = inputHi ^ (*(ulong*) ptr11 - seed);
                        uint128 ret1;
                        if (Bmi2.IsSupported)
                        {
                            ulong product_low;
                            ulong product_high = Bmi2.X64.MultiplyNoFlags(lhs1, rhs1, &product_low);
                            uint128 r128;
                            r128.low64  = product_low;
                            r128.high64 = product_high;
                            ret1 = r128;
                        }
                        else
                        {
                            ulong y4 = rhs1 & 0xFFFFFFFF;
                            ulong loLo = (ulong)(uint)(lhs1 & 0xFFFFFFFF) * (ulong)(uint)(y4);
                            ulong y5 = rhs1 & 0xFFFFFFFF;
                            ulong hiLo = (ulong)(uint)(lhs1 >> 32) * (ulong)(uint)(y5);
                            ulong y6 = rhs1 >> 32;
                            ulong loHi = (ulong)(uint)(lhs1 & 0xFFFFFFFF) * (ulong)(uint)(y6);
                            ulong y7 = rhs1 >> 32;
                            ulong hiHi = (ulong)(uint)(lhs1 >> 32) * (ulong)(uint)(y7);
            
                            ulong cross1 = (loLo >> 32) + (hiLo & 0xFFFFFFFF) + loHi;
                            ulong upper1 = (hiLo >> 32) + (cross1 >> 32)        + hiHi;
                            ulong lower1 = (cross1 << 32) | (loLo & 0xFFFFFFFF);

                            uint128 r129;
                            r129.low64  = lower1;
                            r129.high64 = upper1;
                            ret1 = r129;
                        }

                        uint128 product1 = ret1;
                        acc1.high64 += product1.low64 ^ product1.high64;
                        byte* ptr1 = input1 + 8;
                        acc1.high64 ^= *(ulong*) input1 + *(ulong*) ptr1;
                        acc = acc1;
                    }

                    uint128 acc2 = acc;
                    byte* input3 = input+32;
                    byte* input4 = input+len-48;
                    byte* secret2 = secret+64;
                    byte* secret6 = secret2 + 0;
                    ulong inputLo1 = *(ulong*) input3;
                    byte* ptr12 = input3 + 8;
                    ulong inputHi1 = *(ulong*) ptr12;

                    byte* ptr13 = secret6 + 8;
                    ulong lhs2 = inputLo1 ^ (*(ulong*) secret6 + seed);
                    ulong rhs2 = inputHi1 ^ (*(ulong*) ptr13 - seed);
                    uint128 ret2;
                    if (Bmi2.IsSupported)
                    {
                        ulong product_low;
                        ulong product_high = Bmi2.X64.MultiplyNoFlags(lhs2, rhs2, &product_low);
                        uint128 r128;
                        r128.low64  = product_low;
                        r128.high64 = product_high;
                        ret2 = r128;
                    }
                    else
                    {
                        ulong y8 = rhs2 & 0xFFFFFFFF;
                        ulong loLo1 = (ulong)(uint)(lhs2 & 0xFFFFFFFF) * (ulong)(uint)(y8);
                        ulong y9 = rhs2 & 0xFFFFFFFF;
                        ulong hiLo1 = (ulong)(uint)(lhs2 >> 32) * (ulong)(uint)(y9);
                        ulong y10 = rhs2 >> 32;
                        ulong loHi1 = (ulong)(uint)(lhs2 & 0xFFFFFFFF) * (ulong)(uint)(y10);
                        ulong y11 = rhs2 >> 32;
                        ulong hiHi1 = (ulong)(uint)(lhs2 >> 32) * (ulong)(uint)(y11);
            
                        ulong cross2 = (loLo1 >> 32) + (hiLo1 & 0xFFFFFFFF) + loHi1;
                        ulong upper2 = (hiLo1 >> 32) + (cross2 >> 32)        + hiHi1;
                        ulong lower2 = (cross2 << 32) | (loLo1 & 0xFFFFFFFF);

                        uint128 r1210;
                        r1210.low64  = lower2;
                        r1210.high64 = upper2;
                        ret2 = r1210;
                    }

                    uint128 product2 = ret2;
                    acc2.low64 += product2.low64 ^ product2.high64;
                    byte* ptr2 = input4 + 8;
                    acc2.low64 ^= *(ulong*) input4 + *(ulong*) ptr2;
                    byte* secret7 = secret2 + 16;
                    ulong inputLo2 = *(ulong*) input4;
                    byte* ptr14 = input4 + 8;
                    ulong inputHi2 = *(ulong*) ptr14;

                    byte* ptr15 = secret7 + 8;
                    ulong lhs3 = inputLo2 ^ (*(ulong*) secret7 + seed);
                    ulong rhs3 = inputHi2 ^ (*(ulong*) ptr15 - seed);
                    uint128 ret3;
                    if (Bmi2.IsSupported)
                    {
                        ulong product_low;
                        ulong product_high = Bmi2.X64.MultiplyNoFlags(lhs3, rhs3, &product_low);
                        uint128 r128;
                        r128.low64  = product_low;
                        r128.high64 = product_high;
                        ret3 = r128;
                    }
                    else
                    {
                        ulong y12 = rhs3 & 0xFFFFFFFF;
                        ulong loLo2 = (ulong)(uint)(lhs3 & 0xFFFFFFFF) * (ulong)(uint)(y12);
                        ulong y13 = rhs3 & 0xFFFFFFFF;
                        ulong hiLo2 = (ulong)(uint)(lhs3 >> 32) * (ulong)(uint)(y13);
                        ulong y14 = rhs3 >> 32;
                        ulong loHi2 = (ulong)(uint)(lhs3 & 0xFFFFFFFF) * (ulong)(uint)(y14);
                        ulong y15 = rhs3 >> 32;
                        ulong hiHi2 = (ulong)(uint)(lhs3 >> 32) * (ulong)(uint)(y15);
            
                        ulong cross3 = (loLo2 >> 32) + (hiLo2 & 0xFFFFFFFF) + loHi2;
                        ulong upper3 = (hiLo2 >> 32) + (cross3 >> 32)        + hiHi2;
                        ulong lower3 = (cross3 << 32) | (loLo2 & 0xFFFFFFFF);

                        uint128 r1211;
                        r1211.low64  = lower3;
                        r1211.high64 = upper3;
                        ret3 = r1211;
                    }

                    uint128 product3 = ret3;
                    acc2.high64 += product3.low64 ^ product3.high64;
                    byte* ptr3 = input3 + 8;
                    acc2.high64 ^= *(ulong*) input3 + *(ulong*) ptr3;
                    acc = acc2;
                }

                uint128 acc3 = acc;
                byte* input5 = input+16;
                byte* input6 = input+len-32;
                byte* secret3 = secret+32;
                byte* secret8 = secret3 + 0;
                ulong inputLo3 = *(ulong*) input5;
                byte* ptr16 = input5 + 8;
                ulong inputHi3 = *(ulong*) ptr16;

                byte* ptr17 = secret8 + 8;
                ulong lhs4 = inputLo3 ^ (*(ulong*) secret8 + seed);
                ulong rhs4 = inputHi3 ^ (*(ulong*) ptr17 - seed);
                uint128 ret4;
                if (Bmi2.IsSupported)
                {
                    ulong product_low;
                    ulong product_high = Bmi2.X64.MultiplyNoFlags(lhs4, rhs4, &product_low);
                    uint128 r128;
                    r128.low64  = product_low;
                    r128.high64 = product_high;
                    ret4 = r128;
                }
                else
                {
                    ulong y16 = rhs4 & 0xFFFFFFFF;
                    ulong loLo3 = (ulong)(uint)(lhs4 & 0xFFFFFFFF) * (ulong)(uint)(y16);
                    ulong y17 = rhs4 & 0xFFFFFFFF;
                    ulong hiLo3 = (ulong)(uint)(lhs4 >> 32) * (ulong)(uint)(y17);
                    ulong y18 = rhs4 >> 32;
                    ulong loHi3 = (ulong)(uint)(lhs4 & 0xFFFFFFFF) * (ulong)(uint)(y18);
                    ulong y19 = rhs4 >> 32;
                    ulong hiHi3 = (ulong)(uint)(lhs4 >> 32) * (ulong)(uint)(y19);
            
                    ulong cross4 = (loLo3 >> 32) + (hiLo3 & 0xFFFFFFFF) + loHi3;
                    ulong upper4 = (hiLo3 >> 32) + (cross4 >> 32)        + hiHi3;
                    ulong lower4 = (cross4 << 32) | (loLo3 & 0xFFFFFFFF);

                    uint128 r1212;
                    r1212.low64  = lower4;
                    r1212.high64 = upper4;
                    ret4 = r1212;
                }

                uint128 product4 = ret4;
                acc3.low64 += product4.low64 ^ product4.high64;
                byte* ptr4 = input6 + 8;
                acc3.low64 ^= *(ulong*) input6 + *(ulong*) ptr4;
                byte* secret9 = secret3 + 16;
                ulong inputLo4 = *(ulong*) input6;
                byte* ptr18 = input6 + 8;
                ulong inputHi4 = *(ulong*) ptr18;

                byte* ptr19 = secret9 + 8;
                ulong lhs5 = inputLo4 ^ (*(ulong*) secret9 + seed);
                ulong rhs5 = inputHi4 ^ (*(ulong*) ptr19 - seed);
                uint128 ret5;
                if (Bmi2.IsSupported)
                {
                    ulong product_low;
                    ulong product_high = Bmi2.X64.MultiplyNoFlags(lhs5, rhs5, &product_low);
                    uint128 r128;
                    r128.low64  = product_low;
                    r128.high64 = product_high;
                    ret5 = r128;
                }
                else
                {
                    ulong y20 = rhs5 & 0xFFFFFFFF;
                    ulong loLo4 = (ulong)(uint)(lhs5 & 0xFFFFFFFF) * (ulong)(uint)(y20);
                    ulong y21 = rhs5 & 0xFFFFFFFF;
                    ulong hiLo4 = (ulong)(uint)(lhs5 >> 32) * (ulong)(uint)(y21);
                    ulong y22 = rhs5 >> 32;
                    ulong loHi4 = (ulong)(uint)(lhs5 & 0xFFFFFFFF) * (ulong)(uint)(y22);
                    ulong y23 = rhs5 >> 32;
                    ulong hiHi4 = (ulong)(uint)(lhs5 >> 32) * (ulong)(uint)(y23);
            
                    ulong cross5 = (loLo4 >> 32) + (hiLo4 & 0xFFFFFFFF) + loHi4;
                    ulong upper5 = (hiLo4 >> 32) + (cross5 >> 32)        + hiHi4;
                    ulong lower5 = (cross5 << 32) | (loLo4 & 0xFFFFFFFF);

                    uint128 r1213;
                    r1213.low64  = lower5;
                    r1213.high64 = upper5;
                    ret5 = r1213;
                }

                uint128 product5 = ret5;
                acc3.high64 += product5.low64 ^ product5.high64;
                byte* ptr5 = input5 + 8;
                acc3.high64 ^= *(ulong*) input5 + *(ulong*) ptr5;
                acc = acc3;
            }

            uint128 acc4 = acc;
            byte* input7 = input+len-16;
            byte* secret10 = secret + 0;
            ulong inputLo5 = *(ulong*) input;
            byte* ptr20 = input + 8;
            ulong inputHi5 = *(ulong*) ptr20;

            byte* ptr21 = secret10 + 8;
            ulong lhs6 = inputLo5 ^ (*(ulong*) secret10 + seed);
            ulong rhs6 = inputHi5 ^ (*(ulong*) ptr21 - seed);
            uint128 ret6;
            if (Bmi2.IsSupported)
            {
                ulong product_low;
                ulong product_high = Bmi2.X64.MultiplyNoFlags(lhs6, rhs6, &product_low);
                uint128 r128;
                r128.low64  = product_low;
                r128.high64 = product_high;
                ret6 = r128;
            }
            else
            {
                ulong y24 = rhs6 & 0xFFFFFFFF;
                ulong loLo5 = (ulong)(uint)(lhs6 & 0xFFFFFFFF) * (ulong)(uint)(y24);
                ulong y25 = rhs6 & 0xFFFFFFFF;
                ulong hiLo5 = (ulong)(uint)(lhs6 >> 32) * (ulong)(uint)(y25);
                ulong y26 = rhs6 >> 32;
                ulong loHi5 = (ulong)(uint)(lhs6 & 0xFFFFFFFF) * (ulong)(uint)(y26);
                ulong y27 = rhs6 >> 32;
                ulong hiHi5 = (ulong)(uint)(lhs6 >> 32) * (ulong)(uint)(y27);
            
                ulong cross6 = (loLo5 >> 32) + (hiLo5 & 0xFFFFFFFF) + loHi5;
                ulong upper6 = (hiLo5 >> 32) + (cross6 >> 32)        + hiHi5;
                ulong lower6 = (cross6 << 32) | (loLo5 & 0xFFFFFFFF);

                uint128 r1214;
                r1214.low64  = lower6;
                r1214.high64 = upper6;
                ret6 = r1214;
            }

            uint128 product6 = ret6;
            acc4.low64 += product6.low64 ^ product6.high64;
            byte* ptr6 = input7 + 8;
            acc4.low64 ^= *(ulong*) input7 + *(ulong*) ptr6;
            byte* secret11 = secret + 16;
            ulong inputLo6 = *(ulong*) input7;
            byte* ptr22 = input7 + 8;
            ulong inputHi6 = *(ulong*) ptr22;

            byte* ptr23 = secret11 + 8;
            ulong lhs7 = inputLo6 ^ (*(ulong*) secret11 + seed);
            ulong rhs7 = inputHi6 ^ (*(ulong*) ptr23 - seed);
            uint128 ret7;
            if (Bmi2.IsSupported)
            {
                ulong product_low;
                ulong product_high = Bmi2.X64.MultiplyNoFlags(lhs7, rhs7, &product_low);
                uint128 r128;
                r128.low64  = product_low;
                r128.high64 = product_high;
                ret7 = r128;
            }
            else
            {
                ulong y28 = rhs7 & 0xFFFFFFFF;
                ulong loLo6 = (ulong)(uint)(lhs7 & 0xFFFFFFFF) * (ulong)(uint)(y28);
                ulong y29 = rhs7 & 0xFFFFFFFF;
                ulong hiLo6 = (ulong)(uint)(lhs7 >> 32) * (ulong)(uint)(y29);
                ulong y30 = rhs7 >> 32;
                ulong loHi6 = (ulong)(uint)(lhs7 & 0xFFFFFFFF) * (ulong)(uint)(y30);
                ulong y31 = rhs7 >> 32;
                ulong hiHi6 = (ulong)(uint)(lhs7 >> 32) * (ulong)(uint)(y31);
            
                ulong cross7 = (loLo6 >> 32) + (hiLo6 & 0xFFFFFFFF) + loHi6;
                ulong upper7 = (hiLo6 >> 32) + (cross7 >> 32)        + hiHi6;
                ulong lower7 = (cross7 << 32) | (loLo6 & 0xFFFFFFFF);

                uint128 r1215;
                r1215.low64  = lower7;
                r1215.high64 = upper7;
                ret7 = r1215;
            }

            uint128 product7 = ret7;
            acc4.high64 += product7.low64 ^ product7.high64;
            byte* ptr7 = input + 8;
            acc4.high64 ^= *(ulong*) input + *(ulong*) ptr7;
            acc = acc4;

            uint128 h128;
            h128.low64 = acc.low64 + acc.high64;
            h128.high64 = (acc.low64  * XXH_PRIME64_1)
                          + (acc.high64   * XXH_PRIME64_4)
                          + (((ulong) len - seed) * XXH_PRIME64_2);
            ulong h64 = h128.low64;
            h64 = h64 ^ (h64 >> 37);
            h64 *= 0x165667919E3779F9UL;
            h64 = h64 ^ (h64 >> 32);
            h128.low64 = h64;
            ulong h65 = h128.high64;
            h65 = h65 ^ (h65 >> 37);
            h65 *= 0x165667919E3779F9UL;
            h65 = h65 ^ (h65 >> 32);
            h128.high64 = (ulong) 0 - h65;
            return h128;
        }

        if (len <= XXH3_MIDSIZE_MAX)
        {
            uint128 acc;
            int nbRounds = len / 32;
            
            acc.low64 = (ulong) len * XXH_PRIME64_1;
            acc.high64 = 0;
            for (int i = 0; i < 4; i++)
            {
                uint128 acc1 = acc;
                byte* input1 = input  + (32 * i);
                byte* input2 = input  + (32 * i) + 16;
                byte* secret1 = secret + (32 * i);
                byte* secret3 = secret1 + 0;
                ulong input_lo = *(ulong*) input1;
                byte* ptr4 = input1 + 8;
                ulong input_hi = *(ulong*) ptr4;

                byte* ptr5 = secret3 + 8;
                ulong lhs = input_lo ^ (*(ulong*) secret3 + seed);
                ulong rhs = input_hi ^ (*(ulong*) ptr5 - seed);
                uint128 ret;
                if (Bmi2.IsSupported)
                {
                    ulong product_low;
                    ulong product_high = Bmi2.X64.MultiplyNoFlags(lhs, rhs, &product_low);
                    uint128 r128;
                    r128.low64  = product_low;
                    r128.high64 = product_high;
                    ret = r128;
                }
                else
                {
                    ulong y = rhs & 0xFFFFFFFF;
                    ulong lo_lo = (ulong)(uint)(lhs & 0xFFFFFFFF) * (ulong)(uint)(y);
                    ulong y1 = rhs & 0xFFFFFFFF;
                    ulong hi_lo = (ulong)(uint)(lhs >> 32) * (ulong)(uint)(y1);
                    ulong y2 = rhs >> 32;
                    ulong lo_hi = (ulong)(uint)(lhs & 0xFFFFFFFF) * (ulong)(uint)(y2);
                    ulong y3 = rhs >> 32;
                    ulong hi_hi = (ulong)(uint)(lhs >> 32) * (ulong)(uint)(y3);
            
                    ulong cross = (lo_lo >> 32) + (hi_lo & 0xFFFFFFFF) + lo_hi;
                    ulong upper = (hi_lo >> 32) + (cross >> 32)        + hi_hi;
                    ulong lower = (cross << 32) | (lo_lo & 0xFFFFFFFF);

                    uint128 r128;
                    r128.low64  = lower;
                    r128.high64 = upper;
                    ret = r128;
                }

                uint128 product = ret;
                acc1.low64 += product.low64 ^ product.high64;
                byte* ptr = input2 + 8;
                acc1.low64 ^= *(ulong*) input2 + *(ulong*) ptr;
                byte* secret4 = secret1 + 16;
                ulong inputLo = *(ulong*) input2;
                byte* ptr6 = input2 + 8;
                ulong inputHi = *(ulong*) ptr6;

                byte* ptr7 = secret4 + 8;
                ulong lhs1 = inputLo ^ (*(ulong*) secret4 + seed);
                ulong rhs1 = inputHi ^ (*(ulong*) ptr7 - seed);
                uint128 ret1;
                if (Bmi2.IsSupported)
                {
                    ulong product_low;
                    ulong product_high = Bmi2.X64.MultiplyNoFlags(lhs1, rhs1, &product_low);
                    uint128 r128;
                    r128.low64  = product_low;
                    r128.high64 = product_high;
                    ret1 = r128;
                }
                else
                {
                    ulong y4 = rhs1 & 0xFFFFFFFF;
                    ulong loLo = (ulong)(uint)(lhs1 & 0xFFFFFFFF) * (ulong)(uint)(y4);
                    ulong y5 = rhs1 & 0xFFFFFFFF;
                    ulong hiLo = (ulong)(uint)(lhs1 >> 32) * (ulong)(uint)(y5);
                    ulong y6 = rhs1 >> 32;
                    ulong loHi = (ulong)(uint)(lhs1 & 0xFFFFFFFF) * (ulong)(uint)(y6);
                    ulong y7 = rhs1 >> 32;
                    ulong hiHi = (ulong)(uint)(lhs1 >> 32) * (ulong)(uint)(y7);
            
                    ulong cross1 = (loLo >> 32) + (hiLo & 0xFFFFFFFF) + loHi;
                    ulong upper1 = (hiLo >> 32) + (cross1 >> 32)        + hiHi;
                    ulong lower1 = (cross1 << 32) | (loLo & 0xFFFFFFFF);

                    uint128 r129;
                    r129.low64  = lower1;
                    r129.high64 = upper1;
                    ret1 = r129;
                }

                uint128 product1 = ret1;
                acc1.high64 += product1.low64 ^ product1.high64;
                byte* ptr1 = input1 + 8;
                acc1.high64 ^= *(ulong*) input1 + *(ulong*) ptr1;
                acc = acc1;
            }

            ulong h64 = acc.low64;
            h64 = h64 ^ (h64 >> 37);
            h64 *= 0x165667919E3779F9UL;
            h64 = h64 ^ (h64 >> 32);
            acc.low64 = h64;
            ulong h65 = acc.high64;
            h65 = h65 ^ (h65 >> 37);
            h65 *= 0x165667919E3779F9UL;
            h65 = h65 ^ (h65 >> 32);
            acc.high64 = h65;

            for (int i = 4 ; i < nbRounds; i++)
            {
                uint128 acc1 = acc;
                byte* input1 = input + (32 * i);
                byte* input2 = input + (32 * i) + 16;
                byte* secret1 = secret + XXH3_MIDSIZE_STARTOFFSET + (32 * (i - 4));
                byte* secret3 = secret1 + 0;
                ulong input_lo = *(ulong*) input1;
                byte* ptr4 = input1 + 8;
                ulong input_hi = *(ulong*) ptr4;

                byte* ptr5 = secret3 + 8;
                ulong lhs = input_lo ^ (*(ulong*) secret3 + seed);
                ulong rhs = input_hi ^ (*(ulong*) ptr5 - seed);
                uint128 ret;
                if (Bmi2.IsSupported)
                {
                    ulong product_low;
                    ulong product_high = Bmi2.X64.MultiplyNoFlags(lhs, rhs, &product_low);
                    uint128 r128;
                    r128.low64  = product_low;
                    r128.high64 = product_high;
                    ret = r128;
                }
                else
                {
                    ulong y = rhs & 0xFFFFFFFF;
                    ulong lo_lo = (ulong)(uint)(lhs & 0xFFFFFFFF) * (ulong)(uint)(y);
                    ulong y1 = rhs & 0xFFFFFFFF;
                    ulong hi_lo = (ulong)(uint)(lhs >> 32) * (ulong)(uint)(y1);
                    ulong y2 = rhs >> 32;
                    ulong lo_hi = (ulong)(uint)(lhs & 0xFFFFFFFF) * (ulong)(uint)(y2);
                    ulong y3 = rhs >> 32;
                    ulong hi_hi = (ulong)(uint)(lhs >> 32) * (ulong)(uint)(y3);
            
                    ulong cross = (lo_lo >> 32) + (hi_lo & 0xFFFFFFFF) + lo_hi;
                    ulong upper = (hi_lo >> 32) + (cross >> 32)        + hi_hi;
                    ulong lower = (cross << 32) | (lo_lo & 0xFFFFFFFF);

                    uint128 r128;
                    r128.low64  = lower;
                    r128.high64 = upper;
                    ret = r128;
                }

                uint128 product = ret;
                acc1.low64 += product.low64 ^ product.high64;
                byte* ptr = input2 + 8;
                acc1.low64 ^= *(ulong*) input2 + *(ulong*) ptr;
                byte* secret4 = secret1 + 16;
                ulong inputLo = *(ulong*) input2;
                byte* ptr6 = input2 + 8;
                ulong inputHi = *(ulong*) ptr6;

                byte* ptr7 = secret4 + 8;
                ulong lhs1 = inputLo ^ (*(ulong*) secret4 + seed);
                ulong rhs1 = inputHi ^ (*(ulong*) ptr7 - seed);
                uint128 ret1;
                if (Bmi2.IsSupported)
                {
                    ulong product_low;
                    ulong product_high = Bmi2.X64.MultiplyNoFlags(lhs1, rhs1, &product_low);
                    uint128 r128;
                    r128.low64  = product_low;
                    r128.high64 = product_high;
                    ret1 = r128;
                }
                else
                {
                    ulong y4 = rhs1 & 0xFFFFFFFF;
                    ulong loLo = (ulong)(uint)(lhs1 & 0xFFFFFFFF) * (ulong)(uint)(y4);
                    ulong y5 = rhs1 & 0xFFFFFFFF;
                    ulong hiLo = (ulong)(uint)(lhs1 >> 32) * (ulong)(uint)(y5);
                    ulong y6 = rhs1 >> 32;
                    ulong loHi = (ulong)(uint)(lhs1 & 0xFFFFFFFF) * (ulong)(uint)(y6);
                    ulong y7 = rhs1 >> 32;
                    ulong hiHi = (ulong)(uint)(lhs1 >> 32) * (ulong)(uint)(y7);
            
                    ulong cross1 = (loLo >> 32) + (hiLo & 0xFFFFFFFF) + loHi;
                    ulong upper1 = (hiLo >> 32) + (cross1 >> 32)        + hiHi;
                    ulong lower1 = (cross1 << 32) | (loLo & 0xFFFFFFFF);

                    uint128 r129;
                    r129.low64  = lower1;
                    r129.high64 = upper1;
                    ret1 = r129;
                }

                uint128 product1 = ret1;
                acc1.high64 += product1.low64 ^ product1.high64;
                byte* ptr1 = input1 + 8;
                acc1.high64 ^= *(ulong*) input1 + *(ulong*) ptr1;
                acc = acc1;
            }

            uint128 acc2 = acc;
            byte* input3 = input + len - 16;
            byte* input4 = input + len - 32;
            byte* secret2 = secret + XXH3_SECRET_SIZE_MIN - XXH3_MIDSIZE_LASTOFFSET - 16;
            ulong seed1 = 0UL - seed;
            byte* secret5 = secret2 + 0;
            ulong inputLo1 = *(ulong*) input3;
            byte* ptr8 = input3 + 8;
            ulong inputHi1 = *(ulong*) ptr8;

            byte* ptr9 = secret5 + 8;
            ulong lhs2 = inputLo1 ^ (*(ulong*) secret5 + seed1);
            ulong rhs2 = inputHi1 ^ (*(ulong*) ptr9 - seed1);
            uint128 ret2;
            if (Bmi2.IsSupported)
            {
                ulong product_low;
                ulong product_high = Bmi2.X64.MultiplyNoFlags(lhs2, rhs2, &product_low);
                uint128 r128;
                r128.low64  = product_low;
                r128.high64 = product_high;
                ret2 = r128;
            }
            else
            {
                ulong y8 = rhs2 & 0xFFFFFFFF;
                ulong loLo1 = (ulong)(uint)(lhs2 & 0xFFFFFFFF) * (ulong)(uint)(y8);
                ulong y9 = rhs2 & 0xFFFFFFFF;
                ulong hiLo1 = (ulong)(uint)(lhs2 >> 32) * (ulong)(uint)(y9);
                ulong y10 = rhs2 >> 32;
                ulong loHi1 = (ulong)(uint)(lhs2 & 0xFFFFFFFF) * (ulong)(uint)(y10);
                ulong y11 = rhs2 >> 32;
                ulong hiHi1 = (ulong)(uint)(lhs2 >> 32) * (ulong)(uint)(y11);
            
                ulong cross2 = (loLo1 >> 32) + (hiLo1 & 0xFFFFFFFF) + loHi1;
                ulong upper2 = (hiLo1 >> 32) + (cross2 >> 32)        + hiHi1;
                ulong lower2 = (cross2 << 32) | (loLo1 & 0xFFFFFFFF);

                uint128 r1210;
                r1210.low64  = lower2;
                r1210.high64 = upper2;
                ret2 = r1210;
            }

            uint128 product2 = ret2;
            acc2.low64 += product2.low64 ^ product2.high64;
            byte* ptr2 = input4 + 8;
            acc2.low64 ^= *(ulong*) input4 + *(ulong*) ptr2;
            byte* secret6 = secret2 + 16;
            ulong inputLo2 = *(ulong*) input4;
            byte* ptr10 = input4 + 8;
            ulong inputHi2 = *(ulong*) ptr10;

            byte* ptr11 = secret6 + 8;
            ulong lhs3 = inputLo2 ^ (*(ulong*) secret6 + seed1);
            ulong rhs3 = inputHi2 ^ (*(ulong*) ptr11 - seed1);
            uint128 ret3;
            if (Bmi2.IsSupported)
            {
                ulong product_low;
                ulong product_high = Bmi2.X64.MultiplyNoFlags(lhs3, rhs3, &product_low);
                uint128 r128;
                r128.low64  = product_low;
                r128.high64 = product_high;
                ret3 = r128;
            }
            else
            {
                ulong y12 = rhs3 & 0xFFFFFFFF;
                ulong loLo2 = (ulong)(uint)(lhs3 & 0xFFFFFFFF) * (ulong)(uint)(y12);
                ulong y13 = rhs3 & 0xFFFFFFFF;
                ulong hiLo2 = (ulong)(uint)(lhs3 >> 32) * (ulong)(uint)(y13);
                ulong y14 = rhs3 >> 32;
                ulong loHi2 = (ulong)(uint)(lhs3 & 0xFFFFFFFF) * (ulong)(uint)(y14);
                ulong y15 = rhs3 >> 32;
                ulong hiHi2 = (ulong)(uint)(lhs3 >> 32) * (ulong)(uint)(y15);
            
                ulong cross3 = (loLo2 >> 32) + (hiLo2 & 0xFFFFFFFF) + loHi2;
                ulong upper3 = (hiLo2 >> 32) + (cross3 >> 32)        + hiHi2;
                ulong lower3 = (cross3 << 32) | (loLo2 & 0xFFFFFFFF);

                uint128 r1211;
                r1211.low64  = lower3;
                r1211.high64 = upper3;
                ret3 = r1211;
            }

            uint128 product3 = ret3;
            acc2.high64 += product3.low64 ^ product3.high64;
            byte* ptr3 = input3 + 8;
            acc2.high64 ^= *(ulong*) input3 + *(ulong*) ptr3;
            acc = acc2;

            uint128 h128;
            h128.low64  = acc.low64 + acc.high64;
            h128.high64 = (acc.low64    * XXH_PRIME64_1)
                          + (acc.high64   * XXH_PRIME64_4)
                          + (((ulong)len - seed) * XXH_PRIME64_2);
            ulong h66 = h128.low64;
            h66 = h66 ^ (h66 >> 37);
            h66 *= 0x165667919E3779F9UL;
            h66 = h66 ^ (h66 >> 32);
            h128.low64 = h66;
            ulong h67 = h128.high64;
            h67 = h67 ^ (h67 >> 37);
            h67 *= 0x165667919E3779F9UL;
            h67 = h67 ^ (h67 >> 32);
            h128.high64 = (ulong)0 - h67;
            return h128;
        }

        if (seed == 0)
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

            for (int n = 0; n < nb_blocks; n++) {
                byte* input1 = input + n * block_len;
                for (int n1 = 0; n1 < nbStripesPerBlock; n1++ ) {
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

                            var acc_vec     = Avx2.LoadVector256(acc + uint64_offset);
                            var data_vec    = Avx2.LoadVector256((uint*)inp + uint32_offset);
                            var key_vec     = Avx2.LoadVector256((uint*)secret1 + uint32_offset);
                            var data_key    = Avx2.Xor(data_vec, key_vec);
                            var data_key_lo = Avx2.Shuffle(data_key, _MM_SHUFFLE_0_3_0_1);
                            var product     = Avx2.Multiply(data_key, data_key_lo);
                            var data_swap   = Avx2.Shuffle(data_vec, _MM_SHUFFLE_1_0_3_2).AsUInt64();
                            var sum         = Avx2.Add(acc_vec, data_swap);
                            var result      = Avx2.Add(product, sum);
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

                            var acc_vec     = Sse2.LoadVector128(acc + uint64_offset);
                            var data_vec    = Sse2.LoadVector128((uint*) inp + uint32_offset);
                            var key_vec     = Sse2.LoadVector128((uint*) secret1 + uint32_offset);
                            var data_key    = Sse2.Xor(data_vec, key_vec);
                            var data_key_lo = Sse2.Shuffle(data_key, _MM_SHUFFLE_0_3_0_1);
                            var product     = Sse2.Multiply(data_key, data_key_lo);
                            var data_swap   = Sse2.Shuffle(data_vec, _MM_SHUFFLE_1_0_3_2).AsUInt64();
                            var sum         = Sse2.Add(acc_vec, data_swap);
                            var result      = Sse2.Add(product, sum); 
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
                            xacc[i] += (ulong)(uint)(data_key & 0xFFFFFFFF) * (ulong)(uint)(y);
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

                        var acc_vec     = Avx2.LoadVector256(acc + uint64_offset);
                        var shifted     = Avx2.ShiftRightLogical(acc_vec, 47);
                        var data_vec    = Avx2.Xor(acc_vec, shifted);
                        var key_vec     = Avx2.LoadVector256((ulong*) secret3 + uint64_offset);
                        var data_key    = Avx2.Xor(data_vec, key_vec).AsUInt32();
                        var data_key_hi = Avx2.Shuffle(data_key, _MM_SHUFFLE_0_3_0_1);
                        var prod_lo     = Avx2.Multiply(data_key, prime32);
                        var prod_hi     = Avx2.Multiply(data_key_hi, prime32);
                        var result      = Avx2.Add(prod_lo, Avx2.ShiftLeftLogical(prod_hi, 32));
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

                        var acc_vec     = Sse2.LoadVector128(acc + uint64_offset).AsUInt32();
                        var shifted     = Sse2.ShiftRightLogical(acc_vec, 47);
                        var data_vec    = Sse2.Xor(acc_vec, shifted);
                        var key_vec     = Sse2.LoadVector128((uint*) secret3 + uint32_offset);
                        var data_key    = Sse2.Xor(data_vec, key_vec);
                        var data_key_hi = Sse2.Shuffle(data_key.AsUInt32(), _MM_SHUFFLE_0_3_0_1);
                        var prod_lo     = Sse2.Multiply(data_key, prime32);
                        var prod_hi     = Sse2.Multiply(data_key_hi, prime32);
                        var result      = Sse2.Add(prod_lo, Sse2.ShiftLeftLogical(prod_hi, 32));
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
            for (int n2 = 0; n2 < nbStripes; n2++ ) {
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

                        var acc_vec     = Avx2.LoadVector256(acc + uint64_offset);
                        var data_vec    = Avx2.LoadVector256((uint*)inp1 + uint32_offset);
                        var key_vec     = Avx2.LoadVector256((uint*)secret1 + uint32_offset);
                        var data_key    = Avx2.Xor(data_vec, key_vec);
                        var data_key_lo = Avx2.Shuffle(data_key, _MM_SHUFFLE_0_3_0_1);
                        var product     = Avx2.Multiply(data_key, data_key_lo);
                        var data_swap   = Avx2.Shuffle(data_vec, _MM_SHUFFLE_1_0_3_2).AsUInt64();
                        var sum         = Avx2.Add(acc_vec, data_swap);
                        var result      = Avx2.Add(product, sum);
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

                        var acc_vec     = Sse2.LoadVector128(acc + uint64_offset);
                        var data_vec    = Sse2.LoadVector128((uint*) inp1 + uint32_offset);
                        var key_vec     = Sse2.LoadVector128((uint*) secret1 + uint32_offset);
                        var data_key    = Sse2.Xor(data_vec, key_vec);
                        var data_key_lo = Sse2.Shuffle(data_key, _MM_SHUFFLE_0_3_0_1);
                        var product     = Sse2.Multiply(data_key, data_key_lo);
                        var data_swap   = Sse2.Shuffle(data_vec, _MM_SHUFFLE_1_0_3_2).AsUInt64();
                        var sum         = Sse2.Add(acc_vec, data_swap);
                        var result      = Sse2.Add(product, sum); 
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
                        xacc[i] += (ulong)(uint)(data_key & 0xFFFFFFFF) * (ulong)(uint)(y);
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

                    var acc_vec     = Avx2.LoadVector256(acc + uint64_offset);
                    var data_vec    = Avx2.LoadVector256((uint*)p + uint32_offset);
                    var key_vec     = Avx2.LoadVector256((uint*)secret2 + uint32_offset);
                    var data_key    = Avx2.Xor(data_vec, key_vec);
                    var data_key_lo = Avx2.Shuffle(data_key, _MM_SHUFFLE_0_3_0_1);
                    var product     = Avx2.Multiply(data_key, data_key_lo);
                    var data_swap   = Avx2.Shuffle(data_vec, _MM_SHUFFLE_1_0_3_2).AsUInt64();
                    var sum         = Avx2.Add(acc_vec, data_swap);
                    var result      = Avx2.Add(product, sum);
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

                    var acc_vec     = Sse2.LoadVector128(acc + uint64_offset);
                    var data_vec    = Sse2.LoadVector128((uint*) p + uint32_offset);
                    var key_vec     = Sse2.LoadVector128((uint*) secret2 + uint32_offset);
                    var data_key    = Sse2.Xor(data_vec, key_vec);
                    var data_key_lo = Sse2.Shuffle(data_key, _MM_SHUFFLE_0_3_0_1);
                    var product     = Sse2.Multiply(data_key, data_key_lo);
                    var data_swap   = Sse2.Shuffle(data_vec, _MM_SHUFFLE_1_0_3_2).AsUInt64();
                    var sum         = Sse2.Add(acc_vec, data_swap);
                    var result      = Sse2.Add(product, sum); 
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
                    xacc[i] += (ulong)(uint)(data_key & 0xFFFFFFFF) * (ulong)(uint)(y);
                }
            }

            uint128 uint128;
            byte* secret4 = secret + XXH_SECRET_MERGEACCS_START;
            ulong result64 = (ulong)len * XXH_PRIME64_1;
            
            for (int i1 = 0; i1 < 4; i1++)
            {
                ulong* acc1 = acc + 2 * i1;
                byte* secret1 = secret4 + 16 * i1;
                byte* ptr = secret1+8;
                ulong lhs = acc1[0] ^ *(ulong*) secret1;
                ulong rhs = acc1[1] ^ *(ulong*) ptr;
                uint128 ret;
                if (Bmi2.IsSupported)
                {
                    ulong product_low;
                    ulong product_high = Bmi2.X64.MultiplyNoFlags(lhs, rhs, &product_low);
                    uint128 r128;
                    r128.low64  = product_low;
                    r128.high64 = product_high;
                    ret = r128;
                }
                else
                {
                    ulong y = rhs & 0xFFFFFFFF;
                    ulong lo_lo = (ulong)(uint)(lhs & 0xFFFFFFFF) * (ulong)(uint)(y);
                    ulong y1 = rhs & 0xFFFFFFFF;
                    ulong hi_lo = (ulong)(uint)(lhs >> 32) * (ulong)(uint)(y1);
                    ulong y2 = rhs >> 32;
                    ulong lo_hi = (ulong)(uint)(lhs & 0xFFFFFFFF) * (ulong)(uint)(y2);
                    ulong y3 = rhs >> 32;
                    ulong hi_hi = (ulong)(uint)(lhs >> 32) * (ulong)(uint)(y3);
            
                    ulong cross = (lo_lo >> 32) + (hi_lo & 0xFFFFFFFF) + lo_hi;
                    ulong upper = (hi_lo >> 32) + (cross >> 32)        + hi_hi;
                    ulong lower = (cross << 32) | (lo_lo & 0xFFFFFFFF);

                    uint128 r128;
                    r128.low64  = lower;
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
            uint128.low64 = h64;
            byte* secret5 = secret + secretLen - XXH3_ACC_SIZE - XXH_SECRET_MERGEACCS_START;
            ulong result65 = ~((ulong)len * XXH_PRIME64_2);
            
            for (int i2 = 0; i2 < 4; i2++)
            {
                ulong* acc1 = acc + 2 * i2;
                byte* secret1 = secret5 + 16 * i2;
                byte* ptr = secret1+8;
                ulong lhs = acc1[0] ^ *(ulong*) secret1;
                ulong rhs = acc1[1] ^ *(ulong*) ptr;
                uint128 ret;
                if (Bmi2.IsSupported)
                {
                    ulong product_low;
                    ulong product_high = Bmi2.X64.MultiplyNoFlags(lhs, rhs, &product_low);
                    uint128 r128;
                    r128.low64  = product_low;
                    r128.high64 = product_high;
                    ret = r128;
                }
                else
                {
                    ulong y = rhs & 0xFFFFFFFF;
                    ulong lo_lo = (ulong)(uint)(lhs & 0xFFFFFFFF) * (ulong)(uint)(y);
                    ulong y1 = rhs & 0xFFFFFFFF;
                    ulong hi_lo = (ulong)(uint)(lhs >> 32) * (ulong)(uint)(y1);
                    ulong y2 = rhs >> 32;
                    ulong lo_hi = (ulong)(uint)(lhs & 0xFFFFFFFF) * (ulong)(uint)(y2);
                    ulong y3 = rhs >> 32;
                    ulong hi_hi = (ulong)(uint)(lhs >> 32) * (ulong)(uint)(y3);
            
                    ulong cross = (lo_lo >> 32) + (hi_lo & 0xFFFFFFFF) + lo_hi;
                    ulong upper = (hi_lo >> 32) + (cross >> 32)        + hi_hi;
                    ulong lower = (cross << 32) | (lo_lo & 0xFFFFFFFF);

                    uint128 r128;
                    r128.low64  = lower;
                    r128.high64 = upper;
                    ret = r128;
                }

                uint128 product = ret;
                result65 += product.low64 ^ product.high64;
            }

            ulong h65 = result65;
            h65 = h65 ^ (h65 >> 37);
            h65 *= 0x165667919E3779F9UL;
            h65 = h65 ^ (h65 >> 32);
            uint128.high64 = h65;

            return uint128;
        }

        int customSecretSize = XXH3_SECRET_DEFAULT_SIZE;
        byte* customSecret = stackalloc byte[customSecretSize];

        fixed (byte* ptr24 = &XXH3_SECRET[0])
        {
            for (int i1 = 0; i1 < customSecretSize; i1 += 8)
            {
                customSecret[i1]   = ptr24[i1];
                customSecret[i1+1] = ptr24[i1+1];
                customSecret[i1+2] = ptr24[i1+2];
                customSecret[i1+3] = ptr24[i1+3];
                customSecret[i1+4] = ptr24[i1+4];
                customSecret[i1+5] = ptr24[i1+5];
                customSecret[i1+6] = ptr24[i1+6];
                customSecret[i1+7] = ptr24[i1+7];
            }
        }

        if (Avx2.IsSupported)
        {
            const int m256i_size = 32;

            var seed1 = Vector256.Create((ulong)seed, (ulong)(0U - seed), (ulong)seed, (ulong)(0U - seed));

            fixed (byte* secret1 = &XXH3_SECRET[0])
            {
                for (int i = 0; i < XXH_SECRET_DEFAULT_SIZE / m256i_size; i++)
                {
                    int uint64_offset = i * 4;

                    var src32 = Avx2.LoadVector256(((ulong*)secret1) + uint64_offset);
                    var dst32 = Avx2.Add(src32, seed1);
                    Avx2.Store((ulong*) customSecret + uint64_offset, dst32);
                }
            }
        }
        else if (Sse2.IsSupported)
        {
            const int m128i_size = 16;

            var seed1 = Vector128.Create((long)seed, (long)(0U - seed));

            fixed (byte* secret1 = &XXH3_SECRET[0])
            {
                for (int i = 0; i < XXH_SECRET_DEFAULT_SIZE / m128i_size; i++) 
                {
                    int uint64_offset = i * 2;

                    var src16 = Sse2.LoadVector128(((long*) secret1) + uint64_offset);
                    var dst16 = Sse2.Add(src16, seed1);
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
                    ulong lo = *(ulong*) ptr + seed;
                    byte* ptr1 = kSecretPtr + 16 * i + 8;
                    ulong hi = *(ulong*) ptr1 - seed;
                    byte* dst = (byte*) customSecret + 16 * i;
                    *(ulong*) dst = lo;
                    byte* dst1 = (byte*) customSecret + 16 * i + 8;
                    *(ulong*) dst1 = hi;
                }
            }
        }

        ulong* acc5 = stackalloc ulong[8];
            
        fixed (ulong* ptr25 = &XXH3_INIT_ACC[0])
        {
            acc5[0] = ptr25[0];
            acc5[1] = ptr25[1];
            acc5[2] = ptr25[2];
            acc5[3] = ptr25[3];
            acc5[4] = ptr25[4];
            acc5[5] = ptr25[5];
            acc5[6] = ptr25[6];
            acc5[7] = ptr25[7];
        }
        
        int nbStripesPerBlock1 = (customSecretSize - XXH_STRIPE_LEN) / XXH_SECRET_CONSUME_RATE;
        int blockLen = XXH_STRIPE_LEN * nbStripesPerBlock1;
        int nbBlocks = (len - 1) / blockLen;

        for (int n1 = 0; n1 < nbBlocks; n1++) {
            byte* input1 = input + n1 * blockLen;
            for (int n = 0; n < nbStripesPerBlock1; n++ ) {
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

                        var acc_vec     = Avx2.LoadVector256(acc5 + uint64_offset);
                        var data_vec    = Avx2.LoadVector256((uint*)inp + uint32_offset);
                        var key_vec     = Avx2.LoadVector256((uint*)secret1 + uint32_offset);
                        var data_key    = Avx2.Xor(data_vec, key_vec);
                        var data_key_lo = Avx2.Shuffle(data_key, _MM_SHUFFLE_0_3_0_1);
                        var product     = Avx2.Multiply(data_key, data_key_lo);
                        var data_swap   = Avx2.Shuffle(data_vec, _MM_SHUFFLE_1_0_3_2).AsUInt64();
                        var sum         = Avx2.Add(acc_vec, data_swap);
                        var result      = Avx2.Add(product, sum);
                        Avx2.Store(acc5 + uint64_offset, result);
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

                        var acc_vec     = Sse2.LoadVector128(acc5 + uint64_offset);
                        var data_vec    = Sse2.LoadVector128((uint*) inp + uint32_offset);
                        var key_vec     = Sse2.LoadVector128((uint*) secret1 + uint32_offset);
                        var data_key    = Sse2.Xor(data_vec, key_vec);
                        var data_key_lo = Sse2.Shuffle(data_key, _MM_SHUFFLE_0_3_0_1);
                        var product     = Sse2.Multiply(data_key, data_key_lo);
                        var data_swap   = Sse2.Shuffle(data_vec, _MM_SHUFFLE_1_0_3_2).AsUInt64();
                        var sum         = Sse2.Add(acc_vec, data_swap);
                        var result      = Sse2.Add(product, sum); 
                        Sse2.Store(acc5 + uint64_offset, result);
                    }
                }
                else
                {
                    for (int i = 0; i < XXH_ACC_NB; i++)
                    {
                        ulong* xacc = acc5;
                        byte* xinput = inp;
                        byte* xsecret = secret1;

                        byte* ptr = xinput + i * 8;
                        ulong data_val = *(ulong*) ptr;
                        byte* ptr1 = xsecret + i * 8;
                        ulong data_key = data_val ^ *(ulong*) ptr1;
                        xacc[i ^ 1] += data_val;
                        ulong y = data_key >> 32;
                        xacc[i] += (ulong)(uint)(data_key & 0xFFFFFFFF) * (ulong)(uint)(y);
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

                    var acc_vec     = Avx2.LoadVector256(acc5 + uint64_offset);
                    var shifted     = Avx2.ShiftRightLogical(acc_vec, 47);
                    var data_vec    = Avx2.Xor(acc_vec, shifted);
                    var key_vec     = Avx2.LoadVector256((ulong*) secret2 + uint64_offset);
                    var data_key    = Avx2.Xor(data_vec, key_vec).AsUInt32();
                    var data_key_hi = Avx2.Shuffle(data_key, _MM_SHUFFLE_0_3_0_1);
                    var prod_lo     = Avx2.Multiply(data_key, prime32);
                    var prod_hi     = Avx2.Multiply(data_key_hi, prime32);
                    var result      = Avx2.Add(prod_lo, Avx2.ShiftLeftLogical(prod_hi, 32));
                    Avx2.Store(acc5 + uint64_offset, result);
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

                    var acc_vec     = Sse2.LoadVector128(acc5 + uint64_offset).AsUInt32();
                    var shifted     = Sse2.ShiftRightLogical(acc_vec, 47);
                    var data_vec    = Sse2.Xor(acc_vec, shifted);
                    var key_vec     = Sse2.LoadVector128((uint*) secret2 + uint32_offset);
                    var data_key    = Sse2.Xor(data_vec, key_vec);
                    var data_key_hi = Sse2.Shuffle(data_key.AsUInt32(), _MM_SHUFFLE_0_3_0_1);
                    var prod_lo     = Sse2.Multiply(data_key, prime32);
                    var prod_hi     = Sse2.Multiply(data_key_hi, prime32);
                    var result      = Sse2.Add(prod_lo, Sse2.ShiftLeftLogical(prod_hi, 32));
                    Sse2.Store(acc5 + uint64_offset, result);
                }
            }
            else
            {
                for (int i = 0; i < XXH_ACC_NB; i++)
                {
                    ulong* xacc = acc5;
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
        byte* input8 = input + nbBlocks * blockLen;
        for (int n3 = 0; n3 < nbStripes1; n3++ ) {
            byte* inp2 = input8 + n3 * XXH_STRIPE_LEN;
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

                    var acc_vec     = Avx2.LoadVector256(acc5 + uint64_offset);
                    var data_vec    = Avx2.LoadVector256((uint*)inp2 + uint32_offset);
                    var key_vec     = Avx2.LoadVector256((uint*)secret1 + uint32_offset);
                    var data_key    = Avx2.Xor(data_vec, key_vec);
                    var data_key_lo = Avx2.Shuffle(data_key, _MM_SHUFFLE_0_3_0_1);
                    var product     = Avx2.Multiply(data_key, data_key_lo);
                    var data_swap   = Avx2.Shuffle(data_vec, _MM_SHUFFLE_1_0_3_2).AsUInt64();
                    var sum         = Avx2.Add(acc_vec, data_swap);
                    var result      = Avx2.Add(product, sum);
                    Avx2.Store(acc5 + uint64_offset, result);
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

                    var acc_vec     = Sse2.LoadVector128(acc5 + uint64_offset);
                    var data_vec    = Sse2.LoadVector128((uint*) inp2 + uint32_offset);
                    var key_vec     = Sse2.LoadVector128((uint*) secret1 + uint32_offset);
                    var data_key    = Sse2.Xor(data_vec, key_vec);
                    var data_key_lo = Sse2.Shuffle(data_key, _MM_SHUFFLE_0_3_0_1);
                    var product     = Sse2.Multiply(data_key, data_key_lo);
                    var data_swap   = Sse2.Shuffle(data_vec, _MM_SHUFFLE_1_0_3_2).AsUInt64();
                    var sum         = Sse2.Add(acc_vec, data_swap);
                    var result      = Sse2.Add(product, sum); 
                    Sse2.Store(acc5 + uint64_offset, result);
                }
            }
            else
            {
                for (int i = 0; i < XXH_ACC_NB; i++)
                {
                    ulong* xacc = acc5;
                    byte* xinput = inp2;
                    byte* xsecret = secret1;

                    byte* ptr = xinput + i * 8;
                    ulong data_val = *(ulong*) ptr;
                    byte* ptr1 = xsecret + i * 8;
                    ulong data_key = data_val ^ *(ulong*) ptr1;
                    xacc[i ^ 1] += data_val;
                    ulong y = data_key >> 32;
                    xacc[i] += (ulong)(uint)(data_key & 0xFFFFFFFF) * (ulong)(uint)(y);
                }
            }
        }

        byte* p1 = input + len - XXH_STRIPE_LEN;
        byte* secret12 = customSecret + customSecretSize - XXH_STRIPE_LEN - XXH_SECRET_LASTACC_START;
        if (Avx2.IsSupported)
        {
            const int m256i_size = 32;
            const byte _MM_SHUFFLE_0_3_0_1 = 0b0011_0001;
            const byte _MM_SHUFFLE_1_0_3_2 = 0b0100_1110;

            for (int i = 0; i < XXH_STRIPE_LEN / m256i_size; i++)
            {
                int uint32_offset = i * 8;
                int uint64_offset = i * 4;

                var acc_vec     = Avx2.LoadVector256(acc5 + uint64_offset);
                var data_vec    = Avx2.LoadVector256((uint*)p1 + uint32_offset);
                var key_vec     = Avx2.LoadVector256((uint*)secret12 + uint32_offset);
                var data_key    = Avx2.Xor(data_vec, key_vec);
                var data_key_lo = Avx2.Shuffle(data_key, _MM_SHUFFLE_0_3_0_1);
                var product     = Avx2.Multiply(data_key, data_key_lo);
                var data_swap   = Avx2.Shuffle(data_vec, _MM_SHUFFLE_1_0_3_2).AsUInt64();
                var sum         = Avx2.Add(acc_vec, data_swap);
                var result      = Avx2.Add(product, sum);
                Avx2.Store(acc5 + uint64_offset, result);
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

                var acc_vec     = Sse2.LoadVector128(acc5 + uint64_offset);
                var data_vec    = Sse2.LoadVector128((uint*) p1 + uint32_offset);
                var key_vec     = Sse2.LoadVector128((uint*) secret12 + uint32_offset);
                var data_key    = Sse2.Xor(data_vec, key_vec);
                var data_key_lo = Sse2.Shuffle(data_key, _MM_SHUFFLE_0_3_0_1);
                var product     = Sse2.Multiply(data_key, data_key_lo);
                var data_swap   = Sse2.Shuffle(data_vec, _MM_SHUFFLE_1_0_3_2).AsUInt64();
                var sum         = Sse2.Add(acc_vec, data_swap);
                var result      = Sse2.Add(product, sum); 
                Sse2.Store(acc5 + uint64_offset, result);
            }
        }
        else
        {
            for (int i = 0; i < XXH_ACC_NB; i++)
            {
                ulong* xacc = acc5;
                byte* xinput = p1;
                byte* xsecret = secret12;

                byte* ptr = xinput + i * 8;
                ulong data_val = *(ulong*) ptr;
                byte* ptr1 = xsecret + i * 8;
                ulong data_key = data_val ^ *(ulong*) ptr1;
                xacc[i ^ 1] += data_val;
                ulong y = data_key >> 32;
                xacc[i] += (ulong)(uint)(data_key & 0xFFFFFFFF) * (ulong)(uint)(y);
            }
        }

        uint128 uint129;
        byte* secret13 = customSecret + XXH_SECRET_MERGEACCS_START;
        ulong result66 = (ulong)len * XXH_PRIME64_1;
            
        for (int i3 = 0; i3 < 4; i3++)
        {
            ulong* acc = acc5 + 2 * i3;
            byte* secret1 = secret13 + 16 * i3;
            byte* ptr = secret1+8;
            ulong lhs = acc[0] ^ *(ulong*) secret1;
            ulong rhs = acc[1] ^ *(ulong*) ptr;
            uint128 ret;
            if (Bmi2.IsSupported)
            {
                ulong product_low;
                ulong product_high = Bmi2.X64.MultiplyNoFlags(lhs, rhs, &product_low);
                uint128 r128;
                r128.low64  = product_low;
                r128.high64 = product_high;
                ret = r128;
            }
            else
            {
                ulong y = rhs & 0xFFFFFFFF;
                ulong lo_lo = (ulong)(uint)(lhs & 0xFFFFFFFF) * (ulong)(uint)(y);
                ulong y1 = rhs & 0xFFFFFFFF;
                ulong hi_lo = (ulong)(uint)(lhs >> 32) * (ulong)(uint)(y1);
                ulong y2 = rhs >> 32;
                ulong lo_hi = (ulong)(uint)(lhs & 0xFFFFFFFF) * (ulong)(uint)(y2);
                ulong y3 = rhs >> 32;
                ulong hi_hi = (ulong)(uint)(lhs >> 32) * (ulong)(uint)(y3);
            
                ulong cross = (lo_lo >> 32) + (hi_lo & 0xFFFFFFFF) + lo_hi;
                ulong upper = (hi_lo >> 32) + (cross >> 32)        + hi_hi;
                ulong lower = (cross << 32) | (lo_lo & 0xFFFFFFFF);

                uint128 r128;
                r128.low64  = lower;
                r128.high64 = upper;
                ret = r128;
            }

            uint128 product = ret;
            result66 += product.low64 ^ product.high64;
        }

        ulong h68 = result66;
        h68 = h68 ^ (h68 >> 37);
        h68 *= 0x165667919E3779F9UL;
        h68 = h68 ^ (h68 >> 32);
        uint129.low64 = h68;
        byte* secret14 = customSecret + customSecretSize - XXH3_ACC_SIZE - XXH_SECRET_MERGEACCS_START;
        ulong result67 = ~((ulong)len * XXH_PRIME64_2);
            
        for (int i4 = 0; i4 < 4; i4++)
        {
            ulong* acc = acc5 + 2 * i4;
            byte* secret1 = secret14 + 16 * i4;
            byte* ptr = secret1+8;
            ulong lhs = acc[0] ^ *(ulong*) secret1;
            ulong rhs = acc[1] ^ *(ulong*) ptr;
            uint128 ret;
            if (Bmi2.IsSupported)
            {
                ulong product_low;
                ulong product_high = Bmi2.X64.MultiplyNoFlags(lhs, rhs, &product_low);
                uint128 r128;
                r128.low64  = product_low;
                r128.high64 = product_high;
                ret = r128;
            }
            else
            {
                ulong y = rhs & 0xFFFFFFFF;
                ulong lo_lo = (ulong)(uint)(lhs & 0xFFFFFFFF) * (ulong)(uint)(y);
                ulong y1 = rhs & 0xFFFFFFFF;
                ulong hi_lo = (ulong)(uint)(lhs >> 32) * (ulong)(uint)(y1);
                ulong y2 = rhs >> 32;
                ulong lo_hi = (ulong)(uint)(lhs & 0xFFFFFFFF) * (ulong)(uint)(y2);
                ulong y3 = rhs >> 32;
                ulong hi_hi = (ulong)(uint)(lhs >> 32) * (ulong)(uint)(y3);
            
                ulong cross = (lo_lo >> 32) + (hi_lo & 0xFFFFFFFF) + lo_hi;
                ulong upper = (hi_lo >> 32) + (cross >> 32)        + hi_hi;
                ulong lower = (cross << 32) | (lo_lo & 0xFFFFFFFF);

                uint128 r128;
                r128.low64  = lower;
                r128.high64 = upper;
                ret = r128;
            }

            uint128 product = ret;
            result67 += product.low64 ^ product.high64;
        }

        ulong h69 = result67;
        h69 = h69 ^ (h69 >> 37);
        h69 *= 0x165667919E3779F9UL;
        h69 = h69 ^ (h69 >> 32);
        uint129.high64 = h69;

        return uint129;
    }
}