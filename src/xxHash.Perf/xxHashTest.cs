namespace xxHash.Perf
{
    using System;
    using BenchmarkDotNet.Attributes;
    using BenchmarkDotNet.Attributes.Columns;
    using BenchmarkDotNet.Attributes.Exporters;
    using Lib;

    [RPlotExporter, RankColumn]
    [MinColumn, MaxColumn]
    [MemoryDiagnoser]
    [DisassemblyDiagnoser(printAsm: true, printSource: true)]
    public class xxHashTest
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
        public uint Hash32()
        {
            return xxHash32.ComputeHash(data, data.Length);
        }

        [Benchmark]
        public ulong Hash64()
        {
            return xxHash64.ComputeHash(data, data.Length);
        }
    }
}
