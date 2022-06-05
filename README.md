<p align="center">
  <a href="#" target="_blank" rel="noopener noreferrer">
    <img width="550" src="https://user-images.githubusercontent.com/1567570/39971158-5b213cca-56ff-11e8-9a1e-6c717e95d092.png" alt="xxHash.st">
  </a>
</p>
<p align="center">
  Extremely fast non-cryptographic hash algorithm <a href="http://www.xxhash.com/" target="_blank">xxhash</a>
</p>
<br>
<p align="center">
  <a href="https://ci.appveyor.com/project/uranium62/xxhash">
    <img src="https://ci.appveyor.com/api/projects/status/j5gkm2rvxwu4gu3q?svg=true" alt="build" />
  </a>
  <a href="https://codecov.io/gh/uranium62/xxHash">
    <img src="https://codecov.io/gh/uranium62/xxHash/branch/master/graph/badge.svg" alt="coverage"/>
  </a>
  <a href="https://www.nuget.org/packages/Standart.Hash.xxHash">
    <img src="https://img.shields.io/badge/nuget-1.0.6-green.svg?style=flat-square" alt="nuget"/>
  </a>
  <a href="https://www.nuget.org/packages/Standart.Hash.xxHash">
    <img src="https://img.shields.io/badge/platform-x64-blue.svg?longCache=true" alt="platform"/>
  </a>
  <a href="https://github.com/uranium62/xxHash/blob/master/LICENSE">
    <img src="https://img.shields.io/badge/License-MIT-yellow.svg" alt="license" />
  </a>
</p>

xxHash is an Extremely fast Hash algorithm, running at RAM speed limits. It successfully completes the **SMHasher** test suite which evaluates collision, dispersion and randomness qualities of hash functions.

## Instalation
```
PM> Install-Package Standart.Hash.xxHash
```

## Benchmarks
This benchmark was launched on a **Windows 10.0.19044.1706 (21H2)**. The reference system uses a **AMD Ryzen 7 2700, 1 CPU, 16 logical and 8 physical cores**
```
BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19044.1706 (21H2)
AMD Ryzen 7 2700, 1 CPU, 16 logical and 8 physical cores
.NET SDK=6.0.300
  [Host]     : .NET 6.0.5 (6.0.522.21309), X64 RyuJIT
  Job-HQVLOG : .NET 6.0.5 (6.0.522.21309), X64 RyuJIT
Runtime=.NET 6.0  
```

| Method         |       x64  |
|:---------------|----------:|
| Hash32 Array   | 5.87 GB/s  |
| Hash64 Array   | 9.07 GB/s  |
| Hash128 Array  | 12.04 GB/s |
| Hash32 Span    | 5.87 GB/s  |
| Hash64 Span    | 9.07 GB/s  |
| Hash128 Span   | 12.04 GB/s |
| Hash32 Stream  | 3.22 GB/s  |
| Hash64 Stream  | 4.81 GB/s  |

## Comparison between С# and C implementation

| Method             | Platform | Language |  1KB Time |  1MB Time |  1GB Time |     Speed  |
|:-------------------|---------:|---------:|----------:|----------:|----------:|-----------:|
| Hash32             |      x64 |       C# |  151.5 ns |  143.4 us |  170.3 ms | 5.87 GB/s  |
| Hash32             |      x64 |       C  |  138.5 ns |  129.5 us |  152.4 ms | 6.56 GB/s  |
| Hash64             |      x64 |       C# |  84.6 ns  |   77.9 us |  110.2 ms | 9.07 GB/s  |
| Hash64             |      x64 |       C  |  74.2 ns  |   64.8 us |  83.0 ms  | 12.04 GB/s |
| Hash128 (SSE2/AVX2)|      x64 |       C# |  151.6 ns |   64.5 us |  80.5 ms  | 12.04 GB/s |
| Hash128 (SSE2/AVX2)|      x64 |       C  |  84.4 ns  |   38.3 us |  57.4 ms  | 17.42 GB/s |


