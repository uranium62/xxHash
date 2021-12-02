namespace Standart.Hash.xxHash
{
    using System.Runtime.CompilerServices;

    public static partial class xxHash32
    {
        private const uint p1 = 2654435761U;
        private const uint p2 = 2246822519U;
        private const uint p3 = 3266489917U;
        private const uint p4 = 668265263U;
        private const uint p5 = 374761393U;

        /// <summary>
        /// Compute xxhash32 for the unsafe array of memory
        /// </summary>
        /// <param name="ptr"></param>
        /// <param name="length"></param>
        /// <param name="seed"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe uint UnsafeComputeHash(byte* ptr, int length, uint seed)
        {
            byte* end = ptr + length;
            uint h32;

            if (length >= 16)
            {
                byte* limit = end - 16;

                uint v1 = seed + p1 + p2;
                uint v2 = seed + p2;
                uint v3 = seed + 0;
                uint v4 = seed - p1;

                do
                {
                    v1 += *((uint*)ptr) * p2;
                    v1 = BitUtils.RotateLeft(v1, 13); // rotl 13
                    v1 *= p1;
                    ptr += 4;

                    v2 += *((uint*)ptr) * p2;
                    v2 = BitUtils.RotateLeft(v2, 13); // rotl 13
                    v2 *= p1;
                    ptr += 4;

                    v3 += *((uint*)ptr) * p2;
                    v3 = BitUtils.RotateLeft(v3, 13); // rotl 13
                    v3 *= p1;
                    ptr += 4;

                    v4 += *((uint*)ptr) * p2;
                    v4 = BitUtils.RotateLeft(v4, 13); // rotl 13
                    v4 *= p1;
                    ptr += 4;

                } while (ptr <= limit);

                h32 = BitUtils.RotateLeft(v1, 1) +   // rotl 1
                      BitUtils.RotateLeft(v2, 7) +   // rotl 7
                      BitUtils.RotateLeft(v3, 12) +  // rotl 12
                      BitUtils.RotateLeft(v4, 18);   // rotl 18
            }
            else
            {
                h32 = seed + p5;
            }

            h32 += (uint)length;

            // finalize
            while (ptr <= end - 4)
            {
                h32 += *((uint*)ptr) * p3;
                h32 = BitUtils.RotateLeft(h32, 17) * p4; // (rotl 17) * p4
                ptr += 4;
            }

            while (ptr < end)
            {
                h32 += *((byte*)ptr) * p5;
                h32 = BitUtils.RotateLeft(h32, 11) * p1; // (rotl 11) * p1
                ptr += 1;
            }

            // avalanche
            h32 ^= h32 >> 15;
            h32 *= p2;
            h32 ^= h32 >> 13;
            h32 *= p3;
            h32 ^= h32 >> 16;

            return h32;
        }

        /// <summary>
        /// Compute the first part of xxhash32 (need for the streaming api)
        /// </summary>
        /// <param name="data"></param>
        /// <param name="l"></param>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <param name="v3"></param>
        /// <param name="v4"></param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static unsafe void UnsafeAlign(byte[] data, int l, ref uint v1, ref uint v2, ref uint v3, ref uint v4)
        {
            fixed (byte* pData = &data[0])
            {
                byte* ptr = pData;
                byte* limit = ptr + l;

                do
                {
                    v1 += *((uint*)ptr) * p2;
                    v1 = BitUtils.RotateLeft(v1, 13); // rotl 13
                    v1 *= p1;
                    ptr += 4;

                    v2 += *((uint*)ptr) * p2;
                    v2 = BitUtils.RotateLeft(v2, 13); // rotl 13
                    v2 *= p1;
                    ptr += 4;

                    v3 += *((uint*)ptr) * p2;
                    v3 = BitUtils.RotateLeft(v3, 13); // rotl 13
                    v3 *= p1;
                    ptr += 4;

                    v4 += *((uint*)ptr) * p2;
                    v4 = BitUtils.RotateLeft(v4, 13); // rotl 13
                    v4 *= p1;
                    ptr += 4;

                } while (ptr < limit);
            }
        }

        /// <summary>
        /// Compute the second part of xxhash32 (need for the streaming api)
        /// </summary>
        /// <param name="data"></param>
        /// <param name="l"></param>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <param name="v3"></param>
        /// <param name="v4"></param>
        /// <param name="length"></param>
        /// <param name="seed"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static unsafe uint UnsafeFinal(byte[] data, int l, ref uint v1, ref uint v2, ref uint v3, ref uint v4, long length, uint seed)
        {
            fixed (byte* pData = &data[0])
            {
                byte* ptr = pData;
                byte* end = pData + l;
                uint h32;

                if (length >= 16)
                {
                    h32 = BitUtils.RotateLeft(v1, 1) +  // rotl 1
                          BitUtils.RotateLeft(v2, 7) +  // rotl 7
                          BitUtils.RotateLeft(v3, 12) + // rotl 12
                          BitUtils.RotateLeft(v4, 18);  // rotl 18
                }
                else
                {
                    h32 = seed + p5;
                }

                h32 += (uint)length;

                // finalize
                while (ptr <= end - 4)
                {
                    h32 += *((uint*)ptr) * p3;
                    h32 = BitUtils.RotateLeft(h32, 17) * p4; // (rotl 17) * p4
                    ptr += 4;
                }

                while (ptr < end)
                {
                    h32 += *((byte*)ptr) * p5;
                    h32 = BitUtils.RotateLeft(h32, 11) * p1; // (rotl 11) * p1
                    ptr += 1;
                }

                // avalanche
                h32 ^= h32 >> 15;
                h32 *= p2;
                h32 ^= h32 >> 13;
                h32 *= p3;
                h32 ^= h32 >> 16;

                return h32;
            }
        }
    }
}
