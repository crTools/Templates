using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text;

namespace crTools.Templates
{
	public class TemplateSettings
	{
		// Construction
		public TemplateSettings( TemplateSettings parent = null )
		{
			_parent = parent;
		}

		// Properties
		public TemplateData Data
		{
			get
			{
				return _data;
			}
		}

		public object QuickData
		{
			get
			{
				return _quickData;
			}
			set
			{
				_quickData = value;
			}
		}

		public object CurrentQuickData
		{
			get
			{
				for( var settings = this; settings != null; settings = settings._parent )
					if( settings._quickData != null )
						return settings._quickData;

				return null;
			}
		}

		public CultureInfo Culture
		{
			get
			{
				return _culture;
			}
			set
			{
				_culture = value;
			}
		}

		public CultureInfo CurrentCulture
		{
			get
			{
				for( var settings = this; settings != null; settings = settings._parent )
					if( settings._culture != null )
						return settings._culture;

				return CultureInfo.CurrentCulture;
			}
		}

		public Encoding Encoding
		{
			get
			{
				return _encoding;
			}
			set
			{
				_encoding = value;
			}
		}

		public Encoding CurrentEncoding
		{
			get
			{
				for( var settings = this; settings != null; settings = settings._parent )
					if( settings._encoding != null )
						return settings._encoding;

				return Encoding.Default;
			}
		}

		// Operations
		public void AutoRegister( Assembly assembly )
		{
			foreach( var type in assembly.GetExportedTypes() )
			{
				if( type.GetCustomAttributes( typeof( TemplateAutoRegisterAttribute ), false ).Length > 0 )
				{
					if( type.IsSubclassOf( typeof( Elements.Tag ) ) )
					{
						RegisterTag( type );
					}
					else
					{
						RegisterFilters( type );
					}
				}
			}
		}

		public void RegisterTag( Type type )
		{
			if( type == null )
				throw new ArgumentNullException( "type" );

			_tags.Add( type );
		}

		public void UnregisterTag( Type type )
		{
			if( type == null )
				throw new ArgumentNullException( "type" );

			_tags.Remove( type );
		}

		public void ClearTags()
		{
			_tags.Clear();
		}

		public Type FindTag( string name )
		{
			if( string.IsNullOrWhiteSpace( name ) )
				throw new ArgumentNullException( "name" );

			for( var settings = this; settings != null; settings = settings._parent )
				foreach( var type in settings._tags )
					if( name.Equals( type.Name, StringComparison.InvariantCultureIgnoreCase ) )
						return type;

			return null;
		}

		public void RegisterFilters( Type type )
		{
			if( type == null )
				throw new ArgumentNullException( "type" );

			foreach( var method in type.GetMethods( BindingFlags.Public|BindingFlags.Static ) )
				_filters.Add( method );
		}

		public void UnregisterFilters( Type type )
		{
			if( type == null )
				throw new ArgumentNullException( "type" );

			foreach( var method in type.GetMethods( BindingFlags.Public | BindingFlags.Static ) )
				_filters.Remove( method );
		}

		public void ClearFilters()
		{
			_filters.Clear();
		}

		public MethodInfo FindFilter( string name )
		{
			if( string.IsNullOrWhiteSpace( name ) )
				throw new ArgumentNullException( "name" );

			name = name.Replace( "_", "" );

			for( var settings = this; settings != null; settings = settings._parent )
				foreach( var method in settings._filters )
					if( name.Equals( method.Name, StringComparison.InvariantCultureIgnoreCase ) )
						return method;

			return null;
		}

		public void RegisterPath( string path )
		{
			if( string.IsNullOrWhiteSpace( path ) )
				throw new ArgumentNullException( "path" );

			_paths.Add( path );
		}

		public void UnregisterPath( string path )
		{
			if( string.IsNullOrWhiteSpace( path ) )
				throw new ArgumentNullException( "path" );

			_paths.Remove( path );
		}

		public void ClearPaths()
		{
			_paths.Clear();
		}

		public string FindFile( string file )
		{
			if( string.IsNullOrWhiteSpace( file ) )
				throw new ArgumentNullException( "file" );

			if( File.Exists( file ) )
				return file;

			for( var settings = this; settings != null; settings = settings._parent )
			{
				foreach( var path in settings._paths )
				{
					string full = Path.Combine( path, file );
					if( File.Exists( full ) )
						return full;
				}
			}

			return null;
		}

		// Private data
		readonly TemplateSettings	_parent;
		readonly TemplateData		_data		= new TemplateData();
		readonly List<Type>			_tags		= new List<Type>();
		readonly List<MethodInfo>	_filters	= new List<MethodInfo>();
		readonly List<string>		_paths		= new List<string>();

		object		_quickData = null;
		CultureInfo _culture   = null;
		Encoding	_encoding  = null;
	}
}