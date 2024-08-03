namespace GooseHtml.Attributes;

public record Required(string value): Attribute("required", value);

