using System;
using System.IO;
using System.Text;

namespace crTools.Templates
{
	public class Template : TemplateSettings
	{
		// Static tools
		public static Template FromText( string text, TemplateData data = null )
		{
			var template = new Template( data );
			template.Parse( text );
			return template;
		}

		public static Template FromFile( string file, TemplateData data = null )
		{
			var template = new Template( data );
			template.Read( file );
			return template;
		}

		public static Template FromStream( Stream stream, TemplateData data = null )
		{
			var template = new Template( data );
			template.Read( stream );
			return template;
		}

		public static string RenderFromText( string text, TemplateData data = null )
		{
			return FromText( text, data ).Render();
		}

		public static string RenderFromFile( string file, TemplateData data = null )
		{
			return FromFile( file, data ).Render();
		}

		public static string RenderFromStream( Stream stream, TemplateData data = null )
		{
			return FromStream( stream, data ).Render();
		}

		// Construction
		public Template( TemplateData data = null )
			: base( _global )
		{
			_data = data;
		}

		// Operations
		public void Read( string file )
		{
			if( string.IsNullOrWhiteSpace( file ) )
				throw new ArgumentNullException( "file" );

			string path = FindFile( file );
			if( path == null )
				throw new FileNotFoundException();

			using( var reader = new StreamReader( path, CurrentEncoding, true ) )
				Parse( reader.ReadToEnd() );
		}

		public void Read( Stream stream )
		{
			if( stream == null )
				throw new ArgumentNullException( "stream" );

			using( var reader = new StreamReader( stream, CurrentEncoding, true ) )
				Parse( reader.ReadToEnd() );
		}

		public void Read( TextReader reader )
		{
			if( reader == null )
				throw new ArgumentNullException( "reader" );

			Parse( reader.ReadToEnd() );
		}

		public void Parse( string text )
		{
			if( text == null )
				throw new ArgumentNullException( "text" );

			_elements = new Elements.Root( this, text );
		}

		public void Render( TextWriter writer, TemplateData data = null )
		{
			var context = new TemplateContext( this, writer );

			context.PushData( _global.Data );

			if( _data != null )
				context.PushData( _data );

			if( data != null )
				context.PushData( data );

			context.PushData( Data );

			_elements.Render( context );
		}

		public string Render( TemplateData data = null )
		{
			var builder = new StringBuilder();

			using( var writer = new StringWriter( builder ) )
				Render( writer, data );

			return builder.ToString();
		}

		// Static properties
		public static TemplateSettings Global
		{
			get
			{
				return _global;
			}
		}

		// Static construction
		static Template()
		{
			_global = new TemplateSettings( null );
			_global.AutoRegister( typeof( Template ).Assembly );
		}

		// Private data
		readonly TemplateData _data;

		Elements.Block _elements;

		static TemplateSettings _global;
	}
}