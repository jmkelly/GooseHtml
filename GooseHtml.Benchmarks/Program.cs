using AngleSharp;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Diagnostics.Windows.Configs;
using BenchmarkDotNet.Running;
using GooseHtml;
using MariGold.HtmlParser;

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
    private IBrowsingContext context;
    private string html;

    [GlobalSetup] // Runs once before benchmarking
    public void Setup()
    {
        //_htmlFiles.Add(Path.GetFileName("developer.mozilla.org__2025-03.html"), File.ReadAllText("developer.mozilla.org__2025-03.html"));

        IConfiguration config = Configuration.Default;
        html =  File.ReadAllText("www.cnn.com__2025-04.html");
        //Create a new context for evaluating webpages with the given config
        context = BrowsingContext.New(config);
    }
 

    [Benchmark(Baseline = true)]
    public void ParseGooseHtml()
    {
        var parser = new GooseHtml.HtmlParser(html); 
        parser.Parse();
    }

    [Benchmark]
    public void ParseMonkeyHtml()
    {
        SoftCircuits.HtmlMonkey.HtmlDocument.FromHtml(html);
    }

    [Benchmark]
    public void ParseAgilityPack()
    {
        var doc = new HtmlAgilityPack.HtmlDocument();
        doc.LoadHtml(html);
    }

    [Benchmark]
    public async Task ParseAngleSharp()
    {
        await context.OpenAsync(req => req.Content(html));
    }

    [Benchmark]
    public void ParseMarigold()
    {
        var parser = new HtmlTextParser(html);
        parser.Parse();
    }


}