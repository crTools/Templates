using System;
using System.Collections.Generic;

namespace crTools.Templates.Tags
{
	[TemplateAutoRegister]
	public class Switch : Elements.Block
	{
		public Switch( TemplateSettings settings, string name, string args )
			: base( settings, name, args )
		{
			_expression = args;
		}

		public override void Render( TemplateContext context )
		{
			var left = context.Evaluate( _expression );

			foreach( Condition condition in _conditions )
			{
				if( condition.Check( context, left ) )
				{
					condition.Render( context );
					break;
				}
			}
		}

		public override void AddElement( Elements.IElement element )
		{
			if( _conditions.Count > 0 )
				_conditions[ _conditions.Count - 1 ].AddElement( element );
		}

		public override bool CheckOtherTag( string name, string args )
		{
			if( string.Equals( name, "Case", StringComparison.InvariantCultureIgnoreCase ) ||
				string.Equals( name, "When", StringComparison.InvariantCultureIgnoreCase ) )
			{
				_conditions.Add( new Condition( args ) );
				return true;
			}
			else if( string.Equals( name, "Default", StringComparison.InvariantCultureIgnoreCase ) ||
					 string.Equals( name, "Else", StringComparison.InvariantCultureIgnoreCase ) )
			{
				_conditions.Add( new Condition( null ) );
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

			public bool Check( TemplateContext context, object left )
			{
				if( _expression == null )
					return true;

				object right = context.Evaluate( _expression );
				return Parsing.Operations.Compare( context, left, right ) == 0;
			}

			readonly string _expression;
		}

		readonly string			 _expression;
		readonly List<Condition> _conditions = new List<Condition>();
	}
}