namespace crTools.Templates.Tags
{
	[TemplateAutoRegister]
	public class SetData : Elements.Tag
	{
		public SetData( TemplateSettings settings, string name, string args )
			: base( settings, name, args )
		{
			var match = TemplateRegex.RegexSetDataTag.Match( args );
			if( !match.Success )
				throw new TagSyntaxErrorException( "SetData", args );

			_variable	= match.Groups[ 1 ].Value;
			_expression	= match.Groups[ 2 ].Value;
		}

		public override void Render( TemplateContext context )
		{
			context.Settings.Data.Set( _variable, context.Evaluate( _expression ) );
		}

		readonly string _variable;
		readonly string _expression;
	}
}