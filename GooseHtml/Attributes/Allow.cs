namespace GooseHtml.Attributes;

public record Allow(string Value) : Attribute("allow", Value); 

