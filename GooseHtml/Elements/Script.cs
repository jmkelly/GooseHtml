namespace GooseHtml;

using GooseHtml.Attributes;

public class Script : Element
{
	public Script() : base("script")
	{
	}

	public Script(Text text): base()
	{
		Add(text);
	}

	public Script(string src) : base("script", new [] {new Attribute("src", src)})
	{
	}

	public Script(Attribute[] attributes) : base("script", attributes)
	{
	}

	public Script(string src, string integrity, string crossorigin) : base("script", [new Attribute("src", src), new Attribute("integrity", integrity), new Attribute("crossorigin", crossorigin)])
	{
	}

}

public class HtmxScript : Script
{
	public HtmxScript() : base("https://unpkg.com/htmx.org@2.0.3", "sha384-0895/pl2MU10Hqc6jd4RvrthNlDiE9U1tWmX7WRESftEDRosgxNsQG/Ze9YMRzHq", "anonymous")
	{
		Version = "2.0.3";
	}

	public string Version {get;}
}

