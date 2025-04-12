using Shouldly;

namespace GooseHtml.Tests
{
	//area, base, br, col, embed, hr, img, input, link, meta, source, track, wbr
	public class ElementTests 
	{
		[Fact]
		public void Input_ShouldBeAElement()
		{
			var input = new Input();
			input.ToString().ShouldBe("<input>");

		}
		[Fact]
		public void Img_ShouldBeAElement()
		{
			List<string> listOfElements = ["area", "base", "br", "col", "embed", "hr", "img", "input", "link", "meta", "source", "track", "wbr"];
			foreach (var el in listOfElements)
			{
				var element = ElementFactory.Create(el);
				element.ToString().ShouldBe($"<{el}>");
			}
		}
	}
}
