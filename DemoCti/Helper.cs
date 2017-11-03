using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Xml;

namespace DemoCti
{
	internal class Helper
	{
		private static string _md5key;

		private static int _HeartBeatTimeout = 30;

		private static string _ServerIp;

		private static int _Port;

		private static string _CallInWebURL;

		private static string _CallOutWebURL;

		private static string _HomePageWebURL;

		private static string _DefaultAgentNum;

		private static string _AgentPwd;

		private static string _DefaultExten;

		private static string _DefaultStatus;

		private static int _SelectStatus;

		private static string _LoginStatus;

		private static string _sipNum;

		private static string _sipPwd;

		private static string _sipServer;

		private static int _sipPort;

		private static string _sipCallid;

		private static bool _sipAutoAnswer;

		private static int _sipRegistTime = 60;

		private static int _SoftPhoneType;

		private static string _SoftPhone;

		private static string _ExternalPhone;

		private static string _NotBind;

		private static string _Idle;

		private static string _Busy;

		private static string _ManualCallOut;

		private static DES _md5;

		private static bool _isRemember;

		private static string _SigninID;

		private static bool _openUrlWhenThreeWayCallin;

		private static bool _openUrlWhenConsultCallin;

		private static bool _openUrlWhenEavesDropCallin;

		private static bool _openUrlWhenWhisperCallin;

		private static bool _openUrlWhenBargeinCallin;

		private static bool _openUrlWhenForceHangupCallin;

		private static int _sipSoftPhone_logoff_cmd;

		private static int _sipSoftPhone_answer_cmd;

		private static int _sipSoftPhone_min_cmd;

		private static string _sipSoftPhone_exe_path;

		private static int _sipSoftPhone_msg_value;

		private static string _sipSoftPhone_app_name;

		private static string _sipSoftPhone_app_class_name;

		private static string _sipSoftPhone_app_process_name;

		private static int _sipSoftPhone_after_login_delay_time;

		private static Dictionary<string, string> _ErrorCodeDic;

		private static int _SigninIntervalAfterSoftPhoneRegisted;

		private static string _application_title = "Polylink CTI Client";

		private static string _company_name = "Polylink";

		private static bool _open_one_instance;

		private static string _app_directory_name = "CTIClient";

		private static string _hot_Key_Setting_Idle = "Ctrl+1";

		private static string _hot_Key_Setting_Busy = "Ctrl+2";

		private static string _hot_Key_Setting_Leave = "Ctrl+3";

		private static string _hot_Key_Setting_CallOut = "Ctrl+4";

		private static string _hot_Key_Setting_Monitor = "Ctrl+5";

		private static bool _queueMonitorEnable;

		private static string _default_access_num;

		private static int _autoSetIdleFromACKTime = 0;

		private static bool _isMonitorOfflineAgent = false;

		private static int _reportStatisInterval = 60;

		private static int _default_agent_state_after_hangup;

		private static string _noAnswerCallsURL;

		private static bool _isSoftphoneAutoAnswer = false;

		private static string _isHold;

		private static string _isMute;

		private static string _isThreeWay;

		private static string _isConsult;

		private static string _isStopConsult;

		private static string _isConsultTransfer;

		private static string _isTransfer;

		private static string _isGrade;

		private static string _isCallAgent;

		private static string _isCallOut;

		private static string _isListen;

		private static string _isWhisper;

		private static string _isBargein;

		private static string _isForceHangup;

		private static string _isCheck;

		private static string _isMonitor;

		private static string _isCancelApplication;

		private static string _isdbAfterHangup;

		private static string _isApprove;

		private static string _isNoAnswerCalls;

		private static string _isControls;

		private static string _isBasic;

		private static string _isAdvanced;

		private static string _isOthers;

		private static string _screenPhoneFormat;

		public static string AgentID = string.Empty;

		public static string isHold
		{
			get
			{
				return Helper._isHold;
			}
			set
			{
				Helper._isHold = value;
			}
		}

		public static string isMute
		{
			get
			{
				return Helper._isMute;
			}
			set
			{
				Helper._isMute = value;
			}
		}

		public static string isThreeWay
		{
			get
			{
				return Helper._isThreeWay;
			}
			set
			{
				Helper._isThreeWay = value;
			}
		}

		public static string isConsult
		{
			get
			{
				return Helper._isConsult;
			}
			set
			{
				Helper._isConsult = value;
			}
		}

		public static string isStopConsult
		{
			get
			{
				return Helper._isStopConsult;
			}
			set
			{
				Helper._isStopConsult = value;
			}
		}

		public static string isConsultTransfer
		{
			get
			{
				return Helper._isConsultTransfer;
			}
			set
			{
				Helper._isConsultTransfer = value;
			}
		}

		public static string isTransfer
		{
			get
			{
				return Helper._isTransfer;
			}
			set
			{
				Helper._isTransfer = value;
			}
		}

		public static string isGrade
		{
			get
			{
				return Helper._isGrade;
			}
			set
			{
				Helper._isGrade = value;
			}
		}

		public static string isCallAgent
		{
			get
			{
				return Helper._isCallAgent;
			}
			set
			{
				Helper._isCallAgent = value;
			}
		}

		public static string isCallOut
		{
			get
			{
				return Helper._isCallOut;
			}
			set
			{
				Helper._isCallOut = value;
			}
		}

		public static string isListen
		{
			get
			{
				return Helper._isListen;
			}
			set
			{
				Helper._isListen = value;
			}
		}

		public static string isWhisper
		{
			get
			{
				return Helper._isWhisper;
			}
			set
			{
				Helper._isWhisper = value;
			}
		}

		public static string isBargein
		{
			get
			{
				return Helper._isBargein;
			}
			set
			{
				Helper._isBargein = value;
			}
		}

		public static string isForceHangup
		{
			get
			{
				return Helper._isForceHangup;
			}
			set
			{
				Helper._isForceHangup = value;
			}
		}

		public static string isCheck
		{
			get
			{
				return Helper._isCheck;
			}
			set
			{
				Helper._isCheck = value;
			}
		}

		public static string isMonitor
		{
			get
			{
				return Helper._isMonitor;
			}
			set
			{
				Helper._isMonitor = value;
			}
		}

		public static string isCancelApplication
		{
			get
			{
				return Helper._isCancelApplication;
			}
			set
			{
				Helper._isCancelApplication = value;
			}
		}

		public static string isdbAfterHangup
		{
			get
			{
				return Helper._isdbAfterHangup;
			}
			set
			{
				Helper._isdbAfterHangup = value;
			}
		}

		public static string isApprove
		{
			get
			{
				return Helper._isApprove;
			}
			set
			{
				Helper._isApprove = value;
			}
		}

		public static string isNoAnswerCalls
		{
			get
			{
				return Helper._isNoAnswerCalls;
			}
			set
			{
				Helper._isNoAnswerCalls = value;
			}
		}

		public static string isControls
		{
			get
			{
				return Helper._isControls;
			}
			set
			{
				Helper._isControls = value;
			}
		}

		public static string isBasic
		{
			get
			{
				return Helper._isBasic;
			}
			set
			{
				Helper._isBasic = value;
			}
		}

		public static string isAdvanced
		{
			get
			{
				return Helper._isAdvanced;
			}
			set
			{
				Helper._isAdvanced = value;
			}
		}

		public static string isOthers
		{
			get
			{
				return Helper._isOthers;
			}
			set
			{
				Helper._isOthers = value;
			}
		}

		public static string ApplicationTitle
		{
			get
			{
				return Helper._application_title;
			}
			set
			{
				Helper._application_title = value;
			}
		}

		public static string CompanyName
		{
			get
			{
				return Helper._company_name;
			}
			set
			{
				Helper._company_name = value;
			}
		}

		public static string Md5Key
		{
			get
			{
				return Helper._md5key;
			}
			set
			{
				Helper._md5key = value;
			}
		}

		public static DES Md5
		{
			get
			{
				return Helper._md5;
			}
			set
			{
				Helper._md5 = value;
			}
		}

