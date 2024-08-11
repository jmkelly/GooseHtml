using Attribute = GooseHtml.Attributes.Attribute;

namespace GooseHtml.Htmx;

public record HxBoost(string Value) : Attribute("hx-boost", Value);

