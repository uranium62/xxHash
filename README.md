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
    <img src="https://img.shields.io/badge/nuget-1.0.5-green.svg?style=flat-square" alt="nuget"/>
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
This benchmark was launched on a **Windows 10 (10.0.16299.309)**. The reference system uses a **Intel Core i7-4700MQ CPU 2.40GHz (Haswell)**

| Method        |       x64 |
|---------------|----------:|
| Hash32 Array  | 5.05 GB/s |
| Hash64 Array  | 8.92 GB/s |
| Hash32 Span   | 5.05 GB/s |
| Hash64 Span   | 8.92 GB/s |
| Hash32 Stream | 3.22 GB/s |
| Hash64 Stream | 4.81 GB/s |

## Comparison between С# and C implementation

| Method        |      Time |
|---------------|----------:|
| Hash32 C# 1KB |  185.1 ns |
| Hash32 C  1KB |  183.5 ns |
| Hash64 C# 1KB |  117.3 ns |
| Hash64 C  1KB |  104.8 ns |
| Hash32 C# 1MB |  170.6 us |
| Hash32 C  1MB |  170.1 us |
| Hash64 C# 1MB |   87.1 us |
| Hash64 C  1MB |   85.3 us |
| Hash32 C# 1GB |  193.6 ms |
| Hash32 C  1GB |  190.8 ms |
| Hash64 C# 1GB |  116.9 ms |
| Hash64 C  1GB |  114.1 ms |

## Defference

| Method | Platform | Language | Size |      Time |     Speed | Difference |
|--------|---------:|---------:|-----:|----------:|----------:|-----------:|      
| Hash32 |      x64 |       C# |  1GB |  193.6 ms | 5.16 GB/s |      1.4 % |
| Hash32 |      x64 |       C  |  1GB |  190.8 ms | 5.24 GB/s |      1.4 % |
| Hash64 |      x64 |       C# |  1GB |  116.9 ms | 8.55 GB/s |      2.4 % |
| Hash64 |      x64 |       C  |  1GB |  114.1 ms | 8.76 GB/s |      2.4 % |

## Api
```cs
public static uint ComputeHash(byte[] data, int length, uint seed = 0) { throw null; }
public static uint ComputeHash(Span<byte> data, int length, uint seed = 0) { throw null; }
public static uint ComputeHash(Stream stream, int bufferSize = 4096, uint seed = 0) { throw null; }
public static async Task<uint> ComputeHashAsync(Stream stream, int bufferSize = 4096, uint seed = 0) { throw null; }

public static ulong ComputeHash(byte[] data, int length, ulong seed = 0) { throw null; }
public static ulong ComputeHash(Span<byte> data, int length, ulong seed = 0) { throw null; }
public static ulong ComputeHash(Stream stream, int bufferSize = 8192, ulong seed = 0) { throw null; }
public static async Task<ulong> ComputeHashAsync(Stream stream, int bufferSize = 8192, ulong seed = 0) { throw null; }
```

## Examples
A few examples of how to use api
```cs
byte[] data = Encoding.UTF8.GetBytes("veni vidi vici");

ulong h64_1 = xxHash64.ComputeHash(data, data.Length);
ulong h64_2 = xxHash64.ComputeHash(new MemoryStream(data));
ulong h64_3 = await xxHash64.ComputeHashAsync(new MemoryStream(data));

uint h32_1 = xxHash32.ComputeHash(data, data.Length);
uint h32_2 = xxHash32.ComputeHash(new MemoryStream(data));
uint h32_3 = await xxHash32.ComputeHashAsync(new MemoryStream(data));
```
---
<p align="center">
Made in :beginner: Ukraine with :heart:
</p>
