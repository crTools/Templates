namespace crTools.Templates.Tags
{
	[TemplateAutoRegister]
	public class Case : Switch
	{
		public Case( TemplateSettings settings, string name, string args )
			: base( settings, name, args )
		{
		}
	}
}