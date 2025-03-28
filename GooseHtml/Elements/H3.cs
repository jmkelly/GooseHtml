namespace GooseHtml;

public class H3 : Element
{
    public H3(): base(ElementNames.H3)
    {
    }

    public H3(Text value) : base(ElementNames.H3)
    {
		Add(value);
    }
public H3(string value): base(ElementNames.H3)
	{
		Add(new Text(value));
	}
}
