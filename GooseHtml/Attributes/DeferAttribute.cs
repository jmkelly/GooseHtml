namespace GooseHtml.Attributes;

public record DeferAttribute(string Value): Attribute("defer", Value);

