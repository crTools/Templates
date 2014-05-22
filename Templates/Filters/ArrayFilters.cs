using System;
using System.Collections;

namespace crTools.Templates.Filters
{
	[TemplateAutoRegister]
	public static class ArrayFilters
	{
		public static object Split( string input, string separator = null )
		{
			if( string.IsNullOrEmpty( separator ) )
				return input.ToCharArray();

			return input.Split( new string[] { separator }, StringSplitOptions.RemoveEmptyEntries );
		}

		public static string Join( TemplateContext context, object input, string separator = null )
		{
			if( input == null )
				return null;

			if( input is IEnumerable )
				return Parsing.Operations.Join( context, ( IEnumerable )input, separator );

			throw new OperationUnsupportedTypeException( "Join", input );
		}

		public static ArrayList Sort( ArrayList input )
		{
			input.Sort();

			return input;
		}

		public static ArrayList Reverse( ArrayList input )
		{
			input.Reverse();

			return input;
		}

		public static object First( ArrayList input, int count = 1 )
		{
			if( count < 0 )
				throw new IndexOutOfRangeException();

			if( count == 1 && input.Count >= 1 )
				return input[ 0 ];

			if( input.Count > count )
				input.RemoveRange( count, input.Count - count );

			return input;
		}

		public static object Last( ArrayList input, int count = 1 )
		{
			if( count < 0 )
				throw new IndexOutOfRangeException();

			if( count == 1 && input.Count >= 1 )
				return input[ input.Count - 1 ];

			if( input.Count > count )
				input.RemoveRange( 0, input.Count - count );

			return input;
		}

		public static ArrayList SkipFirst( ArrayList input, int count = 1 )
		{
			if( count < 0 )
				throw new IndexOutOfRangeException();

			if( input.Count > count )
			{
				input.RemoveRange( 0, count );
			}
			else
			{
				input.Clear();
			}

			return input;
		}

		public static ArrayList SkipLast( ArrayList input, int count = 1 )
		{
			if( count < 0 )
				throw new IndexOutOfRangeException();

			if( input.Count > count )
			{
				input.RemoveRange( input.Count - count, count );
			}
			else
			{
				input.Clear();
			}

			return input;
		}

		public static ArrayList Range( ArrayList input, int start, int count )
		{
			if( start < 0 || count < 0 )
				throw new IndexOutOfRangeException();

			if( start > 0 )
			{
				if( input.Count > start )
				{
					input.RemoveRange( 0, start );
				}
				else
				{
					input.Clear();
				}
			}

			if( input.Count > count )
				input.RemoveRange( count, input.Count - count );

			return input;
		}
	}
}