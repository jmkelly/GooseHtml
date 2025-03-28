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
		head.Add(new HtmxScript());
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
		var gridContainer = new Div(new GooseHtml.Attributes.Class("grid-container"));

		foreach (var code in codes)
		{
			var div = Program.CreateGridComponent(code);
			gridContainer.Add(div);
		}

		return gridContainer;
			
	}
}


