
namespace GooseHtml.Attributes;

public abstract record BooleanAttribute(string Name)
{
	public override string ToString()
	{
		return Name;
	}
}
