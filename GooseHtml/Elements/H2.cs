namespace GooseHtml;

public class H2 : Element
{
    public H2(): base(ElementNames.H2)
    {
    }

    public H2(Text value) : base(ElementNames.H2)
    {
		Add(value);
    }

	public H2(string value): base(ElementNames.H2)
	{
		Add(new Text(value));
	}
}




