namespace GooseHtml;

public class Li : Element
{

	public Li(): base("li")
	{
	}

	public Li(Text value) : base()
	{
		Add(value);
	}

	public Li(Element[] elements) : base("li")
	{
		AddRange(elements);
	}
}



