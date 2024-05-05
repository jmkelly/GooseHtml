using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace HtmlBuilder.Tests;


public class HtmlFormatter
{
	public string Pretty(string html)
	{

		var sb = new StringBuilder();
		var element = XElement.Parse(html);

		var settings = new XmlWriterSettings();
		settings.OmitXmlDeclaration = true;
		settings.Indent = true;

		using (var xmlWriter = XmlWriter.Create(sb, settings))
		{
			element.Save(xmlWriter);
		}

		return sb.ToString();
	}
}