		public static string ServerIP
		{
			get
			{
				return Helper._ServerIp;
			}
			set
			{
				Helper._ServerIp = value;
			}
		}

		public static int Port
		{
			get
			{
				return Helper._Port;
			}
			set
			{
				Helper._Port = value;
			}
		}

		public static string CallInWebURL
		{
			get
			{
				return Helper._CallInWebURL;
			}
			set
			{
				Helper._CallInWebURL = value;
			}
		}

		public static string CallOutWebURL
		{
			get
			{
				return Helper._CallOutWebURL;
			}
			set
			{
				Helper._CallOutWebURL = value;
			}
		}

		public static string HomePageWebURL
		{
			get
			{
				return Helper._HomePageWebURL;
			}
			set
			{
				Helper._HomePageWebURL = value;
			}
		}

		public static int HeartTimeout
		{
			get
			{
				return Helper._HeartBeatTimeout;
			}
			set
			{
				Helper._HeartBeatTimeout = value;
			}
		}

		public static string DefaultAgentNum
		{
			get
			{
				return Helper._DefaultAgentNum;
			}
			set
			{
				Helper._DefaultAgentNum = value;
			}
		}

		public static string AgentPwd
		{
			get
			{
				return Helper._AgentPwd;
			}
			set
			{
				Helper._AgentPwd = value;
			}
		}

		public static string DefaultExten
		{
			get
			{
				return Helper._DefaultExten;
			}
			set
			{
				Helper._DefaultExten = value;
			}
		}

		public static string DefaultStatus
		{
			get
			{
				return Helper._DefaultStatus;
			}
			set
			{
				Helper._DefaultStatus = value;
			}
		}

		public static int SelectStatus
		{
			get
			{
				return Helper._SelectStatus;
			}
			set
			{
				Helper._SelectStatus = value;
			}
		}

		public static string LoginStatus
		{
			get
			{
				return Helper._LoginStatus;
			}
			set
			{
				Helper._LoginStatus = value;
			}
		}

		public static string SipNum
		{
			get
			{
				return Helper._sipNum;
			}
			set
			{
				Helper._sipNum = value;
			}
		}

		public static string SipPwd
		{
			get
			{
				return Helper._sipPwd;
			}
			set
			{
				Helper._sipPwd = value;
			}
		}

		public static string SipServer
		{
			get
			{
				return Helper._sipServer;
			}
			set
			{
				Helper._sipServer = value;
			}
		}

		public static int SipPort
		{
			get
			{
				return Helper._sipPort;
			}
			set
			{
				Helper._sipPort = value;
			}
		}

		public static string SipCallid
		{
			get
			{
				return Helper._sipCallid;
			}
			set
			{
				Helper._sipCallid = value;
			}
		}

		public static bool SipAutoAnswer
		{
			get
			{
				return Helper._sipAutoAnswer;
			}
			set
			{
				Helper._sipAutoAnswer = value;
			}
		}

		public static int SipRegistTime
		{
			get
			{
				return Helper._sipRegistTime;
			}
			set
			{
				Helper._sipRegistTime = value;
			}
		}

		public static int SoftPhoneType
		{
			get
			{
				return Helper._SoftPhoneType;
			}
			set
			{
				Helper._SoftPhoneType = value;
			}
		}

		public static bool OpenUrlWhenThreeWayCallin
		{
			get
			{
				return Helper._openUrlWhenThreeWayCallin;
			}
			set
			{
				Helper._openUrlWhenThreeWayCallin = value;
			}
		}

		public static bool OpenUrlWhenConsultCallin
		{
			get
			{
				return Helper._openUrlWhenConsultCallin;
			}
			set
			{
				Helper._openUrlWhenConsultCallin = value;
			}
		}

		public static bool OpenUrlWhenEavesDropCallin
		{
			get
			{
				return Helper._openUrlWhenEavesDropCallin;
			}
			set
			{
				Helper._openUrlWhenEavesDropCallin = value;
			}
		}

		public static bool OpenUrlWhenWhisperCallin
		{
			get
			{
				return Helper._openUrlWhenWhisperCallin;
			}
			set
			{
				Helper._openUrlWhenWhisperCallin = value;
			}
		}

		public static bool OpenUrlWhenBargeinCallin
		{
			get
			{
				return Helper._openUrlWhenBargeinCallin;
			}
			set
			{
				Helper._openUrlWhenBargeinCallin = value;
			}
		}

		public static bool OpenUrlWhenForceHangupCallin
		{
			get
			{
				return Helper._openUrlWhenForceHangupCallin;
			}
			set
			{
				Helper._openUrlWhenForceHangupCallin = value;
			}
		}

		public static string SigninID
		{
			get
			{
				return Helper._SigninID;
			}
			set
			{
				Helper._SigninID = value;
			}
		}

		public static Dictionary<string, string> ErrorCodeDic
		{
			get
			{
				return Helper._ErrorCodeDic;
			}
		}

		public static int SigninIntervalAfterSoftPhoneRegisted
		{
			get
			{
				return Helper._SigninIntervalAfterSoftPhoneRegisted;
			}
		}

		public static int SipSoftPhone_Min_Cmd
		{
			get
			{
				return Helper._sipSoftPhone_min_cmd;
			}
		}

		public static int SipSoftPhone_Answer_Cmd
		{
			get
			{
				return Helper._sipSoftPhone_answer_cmd;
			}
		}

		public static int SipSoftPhone_Logoff_Cmd
		{
			get
			{
				return Helper._sipSoftPhone_logoff_cmd;
			}
		}

		public static string SipSoftPhone_Exe_Path
		{
			get
			{
				return Helper._sipSoftPhone_exe_path;
			}
		}

		public static int SipSoftPhone_Msg_Value
		{
			get
			{
				return Helper._sipSoftPhone_msg_value;
			}
		}

		public static string SipSoftPhone_App_Name
		{
			get
			{
				return Helper._sipSoftPhone_app_name;
			}
		}

		public static string SipSoftPhone_App_Class_Name
		{
			get
			{
				return Helper._sipSoftPhone_app_class_name;
			}
		}

		public static string SoftPhoneAppProcessName
		{
			get
			{
				return Helper._sipSoftPhone_app_process_name;
			}
		}

		public static int SipSoftPhone_after_login_delay_time
		{
			get
			{
				return Helper._sipSoftPhone_after_login_delay_time;
			}
		}

		public static bool Open_One_Instance
		{
			get
			{
				return Helper._open_one_instance;
			}
		}

		public static string App_Directory_Name
		{
			get
			{
				return Helper._app_directory_name;
			}
		}

		public static string Hot_Key_Setting_Idle
		{
			get
			{
				return Helper._hot_Key_Setting_Idle;
			}
		}

		public static string Hot_Key_Setting_Busy
		{
			get
			{
				return Helper._hot_Key_Setting_Busy;
			}
		}

		public static string Hot_Key_Setting_Leave
		{
			get
			{
				return Helper._hot_Key_Setting_Leave;
			}
		}

		public static string Hot_Key_Setting_CallOut
		{
			get
			{
				return Helper._hot_Key_Setting_CallOut;
			}
		}

		public static string Hot_Key_Setting_Monitor
		{
			get
			{
				return Helper._hot_Key_Setting_Monitor;
			}
		}

		public static bool Queue_Monitor_Enable
		{
			get
			{
				return Helper._queueMonitorEnable;
			}
			set
			{
				Helper._queueMonitorEnable = value;
			}
		}

		public static string Default_Access_Number
		{
			get
			{
				return Helper._default_access_num;
			}
		}

		public static int AutoSetIdleFromACKTime
		{
			get
			{
				return Helper._autoSetIdleFromACKTime;
			}
			set
			{
				Helper._autoSetIdleFromACKTime = value;
			}
		}

		public static bool IsMonitorOfflineAgent
		{
			get
			{
				return Helper._isMonitorOfflineAgent;
			}
			set
			{
				Helper._isMonitorOfflineAgent = value;
			}
		}

