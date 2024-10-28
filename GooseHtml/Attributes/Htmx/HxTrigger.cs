namespace GooseHtml.Attributes.Htmx;

public record HxTrigger(string Value) : Attribute("hx-trigger", Value);
