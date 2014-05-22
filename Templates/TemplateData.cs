using System;
using System.Collections.Generic;

namespace crTools.Templates
{
	public class TemplateData
	{
		// Construction
		public TemplateData()
		{
		}

		// Operations
		public void Set( string name, object value )
		{
			if( string.IsNullOrWhiteSpace( name ) )
				throw new ArgumentNullException( "name" );

			if( value != null )
			{
				_data[ name ] = value;
			}
			else
			{
				_data.Remove( name );
			}
		}

		public object Get( string name )
		{
			if( string.IsNullOrWhiteSpace( name ) )
				throw new ArgumentNullException( "name" );

			object value;
			if( _data.TryGetValue( name, out value ) )
				return value;

			return null;
		}

		public void Remove( string name )
		{
			Set( name, null );
		}

		public void Clear()
		{
			_data.Clear();
		}

		// Private data
		readonly Dictionary<string, object> _data = new Dictionary<string, object>();
	}
}