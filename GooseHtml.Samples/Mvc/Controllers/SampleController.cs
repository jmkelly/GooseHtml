using Microsoft.AspNetCore.Mvc;
namespace GooseHtml.Samples.Mvc.Controllers;

[ApiController]
[Route("[controller]")]
public class SampleController : Controller
{
    public SampleController()
    {
    }

	[HttpGet]
	public IActionResult Get()
	{
		//create a view model
		var vm = new ViewModel("james", "james@james");
		return new ExampleLayout(vm.AsElement()).ToActionResult();
	}
}

public class ViewModel(string name, string email) : IViewModel
{
    public string Name { get; } = name;
    public string Email { get; } = email;

    public Element AsElement()
    {
		var div = new Div();
		var form = new Form();
		form.Add(new Input(Type.Text, name: Name, value: Name));
		form.Add(new Input(Type.Text, name: Email, value: Email));
		div.Add(form);
		return div;
	}
}

