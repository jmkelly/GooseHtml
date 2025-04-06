using Blend.Html.Lexer;

namespace GooseHtml.Tests;

public class Page : Html
    {
		public void LoadHtml(string html)
		{
			//need to work out some way to map the fragement to my own elements.....
			var elements = new HtmlParser(html).Parse();

			//figure out some way to extract specific elements by type
			//Body().Add(body);
			//Head().Add(head);
		}

        public Body Body() 
        {
			var body = new Body();
			return body;
        }

        public Head Head()
        {
			var head = new Head();
			return head;
        }
    }



