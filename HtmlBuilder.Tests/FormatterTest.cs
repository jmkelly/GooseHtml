using Shouldly;
namespace HtmlBuilder.Tests;

public class HtmlConversionTests
    {
        [Fact]
        public void ConvertTemplateToPrettyFormat()
        {
            string template = @"<html lang=""en-US""><head><meta charset=""utf-8""/><title></title></head><body></body></html>";
            string expectedPretty = @"<html lang=""en-US"">
  <head>
    <meta charset=""utf-8"" />
    <title></title>
  </head>
  <body></body>
</html>";


			var formatter = new HtmlFormatter();
			formatter.Pretty(template).ShouldBe(expectedPretty);
        }

}
