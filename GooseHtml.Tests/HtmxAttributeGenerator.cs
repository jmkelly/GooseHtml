using System.Text;
using Humanizer;

namespace GooseHtml.Tests;


public class HtmxAttributeGenerator
{
	string[] htmxAttributes = {
    "hx-get",
    "hx-post",
    "hx-put",
    "hx-delete",
    "hx-patch",
    "hx-trigger",
    "hx-target",
    "hx-swap",
    "hx-select",
    "hx-include",
    "hx-params",
    "hx-push-url",
    "hx-prompt",
    "hx-confirm",
    "hx-disable",
    "hx-indicator",
    "hx-headers",
    "hx-sync",
    "hx-sse",
    "hx-ws",
    "hx-boost",
    "hx-replace-url",
    "hx-ext",
    "hx-vars"
	};


	//[Fact]
	[Fact(Skip="autogen done")]
	public void Generator()
	{
		//format is 
		// public record Action(string Value) : Attribute("action", Value)
		// {
		// }

		string folder = "../../../../GooseHtml.Htmx/";

		foreach (var att in htmxAttributes)
		{
			string titleCased = att.Humanize(LetterCasing.Title);
			string fileName = $"{titleCased}.cs";
			string fullPath = System.IO.Path.Join(folder, fileName);
			StringBuilder sb = new StringBuilder();
			sb.AppendLine("namespace GooseHtml.Htmx;");
			sb.AppendLine($"public record {titleCased}(string Value) : Attribute(\"{att}\", Value)"); 
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
