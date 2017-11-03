using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization.Json;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Script.Serialization;

namespace PLAgentBar
{
	public static class ComFunc
	{
		public static string VECTOR_STR = "^SjQM1J9";

		public static string mTodayDate = "";

		public static string mDESKey = "";

		public static string APPDATA_PATH = Environment.GetEnvironmentVariable("APPDATA");

		public static string SOFTPHONE_CONFIG_FOLDER_PATH = ComFunc.APPDATA_PATH + "\\wonderUsers\\";

		public static string object2Json(object dic)
		{
			return new JavaScriptSerializer().Serialize(dic);
		}

		public static string Obj2JSON<T>(T obj)
		{
			string result = string.Empty;
			try
			{
				DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(T));
				using (MemoryStream ms = new MemoryStream())
				{
					serializer.WriteObject(ms, obj);
					result = Encoding.UTF8.GetString(ms.ToArray());
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return result;
		}

		public static string List2JSON<T>(List<T> vals)
		{
			StringBuilder st = new StringBuilder();
			try
			{
				DataContractJsonSerializer s = new DataContractJsonSerializer(typeof(T));
				foreach (T city in vals)
				{
					using (MemoryStream ms = new MemoryStream())
					{
						s.WriteObject(ms, city);
						st.Append(Encoding.UTF8.GetString(ms.ToArray()));
					}
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return st.ToString();
		}

		public static T ParseFormByJson<T>(string jsonStr)
		{
			T obj = Activator.CreateInstance<T>();
			T result;
			using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(jsonStr)))
			{
				DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(T));
				result = (T)((object)serializer.ReadObject(ms));
			}
			return result;
		}

		public static string ListToJson<T>(IList<T> list, string jsonName)
		{
			StringBuilder Json = new StringBuilder();
			if (string.IsNullOrEmpty(jsonName))
			{
				T t = list[0];
				jsonName = t.GetType().Name;
			}
			Json.Append("{\"" + jsonName + "\":[");
			if (list.Count > 0)
			{
				for (int i = 0; i < list.Count; i++)
				{
					T obj = Activator.CreateInstance<T>();
					FieldInfo[] pi = obj.GetType().GetFields();
					Json.Append("{");
					for (int j = 0; j < pi.Length; j++)
					{
						string fieldValue = "";
						Type type;
						if (pi[j].GetValue(list[i]) != null)
						{
							type = pi[j].GetValue(list[i]).GetType();
							fieldValue = pi[j].GetValue(list[i]).ToString();
						}
						else
						{
							type = null;
						}
						Json.Append("\"" + pi[j].Name.ToString() + "\":" + ComFunc.StringFormat(fieldValue, type));
						if (j < pi.Length - 1)
						{
							Json.Append(",");
						}
					}
					Json.Append("}");
					if (i < list.Count - 1)
					{
						Json.Append(",");
					}
				}
			}
			Json.Append("]}");
			return Json.ToString();
		}

		public static string ListToJson<T>(IList<T> list)
		{
			object obj = list[0];
			return ComFunc.ListToJson<T>(list, obj.GetType().Name);
		}

		public static string ToJson(object jsonObject)
		{
			string jsonString = "{";
			PropertyInfo[] propertyInfo = jsonObject.GetType().GetProperties();
			for (int i = 0; i < propertyInfo.Length; i++)
			{
				object objectValue = propertyInfo[i].GetGetMethod().Invoke(jsonObject, null);
				string value = string.Empty;
				if (objectValue is DateTime || objectValue is Guid || objectValue is TimeSpan)
				{
					value = "'" + objectValue.ToString() + "'";
				}
				else if (objectValue is string)
				{
					value = "'" + ComFunc.ToJson(objectValue.ToString()) + "'";
				}
				else if (objectValue is IEnumerable)
				{
					value = ComFunc.ToJson((IEnumerable)objectValue);
				}
				else
				{
					value = ComFunc.ToJson(objectValue.ToString());
				}
				string text = jsonString;
				jsonString = string.Concat(new string[]
				{
					text,
					"\"",
					ComFunc.ToJson(propertyInfo[i].Name),
					"\":",
					value,
					","
				});
			}
			jsonString.Remove(jsonString.Length - 1, jsonString.Length);
			return jsonString + "}";
		}

