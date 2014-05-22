namespace crTools.Templates.Elements
{
	public class Quick : IElement
	{
		public Quick( string path )
		{
			_path = path;
		}

		public void Render( TemplateContext context )
		{
			var data = context.Settings.CurrentQuickData;
			var root = data == null;

			var match = TemplateRegex.RegexQuickIndexes.Match( _path );
			while( match.Success )
			{
				var index = match.Groups[ 1 ].Value;
				if( index != "" )
				{
					if( root )
					{
						root = false;
						data = context.GetData( index );
					}
					else
					{
						data = Parsing.Operations.Index( context, data, index );
					}
				}

				match = match.NextMatch();
			}

			context.Writer.Write( Parsing.Operations.ToString( context, data ) );
		}

		readonly string _path;
	}
}