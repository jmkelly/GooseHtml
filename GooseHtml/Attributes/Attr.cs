namespace HtmlBuilder.Tests;

public record Attribute(string Name,
                    string Value)
	{
		public override string ToString()
		{
			return $"{Name}=\"{Value}\"";
		}
	}

