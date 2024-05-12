using HtmlBuilder.Tests;
using Microsoft.AspNetCore.Mvc;

namespace HtmlBuilder.Samples.Controllers;

public static class ActionResultExtensions
{
	public static IActionResult AsActionResult(this IViewModel vm)
	{

		var result = new ElementResult(vm.AsElement());
		return result;
	}
}


