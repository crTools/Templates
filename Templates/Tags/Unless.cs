namespace crTools.Templates.Tags
{
	[TemplateAutoRegister]
	public class Unless : Elements.Block
	{
		public Unless( TemplateSettings settings, string name, string args )
			: base( settings, name, args )
		{
			_expression = args;
		}

		public override void Render( TemplateContext context )
		{
			if( !context.EvaluateToBool( _expression ) )
				RenderElements( context );
		}

		readonly string _expression;
	}
}