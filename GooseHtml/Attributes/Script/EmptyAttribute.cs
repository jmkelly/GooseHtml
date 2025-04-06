namespace GooseHtml.Attributes;

public class EmptyAttribute(string Name)  : Attribute(Name, string.Empty)
{
	public override string ToString()
	{
		return Name;
	}
}
