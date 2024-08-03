namespace GooseHtml.Attributes;

public record Attribute(string Name,
		string Value)
{
	public sealed override string ToString()
	{
		return $"{Name}=\"{Value}\"";
	}
}
