namespace GooseHtml.Attributes;

public record Pattern(string Value): Attribute("pattern", Value);

