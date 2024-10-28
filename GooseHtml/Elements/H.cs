namespace GooseHtml;

public abstract class H : Element
{
	public H(Text value): base()
	{
		Add(value);
	}

	public H(string value): base()
	{
		Add(new Text(value));
	}
}


