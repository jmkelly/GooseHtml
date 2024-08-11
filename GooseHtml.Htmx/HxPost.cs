using Attribute = GooseHtml.Attributes.Attribute;

namespace GooseHtml.Htmx;

public record HxPost(string Value) : Attribute("hx-post", Value)
{
}
