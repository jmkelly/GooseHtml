using System.Text;
using Humanizer;

namespace GooseHtml.Tests;

public class ElementGenerator
{

    readonly string[] elements = [
    "html", "head", "title", "base", "link", "meta", "style", "body",
    "article", "section", "nav", "aside", "h1", "h2", "h3", "h4", "h5", "h6",
    "header", "footer", "address", "p", "hr", "pre", "blockquote", "ol", "ul",
    "li", "dl", "dt", "dd", "figure", "figcaption", "main", "div", "a", "em",
    "strong", "small", "s", "cite", "q", "dfn", "abbr", "ruby", "rt", "rp",
    "data", "time", "code", "var", "samp", "kbd", "sub", "sup", "i", "b",
    "u", "mark", "bdi", "bdo", "span", "br", "wbr", "ins", "del", "img",
    "iframe", "embed", "object", "param", "video", "audio", "source", "track",
    "map", "area", "table", "caption", "colgroup", "col", "tbody", "thead",
    "tfoot", "tr", "td", "th", "form", "label", "input", "button", "select",
    "datalist", "optgroup", "option", "textarea", "output", "progress", 
    "meter", "fieldset", "legend", "details", "summary", "dialog", "script", 
    "noscript", "template", "canvas", "svg", "math", "cite", "aside", 
    "section", "nav", "article", "address", "header", "footer", "figure", 
    "figcaption", "output"
];

	[Fact(Skip="ignore")]
	//[Fact]
	public void Generate()
	{
		//generate all the elements as classes
		string folder = "../../../../GooseHtml/Elements/";

		foreach (var el in elements)
		{
			string titleCased = el.Humanize(LetterCasing.Title);
			string fileName = $"{titleCased}.cs";
			string fullPath = System.IO.Path.Join(folder, fileName);
			StringBuilder sb = new StringBuilder();
			sb.AppendLine("namespace GooseHtml;");
			sb.AppendLine($"public class {titleCased} : Element");
			sb.AppendLine("{");
			sb.AppendLine("}");

			//check if the file already exists
			if (!File.Exists(fullPath)) 
			{
				File.WriteAllText(fullPath, sb.ToString());
			}
		}
	}
}


