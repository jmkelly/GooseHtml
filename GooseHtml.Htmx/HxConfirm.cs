using Attribute = GooseHtml.Attributes.Attribute;

namespace GooseHtml.Htmx;

public record HxConfirm(string Value) : Attribute("hx-confirm", Value);
