using System;
using System.Collections;
using System.Reflection;
using System.Text;

namespace crTools.Templates.Parsing
{
	public static class Operations
	{
		// Conversion
		public static string ToString( TemplateContext context, object value )
		{
			if( value == null )
				return "";

			if( value is string )
				return ( string )value;

			if( value is IEnumerable )
				return Join( context, ( IEnumerable )value );

			if( value is IFormattable )
				return ( ( IFormattable )value ).ToString( null, context.Settings.CurrentCulture );

			return value.ToString();
		}

		public static bool ToBool( TemplateContext context, object value )
		{
			if( value == null )
				return false;

			if( value is bool )
				return ( bool )value;

			if( value is string )
				return ( ( string )value ).Length > 0;

			if( value is int )
				return ( ( int )value ) != 0;

			if( isNumber( value ) )
				return Convert.ToDecimal( value, context.Settings.CurrentCulture ) != 0.0m;

			if( value is ICollection )
				return ( ( ICollection )value ).Count > 0;

			if( value is IEnumerable )
			{
				foreach( object obj in ( IEnumerable )value )
					return true;
				return false;
			}

			throw new OperationUnsupportedTypeException( "ToBool", value );
		}

		public static int ToInteger( TemplateContext context, object value )
		{
			if( value == null )
				return 0;

			return Convert.ToInt32( value, context.Settings.CurrentCulture );
		}

		public static decimal ToDecimal( TemplateContext context, object value )
		{
			if( value == null )
				return 0m;

			return Convert.ToDecimal( value, context.Settings.CurrentCulture );
		}

		public static ArrayList ToArray( object input )
		{
			if( input == null )
				return null;

			if( input is ArrayList )
				return ( ArrayList )input;

			if( input is ICollection )
				return new ArrayList( ( ICollection )input );

			if( input is IEnumerable )
			{
				var list = new ArrayList();
				foreach( object obj in ( IEnumerable )input )
					list.Add( obj );
				return list;
			}

			throw new OperationUnsupportedTypeException( "ToArray", input );
		}

		public static object ToType( TemplateContext context, Type type, object input )
		{
			if( type == typeof( string ) )
				return ToString( context, input );

			if( type == typeof( int ) )
				return ToInteger( context, input );

			if( type == typeof( decimal ) )
				return ToDecimal( context, input );

			if( type == typeof( bool ) )
				return ToBool( context, input );

			if( type == typeof( ArrayList ) )
				return ToArray( input );

			throw new OperationUnsupportedTypeException( "ToType", type );
		}

		// Math
		public static object Add( TemplateContext context, object value1, object value2 )
		{
			if( value1 == null )
				return value2;

			if( value2 == null )
				return value1;

			if( value1 is string || value2 is string )
				return ToString( context, value1 ) + ToString( context, value2 );

			if( value1 is int && value2 is int )
				return ( int )value1 + ( int )value2;

			if( isNumber( value1 ) && isNumber( value2 ) )
				return Convert.ToDecimal( value1, context.Settings.CurrentCulture ) + Convert.ToDecimal( value2, context.Settings.CurrentCulture );

			throw new OperationUnsupportedTypesException( "Add", value1, value2 );
		}

		public static object Sub( TemplateContext context, object value1, object value2 )
		{
			if( value1 == null || value2 == null )
				throw new OperationUnsupportedNullException( "Sub" );

			if( value1 is int && value2 is int )
				return ( int )value1 - ( int )value2;

			if( isNumber( value1 ) && isNumber( value2 ) )
				return Convert.ToDecimal( value1, context.Settings.CurrentCulture ) - Convert.ToDecimal( value2, context.Settings.CurrentCulture );

			throw new OperationUnsupportedTypesException( "Sub", value1, value2 );
		}

		public static object Mul( TemplateContext context, object value1, object value2 )
		{
			if( value1 == null || value2 == null )
				throw new OperationUnsupportedNullException( "Mul" );

			if( value1 is int && value2 is int )
				return ( int )value1 * ( int )value2;

			if( isNumber( value1 ) && isNumber( value2 ) )
				return Convert.ToDecimal( value1, context.Settings.CurrentCulture ) * Convert.ToDecimal( value2, context.Settings.CurrentCulture );

			throw new OperationUnsupportedTypesException( "Mul", value1, value2 );
		}

		public static object Div( TemplateContext context, object value1, object value2 )
		{
			if( value1 == null || value2 == null )
				throw new OperationUnsupportedNullException( "Div" );

			if( value1 is int && value2 is int )
				return ( int )value1 / ( int )value2;

			if( isNumber( value1 ) && isNumber( value2 ) )
				return Convert.ToDecimal( value1, context.Settings.CurrentCulture ) / Convert.ToDecimal( value2, context.Settings.CurrentCulture );

