namespace GooseHtml;
public class H5 : Element
{
	public H5(): base(ElementNames.H5)
	{
	}

	public H5(Text value) : base(ElementNames.H5)
	{
		Add(value);
	}

	public H5(string value): base(ElementNames.H5)
	{
		Add(new Text(value));
	}
}
