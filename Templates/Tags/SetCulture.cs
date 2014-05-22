using System.Globalization;

namespace crTools.Templates.Tags
{
	[TemplateAutoRegister]
	public class SetCulture : Elements.Tag
	{
		public SetCulture( TemplateSettings settings, string name, string args )
			: base( settings, name, args )
		{
			_expression = args;
		}

		public override void Render( TemplateContext context )
		{
			var culture = context.EvaluateToString( _expression );
			context.Settings.Culture = string.IsNullOrWhiteSpace( culture ) ? null : new CultureInfo( culture );
		}

		readonly string _expression;
	}
}