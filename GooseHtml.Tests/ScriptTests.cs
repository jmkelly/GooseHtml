using GooseHtml.Attributes;
using Shouldly;

namespace GooseHtml.Tests;

public class ScriptTests
{
	const string output = @"<script async src=""https://pagead2.googlesyndication.com/pagead/js/adsbygoogle.js?client=ca-pub-XXXXXXXXXXXXXXXX"" crossorigin=""anonymous""></script>";

	[Fact]
	public void Script_ShouldReturnScript()
	{
		var script = new Script();
		script.Add(new Async());
		script.Add(new Src("https://pagead2.googlesyndication.com/pagead/js/adsbygoogle.js?client=ca-pub-XXXXXXXXXXXXXXXX"));
		script.Add(CrossOrigins.Anonymous());
		script.ToString().ShouldBe(output);
	}

	[Fact]
	public void Async_ShouldReturnAsync()
	{
		var async = new Async();
		async.ToString().ShouldBe("async");
	}

	

}
