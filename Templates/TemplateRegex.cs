using System.Text.RegularExpressions;

namespace crTools.Templates
{
	public static class TemplateRegex
	{
		// Template engine syntax definition
		const string TagStart		= @"\{\%";
		const string TagEnd			= @"\%\}";
		const string VarStart		= @"\{\{";
		const string VarEnd			= @"\}\}";
		const string QuickStart		= @"\{";
		const string QuickEnd		= @"\}";
		const string TrimOption		= "-";

		// Helper
		const string Space			= @"\s*";
		const string Or				= "|";
		const string TrimSpace		= @"[^\S\r\n]*";
		const string TrimNewline	= @"\r?\n?";

		// Trimming
		const string TrimTagStart	= "(?:" + TagStart + Or + TrimSpace + TagStart + TrimOption + ")";
		const string TrimTagEnd		= "(?:" + TagEnd + Or + TrimOption + TagEnd + TrimSpace + TrimNewline + ")";
		const string TrimVarStart	= "(?:" + VarStart + Or + TrimSpace + VarStart + TrimOption + ")";
		const string TrimVarEnd		= "(?:" + VarEnd + Or + TrimOption + VarEnd + TrimSpace + TrimNewline + ")";

		// Groups
		const string TagName		= @"(?<tag>\w+)";
		const string TagArgs		= @"(?<args>.*?)";
		const string VarExpr		= @"(?<var>.+?)";
		const string QuickExpr		= @"(?<quick>[\w\.]+)";

		// Main expressions
		const string Tag			= TrimTagStart + Space + TagName + Space + TagArgs + Space + TrimTagEnd;
		const string Variable		= TrimVarStart + Space + VarExpr + Space + TrimVarEnd;
		const string Quick			= QuickStart + QuickExpr + QuickEnd;
		const string Elements		= Tag + Or + Variable + Or + Quick;

		// Regex instances
		public static Regex RegexElements		= new Regex( Elements, RegexOptions.Compiled );
		public static Regex RegexQuickIndexes	= new Regex( @"(\w+)", RegexOptions.Compiled );
		public static Regex RegexForTag			= new Regex( @"^(\w+)\s+in\s+(.+)$", RegexOptions.Compiled );
		public static Regex RegexSetDataTag		= new Regex( @"^(\w+)\s*[=:]\s*(.+)$", RegexOptions.Compiled );
		public static Regex RegexNewLine		= new Regex( @"(\r?\n)", RegexOptions.Compiled );
		public static Regex RegexHtmlTag		= new Regex( @"<.*?>", RegexOptions.Compiled );
	}
}