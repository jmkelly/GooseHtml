using Attribute = GooseHtml.Attributes.Attribute;

namespace GooseHtml.Htmx;

public record HxSelect(string Value) : Attribute("hx-select", Value)
{
}
