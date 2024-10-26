namespace GooseHtml.Attributes;

public record Required(string Value): Attribute("required", Value);

