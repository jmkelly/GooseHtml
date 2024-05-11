using Blend.Html.Lexer;

namespace HtmlBuilder.Tests;
public static class HtmlParserExtensions
{

	public static List<Element> MapBodyElements(this List<Fragment> fragments)
	{
		throw new NotImplementedException();
	}

	public static List<Element> MapHeadElements(this List<Fragment> fragments)
	{
		
		throw new NotImplementedException();
	}
}

public class HtmlParser
{

    public class Page : Html
    {
		public class LoadHtml(List<Fragment> fragments )
		{
			//need to work out some way to map the fragement to my own elements.....
			var bodyElements = fragments.MapBodyElements()
			var headElements = fragments.MapHeadElements()

			body.Add(bodyElements);
			head.Add(headElements);
		}

        public override Body Body() 
        {
			var body = new Body();
			return body;
        }

        public override Head Head()
        {
			var head = new Head();
			return head;
        }
    }
    public Html Parse(string html)
    {

    }
}


