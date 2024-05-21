namespace GooseHtml.Docs;

 public class Program
 {
     private static async Task Main(string[] args)
     {
		var app = WebApplication.Create(args);
		app.UseStaticFiles();
		app.MapGet("/", async (context) =>
        {
            await WritePage(context);
        });

		await app.RunAsync();
	 }

    private static async Task WritePage(HttpContext context)
    {
        var writer = new HtmlWriter();
        var page = new Page();
        var formatter = new HtmlFormatter();
		var unformatted = page.ToString();
        await writer.WriteAsync("index.html", unformatted);
        context.Response.ContentType = "text/html";
        await context.Response.WriteAsync(unformatted);
    }
}
