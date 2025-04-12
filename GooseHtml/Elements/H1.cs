namespace GooseHtml;

public class H1 : Element
{
    public H1(): base(ElementNames.H1)
    {
    }

    public H1(Text value) : base(ElementNames.H1)
    {
		Add(value);
    }

	public H1(string value): base(ElementNames.H1)
	{
		Add(new Text(value));
	}

}

