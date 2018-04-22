namespace Standart.Hash.xxHash.Test
{
    using System.Text;
    using Xunit;

    public class xxHash32Test
    {
        [Fact]
        public void Compute_hash_for_the_byte_array()
        {
            // Arrange
            byte[] data = new byte[]
            {
                0xde, 0x55, 0x47, 0x7f, 0x14, 0x8f, 0xf1, 0x48,
                0x22, 0x3a, 0x40, 0x96, 0x56, 0xc5, 0xdc, 0xbb,
                0x0e, 0x59, 0x4d, 0x42, 0xc5, 0x07, 0x21, 0x08,
                0x1c, 0x2c, 0xc9, 0x38, 0x7d, 0x43, 0x83, 0x11,
            };

            uint[] actual = new uint[]
            {
                0x2330eac0, 0x2c006f7e, 0xa1481114, 0x28d386f9, 0x112348ba, 0x65da8e31, 0x7555bcbd, 0x46cf19ed,
                0x27802c05, 0x8d0a6fae, 0x6075daf0, 0x4c5aef64, 0x8c6b5a5e, 0x00c1810a, 0x863d91cf, 0xcdf89609,
                0xbca8f924, 0xed028fb5, 0x81c19b77, 0x1a0550fa, 0xf4518e14, 0xae2eace8, 0xe7f85aa8, 0xcf8df264,
                0xb4fd0c90, 0x511934b0, 0x796f989a, 0x26b664a6, 0x8c242079, 0x5842beef, 0xd1d0b350, 0xf8497daa,
            };

            for (int len = 1; len <= data.Length; len++)
            {
                // Act
                uint hash = xxHash32.ComputeHash(data, len);
                
                // Assert
                Assert.Equal(hash, actual[len - 1]);
            }
        }

        [Fact]
        public void Compute_hash_for_the_string()
        {
            // Arrange
            byte[] data = Encoding.UTF8.GetBytes("Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod");

            // Act
            uint hash = xxHash32.ComputeHash(data, data.Length);

            // Assert
            Assert.Equal(0xe3cd4dee, hash);
        }
    }
}