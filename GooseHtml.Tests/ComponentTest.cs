using Shouldly;
using GooseHtml.Components;

namespace GooseHtml.Tests;

public class ComponentTest
{
	[Fact]
	public void Component_ShouldHaveAType()
	{
		var component =  FormComponent.Create();
		var firstNameInput = new Input(Type.Text, "firstname", "");
		var lastNameInput = new Input(Type.Text, "lastname", "");
		var button = new Input(Type.Submit, "Save", "Save");
		component.Element.Add(firstNameInput);
		component.Element.Add(lastNameInput);
		component.Element.Add(button);
		component.Route.ShouldBe("form");
	}
}
