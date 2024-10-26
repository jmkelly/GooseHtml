namespace GooseHtml.Attributes;

public record Preload(string Value): Attribute("preload", Value);

