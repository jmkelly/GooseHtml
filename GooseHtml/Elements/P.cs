namespace GooseHtml;
public class P: Element
{
	public P(): base(){}

	public P(Text value): base()
	{
		Add(value);
	}
}
