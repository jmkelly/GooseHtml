
using GooseHtml.Attributes;

namespace GooseHtml.Docs
{
    public class Page : Html
    {
		public Page() : base("html") 
		{
			var head = new Head();

			head.Add(new Link(href: "style.css", rel: "stylesheet"));
			Add(head);

			var body = new Body();
			body.Add(Nav());
			body.Add(Hero());
			body.Add(CallToAction());
			body.Add(WhyChoose());
			body.Add(KeyFeatures());
			body.Add(GetStarted());
			Add(body);
		}

		Nav Nav()
		{
			var nav = new Nav();
			var brandLink = new A(new Href("#"), new Text("GooseHtml"));
			brandLink.Add(new Class("navbrand"));
			var docLink = new A(new Href("#"), new Text("Docs"));
			docLink.Add(new Class("navlink"));
			nav.Add(brandLink);
			nav.Add(docLink);
			return nav;
		}

		Section Hero()
		{
			var hero = new Section();
			hero.Add(new Class("hero"));
			var div = new Div();
			var h1 = new H1(new Text("GooseHtml"));
			var p = new P(new Text("Create Type-Safe HTML with ease using C# and GooseHtml!"));
			div.Add(h1);
			div.Add(p);
			hero.Add(div);
			hero.Add(CodeSample());
			return hero;
		}


		Div CodeSample()
		{
			var codeSample = new Div();
			codeSample.Add(new Class("codeSample"));
			var codeWrapper = new Pre();
			var code = new Code();
			//read the Page.cs file into text 
			code.Add(new Text(File.ReadAllText("Page.cs")));
			codeWrapper.Add(code);
			codeSample.Add(codeWrapper);
			return codeSample;
		}

		Section CallToAction()
		{

			var div = new Section();
			var h1 = new H1(new Text("Create Type-Safe HTML with Ease using GooseHtml!"));
			var p = new P(new Text("Are you tired of dealing with fragile strings and runtime errors when generating HTML in your C# projects? Say goodbye to those hassles with GooseHtml, the powerful C# HTML generation library!"));
			div.Add(h1);
			div.Add(p);
			return div;

		}

		Section WhyChoose()
		{
			var div = new Section();
			var h2 = new H2(new Text("Why Choose GooseHtml?"));
			var list = new Ul();
			list.Add(new Li(new Text("Type Safety: Leverage the power of C#'s strong typing to ensure your HTML is error-free at compile time.")));
			list.Add(new Li(new Text("It's just C#: No templating syntax or libary to learn, your html views are c# classes. Leverage object oriented design principles to create typesafe modular html components.")));
			list.Add(new Li(new Text("Intuitive API: Create complex HTML structures with a clean and readable syntax.")));
			list.Add(new Li(new Text("Seamless Integration: Easily integrate with your existing C# projects and start generating HTML right away.")));
			list.Add(new Li(new Text("Enhanced Testing: Because your html views are c# classes, you can easily unit test them without requiring any additional libraries")));
			div.Add(h2);
			div.Add(list);
			return div;
		}

		Li BuildListItem(string header, string description)
		{
			var li = new Li();
			li.Add(new Strong(new Text(header)));
			li.Add(new Text(description));
			return li;
		}


		Section KeyFeatures()
		{
			var div = new Section();
			var h2 = new H2(new Text("Key Features"));
			var list = new Ul();
			var vals = new List<HeaderAndDescription>();

			vals.Add(new HeaderAndDescription{Header="Fluent API: ", Description="Chain methods together to build HTML elements intuitively."});
			vals.Add(new HeaderAndDescription{Header="Comprehensive Coverage: ", Description="Support for all standard HTML5 elements and attributes."});
			vals.Add(new HeaderAndDescription{Header="Custom Extensions: ", Description="Easily extend the library with your own custom components."});
			vals.Add(new HeaderAndDescription{Header="Custom Extensions: ", Description="Performance Optimized: Generate HTML quickly without sacrificing performance."});

			vals.ForEach(val => list.Add(BuildListItem(val.Header, val.Description)));
			vals.Clear();



			vals.Add(new HeaderAndDescription{Header="Comprehensive Coverage: " , Description="Support for all standard HTML5 elements and attributes."});
			vals.Add(new HeaderAndDescription{Header="Custom Extensions: ", Description = "Easily extend the library with your own custom components."});
			vals.Add(new HeaderAndDescription{Header="Custom Extensions: ", Description = "Performance Optimized: Generate HTML quickly without sacrificing performance."});
			vals.ForEach(val => list.Add(BuildListItem(val.Header, val.Description)));

			div.Add(h2);
			div.Add(list);

			return div;
		}

		Section GetStarted()
		{
			var div = new Section();
			var h2 = new H2(new Text("Get Started with GooseHtml Today!"));
			var p = new P(new Text("Transform your C# projects with type-safe, maintainable, and elegant HTML generation."));
			var list = new Ul();

			var li = new Li();
			li.Add(new Text("Install the Library: Add GooseHtml via NuGet:"));	
			li.Add(new Code("Install-Package GooseHtml"));
			li.Add(new TextElement(" or "));
			li.Add(new Code("dotnet add package GooseHtml"));
			list.Add(li);

			var li2 = new Li();
			var link = new A(new Href("https://github.com/jmkelly/GooseHtml"), new Text("GooseHtml on GitHub"));
			link.Add(new Class("btn btn-large"));
			li2.Add(link);
			list.Add(li2);

			var p2 = new P(new Text("Don't let HTML generation be a source of bugs and frustration.  Elevate your coding experience with GooseHtml!"));
			div.Add(h2);
			div.Add(p);
			div.Add(list);
			div.Add(p2);
			return div;
		}
	}

}
