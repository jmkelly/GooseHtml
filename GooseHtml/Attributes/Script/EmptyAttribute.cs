
namespace GooseHtml.Attributes;

public abstract class EmptyAttribute(string Name) 
{
	public override string ToString()
	{
		return Name;
	}
}
