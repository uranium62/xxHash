namespace Standart.Hash.xxHash.Test
{
    using System;
    using System.IO;
    using System.Threading;
    using System.Threading.Tasks;
    using Xunit;

    public class xxHash64Test
    {
        [Fact]
        public void Compute_hash64_for_the_length_1()
        {
            // Arrange
            byte[] data = {0x60};
            Span<byte> span = new Span<byte>(data);
            ReadOnlySpan<byte> rspan = new ReadOnlySpan<byte>(data);
            
            // Act
            ulong hash1 = xxHash64.ComputeHash(data, data.Length);
            ulong hash2 = xxHash64.ComputeHash(span, span.Length);
            ulong hash3 = xxHash64.ComputeHash(rspan, rspan.Length);
            
            // Assert
            Assert.Equal(hash1, (ulong) 0xb3e7ca6ca5ba3445);
            Assert.Equal(hash2, (ulong) 0xb3e7ca6ca5ba3445);
            Assert.Equal(hash3, (ulong) 0xb3e7ca6ca5ba3445);
        }
        
        [Fact]
        public void Compute_hash64_for_the_length_5()
        {
            // Arrange
            byte[] data = {0x60, 0x82, 0x40, 0x77, 0x8a};
            Span<byte> span = new Span<byte>(data);
            ReadOnlySpan<byte> rspan = new ReadOnlySpan<byte>(data);
            
            // Act
            ulong hash1 = xxHash64.ComputeHash(data, data.Length);
            ulong hash2 = xxHash64.ComputeHash(span, span.Length);
            ulong hash3 = xxHash64.ComputeHash(rspan, rspan.Length);
            
            // Assert
            Assert.Equal(hash1, (ulong) 0x917b11ed024938fc);
            Assert.Equal(hash2, (ulong) 0x917b11ed024938fc);
            Assert.Equal(hash3, (ulong) 0x917b11ed024938fc);
        }
        
        [Fact]
        public void Compute_hash64_for_the_length_13()
        {
            // Arrange
            byte[] data = 
            {
                0x60, 0x82, 0x40, 0x77, 0x8a, 0x0e, 0xe4, 0xd5,
                0x85, 0x1f, 0xa6, 0x86, 0x34,
            };
            Span<byte> span = new Span<byte>(data);
            ReadOnlySpan<byte> rspan = new ReadOnlySpan<byte>(data);
            
            // Act
            ulong hash1 = xxHash64.ComputeHash(data, data.Length);
            ulong hash2 = xxHash64.ComputeHash(span, span.Length);
            ulong hash3 = xxHash64.ComputeHash(rspan, rspan.Length);
            
            // Assert
            Assert.Equal(hash1, (ulong) 0x9d1cb0d181d58bee);
            Assert.Equal(hash2, (ulong) 0x9d1cb0d181d58bee);
            Assert.Equal(hash3, (ulong) 0x9d1cb0d181d58bee);
        }
        
        [Fact]
        public void Compute_hash64_for_the_length_32()
        {
            // Arrange
            byte[] data = 
            {
                0x60, 0x82, 0x40, 0x77, 0x8a, 0x0e, 0xe4, 0xd5,
                0x85, 0x1f, 0xa6, 0x86, 0x34, 0x01, 0xd7, 0xf2,
                0x30, 0x5d, 0x84, 0x54, 0x15, 0xf9, 0xbd, 0x03,
                0x4b, 0x0f, 0x90, 0x4e, 0xf5, 0x57, 0x21, 0x21,
            };
            Span<byte> span = new Span<byte>(data);
            ReadOnlySpan<byte> rspan = new ReadOnlySpan<byte>(data);
            
            // Act
            ulong hash1 = xxHash64.ComputeHash(data, data.Length);
            ulong hash2 = xxHash64.ComputeHash(span, span.Length);
            ulong hash3 = xxHash64.ComputeHash(rspan, rspan.Length);
            
            // Assert
            Assert.Equal(hash1, (ulong) 0x9233096b7804e12c);
            Assert.Equal(hash2, (ulong) 0x9233096b7804e12c);
            Assert.Equal(hash3, (ulong) 0x9233096b7804e12c);
        }
        
        [Fact]
        public void Compute_hash64_for_the_length_64()
        {
            // Arrange
            byte[] data = 
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
            Span<byte> span = new Span<byte>(data);
            ReadOnlySpan<byte> rspan = new ReadOnlySpan<byte>(data);
            
            // Act
            ulong hash1 = xxHash64.ComputeHash(data, data.Length);
            ulong hash2 = xxHash64.ComputeHash(span, span.Length);
            ulong hash3 = xxHash64.ComputeHash(rspan, rspan.Length);
            
            // Assert
            Assert.Equal(hash1, (ulong) 0x4c0a65b1ef9ea060);
            Assert.Equal(hash2, (ulong) 0x4c0a65b1ef9ea060);
            Assert.Equal(hash3, (ulong) 0x4c0a65b1ef9ea060);
        }
        
        [Fact]
        public void Compute_hash64_for_the_stream_1()
        {
            // Arrange
            byte[] data = {0x60};
            
            // Act
            ulong hash = xxHash64.ComputeHash(new MemoryStream(data));
            
            // Assert
            Assert.Equal(hash, (ulong) 0xb3e7ca6ca5ba3445);
        }
        
        [Fact]
        public void Compute_hash64_for_the_stream_5()
        {
            // Arrange
            byte[] data = {0x60, 0x82, 0x40, 0x77, 0x8a};
            
            // Act
            ulong hash = xxHash64.ComputeHash(new MemoryStream(data));
            
            // Assert
            Assert.Equal(hash, (ulong) 0x917b11ed024938fc);
        }
        
        [Fact]
        public void Compute_hash64_for_the_stream_13()
        {
            // Arrange
            byte[] data = 
            {
                0x60, 0x82, 0x40, 0x77, 0x8a, 0x0e, 0xe4, 0xd5,
                0x85, 0x1f, 0xa6, 0x86, 0x34,
            };
            
            // Act
            ulong hash = xxHash64.ComputeHash(new MemoryStream(data));
            
            // Assert
            Assert.Equal(hash, (ulong) 0x9d1cb0d181d58bee);
        }
        
        [Fact]
        public void Compute_hash64_for_the_stream_32()
        {
            // Arrange
            byte[] data = 
            {
                0x60, 0x82, 0x40, 0x77, 0x8a, 0x0e, 0xe4, 0xd5,
                0x85, 0x1f, 0xa6, 0x86, 0x34, 0x01, 0xd7, 0xf2,
                0x30, 0x5d, 0x84, 0x54, 0x15, 0xf9, 0xbd, 0x03,
                0x4b, 0x0f, 0x90, 0x4e, 0xf5, 0x57, 0x21, 0x21,
            };
            
            // Act
            ulong hash = xxHash64.ComputeHash(new MemoryStream(data));
            
            // Assert
            Assert.Equal(hash, (ulong) 0x9233096b7804e12c);
        }
        
        [Fact]
        public void Compute_hash64_for_the_stream_64()
        {
            // Arrange
            byte[] data = 
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
            
            // Act
            ulong hash = xxHash64.ComputeHash(new MemoryStream(data));
            
            // Assert
            Assert.Equal(hash, (ulong) 0x4c0a65b1ef9ea060);
        }
        
        
        [Fact]
        public void Compute_hash64_for_the_random_stream()
        {
            // Arrange
            int[] size =
            {
                1024,
                1024 * 3,
                1024 * 3 + 1,
                1024 * 3 + 5,
                1024 * 3 + 13,
                1024 * 3 + 32,
                1024 * 3 + 41
            };
            var random = new Random();

            for (int i = 0; i < size.Length; i++)
            {
                // Arrange
                var data = new byte[size[i]];
                random.NextBytes(data);
                
                // Act
                ulong hash1 = xxHash64.ComputeHash(data, data.Length);
                ulong hash2 = xxHash64.ComputeHash(new MemoryStream(data), 64);
                
                // Assert
                Assert.Equal(hash1, hash2);
            }
        }
        
        [Fact]
        public async Task Compute_hash64_for_the_async_stream_1()
        {
            // Arrange
            byte[] data = {0x60};
            
            // Act
            ulong hash = await xxHash64.ComputeHashAsync(new MemoryStream(data));
            
            // Assert
            Assert.Equal(hash, (ulong) 0xb3e7ca6ca5ba3445);
        }
        
        [Fact]
        public async Task Compute_hash64_for_the_async_stream_5()
        {
            // Arrange
            byte[] data = {0x60, 0x82, 0x40, 0x77, 0x8a};
            
            // Act
            ulong hash = await xxHash64.ComputeHashAsync(new MemoryStream(data));
            
            // Assert
            Assert.Equal(hash, (ulong) 0x917b11ed024938fc);
        }
        
        [Fact]
        public async Task Compute_hash64_for_the_async_stream_13()
        {
            // Arrange
            byte[] data = 
            {
                0x60, 0x82, 0x40, 0x77, 0x8a, 0x0e, 0xe4, 0xd5,
                0x85, 0x1f, 0xa6, 0x86, 0x34,
            };
            
            // Act
            ulong hash = await xxHash64.ComputeHashAsync(new MemoryStream(data));
            
            // Assert
            Assert.Equal(hash, (ulong) 0x9d1cb0d181d58bee);
        }
        
        [Fact]
        public async Task Compute_hash64_for_the_async_stream_32()
        {
            // Arrange
            byte[] data = 
            {
                0x60, 0x82, 0x40, 0x77, 0x8a, 0x0e, 0xe4, 0xd5,
                0x85, 0x1f, 0xa6, 0x86, 0x34, 0x01, 0xd7, 0xf2,
                0x30, 0x5d, 0x84, 0x54, 0x15, 0xf9, 0xbd, 0x03,
                0x4b, 0x0f, 0x90, 0x4e, 0xf5, 0x57, 0x21, 0x21,
            };
            
            // Act
            ulong hash = await xxHash64.ComputeHashAsync(new MemoryStream(data));
            
            // Assert
            Assert.Equal(hash, (ulong) 0x9233096b7804e12c);
        }
        
        [Fact]
        public async Task Compute_hash64_for_the_async_stream_64()
        {
            // Arrange
            byte[] data = 
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
            
            // Act
            ulong hash = await xxHash64.ComputeHashAsync(new MemoryStream(data));
            
            // Assert
            Assert.Equal(hash, (ulong) 0x4c0a65b1ef9ea060);
        }
        
        [Fact]
        public async Task Compute_hash64_for_the_async_stream_64_with_cancelation_token()
        {
            // Arrange
            byte[] data = 
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
            CancellationTokenSource tokenSource = new CancellationTokenSource();
            
            // Act
            tokenSource.Cancel();
            
            // Assert
            await Assert.ThrowsAsync<TaskCanceledException>(async() =>
            {
                await xxHash64.ComputeHashAsync(new MemoryStream(data), 8192, 0, tokenSource.Token);
            });
        }
        
        [Fact]
        public async Task Compute_hash64_for_the_async_random_stream()
        {
            // Arrange
            int[] size =
            {
                //1024,
                1024 * 3,
                1024 * 3 + 1,
                1024 * 3 + 5,
                1024 * 3 + 13,
                1024 * 3 + 32,
                1024 * 3 + 41
            };
            var random = new Random();

            for (int i = 0; i < size.Length; i++)
            {
                // Arrange
                var data = new byte[size[i]];
                random.NextBytes(data);
                
                // Act
                ulong hash1 = xxHash64.ComputeHash(data, data.Length);
                ulong hash2 = await xxHash64.ComputeHashAsync(new MemoryStream(data), 64);
                
                // Assert
                Assert.Equal(hash1, hash2);
            }
        }
        
    }
}