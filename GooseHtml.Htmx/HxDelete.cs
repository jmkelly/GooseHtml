namespace GooseHtml.Htmx;
using GooseHtml.Attributes;

public record HxDelete(string Value) : Attribute("hx-delete", Value)
{
}
