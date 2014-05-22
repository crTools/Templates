using System;
using System.Globalization;
using System.IO;
using System.Linq;
using crTools.Templates;

namespace crTools
{
	class Program
	{
		static void Main( string[] args )
		{
			string path = Environment.CurrentDirectory;
			while( !Directory.Exists( Path.Combine( path, "Input" ) ) )
				path = Path.GetDirectoryName( path );

			string inputPath  = Path.Combine( path, "Input" );
			string outputPath = Path.Combine( path, "Output" );

			Template.Global.RegisterPath( inputPath );
			Template.Global.Culture = CultureInfo.InvariantCulture;

			Template.Global.Data.Set( "Lang", TemplateTools.ReadStrings( Path.Combine( inputPath, "Strings.txt" ) ) );
			Template.Global.QuickData = Template.Global.Data.Get( "Lang" );

			var data = new TemplateData();
			data.Set( "User", Environment.UserName );
			data.Set( "Version", Environment.OSVersion.Version );

			var query = from server in new int[] { 10, 20, 50, 51, 52, 53, 54 }
						select new { Server = "Server" + server, Number = server, Status = "OK" };
			data.Set( "Servers", query );

			var drives = DriveInfo.GetDrives().Where( drive => drive.IsReady );
			data.Set( "Drives", drives );

			foreach( string input in Directory.GetFiles( inputPath, "*.htm" ) )
			{
				var output	 = Path.Combine( outputPath, Path.GetFileName( input ) );
				var template = Template.FromFile( input, data );
				var result	 = template.Render();

				File.WriteAllText( output, result, template.CurrentEncoding );
			}
		}
	}
}