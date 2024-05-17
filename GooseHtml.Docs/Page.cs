using GooseHtml;

namespace GooseHtml.Docs
{
    //introduction page
    //
    public class Page : Html
    {
		public Page() : base("html") 
		{
			//add head elements
			var head = new Head();
			Add(head);

			//add body elements
			var body = new Body();
			body.Add(Hero());
			Add(body);

		}

		Div Hero()
		{
			var hero = new Div();
			hero.Add(new Class("hero"));
			var h1 = new H1(new Text("GooseHtml"));
			hero.Add(h1);
			return hero;
		}

	}



}
