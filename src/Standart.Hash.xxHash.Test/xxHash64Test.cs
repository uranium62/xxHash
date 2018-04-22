namespace Standart.Hash.xxHash.Test
{
    using System.Text;
    using Hash;
    using Xunit;

    public class xxHash64Test
    {
        [Fact]
        public void Compute_hash_for_the_byte_array()
        {
            // Arrange
            byte[] data = new byte[]
            {
                0x60, 0x82, 0x40, 0x77, 0x8a, 0x0e, 0xe4, 0xd5,
                0x85, 0x1f, 0xa6, 0x86, 0x34, 0x01, 0xd7, 0xf2,
                0x30, 0x5d, 0x84, 0x54, 0x15, 0xf9, 0xbd, 0x03,
                0x4b, 0x0f, 0x90, 0x4e, 0xf5, 0x57, 0x21, 0x21,
                0xed, 0x8c, 0x19, 0x93, 0xbd, 0x01, 0x12, 0x0c,
                0x20, 0xb0, 0x33, 0x98, 0x4b, 0xe7, 0xc1, 0x0a,
                0x27, 0x6d, 0xb3, 0x5c, 0xc7, 0xc0, 0xd0, 0xa0,
                0x7e, 0x28, 0xce, 0x46, 0x85, 0xb7, 0x2b, 0x16,
            };

            ulong[] actual = new ulong[]
            {
                0xb3e7ca6ca5ba3445, 0xc48d23e7117c5c9f, 0xbdde5e6c403b877f, 0xb1f2d0131359c662, 0x917b11ed024938fc, 0xf1f919ac6d7f76e4, 0xea769e99c9c9bbd8, 0x703ba74928ca5564,
                0xe7c3e0490a3b1d95, 0x3029ac2ba355e675, 0x4c0457cbdeef0b12, 0xe62ef3a1d378c12e, 0x9d1cb0d181d58bee, 0x9a5fb7dcdf81ce31, 0x4b34ada6b021b0f0, 0x06a48a5ee2da8aa0,
                0xc2c6ca165cef49be, 0xd65d8cf3e8303c45, 0x9c4fe072aa5ba2c3, 0x6fdcaa87f4c0027d, 0xdf36b78f6ed0d0c4, 0x4c2de293bd035f97, 0x80e9997ecb4b4dba, 0xedc569773e3c1aae,
                0xce488e21bb53a4e2, 0x9eba278ca7d6167f, 0xafc41ab957fc123c, 0x221bc4f34b8afc02, 0x89448861c1506213, 0x1e790a0825d2edd5, 0xbac80f4da00a5ddb, 0x9233096b7804e12c,
                0xa596aeb3e9e2a0fb, 0xe5e8a197b29353c0, 0x75adcf4cf52af71f, 0x98e480d02a353d0b, 0xba1959292dce8087, 0x244320e6b95005cf, 0xc7a8f723dedfcb7f, 0x5557772a50b43c1a,
                0xdfe1c66f17b8caad, 0xcce29f0e017c531c, 0xc7582d7753b82d55, 0xfd7a0f9119288736, 0xd9d70c5d5d544166, 0x1e45f4055ed9dd64, 0xbdc0b3dedd7f9ba6, 0xfef1d3bf774540d4,
                0xa9c399777e64b4fa, 0xa6a3a1e8bcfa2b2d, 0xb3e8ee1513ebb5cf, 0x1705345d27cd571e, 0x101c3fe5b4cbbc77, 0xc0990c64b0b3b7ab, 0x9e0a9991f51b4ff5, 0xd24f884f6c74e067,
                0x5ae156ca6a9202c9, 0x60786b148def4de5, 0x36648dcc3f16c761, 0x3b5d877329197688, 0x22cb98d859720834, 0x77c5f375685b7942, 0x7a9f2086cdc70d02, 0x4c0a65b1ef9ea060,
            };

            for (int len = 1; len <= data.Length; len++)
            {         
                // Act
                ulong hash = xxHash64.ComputeHash(data, len);
                
                // Assert
                Assert.Equal(hash, actual[len-1]);
            }
        }

        [Fact]
        public void Compute_hash_for_the_string()
        {
            // Arrange
            byte[] data = Encoding.UTF8.GetBytes("Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod");

            // Act
            ulong hash = xxHash64.ComputeHash(data, data.Length);

            // Assert
            Assert.Equal((ulong) 0x5dee64ff7c935d7f, hash);
        }
    }
}