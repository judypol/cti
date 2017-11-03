using System;
using System.Configuration;
using System.Xml;

namespace PLClient
{
	public class Md5TextElement : ConfigurationElement
	{
		[ConfigurationProperty("data", IsRequired = false)]
		public string CommandText
		{
			get
			{
				return base["data"].ToString();
			}
			set
			{
				base["data"] = value;
			}
		}

		protected override void DeserializeElement(XmlReader reader, bool serializeCollectionKey)
		{
			this.CommandText = (reader.ReadElementContentAs(typeof(string), null) as string);
		}

		protected override bool SerializeElement(XmlWriter writer, bool serializeCollectionKey)
		{
			if (writer != null)
			{
				writer.WriteCData(this.CommandText);
			}
			return true;
		}
	}
}
