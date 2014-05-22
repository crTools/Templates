using System;

namespace crTools.Templates
{
	[AttributeUsage(AttributeTargets.Class)]
	public class TemplateAutoRegisterAttribute : Attribute
	{
		public TemplateAutoRegisterAttribute()
		{
		}
	}
}