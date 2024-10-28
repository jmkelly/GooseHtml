namespace GooseHtml.Attributes;

public record HttpEquiv(string Value): Attribute("http-equiv", Value);

