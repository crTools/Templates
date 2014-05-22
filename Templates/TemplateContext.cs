using System;
using System.Collections.Generic;
using System.IO;

namespace crTools.Templates
{
	public class TemplateContext
	{
		// Construction
		public TemplateContext( TemplateSettings settings, TextWriter writer )
		{
			_settings = settings;
			_writer	  = writer;
		}

		// Properties
		public TemplateSettings Settings
		{
			get
			{
				return _settings;
			}
		}

		public TextWriter Writer
		{
			get
			{
				return _writer;
			}
		}

		// Operations
		public void PushData( TemplateData data )
		{
			if( data == null )
				throw new ArgumentNullException( "data" );

			_stack.Add( data );
		}

		public void PopData()
		{
			if( _stack.Count == 0 )
				throw new InvalidOperationException();

			_stack.RemoveAt( _stack.Count - 1 );
		}

		public object GetData( string name )
		{
			for( int pos = _stack.Count - 1; pos >= 0; pos-- )
			{
				object value = _stack[ pos ].Get( name );
				if( value != null )
					return value;
			}

			return null;
		}

		// Evaluate tools
		public object Evaluate( string expression )
		{
			var parser = new Parsing.Parser( this, expression );
			var result = parser.Evaluate();
			return result;
		}

		public string EvaluateToString( string expression )
		{
			return Parsing.Operations.ToString( this, Evaluate( expression ) );
		}

		public bool EvaluateToBool( string expression )
		{
			return Parsing.Operations.ToBool( this, Evaluate( expression ) );
		}

		// Private data
		readonly TemplateSettings	_settings;
		readonly TextWriter			_writer;
		readonly List<TemplateData>	_stack = new List<TemplateData>();
	}
}