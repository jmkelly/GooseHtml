namespace GooseHtml.Attributes;

public record AccessKey(string value) : Attribute("accesskey", value);

