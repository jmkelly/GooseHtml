namespace GooseHtml.Attributes;

public record AsyncAttribute(string Value) : Attribute("async", Value);

