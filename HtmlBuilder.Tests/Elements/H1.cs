namespace HtmlBuilder.Tests;

public class H1 : Element
{
	public H1(Text value) : base(value, selfClosing:true )
	{
	}
}


public class Input : Element
{
	public Input(): base(selfClosing:false)
	{

	}
}
