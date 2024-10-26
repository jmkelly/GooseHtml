namespace GooseHtml.Attributes;

public record TitleAttribute(string Value): Attribute("title", Value);

