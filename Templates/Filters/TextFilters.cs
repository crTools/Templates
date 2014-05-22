using System.Net;
using System.Text.RegularExpressions;

namespace crTools.Templates.Filters
{
	[TemplateAutoRegister]
	public static class TextFilters
	{
		public static string Upcase( string input )
		{
			return input.ToUpper();
		}

		public static string Downcase( string input )
		{
			return input.ToLower();
		}

		public static string Capitalize( TemplateContext context, string input )
		{
			return context.Settings.CurrentCulture.TextInfo.ToTitleCase( input );
		}

		public static string Replace( string input, string find, string text = "" )
		{
			if( string.IsNullOrEmpty( find ) )
				return input;

			return input.Replace( find, text );
		}

		public static string ReplaceFirst( string input, string find, string text = "" )
		{
			if( string.IsNullOrEmpty( find ) )
				return input;

			int pos = input.IndexOf( find );
			if( pos >= 0 )
				return input.Remove( pos ) + text + input.Substring( pos + find.Length );

			return input;
		}

		public static string Remove( string input, string find )
		{
			return Replace( input, find );
		}

		public static string RemoveFirst( string input, string find )
		{
			return ReplaceFirst( input, find );
		}

		public static string RegexReplace( string input, string find, string text = "" )
		{
			if( string.IsNullOrEmpty( find ) )
				return input;

			return Regex.Replace( input, find, text );
		}

		public static string Prepend( string input, string text )
		{
			return text + input;
		}

		public static string Append( string input, string text )
		{
			return input + text;
		}

		public static string Truncate( string input, int length = 20, string text = "..." )
		{
			if( input.Length <= length )
				return input;

			return input.Remove( length - text.Length ) + text;
		}

		public static string Escape( string input )
		{
			return WebUtility.HtmlEncode( input );
		}

		public static string NewlineToBR( string input, string tag = "<br />" )
		{
			return TemplateRegex.RegexNewLine.Replace( input, tag + "$1" );
		}

		public static string StripNewlines( string input )
		{
			return TemplateRegex.RegexNewLine.Replace( input, "" );
		}

		public static string StripHtml( string input )
		{
			return TemplateRegex.RegexHtmlTag.Replace( input, "" );
		}

		public static string Trim( string input )
		{
			return input.Trim();
		}

		public static string TrimStart( string input )
		{
			return input.TrimStart();
		}

		public static string TrimEnd( string input )
		{
			return input.TrimEnd();
		}

		public static bool Contains( string input, string text )
		{
			return input.Contains( text );
		}

		public static bool StartsWith( string input, string text )
		{
			return input.StartsWith( text );
		}

		public static bool EndsWith( string input, string text )
		{
			return input.EndsWith( text );
		}
	}
}