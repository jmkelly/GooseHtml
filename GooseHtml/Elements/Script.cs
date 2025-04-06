namespace GooseHtml;

using GooseHtml.Attributes;

public class Script : Element
{
	public Script() : base(ElementNames.Script)
	{
	}

	public Script(Text text): base(ElementNames.Script)
	{
		Add(text);
	}

	public Script(string src) : base(ElementNames.Script, [new Attribute("src", src)])
	{
	}

	public Script(Attribute[] attributes) : base(ElementNames.Script, attributes)
	{
	}

	public Script(string src, string integrity, string crossorigin) : base(ElementNames.Script, [new Attribute("src", src), new Attribute("integrity", integrity), new Attribute("crossorigin", crossorigin)])
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

