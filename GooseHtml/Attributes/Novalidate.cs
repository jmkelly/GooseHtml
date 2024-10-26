namespace GooseHtml.Attributes;

public record NoValidate(string Value): Attribute("noValidate", Value);