		public static string ToJson(IEnumerable array)
		{
			string jsonString = "[";
			foreach (object item in array)
			{
				jsonString = jsonString + ComFunc.ToJson(item) + ",";
			}
			jsonString.Remove(jsonString.Length - 1, jsonString.Length);
			return jsonString + "]";
		}

		public static string ToArrayString(IEnumerable array)
		{
			string jsonString = "[";
			foreach (object item in array)
			{
				jsonString = ComFunc.ToJson(item.ToString()) + ",";
			}
			jsonString.Remove(jsonString.Length - 1, jsonString.Length);
			return jsonString + "]";
		}

		public static string ToJson(DataTable dt)
		{
			StringBuilder jsonString = new StringBuilder();
			jsonString.Append("[");
			DataRowCollection drc = dt.Rows;
			for (int i = 0; i < drc.Count; i++)
			{
				jsonString.Append("{");
				for (int j = 0; j < dt.Columns.Count; j++)
				{
					string strKey = dt.Columns[j].ColumnName;
					string strValue = drc[i][j].ToString();
					Type type = dt.Columns[j].DataType;
					jsonString.Append("\"" + strKey + "\":");
					strValue = ComFunc.StringFormat(strValue, type);
					if (j < dt.Columns.Count - 1)
					{
						jsonString.Append(strValue + ",");
					}
					else
					{
						jsonString.Append(strValue);
					}
				}
				jsonString.Append("},");
			}
			jsonString.Remove(jsonString.Length - 1, 1);
			jsonString.Append("]");
			return jsonString.ToString();
		}

		public static string ToJson(DataTable dt, string jsonName)
		{
			StringBuilder Json = new StringBuilder();
			if (string.IsNullOrEmpty(jsonName))
			{
				jsonName = dt.TableName;
			}
			Json.Append("{\"" + jsonName + "\":[");
			if (dt.Rows.Count > 0)
			{
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					Json.Append("{");
					for (int j = 0; j < dt.Columns.Count; j++)
					{
						Type type = dt.Rows[i][j].GetType();
						Json.Append("\"" + dt.Columns[j].ColumnName.ToString() + "\":" + ComFunc.StringFormat(dt.Rows[i][j].ToString(), type));
						if (j < dt.Columns.Count - 1)
						{
							Json.Append(",");
						}
					}
					Json.Append("}");
					if (i < dt.Rows.Count - 1)
					{
						Json.Append(",");
					}
				}
			}
			Json.Append("]}");
			return Json.ToString();
		}

		public static string ToJson(DbDataReader dataReader)
		{
			StringBuilder jsonString = new StringBuilder();
			jsonString.Append("[");
			while (dataReader.Read())
			{
				jsonString.Append("{");
				for (int i = 0; i < dataReader.FieldCount; i++)
				{
					Type type = dataReader.GetFieldType(i);
					string strKey = dataReader.GetName(i);
					string strValue = dataReader[i].ToString();
					jsonString.Append("\"" + strKey + "\":");
					strValue = ComFunc.StringFormat(strValue, type);
					if (i < dataReader.FieldCount - 1)
					{
						jsonString.Append(strValue + ",");
					}
					else
					{
						jsonString.Append(strValue);
					}
				}
				jsonString.Append("},");
			}
			dataReader.Close();
			jsonString.Remove(jsonString.Length - 1, 1);
			jsonString.Append("]");
			return jsonString.ToString();
		}

		public static string ToJson(DataSet dataSet)
		{
			string jsonString = "{";
			foreach (DataTable table in dataSet.Tables)
			{
				string text = jsonString;
				jsonString = string.Concat(new string[]
				{
					text,
					"\"",
					table.TableName,
					"\":",
					ComFunc.ToJson(table),
					","
				});
			}
			jsonString = jsonString.TrimEnd(new char[]
			{
				','
			});
			return jsonString + "}";
		}

