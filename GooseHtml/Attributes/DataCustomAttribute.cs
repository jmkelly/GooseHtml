namespace GooseHtml.Attributes;

public class DataCustomAttribute(string Key, string Value): Attribute($"data-{Key}", Value); 

