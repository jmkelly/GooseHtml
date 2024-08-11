using Attribute = GooseHtml.Attributes.Attribute;

namespace GooseHtml.Htmx;

public record HxInclude(string Value) : Attribute("hx-include", Value)
{
}
