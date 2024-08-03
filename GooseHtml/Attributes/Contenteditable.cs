namespace GooseHtml.Attributes;

public record Contenteditable(string value): Attribute("contenteditable", value);

