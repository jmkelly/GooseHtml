using Shouldly;

namespace HtmlBuilder.Tests;

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
		var vm = new TestViewModel();
		vm.Name = "test name";
		vm.Address = "1 test way, test city, test postcode";
		vm.Phone = "01234 567890";

		var layout = new ViewModelLayout(vm);

		layout.ToString().ShouldBe("<html><head></head><body><nav class=\"nav navbar\"></nav><div class=\"sidebar\"></div><footer class=\"footer\"></footer></body></html>");

	}
}

public class TestViewModel : IViewModel
{
	public string Name {get;set;}  
	public string Address {get;set;}
	public string Phone {get;set;}

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

