namespace GooseHtml.Attributes;

public record Lang(string value): Attribute("lang", value);
namespace GooseHtml.Attributes;

public record Lang(): Attribute("lang", "en-US")
{
}

