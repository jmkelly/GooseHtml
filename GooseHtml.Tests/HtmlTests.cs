namespace GooseHtml.Tests;

using GooseHtml.Attributes;
using Shouldly;

public class HtmlTests
{
	static string template = @"<!DOCTYPE html><html lang=""en-US""><head><meta charset=""utf-8""/><title></title></head><body></body></html>";

	[Fact]
	public void Html_ExtendedShouldReturnHtml()
	{
		var example = new HtmlExtended();
		example.ToString().ShouldBe(template);
	}

	[Fact]
	public void Html_ShouldReturnHtml()
	{
		var html = new Html();
		var head = new Head();
		var meta = new Meta();
		meta.Add(new Charset());

		head.Add(meta);
		head.Add(new Title());

		var body = new Body();

		html.Add(head);
		html.Add(body);
		html.Add(new Lang());

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
		meta.Add(new Attribute("http-equiv", "refresh"));
		meta.Add(new Attribute("content", "3;url=https://www.mozilla.org"));
		meta.ToString().ShouldBe(result);
	}

	[Fact]
	public void HeadElement_WithNestedMetaAddedShouldContainMetaElements()
	{
		var head = new Head();
		var meta = new Meta();
		meta.Add(new Attribute("http-equiv", "refresh"));
		meta.Add(new Attribute("content", "3;url=https://www.mozilla.org"));
		head.Add(meta);
		head.ToString().ShouldBe("<head><meta http-equiv=\"refresh\" content=\"3;url=https://www.mozilla.org\"/></head>");
	}
}
