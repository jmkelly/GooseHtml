namespace GooseHtml.Attributes;

public record Accept(string Value) : Attribute("accept", Value);

