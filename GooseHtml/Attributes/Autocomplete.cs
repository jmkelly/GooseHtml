namespace GooseHtml.Attributes;

public record Autocomplete(string Value): Attribute("autocomplete", Value);

