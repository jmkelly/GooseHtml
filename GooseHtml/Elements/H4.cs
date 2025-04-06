namespace GooseHtml;
public class H4 : Element
{
	public H4(): base(ElementNames.H4)
	{
	}

	public H4(Text value) : base(ElementNames.H4)
	{
		Add(value);
	}

	public H4(string value): base(ElementNames.H4)
	{
		Add(new Text(value));
	}
}
