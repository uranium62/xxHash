### xxHash
Extremely fast non-cryptographic hash algorithm http://www.xxhash.com/

### Benchmark
``` ini

BenchmarkDotNet=v0.10.13, OS=Windows 10 Redstone 3 [1709, Fall Creators Update] (10.0.16299.309)
Intel Core i7-4700MQ CPU 2.40GHz (Haswell), 1 CPU, 8 logical cores and 4 physical cores
Frequency=2338346 Hz, Resolution=427.6527 ns, Timer=TSC
  [Host]     : .NET Framework 4.7.1 (CLR 4.0.30319.42000), 64bit RyuJIT-v4.7.2633.0
  DefaultJob : .NET Framework 4.7.1 (CLR 4.0.30319.42000), 64bit RyuJIT-v4.7.2633.0


```
### Platform (x64)
| Method |      Bytes |             Mean |              Min |              Max | Allocated |
|------- |----------- |-----------------:|-----------------:|-----------------:|----------:|
| **Hash32** |       **1024** |         **179.1 ns** |         **176.9 ns** |         **181.5 ns** |       **0 B** |
| Hash64 |       1024 |         112.8 ns |         112.5 ns |         113.1 ns |       0 B |
| **Hash32** |    **1048576** |     **167,038.5 ns** |     **164,833.0 ns** |     **168,950.2 ns** |       **0 B** |
| Hash64 |    1048576 |      83,941.7 ns |      82,805.5 ns |      86,072.0 ns |       0 B |
| **Hash32** | **1073741824** | **198,183,743.4 ns** | **197,721,867.3 ns** | **198,782,071.8 ns** |       **0 B** |
| Hash64 | 1073741824 | 112,638,993.1 ns | 111,995,167.3 ns | 113,573,740.3 ns |       0 B |

### Platform (x86)
| Method |      Bytes |               Mean |                Min |                Max | Allocated |
|------- |----------- |-------------------:|-------------------:|-------------------:|----------:|
| **Hash32** |       **1024** |           **354.9 ns** |           **354.5 ns** |           **355.5 ns** |       **0 B** |
| Hash64 |       1024 |         1,380.3 ns |         1,361.0 ns |         1,398.3 ns |       0 B |
| **Hash32** |    **1048576** |       **357,399.4 ns** |       **356,245.1 ns** |       **358,044.0 ns** |       **0 B** |
| Hash64 |    1048576 |     1,343,269.5 ns |     1,340,923.2 ns |     1,345,356.7 ns |       0 B |
| **Hash32** | **1073741824** |   **365,925,742.3 ns** |   **360,332,613.3 ns** |   **369,159,819.6 ns** |       **0 B** |
| Hash64 | 1073741824 | 1,388,585,469.4 ns | 1,368,307,867.2 ns | 1,428,744,362.7 ns |       0 B |

### Speed
| Method |       x86 |       x64 |
|-------:|----------:|----------:|
| Hash32 | 2.73 GB/s | 720  MB/s |
| Hash64 | 5.05 GB/s | 8.92 GB/s |

### How to use
``` cs
	byte[] data = Encoding.UTF8.GetBytes("veni vidi vici");

	uint hash64 = xxHash64.ComputeHash(data, data.Length);
	ulong hash32 = xxHash32.ComputeHash(data, data.Length);

```