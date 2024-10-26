namespace GooseHtml.Attributes;

public record DataCustomAttribute(string Key, string Value): Attribute($"data-{Key}", Value); 

