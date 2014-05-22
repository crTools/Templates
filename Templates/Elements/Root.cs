using System;
using System.Collections.Generic;
using System.Reflection;

namespace crTools.Templates.Elements
{
	class Root : Block
	{
		public Root( TemplateSettings settings, string text )
			: base( settings, null, null )
		{
			Block block = this;
			var	  stack = new Stack<Block>();

			var match = TemplateRegex.RegexElements.Match( text );
			int last  = 0;

			while( match.Success )
			{
				if( match.Index > last )
					block.AddElement( new Literal( text.Substring( last, match.Index - last ) ) );

				if( match.Groups[ "tag" ].Value != "" )
				{
					var name = match.Groups[ "tag" ].Value;
					var args = match.Groups[ "args" ].Value;

					if( block.CheckEndTag( name, args ) )
					{
						block = stack.Pop();
					}
					else if( block.CheckOtherTag( name, args ) )
					{
					}
					else
					{
						var type = settings.FindTag( name );
						if( type == null )
							throw new UnknownTagException( name );

						IElement tag;

						try
						{
							tag = ( IElement )Activator.CreateInstance( type, settings, name, args );
						}
						catch( TargetInvocationException e )
						{
							throw e.InnerException;
						}

						block.AddElement( tag );

						if( tag is Block )
						{
							stack.Push( block );
							block = ( Block )tag;
						}
					}
				}
				else if( match.Groups[ "var" ].Value != "" )
				{
					block.AddElement( new Variable( match.Groups[ "var" ].Value ) );
				}
				else if( match.Groups[ "quick" ].Value != "" )
				{
					block.AddElement( new Quick( match.Groups[ "quick" ].Value ) );
				}

				last = match.Index + match.Length;

				match = match.NextMatch();
			}

			if( stack.Count != 0 )
				throw new TagEndNotFoundException( block.Name );

			if( last < text.Length )
				block.AddElement( new Literal( text.Substring( last, text.Length - last ) ) );
		}

		public override bool CheckEndTag( string name, string param )
		{
			return false;
		}
	}
}