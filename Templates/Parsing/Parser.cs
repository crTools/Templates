using System;
using System.Collections;
using System.Globalization;

namespace crTools.Templates.Parsing
{
	public sealed class Parser
	{
		// Construction
		public Parser( TemplateContext context, string expression )
		{
			if( context == null )
				throw new ArgumentNullException( "context" );

			if( expression == null )
				throw new ArgumentNullException( "expression" );

			_context   = context;
			_tokenizer = new Tokenizer( expression );
			_tokenType = _tokenizer.GetToken( out _tokenText );
		}

		// Operations
		public object Evaluate()
		{
			object expr = expression();
			expect( Token.End );
			return expr;
		}

		// Private operations
		object expression()
		{
			object expr = conditional_expression();

			while( match( Token.Filter ) )
				expr = filter( expr );

			return expr;
		}

		object conditional_expression()
		{
			object expr = logical_or_expression();

			if( match( Token.ConditionFirst ) )
			{
				object expr1 = conditional_expression();
				expect( Token.ConditionSecond );
				object expr2 = conditional_expression();
				expr = Operations.ToBool( _context, expr ) ? expr1 : expr2;
			}
			else if( match( Token.ConditionNull ) )
			{
				object expr1 = conditional_expression();
				if( !Operations.ToBool( _context, expr ) )
					expr = expr1;
			}

			return expr;
		}

		object logical_or_expression()
		{
			object expr = logical_and_expression();

			if( match( Token.BoolOr ) )
			{
				object expr2 = logical_or_expression();
				expr = Operations.ToBool( _context, expr ) || Operations.ToBool( _context, expr2 );
			}

			return expr;
		}

		object logical_and_expression()
		{
			object expr = equality_expression();

			if( match( Token.BoolAnd ) )
			{
				object expr2 = logical_and_expression();
				expr = Operations.ToBool( _context, expr ) && Operations.ToBool( _context, expr2 );
			}

			return expr;
		}

		object equality_expression()
		{
			object expr = relational_expression();

			if( match( Token.CompareEqual ) )
			{
				object expr2 = equality_expression();
				expr = Operations.Compare( _context, expr, expr2 ) == 0;
			}
			else if( match( Token.CompareNotEqual ) )
			{
				object expr2 = equality_expression();
				expr = Operations.Compare( _context, expr, expr2 ) != 0;
			}

			return expr;
		}

		object relational_expression()
		{
			object expr = additive_expression();

			if( match( Token.CompareLower ) )
			{
				object expr2 = relational_expression();
				expr = Operations.Compare( _context, expr, expr2 ) < 0;
			}
			else if( match( Token.CompareLowerOrEqual ) )
			{
				object expr2 = relational_expression();
				expr = Operations.Compare( _context, expr, expr2 ) <= 0;
			}
			if( match( Token.CompareGreater ) )
			{
				object expr2 = relational_expression();
				expr = Operations.Compare( _context, expr, expr2 ) > 0;
			}
			else if( match( Token.CompareGreaterOrEqual ) )
			{
				object expr2 = relational_expression();
				expr = Operations.Compare( _context, expr, expr2 ) >= 0;
			}

			return expr;
		}

		object additive_expression()
		{
			object expr = multiplicative_expression();

			if( match( Token.OperatorAdd ) )
			{
				object expr2 = additive_expression();
				expr = Operations.Add( _context, expr, expr2 );
			}
			else if( match( Token.OperatorSub ) )
			{
				object expr2 = additive_expression();
				expr = Operations.Sub( _context, expr, expr2 );
			}

			return expr;
		}

		object multiplicative_expression()
		{
			object expr = unary_expression();

			if( match( Token.OperatorMul ) )
			{
				object expr2 = multiplicative_expression();
				expr = Operations.Mul( _context, expr, expr2 );
			}
			else if( match( Token.OperatorDiv ) )
			{
				object expr2 = multiplicative_expression();
				expr = Operations.Div( _context, expr, expr2 );
			}
			else if( match( Token.OperatorMod ) )
			{
				object expr2 = multiplicative_expression();
				expr = Operations.Mod( _context, expr, expr2 );
			}

			return expr;
		}

		object unary_expression()
		{
			if( match( Token.OperatorAdd ) )
			{
				return unary_expression();
			}
			else if( match( Token.OperatorSub ) )
			{
				return Operations.Inv( _context, unary_expression() );
			}
			else if( match( Token.OperatorNot ) )
			{
				return !Operations.ToBool( _context, unary_expression() );
			}
			else
			{
				return postfix_expression();
			}
		}

		object postfix_expression()
		{
			object expr = primary_expression();

			for( ;; )
			{
				if( match( Token.IndexOpen ) )
				{
					object index = expression();
					expect( Token.IndexClose );
					expr = Operations.Index( _context, expr, index );
				}
				else if( match( Token.IndexPoint ) )
				{
					expect( Token.Identifier );
					expr = Operations.Index( _context, expr, _matchText );
				}
				else
				{
					break;
				}
			}

			return expr;
		}

		object primary_expression()
		{
			if( match( Token.Identifier ) )
			{
				return _context.GetData( _matchText );
			}
			else if( match( Token.String ) )
			{
				return _matchText;
			}
			else if( match( Token.Integer ) )
			{
				return int.Parse( _matchText, CultureInfo.InvariantCulture );
			}
			else if( match( Token.Decimal ) )
			{
				return decimal.Parse( _matchText, CultureInfo.InvariantCulture );
			}
			else if( match( Token.ParLeft ) )
			{
				object expr = expression();
				expect( Token.ParRight );
				return expr;
			}
			else
			{
				throw new UnexpectedTokenException( _tokenType );
			}
		}

		object filter( object expr )
		{
			if( match( Token.String ) )
			{
				return filter_convert( expr, _matchText );
			}
			else if( match( Token.Identifier ) )
			{
				return filter_method( expr, _matchText );
			}
			else
			{
				throw new UnexpectedTokenException( _tokenType );
			}
		}

		object filter_convert( object expr, string format )
		{
			IFormatProvider provider;

			if( match( Token.ArgsSeparator ) )
			{
				expect( Token.String );
				provider = new CultureInfo( _matchText );
			}
			else
			{
				provider = _context.Settings.CurrentCulture;
			}

			if( expr != null )
				if( expr is IFormattable )
					expr = ( ( IFormattable )expr ).ToString( format, provider );

			return expr;
		}

		object filter_method( object expr, string name )
		{
			var method = _context.Settings.FindFilter( name );
			if( method == null )
				throw new UnknownFilterException( name );

			object[] args = null;

			if( match( Token.ParLeft ) )
			{
				if( !match( Token.ParRight ) )
				{
					args = filter_args();
					expect( Token.ParRight );
				}
			}
			else if( match( Token.ConditionSecond ) )
			{
				args = filter_args();
			}

			return Operations.Filter( _context, expr, method, args );
		}

		object[] filter_args()
		{
			var args = new ArrayList();

			do
			{
				args.Add( expression() );
			}
			while( match( Token.ArgsSeparator ) );

			return args.ToArray();
		}

		// Private tools
		bool match( Token token )
		{
			if( _tokenType != token )
				return false;

			_matchText = _tokenText;
			_tokenType = _tokenizer.GetToken( out _tokenText );

			return true;
		}

		void expect( Token token )
		{
			if( !match( token ) )
				throw new TokenExpectedException( _tokenType, token );
		}

		// Private data
		readonly TemplateContext _context;
		readonly Tokenizer		 _tokenizer;

		Token  _tokenType;
		string _tokenText;
		string _matchText;
	}
}