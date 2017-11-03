using System;
using System.ComponentModel;
using System.Reflection;

namespace PLAgentDll
{
	public static class EnumHelper
	{
		public static string GetEnumDescription(object enumSubitem)
		{
			enumSubitem = (Enum)enumSubitem;
			string text = enumSubitem.ToString();
			FieldInfo field = enumSubitem.GetType().GetField(text);
			string result;
			if (field != null)
			{
				object[] customAttributes = field.GetCustomAttributes(typeof(DescriptionAttribute), false);
				if (customAttributes == null || customAttributes.Length == 0)
				{
					result = text;
				}
				else
				{
					DescriptionAttribute descriptionAttribute = (DescriptionAttribute)customAttributes[0];
					result = descriptionAttribute.Description;
				}
			}
			else
			{
				result = "";
			}
			return result;
		}
	}
}
