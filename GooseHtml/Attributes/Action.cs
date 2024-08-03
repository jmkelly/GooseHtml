namespace GooseHtml.Attributes;

public record Action(string value) : Attribute("action", value)
{
}
