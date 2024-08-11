namespace GooseHtml.Attributes;

public record Novalidate(string value): Attribute("novalidate", value);

