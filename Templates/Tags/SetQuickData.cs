namespace crTools.Templates.Tags
{
	[TemplateAutoRegister]
	public class SetQuickData : Elements.Tag
	{
		public SetQuickData( TemplateSettings settings, string name, string args )
			: base( settings, name, args )
		{
			_expression = args;
		}

		public override void Render( TemplateContext context )
		{
			context.Settings.QuickData = context.Evaluate( _expression );
		}

		readonly string _expression;
	}
}