		public static string NoAnswerCallsURL
		{
			get
			{
				return Helper._noAnswerCallsURL;
			}
			set
			{
				Helper._noAnswerCallsURL = value;
			}
		}

		public static bool IsSoftphoneAutoAnswer
		{
			get
			{
				return Helper._isSoftphoneAutoAnswer;
			}
			set
			{
				Helper._isSoftphoneAutoAnswer = value;
			}
		}

		public static string getScreenPhoneFormat
		{
			get
			{
				return Helper._screenPhoneFormat;
			}
			set
			{
				Helper._screenPhoneFormat = value;
			}
		}

		public static string ChkSoftPhone
		{
			get
			{
				return Helper._SoftPhone;
			}
			set
			{
				Helper._SoftPhone = value;
			}
		}

		public static string ChkExternalPhone
		{
			get
			{
				return Helper._ExternalPhone;
			}
			set
			{
				Helper._ExternalPhone = value;
			}
		}

		public static string ChkNotBind
		{
			get
			{
				return Helper._NotBind;
			}
			set
			{
				Helper._NotBind = value;
			}
		}

		public static string ChkIdle
		{
			get
			{
				return Helper._Idle;
			}
			set
			{
				Helper._Idle = value;
			}
		}

		public static string ChkBusy
		{
			get
			{
				return Helper._Busy;
			}
			set
			{
				Helper._Busy = value;
			}
		}

		public static string ChkManualCallOut
		{
			get
			{
				return Helper._ManualCallOut;
			}
			set
			{
				Helper._ManualCallOut = value;
			}
		}

		public static bool isRemembered
		{
			get
			{
				return Helper._isRemember;
			}
		}

		public static int ReportStatisInterval
		{
			get
			{
				return Helper._reportStatisInterval;
			}
		}

		public static int Default_Agent_State_After_Hangup
		{
			get
			{
				return Helper._default_agent_state_after_hangup;
			}
		}

		public Helper(string serverip, int port, string sipNum, string sipPwd, string sipServer, int sipPort, string sipCallid, bool sipAutoAnswer, int sipRegistTime, string strSoftPhone, string strExternalPhone, string strNotBind, string strIdle, string strBusy, string strManualCallOut)
		{
			Helper._ServerIp = serverip;
			Helper._Port = port;
			Helper._sipNum = sipNum;
			Helper._sipPwd = sipPwd;
			Helper._sipServer = sipServer;
			Helper._sipPort = sipPort;
			Helper._sipCallid = sipCallid;
			Helper._sipAutoAnswer = sipAutoAnswer;
			if (sipRegistTime != 0)
			{
				Helper._sipRegistTime = Convert.ToInt32(sipRegistTime);
			}
			Helper._SoftPhone = strSoftPhone;
			Helper._ExternalPhone = strExternalPhone;
			Helper._NotBind = strNotBind;
			Helper._Idle = strIdle;
			Helper._Busy = strBusy;
			Helper._ManualCallOut = strManualCallOut;
		}

		public static void initMd5()
		{
			Helper._md5 = new DES();
			Helper._md5key = "jackzhao";
		}

		public static bool reload_sys_AgentStatueAndPhoneType()
		{
			bool result;
			try
			{
				Helper.initMd5();
				ExeConfigurationFileMap map = new ExeConfigurationFileMap();
				Configuration config;
				if (string.IsNullOrEmpty(Helper.AgentID))
				{
					map.ExeConfigFilename = Environment.GetEnvironmentVariable("APPDATA") + "\\" + Helper.App_Directory_Name + "\\user.Config";
					config = ConfigurationManager.OpenMappedExeConfiguration(map, ConfigurationUserLevel.None);
				}
				else
				{
					map.ExeConfigFilename = string.Concat(new string[]
					{
						Environment.GetEnvironmentVariable("APPDATA"),
						"\\",
						Helper.App_Directory_Name,
						"\\",
						Helper.AgentID,
						"_user.Config"
					});
					config = ConfigurationManager.OpenMappedExeConfiguration(map, ConfigurationUserLevel.None);
					if (!config.HasFile)
					{
						map.ExeConfigFilename = Environment.GetEnvironmentVariable("APPDATA") + "\\" + Helper.App_Directory_Name + "\\user.Config";
						config = ConfigurationManager.OpenMappedExeConfiguration(map, ConfigurationUserLevel.None);
					}
				}
				Helper._SoftPhone = config.AppSettings.Settings["ChkSoftPhone"].Value;
				Helper._ExternalPhone = config.AppSettings.Settings["ChkExternalPhone"].Value;
				Helper._NotBind = config.AppSettings.Settings["ChkNotBind"].Value;
				Helper._Idle = config.AppSettings.Settings["ChkIdle"].Value;
				Helper._Busy = config.AppSettings.Settings["ChkBusy"].Value;
				Helper._ManualCallOut = config.AppSettings.Settings["ChkManualCallOut"].Value;
				result = true;
			}
			catch (Exception ex)
			{
				string aaa = ex.Message;
				result = false;
			}
			return result;
		}

		public static bool load_sys_AgentStatueAndPhoneType()
		{
			bool result;
			try
			{
				Helper.initMd5();
				ExeConfigurationFileMap map = new ExeConfigurationFileMap();
				Configuration config;
				if (string.IsNullOrEmpty(Program.LastSigninAgentID))
				{
					map.ExeConfigFilename = Environment.GetEnvironmentVariable("APPDATA") + "\\" + Helper.App_Directory_Name + "\\user.Config";
					config = ConfigurationManager.OpenMappedExeConfiguration(map, ConfigurationUserLevel.None);
				}
				else
				{
					map.ExeConfigFilename = string.Concat(new string[]
					{
						Environment.GetEnvironmentVariable("APPDATA"),
						"\\",
						Helper.App_Directory_Name,
						"\\",
						Program.LastSigninAgentID,
						"_user.Config"
					});
					config = ConfigurationManager.OpenMappedExeConfiguration(map, ConfigurationUserLevel.None);
					if (!config.HasFile)
					{
						map.ExeConfigFilename = Environment.GetEnvironmentVariable("APPDATA") + "\\" + Helper.App_Directory_Name + "\\user.Config";
						config = ConfigurationManager.OpenMappedExeConfiguration(map, ConfigurationUserLevel.None);
					}
				}
				Helper._SoftPhone = config.AppSettings.Settings["ChkSoftPhone"].Value;
				Helper._ExternalPhone = config.AppSettings.Settings["ChkExternalPhone"].Value;
				Helper._NotBind = config.AppSettings.Settings["ChkNotBind"].Value;
				Helper._Idle = config.AppSettings.Settings["ChkIdle"].Value;
				Helper._Busy = config.AppSettings.Settings["ChkBusy"].Value;
				Helper._ManualCallOut = config.AppSettings.Settings["ChkManualCallOut"].Value;
				result = true;
			}
			catch (Exception ex)
			{
				string aaa = ex.Message;
				result = false;
			}
			return result;
		}

		public static bool load_sys_AndPhoneTypeNum()
		{
			bool result;
			try
			{
				Helper.initMd5();
				ExeConfigurationFileMap map = new ExeConfigurationFileMap();
				Configuration config;
				if (string.IsNullOrEmpty(Helper.AgentID))
				{
					map.ExeConfigFilename = Environment.GetEnvironmentVariable("APPDATA") + "\\" + Helper.App_Directory_Name + "\\user.Config";
					config = ConfigurationManager.OpenMappedExeConfiguration(map, ConfigurationUserLevel.None);
				}
				else
				{
					map.ExeConfigFilename = string.Concat(new string[]
					{
						Environment.GetEnvironmentVariable("APPDATA"),
						"\\",
						Helper.App_Directory_Name,
						"\\",
						Helper.AgentID,
						"_user.Config"
					});
					config = ConfigurationManager.OpenMappedExeConfiguration(map, ConfigurationUserLevel.None);
					if (!config.HasFile)
					{
						map.ExeConfigFilename = Environment.GetEnvironmentVariable("APPDATA") + "\\" + Helper.App_Directory_Name + "\\user.Config";
						config = ConfigurationManager.OpenMappedExeConfiguration(map, ConfigurationUserLevel.None);
					}
				}
				Helper._sipNum = config.AppSettings.Settings["sipNum"].Value;
				result = true;
			}
			catch (Exception ex)
			{
				string aaa = ex.Message;
				result = false;
			}
			return result;
		}

