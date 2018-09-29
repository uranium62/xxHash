namespace Standart.Hash.xxHash.Perf
{
    using BenchmarkDotNet.Running;

    public class Program
    {
        public static void Main(string[] args)
        {
            var summary = BenchmarkRunner.Run<NativeXXHashBenchmark>();
        }
    }
}