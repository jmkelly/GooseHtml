namespace GooseHtml;

public class Li : Element
{

	public Li(): base(ElementNames.Li)
	{
	}

	public Li(Text value) : base(ElementNames.Li)
	{
		Add(value);
	}

	public Li(List<Either<Element, VoidElement>> elements) : base(ElementNames.Li)
	{
		AddRange(elements);
	}
}

