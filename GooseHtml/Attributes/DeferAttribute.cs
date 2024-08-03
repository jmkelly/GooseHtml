namespace GooseHtml.Attributes;

public record DeferAttribute(string value): Attribute("defer", value);

