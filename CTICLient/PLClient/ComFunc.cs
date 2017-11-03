using System;
using System.Diagnostics;
using System.Reflection;
using System.Text.RegularExpressions;

namespace PLClient
{
	public static class ComFunc
	{
		public static string AssemblyVersion
		{
			get
			{
				return Assembly.GetExecutingAssembly().GetName().Version.ToString();
			}
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

		public static string CalculateTimeLength(string current_time, string old_time)
		{
			string result;
			try
			{
				ulong old_sec = (Convert.ToUInt64(current_time) - Convert.ToUInt64(old_time)) / 1000000uL;
				int sec = Convert.ToInt32(old_sec);
				string rt = ComFunc.ConverToFormatTimeLength(sec);
				if (rt == "")
				{
					result = "0秒";
				}
				else
				{
					result = ComFunc.ConverToFormatTimeLength(sec);
				}
			}
			catch (Exception ex_4C)
			{
				result = ">100天";
			}
			return result;
		}

		public static ulong CalculateTimeLengthToSec(string current_time, string old_time)
		{
			ulong result;
			try
			{
				ulong interval_sec = (Convert.ToUInt64(current_time) - Convert.ToUInt64(old_time)) / 1000000uL;
				result = interval_sec;
			}
			catch (Exception ex_1B)
			{
				result = 0uL;
			}
			return result;
		}

		public static string ConverToFormatTimeLength(int len_sec)
		{
			string time_length = "";
			string result;
			if (len_sec <= 0)
			{
				result = "0";
			}
			else
			{
				TimeSpan ts = new TimeSpan(0, 0, len_sec);
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
				result = time_length;
			}
			return result;
		}

		public static bool checkIp4IsLegal(string strIp)
		{
			bool bLegal = false;
			try
			{
				if (!string.IsNullOrEmpty(strIp))
				{
					string[] ips = strIp.Split(new char[]
					{
						'.'
					});
					if (ips.Length == 4)
					{
						int index;
						for (index = 0; index < 4; index++)
						{
							uint iTemp = Convert.ToUInt32(ips[index]);
							if (iTemp < 0u || iTemp > 255u)
							{
								break;
							}
						}
						if (index == 4)
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

		public static bool checkEmailAddrIsLegal(string strEmailAddr)
		{
			bool bLegal = false;
			try
			{
				strEmailAddr = strEmailAddr.Trim();
				if (!string.IsNullOrEmpty(strEmailAddr))
				{
					string EmailPattern = "\\w[-\\w.+]*@([A-Za-z0-9][-A-Za-z0-9]+\\.)+[A-Za-z]{2,14}";
					if (Regex.IsMatch(strEmailAddr, EmailPattern))
					{
						bLegal = true;
					}
				}
			}
			catch (Exception)
			{
			}
			return bLegal;
		}

		public static int GetProcessInfomationOfHandleCount(string processNmae)
		{
			Process pro = Process.GetProcessesByName(processNmae)[0];
			return pro.HandleCount;
		}
	}
}
