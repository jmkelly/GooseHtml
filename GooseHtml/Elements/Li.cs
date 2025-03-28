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

	public Li(Either<Element, VoidElement>[] elements) : base("li")
	{
		AddRange(elements);
	}
}

