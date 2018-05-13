namespace Standart.Hash.xxHash.Test
{
    using System;
    using System.IO;
    using System.Threading.Tasks;
    using Xunit;

    public class xxHash32Test
    {
        [Fact]
        public void Compute_hash32_for_the_length_1()
        {
            // Arrange
            byte[] data = {0xde};

            // Act
            uint hash = xxHash32.ComputeHash(data, data.Length);

            // Assert
            Assert.Equal(hash, (uint) 0x2330eac0);
        }

        [Fact]
        public void Compute_hash32_for_the_length_5()
        {
            // Arrange
            byte[] data = {0xde, 0x55, 0x47, 0x7f, 0x14};

            // Act
            uint hash = xxHash32.ComputeHash(data, data.Length);

            // Assert
            Assert.Equal(hash, (uint) 0x112348ba);
        }

        [Fact]
        public void Compute_hash32_for_the_length_16()
        {
            // Arrange
            byte[] data = new byte[]
            {
                0xde, 0x55, 0x47, 0x7f, 0x14, 0x8f, 0xf1, 0x48,
                0x22, 0x3a, 0x40, 0x96, 0x56, 0xc5, 0xdc, 0xbb
            };
            
            // Act
            uint hash = xxHash32.ComputeHash(data, data.Length);

            // Assert
            Assert.Equal(hash, (uint) 0xcdf89609);
        }

        [Fact]
        public void Compute_hash32_for_the_length_17()
        {
            // Arrange
            byte[] data = new byte[]
            {
                0xde, 0x55, 0x47, 0x7f, 0x14, 0x8f, 0xf1, 0x48,
                0x22, 0x3a, 0x40, 0x96, 0x56, 0xc5, 0xdc, 0xbb,
                0x0e
            };
            
            // Act
            uint hash = xxHash32.ComputeHash(data, data.Length);

            // Assert
            Assert.Equal(hash, (uint) 0xbca8f924);
        }

        [Fact]
        public void Compute_hash32_for_the_length_21()
        {
            // Arrange
            byte[] data = new byte[]
            {
                0xde, 0x55, 0x47, 0x7f, 0x14, 0x8f, 0xf1, 0x48,
                0x22, 0x3a, 0x40, 0x96, 0x56, 0xc5, 0xdc, 0xbb,
                0x0e, 0x59, 0x4d, 0x42, 0xc5
            };
            
            // Act
            uint hash = xxHash32.ComputeHash(data, data.Length);

            // Assert
            Assert.Equal(hash, (uint) 0xf4518e14);
        }
        
        [Fact]
        public void Compute_hash32_for_the_length_32()
        {
            // Arrange
            byte[] data = new byte[]
            {
                0xde, 0x55, 0x47, 0x7f, 0x14, 0x8f, 0xf1, 0x48,
                0x22, 0x3a, 0x40, 0x96, 0x56, 0xc5, 0xdc, 0xbb,
                0x0e, 0x59, 0x4d, 0x42, 0xc5, 0x07, 0x21, 0x08,
                0x1c, 0x2c, 0xc9, 0x38, 0x7d, 0x43, 0x83, 0x11,
            };
            
            // Act
            uint hash = xxHash32.ComputeHash(data, data.Length);

            // Assert
            Assert.Equal(hash, (uint) 0xf8497daa);
        }

        [Fact]
        public void Compute_hash32_for_the_stream_1()
        {
            // Arrange
            byte[] data = {0xde};

            // Act
            uint hash = xxHash32.ComputeHash(new MemoryStream(data));

            // Assert
            Assert.Equal(hash, (uint) 0x2330eac0);
        }

        [Fact]
        public void Compute_hash32_for_the_stream_5()
        {
            // Arrange
            byte[] data = {0xde, 0x55, 0x47, 0x7f, 0x14};

            // Act
            uint hash = xxHash32.ComputeHash(new MemoryStream(data));

            // Assert
            Assert.Equal(hash, (uint) 0x112348ba);
        }

        [Fact]
        public void Compute_hash32_for_the_stream_16()
        {
            // Arrange
            byte[] data = new byte[]
            {
                0xde, 0x55, 0x47, 0x7f, 0x14, 0x8f, 0xf1, 0x48,
                0x22, 0x3a, 0x40, 0x96, 0x56, 0xc5, 0xdc, 0xbb
            };
            
            // Act
            uint hash = xxHash32.ComputeHash(new MemoryStream(data));

            // Assert
            Assert.Equal(hash, (uint) 0xcdf89609);
        }

        [Fact]
        public void Compute_hash32_for_the_stream_17()
        {
            // Arrange
            byte[] data = new byte[]
            {
                0xde, 0x55, 0x47, 0x7f, 0x14, 0x8f, 0xf1, 0x48,
                0x22, 0x3a, 0x40, 0x96, 0x56, 0xc5, 0xdc, 0xbb,
                0x0e
            };

            // Act
            uint hash = xxHash32.ComputeHash(new MemoryStream(data));

            // Assert
            Assert.Equal(hash, (uint) 0xbca8f924);
        }
        
        [Fact]
        public void Compute_hash32_for_the_stream_21()
        {
            // Arrange
            byte[] data = new byte[]
            {
                0xde, 0x55, 0x47, 0x7f, 0x14, 0x8f, 0xf1, 0x48,
                0x22, 0x3a, 0x40, 0x96, 0x56, 0xc5, 0xdc, 0xbb,
                0x0e, 0x59, 0x4d, 0x42, 0xc5
            };

            // Act
            uint hash = xxHash32.ComputeHash(new MemoryStream(data));

            // Assert
            Assert.Equal(hash, (uint) 0xf4518e14);
        }
        
        [Fact]
        public void Compute_hash32_for_the_stream_32()
        {
            // Arrange
            byte[] data = new byte[]
            {
                0xde, 0x55, 0x47, 0x7f, 0x14, 0x8f, 0xf1, 0x48,
                0x22, 0x3a, 0x40, 0x96, 0x56, 0xc5, 0xdc, 0xbb,
                0x0e, 0x59, 0x4d, 0x42, 0xc5, 0x07, 0x21, 0x08,
                0x1c, 0x2c, 0xc9, 0x38, 0x7d, 0x43, 0x83, 0x11,
            };

            // Act
            uint hash = xxHash32.ComputeHash(new MemoryStream(data));

            // Assert
            Assert.Equal(hash, (uint) 0xf8497daa);
        }

        [Fact]
        public void Compute_hash32_for_the_random_stream()
        {
            // Arrange
            int[] size =
            {
                256,
                256 * 3,
                256 * 3 + 1,
                256 * 3 + 15,
                256 * 3 + 16,
                256 * 3 + 41
            };
            var random = new Random();

            for (int i = 0; i < size.Length; i++)
            {
                // Arrange
                var data = new byte[size[i]];
                random.NextBytes(data);
                
                // Act
                uint hash1 = xxHash32.ComputeHash(data, data.Length);
                uint hash2 = xxHash32.ComputeHash(new MemoryStream(data), 32);
                
                // Assert
                Assert.Equal(hash1, hash2);
            }
        }
        
        [Fact]
        public async Task Compute_hash32_for_the_async_stream_1()
        {
            // Arrange
            byte[] data = {0xde};

            // Act
            uint hash = await xxHash32.ComputeHashAsync(new MemoryStream(data));

            // Assert
            Assert.Equal(hash, (uint) 0x2330eac0);
        }

        [Fact]
        public async Task Compute_hash32_for_the_async_stream_5()
        {
            // Arrange
            byte[] data = {0xde, 0x55, 0x47, 0x7f, 0x14};

            // Act
            uint hash = await xxHash32.ComputeHashAsync(new MemoryStream(data));

            // Assert
            Assert.Equal(hash, (uint) 0x112348ba);
        }

        [Fact]
        public async Task Compute_hash32_for_the_async_stream_16()
        {
            // Arrange
            byte[] data = new byte[]
            {
                0xde, 0x55, 0x47, 0x7f, 0x14, 0x8f, 0xf1, 0x48,
                0x22, 0x3a, 0x40, 0x96, 0x56, 0xc5, 0xdc, 0xbb
            };
            
            // Act
            uint hash = await xxHash32.ComputeHashAsync(new MemoryStream(data));

            // Assert
            Assert.Equal(hash, (uint) 0xcdf89609);
        }

        [Fact]
        public async Task Compute_hash32_for_the_async_stream_17()
        {
            // Arrange
            byte[] data = new byte[]
            {
                0xde, 0x55, 0x47, 0x7f, 0x14, 0x8f, 0xf1, 0x48,
                0x22, 0x3a, 0x40, 0x96, 0x56, 0xc5, 0xdc, 0xbb,
                0x0e
            };

            // Act
            uint hash = await xxHash32.ComputeHashAsync(new MemoryStream(data));

            // Assert
            Assert.Equal(hash, (uint) 0xbca8f924);
        }
        
        [Fact]
        public async Task Compute_hash32_for_the_async_stream_21()
        {
            // Arrange
            byte[] data = new byte[]
            {
                0xde, 0x55, 0x47, 0x7f, 0x14, 0x8f, 0xf1, 0x48,
                0x22, 0x3a, 0x40, 0x96, 0x56, 0xc5, 0xdc, 0xbb,
                0x0e, 0x59, 0x4d, 0x42, 0xc5
            };

            // Act
            uint hash = await xxHash32.ComputeHashAsync(new MemoryStream(data));

            // Assert
            Assert.Equal(hash, (uint) 0xf4518e14);
        }
        
        [Fact]
        public async Task Compute_hash32_for_the_async_stream_32()
        {
            // Arrange
            byte[] data = new byte[]
            {
                0xde, 0x55, 0x47, 0x7f, 0x14, 0x8f, 0xf1, 0x48,
                0x22, 0x3a, 0x40, 0x96, 0x56, 0xc5, 0xdc, 0xbb,
                0x0e, 0x59, 0x4d, 0x42, 0xc5, 0x07, 0x21, 0x08,
                0x1c, 0x2c, 0xc9, 0x38, 0x7d, 0x43, 0x83, 0x11,
            };

            // Act
            uint hash = await xxHash32.ComputeHashAsync(new MemoryStream(data));

            // Assert
            Assert.Equal(hash, (uint) 0xf8497daa);
        }

        [Fact]
        public async Task Compute_hash32_for_the_async_random_stream()
        {
            // Arrange
            int[] size =
            {
                256,
                256 * 3,
                256 * 3 + 1,
                256 * 3 + 15,
                256 * 3 + 16,
                256 * 3 + 41
            };
            var random = new Random();

            for (int i = 0; i < size.Length; i++)
            {
                // Arrange
                var data = new byte[size[i]];
                random.NextBytes(data);
                
                // Act
                uint hash1 = xxHash32.ComputeHash(data, data.Length);
                uint hash2 = await xxHash32.ComputeHashAsync(new MemoryStream(data), 32);
                
                // Assert
                Assert.Equal(hash1, hash2);
            }
        }
    }
}