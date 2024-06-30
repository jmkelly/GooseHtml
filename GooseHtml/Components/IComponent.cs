using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace GooseHtml.Components;

public interface IComponent
{
	Element Element { get; }
	public Func<HttpContext, IActionResult> Handle();
	public Func<HttpContext, Task<IActionResult>> HandleAsync();
}

