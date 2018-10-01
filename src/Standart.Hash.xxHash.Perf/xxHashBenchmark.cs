namespace Standart.Hash.xxHash.Perf
{
    using System;
    using System.IO;
    using System.Threading.Tasks;
    using BenchmarkDotNet.Attributes;

    [RPlotExporter, RankColumn]
    [MinColumn, MaxColumn]
    [MemoryDiagnoser]
    //[DisassemblyDiagnoser(printAsm: true, printSource: true)]
    public class xxHashBenchmark
    {
        const int KB = 1024;
        const int MB = 1024 * KB;
        const int GB = 1024 * MB;

        private byte[] data;
        private MemoryStream stream;

        [Params(KB, MB, GB)]
        public int N;

        [GlobalSetup]
        public void Setup()
        {
            Random rand = new Random(42);

            data = new byte[N];
            rand.NextBytes(data);
            stream = new MemoryStream(data);
        }
        
        [Benchmark]
        public uint Hash32_Array()
        {
            return xxHash32.ComputeHash(data, data.Length);
        }

        [Benchmark]
        public uint Hash32_Span()
        {
            Span<byte> span = new Span<byte>(data);
            return xxHash32.ComputeHash(span, span.Length);
        }

        [Benchmark]
        public uint Hash32_Stream()
        {
            stream.Seek(0, SeekOrigin.Begin);
            return xxHash32.ComputeHash(stream);
        }
        
        [Benchmark]
        public async Task<uint> Hash32_StreamAsync()
        {
            stream.Seek(0, SeekOrigin.Begin);
            return await xxHash32.ComputeHashAsync(stream);
        }
        
        [Benchmark]
        public ulong Hash64_Array()
        {
            return xxHash64.ComputeHash(data, data.Length);
        }
        
        [Benchmark]
        public ulong Hash64_Span()
        {
            Span<byte> span = new Span<byte>(data);
            return xxHash64.ComputeHash(span, span.Length);
        }
        
        [Benchmark]
        public ulong Hash64_Stream()
        {
            stream.Seek(0, SeekOrigin.Begin);
            return xxHash64.ComputeHash(stream);
        }
        
        [Benchmark]
        public async Task<ulong> Hash64_StreamAsync()
        {
            stream.Seek(0, SeekOrigin.Begin);
            return await xxHash64.ComputeHashAsync(stream);
        }
    }
}