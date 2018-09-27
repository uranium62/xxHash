namespace Standart.Hash.xxHash.Perf
{
    using System;
    using BenchmarkDotNet.Attributes;
    using BenchmarkDotNet.Attributes.Columns;
    using BenchmarkDotNet.Attributes.Exporters;
    
    [RPlotExporter, RankColumn]
    [MinColumn, MaxColumn]
    [MemoryDiagnoser]
    public class xxBufferBenchmark
    {
        private byte[] src;
        private byte[] des;
        
        [GlobalSetup]
        public void Setup()
        {
            src = new byte[32];
            des = new byte[32];
        }

        [Benchmark]
        public void ArrayCopy()
        {
            Array.Copy(src, 0, des, 0, 32);
        }
        
        [Benchmark]
        public void BufferCopy()
        {
            Buffer.BlockCopy(src, 0, des, 0, 32);
        }
        
        [Benchmark]
        public void xxBufferCopy()
        {
            xxBuffer.BlockCopy(src, 0, des, 0, 32);
        }
    }
}