		public static bool load_sys_LastAgentconfig()
		{
			bool result;
			try
			{
				Helper.initMd5();
				ExeConfigurationFileMap map = new ExeConfigurationFileMap();
				Configuration config;
				if (string.IsNullOrEmpty(Program.LastSigninAgentID))
				{
					map.ExeConfigFilename = Environment.GetEnvironmentVariable("APPDATA") + "\\" + Helper.App_Directory_Name + "\\user.Config";
					config = ConfigurationManager.OpenMappedExeConfiguration(map, ConfigurationUserLevel.None);
				}
				else
				{
					map.ExeConfigFilename = string.Concat(new string[]
					{
						Environment.GetEnvironmentVariable("APPDATA"),
						"\\",
						Helper.App_Directory_Name,
						"\\",
						Program.LastSigninAgentID,
						"_user.Config"
					});
					config = ConfigurationManager.OpenMappedExeConfiguration(map, ConfigurationUserLevel.None);
					if (!config.HasFile)
					{
						map.ExeConfigFilename = Environment.GetEnvironmentVariable("APPDATA") + "\\" + Helper.App_Directory_Name + "\\user.Config";
						config = ConfigurationManager.OpenMappedExeConfiguration(map, ConfigurationUserLevel.None);
					}
				}
				Helper._ServerIp = config.AppSettings.Settings["serverIP"].Value;
				Helper._Port = Convert.ToInt32(config.AppSettings.Settings["port"].Value);
				Helper._DefaultExten = config.AppSettings.Settings["defaultExten"].Value;
				Helper._CallInWebURL = config.AppSettings.Settings["callinWebURL"].Value;
				Helper._CallOutWebURL = config.AppSettings.Settings["calloutWebURL"].Value;
				Helper._HomePageWebURL = config.AppSettings.Settings["homePageWebURL"].Value;
				Helper._DefaultAgentNum = config.AppSettings.Settings["defaultAgentNum"].Value;
				if (Helper._md5key == "" || config.AppSettings.Settings["agentPwd"].Value == "")
				{
					Helper._AgentPwd = "";
				}
				else
				{
					Helper._AgentPwd = Helper._md5.MD5Decrypt(config.AppSettings.Settings["agentPwd"].Value, Helper._md5key);
				}
				Helper._DefaultExten = config.AppSettings.Settings["defaultExten"].Value;
				Helper._DefaultStatus = config.AppSettings.Settings["defaultStatus"].Value;
				Helper._SoftPhoneType = Convert.ToInt32(config.AppSettings.Settings["SoftPhoneType"].Value);
				Helper._sipNum = config.AppSettings.Settings["sipNum"].Value;
				if (Helper._md5key == "" || config.AppSettings.Settings["sipPwd"].Value == "")
				{
					Helper._sipPwd = "";
				}
				else
				{
					Helper._sipPwd = Helper._md5.MD5Decrypt(config.AppSettings.Settings["sipPwd"].Value, Helper._md5key);
				}
				Helper._sipServer = config.AppSettings.Settings["sipServer"].Value;
				Helper._sipPort = Convert.ToInt32(config.AppSettings.Settings["sipPort"].Value);
				Helper._sipCallid = config.AppSettings.Settings["sipCallid"].Value;
				Helper._SigninIntervalAfterSoftPhoneRegisted = Convert.ToInt32(config.AppSettings.Settings["SigninIntervalAfterSoftPhoneRegisted"].Value);
				Helper._application_title = config.AppSettings.Settings["applicationTitle"].Value;
				Helper._company_name = config.AppSettings.Settings["companyName"].Value;
				if (config.AppSettings.Settings["sipAutoAnswer"].Value == "1")
				{
					Helper._sipAutoAnswer = true;
				}
				else
				{
					Helper._sipAutoAnswer = false;
				}
				Helper._sipRegistTime = Convert.ToInt32(config.AppSettings.Settings["sipRegistTime"].Value);
				if (Helper._CallInWebURL.IndexOf('.', 0) == 0)
				{
					Helper._CallInWebURL = Environment.CurrentDirectory.ToString() + Helper._CallInWebURL.Substring(2);
				}
				Helper._sipSoftPhone_answer_cmd = Convert.ToInt32(config.AppSettings.Settings["SipSoftPhone_answer_cmd"].Value);
				Helper._sipSoftPhone_logoff_cmd = Convert.ToInt32(config.AppSettings.Settings["SipSoftPhone_logoff_cmd"].Value);
				Helper._sipSoftPhone_min_cmd = Convert.ToInt32(config.AppSettings.Settings["SipSoftPhone_min_cmd"].Value);
				Helper._sipSoftPhone_exe_path = config.AppSettings.Settings["SipSoftPhone_exe_path"].Value;
				Helper._sipSoftPhone_msg_value = Convert.ToInt32(config.AppSettings.Settings["SipSoftPhone_msg_value"].Value);
				Helper._sipSoftPhone_app_name = config.AppSettings.Settings["SipSoftPhone_app_name"].Value;
				Helper._sipSoftPhone_app_class_name = config.AppSettings.Settings["SipSoftPhone_app_class_name"].Value;
				Helper._sipSoftPhone_app_process_name = config.AppSettings.Settings["SipSoftPhone_app_process_name"].Value;
				Helper._sipSoftPhone_after_login_delay_time = Convert.ToInt32(config.AppSettings.Settings["SipSoftPhone_login_after_delay_time"].Value);
				if (config.AppSettings.Settings["OpenOneInstance"].Value == "1")
				{
					Helper._open_one_instance = true;
				}
				else
				{
					Helper._open_one_instance = false;
				}
				if (config.AppSettings.Settings["QueueMonitorTipEnable"].Value == "1")
				{
					Helper._queueMonitorEnable = true;
				}
				else
				{
					Helper._queueMonitorEnable = false;
				}
				Helper._default_access_num = config.AppSettings.Settings["DefaultAccessNumber"].Value;
				Helper._autoSetIdleFromACKTime = Convert.ToInt32(config.AppSettings.Settings["AutoSetIdleFromACKTime"].Value);
				if (config.AppSettings.Settings["IsMonitorOfflineAgent"].Value == "1")
				{
					Helper._isMonitorOfflineAgent = true;
				}
				else
				{
					Helper._isMonitorOfflineAgent = false;
				}
				if (config.AppSettings.Settings["IsRemembered"].Value == "1")
				{
					Helper._isRemember = true;
				}
				else
				{
					Helper._isRemember = false;
				}
				Helper._reportStatisInterval = Convert.ToInt32(config.AppSettings.Settings["RefreshReportStatisInterval"].Value);
				Helper._default_agent_state_after_hangup = Convert.ToInt32(config.AppSettings.Settings["DefaultAgentStateAfterHangup"].Value);
				Helper._noAnswerCallsURL = config.AppSettings.Settings["NoAnswerCallsURL"].Value;
				result = true;
			}
			catch (Exception ex)
			{
				string aaa = ex.Message;
				result = false;
			}
			return result;
		}

