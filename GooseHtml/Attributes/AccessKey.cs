namespace GooseHtml.Attributes;

public record AccessKey(string Value) : Attribute("accesskey", Value);

