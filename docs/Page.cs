using GooseHtml.Attributes;

namespace GooseHtml.Docs
{
    public class Page : Html
    {
		public Page() : base("html") 
		{
			var head = new Head();

			head.Add(new Link(href: "style.css", rel: "stylesheet"));
			head.Add(new Link(href: "https://cdnjs.cloudflare.com/ajax/libs/highlight.js/11.10.0/styles/atom-one-dark.min.css", rel: "stylesheet"));
			head.Add(new Script(src: "https://cdnjs.cloudflare.com/ajax/libs/highlight.js/11.10.0/highlight.min.js"));
			head.Add(new Script(src: "site.js"));
			Add(head);

			var body = new Body();
			body.Add(Nav());
			body.Add(Hero());
			body.Add(CodeSample("codeSample"));
			var middle = new Section(new Class("middle"));
			middle.Add(CallToAction());
			middle.Add(CodeSample("code-sample-in-page"));
			middle.Add(WhyChoose());
			middle.Add(KeyFeatures());
			middle.Add(GetStarted());
			body.Add(middle);
			body.Add(Footer());
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
			var h2 = new H2(new Text("Create Type-Safe HTML With Ease"));
			div.Add(h1);
			div.Add(h2);
			hero.Add(div);
			return hero;
		}


		Section CodeSample(string className)
		{
			var section = new Section();
			var codeSample = new Div();
			codeSample.Add(new Class(className));
			var codeWrapper = new Pre();
			var code = new Code();
			code.Add(new Class("hljs language-csharp atom-one-dark"));
			//read the Page.cs file into text 
			code.Add(new Text(File.ReadAllText("Page.cs")));
			codeWrapper.Add(code);
			codeSample.Add(codeWrapper);
			section.Add(codeSample);
			return section;
		}

		Div CreateFlexItem()
		{
			var div = new Div();
			div.Add(new Class("flexitem"));
			return div;
		}

		Section CallToAction()
		{

			var section = new Section();
			section.Add(new Class("calltoaction"));
			var p = new P(new Text("The library for compile time checking of your html in C#"));

			var button = new Button();
			button.Add(new Text("Get Started"));
			button.Add(new Class("btn btn-large"));

			section.Add(p);
			section.Add(button);
			return section;

		}

		Section WhyChoose()
		{
			var div = new Section();
			var h2 = new H2(new Text("Why Choose GooseHtml?"));
			var list = new Ul();

			list.Add(BuildListItem("Type Safety: ", "Leverage the power of C#'s strong typing to ensure your HTML is error-free at compile time."));
			list.Add(BuildListItem("It's just C#: ", "No templating syntax or libary to learn, your html views are c# classes. Leverage object oriented design principles to create typesafe modular html components."));
			list.Add(BuildListItem("Intuitive API: ", "Create complex HTML structures with a clean and readable syntax."));
			list.Add(BuildListItem("Seamless Integration: ", "Easily integrate with your existing C# projects and start generating HTML right away."));
			list.Add(BuildListItem("Enhanced Testing: ", "Because your html views are c# classes, you can easily unit test them without requiring any additional libraries"));

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
			list.Add(BuildListItem("Fluent API: ", "Chain methods together to build HTML elements intuitively."));
			list.Add(BuildListItem("Comprehensive Coverage: ", "Support for all standard HTML5 elements and attributes."));
			list.Add(BuildListItem("Custom Extensions: ", "Easily extend the library with your own custom components."));
			div.Add(h2);
			div.Add(list);
			return div;
		}

		Section GetStarted()
		{
			var section = new Section();
			var h2 = new H2(new Text("Get Started with GooseHtml Today!"));
			var p = new P(new Text("Transform your C# projects with type-safe, maintainable, and elegant HTML generation."));

			var p2 = new P();
			p2.Add(new Text("Install the Library: Add GooseHtml via NuGet:"));	
			var p3 = new P();
			p3.Add(new Code("Install-Package GooseHtml"));
			var p4 = new P();
			p4.Add(new TextElement("Add GooseHtml via dotnet tool:"));
			var p5 = new P();
			p5.Add(new Code("dotnet add package GooseHtml"));


			section.Add(h2);
			section.Add(p);
			section.Add(p2);
			section.Add(p3);
			section.Add(p4);
			section.Add(p5);
			return section;
		}

		Footer Footer()
		{

			var section = new Footer();
			var div = new Div();
			div.Add(new Class("bottombar"));

			var link = new A(new Href("https://github.com/jmkelly/Goose"), new Text("")); 
			link.Add(new Class("btn btn-large"));
			var svg = new Svg();
			svg.Add(new Attributes.Attribute("xmlns", "http://www.w3.org/2000/svg"));
			svg.Add(new Attributes.Attribute("viewBox", "0 0 32 32"));
			svg.Add(new Attributes.Attribute("width", "36"));
			svg.Add(new Attributes.Attribute("aria-hidden", "true"));
			var path = new Path();
			path.Add(new Fill("currentColor"));
			path.Add(new D("M8 0C3.58 0 0 3.58 0 8c0 3.54 2.29 6.53 5.47 7.59.4.07.55-.17.55-.38 0-.19-.01-.82-.01-1.49-2.01.37-2.53-.49-2.69-.94-.09-.23-.48-.94-.82-1.13-.28-.15-.68-.52-.01-.53.63-.01 1.08.58 1.23.82.72 1.21 1.87.87 2.33.66.07-.52.28-.87.51-1.07-1.78-.2-3.64-.89-3.64-3.95 0-.87.31-1.59.82-2.15-.08-.2-.36-1.02.08-2.12 0 0 .67-.21 2.2.82.64-.18 1.32-.27 2-.27.68 0 1.36.09 2 .27 1.53-1.04 2.2-.82 2.2-.82.44 1.1.16 1.92.08 2.12.51.56.82 1.27.82 2.15 0 3.07-1.87 3.75-3.65 3.95.29.25.54.73.54 1.48 0 1.07-.01 1.93-.01 2.2 0 .21.15.46.55.38A8.013 8.013 0 0016 8c0-4.42-3.58-8-8-8z"));
			svg.Add(path);
			link.Add(svg);

			div.Add(link);

			section.Add(div);
			return section;
		}
	}

}
