namespace Standart.Hash.xxHash.Test
{
    using System;
    using Xunit;

    public class UnsafeBufferTest
    {
        [Fact]
        public void Copy_different_blocks()
        {
            // Arrange
            byte[] src = new byte[100];
            byte[] dst = new byte[100];

            int[] counts = new int[]
            {
                 0,  1,  2,  3,  4,  5,  6,  7,
                 8,  9, 10, 11, 12, 13, 14, 15, 
                16, 17, 18, 19, 20, 21, 22, 23,
                24, 25, 26, 27, 28, 29, 30, 31,
                32, 33, 34, 64, 65, 66, 96, 99
            };

            var rand = new Random(42);
            rand.NextBytes(src);

            // Act, Assert
            foreach (int count in counts)
            {
                UnsafeBuffer.BlockCopy(src, 0, dst, 0, count);

                for (int i = 0; i < count; i++)
                {
                    Assert.Equal(src[i], dst[i]);
                }
            }
        }
    }
}