namespace Standart.Hash.xxHash.Test
{
    using System;
    using Xunit;

    public class UtilsTest
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
                Utils.BlockCopy(src, 0, dst, 0, count);

                for (int i = 0; i < count; i++)
                {
                    Assert.Equal(src[i], dst[i]);
                }
            }
        }

        [Fact]
        public void Should_convert_to_bytes()
        {
            var hash = new uint128
            {
                high64 = 3466251221427321594,
                low64 = 2862260537881727713
            };
            
            // (hBits * 18446744073709551616) + lBits
            // (3466251221427321594 * 18446744073709551616) + 2862260537881727713
        
            // dec: 63941049176852939372872402763456123617
            // hex: 301A991EF3707AFA27B8CACB570F12E1
            var expected = new byte[]
            {
                0xe1, 0x12, 0x0F, 0x57, 0xcb, 0xca, 0xb8, 0x27,
                0xfa, 0x7a, 0x70, 0xf3, 0x1e, 0x99, 0x1a, 0x30
            };
            
            // Act
            var actual = hash.ToBytes();
            
            // Assert
            for (int i = 0; i < 16; i++)
                Assert.Equal(expected[i], actual[i]);
        }

        [Fact]
        public void Should_convert_to_guid()
        {
            var hash = new uint128
            {
                high64 = 3466251221427321594,
                low64 = 2862260537881727713
            };

            // Act
            var guid1 = new Guid(hash.ToBytes());
            var guid2 = hash.ToGuid();
            
            // Assert
            Assert.Equal(guid1, guid2);
        }
    }
}