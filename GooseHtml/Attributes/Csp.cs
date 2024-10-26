namespace GooseHtml.Attributes;

public record Csp(string Value): Attribute("csp", Value);