		public static bool load_sysAgentconfig()
		{
			bool result;
			try
			{
				Helper.initMd5();
				ExeConfigurationFileMap map = new ExeConfigurationFileMap();
				Configuration config;
				if (string.IsNullOrEmpty(Helper.AgentID))
				{
					map.ExeConfigFilename = Environment.GetEnvironmentVariable("APPDATA") + "\\" + Helper.App_Directory_Name + "\\user.Config";
					config = ConfigurationManager.OpenMappedExeConfiguration(map, ConfigurationUserLevel.None);
				}
				else
				{
					map.ExeConfigFilename = string.Concat(new string[]
					{
						Environment.GetEnvironmentVariable("APPDATA"),
						"\\",
						Helper.App_Directory_Name,
						"\\",
						Helper.AgentID,
						"_user.Config"
					});
					config = ConfigurationManager.OpenMappedExeConfiguration(map, ConfigurationUserLevel.None);
					if (!config.HasFile)
					{
						map.ExeConfigFilename = Environment.GetEnvironmentVariable("APPDATA") + "\\" + Helper.App_Directory_Name + "\\user.Config";
						config = ConfigurationManager.OpenMappedExeConfiguration(map, ConfigurationUserLevel.None);
					}
				}
				Helper._ServerIp = config.AppSettings.Settings["serverIP"].Value;
				Helper._Port = Convert.ToInt32(config.AppSettings.Settings["port"].Value);
				Helper._sipNum = config.AppSettings.Settings["sipNum"].Value;
				if (Helper._md5key == "" || config.AppSettings.Settings["sipPwd"].Value == "")
				{
					Helper._sipPwd = "";
				}
				else
				{
					Helper._sipPwd = Helper._md5.MD5Decrypt(config.AppSettings.Settings["sipPwd"].Value, Helper._md5key);
				}
				Helper._sipServer = config.AppSettings.Settings["sipServer"].Value;
				Helper._sipPort = Convert.ToInt32(config.AppSettings.Settings["sipPort"].Value);
				if (config.AppSettings.Settings["sipAutoAnswer"].Value == "1")
				{
					Helper._sipAutoAnswer = true;
				}
				else
				{
					Helper._sipAutoAnswer = false;
				}
				Helper._sipRegistTime = Convert.ToInt32(config.AppSettings.Settings["sipRegistTime"].Value);
				Helper.ChkSoftPhone = config.AppSettings.Settings["ChkSoftPhone"].Value;
				Helper.ChkExternalPhone = config.AppSettings.Settings["ChkExternalPhone"].Value;
				Helper.ChkNotBind = config.AppSettings.Settings["ChkNotBind"].Value;
				Helper.ChkIdle = config.AppSettings.Settings["ChkIdle"].Value;
				Helper.ChkBusy = config.AppSettings.Settings["ChkBusy"].Value;
				Helper.ChkManualCallOut = config.AppSettings.Settings["ChkManualCallOut"].Value;
				result = true;
			}
			catch (Exception ex)
			{
				string aaa = ex.Message;
				result = false;
			}
			return result;
		}

		public static bool load_sys_config()
		{
			bool result;
			try
			{
				Helper.initMd5();
				ExeConfigurationFileMap map = new ExeConfigurationFileMap();
				map.ExeConfigFilename = string.Concat(new string[]
				{
					Environment.GetEnvironmentVariable("APPDATA"),
					"\\",
					Helper.App_Directory_Name,
					"\\",
					Helper._SigninID,
					"_user.Config"
				});
				Configuration config = ConfigurationManager.OpenMappedExeConfiguration(map, ConfigurationUserLevel.None);
				if (!config.HasFile)
				{
					map.ExeConfigFilename = Environment.GetEnvironmentVariable("APPDATA") + "\\" + Helper.App_Directory_Name + "\\user.Config";
					config = ConfigurationManager.OpenMappedExeConfiguration(map, ConfigurationUserLevel.None);
				}
				Helper._ServerIp = config.AppSettings.Settings["serverIP"].Value;
				Helper._DefaultExten = config.AppSettings.Settings["defaultExten"].Value;
				Helper._CallInWebURL = config.AppSettings.Settings["callinWebURL"].Value;
				Helper._CallOutWebURL = config.AppSettings.Settings["calloutWebURL"].Value;
				Helper._HomePageWebURL = config.AppSettings.Settings["homePageWebURL"].Value;
				Helper._DefaultAgentNum = config.AppSettings.Settings["defaultAgentNum"].Value;
				if (Helper._md5key == "" || config.AppSettings.Settings["agentPwd"].Value == "")
				{
					Helper._AgentPwd = "";
				}
				else
				{
					Helper._AgentPwd = Helper._md5.MD5Decrypt(config.AppSettings.Settings["agentPwd"].Value, Helper._md5key);
				}
				Helper._DefaultExten = config.AppSettings.Settings["defaultExten"].Value;
				Helper._DefaultStatus = config.AppSettings.Settings["defaultStatus"].Value;
				Helper._SoftPhoneType = Convert.ToInt32(config.AppSettings.Settings["SoftPhoneType"].Value);
				Helper._sipCallid = config.AppSettings.Settings["sipCallid"].Value;
				Helper._SigninIntervalAfterSoftPhoneRegisted = Convert.ToInt32(config.AppSettings.Settings["SigninIntervalAfterSoftPhoneRegisted"].Value);
				Helper._application_title = config.AppSettings.Settings["applicationTitle"].Value;
				Helper._company_name = config.AppSettings.Settings["companyName"].Value;
				if (config.AppSettings.Settings["OpenUrlWhenThreeWay"].Value == "1")
				{
					Helper._openUrlWhenThreeWayCallin = true;
				}
				else
				{
					Helper._openUrlWhenThreeWayCallin = false;
				}
				if (config.AppSettings.Settings["OpenUrlWhenConsult"].Value == "1")
				{
					Helper._openUrlWhenConsultCallin = true;
				}
				else
				{
					Helper._openUrlWhenConsultCallin = false;
				}
				if (config.AppSettings.Settings["OpenUrlWhenEavesDrop"].Value == "1")
				{
					Helper._openUrlWhenEavesDropCallin = true;
				}
				else
				{
					Helper._openUrlWhenEavesDropCallin = false;
				}
				if (config.AppSettings.Settings["OpenUrlWhenWhisper"].Value == "1")
				{
					Helper._openUrlWhenWhisperCallin = true;
				}
				else
				{
					Helper._openUrlWhenWhisperCallin = false;
				}
				if (config.AppSettings.Settings["OpenUrlWhenBargin"].Value == "1")
				{
					Helper._openUrlWhenBargeinCallin = true;
				}
				else
				{
					Helper._openUrlWhenBargeinCallin = false;
				}
				if (config.AppSettings.Settings["OpenUrlWhenForceHangup"].Value == "1")
				{
					Helper._openUrlWhenForceHangupCallin = true;
				}
				else
				{
					Helper._openUrlWhenForceHangupCallin = false;
				}
				if (Helper._CallInWebURL.IndexOf('.', 0) == 0)
				{
					Helper._CallInWebURL = Environment.CurrentDirectory.ToString() + Helper._CallInWebURL.Substring(2);
				}
				Helper._sipSoftPhone_answer_cmd = Convert.ToInt32(config.AppSettings.Settings["SipSoftPhone_answer_cmd"].Value);
				Helper._sipSoftPhone_logoff_cmd = Convert.ToInt32(config.AppSettings.Settings["SipSoftPhone_logoff_cmd"].Value);
				Helper._sipSoftPhone_min_cmd = Convert.ToInt32(config.AppSettings.Settings["SipSoftPhone_min_cmd"].Value);
				Helper._sipSoftPhone_exe_path = config.AppSettings.Settings["SipSoftPhone_exe_path"].Value;
				Helper._sipSoftPhone_msg_value = Convert.ToInt32(config.AppSettings.Settings["SipSoftPhone_msg_value"].Value);
				Helper._sipSoftPhone_app_name = config.AppSettings.Settings["SipSoftPhone_app_name"].Value;
				Helper._sipSoftPhone_app_class_name = config.AppSettings.Settings["SipSoftPhone_app_class_name"].Value;
				Helper._sipSoftPhone_app_process_name = config.AppSettings.Settings["SipSoftPhone_app_process_name"].Value;
				Helper._sipSoftPhone_after_login_delay_time = Convert.ToInt32(config.AppSettings.Settings["SipSoftPhone_login_after_delay_time"].Value);
				Helper._hot_Key_Setting_Idle = config.AppSettings.Settings["HotKeySettingIdle"].Value;
				Helper._hot_Key_Setting_Busy = config.AppSettings.Settings["HotKeySettingBusy"].Value;
				Helper._hot_Key_Setting_Leave = config.AppSettings.Settings["HotKeySettingLeave"].Value;
				Helper._hot_Key_Setting_CallOut = config.AppSettings.Settings["HotKeySettingCallOut"].Value;
				Helper._hot_Key_Setting_Monitor = config.AppSettings.Settings["HotKeySettingMonitor"].Value;
				if (config.AppSettings.Settings["OpenOneInstance"].Value == "1")
				{
					Helper._open_one_instance = true;
				}
				else
				{
					Helper._open_one_instance = false;
				}
				if (config.AppSettings.Settings["QueueMonitorTipEnable"].Value == "1")
				{
					Helper._queueMonitorEnable = true;
				}
				else
				{
					Helper._queueMonitorEnable = false;
				}
				Helper._default_access_num = config.AppSettings.Settings["DefaultAccessNumber"].Value;
				Helper._autoSetIdleFromACKTime = Convert.ToInt32(config.AppSettings.Settings["AutoSetIdleFromACKTime"].Value);
				if (config.AppSettings.Settings["IsMonitorOfflineAgent"].Value == "1")
				{
					Helper._isMonitorOfflineAgent = true;
				}
				else
				{
					Helper._isMonitorOfflineAgent = false;
				}
				if (config.AppSettings.Settings["IsRemembered"].Value == "1")
				{
					Helper._isRemember = true;
				}
				else
				{
					Helper._isRemember = false;
				}
				if (config.AppSettings.Settings["IsSoftphoneAutoAnswer"].Value == "1")
				{
					Helper._isSoftphoneAutoAnswer = true;
				}
				else
				{
					Helper._isSoftphoneAutoAnswer = false;
				}
				Helper._reportStatisInterval = Convert.ToInt32(config.AppSettings.Settings["RefreshReportStatisInterval"].Value);
				Helper._default_agent_state_after_hangup = Convert.ToInt32(config.AppSettings.Settings["DefaultAgentStateAfterHangup"].Value);
				Helper._noAnswerCallsURL = config.AppSettings.Settings["NoAnswerCallsURL"].Value;
				Helper._isHold = config.AppSettings.Settings["IsHold"].Value;
				Helper._isMute = config.AppSettings.Settings["IsMute"].Value;
				Helper._isThreeWay = config.AppSettings.Settings["IsThreeWay"].Value;
				Helper._isConsult = config.AppSettings.Settings["IsConsult"].Value;
				Helper._isStopConsult = config.AppSettings.Settings["IsStopConsult"].Value;
				Helper._isConsultTransfer = config.AppSettings.Settings["IsConsultTransfer"].Value;
				Helper._isTransfer = config.AppSettings.Settings["IsTransfer"].Value;
				Helper._isGrade = config.AppSettings.Settings["IsGrade"].Value;
				Helper._isCallAgent = config.AppSettings.Settings["IsCallAgent"].Value;
				Helper._isCallOut = config.AppSettings.Settings["IsCallOut"].Value;
				Helper._isListen = config.AppSettings.Settings["IsListen"].Value;
				Helper._isWhisper = config.AppSettings.Settings["IsWhisper"].Value;
				Helper._isBargein = config.AppSettings.Settings["IsBargein"].Value;
				Helper._isForceHangup = config.AppSettings.Settings["IsForceHangup"].Value;
				Helper._isCheck = config.AppSettings.Settings["IsCheck"].Value;
				Helper._isMonitor = config.AppSettings.Settings["IsMonitor"].Value;
				Helper._isCancelApplication = config.AppSettings.Settings["IsCancelApplication"].Value;
				Helper._isdbAfterHangup = config.AppSettings.Settings["IsdbAfterHangup"].Value;
				Helper._isApprove = config.AppSettings.Settings["IsApprove"].Value;
				Helper._isNoAnswerCalls = config.AppSettings.Settings["IsNoAnswerCalls"].Value;
				Helper._screenPhoneFormat = config.AppSettings.Settings["ScreenPhoneFormat"].Value;
				result = true;
			}
			catch (Exception ex_A83)
			{
				result = false;
			}
			return result;
		}

