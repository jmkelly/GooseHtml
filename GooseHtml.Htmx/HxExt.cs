using Attribute = GooseHtml.Attributes.Attribute;

namespace GooseHtml.Htmx;

public record HxExt(string value) : Attribute("hx-ext", value)
{
}
