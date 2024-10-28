using GooseHtml;
using GooseHtml.Attributes.Htmx;

class HtmxPage
{
	const int refreshRateInMs = 100;
    private readonly Html page;
    private readonly Body body;
    internal HtmxPage()
	{
		page = new Html();
		var head = new Head();
		body = new Body();
		head.Add(new Script(src: "https://unpkg.com/htmx.org@2.0.3", integrity: "sha384-0895/pl2MU10Hqc6jd4RvrthNlDiE9U1tWmX7WRESftEDRosgxNsQG/Ze9YMRzHq", crossorigin: "anonymous"));
		head.Add(new Link(href: "style.css", rel: "stylesheet"));
		body.Add(new HxTrigger($"every {refreshRateInMs}ms"));
		body.Add(new HxGet("/"));
		page.Add(head);
	}

	internal Html Build(List<StockCode> codes) 
	{
		body.Add(AddCodes(codes));
		page.Add(body);
		return page;
	}

	private static Div AddCodes(List<StockCode> codes)
	{
		var gridContainer = new Div(new Class("grid-container"));

		foreach (var code in codes)
		{
			var div = Program.CreateGridComponent(code);
			gridContainer.Add(div);
		}

		return gridContainer;
			
	}
}