		public static bool Controlsload_sys_config()
		{
			bool result;
			try
			{
				Helper.initMd5();
				ExeConfigurationFileMap map = new ExeConfigurationFileMap();
				map.ExeConfigFilename = string.Concat(new string[]
				{
					Environment.GetEnvironmentVariable("APPDATA"),
					"\\",
					Helper.App_Directory_Name,
					"\\",
					Helper._SigninID,
					"_user.Config"
				});
				Configuration config = ConfigurationManager.OpenMappedExeConfiguration(map, ConfigurationUserLevel.None);
				if (!config.HasFile)
				{
					map.ExeConfigFilename = Environment.GetEnvironmentVariable("APPDATA") + "\\" + Helper.App_Directory_Name + "\\user.Config";
					config = ConfigurationManager.OpenMappedExeConfiguration(map, ConfigurationUserLevel.None);
				}
				Helper._isHold = config.AppSettings.Settings["IsHold"].Value;
				Helper._isMute = config.AppSettings.Settings["IsMute"].Value;
				Helper._isThreeWay = config.AppSettings.Settings["IsThreeWay"].Value;
				Helper._isConsult = config.AppSettings.Settings["IsConsult"].Value;
				Helper._isStopConsult = config.AppSettings.Settings["IsStopConsult"].Value;
				Helper._isConsultTransfer = config.AppSettings.Settings["IsConsultTransfer"].Value;
				Helper._isTransfer = config.AppSettings.Settings["IsTransfer"].Value;
				Helper._isGrade = config.AppSettings.Settings["IsGrade"].Value;
				Helper._isCallAgent = config.AppSettings.Settings["IsCallAgent"].Value;
				Helper._isCallOut = config.AppSettings.Settings["IsCallOut"].Value;
				Helper._isListen = config.AppSettings.Settings["IsListen"].Value;
				Helper._isWhisper = config.AppSettings.Settings["IsWhisper"].Value;
				Helper._isBargein = config.AppSettings.Settings["IsBargein"].Value;
				Helper._isForceHangup = config.AppSettings.Settings["IsForceHangup"].Value;
				Helper._isCheck = config.AppSettings.Settings["IsCheck"].Value;
				Helper._isMonitor = config.AppSettings.Settings["IsMonitor"].Value;
				Helper._isCancelApplication = config.AppSettings.Settings["IsCancelApplication"].Value;
				Helper._isdbAfterHangup = config.AppSettings.Settings["IsdbAfterHangup"].Value;
				Helper._isApprove = config.AppSettings.Settings["IsApprove"].Value;
				Helper._isNoAnswerCalls = config.AppSettings.Settings["IsNoAnswerCalls"].Value;
				Helper._isControls = config.AppSettings.Settings["IsControlsSet"].Value;
				Helper._isBasic = config.AppSettings.Settings["IsBasic"].Value;
				Helper._isAdvanced = config.AppSettings.Settings["IsAdvanced"].Value;
				Helper._isOthers = config.AppSettings.Settings["IsOthers"].Value;
				result = true;
			}
			catch (Exception ex_38E)
			{
				result = false;
			}
			return result;
		}

		public static bool load_error_code()
		{
			Helper._ErrorCodeDic = new Dictionary<string, string>();
			Helper._ErrorCodeDic.Clear();
			Configuration config = ConfigurationManager.OpenMappedExeConfiguration(new ExeConfigurationFileMap
			{
				ExeConfigFilename = Environment.GetEnvironmentVariable("APPDATA") + "\\" + Helper.App_Directory_Name + "\\PLClient.exe.Config"
			}, ConfigurationUserLevel.None);
			string[] allKeys = config.AppSettings.Settings.AllKeys;
			for (int i = 0; i < allKeys.Length; i++)
			{
				string str = allKeys[i];
				Helper._ErrorCodeDic.Add(str, config.AppSettings.Settings[str].Value);
			}
			return true;
		}

