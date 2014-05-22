namespace crTools.Templates.Elements
{
	public class Literal : IElement
	{
		public Literal( string text )
		{
			_text = text;
		}

		public void Render( TemplateContext context )
		{
			context.Writer.Write( _text );
		}

		readonly string _text;
	}
}