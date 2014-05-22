using System;
using System.Collections.Generic;

namespace crTools.Templates.Tags
{
	[TemplateAutoRegister]
	public class If : Elements.Block
	{
		public If( TemplateSettings settings, string name, string args )
			: base( settings, name, args )
		{
			_conditions.Add( new Condition( args ) );
		}

		public override void Render( TemplateContext context )
		{
			foreach( Condition condition in _conditions )
			{
				if( condition.Check( context ) )
				{
					condition.Render( context );
					return;
				}
			}

			if( _else != null )
				_else.Render( context );
		}

		public override void AddElement( Elements.IElement element )
		{
			if( _else == null )
			{
				_conditions[ _conditions.Count - 1 ].AddElement( element );
			}
			else
			{
				_else.AddElement( element );
			}
		}

		public override bool CheckOtherTag( string name, string args )
		{
			if( string.Equals( name, "ElseIf", StringComparison.InvariantCultureIgnoreCase ) ||
				string.Equals( name, "ElsIf", StringComparison.InvariantCultureIgnoreCase ) ||
				string.Equals( name, "ElIf", StringComparison.InvariantCultureIgnoreCase ) )
			{
				_conditions.Add( new Condition( args ) );
				return true;
			}
			else if( string.Equals( name, "Else", StringComparison.InvariantCultureIgnoreCase ) )
			{
				if( _else != null )
					throw new UnexpectedTagException( name );

				_else = new Else();
				return true;
			}

			return false;
		}

		class Condition : Elements.Block
		{
			public Condition( string expression )
				: base( null, null, null )
			{
				_expression = expression;
			}

			public bool Check( TemplateContext context )
			{
				if( _expression == null )
					return true;

				return context.EvaluateToBool( _expression );
			}

			readonly string _expression;
		}

		class Else : Elements.Block
		{
			public Else()
				: base( null, null, null )
			{
			}
		}

		readonly List<Condition> _conditions = new List<Condition>();

		Else _else;
	}
}