using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Diagnostics.Windows.Configs;
using BenchmarkDotNet.Running;
using GooseHtml;

class Program
{
    static void Main(string[] args)
    {
        BenchmarkRunner.Run<HtmlParserBenchmark>();
    }
}

[EtwProfiler] 
[MemoryDiagnoser]
public class HtmlParserBenchmark
{
    private string html;

    [GlobalSetup] // Runs once before benchmarking
    public void Setup()
    {
		html = File.ReadAllText("developer.mozilla.org__2025-03.html");
    }

    [Benchmark]
    public void Parse()
    {
		var parser = new HtmlParser(html);
		parser.Parse();
    }
}