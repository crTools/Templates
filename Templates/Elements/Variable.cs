namespace crTools.Templates.Elements
{
	public class Variable : IElement
	{
		public Variable( string expression )
		{
			_expression = expression;
		}

		public void Render( TemplateContext context )
		{
			context.Writer.Write( context.EvaluateToString( _expression ) );
		}

		readonly string _expression;
	}
}