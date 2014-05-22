using System;
using System.Collections.Generic;

namespace crTools.Templates.Elements
{
	public abstract class Block : Tag
	{
		public Block( TemplateSettings settings, string name, string args )
			: base( settings, name, args )
		{
		}

		public override void Render( TemplateContext context )
		{
			RenderElements( context );
		}

		public virtual void RenderElements( TemplateContext context )
		{
			foreach( var element in _elements )
				element.Render( context );
		}

		public virtual void AddElement( IElement element )
		{
			_elements.Add( element );
		}

		public virtual bool CheckEndTag( string name, string args )
		{
			return name.Equals( "End", StringComparison.InvariantCultureIgnoreCase ) ||
				   name.Equals( "End" + Name, StringComparison.InvariantCultureIgnoreCase ) ||
				   name.Equals( Name + "End", StringComparison.InvariantCultureIgnoreCase );
		}

		public virtual bool CheckOtherTag( string name, string args )
		{
			return false;
		}

		readonly List<IElement> _elements = new List<IElement>();
	}
}