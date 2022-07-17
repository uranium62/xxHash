using System;
using System.Text;
using Xunit;

namespace Standart.Hash.xxHash.Test
{
    public class xxHash3Test
    {
        [Fact]
        public void Compute_xxhash3_for_bytes()
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
            var expected = 6698906707421582347UL;

            // Act
            var hash = xxHash3.ComputeHash(bytes, bytes.Length);

            // Assert
            Assert.Equal(expected, hash);
        }

        [Fact]
        public void Compute_xxhash3_for_span()
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
            var expected = 6698906707421582347UL;

            // Act
            var hash = xxHash3.ComputeHash(span, span.Length);

            // Assert
            Assert.Equal(expected, hash);
        }


        [Fact]
        public void Compute_xxhash3_for_string()
        {
            // Arrange
            var str = "veni vidi vici";
            var bytes = Encoding.Unicode.GetBytes(str);

            // Act
            var hash1 = xxHash3.ComputeHash(str);
            var hash2 = xxHash3.ComputeHash(bytes, bytes.Length);

            // Assert
            Assert.Equal(hash1, hash2);
        }

        [Fact]
        public void Compute_xxhash3_fuzzing()
        {
            var rand = new Random();

            for (int i = 0; i < 100000; i++)
            {
                // Arrange
                var seed = (ulong)rand.Next();
                var data = new byte[rand.Next(1024) + 1];
                rand.NextBytes(data);

                // Act
                var hash1 = xxHash3.ComputeHash(data, data.Length, seed);
                var hash2 = xxHashNative.ComputeXXH3(data, (ulong)data.Length, seed);

                // Assert
                Assert.Equal(hash1, hash2);
            }
        }
    }
}
