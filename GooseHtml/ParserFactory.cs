namespace GooseHtml;

public static class ParserFactory
{
	public static IParser Create(string html) => new HtmlParser(html);
	public static List<IParser> CreateAll(string html)  
	{
		var list = new List<IParser>
        {
            new HtmlParser(html),
        };

		return list;
	}
}

