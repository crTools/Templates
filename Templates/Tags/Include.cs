using System.IO;

namespace crTools.Templates.Tags
{
	[TemplateAutoRegister]
	public class Include : Elements.Tag
	{
		public Include( TemplateSettings settings, string name, string args )
			: base( settings, name, args )
		{
			// Check for static filename
			var tokenizer = new Parsing.Tokenizer( args );

			string file = null;
			if( tokenizer.GetToken( out file ) == Parsing.Token.String )
			{
				string temp = null;
				if( tokenizer.GetToken( out temp ) == Parsing.Token.End )
					_elements = read( settings, file );
			}

			if( _elements == null )
				_expression = args;
		}

		public override void Render( TemplateContext context )
		{
			if( _elements != null )
			{
				_elements.Render( context );
			}
			else
			{
				read( context.Settings, context.EvaluateToString( _expression ) ).Render( context );
			}
		}

		Elements.Block read( TemplateSettings settings, string file )
		{
			file = Path.GetFileName( file ); // Prevent directory changing!

			file = settings.FindFile( file );
			if( file == null )
				throw new FileNotFoundException();

			using( var reader = new StreamReader( file, settings.CurrentEncoding, true ) )
				return new Elements.Root( settings, reader.ReadToEnd() );
		}

		readonly Elements.Block	_elements;
		readonly string			_expression;
	}
}