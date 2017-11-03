using System;
using System.Configuration;

namespace PLClient
{
	public class Md5keySection : ConfigurationSection
	{
		[ConfigurationProperty("md5key", IsRequired = true)]
		public Md5TextElement md5key
		{
			get
			{
				return (Md5TextElement)base["md5key"];
			}
		}
	}
}
