namespace GooseHtml.Attributes;

public record Charset(string Value):Attribute("charset", Value)
{
}

