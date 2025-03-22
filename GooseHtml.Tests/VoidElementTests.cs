using Shouldly;

namespace GooseHtml.Tests
{
	//area, base, br, col, embed, hr, img, input, link, meta, source, track, wbr
	public class VoidElementTests 
	{
		[Fact]
		public void Input_ShouldBeAVoidElement()
		{
			var input = new Input();
			input.ToString().ShouldBe("<input>");

		}
		[Fact]
		public void Img_ShouldBeAVoidElement()
		{
			List<string> listOfVoidElements = ["area", "base", "br", "col", "embed", "hr", "img", "input", "link", "meta", "source", "track", "wbr"];
			foreach (var el in listOfVoidElements)
			{
				var element = ElementFactory.Create(el);
				element.ToString().ShouldBe($"<{el}>");
			}
		}
	}
}