		public static string String2Json(string s)
		{
			StringBuilder sb = new StringBuilder();
			int i = 0;
			while (i < s.Length)
			{
				char c = s.ToCharArray()[i];
				char c2 = c;
				if (c2 <= '"')
				{
					switch (c2)
					{
					case '\b':
						sb.Append("\\b");
						break;
					case '\t':
						sb.Append("\\t");
						break;
					case '\n':
						sb.Append("\\n");
						break;
					case '\v':
						goto IL_C8;
					case '\f':
						sb.Append("\\f");
						break;
					case '\r':
						sb.Append("\\r");
						break;
					default:
						if (c2 != '"')
						{
							goto IL_C8;
						}
						sb.Append("\\\"");
						break;
					}
				}
				else if (c2 != '/')
				{
					if (c2 != '\\')
					{
						goto IL_C8;
					}
					sb.Append("\\\\");
				}
				else
				{
					sb.Append("\\/");
				}
				IL_D2:
				i++;
				continue;
				IL_C8:
				sb.Append(c);
				goto IL_D2;
			}
			return sb.ToString();
		}

		private static string StringFormat(string str, Type type)
		{
			if (type == typeof(string))
			{
				str = ComFunc.String2Json(str);
				str = "\"" + str + "\"";
			}
			else if (type == typeof(DateTime))
			{
				str = "\"" + str + "\"";
			}
			else if (type == typeof(bool))
			{
				str = str.ToLower();
			}
			return str;
		}

		public static bool checkNumIsLegal(string strNum)
		{
			bool bLegal = false;
			if (!string.IsNullOrEmpty(strNum))
			{
				int intIndex;
				for (intIndex = 0; intIndex < strNum.Length; intIndex++)
				{
					if (!char.IsNumber(strNum[intIndex]))
					{
						break;
					}
				}
				if (intIndex == strNum.Length)
				{
					bLegal = true;
				}
			}
			return bLegal;
		}

		public static bool IsInteger(string s)
		{
			bool result;
			if (string.IsNullOrEmpty(s))
			{
				result = false;
			}
			else
			{
				string pattern = "^\\d*$";
				result = Regex.IsMatch(s, pattern);
			}
			return result;
		}

		public static string countStatusLength(string current_time, string microsecond)
		{
			string result;
			try
			{
				ulong old_sec = (Convert.ToUInt64(current_time) - Convert.ToUInt64(microsecond)) / 1000000uL;
				int sec = Convert.ToInt32(old_sec);
				string rt = ComFunc.converToTimeLength(sec.ToString());
				if (rt == "")
				{
					result = "0秒";
				}
				else
				{
					result = ComFunc.converToTimeLength(sec.ToString());
				}
			}
			catch (Exception ex_58)
			{
				result = ">100天";
			}
			return result;
		}

		public static int get_time_interval_sec(string current_time, string microsecond)
		{
			int result;
			try
			{
				if (current_time == "" || current_time == "0" || microsecond == "" || microsecond == "0")
				{
					result = 0;
				}
				else
				{
					ulong old_sec = (Convert.ToUInt64(current_time) - Convert.ToUInt64(microsecond)) / 1000000uL;
					int sec = Convert.ToInt32(old_sec);
					result = sec;
				}
			}
			catch (Exception ex_64)
			{
				result = 0;
			}
			return result;
		}

		public static string TotalMicroSecondToTime(string ms)
		{
			string strSec = (Convert.ToInt64(ms) / 1000000L).ToString();
			DateTime d = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
			string result;
			if (strSec.Length < 10)
			{
				result = d.ToShortTimeString();
			}
			else
			{
				result = d.AddSeconds(Convert.ToDouble(strSec)).ToLongTimeString();
			}
			return result;
		}

		public static string TotalSecondToDateTime(string ms)
		{
			string strSec = Convert.ToInt64(ms).ToString();
			DateTime d = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
			string result;
			if (strSec.Length < 10)
			{
				result = d.ToShortTimeString();
			}
			else
			{
				DateTime dt = d.AddSeconds(Convert.ToDouble(strSec));
				result = dt.ToShortDateString() + " " + dt.ToLongTimeString();
			}
			return result;
		}

		public static string converToTimeLength(string len_sec)
		{
			int sec = Convert.ToInt32(len_sec);
			string time_length = "";
			TimeSpan ts = new TimeSpan(0, 0, sec);
			if (ts.Days > 0)
			{
				time_length = ts.Days + "天";
			}
			if (ts.Hours > 0)
			{
				time_length = time_length + ts.Hours + "小时";
			}
			if (ts.Minutes > 0)
			{
				time_length = time_length + ts.Minutes + "分";
			}
			if (ts.Seconds > 0)
			{
				time_length = time_length + ts.Seconds + "秒";
			}
			return time_length;
		}

