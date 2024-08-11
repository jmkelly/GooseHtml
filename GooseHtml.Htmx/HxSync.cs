using Attribute = GooseHtml.Attributes.Attribute;

namespace GooseHtml.Htmx;

public record HxSync(string Value) : Attribute("hx-sync", Value)
{
}
