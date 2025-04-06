namespace GooseHtml;
public class P: Element
{
    public P(): base(ElementNames.P)
    {
    }

    public P(Text value): base(ElementNames.P)
	{
		Add(value);
	}

	public P(string value): base(ElementNames.P)
	{
		Add(new Text(value));
	}

}
