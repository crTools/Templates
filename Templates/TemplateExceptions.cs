using System;

namespace crTools.Templates
{
	// Exception classes
	public class TemplateException : ApplicationException
	{
		public TemplateException( string message )
			: base( TemplateErrors.TemplateException + message )
		{
		}

		public TemplateException( string message, params object[] args )
			: this( string.Format( message, args ) )
		{
		}

		protected static string getTypeName( object value )
		{
			if( value == null )
				return "";

			if( value is Type )
				return ( ( Type )value ).Name;

			return value.GetType().Name;
		}
	}

	public class UnknownTagException : TemplateException
	{
		public UnknownTagException( string tag )
			: base( string.Format( TemplateErrors.UnknownTag, tag ) )
		{
		}
	}

	public class UnexpectedTagException : TemplateException
	{
		public UnexpectedTagException( string tag )
			: base( string.Format( TemplateErrors.UnexpectedTag, tag ) )
		{
		}
	}

	public class TagEndNotFoundException : TemplateException
	{
		public TagEndNotFoundException( string tag )
			: base( string.Format( TemplateErrors.TagEndNotFound, tag ) )
		{
		}
	}

	public class TagSyntaxErrorException : TemplateException
	{
		public TagSyntaxErrorException( string tag, string text )
			: base( string.Format( TemplateErrors.TagSyntaxError, tag, text ) )
		{
		}
	}

	public class TagRenderErrorException : TemplateException
	{
		public TagRenderErrorException( string tag, string text )
			: base( string.Format( TemplateErrors.TagRenderError, tag, text ) )
		{
		}
	}

	public class UnknownTokenException : TemplateException
	{
		public UnknownTokenException( char token )
			: base( string.Format( TemplateErrors.UnknownToken, token ) )
		{
		}
	}

	public class UnexpectedTokenException : TemplateException
	{
		public UnexpectedTokenException( Parsing.Token token )
			: base( string.Format( TemplateErrors.UnexpectedToken, token ) )
		{
		}
	}

	public class TokenExpectedException : TemplateException
	{
		public TokenExpectedException( Parsing.Token token, Parsing.Token expected )
			: base( string.Format( TemplateErrors.TokenExpected, token, expected ) )
		{
		}
	}

	public class OperationUnsupportedTypeException : TemplateException
	{
		public OperationUnsupportedTypeException( string operation, object value )
			: base( string.Format( TemplateErrors.OperationUnsupportedType, operation, getTypeName( value ) ) )
		{
		}
	}

	public class OperationUnsupportedTypesException : TemplateException
	{
		public OperationUnsupportedTypesException( string operation, object value1, object value2 )
			: base( string.Format( TemplateErrors.OperationUnsupportedTypes, operation, getTypeName( value1 ), getTypeName( value2 ) ) )
		{
		}
	}

	public class OperationUnsupportedNullException : TemplateException
	{
		public OperationUnsupportedNullException( string operation )
			: base( string.Format( TemplateErrors.OperationUnsupportedNull, operation ) )
		{
		}
	}

	public class StringEndNotFoundException : TemplateException
	{
		public StringEndNotFoundException()
			: base( TemplateErrors.StringEndNotFound )
		{
		}
	}

	public class UnregonizedEscapeSequenceException : TemplateException
	{
		public UnregonizedEscapeSequenceException()
			: base( TemplateErrors.UnregonizedEscapeSequence )
		{
		}
	}

	public class InvalidIndexException : TemplateException
	{
		public InvalidIndexException( object data, object index )
			: base( string.Format( TemplateErrors.InvalidIndex, getTypeName( data ), index ) )
		{
		}
	}

	public class UnknownFilterException : TemplateException
	{
		public UnknownFilterException( string filter )
			: base( string.Format( TemplateErrors.UnknownFilter, filter ) )
		{
		}
	}

	public class FilterParameterException : TemplateException
	{
		public FilterParameterException( string filter, string parameter )
			: base( string.Format( TemplateErrors.FilterParameter, filter, parameter ) )
		{
		}
	}

	// Error strings
	public static class TemplateErrors
	{
		public const string TemplateException			= "Template error: ";
		public const string UnknownTag					= "Unknown tag '{0}' found.";
		public const string UnexpectedTag				= "Unexpected tag '{0}' found.";
		public const string TagEndNotFound				= "End tag for '{0}' not found.";
		public const string TagSyntaxError				= "Tag '{0}' syntax error: '{1}'";
		public const string TagRenderError				= "Tag '{0}' render error: '{1}'";
		public const string UnknownToken				= "Unknown token '{0}' found.";
		public const string UnexpectedToken				= "Unexpected token '{0}' found.";
		public const string TokenExpected				= "Unexpected token '{0}' found; '{1}' expected.";
		public const string OperationUnsupportedType	= "Operation '{0}' did not support type '{1}'.";
		public const string OperationUnsupportedTypes	= "Operation '{0}' did not support types '{1}' and '{2}'.";
		public const string OperationUnsupportedNull	= "Operation '{0}' did not support null value.";
		public const string StringEndNotFound			= "String literal end not found.";
		public const string UnregonizedEscapeSequence	= "Unregonized escape sequence.";
		public const string InvalidIndex				= "Invald index value '{1}' for data '{0}'.";
		public const string UnknownFilter				= "Unknown filter '{0}'.";
		public const string FilterParameter				= "Filter '{0}' need parameter '{1}'.";
		public const string ExpressionNotEnumerable		= "Expression is not enumerable.";
	}
}