using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace PLClient
{
	internal class DES
	{
		public string GenerateKey()
		{
			DESCryptoServiceProvider desCrypto = (DESCryptoServiceProvider)System.Security.Cryptography.DES.Create();
			return Encoding.ASCII.GetString(desCrypto.Key);
		}

		public string MD5Encrypt(string pToEncrypt, string sKey)
		{
			DESCryptoServiceProvider des = new DESCryptoServiceProvider();
			byte[] inputByteArray = Encoding.Default.GetBytes(pToEncrypt);
			des.Key = Encoding.ASCII.GetBytes(sKey);
			des.IV = Encoding.ASCII.GetBytes(sKey);
			MemoryStream ms = new MemoryStream();
			CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);
			cs.Write(inputByteArray, 0, inputByteArray.Length);
			cs.FlushFinalBlock();
			StringBuilder ret = new StringBuilder();
			byte[] array = ms.ToArray();
			for (int i = 0; i < array.Length; i++)
			{
				byte b = array[i];
				ret.AppendFormat("{0:X2}", b);
			}
			ret.ToString();
			return ret.ToString();
		}

		public string make_md5_str(string source)
		{
			string result;
			using (MD5 md5Hash = MD5.Create())
			{
				string hash = DES.GetMd5Hash(md5Hash, source);
				result = hash;
			}
			return result;
		}

		public string MD5Decrypt(string pToDecrypt, string sKey)
		{
			string result;
			try
			{
				DESCryptoServiceProvider des = new DESCryptoServiceProvider();
				byte[] inputByteArray = new byte[pToDecrypt.Length / 2];
				for (int x = 0; x < pToDecrypt.Length / 2; x++)
				{
					int i = Convert.ToInt32(pToDecrypt.Substring(x * 2, 2), 16);
					inputByteArray[x] = (byte)i;
				}
				des.Key = Encoding.ASCII.GetBytes(sKey);
				des.IV = Encoding.ASCII.GetBytes(sKey);
				MemoryStream ms = new MemoryStream();
				CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);
				cs.Write(inputByteArray, 0, inputByteArray.Length);
				cs.FlushFinalBlock();
				StringBuilder ret = new StringBuilder();
				result = Encoding.Default.GetString(ms.ToArray());
			}
			catch
			{
				result = "";
			}
			return result;
		}

		public static string GetMd5Hash(MD5 md5Hash, string input)
		{
			byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));
			StringBuilder sBuilder = new StringBuilder();
			for (int i = 0; i < data.Length; i++)
			{
				sBuilder.Append(data[i].ToString("x2"));
			}
			return sBuilder.ToString();
		}

		public static bool VerifyMd5Hash(MD5 md5Hash, string input, string hash)
		{
			string hashOfInput = DES.GetMd5Hash(md5Hash, input);
			StringComparer comparer = StringComparer.OrdinalIgnoreCase;
			return 0 == comparer.Compare(hashOfInput, hash);
		}
	}
}
