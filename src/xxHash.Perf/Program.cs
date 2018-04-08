namespace xxHash.Perf
{
    using BenchmarkDotNet.Running;

    class Program
    {
        static void Main(string[] args)
        {
            var summary = BenchmarkRunner.Run<xxHashTest>();
        }
    }
}
