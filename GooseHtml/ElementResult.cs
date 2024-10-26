using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GooseHtml;

public class ElementResult : IActionResult
{
    public ElementResult(Element element)
    {
		ArgumentNullException.ThrowIfNull(element);
		Element = element;
    }

    public Element Element { get; }

    public async Task ExecuteResultAsync(ActionContext context)
    {			
		var response = context.HttpContext.Response;
		response.ContentType = "text/html; charset=utf-8";
		await response.WriteAsync(Element.ToString());
    }
}


