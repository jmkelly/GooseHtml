#  Goose Html

## Create Type-Safe HTML with ease using C# and GooseHtml!


### Create Type-Safe HTML with Ease using GooseHtml!

Are you tired of dealing with fragile strings and runtime errors when generating HTML in your C# projects? Say goodbye to those hassles with GooseHtml, the powerful C# HTML generation library!
Why Choose GooseHtml?

- Type Safety: Leverage the power of C#'s strong typing to ensure your HTML is error-free at compile time.
- It's just C#: No templating syntax or libary to learn, your html views are c# classes. Leverage object oriented design principles to create typesafe modular html components.
- Intuitive API: Create complex HTML structures with a clean and readable syntax.
- Seamless Integration: Easily integrate with your existing C# projects and start generating HTML right away.
- Enhanced Testing: Because your html views are c# classes, you can easily unit test them without requiring any additional libraries

### Key Features

- Fluent API: Chain methods together to build HTML elements intuitively.
- Comprehensive Coverage: Support for all standard HTML5 elements and attributes.
- Custom Extensions: Easily extend the library with your own custom components.

### How to use

- [docs](https://github.com/jmkelly/GooseHtml/tree/main/docs)
- [GooseHtml.Samples](https://github.com/jmkelly/GooseHtml/tree/main/GooseHtml.Samples)
- [Gihub Pages (built version of docs)](https://jmkelly.github.io/GooseHtml/)

### NuGet

https://www.nuget.org/packages/GooseHtml

### Sample Program.cs (see [GooseHtml.Samples](https://github.com/jmkelly/GooseHtml/tree/main/GooseHtml.Samples) for full samples) 

```
using GooseHtml;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

//make a very simple html page

var page = new Html();
var head = new Head();
page.Add(head);
var body = new Body();
body.Add(new H1("Hello World!"));
page.Add(body);

app.MapGet("/", () => page.ToHtmlResult());

app.Run();
```
