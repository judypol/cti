using System;
using System.Text;

namespace PLAgent
{
	public sealed class Base64
	{
		public static string EncodeBase64(Encoding code_type, string source)
		{
			string encode = "";
			byte[] bytes = code_type.GetBytes(source);
			try
			{
				encode = Convert.ToBase64String(bytes);
			}
			catch
			{
				encode = source;
			}
			return encode;
		}

		public static string EncodeBase64(string source)
		{
			return Base64.EncodeBase64(Encoding.UTF8, source);
		}

		public static string DecodeBase64(Encoding code_type, string result)
		{
			string decode = "";
			byte[] bytes = Convert.FromBase64String(result);
			try
			{
				decode = code_type.GetString(bytes);
			}
			catch
			{
				decode = result;
			}
			return decode;
		}

		public static string DecodeBase64(string result)
		{
			return Base64.DecodeBase64(Encoding.UTF8, result);
		}
	}
}
