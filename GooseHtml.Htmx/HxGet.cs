using Attribute = GooseHtml.Attributes.Attribute;

namespace GooseHtml.Htmx;

public record HxGet(string value) : Attribute("hx-get", value)
{
}
