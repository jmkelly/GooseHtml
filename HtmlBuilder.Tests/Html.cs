namespace HtmlBuilder.Tests;

    public class Html : Element
	{
		public Html() : base(true)
		{
			Head = new Head();
			Body = new Body();
		}

    public Head Head { get; }
    public Body Body { get; }
}

