using GooseHtml;
using GooseHtml.Docs;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.UseStaticFiles();
app.MapGet("/", Demo.WritePage);

app.Run();


internal static class Demo
{
	internal static async Task WritePage(HttpContext context)
	{
		var writer = new HtmlWriter();
		var page = new Page();
		var unformatted = page.ToString();
		await writer.WriteAsync("index.html", unformatted);
		context.Response.ContentType = "text/html";
		await context.Response.WriteAsync(unformatted);
	}
}
