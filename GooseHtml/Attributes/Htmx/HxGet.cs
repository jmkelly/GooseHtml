namespace GooseHtml.Attributes.Htmx;

public record HxGet(string Value) : Attribute("hx-get", Value);

