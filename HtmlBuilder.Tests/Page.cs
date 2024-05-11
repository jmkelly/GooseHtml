using Blend.Html.Lexer;

namespace HtmlBuilder.Tests;

public class Page : Html
    {
		public void LoadHtml(List<Fragment> fragments)
		{
			//need to work out some way to map the fragement to my own elements.....
			var bodyElements = fragments.MapBodyElements();
			var headElements = fragments.MapHeadElements();

			//Body().Add(bodyElements);
			//Head().Add(headElements);
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