		public static bool write_config(string strKey, string strValue)
		{
			ExeConfigurationFileMap map = new ExeConfigurationFileMap();
			map.ExeConfigFilename = string.Concat(new string[]
			{
				Environment.GetEnvironmentVariable("APPDATA"),
				"\\",
				Helper.App_Directory_Name,
				"\\",
				Helper._SigninID,
				"_user.Config"
			});
			Configuration config = ConfigurationManager.OpenMappedExeConfiguration(map, ConfigurationUserLevel.None);
			if (!config.HasFile)
			{
				string path = Environment.GetEnvironmentVariable("APPDATA") + "\\" + Helper.App_Directory_Name + "\\user.Config";
				string path2 = string.Concat(new string[]
				{
					Environment.GetEnvironmentVariable("APPDATA"),
					"\\",
					Helper.App_Directory_Name,
					"\\",
					Helper._SigninID,
					"_user.Config"
				});
				string UserConfigPath = path;
				string LastUserConfigPath = path2;
				FileInfo fi = new FileInfo(UserConfigPath);
				FileInfo fi2 = new FileInfo(LastUserConfigPath);
				fi.CopyTo(LastUserConfigPath);
				fi.CopyTo(LastUserConfigPath, true);
				config = ConfigurationManager.OpenMappedExeConfiguration(map, ConfigurationUserLevel.None);
			}
			if (strKey == "agentPwd" || strKey == "sipPwd")
			{
				if (Helper._md5key == "")
				{
					Helper._md5key = "jackzhao";
				}
				config.AppSettings.Settings[strKey].Value = Helper._md5.MD5Encrypt(strValue, Helper._md5key);
			}
			else
			{
				config.AppSettings.Settings[strKey].Value = strValue;
			}
			config.Save(ConfigurationSaveMode.Modified);
			ConfigurationManager.RefreshSection("appSettings");
			return true;
		}

		public static bool write_conf_to_softphone(string filePath, string autoAnswer, string registTime)
		{
			bool result;
			try
			{
				XmlDocument xmlDoc = new XmlDocument();
				xmlDoc.Load(filePath);
				XmlNode xns = xmlDoc.SelectSingleNode("TERMINAL");
				XmlNodeList xnl = xns.ChildNodes;
				foreach (XmlNode xn in xnl)
				{
					if (xn.NodeType == XmlNodeType.Element)
					{
						XmlElement xe = (XmlElement)xn;
						if (xe.Name == "GENERALCONFIG")
						{
							XmlNodeList xnls = xe.ChildNodes;
							foreach (XmlNode xnls2 in xnls)
							{
								string name = xnls2.Name;
								if (name != null)
								{
									if (!(name == "LoginWindowState"))
									{
										if (!(name == "AutoAnswer"))
										{
											if (name == "RegistTime")
											{
												xnls2.InnerText = registTime;
											}
										}
										else
										{
											xnls2.InnerText = autoAnswer;
										}
									}
									else
									{
										xnls2.InnerText = "1";
									}
								}
							}
							break;
						}
					}
				}
				xmlDoc.Save(filePath);
			}
			catch (Exception e_16A)
			{
				result = false;
				return result;
			}
			result = true;
			return result;
		}

		public static bool write_ControlsConfig_to_file()
		{
			if (Helper._md5key == "")
			{
				Helper._md5key = "jackzhao";
			}
			ExeConfigurationFileMap map = new ExeConfigurationFileMap();
			map.ExeConfigFilename = string.Concat(new string[]
			{
				Environment.GetEnvironmentVariable("APPDATA"),
				"\\",
				Helper.App_Directory_Name,
				"\\",
				Helper._SigninID,
				"_user.Config"
			});
			Configuration config = ConfigurationManager.OpenMappedExeConfiguration(map, ConfigurationUserLevel.None);
			if (!config.HasFile)
			{
				string path = Environment.GetEnvironmentVariable("APPDATA") + "\\" + Helper.App_Directory_Name + "\\user.Config";
				string path2 = string.Concat(new string[]
				{
					Environment.GetEnvironmentVariable("APPDATA"),
					"\\",
					Helper.App_Directory_Name,
					"\\",
					Helper._SigninID,
					"_user.Config"
				});
				string UserConfigPath = path;
				string LastUserConfigPath = path2;
				FileInfo fi = new FileInfo(UserConfigPath);
				FileInfo fi2 = new FileInfo(LastUserConfigPath);
				fi.CopyTo(LastUserConfigPath);
				fi.CopyTo(LastUserConfigPath, true);
				config = ConfigurationManager.OpenMappedExeConfiguration(map, ConfigurationUserLevel.None);
			}
			config.AppSettings.Settings["IsHold"].Value = Helper._isHold;
			config.AppSettings.Settings["IsMute"].Value = Helper._isMute;
			config.AppSettings.Settings["IsThreeWay"].Value = Helper._isThreeWay;
			config.AppSettings.Settings["IsConsult"].Value = Helper._isConsult;
			config.AppSettings.Settings["IsStopConsult"].Value = Helper._isStopConsult;
			config.AppSettings.Settings["IsConsultTransfer"].Value = Helper._isConsultTransfer;
			config.AppSettings.Settings["IsTransfer"].Value = Helper._isTransfer;
			config.AppSettings.Settings["IsGrade"].Value = Helper._isGrade;
			config.AppSettings.Settings["IsCallAgent"].Value = Helper._isCallAgent;
			config.AppSettings.Settings["IsCallOut"].Value = Helper._isCallOut;
			config.AppSettings.Settings["IsListen"].Value = Helper._isListen;
			config.AppSettings.Settings["IsWhisper"].Value = Helper._isWhisper;
			config.AppSettings.Settings["IsBargein"].Value = Helper._isBargein;
			config.AppSettings.Settings["IsForceHangup"].Value = Helper._isForceHangup;
			config.AppSettings.Settings["IsCheck"].Value = Helper._isCheck;
			config.AppSettings.Settings["IsMonitor"].Value = Helper._isMonitor;
			config.AppSettings.Settings["IsCancelApplication"].Value = Helper._isCancelApplication;
			config.AppSettings.Settings["IsdbAfterHangup"].Value = Helper._isdbAfterHangup;
			config.AppSettings.Settings["IsApprove"].Value = Helper._isApprove;
			config.AppSettings.Settings["IsNoAnswerCalls"].Value = Helper._isNoAnswerCalls;
			config.AppSettings.Settings["IsControlsSet"].Value = Helper._isControls;
			config.AppSettings.Settings["IsBasic"].Value = Helper._isBasic;
			config.AppSettings.Settings["IsAdvanced"].Value = Helper._isAdvanced;
			config.AppSettings.Settings["IsOthers"].Value = Helper._isOthers;
			config.Save(ConfigurationSaveMode.Modified);
			ConfigurationManager.RefreshSection("appSettings");
			return true;
		}

