namespace GooseHtml.Attributes;

public record AsyncAttribute(string value) : Attribute("async", value);

