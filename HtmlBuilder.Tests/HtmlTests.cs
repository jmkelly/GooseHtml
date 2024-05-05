using System.Text;
using System.Text.RegularExpressions;
using Shouldly;
namespace HtmlBuilder.Tests;

public partial class HtmlTests
{
	static string template = @"<html lang=""en-US""><head><meta charset=""utf-8""/><title></title></head><body></body></html>";

	[Fact]
	public void Html_ShouldReturnHtml()
	{
		var html = new Html();
		var head = new Head();
		var meta = new Meta();
		meta.Add(new CharsetAttr());

		head.Add(meta);
		head.Add(new Title());

		var body = new Body();

		html.Add(head);
		html.Add(body);
		html.Add(new LangAttr());

		html.ToString().ShouldBe(template);
	}

	[Fact]
	public void MetaElement_ShouldHaveMetaCharsetUtf8By()
	{
		var meta = new Meta();
		meta.ToString().ShouldBe("<meta/>");
	}

	[Fact]
	public void TitleElement_ShouldBeEmpty()
	{
		var title = new Title();
		title.ToString().ShouldBe("<title></title>");
	}

	[Fact]
	public void BodyElement_ShouldBeEmpty()
	{
		var body = new Body();
		body.ToString().ShouldBe("<body></body>");
	}

	[Fact]
	public void HeadElement_ShouldBeEmpty()
	{
		var head = new Head();
		head.ToString().ShouldBe("<head></head>");
	}

	[Fact]
	public void HeadElement_WithMetaAddedShouldContainMetaElement()
	{
		var head = new Head();
		head.Add(new Meta());
		head.ToString().ShouldBe("<head><meta/></head>");
	}


	[Fact]
	public void MetaElement_ShouldReturnCorrectHtmlForAdditionalAttributes()
	{
		string result = "<meta http-equiv=\"refresh\" content=\"3;url=https://www.mozilla.org\"/>";
		var meta = new Meta();
		meta.Add(new Attr("http-equiv", "refresh"));
		meta.Add(new Attr("content", "3;url=https://www.mozilla.org"));
		meta.ToString().ShouldBe(result);
	}

	[Fact]
	public void HeadElement_WithNestedMetaAddedShouldContainMetaElements()
	{
		var head = new Head();
		var meta = new Meta();
		meta.Add(new Attr("http-equiv", "refresh"));
		meta.Add(new Attr("content", "3;url=https://www.mozilla.org"));
		head.Add(meta);
		head.ToString().ShouldBe("<head><meta http-equiv=\"refresh\" content=\"3;url=https://www.mozilla.org\"/></head>");
	}
}

	public record Text(string Value);
	public record EmptyValue(): Text("");


	public class H1 : Element
	{
		public H1(Text value) : base(value, autoClose:true )
		{
		}
	}

	public class Body : Element
	{
		public Body() : base( autoClose:true)
		{
		}
	}

	public class Title : Element
	{
		public Title() : base(autoClose:true)
		{
		}
	}

	public class Meta : Element
	{

		public Meta() : base()
		{
		}
	}

	public record LangAttr(): Attr("lang", "en-US")
	{
		public override string ToString()
		{
			return base.ToString();
		}
	}
	public record CharsetAttr():Attr("charset", "utf-8")
	{
		public override string ToString()
		{
			return base.ToString();
		}
	}

	public record Attr(string Name, string Value)
	{
		public override string ToString()
		{
			return $"{Name}=\"{Value}\"";
		}
	}

	public class Head : Element
	{
		public Head() : base("head", true)
		{
		}
	}