## Api
```cs
public static uint ComputeHash(byte[] data, int length, uint seed = 0) { throw null; }
public static uint ComputeHash(Span<byte> data, int length, uint seed = 0) { throw null; }
public static uint ComputeHash(Stream stream, int bufferSize = 4096, uint seed = 0) { throw null; }
public static async ValueTask<uint> ComputeHashAsync(Stream stream, int bufferSize = 4096, uint seed = 0) { throw null; }
public static uint ComputeHash(string str, uint seed = 0) { throw null; }


public static ulong ComputeHash(byte[] data, int length, ulong seed = 0) { throw null; }
public static ulong ComputeHash(Span<byte> data, int length, ulong seed = 0) { throw null; }
public static ulong ComputeHash(Stream stream, int bufferSize = 8192, ulong seed = 0) { throw null; }
public static async ValueTask<ulong> ComputeHashAsync(Stream stream, int bufferSize = 8192, ulong seed = 0) { throw null; }
public static ulong ComputeHash(string str, uint seed = 0) { throw null; }

public static uint128 ComputeHash(byte[] data, int length, uint seed = 0) { throw null; }
public static uint128 ComputeHash(Span<byte> data, int length, uint seed = 0) { throw null; }
public static uint128 ComputeHash(string str, uint seed = 0) { throw null; }

// allocations
public static byte[] ComputeHashBytes(byte[] data, int length, uint seed = 0) { throw null; }
public static byte[] ComputeHashBytes(Span<byte> data, int length, uint seed = 0) { throw null; }
public static byte[] ComputeHashBytes(string str, uint seed = 0) { throw null; }

```

## Examples
A few examples of how to use api
```cs
byte[] data = Encoding.UTF8.GetBytes("veni vidi vici");

ulong h64_1 = xxHash64.ComputeHash(data, data.Length);
ulong h64_2 = xxHash64.ComputeHash(new Span<byte>(data), data.Length);
ulong h64_3 = xxHash64.ComputeHash(new ReadOnlySpan<byte>(data), data.Length);
ulong h64_4 = xxHash64.ComputeHash(new MemoryStream(data));
ulong h64_5 = await xxHash64.ComputeHashAsync(new MemoryStream(data));
ulong h64_6 = xxHash64.ComputeHash("veni vidi vici");

uint h32_1 = xxHash32.ComputeHash(data, data.Length);
uint h32_2 = xxHash32.ComputeHash(new Span<byte>(data), data.Length);
uint h32_3 = xxHash32.ComputeHash(new ReadOnlySpan<byte>(data), data.Length);
uint h32_4 = xxHash32.ComputeHash(new MemoryStream(data));
uint h32_5 = await xxHash32.ComputeHashAsync(new MemoryStream(data));
uint h32_6 = xxHash32.ComputeHash("veni vidi vici");

uint128 h128_1 = xxHash128.ComputeHash(data, data.Length);
uint128 h128_2 = xxHash128.ComputeHash(new Span<byte>(data), data.Length);
uint128 h128_3 = xxHash128.ComputeHash(new ReadOnlySpan<byte>(data), data.Length);
uint128 h128_4 = xxHash128.ComputeHash("veni vidi vici");

Guid guid    = h128_1.ToGuid();
byte[] bytes = h128_1.ToBytes();

byte[] hash_bytes_1 = xxHash128.ComputeHashBytes(data, data.Length);
byte[] hash_bytes_2 = xxHash128.ComputeHashBytes(new Span<byte>(data), data.Length);
byte[] hash_bytes_3 = xxHash128.ComputeHashBytes(new ReadOnlySpan<byte>(data), data.Length);
byte[] hash_bytes_4 = xxHash128.ComputeHashBytes("veni vidi vici");

```
---
<p align="center">
Made in :beginner: Ukraine with :heart:
</p>