		public static string make_md5_str(string source)
		{
			string result;
			using (MD5 md5Hash = MD5.Create())
			{
				string hash = ComFunc.GetMd5Hash(md5Hash, source);
				result = hash;
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

		public static string PLDESDecrypt(string sourceString, string saltKey, string todayDate)
		{
			string result;
			if (string.IsNullOrEmpty(saltKey) || string.IsNullOrEmpty(todayDate))
			{
				result = "";
			}
			else
			{
				if (ComFunc.mTodayDate != todayDate)
				{
					ComFunc.mTodayDate = todayDate;
					string key_old = ComFunc.make_md5_str(saltKey + todayDate);
					string key = string.Concat(new string[]
					{
						key_old.Substring(0, 1),
						key_old.Substring(4, 1),
						key_old.Substring(8, 1),
						key_old.Substring(12, 1),
						key_old.Substring(16, 1),
						key_old.Substring(20, 1),
						key_old.Substring(24, 1),
						key_old.Substring(28, 1)
					});
					ComFunc.mDESKey = key.ToUpper();
				}
				string desDecryptStr = ComFunc.DESDecrypt(sourceString, ComFunc.mDESKey, ComFunc.VECTOR_STR);
				desDecryptStr = desDecryptStr.Replace("|##|", "|");
				desDecryptStr = desDecryptStr.Replace("|==|", "=");
				result = desDecryptStr;
			}
			return result;
		}

		public static string DESEncrypt(string sourceString, string key, string iv)
		{
			string result;
			try
			{
				byte[] btKey = Encoding.UTF8.GetBytes(key);
				byte[] btIV = Encoding.UTF8.GetBytes(iv);
				DESCryptoServiceProvider des = new DESCryptoServiceProvider();
				using (MemoryStream ms = new MemoryStream())
				{
					byte[] inData = Encoding.UTF8.GetBytes(sourceString);
					try
					{
						using (CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(btKey, btIV), CryptoStreamMode.Write))
						{
							cs.Write(inData, 0, inData.Length);
							cs.FlushFinalBlock();
						}
						result = Convert.ToBase64String(ms.ToArray());
						return result;
					}
					catch
					{
						result = sourceString;
						return result;
					}
				}
			}
			catch
			{
			}
			result = "DES加密出错";
			return result;
		}

		public static string DESDecrypt(string encryptedString, string key, string iv)
		{
			byte[] btKey = Encoding.UTF8.GetBytes(key);
			byte[] btIV = Encoding.UTF8.GetBytes(iv);
			DESCryptoServiceProvider des = new DESCryptoServiceProvider();
			string result;
			using (MemoryStream ms = new MemoryStream())
			{
				byte[] inData = Convert.FromBase64String(encryptedString);
				try
				{
					using (CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(btKey, btIV), CryptoStreamMode.Write))
					{
						cs.Write(inData, 0, inData.Length);
						cs.FlushFinalBlock();
					}
					result = Encoding.UTF8.GetString(ms.ToArray());
				}
				catch
				{
					result = encryptedString;
				}
			}
			return result;
		}

		public static bool checkPortIsLegal(string strPort)
		{
			bool bLegal = false;
			try
			{
				if (!string.IsNullOrEmpty(strPort))
				{
					int intIndex;
					for (intIndex = 0; intIndex < strPort.Length; intIndex++)
					{
						if (!char.IsNumber(strPort[intIndex]))
						{
							break;
						}
					}
					if (intIndex == strPort.Length)
					{
						uint iPort = Convert.ToUInt32(strPort);
						if (iPort >= 0u && iPort <= 65535u)
						{
							bLegal = true;
						}
					}
				}
			}
			catch (Exception)
			{
			}
			return bLegal;
		}

		public static bool checkPortIsLegal(int nPort)
		{
			bool bLegal = false;
			if (nPort >= 0 && nPort <= 65535)
			{
				bLegal = true;
			}
			return bLegal;
		}
	}
}
