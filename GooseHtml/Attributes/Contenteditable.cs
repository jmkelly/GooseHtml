namespace GooseHtml.Attributes;

public record Contenteditable(string Value): Attribute("contenteditable", Value);

