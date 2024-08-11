using Attribute = GooseHtml.Attributes.Attribute;

namespace GooseHtml.Htmx;

public record HxHeaders(string value) : Attribute("hx-headers", value)
{
}
