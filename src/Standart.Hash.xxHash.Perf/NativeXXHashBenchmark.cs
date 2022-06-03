namespace Standart.Hash.xxHash.Perf
{
    using System;
    using BenchmarkDotNet.Attributes;
    
    [RPlotExporter, RankColumn]
    [MinColumn, MaxColumn]
    [MemoryDiagnoser]
    [CoreJob]
    //[DisassemblyDiagnoser(printAsm: true, printSource: true)]
    public class NativeXXHashBenchmark
    {
        const int KB = 1024;
        const int MB = 1024 * KB;
        const int GB = 1024 * MB;

        private byte[] data;

        [Params(KB, MB, GB)]
        public int N;

        [GlobalSetup]
        public void Setup()
        {
            data = new byte[N];
            new Random(42).NextBytes(data);
        }

        [Benchmark]
        public uint Hash32_Array()
        {
            return xxHash32.ComputeHash(data, data.Length);
        }

        [Benchmark]
        public uint Hash32_Native()
        {
            return xxHashNative.ComputeXXH32(data, (ulong)data.Length);
        }

        [Benchmark]
        public ulong Hash64_Array()
        {
            return xxHash64.ComputeHash(data, data.Length);
        }

        [Benchmark]
        public ulong Hash64_Native()
        {
            return xxHashNative.ComputeXXH64(data, (ulong)data.Length);
        }

        [Benchmark]
        public uint128 Hash128_Array()
        {
            return xxHash128.ComputeHash(data, data.Length);
        }

        [Benchmark]
        public XXH128_hash_t Hash128_Native()
        {
            return xxHashNative.ComputeXXH128(data, (ulong)data.Length);
        }

    }
}