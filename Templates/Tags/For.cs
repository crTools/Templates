using System;
using System.Collections;

namespace crTools.Templates.Tags
{
	[TemplateAutoRegister]
	public class For : Elements.Block
	{
		public For( TemplateSettings settings, string name, string args )
			: base( settings, name, args )
		{
			var match = TemplateRegex.RegexForTag.Match( args );
			if( !match.Success )
				throw new TagSyntaxErrorException( "For", args );

			_variable	= match.Groups[ 1 ].Value;
			_expression	= match.Groups[ 2 ].Value;
		}

		public override void Render( TemplateContext context )
		{
			var result = context.Evaluate( _expression );

			if( result == null )
			{
				if( _else  != null )
					_else.Render( context );
			}
			else if( result is IEnumerable )
			{
				var count = Parsing.Operations.GetCount( result );
				if( count > 0 )
				{
					var state = new State( count );

					var data = new TemplateData();
					data.Set( "forloop", state );
					data.Set( _variable + "_for", state );

					context.PushData( data );

					foreach( object item in ( IEnumerable )result )
					{
						data.Set( _variable, item );
						RenderElements( context );
						state.Next();
					}

					context.PopData();
				}
				else
				{
					if( _else != null )
						_else.Render( context );
				}
			}
			else
			{
				throw new TagRenderErrorException( "For", TemplateErrors.ExpressionNotEnumerable );
			}
		}

		public override void AddElement( Elements.IElement element )
		{
			if( _else == null )
			{
				base.AddElement( element );
			}
			else
			{
				_else.AddElement( element );
			}
		}

		public override bool CheckOtherTag( string name, string args )
		{
			if( string.Equals( name, "Else", StringComparison.InvariantCultureIgnoreCase ) )
			{
				if( _else != null )
					throw new UnexpectedTagException( name );

				_else = new Else();
				return true;
			}

			return false;
		}

		class State
		{
			public State( int count )
			{
				_count = count;
				_pos   = 0;
			}

			public void Next()
			{
				_pos++;
			}

			public int Count
			{
				get
				{
					return _count;
				}
			}

			public int Length
			{
				get
				{
					return _count;
				}
			}

			public int Index
			{
				get
				{
					return _pos + 1;
				}
			}

			public int Index0
			{
				get
				{
					return _pos;
				}
			}

			public int RIndex
			{
				get
				{
					return _count - _pos;
				}
			}

			public int RIndex0
			{
				get
				{
					return _count - _pos - 1;
				}
			}

			public bool First
			{
				get
				{
					return _pos == 0;
				}
			}

			public bool Last
			{
				get
				{
					return _pos == _count - 1;
				}
			}

			public bool Odd
			{
				get
				{
					return _pos % 2 > 0;
				}
			}

			public override string ToString()
			{
				return Index + "/" + Count;
			}

			readonly int _count;

			int _pos;
		}

		class Else : Elements.Block
		{
			public Else()
				: base( null, null, null )
			{
			}
		}

		readonly string _variable;
		readonly string _expression;

		Else _else;
	}
}