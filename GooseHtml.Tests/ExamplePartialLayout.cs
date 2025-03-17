using Shouldly;

namespace GooseHtml.Tests;

public class ExamplePartialLayout : ExampleLayout
{
	public override Body Body()
	{
		var body = new Body();
		body.Add(new Footer(new Class("footer")));
		return body;
	}
}

public class ViewModelLayout : ExampleLayout
{
	public ViewModelLayout(IViewModel vm) : base()
	{
		Body().Add(vm.AsElement());
	}
}

public class ViewModelTests
{
	[Fact]
	public void ViewModel_ShouldBeAddedIntoLayout()
	{
		var vm = new TestViewModel(
			Name : "test name",
			Address : "1 test way, test city, test postcode",
			Phone : "01234 567890"
		);

		var layout = new ViewModelLayout(vm);

		layout.ToString().ShouldBe("<!DOCTYPE html><html><head></head><body><nav class=\"nav navbar\"></nav><div class=\"sidebar\"></div><footer class=\"footer\"></footer></body></html>");

	}
}

public class TestViewModel(string Name, string Address, string Phone) : IViewModel
{

    public Element AsElement()
    {
		var div = new Div();

		var sp1 = new Span(new Text(Name));
		var sp2 = new Span(new Text(Address));
		var sp3 = new Span(new Text(Phone));
		div.Add(sp1);
		div.Add(sp2);
		div.Add(sp3);
		return div;
    }
}

