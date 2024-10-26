namespace GooseHtml.Attributes;

public record Action(string Value) : Attribute("action", Value)
{
}
