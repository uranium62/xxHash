using System;
using System.Text;
using Xunit;

namespace Standart.Hash.xxHash.Test
{
    public class xxHash128Test
    {
        [Fact]
        public void Compute_hash128_for_bytes()
        {
            // Arrange
            var bytes = new byte[]
            {
                0xd2, 0x94, 0x29, 0xc9, 0x4c, 0xc5, 0x0f, 0xbb,
                0xaa, 0xf4, 0x7c, 0xd5, 0x69, 0x5a, 0xa9, 0xbd,
                0xaf, 0xd8, 0x3f, 0xfb, 0xca, 0x6a, 0xd4, 0x2c,
                0x6c, 0x69, 0x7a, 0x5b, 0x0d, 0xe8, 0xd2, 0xb1,
                0x41, 0xb3, 0x1b, 0x23, 0xdb, 0x8c, 0x25, 0xb4,
                0x6c, 0xfb
            };

            ulong expectedH = 3466251221427321594;
            ulong expectedL = 2862260537881727713;

            // Act
            var hash = xxHash128.ComputeHash(bytes, bytes.Length);

            // Assert
            Assert.Equal(expectedH, hash.high64);
            Assert.Equal(expectedL, hash.low64);
        }

        [Fact]
        public void Compute_hash128_for_span()
        {
            // Arrange
            var bytes = new byte[]
            {
                0xd2, 0x94, 0x29, 0xc9, 0x4c, 0xc5, 0x0f, 0xbb,
                0xaa, 0xf4, 0x7c, 0xd5, 0x69, 0x5a, 0xa9, 0xbd,
                0xaf, 0xd8, 0x3f, 0xfb, 0xca, 0x6a, 0xd4, 0x2c,
                0x6c, 0x69, 0x7a, 0x5b, 0x0d, 0xe8, 0xd2, 0xb1,
                0x41, 0xb3, 0x1b, 0x23, 0xdb, 0x8c, 0x25, 0xb4,
                0x6c, 0xfb
            };
            var span = bytes.AsSpan();

            ulong expectedH = 3466251221427321594;
            ulong expectedL = 2862260537881727713;

            // Act
            var hash = xxHash128.ComputeHash(span, span.Length);

            // Assert
            Assert.Equal(expectedH, hash.high64);
            Assert.Equal(expectedL, hash.low64);
        }


        [Fact]
        public void Compute_hash128_for_string()
        {
            // Arrange
            var str = "veni vidi vici";
            var bytes = Encoding.Unicode.GetBytes(str);

            // Act
            var hash1 = xxHash128.ComputeHash(str);
            var hash2 = xxHash128.ComputeHash(bytes, bytes.Length);

            // Assert
            Assert.Equal(hash1.high64, hash2.high64);
            Assert.Equal(hash1.low64, hash2.low64);
        }

        [Fact]
        public void Compute_hash128_bytes_for_bytes()
        {
            // Arrange
            var bytes = new byte[]
            {
                0xd2, 0x94, 0x29, 0xc9, 0x4c, 0xc5, 0x0f, 0xbb,
                0xaa, 0xf4, 0x7c, 0xd5, 0x69, 0x5a, 0xa9, 0xbd,
                0xaf, 0xd8, 0x3f, 0xfb, 0xca, 0x6a, 0xd4, 0x2c,
                0x6c, 0x69, 0x7a, 0x5b, 0x0d, 0xe8, 0xd2, 0xb1,
                0x41, 0xb3, 0x1b, 0x23, 0xdb, 0x8c, 0x25, 0xb4,
                0x6c, 0xfb
            };

            // ulong expectedH = 3466251221427321594;
            // ulong expectedL = 2862260537881727713;

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
            var hash = xxHash128.ComputeHashBytes(bytes, bytes.Length);

            // Assert
            for (int i = 0; i < 16; i++)
                Assert.Equal(expected[i], hash[i]);
        }

    }
}