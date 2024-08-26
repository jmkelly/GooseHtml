using Microsoft.AspNetCore.Http;

namespace GooseHtml;
    
public static class ResultExtension
{
	public static IResult ToHtmlResult(this Element element)
	{
		return Results.Content(element.ToString(), HtmlContentType);
	}

	public const string HtmlContentType = "text/html";
}