		public static bool write_SystemConfig_to_file()
		{
			if (Helper._md5key == "")
			{
				Helper._md5key = "jackzhao";
			}
			ExeConfigurationFileMap map = new ExeConfigurationFileMap();
			Configuration config;
			if (string.IsNullOrEmpty(Helper.AgentID))
			{
				map.ExeConfigFilename = Environment.GetEnvironmentVariable("APPDATA") + "\\" + Helper.App_Directory_Name + "\\user.Config";
				config = ConfigurationManager.OpenMappedExeConfiguration(map, ConfigurationUserLevel.None);
			}
			else
			{
				map.ExeConfigFilename = string.Concat(new string[]
				{
					Environment.GetEnvironmentVariable("APPDATA"),
					"\\",
					Helper.App_Directory_Name,
					"\\",
					Helper.AgentID,
					"_user.Config"
				});
				config = ConfigurationManager.OpenMappedExeConfiguration(map, ConfigurationUserLevel.None);
				if (!config.HasFile)
				{
					map.ExeConfigFilename = Environment.GetEnvironmentVariable("APPDATA") + "\\" + Helper.App_Directory_Name + "\\user.Config";
					config = ConfigurationManager.OpenMappedExeConfiguration(map, ConfigurationUserLevel.None);
				}
			}
			config.AppSettings.Settings["serverIP"].Value = Helper._ServerIp;
			config.AppSettings.Settings["port"].Value = Helper._Port.ToString();
			config.AppSettings.Settings["sipServer"].Value = Helper._sipServer;
			config.AppSettings.Settings["sipPort"].Value = Helper._sipPort.ToString();
			config.AppSettings.Settings["sipNum"].Value = Helper._sipNum;
			config.AppSettings.Settings["sipPwd"].Value = Helper._md5.MD5Encrypt(Helper._sipPwd, Helper._md5key);
			if (Helper._sipAutoAnswer)
			{
				config.AppSettings.Settings["sipAutoAnswer"].Value = "1";
			}
			else
			{
				config.AppSettings.Settings["sipAutoAnswer"].Value = "0";
			}
			config.AppSettings.Settings["sipRegistTime"].Value = Helper._sipRegistTime.ToString();
			config.AppSettings.Settings["NoAnswerCallsURL"].Value = Helper._noAnswerCallsURL;
			config.AppSettings.Settings["ChkSoftPhone"].Value = Helper._SoftPhone;
			config.AppSettings.Settings["ChkExternalPhone"].Value = Helper._ExternalPhone;
			config.AppSettings.Settings["ChkNotBind"].Value = Helper._NotBind;
			config.AppSettings.Settings["ChkIdle"].Value = Helper._Idle;
			config.AppSettings.Settings["ChkBusy"].Value = Helper._Busy;
			config.AppSettings.Settings["ChkManualCallOut"].Value = Helper._ManualCallOut;
			config.Save(ConfigurationSaveMode.Modified);
			ConfigurationManager.RefreshSection("appSettings");
			return true;
		}

		public static bool reWrite_SystemConfig_to_file()
		{
			if (Helper._md5key == "")
			{
				Helper._md5key = "jackzhao";
			}
			ExeConfigurationFileMap map = new ExeConfigurationFileMap();
			map.ExeConfigFilename = string.Concat(new string[]
			{
				Environment.GetEnvironmentVariable("APPDATA"),
				"\\",
				Helper.App_Directory_Name,
				"\\",
				Helper._SigninID,
				"_user.Config"
			});
			Configuration config = ConfigurationManager.OpenMappedExeConfiguration(map, ConfigurationUserLevel.None);
			if (!config.HasFile)
			{
				string path = Environment.GetEnvironmentVariable("APPDATA") + "\\" + Helper.App_Directory_Name + "\\user.Config";
				string path2 = string.Concat(new string[]
				{
					Environment.GetEnvironmentVariable("APPDATA"),
					"\\",
					Helper.App_Directory_Name,
					"\\",
					Helper._SigninID,
					"_user.Config"
				});
				string UserConfigPath = path;
				string LastUserConfigPath = path2;
				FileInfo fi = new FileInfo(UserConfigPath);
				FileInfo fi2 = new FileInfo(LastUserConfigPath);
				fi.CopyTo(LastUserConfigPath);
				fi.CopyTo(LastUserConfigPath, true);
				config = ConfigurationManager.OpenMappedExeConfiguration(map, ConfigurationUserLevel.None);
			}
			config.AppSettings.Settings["serverIP"].Value = Helper._ServerIp;
			config.AppSettings.Settings["port"].Value = Helper._Port.ToString();
			config.AppSettings.Settings["sipServer"].Value = Helper._sipServer;
			config.AppSettings.Settings["sipPort"].Value = Helper._sipPort.ToString();
			config.AppSettings.Settings["sipNum"].Value = Helper._sipNum;
			config.AppSettings.Settings["sipPwd"].Value = Helper._md5.MD5Encrypt(Helper._sipPwd, Helper._md5key);
			if (Helper._sipAutoAnswer)
			{
				config.AppSettings.Settings["sipAutoAnswer"].Value = "1";
			}
			else
			{
				config.AppSettings.Settings["sipAutoAnswer"].Value = "0";
			}
			config.AppSettings.Settings["sipRegistTime"].Value = Helper._sipRegistTime.ToString();
			config.AppSettings.Settings["NoAnswerCallsURL"].Value = Helper._noAnswerCallsURL;
			config.Save(ConfigurationSaveMode.Modified);
			ConfigurationManager.RefreshSection("appSettings");
			return true;
		}

		public static bool write_all_config_to_file()
		{
			if (Helper._md5key == "")
			{
				Helper._md5key = "jackzhao";
			}
			ExeConfigurationFileMap map = new ExeConfigurationFileMap();
			map.ExeConfigFilename = string.Concat(new string[]
			{
				Environment.GetEnvironmentVariable("APPDATA"),
				"\\",
				Helper.App_Directory_Name,
				"\\",
				Helper._SigninID,
				"_user.Config"
			});
			Configuration config = ConfigurationManager.OpenMappedExeConfiguration(map, ConfigurationUserLevel.None);
			if (!config.HasFile)
			{
				string path = Environment.GetEnvironmentVariable("APPDATA") + "\\" + Helper.App_Directory_Name + "\\user.Config";
				string path2 = string.Concat(new string[]
				{
					Environment.GetEnvironmentVariable("APPDATA"),
					"\\",
					Helper.App_Directory_Name,
					"\\",
					Helper._SigninID,
					"_user.Config"
				});
				string UserConfigPath = path;
				string LastUserConfigPath = path2;
				FileInfo fi = new FileInfo(UserConfigPath);
				FileInfo fi2 = new FileInfo(LastUserConfigPath);
				fi.CopyTo(LastUserConfigPath);
				fi.CopyTo(LastUserConfigPath, true);
				config = ConfigurationManager.OpenMappedExeConfiguration(map, ConfigurationUserLevel.None);
			}
			config.AppSettings.Settings["callinWebURL"].Value = Helper._CallInWebURL;
			config.AppSettings.Settings["calloutWebURL"].Value = Helper._CallOutWebURL;
			config.AppSettings.Settings["homePageWebURL"].Value = Helper._HomePageWebURL;
			config.AppSettings.Settings["sipCallid"].Value = Helper._sipCallid;
			config.AppSettings.Settings["AutoSetIdleFromACKTime"].Value = Helper._autoSetIdleFromACKTime.ToString();
			if (Helper._isMonitorOfflineAgent)
			{
				config.AppSettings.Settings["IsMonitorOfflineAgent"].Value = "1";
			}
			else
			{
				config.AppSettings.Settings["IsMonitorOfflineAgent"].Value = "0";
			}
			config.AppSettings.Settings["NoAnswerCallsURL"].Value = Helper._noAnswerCallsURL;
			config.AppSettings.Settings["OpenUrlWhenConsult"].Value = (Helper._openUrlWhenConsultCallin ? "1" : "0");
			config.AppSettings.Settings["OpenUrlWhenThreeWay"].Value = (Helper._openUrlWhenThreeWayCallin ? "1" : "0");
			config.AppSettings.Settings["OpenUrlWhenEavesDrop"].Value = (Helper._openUrlWhenEavesDropCallin ? "1" : "0");
			config.AppSettings.Settings["OpenUrlWhenWhisper"].Value = (Helper._openUrlWhenWhisperCallin ? "1" : "0");
			config.AppSettings.Settings["OpenUrlWhenBargin"].Value = (Helper._openUrlWhenBargeinCallin ? "1" : "0");
			config.AppSettings.Settings["OpenUrlWhenForceHangup"].Value = (Helper._openUrlWhenForceHangupCallin ? "1" : "0");
			config.AppSettings.Settings["QueueMonitorTipEnable"].Value = (Helper._queueMonitorEnable ? "1" : "0");
			config.AppSettings.Settings["IsSoftphoneAutoAnswer"].Value = (Helper.IsSoftphoneAutoAnswer ? "1" : "0");
			config.AppSettings.Settings["ScreenPhoneFormat"].Value = Helper._screenPhoneFormat;
			config.Save(ConfigurationSaveMode.Modified);
			ConfigurationManager.RefreshSection("appSettings");
			return true;
		}
	}
}
