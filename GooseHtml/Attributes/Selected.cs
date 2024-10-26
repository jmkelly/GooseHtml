namespace GooseHtml.Attributes;

public record Selected(string Value): Attribute("selected", Value);

