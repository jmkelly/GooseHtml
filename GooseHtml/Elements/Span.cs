namespace GooseHtml;

public class Span : Element
{
	public Span(Attributes.Class @class): base(ElementNames.Span)
	{
		Add(@class);
	}

	public Span(): base(ElementNames.Span)
	{

	}

	public Span(Text value): base(ElementNames.Span)
	{
		Add(value);
	}

	public Span(string value): base(ElementNames.Span)
	{
		Add(new Text(value));
	}
}

