using System;

namespace crTools.Templates.Parsing
{
	public sealed class Tokenizer
	{
		// Construction
		public Tokenizer( string text )
		{
			if( text == null )
				throw new ArgumentNullException( "text" );

			_text = text;
			_len  = text.Length;
			_pos  = 0;
		}

		// Operations
		public Token GetToken( out string text )
		{
			text = null;

			while( _pos < _len && char.IsWhiteSpace( _text[ _pos ] ) )
				_pos++;

			if( _pos == _len )
				return Token.End;

			char chr = _text[ _pos++ ];

			if( chr >= 'a' && chr <= 'z' || chr >= 'A' && chr <= 'Z' || chr == '_' )
				return getIdentifier( ref text );

			if( chr >= '0' && chr <= '9' )
				return getNumber( ref text );

			if( chr == '"' || chr == '\'' )
				return getString( ref text, chr );

			switch( chr )
			{
				case '(': return Token.ParLeft;
				case ')': return Token.ParRight;
				case '[': return Token.IndexOpen;
				case ']': return Token.IndexClose;
				case '.': return Token.IndexPoint;
				case '+': return Token.OperatorAdd;
				case '-': return Token.OperatorSub;
				case '*': return Token.OperatorMul;
				case '/': return Token.OperatorDiv;
				case '%': return Token.OperatorMod;
				case '!': return match( '=' ) ? Token.CompareNotEqual : Token.OperatorNot;
				case '=': match( '=' ); return Token.CompareEqual;
				case '<': return match( '>' ) ? Token.CompareNotEqual : ( match( '=' ) ? Token.CompareLowerOrEqual : Token.CompareLower );
				case '>': return match( '=' ) ? Token.CompareGreaterOrEqual : Token.CompareGreater;
				case '?': return match( '?' ) ? Token.ConditionNull : Token.ConditionFirst;
				case ':': return Token.ConditionSecond;
				case '&': if( match( '&' ) ) return Token.BoolAnd; break;
				case '|': return match( '|' ) ? Token.BoolOr : Token.Filter;
				case ',': return Token.ArgsSeparator;
			}

			throw new UnknownTokenException( chr );
		}

		// Private operations
		Token getIdentifier( ref string text )
		{
			int pos = _pos - 1;

			while( _pos < _len )
			{
				char chr = _text[ _pos ];

				if( !( chr >= 'a' && chr <= 'z' || chr >= 'A' && chr <= 'Z' || chr >= '0' && chr <= '9' || chr == '_' ) )
					break;

				_pos++;
			}

			string ident = _text.Substring( pos, _pos - pos );

			if( ident.Equals( "not", StringComparison.InvariantCultureIgnoreCase ) )
				return Token.OperatorNot;

			if( ident.Equals( "and", StringComparison.InvariantCultureIgnoreCase ) )
				return Token.BoolAnd;

			if( ident.Equals( "or", StringComparison.InvariantCultureIgnoreCase ) )
				return Token.BoolOr;

			text = ident;

			return Token.Identifier;
		}

		Token getNumber( ref string text )
		{
			bool point = false;

			int pos = _pos - 1;

			while( _pos < _len )
			{
				char chr = _text[ _pos ];

				if( chr == '.' )
				{
					if( point )
						break;

					point = true;
				}
				else if( chr < '0' || chr > '9' )
				{
					break;
				}

				_pos++;
			}

			text = _text.Substring( pos, _pos - pos );

			return point ? Token.Decimal : Token.Integer;
		}

		Token getString( ref string text, char quote )
		{
			text = "";

			bool escape = false;

			for( ;; )
			{
				if( _pos == _len )
					throw new StringEndNotFoundException();

				char chr = _text[ _pos++ ];

				if( escape )
				{
					switch( chr )
					{
						case 'n':  text += '\n'; break;
						case 'r':  text += '\r'; break;
						case 't':  text += '\t'; break;
						case '"':  text += '"'; break;
						case '\'': text += '\''; break;
						case '\\': text += '\\'; break;
						default: throw new UnregonizedEscapeSequenceException();
					}

					escape = false;
				}
				else
				{
					if( chr == quote )
						break;

					if( chr == '\\' )
					{
						escape = true;
					}
					else
					{
						text += chr;
					}
				}
			}

			return Token.String;
		}

		bool match( char chr )
		{
			if( _pos == _len )
				return false;

			if( _text[ _pos ] != chr )
				return false;

			_pos++;

			return true;
		}

		// Private data
		readonly string	_text;
		readonly int	_len;

		int _pos;
	}
}