using System.Threading.Tasks;
using GooseHtml;
namespace GooseHtml.Docs;

internal class Program
{
    private static async Task Main(string[] args)
    {
        var writer = new HtmlWriter();
        var page = new Page();
		var formatter = new HtmlFormatter();
        await writer.WriteAsync("docs.html", formatter.Pretty(page.ToString()));
    }
}
