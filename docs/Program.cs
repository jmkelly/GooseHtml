namespace GooseHtml.Docs;

 public class Program
 {
     private static async Task Main(string[] args)
     {
		var app = WebApplication.Create(args);
		app.UseStaticFiles();
		app.MapGet("/", WritePage);

		await app.RunAsync();
	 }

    private static async Task WritePage(HttpContext context)
    {
        var writer = new HtmlWriter();
        var page = new Page();
		var unformatted = page.ToString();
        await writer.WriteAsync("index.html", unformatted);
        context.Response.ContentType = "text/html";
        await context.Response.WriteAsync(unformatted);
    }
}
