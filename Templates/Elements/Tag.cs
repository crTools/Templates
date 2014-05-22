namespace crTools.Templates.Elements
{
	public abstract class Tag : IElement
	{
		public Tag( TemplateSettings settings, string name, string args )
		{
			_name = name;
		}

		public abstract void Render( TemplateContext context );

		public string Name
		{
			get
			{
				return _name;
			}
		}

		readonly string _name;
	}
}