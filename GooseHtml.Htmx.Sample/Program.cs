using GooseHtml;
using GooseHtml.Htmx;
using GooseHtml.Attributes;


internal class Program
{
    private static void Main(string[] args)
    {
		const string htmxEndpoint = "/htmx-endpoint";

        var builder = WebApplication.CreateBuilder(args);
        var app = builder.Build();


        var html = new Html();
        var head = new Head();
		var script = new Script("https://unpkg.com/htmx.org@2.0.1");
		head.Add(script);
        var body = new Body();

        var footer = new Footer();


        html.Add(head);
        html.Add(body);
        html.Add(footer);


        //add some stuff 
        var h1 = new H1(new Text("GooseHtml!!!!!"));
        var p = new P(new Text("This is a paragraph"));
		var target = new Div();
		var targetId = new Id("target");
		target.Add(targetId);

		var button = new Button();
		button.Add(new HxGet(htmxEndpoint));
		button.Add(new HxTarget($"#{targetId.Value}"));
		button.Add(new Text("Click me!"));



        body.Add(h1);
        body.Add(p);
		body.Add(button);
		body.Add(target);


		var resultDiv = new Div();
		var result = new H1(new Text("Htmx is great!"));
		var table = new Table();
		var thead = new Thead();
		//create 3 columns
		var row = new Tr();
		row.Add(new Td(new Text("Column 1")));
		row.Add(new Td(new Text("Column 2")));
		row.Add(new Td(new Text("Column 3")));
		thead.Add(row);

		var tbody = new Tbody();

		table.Add(thead);
		table.Add(tbody);
		resultDiv.Add(result);
		resultDiv.Add(table);

        app.MapGet("/", () => Results.Extensions.Html(html));
        app.MapGet(htmxEndpoint, () => Results.Extensions.Html(resultDiv));

        app.Run();
    }
}
