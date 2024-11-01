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

