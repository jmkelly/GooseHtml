namespace HtmlBuilder.Tests;

public abstract class Html : Element
{
	public Html() : base()
	{
	}

	public abstract Head Head();
	public abstract Body Body();
}

