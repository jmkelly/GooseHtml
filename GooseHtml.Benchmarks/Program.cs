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

[MemoryDiagnoser] // Tracks memory allocations
public class HtmlAttributeBenchmark
{
    private const int N = 100_000; // Number of attributes to create

    // Struct version (readonly for immutability)
    public readonly struct HtmlAttributeStruct(string Name, string Value)
    {
        public string Name { get; } = Name;
        public string Value { get; } = Value;

        public readonly override string ToString()
		{
			return $"{Name}=\"{Value}\"";
		}
    }

    // Class version
    public class HtmlAttributeClass(string Name, string Value)
    {
        public string Name { get; } = Name;
        public string Value { get; } = Value;

        public sealed override string ToString()
		{
			return $"{Name}=\"{Value}\"";
		}

		public sealed override bool Equals(object? obj)
		{
			return obj is HtmlAttributeClass other && Name == other.Name && Value == other.Value;
		}

        public override int GetHashCode()
        {
			return HashCode.Combine(Name, Value);
        }
    }

    private HtmlAttributeStruct[] structAttributes;
    private HtmlAttributeClass[] classAttributes;

    [GlobalSetup] // Runs once before benchmarking
    public void Setup()
    {
        structAttributes = new HtmlAttributeStruct[N];
        classAttributes = new HtmlAttributeClass[N];

        for (int i = 0; i < N; i++)
        {
            structAttributes[i] = new HtmlAttributeStruct("class", "container");
            classAttributes[i] = new HtmlAttributeClass("class", "container");
        }
    }

    [Benchmark]
    public void CreateStructAttributes()
    {
        var attribute = new HtmlAttributeStruct("class", "container");
    }

    [Benchmark]
    public void CreateClassAttributes()
    {
        var attributes = new HtmlAttributeClass("class", "container");
    }

    [Benchmark]
    public void AccessStructAttributes()
    {
        int count = 0;
        foreach (var attr in structAttributes)
        {
            if (attr.Name == "class") count++;
        }
    }

    [Benchmark]
    public void AccessClassAttributes()
    {
        int count = 0;
        foreach (var attr in classAttributes)
        {
            if (attr.Name == "class") count++;
        }
    }

    [Benchmark]
    public void ClassAttributeEquality()
    {
		classAttributes[0].Equals(classAttributes[1]);
    }

    [Benchmark]
    public void StructAttributeEquality()
    {
		structAttributes[0].Equals(structAttributes[1]);
    }

    [Benchmark]
    public void ClassAttributeToString()
    {
		classAttributes[0].ToString();
    }

    [Benchmark]
    public void StructAttributeToString()
    {
		structAttributes[0].ToString();
    }
}


