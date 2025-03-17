namespace GooseHtml;
public class H : Element
{
    public H()
    {
    }

    public H(Text value): base()
	{
		Add(value);
	}

	public H(string value): base()
	{
		Add(new Text(value));
	}
}


