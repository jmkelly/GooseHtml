namespace GooseHtml.Attributes;

public record DataCustomAttribute(string key, string value): Attribute($"data-{key}", value); 

