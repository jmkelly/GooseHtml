using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace GooseHtml;

public class HtmlFormatter
{
	public string Pretty(string html)
	{
		var sb = new StringBuilder();
		var element = XElement.Parse(html);
        var settings = new XmlWriterSettings
        {
            OmitXmlDeclaration = true,
            Indent = true
        };

        using (var xmlWriter = XmlWriter.Create(sb, settings))
		{
			element.Save(xmlWriter);
		}

		return sb.ToString();
	}
}
