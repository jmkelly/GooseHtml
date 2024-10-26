using Microsoft.AspNetCore.Mvc;

namespace GooseHtml;

public static class ActionResultExtensions
{
	public static IActionResult AsActionResult(this IViewModel vm)
	{
		ArgumentNullException.ThrowIfNull(vm);
        var result = new ElementResult(vm.AsElement());
		return result;
	}

	public static IActionResult ToActionResult(this Element element)
	{
		ArgumentNullException.ThrowIfNull(element);
        return new ElementResult(element);
	}
}

