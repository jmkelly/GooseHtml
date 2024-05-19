
namespace GooseHtml.Attributes;

public abstract record BooleanAttribute(string name)
{
	public override string ToString()
	{
		return name;
	}
}
