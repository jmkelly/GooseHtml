namespace GooseHtml;

public static class ParserFactory
{
	public static HtmlParser Create(string html) => new HtmlParser(html);
	public static List<HtmlParser> CreateAll(string html)  
	{
		var list = new List<HtmlParser>
        {
            new HtmlParser(html),
        };

		return list;
	}
}