			throw new OperationUnsupportedTypesException( "Div", value1, value2 );
		}

		public static object Mod( TemplateContext context, object value1, object value2 )
		{
			if( value1 == null || value2 == null )
				throw new OperationUnsupportedNullException( "Mod" );

			if( value1 is int && value2 is int )
				return ( int )value1 % ( int )value2;

			throw new OperationUnsupportedTypesException( "Mod", value1, value2 );
		}

		public static object Inv( TemplateContext context, object value )
		{
			if( value == null )
				throw new OperationUnsupportedNullException( "Inv" );

			if( value is int )
				return -( int )value;

			if( isNumber( value ) )
				return -Convert.ToDecimal( value, context.Settings.CurrentCulture );

			throw new OperationUnsupportedTypeException( "Inv", value );
		}

		// Operations
		public static int GetCount( object value )
		{
			if( value == null )
				return 0;

			if( value is string )
				return ( ( string )value ).Length;

			if( value is ICollection )
				return ( ( ICollection )value ).Count;

			if( value is IEnumerable )
			{
				int count = 0;
				foreach( object obj in ( IEnumerable )value )
					count++;
				return count;
			}

			throw new OperationUnsupportedTypeException( "GetCount", value );
		}

		public static int Compare( TemplateContext context, object value1, object value2 )
		{
			if( value1 == value2 )
				return 0;

			if( value1 == null )
				return -1;

			if( value2 == null )
				return 1;

			if( value1.GetType() == value2.GetType() && value1 is IComparable )
				return ( ( IComparable )value1 ).CompareTo( value2 );

			if( isNumber( value1 ) && isNumber( value2 ) )
				return Convert.ToDecimal( value1, context.Settings.CurrentCulture ).CompareTo( Convert.ToDecimal( value2, context.Settings.CurrentCulture ) );

			throw new OperationUnsupportedTypesException( "Compare", value1, value2 );
		}

		public static string Join( TemplateContext context, IEnumerable enumerable, string separator = null )
		{
			var sb	  = new StringBuilder();
			var first = true;

			foreach( object obj in enumerable )
			{
				if( obj != null )
				{
					if( first )
					{
						first = false;
					}
					else if( separator != null )
					{
						sb.Append( separator );
					}

					sb.Append( ToString( context, obj ) );
				}
			}

			return sb.ToString();
		}

		public static object Index( TemplateContext context, object data, object index )
		{
			if( index == null )
				throw new ArgumentNullException( "index" );

			if( data == null )
				return null;

			if( index is int )
			{
				if( data is IList )
					return ( ( IList )data )[ ( int )index ];

				if( data is IEnumerable )
				{
					int skip = ( int )index;

					foreach( object obj in ( IEnumerable )data )
						if( --skip < 0 )
							return obj;

					throw new InvalidIndexException( data, index );
				}
			}

			string path = ToString( context, index );

			if( string.IsNullOrWhiteSpace( path ) )
				throw new ArgumentNullException( "index" );

			if( data is IDictionary )
			{
				var dictionary = ( IDictionary )data;
				return dictionary[ path ];
			}

			var property = data.GetType().GetProperty( path );
			if( property != null )
				return property.GetValue( data, null );

			throw new InvalidIndexException( data, path );
		}

		public static object Filter( TemplateContext context, object data, MethodInfo filter, object[] filterArgs )
		{
			if( filter == null )
				throw new ArgumentNullException( "filter" );

			var args = new ArrayList();

			var parameters = filter.GetParameters();
			if( parameters.Length > 0 )
			{
				Type type = parameters[ 0 ].ParameterType;

				if( type == typeof( TemplateContext ) )
				{
					args.Add( context );
					type = parameters[ 1 ].ParameterType;
				}

				if( type != typeof( object ) )
				{
					data = ToType( context, type, data );
					if( data == null )
						return null;
				}

				args.Add( data );

				if( filterArgs != null )
					args.AddRange( filterArgs );

				for( int n = args.Count; n < parameters.Length; n++ )
				{
					if( ( parameters[ n ].Attributes & ParameterAttributes.HasDefault ) != ParameterAttributes.HasDefault )
						throw new FilterParameterException( filter.Name, parameters[ n ].Name );

					args.Add( parameters[ n ].DefaultValue );
				}
			}

			try
			{
				return filter.Invoke( null, args.ToArray() );
			}
			catch( TargetInvocationException e )
			{
				throw e.InnerException;
			}
		}

		// Private tools
		static bool isNumber( object value )
		{
			return	value is int || value is decimal || value is float || value is double ||
					value is SByte || value is Int16 || value is Int32 || value is Int64 ||
					value is Byte || value is UInt16 || value is UInt32 || value is UInt64;
		}
	}
}