namespace GooseHtml;
public class P: Element
{
	public P(): base(){}

	public P(Text value): base()
	{
		Add(value);
	}

	public P(string value): base()
	{
		Add(new Text(value));
	}
}
