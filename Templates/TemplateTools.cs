using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace crTools.Templates
{
	public static class TemplateTools
	{
		// Read strings from text file
		public static Dictionary<string, string> ReadStrings( string file )
		{
			return ReadStrings( file, Encoding.Default );
		}

		public static Dictionary<string, string> ReadStrings( string file, Encoding encoding )
		{
			if( string.IsNullOrWhiteSpace( file ) )
				throw new ArgumentNullException( "file" );

			if( encoding == null )
				throw new ArgumentNullException( "encoding" );

			using( var reader = new StreamReader( file, encoding, true ) )
				return ReadStrings( reader );
		}

		public static Dictionary<string, string> ReadStrings( Stream stream )
		{
			return ReadStrings( stream, Encoding.Default );
		}

		public static Dictionary<string, string> ReadStrings( Stream stream, Encoding encoding )
		{
			if( stream == null )
				throw new ArgumentNullException( "stream" );

			if( encoding == null )
				throw new ArgumentNullException( "encoding" );

			using( var reader = new StreamReader( stream, encoding, true ) )
				return ReadStrings( reader );
		}

		public static Dictionary<string, string> ReadStrings( TextReader reader )
		{
			if( reader == null )
				throw new ArgumentNullException( "reader" );

			var strings = new Dictionary<string, string>();

			for( ;; )
			{
				string line = reader.ReadLine();
				if( line == null )
					break;

				int pos = line.IndexOf( '=' );
				if( pos > 0 )
				{
					string key = line.Remove( pos ).Trim();
					string val = line.Substring( pos + 1 ).Trim();
					strings.Add( key, val );
				}
			}

			return strings;
		}
	}
}