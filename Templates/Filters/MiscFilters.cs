using System;
using System.Collections;
using System.Globalization;

namespace crTools.Templates.Filters
{
	[TemplateAutoRegister]
	public static class MiscFilters
	{
		public static int Size( object input )
		{
			return Parsing.Operations.GetCount( input );
		}

		public static string String( string input )
		{
			return input;
		}

		public static int Integer( int input )
		{
			return input;
		}

		public static decimal Decimal( decimal input )
		{
			return input;
		}

		public static ArrayList Array( ArrayList input )
		{
			return input;
		}

		public static string Date( TemplateContext context, string input, string format = "d" )
		{
			return DateTime( context, input, format );
		}

		public static string Time( TemplateContext context, string input, string format = "T" )
		{
			return DateTime( context, input, format );
		}

		public static string DateTime( TemplateContext context, string input, string format = "G" )
		{
			DateTime date;

			if( string.Equals( input, "now", StringComparison.InvariantCultureIgnoreCase ) )
			{
				date = System.DateTime.Now;
			}
			else
			{
				if( !System.DateTime.TryParse( input, context.Settings.CurrentCulture, DateTimeStyles.AllowWhiteSpaces, out date ) )
					return null;
			}

			return date.ToString( format, context.Settings.CurrentCulture );
		}

		public static object Eval( TemplateContext context, string input )
		{
			return context.Evaluate( input );
		}
	}
}