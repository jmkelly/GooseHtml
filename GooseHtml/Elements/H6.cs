namespace GooseHtml;
public class H6 : Element
{
	public H6(): base(ElementNames.H6)
	{
	}

	public H6(Text value) : base(ElementNames.H6)
	{
		Add(value);
	}

	public H6(string value): base(ElementNames.H6)
	{
		Add(new Text(value));
	}
}
