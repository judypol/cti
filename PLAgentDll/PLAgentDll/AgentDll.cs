using log4net;
using NetDll;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace PLAgentDll
{
	[ClassInterface(ClassInterfaceType.None), ComSourceInterfaces(typeof(IAgentEvents)), ComVisible(true), Guid("8864E46A-955E-4e67-81D5-421F30B2A666")]
	public class AgentDll : IAgentDll, IDisposable
	{
		public delegate void EVT_AgentDelegate(AgentEvent agent_event);

		public delegate void HeartBeatEVT_AgentDelegate(AgentEvent agent_event);

		private const int MaxResonseTime = 20000;

		private const int MAX_BUFFER_SIZE = 4096;

		private volatile bool blnConnect;

		private bool blnLogin;

		private bool blnSignIn;

		private bool blnMute;

		private bool blnHold;

		private bool blnListen;

		private bool blnTalking;

		private bool blnIdle;

		private bool blnBusy;

		private string mCuID;

		private string mAgentID;

		private string mAgentExten;

		private string LocalIpAddress;

		private int mPort;

		private volatile bool mIsStop_ThrReceive;

		private volatile bool mIsAlive_ThrReceive = false;

		private bool blnResponsed = true;

		private Timer tmrCheckResponse;

		private int mIntCountResTime;

		private ILog log;

		private volatile Socket client;

		private Thread ReceiveEventThread;

		private Thread HandleEventThread;

		private Thread HandleHeartBeatEventThread;

		private Semaphore sem;

		private Semaphore semHeartBeat;

		private string strQueueEventBuffer;

		private Queue<string> QueueEvents = new Queue<string>();

		private Queue<string> QueueHeartbeatEvents = new Queue<string>();

		private ArrayList event_buffer_arraylst = new ArrayList();

        public event AgentDll.EVT_AgentDelegate AgentEvents;

        public event AgentDll.HeartBeatEVT_AgentDelegate AgentHeartbeatEvents;

		public bool IsConnected
		{
			get
			{
				return this.blnConnect;
			}
		}

		public bool IsLogin
		{
			get
			{
				return this.blnLogin;
			}
		}

		public bool IsSignIn
		{
			get
			{
				return this.blnSignIn;
			}
		}

		public bool IsHold
		{
			get
			{
				return this.blnHold;
			}
		}

		public bool IsMute
		{
			get
			{
				return this.blnMute;
			}
		}

		public bool IsTalking
		{
			get
			{
				return this.blnTalking;
			}
		}

		public bool IsListen
		{
			get
			{
				return this.blnListen;
			}
		}

		public bool IsIdle
		{
			get
			{
				return this.blnIdle;
			}
		}

		public bool IsBusy
		{
			get
			{
				return this.blnBusy;
			}
		}

		public string AgentID
		{
			get
			{
				return this.mAgentID;
			}
		}

		public string AgentExten
		{
			get
			{
				return this.mAgentExten;
			}
		}

		public string LocalIP
		{
			get
			{
				return this.LocalIpAddress;
			}
		}

		public int ClientPort
		{
			get
			{
				return this.mPort;
			}
		}

		public AgentDll()
		{
			this.blnConnect = false;
			this.client = null;
			clsSocket.initSocket();
			XmlControl.init();
			this.Initialize();
			this.sem = new Semaphore(0, 2048);
			this.semHeartBeat = new Semaphore(0, 2048);
			this.HandleEventThread = new Thread(new ThreadStart(this.HandleEvents));
			this.HandleEventThread.Start();
			this.HandleHeartBeatEventThread = new Thread(new ThreadStart(this.HandleHeartBeatEvents));
			this.HandleHeartBeatEventThread.Start();
		}

		public int Initialize()
		{
			int result;
			try
			{
				if (null == this.log)
				{
					this.log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
					this.log.Info("AgentDll init is success！");
				}
				result = 0;
			}
			catch
			{
				result = 200;
			}
			return result;
		}

		private void ChectResponseTimeout(object obj)
		{
			if (this.blnResponsed)
			{
				Thread.Sleep(100);
			}
			else if (this.mIntCountResTime > 2000)
			{
				this.SendMyEvent(new AgentEvent
				{
					deAgentEventType = AgentEventType.AGENT_EVENT_MYDEFINE,
					eEventQualifier = EventQualifier.Sys_ResponseTimeOut
				});
			}
			else
			{
				this.mIntCountResTime++;
			}
		}

		public int testadd(int a, int b)
		{
			return a + b;
		}

		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected void Dispose(bool disposing)
		{
			this.log.Debug("enter Dispose.");
			if (disposing)
			{
			}
			if (this.ReceiveEventThread != null)
			{
				if (this.ReceiveEventThread.IsAlive)
				{
					this.ReceiveEventThread.Abort();
				}
				this.ReceiveEventThread.Join(50);
				this.log.Debug("ReceiveEventThread is close......");
			}
			if (this.HandleEventThread != null)
			{
				if (this.HandleEventThread.IsAlive)
				{
					this.HandleEventThread.Abort();
				}
				this.HandleEventThread.Join(50);
				this.log.Debug("HandleEventThread is close......");
			}
			if (this.HandleHeartBeatEventThread != null)
			{
				if (this.HandleHeartBeatEventThread.IsAlive)
				{
					this.HandleHeartBeatEventThread.Abort();
				}
				this.HandleHeartBeatEventThread.Join(50);
				this.log.Debug("HandleHeartBeatEventThread is close......");
			}
		}

		~AgentDll()
		{
			this.Dispose(false);
		}

		public long PL_ConnectToCti(string ServerIP, int Port)
		{
			this.log.Debug("enter PL_ConnectToCti.ServerIP:ServerIP,Port" + Port);
			if (this.client != null)
			{
				this.log.Debug("the client is not NULL,so first,we will call NetDll.clsSocket.DisConnectServer.");
				lock (this.client)
				{
					if (clsSocket.DisConnectServer(this.client) != 0)
					{
						this.log.Debug("NetDll.clsSocket.DisConnectServer is failed!!we will go on...");
					}
				}
			}
			while (this.mIsAlive_ThrReceive)
			{
				this.mIsStop_ThrReceive = true;
				Thread.Sleep(10);
			}
			if (this.ReceiveEventThread != null)
			{
				if (this.ReceiveEventThread.IsAlive)
				{
					this.ReceiveEventThread.Join(50);
				}
			}
			this.log.Debug(string.Concat(new object[]
			{
				"call NetDll.clsSocket.ConnectServer...serverIP:",
				ServerIP,
				",Port:",
				Port
			}));
			this.client = clsSocket.ConnectServer(ServerIP, Port);
			Thread.Sleep(100);
			lock (this.QueueEvents)
			{
				this.QueueEvents.Clear();
			}
			lock (this.QueueHeartbeatEvents)
			{
				this.QueueHeartbeatEvents.Clear();
			}
			this.log.Debug("清空所有事件成功！");
			long result;
			if (null == this.client)
			{
				this.log.Error("NetDll.clsSocket.ConnectServer is failed!");
				result = 100L;
			}
			else
			{
				this.LocalIpAddress = this.client.LocalEndPoint.ToString();
				this.blnConnect = true;
				this.mIsStop_ThrReceive = false;
				this.ReceiveEventThread = new Thread(new ThreadStart(this.ReceiveEvents));
				this.ReceiveEventThread.Start();
				this.log.Debug("PL_ConnectToCti is success!");
				result = 0L;
			}
			return result;
		}

		public long PL_DisConnectToCti()
		{
			this.log.Debug("enter PL_DisConnectToCti");
			long result;
			if (null == this.log)
			{
				this.log.Debug("PL_DisConnectToCti is failed! reason:log is NULL!");
				result = 204L;
			}
			else if (null == this.client)
			{
				this.log.Debug("PL_DisConnectToCti is failed! reason:the client is NULL!");
				result = 203L;
			}
			else
			{
				try
				{
					this.log.Debug("call NetDll.clsSocket.DisConnectServer.RemoteEndPoint:" + this.client.RemoteEndPoint);
					long num;
					lock (this.client)
					{
						num = (long)clsSocket.DisConnectServer(this.client);
					}
					Thread.Sleep(100);
					if (this.ReceiveEventThread != null)
					{
						this.mIsStop_ThrReceive = true;
						while (this.mIsAlive_ThrReceive)
						{
							this.mIsStop_ThrReceive = true;
							Thread.Sleep(10);
						}
						if (this.ReceiveEventThread.IsAlive)
						{
							this.ReceiveEventThread.Join(50);
						}
					}
					lock (this.QueueEvents)
					{
						this.QueueEvents.Clear();
					}
					lock (this.QueueHeartbeatEvents)
					{
						this.QueueHeartbeatEvents.Clear();
					}
					this.log.Debug("清空所有事件成功！");
					if (num == 0L)
					{
						this.blnConnect = false;
						this.log.Debug("NetDll.clsSocket.DisConnectServer is success!");
						result = 0L;
					}
					else
					{
						this.log.Debug("disconnect is fail!ErrCode=" + num);
						result = num;
					}
				}
				catch (Exception ex)
				{
					this.log.Error(string.Concat(new string[]
					{
						"PL_DisConnectToCti throw exception!",
						ex.Source,
						",信息:",
						ex.Message,
						",堆栈:",
						ex.StackTrace
					}));
					Thread.Sleep(100);
					this.mIsStop_ThrReceive = true;
					while (this.mIsAlive_ThrReceive)
					{
						this.mIsStop_ThrReceive = true;
						Thread.Sleep(100);
					}
					lock (this.QueueEvents)
					{
						this.QueueEvents.Clear();
					}
					lock (this.QueueHeartbeatEvents)
					{
						this.QueueHeartbeatEvents.Clear();
					}
					this.log.Debug("清空所有事件成功！");
					result = 101L;
				}
			}
			return result;
		}

		public long PL_Login(string AgentID, string AgentPwd)
		{
			this.log.Debug("enter PL_Login.AgentID:" + AgentID + ",AgentPwd" + AgentPwd);
			long result;
			if (null == this.client)
			{
				result = 203L;
			}
			else if (null == this.log)
			{
				result = 204L;
			}
			else
			{
				try
				{
					string text = this.MakeCommandLogin("login", AgentID, AgentPwd);
					text = this.MakeCommandHead(text.Length) + text;
					result = this.PL_SendXML(text);
				}
				catch (Exception ex)
				{
					this.client = null;
					this.log.Error(string.Concat(new string[]
					{
						ex.Source,
						",信息:",
						ex.Message,
						",堆栈:",
						ex.StackTrace
					}));
					result = 100L;
				}
			}
			return result;
		}

		public long PL_Logout(string AgentID, string AgentExten)
		{
			this.log.Debug("enter PL_Logout.AgentID:" + AgentID + ",AgentExten" + AgentExten);
			long result;
			if (null == this.client)
			{
				result = 203L;
			}
			else if (null == this.log)
			{
				result = 204L;
			}
			else
			{
				try
				{
					string strXML = this.MakeCommand2Call("logout", AgentID, AgentExten);
					result = this.PL_SendXML(strXML);
				}
				catch (Exception ex)
				{
					this.client = null;
					this.log.Error(string.Concat(new string[]
					{
						ex.Source,
						",信息:",
						ex.Message,
						",堆栈:",
						ex.StackTrace
					}));
					result = 100L;
				}
			}
			return result;
		}

		public long PL_SignIn(string AgentID, string AgentPwd, string AgentExten, string BindExten, string InitStatus, string WebUrl, string ExtenIsOutbound)
		{
			this.log.Debug(string.Concat(new string[]
			{
				"enter PL_SignIn.AgentID:",
				AgentID,
				",AgentPwd:",
				AgentPwd,
				",AgentExten:",
				AgentExten,
				",BindExten:",
				BindExten,
				",InitStatus:",
				InitStatus,
				",WebUrl:",
				WebUrl,
				",ExtenIsOutbound:",
				ExtenIsOutbound
			}));
			long result;
			if (null == this.client)
			{
				result = 203L;
			}
			else if (null == this.log)
			{
				result = 204L;
			}
			else if (AgentID == "" || AgentPwd == "" || BindExten == "" || InitStatus == "")
			{
				result = 400L;
			}
			else
			{
				if (WebUrl == "")
				{
					WebUrl = "http://localhost/";
				}
				try
				{
					string strXML = this.MakeCommandSignIn("signin", AgentID, AgentPwd, AgentExten, BindExten, InitStatus, ExtenIsOutbound);
					result = this.PL_SendXML(strXML);
				}
				catch (Exception ex)
				{
					this.client = null;
					this.log.Error(string.Concat(new string[]
					{
						ex.Source,
						",信息:",
						ex.Message,
						",堆栈:",
						ex.StackTrace
					}));
					result = 100L;
				}
			}
			return result;
		}

		public long PL_SignOut(string AgentID)
		{
			this.log.Debug("enter PL_SignOut.AgentID:" + AgentID);
			long result;
			if (AgentID == "")
			{
				this.log.Info("AgentID is empty,so we will return false！");
				result = 400L;
			}
			else if (null == this.client)
			{
				this.log.Info("client is NULL,so we will return false！");
				result = 203L;
			}
			else if (null == this.log)
			{
				this.log.Info("log is NULL,so we will return false！");
				result = 204L;
			}
			else
			{
				try
				{
					string strXML = this.MakeCommandSignOut("signout", AgentID);
					result = this.PL_SendXML(strXML);
				}
				catch (Exception ex)
				{
					this.client = null;
					this.log.Error(string.Concat(new string[]
					{
						ex.Source,
						",信息:",
						ex.Message,
						",堆栈:",
						ex.StackTrace
					}));
					result = 100L;
				}
			}
			return result;
		}

		public long PL_GetAgentDefineStatus(string AgentID)
		{
			this.log.Debug("enter PL_GetAgentDefineStatus.AgentID:" + AgentID);
			long result;
			if (AgentID == "")
			{
				this.log.Info("AgentID is empty,so we will return false！");
				result = 400L;
			}
			else if (null == this.client)
			{
				result = 203L;
			}
			else if (null == this.log)
			{
				result = 204L;
			}
			else
			{
				try
				{
					string strXML = this.MakeCommandGetAgentDefineStatus("get_defined_status", AgentID);
					result = this.PL_SendXML(strXML);
				}
				catch (Exception ex)
				{
					this.client = null;
					this.log.Error(string.Concat(new string[]
					{
						ex.Source,
						",信息:",
						ex.Message,
						",堆栈:",
						ex.StackTrace
					}));
					result = 100L;
				}
			}
			return result;
		}

		public long PL_GetAgentWebSiteInfo(string AgentID)
		{
			this.log.Debug("enter PL_GetWebSiteInfo.AgentID:" + AgentID);
			long result;
			if (AgentID == "")
			{
				this.log.Info("AgentID is empty,so we will return false！");
				result = 400L;
			}
			else if (null == this.client)
			{
				result = 203L;
			}
			else if (null == this.log)
			{
				result = 204L;
			}
			else
			{
				try
				{
					string strXML = this.MakeCommandGetAgentWebSiteInfo("get_website_info", AgentID);
					result = this.PL_SendXML(strXML);
				}
				catch (Exception ex)
				{
					this.client = null;
					this.log.Error(string.Concat(new string[]
					{
						ex.Source,
						",信息:",
						ex.Message,
						",堆栈:",
						ex.StackTrace
					}));
					result = 100L;
				}
			}
			return result;
		}

		public long PL_CallOut(string AgentID, string CalledID, string DisplayNum, int callout_type, string taskid, string is_send_msg, string send_msg_url, string customerForeignId)
		{
			this.log.Debug(string.Concat(new object[]
			{
				"enter PL_CallOut.AgentID:",
				AgentID,
				",CalledID:",
				CalledID,
				",DisplayNum:",
				DisplayNum,
				",callout_type:",
				callout_type,
				",taskid:",
				taskid,
				",customerForeignId:",
				customerForeignId
			}));
			long result;
			if (null == this.client)
			{
				result = 203L;
			}
			else if (null == this.log)
			{
				result = 204L;
			}
			else if (AgentID == "" || CalledID == "")
			{
				this.log.Info("AgentID or CalledID  is empty,so we will return false！");
				result = 400L;
			}
			else
			{
				try
				{
					string strXML;
					if (callout_type == 2)
					{
						strXML = this.MakeCommandPreviewCallOut("preview_callout", AgentID, CalledID, DisplayNum, taskid, is_send_msg, send_msg_url);
					}
					else
					{
						strXML = this.MakeCommandCallOut("manual_callout", AgentID, CalledID, DisplayNum, customerForeignId);
					}
					result = this.PL_SendXML(strXML);
				}
				catch (Exception ex)
				{
					this.client = null;
					this.log.Error(ex.Message);
					result = 100L;
				}
			}
			return result;
		}

		public long PL_CallAgent(string AgentID, string CalledID)
		{
			this.log.Debug("enter PL_CallAgent.AgentID:" + AgentID + ",CalledID:" + CalledID);
			long result;
			if (null == this.client)
			{
				result = 203L;
			}
			else if (null == this.log)
			{
				result = 204L;
			}
			else if (AgentID == "" || CalledID == "")
			{
				this.log.Info("AgentID or CalledID is empty,so we will return false！");
				result = 400L;
			}
			else
			{
				try
				{
					string strXML = this.MakeCommandCallAgent("internal_call", AgentID, this.AgentExten, CalledID);
					result = this.PL_SendXML(strXML);
				}
				catch (Exception ex)
				{
					this.client = null;
					this.log.Error(ex.Message);
					result = 100L;
				}
			}
			return result;
		}

		public long PL_HangUp(string AgentID, string AgentCallUuid)
		{
			this.log.Debug("enter PL_HangUp.AgentID:" + AgentID + " AgentCallUuid:" + AgentCallUuid);
			long result;
			if (null == this.client)
			{
				result = 203L;
			}
			else if (null == this.log)
			{
				result = 204L;
			}
			else if (AgentID == "")
			{
				this.log.Info("AgentID  is empty,so we will return false！");
				result = 400L;
			}
			else
			{
				try
				{
					string strXML = this.MakeCommandHangup("hangup", AgentID, AgentCallUuid);
					result = this.PL_SendXML(strXML);
				}
				catch (Exception ex)
				{
					this.client = null;
					this.log.Error(ex.Message);
					result = 100L;
				}
			}
			return result;
		}

		public long PL_Grade(string AgentID, string Language, string CustomerCallUuid)
		{
			this.log.Debug(string.Concat(new string[]
			{
				"enter PL_Grade.AgentID：",
				AgentID,
				",Language:",
				Language,
				",CustomerCallUuid:",
				CustomerCallUuid
			}));
			long result;
			if (null == this.client)
			{
				result = 203L;
			}
			else if (null == this.log)
			{
				result = 204L;
			}
			else if (string.IsNullOrEmpty(AgentID) || string.IsNullOrEmpty(CustomerCallUuid))
			{
				this.log.Info("AgentID  is empty,so we will return false！");
				result = 400L;
			}
			else
			{
				try
				{
					string strXML = this.MakeCommandGrade("evaluate", AgentID, Language, CustomerCallUuid);
					result = this.PL_SendXML(strXML);
				}
				catch (Exception ex)
				{
					this.client = null;
					this.log.Error(ex.Message);
					result = 100L;
				}
			}
			return result;
		}

		public long PL_SignAck(string AgentID, string AgentExten)
		{
			long result;
			if (null == this.client)
			{
				result = 203L;
			}
			else if (null == this.log)
			{
				result = 204L;
			}
			else if (AgentID == "" || AgentExten == "")
			{
				this.log.Info("AgentID or AgentExten  is empty,so we will return false！");
				result = 400L;
			}
			else
			{
				try
				{
					string strXML = this.MakeCommand2Call("signin_ack", AgentID, AgentExten);
					result = this.PL_SendXML(strXML);
				}
				catch (Exception ex)
				{
					this.client = null;
					this.log.Error(ex.Message);
					result = 100L;
				}
			}
			return result;
		}

		public long PL_SignFin(string AgentID, string AgentExten)
		{
			long result;
			if (null == this.client)
			{
				result = 203L;
			}
			else if (null == this.log)
			{
				result = 204L;
			}
			else if (AgentID == "" || AgentExten == "")
			{
				this.log.Info("AgentID or AgentExten  is empty,so we will return false！");
				result = 400L;
			}
			else
			{
				try
				{
					string strXML = this.MakeCommand2Call("signin_fin", AgentID, AgentExten);
					result = this.PL_SendXML(strXML);
				}
				catch (Exception ex)
				{
					this.client = null;
					this.log.Error(ex.Message);
					result = 100L;
				}
			}
			return result;
		}

		public long PL_Check(string AgentID)
		{
			this.log.Debug("enter PL_Check.AgentID:" + AgentID);
			long result;
			if (null == this.client)
			{
				result = 203L;
			}
			else if (null == this.log)
			{
				result = 204L;
			}
			else if (AgentID == "")
			{
				this.log.Info("AgentID  is empty,so we will return false！");
				result = 400L;
			}
			else
			{
				try
				{
					string strXML = this.MakeCommandCheck("echo_test", AgentID);
					result = this.PL_SendXML(strXML);
				}
				catch (Exception ex)
				{
					this.client = null;
					this.log.Error(ex.Message);
					result = 100L;
				}
			}
			return result;
		}

		public long PL_SetAgentDefineStatus(string AgentID, int TargetStatus, int IsNeedApproval)
		{
			this.log.Debug(string.Concat(new object[]
			{
				"enter PL_SetAgentDefineStatus.AgentID:",
				AgentID,
				",TargetStatus",
				TargetStatus,
				",IsNeedApproval=",
				IsNeedApproval
			}));
			long result;
			if (null == this.client)
			{
				result = 203L;
			}
			else if (null == this.log)
			{
				result = 204L;
			}
			else if (AgentID == "")
			{
				this.log.Info("AgentID  is empty,so we will return false！");
				result = 400L;
			}
			else
			{
				try
				{
					string strXML = this.MakeCommandAgentStatusChange("change_status", AgentID, TargetStatus, IsNeedApproval);
					result = this.PL_SendXML(strXML);
				}
				catch (Exception ex)
				{
					this.client = null;
					this.log.Error(ex.Message);
					result = 100L;
				}
			}
			return result;
		}

		public long PL_SetAgentDefineStatus_force(string AgentID, int TargetStatus, string needApproveflag)
		{
			this.log.Debug(string.Concat(new object[]
			{
				"enter PL_SetAgentDefineStatus.AgentID:",
				AgentID,
				",TargetStatus",
				TargetStatus,
				",needApproveflag=",
				needApproveflag
			}));
			long result;
			if (null == this.client)
			{
				result = 203L;
			}
			else if (null == this.log)
			{
				result = 204L;
			}
			else if (AgentID == "")
			{
				this.log.Info("AgentID  is empty,so we will return false！");
				result = 400L;
			}
			else
			{
				try
				{
					string strXML = this.MakeCommandAgentStatusChange_force("change_status", AgentID, TargetStatus, needApproveflag);
					result = this.PL_SendXML(strXML);
				}
				catch (Exception ex)
				{
					this.client = null;
					this.log.Error(ex.Message);
					result = 100L;
				}
			}
			return result;
		}

		public long PL_HeartBeat(string AgentID)
		{
			this.log.Debug("enter PL_HeartBeat.AgentID:" + AgentID);
			long result;
			if (null == this.client)
			{
				result = 203L;
			}
			else if (null == this.log)
			{
				result = 204L;
			}
			else
			{
				try
				{
					string strXML = this.MakeCommandHeartBeat("heartbeat", AgentID);
					result = this.PL_SendXML(strXML);
				}
				catch (Exception ex)
				{
					this.client = null;
					this.log.Error(ex.Message);
					result = 100L;
				}
			}
			return result;
		}

		public long PL_Hold(string AgentID, string AgentCallUuid)
		{
			this.log.Debug("enter PL_Hold.AgentID:" + AgentID + ",AgentCallUuid:" + AgentCallUuid);
			long result;
			if ("" == AgentID)
			{
				this.log.Info("AgentID is empty,so we will return false！");
				result = 400L;
			}
			else if (null == this.client)
			{
				result = 203L;
			}
			else if (null == this.log)
			{
				result = 204L;
			}
			else
			{
				try
				{
					string strXML = this.MakeCommandHoldOrMute("hold", AgentID, AgentCallUuid);
					result = this.PL_SendXML(strXML);
				}
				catch (Exception ex)
				{
					this.client = null;
					this.log.Error(ex.Message);
					result = 100L;
				}
			}
			return result;
		}

		public long PL_StopHold(string AgentID, string AgentCallUuid)
		{
			this.log.Debug("enter PL_StopHold.AgentID:" + AgentID + ",AgentCallUuid:" + AgentCallUuid);
			long result;
			if ("" == AgentID)
			{
				this.log.Info("AgentID  is empty,so we will return false！");
				result = 400L;
			}
			else if (null == this.client)
			{
				result = 203L;
			}
			else if (null == this.log)
			{
				result = 204L;
			}
			else
			{
				try
				{
					string strXML = this.MakeCommandStopHoldOrMute("unhold", AgentID, AgentCallUuid);
					result = this.PL_SendXML(strXML);
				}
				catch (Exception ex)
				{
					this.client = null;
					this.log.Error(ex.Message);
					result = 100L;
				}
			}
			return result;
		}

		public long PL_Mute(string AgentID, string AgentCallUuid)
		{
			this.log.Debug("enter PL_Mute.AgentID:" + AgentID + ",AgentCallUuid:" + AgentCallUuid);
			long result;
			if (string.IsNullOrEmpty(AgentID))
			{
				this.log.Info("AgentID is empty,so we will return false！");
				result = 400L;
			}
			else if (null == this.client)
			{
				result = 203L;
			}
			else if (null == this.log)
			{
				result = 204L;
			}
			else
			{
				try
				{
					string strXML = this.MakeCommandHoldOrMute("mute", AgentID, AgentCallUuid);
					result = this.PL_SendXML(strXML);
				}
				catch (Exception ex)
				{
					this.client = null;
					this.log.Error(ex.Message);
					result = 100L;
				}
			}
			return result;
		}

		public long PL_StopMute(string AgentID, string AgentCallUuid)
		{
			this.log.Debug("enter PL_StopMute.AgentID:" + AgentID + ",AgentCallUuid:" + AgentCallUuid);
			long result;
			if (string.IsNullOrEmpty(AgentID))
			{
				this.log.Info("AgentID  is empty,so we will return false！");
				result = 400L;
			}
			else if (null == this.client)
			{
				result = 203L;
			}
			else if (null == this.log)
			{
				result = 204L;
			}
			else
			{
				try
				{
					string strXML = this.MakeCommandStopHoldOrMute("unmute", AgentID, AgentCallUuid);
					result = this.PL_SendXML(strXML);
				}
				catch (Exception ex)
				{
					this.client = null;
					this.log.Error(ex.Message);
					result = 100L;
				}
			}
			return result;
		}

		public long PL_Consult(string AgentID, string DestAgentID, string AgentCallUuid, bool trdIsOutbound)
		{
			this.log.Debug(string.Concat(new string[]
			{
				"enter PL_Consult.AgentID:",
				AgentID,
				",DestAgentID:",
				DestAgentID,
				",AgentCallUuid:",
				AgentCallUuid
			}));
			long result;
			if (null == this.client)
			{
				result = 203L;
			}
			else if (null == this.log)
			{
				result = 204L;
			}
			else if (string.IsNullOrEmpty(AgentID) || string.IsNullOrEmpty(DestAgentID) || string.IsNullOrEmpty(AgentCallUuid))
			{
				this.log.Info("AgentID or DestAgentID  is empty,so we will return false！");
				result = 400L;
			}
			else
			{
				try
				{
					string strTrdIsOutbound;
					if (trdIsOutbound)
					{
						strTrdIsOutbound = "true";
					}
					else
					{
						strTrdIsOutbound = "false";
					}
					string strXML = this.MakeCommandConsult("consult", AgentID, DestAgentID, AgentCallUuid, strTrdIsOutbound);
					result = this.PL_SendXML(strXML);
				}
				catch (Exception ex)
				{
					this.client = null;
					this.log.Error(ex.Message);
					result = 100L;
				}
			}
			return result;
		}

		public long PL_ConsultCancel(string AgentID, string AgentCallUuid)
		{
			this.log.Debug("enter PL_ConsultCancel.AgentID:" + AgentID + ",AgentCallUuid:" + AgentCallUuid);
			long result;
			if (string.IsNullOrEmpty(AgentID) || string.IsNullOrEmpty(AgentCallUuid))
			{
				this.log.Info("AgentID   is empty,so we will return false！");
				result = 400L;
			}
			else if (null == this.client)
			{
				result = 203L;
			}
			else if (null == this.log)
			{
				result = 204L;
			}
			else
			{
				try
				{
					string strXML = this.MakeCommandConsultCancel("consult_cancel", AgentID, AgentCallUuid);
					result = this.PL_SendXML(strXML);
				}
				catch (Exception ex)
				{
					this.client = null;
					this.log.Error(ex.Message);
					result = 100L;
				}
			}
			return result;
		}

		public long PL_Listen(string AgentID, string targetAgentNum)
		{
			this.log.Debug("enter PL_Listen.AgentID:" + AgentID + ",targetAgentNum" + targetAgentNum);
			long result;
			if (null == this.client)
			{
				result = 203L;
			}
			else if (null == this.log)
			{
				result = 204L;
			}
			else if (AgentID == "" || targetAgentNum == "")
			{
				this.log.Info("AgentID or targetAgentNum  is empty,so we will return false！");
				result = 400L;
			}
			else
			{
				try
				{
					string strXML = this.MakeCommandListen("eavesdrop", AgentID, targetAgentNum);
					result = this.PL_SendXML(strXML);
				}
				catch (Exception ex)
				{
					this.client = null;
					this.log.Error(ex.Message);
					result = 100L;
				}
			}
			return result;
		}

		public long PL_Whisper(string AgentID, string targetAgentNum)
		{
			this.log.Debug("enter PL_Whisper.AgentID:" + AgentID + ",targetAgentNum:" + targetAgentNum);
			long result;
			if (null == this.client)
			{
				result = 203L;
			}
			else if (null == this.log)
			{
				result = 204L;
			}
			else if (AgentID == "" || targetAgentNum == "")
			{
				this.log.Info("AgentID or targetAgentNum  is empty,so we will return false！");
				result = 400L;
			}
			else
			{
				try
				{
					string strXML = this.MakeCommandWhisper("whisper", AgentID, targetAgentNum);
					result = this.PL_SendXML(strXML);
				}
				catch (Exception ex)
				{
					this.client = null;
					this.log.Error(ex.Message);
					result = 100L;
				}
			}
			return result;
		}

		public long PL_Bargein(string AgentID, string targetAgentNum)
		{
			this.log.Debug("enter PL_Bargein.AgentID:" + AgentID + ",targetAgentNum:" + targetAgentNum);
			long result;
			if (null == this.client)
			{
				result = 203L;
			}
			else if (null == this.log)
			{
				result = 204L;
			}
			else if (AgentID == "" || targetAgentNum == "")
			{
				this.log.Info("AgentID or targetAgentNum  is empty,so we will return false！");
				result = 400L;
			}
			else
			{
				try
				{
					string strXML = this.MakeCommandBargein("bargein", AgentID, targetAgentNum);
					result = this.PL_SendXML(strXML);
				}
				catch (Exception ex)
				{
					this.client = null;
					this.log.Error(ex.Message);
					result = 100L;
				}
			}
			return result;
		}

		public long PL_ForceHangup(string AgentID, string targetAgentNum)
		{
			this.log.Debug("enter PL_ForceHangup.AgentID:" + AgentID + ",targetAgentNum:" + targetAgentNum);
			long result;
			if (null == this.client)
			{
				result = 203L;
			}
			else if (null == this.log)
			{
				result = 204L;
			}
			else if (AgentID == "" || targetAgentNum == "")
			{
				this.log.Info("AgentID or targetAgentNum  is empty,so we will return false！");
				result = 400L;
			}
			else
			{
				try
				{
					string strXML = this.MakeCommandForceHangup("force_hangup", AgentID, targetAgentNum);
					result = this.PL_SendXML(strXML);
				}
				catch (Exception ex)
				{
					this.client = null;
					this.log.Error(ex.Message);
					result = 100L;
				}
			}
			return result;
		}

		public long PL_StopListen(string AgentID, string AgentExten, string CuID)
		{
			this.log.Debug(string.Concat(new string[]
			{
				"enter PL_StopListen.AgentID:",
				AgentID,
				",AgentExten:",
				AgentExten,
				",CuID",
				CuID
			}));
			long result;
			if (null == this.client)
			{
				result = 203L;
			}
			else if (null == this.log)
			{
				result = 204L;
			}
			else
			{
				try
				{
					string strXML = this.MakeCommand2Call("stoplisten", AgentID, AgentExten, CuID);
					result = this.PL_SendXML(strXML);
				}
				catch (Exception ex)
				{
					this.client = null;
					this.log.Error(ex.Message);
					result = 100L;
				}
			}
			return result;
		}

		public long PL_OnForceChangeStatus(string AgentID, string targetAgentNum, string status)
		{
			this.log.Debug(string.Concat(new string[]
			{
				"enter PL_OnForceChangeStatus.AgentID:",
				AgentID,
				",targetAgentNum:",
				targetAgentNum,
				", status",
				status
			}));
			long result;
			if (null == this.client)
			{
				result = 203L;
			}
			else if (null == this.log)
			{
				result = 204L;
			}
			else if (AgentID == "" || targetAgentNum == "" || status == "")
			{
				this.log.Info("AgentID or targetAgentNum or status is empty,so we will return false！");
				result = 400L;
			}
			else
			{
				try
				{
					string strXML = this.MakeCommandForceChangeStatus("force_change_status", AgentID, targetAgentNum, status);
					result = this.PL_SendXML(strXML);
				}
				catch (Exception ex)
				{
					this.client = null;
					this.log.Error(ex.Message);
					result = 100L;
				}
			}
			return result;
		}

		public long PL_TransferAgent(string AgentID, string destAgentID, string CustomerCallUuid, string strOutBoundFlag)
		{
			this.log.Debug(string.Concat(new string[]
			{
				"enter PL_TransferAgent.AgentID:",
				AgentID,
				",destAgentID:",
				destAgentID,
				",CustomerCallUuid:",
				CustomerCallUuid,
				",strOutBoundFlag:",
				strOutBoundFlag
			}));
			long result;
			if (null == this.client)
			{
				result = 203L;
			}
			else if (null == this.log)
			{
				result = 204L;
			}
			else if (AgentID == "" || destAgentID == "" || strOutBoundFlag == "")
			{
				this.log.Info("AgentID or destAgentID or strOutBoundFlag is empty,so we will return false！");
				result = 400L;
			}
			else
			{
				try
				{
					string strXML = this.MakeCommandTransferAgent("blind_transfer", AgentID, destAgentID, CustomerCallUuid, strOutBoundFlag);
					result = this.PL_SendXML(strXML);
				}
				catch (Exception ex)
				{
					this.client = null;
					this.log.Error(ex.Message);
					result = 100L;
				}
			}
			return result;
		}

		public long PL_TransferIvr(string AgentID, string ivrNum, string CustomerCallUuid)
		{
			this.log.Debug(string.Concat(new string[]
			{
				"enter PL_TransferIvr.AgentID:",
				AgentID,
				",ivrNum:",
				ivrNum,
				",CustomerCallUuid:",
				CustomerCallUuid
			}));
			long result;
			if (null == this.client)
			{
				result = 203L;
			}
			else if (null == this.log)
			{
				result = 204L;
			}
			else if (AgentID == "" || ivrNum == "")
			{
				this.log.Info("AgentID or ivrNum  is empty,so we will return false！");
				result = 400L;
			}
			else
			{
				try
				{
					string strXML = this.MakeCommandTransferIvr("transfer_ivr", AgentID, ivrNum, CustomerCallUuid);
					result = this.PL_SendXML(strXML);
				}
				catch (Exception ex)
				{
					this.client = null;
					this.log.Error(ex.Message);
					result = 100L;
				}
			}
			return result;
		}

		public long PL_TransferQueue(string AgentID, string queueNum, string CustomerCallUuid)
		{
			this.log.Debug(string.Concat(new string[]
			{
				"enter PL_TransferQueue.AgentID:",
				AgentID,
				",queueNum",
				queueNum,
				",CustomerCallUuid",
				CustomerCallUuid
			}));
			long result;
			if (null == this.client)
			{
				result = 203L;
			}
			else if (null == this.log)
			{
				result = 204L;
			}
			else if (AgentID == "" || queueNum == "")
			{
				result = 400L;
			}
			else
			{
				try
				{
					string strXML = this.MakeCommandTransferQueue("transfer_queue", AgentID, queueNum, CustomerCallUuid);
					result = this.PL_SendXML(strXML);
				}
				catch (Exception ex)
				{
					this.client = null;
					this.log.Error(ex.Message);
					result = 100L;
				}
			}
			return result;
		}

		public long PL_TransferIvrProfile(string AgentID, string ivrProfileNum, string CustomerCallUuid)
		{
			this.log.Debug(string.Concat(new string[]
			{
				"enter PL_TransferIvrProfile.AgentID:",
				AgentID,
				",ivrProfileNum:",
				ivrProfileNum,
				",CustomerCallUuid:",
				CustomerCallUuid
			}));
			long result;
			if (null == this.client)
			{
				result = 203L;
			}
			else if (null == this.log)
			{
				result = 204L;
			}
			else if (string.Empty == AgentID || string.Empty == ivrProfileNum)
			{
				this.log.Info("AgentID or ivrProfileNum  is empty,so we will return false！");
				result = 400L;
			}
			else
			{
				try
				{
					string strXML = this.MakeCommandTransferIvrProfile("transfer_ivr_profile", AgentID, ivrProfileNum, CustomerCallUuid);
					result = this.PL_SendXML(strXML);
				}
				catch (Exception ex)
				{
					this.client = null;
					this.log.Error(ex.Message);
					result = 100L;
				}
			}
			return result;
		}

		public long PL_GetAgentOnline(string AgentID, string specificNum, int numType)
		{
			this.log.Debug(string.Concat(new object[]
			{
				"enter PL_GetAgentOnline.AgentID:",
				AgentID,
				",specificNum:",
				specificNum,
				",numType=",
				numType
			}));
			long result;
			if (null == this.client)
			{
				result = 203L;
			}
			else if (null == this.log)
			{
				result = 204L;
			}
			else if (AgentID == "")
			{
				result = 400L;
			}
			else
			{
				try
				{
					string strXML = this.MakeCommandGetAgentOnline("get_online_agents", AgentID, specificNum, numType);
					result = this.PL_SendXML(strXML);
				}
				catch (Exception ex)
				{
					this.client = null;
					this.log.Error(ex.Message);
					result = 100L;
				}
			}
			return result;
		}

		public long PL_GetIvrList(string AgentID)
		{
			this.log.Debug("enter PL_GetIvrList.AgentID:" + AgentID);
			long result;
			if (null == this.client)
			{
				result = 203L;
			}
			else if (AgentID == "")
			{
				result = 400L;
			}
			else if (null == this.log)
			{
				result = 204L;
			}
			else
			{
				try
				{
					string strXML = this.MakeCommandGetIvrList("get_ivr_list", AgentID);
					result = this.PL_SendXML(strXML);
				}
				catch (Exception ex)
				{
					this.client = null;
					this.log.Error(ex.Message);
					result = 100L;
				}
			}
			return result;
		}

		public long PL_GetQueueList(string AgentID)
		{
			this.log.Debug("enter PL_GetQueueList.AgentID:" + AgentID);
			long result;
			if (null == this.client)
			{
				result = 203L;
			}
			else if (AgentID == "" || AgentID == null)
			{
				result = 400L;
			}
			else if (null == this.log)
			{
				result = 204L;
			}
			else
			{
				try
				{
					string strXML = this.MakeCommandGetQueueList("get_queue_list", AgentID);
					result = this.PL_SendXML(strXML);
				}
				catch (Exception ex)
				{
					this.client = null;
					this.log.Error(ex.Message);
					result = 100L;
				}
			}
			return result;
		}

		public long PL_GetIvrProfileList(string AgentID)
		{
			this.log.Debug("enter PL_GetIvrProfileList.AgentID:" + AgentID);
			long result;
			if (null == this.client)
			{
				result = 203L;
			}
			else if (string.Empty == AgentID)
			{
				result = 400L;
			}
			else if (null == this.log)
			{
				result = 204L;
			}
			else
			{
				try
				{
					string strXML = this.MakeCommandGetIvrProfileList("get_ivr_profile_list", AgentID);
					result = this.PL_SendXML(strXML);
				}
				catch (Exception ex)
				{
					this.client = null;
					this.log.Error(ex.Message);
					result = 100L;
				}
			}
			return result;
		}

		public long PL_Get_Defined_Role_Rights(string AgentID, string AgentExten)
		{
			this.log.Debug("enter PL_Get_Defined_Role_Rights.AgentID:" + AgentID + ",AgentExten:" + AgentExten);
			long result;
			if (null == this.client)
			{
				result = 203L;
			}
			else if (AgentID == "" || AgentExten == "")
			{
				result = 400L;
			}
			else if (null == this.log)
			{
				result = 204L;
			}
			else
			{
				try
				{
					string strXML = this.MakeCommandGetDefinedRoleAndRight("get_defined_role_rights", AgentID);
					result = this.PL_SendXML(strXML);
				}
				catch (Exception ex)
				{
					this.client = null;
					this.log.Error(ex.Message);
					result = 100L;
				}
			}
			return result;
		}

		public long PL_Intercept(string AgentID, string AgentExten, string CuID)
		{
			this.log.Debug(string.Concat(new string[]
			{
				"enter PL_Intercept.AgentID:",
				AgentID,
				",AgentExten:",
				AgentExten,
				",Cuid:",
				CuID
			}));
			long result;
			if (null == this.client)
			{
				result = 203L;
			}
			else if (AgentID == "" || AgentExten == "")
			{
				result = 400L;
			}
			else if (null == this.log)
			{
				result = 204L;
			}
			else
			{
				try
				{
					string strXML = this.MakeCommand2Call("intercept", AgentID, AgentExten, CuID);
					result = this.PL_SendXML(strXML);
				}
				catch (Exception ex)
				{
					this.client = null;
					this.log.Error(ex.Message);
					result = 100L;
				}
			}
			return result;
		}

		public long PL_Interrupt(string AgentID, string AgentExten, string CuID)
		{
			this.log.Debug(string.Concat(new string[]
			{
				"enter PL_Interrupt.AgentID:",
				AgentID,
				",AgentExten:",
				AgentExten,
				",Cuid:",
				CuID
			}));
			long result;
			if (null == this.client)
			{
				result = 203L;
			}
			else if (null == this.log)
			{
				result = 204L;
			}
			else
			{
				try
				{
					string strXML = this.MakeCommand2Call("interrupt", AgentID, AgentExten, CuID);
					result = this.PL_SendXML(strXML);
				}
				catch (Exception ex)
				{
					this.client = null;
					this.log.Error(ex.Message);
					result = 100L;
				}
			}
			return result;
		}

		public long PL_ForceDisconnect(string AgentID, string AgentExten, string CuID)
		{
			this.log.Debug(string.Concat(new string[]
			{
				"enter PL_ForceDisconnect.AgentID:",
				AgentID,
				",AgentExten:",
				AgentExten,
				",Cuid:",
				CuID
			}));
			long result;
			if (null == this.client)
			{
				result = 203L;
			}
			else if (null == this.log)
			{
				result = 204L;
			}
			else
			{
				try
				{
					string strXML = this.MakeCommand2Call("forcedisconnect", AgentID, AgentExten, CuID);
					result = this.PL_SendXML(strXML);
				}
				catch (Exception ex)
				{
					this.client = null;
					this.log.Error(ex.Message);
					result = 100L;
				}
			}
			return result;
		}

		public long PL_GetAccessNumbers(string AgentID)
		{
			this.log.Debug("enter PL_GetAccessNumbers.AgentID:" + AgentID);
			long result;
			if (null == this.client)
			{
				result = 203L;
			}
			else if (AgentID == "")
			{
				result = 400L;
			}
			else if (null == this.log)
			{
				result = 204L;
			}
			else
			{
				try
				{
					string strXML = this.MakeCommandGetAccessNumber("get_access_numbers", AgentID);
					result = this.PL_SendXML(strXML);
				}
				catch (Exception ex)
				{
					this.client = null;
					this.log.Error(ex.Message);
					result = 100L;
				}
			}
			return result;
		}

		public long PL_ThreeWay(string AgentID, string TrdAgentNum, string AgentCallUuid, bool trdIsOutbound)
		{
			this.log.Debug(string.Concat(new string[]
			{
				"enter PL_ThreeWay.AgentID:",
				AgentID,
				",TrdAgentNum:",
				TrdAgentNum,
				",AgentCallUuid:",
				AgentCallUuid
			}));
			long result;
			if (string.IsNullOrEmpty(AgentID) || string.IsNullOrEmpty(TrdAgentNum) || string.IsNullOrEmpty(AgentCallUuid))
			{
				result = 400L;
			}
			else if (null == this.client)
			{
				result = 203L;
			}
			else if (null == this.log)
			{
				result = 204L;
			}
			else
			{
				try
				{
					string strTrdIsOutbound;
					if (trdIsOutbound)
					{
						strTrdIsOutbound = "true";
					}
					else
					{
						strTrdIsOutbound = "false";
					}
					string strXML = this.MakeCommandThreeWay("threeway", AgentID, TrdAgentNum, AgentCallUuid, strTrdIsOutbound);
					result = this.PL_SendXML(strXML);
				}
				catch (Exception ex)
				{
					this.client = null;
					this.log.Error(ex.Message);
					result = 100L;
				}
			}
			return result;
		}

		public long PL_ThreeWayCancel(string AgentID, string AgentCallUuid)
		{
			this.log.Debug("enter PL_ThreeWayCancel.AgentID:" + AgentID + ",AgentCallUuid:" + AgentCallUuid);
			long result;
			if (string.IsNullOrEmpty(AgentID) || string.IsNullOrEmpty(AgentCallUuid))
			{
				result = 400L;
			}
			else if (null == this.client)
			{
				result = 203L;
			}
			else if (null == this.log)
			{
				result = 204L;
			}
			else
			{
				try
				{
					string strXML = this.MakeCommandThreeWayCancel("threeway_cancel", AgentID, AgentCallUuid);
					result = this.PL_SendXML(strXML);
				}
				catch (Exception ex)
				{
					this.client = null;
					this.log.Error(ex.Message);
					result = 100L;
				}
			}
			return result;
		}

		public long PL_ConsultTransfer(string AgentID, string AgentCallUuid)
		{
			this.log.Debug("enter PL_ConsultTransfer.AgentID:" + AgentID + ",AgentCallUuid:" + AgentCallUuid);
			long result;
			if (string.IsNullOrEmpty(AgentID) || string.IsNullOrEmpty(AgentCallUuid))
			{
				result = 400L;
			}
			else if (null == this.client)
			{
				result = 203L;
			}
			else if (null == this.log)
			{
				result = 204L;
			}
			else
			{
				try
				{
					string strXML = this.MakeCommandConsultTransfer("consult_transfer", AgentID, AgentCallUuid);
					result = this.PL_SendXML(strXML);
				}
				catch (Exception ex)
				{
					this.client = null;
					this.log.Error(ex.Message);
					result = 100L;
				}
			}
			return result;
		}

		public long PL_GetDefinedRoleAndRight(string AgentID)
		{
			this.log.Debug("enter PL_GetDefinedRoleAndRight.AgentID:" + AgentID);
			long result;
			if (AgentID == "")
			{
				result = 400L;
			}
			else if (null == this.client)
			{
				result = 203L;
			}
			else if (null == this.log)
			{
				result = 204L;
			}
			else
			{
				try
				{
					string strXML = this.MakeCommandGetDefinedRoleAndRight("get_defined_role_rights", AgentID);
					result = this.PL_SendXML(strXML);
				}
				catch (Exception ex)
				{
					this.client = null;
					this.log.Error(ex.Message);
					result = 100L;
				}
			}
			return result;
		}

		public long PL_GetAgentGroupList(string AgentID, string agentGroupRange)
		{
			this.log.Debug("enter PL_GetAgentGroupList.AgentID:" + AgentID + "agentGroupRange:" + agentGroupRange);
			long result;
			if (AgentID == "")
			{
				result = 400L;
			}
			else if (null == this.client)
			{
				result = 203L;
			}
			else if (null == this.log)
			{
				result = 204L;
			}
			else
			{
				try
				{
					string strXML = this.MakeCommandGetAgentGroupList("get_agentgroup_list", AgentID, agentGroupRange);
					result = this.PL_SendXML(strXML);
				}
				catch (Exception ex)
				{
					this.client = null;
					this.log.Error(ex.Message);
					result = 100L;
				}
			}
			return result;
		}

		public long PL_GetAgentsOfQueue(string AgentID, string QueueNum)
		{
			this.log.Debug("enter PL_GetAgentsOfQueue.AgentID:" + AgentID + ",QueueNum:" + QueueNum);
			long result;
			if (AgentID == "" || QueueNum == "")
			{
				result = 400L;
			}
			else if (null == this.client)
			{
				result = 203L;
			}
			else if (null == this.log)
			{
				result = 204L;
			}
			else
			{
				try
				{
					string strXML = this.MakeCommandGetAgentsOfQueue("get_agents_of_queue", AgentID, QueueNum);
					result = this.PL_SendXML(strXML);
				}
				catch (Exception ex)
				{
					this.client = null;
					this.log.Error(ex.Message);
					result = 100L;
				}
			}
			return result;
		}

		public long PL_GetAgentsOfAgentGroup(string AgentID, string AgentGroupNum)
		{
			this.log.Debug("enter PL_GetAgentsOfAgentGroup.AgentID:" + AgentID + ",AgentGroupNum:" + AgentGroupNum);
			long result;
			if (AgentID == "" || AgentGroupNum == "")
			{
				result = 400L;
			}
			else if (null == this.client)
			{
				result = 203L;
			}
			else if (null == this.log)
			{
				result = 204L;
			}
			else
			{
				try
				{
					string strXML = this.MakeCommandGetAgentsOfAgentGroup("get_agents_of_agentgroup", AgentID, AgentGroupNum);
					result = this.PL_SendXML(strXML);
				}
				catch (Exception ex)
				{
					this.client = null;
					this.log.Error(ex.Message);
					result = 100L;
				}
			}
			return result;
		}

		public long PL_GetAgentsMonitorInfo(string AgentID, string AgentsStr)
		{
			this.log.Debug("enter PL_GetAgentsMonitorInfo.AgentID:" + AgentID + ",AgentsStr:" + AgentsStr);
			long result;
			if (AgentID == "" || AgentsStr == "")
			{
				result = 400L;
			}
			else if (null == this.client)
			{
				result = 203L;
			}
			else if (null == this.log)
			{
				result = 204L;
			}
			else
			{
				try
				{
					string strXML = this.MakeCommandGetAgentsMonitorInfo("get_agents_monitor_info", AgentID, AgentsStr);
					result = this.PL_SendXML(strXML);
				}
				catch (Exception ex)
				{
					this.client = null;
					this.log.Error(ex.Message);
					result = 100L;
				}
			}
			return result;
		}

		public long PL_GetDetailCallInfo(string AgentID, string targetAgentNum)
		{
			this.log.Debug("enter PL_GetAgentsMonitorInfo.AgentID:" + AgentID + ",targetAgentNum:" + targetAgentNum);
			long result;
			if (AgentID == "" || targetAgentNum == "")
			{
				result = 400L;
			}
			else if (null == this.client)
			{
				result = 203L;
			}
			else if (null == this.log)
			{
				result = 204L;
			}
			else
			{
				try
				{
					string strXML = this.MakeCommandGetDetailCallInfo("get_detail_call_info", AgentID, targetAgentNum);
					result = this.PL_SendXML(strXML);
				}
				catch (Exception ex)
				{
					this.client = null;
					this.log.Error(ex.Message);
					result = 100L;
				}
			}
			return result;
		}

		public long PL_GetCustomerOfQueue(string AgentID, string queueNumLstStr)
		{
			this.log.Debug("enter PL_GetCustomerOfQueue.AgentID:" + AgentID + ",queueNumLstStr:" + queueNumLstStr);
			long result;
			if (AgentID == "" || queueNumLstStr == "")
			{
				result = 400L;
			}
			else if (null == this.client)
			{
				result = 203L;
			}
			else if (null == this.log)
			{
				result = 204L;
			}
			else
			{
				try
				{
					string strXML = this.MakeCommandGetCustomerOfQueue("get_customer_of_queue", AgentID, queueNumLstStr);
					result = this.PL_SendXML(strXML);
				}
				catch (Exception ex)
				{
					this.client = null;
					this.log.Error(ex.Message);
					result = 100L;
				}
			}
			return result;
		}

		public long PL_GetCustomerOfMyQueue(string AgentID)
		{
			this.log.Debug("enter PL_GetCustomerOfMyQueue.AgentID:" + AgentID);
			long result;
			if (AgentID == "")
			{
				result = 400L;
			}
			else if (null == this.client)
			{
				result = 203L;
			}
			else if (null == this.log)
			{
				result = 204L;
			}
			else
			{
				try
				{
					string strXML = this.MakeCommandGetCustomerOfMyQueue("get_customer_of_my_queue", AgentID);
					result = this.PL_SendXML(strXML);
				}
				catch (Exception ex)
				{
					this.client = null;
					this.log.Error(ex.Message);
					result = 100L;
				}
			}
			return result;
		}

		public long PL_GetQueueStatis(string AgentID, string queueNumLstStr)
		{
			this.log.Debug("enter PL_GetQueueStatis.AgentID:" + AgentID + ",queueNumLstStr:" + queueNumLstStr);
			long result;
			if (AgentID == "" || queueNumLstStr == "")
			{
				result = 400L;
			}
			else if (null == this.client)
			{
				result = 203L;
			}
			else if (null == this.log)
			{
				result = 204L;
			}
			else
			{
				try
				{
					string strXML = this.MakeCommandGetQueueStatis("get_queue_statis", AgentID, queueNumLstStr);
					result = this.PL_SendXML(strXML);
				}
				catch (Exception ex)
				{
					this.client = null;
					this.log.Error(ex.Message);
					result = 100L;
				}
			}
			return result;
		}

		public long PL_GetAllQueueStatis(string AgentID)
		{
			this.log.Debug("enter PL_GetAllQueueStatis.AgentID:" + AgentID);
			long result;
			if (AgentID == "")
			{
				result = 400L;
			}
			else if (null == this.client)
			{
				result = 203L;
			}
			else if (null == this.log)
			{
				result = 204L;
			}
			else
			{
				try
				{
					string strXML = this.MakeCommandGetAllQueueStatis("get_all_queue_statis", AgentID);
					result = this.PL_SendXML(strXML);
				}
				catch (Exception ex)
				{
					this.client = null;
					this.log.Error(ex.Message);
					result = 100L;
				}
			}
			return result;
		}

		public long PL_Reload(string agentID, string reloadType)
		{
			this.log.Debug("enter PL_Reload.AgentID:" + this.AgentID + ",reloadType:" + reloadType);
			long result;
			if (reloadType == "")
			{
				result = 400L;
			}
			else if (null == this.client)
			{
				result = 203L;
			}
			else if (null == this.log)
			{
				result = 204L;
			}
			else
			{
				try
				{
					string strXML = this.MakeCommandReload("reload", agentID, reloadType);
					result = this.PL_SendXML(strXML);
				}
				catch (Exception ex)
				{
					this.client = null;
					this.log.Error(ex.Message);
					result = 100L;
				}
			}
			return result;
		}

		public long PL_UpdateAgent(string agentID, string agentNum)
		{
			this.log.Debug("enter PL_UpdateAgent.AgentID:" + this.AgentID + ",agentNum:" + agentNum);
			long result;
			if (agentNum == "")
			{
				result = 400L;
			}
			else if (null == this.client)
			{
				result = 203L;
			}
			else if (null == this.log)
			{
				result = 204L;
			}
			else
			{
				try
				{
					string strXML = this.MakeCommandUpdateAgent("updateAgent", agentID, agentNum);
					result = this.PL_SendXML(strXML);
				}
				catch (Exception ex)
				{
					this.client = null;
					this.log.Error(ex.Message);
					result = 100L;
				}
			}
			return result;
		}

		public long PL_DelAgent(string agentID, string agentNum)
		{
			this.log.Debug("enter PL_DelAgent.AgentID:" + this.AgentID + ",agentNum:" + agentNum);
			long result;
			if (agentNum == "")
			{
				result = 400L;
			}
			else if (null == this.client)
			{
				result = 203L;
			}
			else if (null == this.log)
			{
				result = 204L;
			}
			else
			{
				try
				{
					string strXML = this.MakeCommandDelAgent("delAgent", agentID, agentNum);
					result = this.PL_SendXML(strXML);
				}
				catch (Exception ex)
				{
					this.client = null;
					this.log.Error(ex.Message);
					result = 100L;
				}
			}
			return result;
		}

		public long PL_GetPersonalInfo(string AgentID)
		{
			this.log.Debug("enter PL_GetPersonalInfo.AgentID:" + AgentID);
			long result;
			if (string.IsNullOrEmpty(AgentID))
			{
				result = 400L;
			}
			else if (null == this.client)
			{
				result = 203L;
			}
			else if (null == this.log)
			{
				result = 204L;
			}
			else
			{
				try
				{
					string strXML = this.MakeCommandGetPersonalInfo("get_agent_personal_info", AgentID);
					result = this.PL_SendXML(strXML);
				}
				catch (Exception ex)
				{
					this.client = null;
					this.log.Error(ex.Message);
					result = 100L;
				}
			}
			return result;
		}

		public long PL_SetPersonalInfo(string AgentID, string strMobile, string strEmail)
		{
			this.log.Debug(string.Concat(new string[]
			{
				"enter PL_SetPersonalInfo.AgentID:",
				AgentID,
				"strMobile:",
				strMobile,
				"strEmail:",
				strEmail
			}));
			long result;
			if (string.IsNullOrEmpty(AgentID) || string.IsNullOrEmpty(strMobile) || string.IsNullOrEmpty(strEmail))
			{
				result = 400L;
			}
			else if (null == this.client)
			{
				result = 203L;
			}
			else if (null == this.log)
			{
				result = 204L;
			}
			else
			{
				try
				{
					string strXML = this.MakeCommandSetPersonalInfo("set_agent_personal_info", AgentID, strMobile, strEmail);
					result = this.PL_SendXML(strXML);
				}
				catch (Exception ex)
				{
					this.client = null;
					this.log.Error(ex.Message);
					result = 100L;
				}
			}
			return result;
		}

		public long PL_ChangePswd(string AgentID, string strOldPswd, string strNewPswd)
		{
			this.log.Debug(string.Concat(new string[]
			{
				"enter PL_SetPersonalInfo.AgentID:",
				AgentID,
				"strOldPswd:",
				strOldPswd,
				"strNewPswd:",
				strNewPswd
			}));
			long result;
			if (string.IsNullOrEmpty(AgentID) || string.IsNullOrEmpty(strOldPswd) || string.IsNullOrEmpty(strNewPswd))
			{
				result = 400L;
			}
			else if (null == this.client)
			{
				result = 203L;
			}
			else if (null == this.log)
			{
				result = 204L;
			}
			else
			{
				try
				{
					string strXML = this.MakeCommandChangePswd("change_pswd", AgentID, strOldPswd, strNewPswd);
					result = this.PL_SendXML(strXML);
				}
				catch (Exception ex)
				{
					this.client = null;
					this.log.Error(ex.Message);
					result = 100L;
				}
			}
			return result;
		}

		public long PL_GetReportStatisInfo(string AgentID)
		{
			this.log.Debug("enter PL_GetReportStatisInfo.AgentID:" + AgentID);
			long result;
			if (string.IsNullOrEmpty(AgentID))
			{
				result = 400L;
			}
			else if (null == this.client)
			{
				result = 203L;
			}
			else if (null == this.log)
			{
				result = 204L;
			}
			else
			{
				try
				{
					string strXML = this.MakeCommandGetReportStatisInfo("get_report_statis_info", AgentID);
					result = this.PL_SendXML(strXML);
				}
				catch (Exception ex)
				{
					this.client = null;
					this.log.Error(ex.Message);
					result = 100L;
				}
			}
			return result;
		}

		public long PL_ApplyChangeStatus(string AgentID, string targetStatus)
		{
			this.log.Debug("enter PL_ApplyChangeStatus.AgentID:" + AgentID);
			long result;
			if (string.IsNullOrEmpty(AgentID))
			{
				result = 400L;
			}
			else if (null == this.client)
			{
				result = 203L;
			}
			else if (null == this.log)
			{
				result = 204L;
			}
			else
			{
				try
				{
					string strXML = this.MakeCommandApplyChangeStatus("apply_change_status", AgentID, targetStatus);
					result = this.PL_SendXML(strXML);
				}
				catch (Exception ex)
				{
					this.client = null;
					this.log.Error(ex.Message);
					result = 100L;
				}
			}
			return result;
		}

		public long PL_ApplyApproval(string AgentID, string applyAgentID, string targetStatus, string passFlag)
		{
			this.log.Debug(string.Concat(new string[]
			{
				"enter PL_ApplyApproval.AgentID:",
				AgentID,
				",targetStatus:",
				targetStatus,
				",approvalResult:",
				passFlag
			}));
			long result;
			if (string.IsNullOrEmpty(AgentID))
			{
				result = 400L;
			}
			else if (null == this.client)
			{
				result = 203L;
			}
			else if (null == this.log)
			{
				result = 204L;
			}
			else
			{
				try
				{
					string strXML = this.MakeCommandApplyApproval("approve_change_status_result", AgentID, applyAgentID, targetStatus, passFlag);
					result = this.PL_SendXML(strXML);
				}
				catch (Exception ex)
				{
					this.client = null;
					this.log.Error(ex.Message);
					result = 100L;
				}
			}
			return result;
		}

		public long PL_ApplyCancel(string AgentID, string targetStatus)
		{
			this.log.Debug("enter PL_ApplyCancel.AgentID:" + AgentID + ",targetStatus:" + targetStatus);
			long result;
			if (string.IsNullOrEmpty(AgentID))
			{
				result = 400L;
			}
			else if (string.IsNullOrEmpty(targetStatus))
			{
				result = 400L;
			}
			else if (null == this.client)
			{
				result = 203L;
			}
			else if (null == this.log)
			{
				result = 204L;
			}
			else
			{
				try
				{
					string strXML = this.MakeCommandApplyCancel("cancel_apply_change_status", AgentID, targetStatus);
					result = this.PL_SendXML(strXML);
				}
				catch (Exception ex)
				{
					this.client = null;
					this.log.Error(ex.Message);
					result = 100L;
				}
			}
			return result;
		}

		public long PL_GetChangeStatusApplyList(string AgentID, string targetStatus)
		{
			this.log.Debug("enter PL_ApplyCancel.AgentID:" + AgentID);
			long result;
			if (string.IsNullOrEmpty(AgentID))
			{
				result = 400L;
			}
			else if (null == this.client)
			{
				result = 203L;
			}
			else if (null == this.log)
			{
				result = 204L;
			}
			else
			{
				try
				{
					string strXML = this.MakeCommandGetChangeStatusApplyList("get_change_status_apply_list", AgentID, targetStatus);
					result = this.PL_SendXML(strXML);
				}
				catch (Exception ex)
				{
					this.client = null;
					this.log.Error(ex.Message);
					result = 100L;
				}
			}
			return result;
		}

		public long PL_getAgentGroupStatusMaxNum(string AgentID, string agentGroupNameLstStr, string agentGroupIdLstStr, string targetStatus)
		{
			this.log.Debug(string.Concat(new string[]
			{
				"enter PL_getAgentGroupStatusMaxNum.AgentID:",
				AgentID,
				"agentGroupNameLstStr:",
				agentGroupNameLstStr,
				",agentGroupIdLstStr",
				agentGroupIdLstStr,
				",targetStatus:",
				targetStatus
			}));
			long result;
			if (string.IsNullOrEmpty(AgentID))
			{
				result = 400L;
			}
			else if (string.IsNullOrEmpty(agentGroupIdLstStr))
			{
				result = 400L;
			}
			else if (string.IsNullOrEmpty(targetStatus))
			{
				result = 400L;
			}
			else if (null == this.client)
			{
				result = 203L;
			}
			else if (null == this.log)
			{
				result = 204L;
			}
			else
			{
				try
				{
					string strXML = this.MakeCommandGetAgentGroupStatusMaxNum("get_agentgroup_status_max_num", AgentID, agentGroupNameLstStr, agentGroupIdLstStr, targetStatus);
					result = this.PL_SendXML(strXML);
				}
				catch (Exception ex)
				{
					this.client = null;
					this.log.Error(ex.Message);
					result = 100L;
				}
			}
			return result;
		}

		public long PL_setAgentGroupStatusMaxNum(string AgentID, string agentGroupNameLstStr, string agentGroupIdLstStr, string targetStatus, string maxStatusNum)
		{
			this.log.Debug(string.Concat(new string[]
			{
				"enter PL_setAgentGroupStatusMaxNum.AgentID:",
				AgentID,
				"agentGroupNameLstStr:",
				agentGroupNameLstStr,
				",agentGroupIdLstStr",
				agentGroupIdLstStr,
				",targetStatus:",
				targetStatus,
				",maxStatusNum:",
				maxStatusNum
			}));
			long result;
			if (string.IsNullOrEmpty(AgentID))
			{
				result = 400L;
			}
			else if (string.IsNullOrEmpty(agentGroupIdLstStr))
			{
				result = 400L;
			}
			else if (string.IsNullOrEmpty(targetStatus))
			{
				result = 400L;
			}
			else if (string.IsNullOrEmpty(maxStatusNum))
			{
				result = 400L;
			}
			else if (null == this.client)
			{
				result = 203L;
			}
			else if (null == this.log)
			{
				result = 204L;
			}
			else
			{
				try
				{
					string strXML = this.MakeCommandSetAgentGroupStatusMaxNum("set_agentgroup_status_max_num", AgentID, agentGroupNameLstStr, agentGroupIdLstStr, targetStatus, maxStatusNum);
					result = this.PL_SendXML(strXML);
				}
				catch (Exception ex)
				{
					this.client = null;
					this.log.Error(ex.Message);
					result = 100L;
				}
			}
			return result;
		}

		private string MakeCommand2Call(string Cmd, string AgentID, string AgentExten)
		{
			string text = string.Concat(new string[]
			{
				"<request>\n\t<cmdType>",
				Cmd,
				"</cmdType>\n\t<agentID>",
				AgentID,
				"</agentID>\n\t<agentExten>",
				AgentExten,
				"</agentExten>\n</request>"
			});
			return this.MakeCommandHead(text.Length) + text;
		}

		private string MakeCommandHeartBeat(string Cmd, string AgentID)
		{
			string text = string.Concat(new string[]
			{
				"<request>\n\t<cmdType>",
				Cmd,
				"</cmdType>\n\t<agentID>",
				AgentID,
				"</agentID>\n\t</request>"
			});
			return this.MakeCommandHead(text.Length) + text;
		}

		private string MakeCommandCheck(string Cmd, string AgentID)
		{
			string text = string.Concat(new string[]
			{
				"<request>\n\t<cmdType>",
				Cmd,
				"</cmdType>\n\t<agentID>",
				AgentID,
				"</agentID>\n\t</request>"
			});
			return this.MakeCommandHead(text.Length) + text;
		}

		private string MakeCommandHoldOrMute(string Cmd, string AgentID, string AgentCallUuid)
		{
			string text = string.Concat(new string[]
			{
				"<request>\n\t<cmdType>",
				Cmd,
				"</cmdType>\n\t<agentID>",
				AgentID,
				"</agentID>\n\t<agentCallUuid>",
				AgentCallUuid,
				"</agentCallUuid>\n\t</request>"
			});
			return this.MakeCommandHead(text.Length) + text;
		}

		private string MakeCommandThreeWayCancel(string Cmd, string AgentID, string AgentCallUuid)
		{
			string text = string.Concat(new string[]
			{
				"<request>\n\t<cmdType>",
				Cmd,
				"</cmdType>\n\t<agentID>",
				AgentID,
				"</agentID>\n\t<agentCallUuid>",
				AgentCallUuid,
				"</agentCallUuid>\n\t</request>"
			});
			return this.MakeCommandHead(text.Length) + text;
		}

		private string MakeCommandConsultTransfer(string Cmd, string AgentID, string AgentCallUuid)
		{
			string text = string.Concat(new string[]
			{
				"<request>\n\t<cmdType>",
				Cmd,
				"</cmdType>\n\t<agentID>",
				AgentID,
				"</agentID>\n\t<agentCallUuid>",
				AgentCallUuid,
				"</agentCallUuid>\n\t</request>"
			});
			return this.MakeCommandHead(text.Length) + text;
		}

		private string MakeCommandThreeWay(string Cmd, string AgentID, string trdAgentNum, string AgentCallUuid, string strTrdIsOutbound)
		{
			string text = string.Concat(new string[]
			{
				"<request>\n\t<cmdType>",
				Cmd,
				"</cmdType>\n\t<agentID>",
				AgentID,
				"</agentID>\n\t<trdAgentNum>",
				trdAgentNum,
				"</trdAgentNum>\n\t<agentCallUuid>",
				AgentCallUuid,
				"</agentCallUuid>\n\t<trdIsOutbound>",
				strTrdIsOutbound,
				"</trdIsOutbound>\n\t</request>"
			});
			return this.MakeCommandHead(text.Length) + text;
		}

		private string MakeCommandStopHoldOrMute(string Cmd, string AgentID, string AgentCallUuid)
		{
			string text = string.Concat(new string[]
			{
				"<request>\n\t<cmdType>",
				Cmd,
				"</cmdType>\n\t<agentID>",
				AgentID,
				"</agentID>\n\t<agentCallUuid>",
				AgentCallUuid,
				"</agentCallUuid>\n\t</request>"
			});
			return this.MakeCommandHead(text.Length) + text;
		}

		private string MakeCommandGetAccessNumber(string Cmd, string AgentID)
		{
			string text = string.Concat(new string[]
			{
				"<request>\n\t<cmdType>",
				Cmd,
				"</cmdType>\n\t<agentID>",
				AgentID,
				"</agentID>\n\t</request>"
			});
			return this.MakeCommandHead(text.Length) + text;
		}

		private string MakeCommandHangup(string Cmd, string AgentID, string AgentCallUuid)
		{
			string text = string.Concat(new string[]
			{
				"<request>\n\t<cmdType>",
				Cmd,
				"</cmdType>\n\t<agentID>",
				AgentID,
				"</agentID>\n\t<agentCallUuid>",
				AgentCallUuid,
				"</agentCallUuid>\n\t</request>"
			});
			return this.MakeCommandHead(text.Length) + text;
		}

		private string MakeCommandGrade(string Cmd, string AgentID, string language, string CustomerCallUuid)
		{
			string text = string.Concat(new string[]
			{
				"<request>\n\t<cmdType>",
				Cmd,
				"</cmdType>\n\t<language>",
				language,
				"</language>\n\t<agentID>",
				AgentID,
				"</agentID>\n\t<customerCallUuid>",
				CustomerCallUuid,
				"</customerCallUuid>\n\t</request>"
			});
			return this.MakeCommandHead(text.Length) + text;
		}

		private string MakeCommandAgentStatusChange(string Cmd, string AgentID, int AgentStatus, int isNeedApproval)
		{
			string text = string.Concat(new object[]
			{
				"<request>\n\t<cmdType>",
				Cmd,
				"</cmdType>\n\t<agentID>",
				AgentID,
				"</agentID>\n\t<targetStatus>",
				AgentStatus,
				"</targetStatus>\n<needApproveflag>",
				isNeedApproval,
				"</needApproveflag>\n</request>"
			});
			return this.MakeCommandHead(text.Length) + text;
		}

		private string MakeCommandAgentStatusChange_force(string Cmd, string AgentID, int AgentStatus, string needApproveflag)
		{
			string text = string.Concat(new object[]
			{
				"<request>\n\t<cmdType>",
				Cmd,
				"</cmdType>\n\t<agentID>",
				AgentID,
				"</agentID>\n\t<targetStatus>",
				AgentStatus,
				"</targetStatus>\n<needApproveflag>",
				needApproveflag,
				"</needApproveflag>\n</request>"
			});
			return this.MakeCommandHead(text.Length) + text;
		}

		private string MakeCommand2Call(string Cmd, string AgentID, string AgentExten, string CuID)
		{
			string text = string.Concat(new string[]
			{
				"<request>\n\t<cmdType>",
				Cmd,
				"</cmdType>\n\t<agentID>",
				AgentID,
				"</agentID>\n\t<agentExten>",
				AgentExten,
				"</agentExten>\n\t<cuID>",
				CuID,
				"</cuID>\n</request>"
			});
			return this.MakeCommandHead(text.Length) + text;
		}

		private string MakeCommand3Call(string Cmd, string AgentID, string AgentExten, string DestAgentID, string CuID)
		{
			string text = string.Concat(new string[]
			{
				"<request>\n\t<cmdType>",
				Cmd,
				"</cmdType>\n\t<agentID>",
				AgentID,
				"</agentID>\n\t<agentExten>",
				AgentExten,
				"</agentExten>\n\t<destAgentID>",
				DestAgentID,
				"</destAgentID>\n\t<cuID>",
				CuID,
				"</cuID>\n</request>"
			});
			return this.MakeCommandHead(text.Length) + text;
		}

		private string MakeCommandCallOut(string Cmd, string AgentID, string CalledID, string DisplayNum, string customerForeignId)
		{
			string text = string.Concat(new string[]
			{
				"<request>\n\t<cmdType>",
				Cmd,
				"</cmdType>\n\t<agentID>",
				AgentID,
				"</agentID>\n\t<customerDisplayNum>",
				DisplayNum,
				"</customerDisplayNum>\n\t<customerNum>",
				CalledID,
				"</customerNum>\n<customerForeignId>",
				customerForeignId,
				"</customerForeignId>\n</request>"
			});
			return this.MakeCommandHead(text.Length) + text;
		}

		private string MakeCommandPreviewCallOut(string Cmd, string AgentID, string CalledID, string DisplayNum, string taskid, string is_send_msg, string send_msg_url)
		{
			string text = string.Concat(new string[]
			{
				"<request>\n\t<cmdType>",
				Cmd,
				"</cmdType>\n\t<agentID>",
				AgentID,
				"</agentID>\n\t<customerDisplayNum>",
				DisplayNum,
				"</customerDisplayNum>\n\t<customerNum>",
				CalledID,
				"</customerNum>\n<taskID>",
				taskid,
				"</taskID>\n<isSendMsg>",
				is_send_msg,
				"</isSendMsg>\n<sendMsgUrl>",
				send_msg_url,
				"</sendMsgUrl>\n</request>"
			});
			return this.MakeCommandHead(text.Length) + text;
		}

		private string MakeCommandCallAgent(string Cmd, string AgentID, string AgentExten, string CalledID)
		{
			string text = string.Concat(new string[]
			{
				"<request>\n\t<cmdType>",
				Cmd,
				"</cmdType>\n\t<agentID>",
				AgentID,
				"</agentID>\n\t<calledAgentNum>",
				CalledID,
				"</calledAgentNum>\n</request>"
			});
			return this.MakeCommandHead(text.Length) + text;
		}

		private string MakeCommandConsult(string Cmd, string AgentID, string DestAgentID, string AgentCallUuid, string strTrdIsOutbound)
		{
			string text = string.Concat(new string[]
			{
				"<request>\n\t<cmdType>",
				Cmd,
				"</cmdType>\n\t<agentID>",
				AgentID,
				"</agentID>\n\t<consulteeAgentNum>",
				DestAgentID,
				"</consulteeAgentNum>\n\t<agentCallUuid>",
				AgentCallUuid,
				"</agentCallUuid>\n\t<consulteeIsOutbound>",
				strTrdIsOutbound,
				"</consulteeIsOutbound>\n</request>"
			});
			return this.MakeCommandHead(text.Length) + text;
		}

		private string MakeCommandConsultCancel(string Cmd, string AgentID, string AgentCallUuid)
		{
			string text = string.Concat(new string[]
			{
				"<request>\n\t<cmdType>",
				Cmd,
				"</cmdType>\n\t<agentID>",
				AgentID,
				"</agentID>\n\t<agentCallUuid>",
				AgentCallUuid,
				"</agentCallUuid>\n\t</request>"
			});
			return this.MakeCommandHead(text.Length) + text;
		}

		private string MakeCommandListen(string Cmd, string AgentID, string targetAgentNum)
		{
			string text = string.Concat(new string[]
			{
				"<request>\n\t<cmdType>",
				Cmd,
				"</cmdType>\n\t<agentID>",
				AgentID,
				"</agentID>\n\t<targetAgentNum>",
				targetAgentNum,
				"</targetAgentNum>\n\t</request>"
			});
			return this.MakeCommandHead(text.Length) + text;
		}

		private string MakeCommandWhisper(string Cmd, string AgentID, string targetAgentNum)
		{
			string text = string.Concat(new string[]
			{
				"<request>\n\t<cmdType>",
				Cmd,
				"</cmdType>\n\t<agentID>",
				AgentID,
				"</agentID>\n\t<targetAgentNum>",
				targetAgentNum,
				"</targetAgentNum>\n\t</request>"
			});
			return this.MakeCommandHead(text.Length) + text;
		}

		private string MakeCommandForceChangeStatus(string Cmd, string AgentID, string targetAgentNum, string strStatus)
		{
			string text = string.Concat(new string[]
			{
				"<request>\n\t<cmdType>",
				Cmd,
				"</cmdType>\n\t<agentID>",
				AgentID,
				"</agentID>\n\t<targetAgentNum>",
				targetAgentNum,
				"</targetAgentNum>\n\t<targetStatus>",
				strStatus,
				"</targetStatus >\n\t</request>"
			});
			return this.MakeCommandHead(text.Length) + text;
		}

		private string MakeCommandBargein(string Cmd, string AgentID, string targetAgentNum)
		{
			string text = string.Concat(new string[]
			{
				"<request>\n\t<cmdType>",
				Cmd,
				"</cmdType>\n\t<agentID>",
				AgentID,
				"</agentID>\n\t<targetAgentNum>",
				targetAgentNum,
				"</targetAgentNum>\n\t</request>"
			});
			return this.MakeCommandHead(text.Length) + text;
		}

		private string MakeCommandTransferAgent(string Cmd, string AgentID, string targetAgentNum, string CustomerCallUuid, string strOutBoundFlag)
		{
			string text = string.Concat(new string[]
			{
				"<request>\n\t<cmdType>",
				Cmd,
				"</cmdType>\n\t<agentID>",
				AgentID,
				"</agentID>\n\t<customerCallUuid>",
				CustomerCallUuid,
				"</customerCallUuid>\n\t<transfereeAgentNum>",
				targetAgentNum,
				"</transfereeAgentNum>\n\t<transfereeIsOutbound>",
				strOutBoundFlag,
				"</transfereeIsOutbound>\n\t</request>"
			});
			return this.MakeCommandHead(text.Length) + text;
		}

		private string MakeCommandTransferIvr(string Cmd, string AgentID, string ivrNum, string CustomerCallUuid)
		{
			string text = string.Concat(new string[]
			{
				"<request>\n\t<cmdType>",
				Cmd,
				"</cmdType>\n\t<agentID>",
				AgentID,
				"</agentID>\n\t<customerCallUuid>",
				CustomerCallUuid,
				"</customerCallUuid>\n\t<ivrNum>",
				ivrNum,
				"</ivrNum>\n\t</request>"
			});
			return this.MakeCommandHead(text.Length) + text;
		}

		private string MakeCommandTransferQueue(string Cmd, string AgentID, string queueNum, string CustomerCallUuid)
		{
			string text = string.Concat(new string[]
			{
				"<request>\n\t<cmdType>",
				Cmd,
				"</cmdType>\n\t<agentID>",
				AgentID,
				"</agentID>\n\t<customerCallUuid>",
				CustomerCallUuid,
				"</customerCallUuid>\n\t<queueNum>",
				queueNum,
				"</queueNum>\n\t</request>"
			});
			return this.MakeCommandHead(text.Length) + text;
		}

		private string MakeCommandTransferIvrProfile(string Cmd, string AgentID, string ivrProfileNum, string CustomerCallUuid)
		{
			string text = string.Concat(new string[]
			{
				"<request>\n\t<cmdType>",
				Cmd,
				"</cmdType>\n\t<agentID>",
				AgentID,
				"</agentID>\n\t<customerCallUuid>",
				CustomerCallUuid,
				"</customerCallUuid>\n\t<ivrProfileNum>",
				ivrProfileNum,
				"</ivrProfileNum>\n\t</request>"
			});
			return this.MakeCommandHead(text.Length) + text;
		}

		private string MakeCommandGetAgentOnline(string Cmd, string AgentID, string specificNum, int numType)
		{
			string text = string.Concat(new object[]
			{
				"<request>\n\t<cmdType>",
				Cmd,
				"</cmdType>\n\t<agentID>",
				AgentID,
				"</agentID>\n\t<specificNum>",
				specificNum,
				"</specificNum>\n\t<numType>",
				numType,
				"</numType>\n\t</request>"
			});
			return this.MakeCommandHead(text.Length) + text;
		}

		private string MakeCommandGetIvrList(string Cmd, string AgentID)
		{
			string text = string.Concat(new string[]
			{
				"<request>\n\t<cmdType>",
				Cmd,
				"</cmdType>\n\t<agentID>",
				AgentID,
				"</agentID>\n\t</request>"
			});
			return this.MakeCommandHead(text.Length) + text;
		}

		private string MakeCommandGetQueueList(string Cmd, string AgentID)
		{
			string text = string.Concat(new string[]
			{
				"<request>\n\t<cmdType>",
				Cmd,
				"</cmdType>\n\t<agentID>",
				AgentID,
				"</agentID>\n\t</request>"
			});
			return this.MakeCommandHead(text.Length) + text;
		}

		private string MakeCommandGetIvrProfileList(string Cmd, string AgentID)
		{
			string text = string.Concat(new string[]
			{
				"<request>\n\t<cmdType>",
				Cmd,
				"</cmdType>\n\t<agentID>",
				AgentID,
				"</agentID>\n\t</request>"
			});
			return this.MakeCommandHead(text.Length) + text;
		}

		private string MakeCommandGetDefinedRoleAndRight(string Cmd, string AgentID)
		{
			string text = string.Concat(new string[]
			{
				"<request>\n\t<cmdType>",
				Cmd,
				"</cmdType>\n\t<agentID>",
				AgentID,
				"</agentID>\n\t</request>"
			});
			return this.MakeCommandHead(text.Length) + text;
		}

		private string MakeCommandGetAgentGroupList(string Cmd, string AgentID, string agentGroupRange)
		{
			string text = string.Concat(new string[]
			{
				"<request>\n\t<cmdType>",
				Cmd,
				"</cmdType>\n\t<agentID>",
				AgentID,
				"</agentID>\n\t<agentGroupRange>",
				agentGroupRange,
				"</agentGroupRange>\n\t</request>"
			});
			return this.MakeCommandHead(text.Length) + text;
		}

		private string MakeCommandGetAgentsOfQueue(string Cmd, string AgentID, string QueueNum)
		{
			string text = string.Concat(new string[]
			{
				"<request>\n\t<cmdType>",
				Cmd,
				"</cmdType>\n\t<agentID>",
				AgentID,
				"</agentID>\n\t<queueNum>",
				QueueNum,
				"</queueNum>\n\t</request>"
			});
			return this.MakeCommandHead(text.Length) + text;
		}

		private string MakeCommandGetAgentsOfAgentGroup(string Cmd, string AgentID, string AgentGroupNum)
		{
			string text = string.Concat(new string[]
			{
				"<request>\n\t<cmdType>",
				Cmd,
				"</cmdType>\n\t<agentID>",
				AgentID,
				"</agentID>\n\t<agentGroupNum>",
				AgentGroupNum,
				"</agentGroupNum>\n\t</request>"
			});
			return this.MakeCommandHead(text.Length) + text;
		}

		private string MakeCommandGetAgentsMonitorInfo(string Cmd, string AgentID, string AgentsStr)
		{
			string text = string.Concat(new string[]
			{
				"<request>\n\t<cmdType>",
				Cmd,
				"</cmdType>\n\t<agentID>",
				AgentID,
				"</agentID>\n\t<agentsStr>",
				AgentsStr,
				"</agentsStr>\n\t</request>"
			});
			return this.MakeCommandHead(text.Length) + text;
		}

		private string MakeCommandGetDetailCallInfo(string Cmd, string AgentID, string targetAgentNum)
		{
			string text = string.Concat(new string[]
			{
				"<request>\n\t<cmdType>",
				Cmd,
				"</cmdType>\n\t<agentID>",
				AgentID,
				"</agentID>\n\t<targetAgentNum>",
				targetAgentNum,
				"</targetAgentNum>\n\t</request>"
			});
			return this.MakeCommandHead(text.Length) + text;
		}

		private string MakeCommandGetCustomerOfQueue(string Cmd, string AgentID, string queueNumLstStr)
		{
			string text = string.Concat(new string[]
			{
				"<request>\n\t<cmdType>",
				Cmd,
				"</cmdType>\n\t<agentID>",
				AgentID,
				"</agentID>\n\t<queueNumLstStr>",
				queueNumLstStr,
				"</queueNumLstStr>\n\t</request>"
			});
			return this.MakeCommandHead(text.Length) + text;
		}

		private string MakeCommandGetCustomerOfMyQueue(string Cmd, string AgentID)
		{
			string text = string.Concat(new string[]
			{
				"<request>\n\t<cmdType>",
				Cmd,
				"</cmdType>\n\t<agentID>",
				AgentID,
				"</agentID>\n\t</request>"
			});
			return this.MakeCommandHead(text.Length) + text;
		}

		private string MakeCommandGetQueueStatis(string Cmd, string AgentID, string queueNumLstStr)
		{
			string text = string.Concat(new string[]
			{
				"<request>\n\t<cmdType>",
				Cmd,
				"</cmdType>\n\t<agentID>",
				AgentID,
				"</agentID>\n\t<queueNumLstStr>",
				queueNumLstStr,
				"</queueNumLstStr>\n\t</request>"
			});
			return this.MakeCommandHead(text.Length) + text;
		}

		private string MakeCommandGetAllQueueStatis(string Cmd, string AgentID)
		{
			string text = string.Concat(new string[]
			{
				"<request>\n\t<cmdType>",
				Cmd,
				"</cmdType>\n\t<agentID>",
				AgentID,
				"</agentID>\n\t</request>"
			});
			return this.MakeCommandHead(text.Length) + text;
		}

		private string MakeCommandForceHangup(string Cmd, string AgentID, string targetAgentNum)
		{
			string text = string.Concat(new string[]
			{
				"<request>\n\t<cmdType>",
				Cmd,
				"</cmdType>\n\t<agentID>",
				AgentID,
				"</agentID>\n\t<targetAgentNum>",
				targetAgentNum,
				"</targetAgentNum>\n\t</request>"
			});
			return this.MakeCommandHead(text.Length) + text;
		}

		private string MakeCommandLogin(string Cmd, string AgentID, string AgentPwd)
		{
			string text = string.Concat(new string[]
			{
				"<request>\n\t<cmdType>",
				Cmd,
				"</cmdType>\n\t<agentID>",
				AgentID,
				"</agentID>\n\t<agentPwd>",
				AgentPwd,
				"</agentPwd>\n</request>"
			});
			return this.MakeCommandHead(text.Length) + text;
		}

		private string MakeCommandGetAgentDefineStatus(string Cmd, string AgentID)
		{
			string text = string.Concat(new string[]
			{
				"<request>\n\t<cmdType>",
				Cmd,
				"</cmdType>\n\t<agentID>",
				AgentID,
				"</agentID>\n\t</request>"
			});
			return this.MakeCommandHead(text.Length) + text;
		}

		private string MakeCommandGetAgentWebSiteInfo(string Cmd, string AgentID)
		{
			string text = string.Concat(new string[]
			{
				"<request>\n\t<cmdType>",
				Cmd,
				"</cmdType>\n\t<agentID>",
				AgentID,
				"</agentID>\n\t</request>"
			});
			return this.MakeCommandHead(text.Length) + text;
		}

		private string MakeCommandSignIn(string Cmd, string AgentID, string AgentPwd, string AgentExten, string BindExten, string InitStatus, string ExtenIsOutbound)
		{
			string text = string.Concat(new string[]
			{
				"<request>\n\t<cmdType>",
				Cmd,
				"</cmdType>\n\t<agentID>",
				AgentID,
				"</agentID>\n\t<agentPwd>",
				AgentPwd,
				"</agentPwd>\n\t<bindExten>",
				BindExten,
				"</bindExten>\n\t<agentExten>",
				AgentExten,
				"</agentExten>\n\t<initStatus>",
				InitStatus,
				"</initStatus>\n<extenIsOutbound>",
				ExtenIsOutbound,
				"</extenIsOutbound>\n</request>"
			});
			return this.MakeCommandHead(text.Length) + text;
		}

		private string MakeCommandSignOut(string Cmd, string AgentID)
		{
			string text = string.Concat(new string[]
			{
				"<request>\n\t<cmdType>",
				Cmd,
				"</cmdType>\n\t<agentID>",
				AgentID,
				"</agentID>\n\t</request>"
			});
			return this.MakeCommandHead(text.Length) + text;
		}

		private string MakeCommandReload(string Cmd, string agentID, string reloadType)
		{
			string str = string.Concat(new string[]
			{
				"<request>\n\t<agentID>",
				agentID,
				"</agentID>\n\t<cmdType>",
				Cmd,
				"</cmdType>\n\t<reloadType>",
				reloadType,
				"</reloadType>\n\t</request>"
			});
			return this.MakeCommandHead(500) + str;
		}

		private string MakeCommandUpdateAgent(string Cmd, string agentID, string agentNum)
		{
			string text = string.Concat(new string[]
			{
				"<request>\n\t<agentID>",
				agentID,
				"</agentID>\n\t<cmdType>",
				Cmd,
				"</cmdType>\n\t<agentNum>",
				agentNum,
				"</agentNum>\n\t</request>"
			});
			return this.MakeCommandHead(text.Length) + text;
		}

		private string MakeCommandDelAgent(string Cmd, string agentID, string agentNum)
		{
			string text = string.Concat(new string[]
			{
				"<request>\n\t<agentID>",
				agentID,
				"</agentID>\n\t<cmdType>",
				Cmd,
				"</cmdType>\n\t<agentNum>",
				this.AgentID,
				"</agentNum>\n\t</request>"
			});
			return this.MakeCommandHead(text.Length) + text;
		}

		private string MakeCommandGetPersonalInfo(string Cmd, string agentID)
		{
			string text = string.Concat(new string[]
			{
				"<request>\n\t<agentID>",
				agentID,
				"</agentID>\n\t<cmdType>",
				Cmd,
				"</cmdType>\n\t</request>"
			});
			return this.MakeCommandHead(text.Length) + text;
		}

		private string MakeCommandSetPersonalInfo(string Cmd, string agentID, string strMobile, string strEmail)
		{
			string text = string.Concat(new string[]
			{
				"<request>\n\t<agentID>",
				agentID,
				"</agentID>\n\t<cmdType>",
				Cmd,
				"</cmdType>\n\t<agentMobile>",
				strMobile,
				"</agentMobile>\n\t<agentEmail>",
				strEmail,
				"</agentEmail>\n\t</request>"
			});
			return this.MakeCommandHead(text.Length) + text;
		}

		private string MakeCommandChangePswd(string Cmd, string agentID, string stOldPswd, string strNewPswd)
		{
			string text = string.Concat(new string[]
			{
				"<request>\n\t<agentID>",
				agentID,
				"</agentID>\n\t<cmdType>",
				Cmd,
				"</cmdType>\n\t<oldPswd>",
				stOldPswd,
				"</oldPswd>\n\t<newPswd>",
				strNewPswd,
				"</newPswd>\n\t</request>"
			});
			return this.MakeCommandHead(text.Length) + text;
		}

		private string MakeCommandGetReportStatisInfo(string Cmd, string agentID)
		{
			string text = string.Concat(new string[]
			{
				"<request>\n\t<agentID>",
				agentID,
				"</agentID>\n\t<cmdType>",
				Cmd,
				"</cmdType>\n\t</request>"
			});
			return this.MakeCommandHead(text.Length) + text;
		}

		private string MakeCommandApplyChangeStatus(string Cmd, string agentID, string targetStatus)
		{
			string text = string.Concat(new string[]
			{
				"<request>\n\t<agentID>",
				agentID,
				"</agentID>\n\t<cmdType>",
				Cmd,
				"</cmdType>\n\t<targetStatus>",
				targetStatus,
				"</targetStatus>\n\t</request>"
			});
			return this.MakeCommandHead(text.Length) + text;
		}

		private string MakeCommandApplyApproval(string Cmd, string agentID, string applyAgentID, string targetStatus, string passFlag)
		{
			string text = string.Concat(new string[]
			{
				"<request>\n\t<agentID>",
				agentID,
				"</agentID>\n\t<cmdType>",
				Cmd,
				"</cmdType>\n\t<applyAgentID>",
				applyAgentID,
				"</applyAgentID>\n\t<targetStatus>",
				targetStatus,
				"</targetStatus>\n\t<approveResult>",
				passFlag,
				"</approveResult>\n\t</request>"
			});
			return this.MakeCommandHead(text.Length) + text;
		}

		private string MakeCommandApplyCancel(string Cmd, string agentID, string targetStatus)
		{
			string text = string.Concat(new string[]
			{
				"<request>\n\t<agentID>",
				agentID,
				"</agentID>\n\t<cmdType>",
				Cmd,
				"</cmdType>\n\t<targetStatus>",
				targetStatus,
				"</targetStatus>\n\t</request>"
			});
			return this.MakeCommandHead(text.Length) + text;
		}

		private string MakeCommandGetChangeStatusApplyList(string Cmd, string agentID, string targetStatus)
		{
			string text = string.Concat(new string[]
			{
				"<request>\n\t<agentID>",
				agentID,
				"</agentID>\n\t<cmdType>",
				Cmd,
				"</cmdType>\n\t<targetStatus>",
				targetStatus,
				"</targetStatus>\n\t</request>"
			});
			return this.MakeCommandHead(text.Length) + text;
		}

		private string MakeCommandGetAgentGroupStatusMaxNum(string Cmd, string AgentID, string agentGroupNameLstStr, string agentGroupIdLstStr, string targetStatus)
		{
			string text = string.Concat(new string[]
			{
				"<request>\n\t<agentID>",
				AgentID,
				"</agentID>\n\t<cmdType>",
				Cmd,
				"</cmdType>\n\t<targetAgentGroupNameList>",
				agentGroupNameLstStr,
				"</targetAgentGroupNameList>\n\t<targetAgentGroupIdList>",
				agentGroupIdLstStr,
				"</targetAgentGroupIdList>\n\t<targetStatus>",
				targetStatus,
				"</targetStatus>\n\t</request>"
			});
			return this.MakeCommandHead(text.Length) + text;
		}

		private string MakeCommandSetAgentGroupStatusMaxNum(string Cmd, string AgentID, string agentGroupNameLstStr, string agentGroupIdLstStr, string targetStatus, string maxStatusNum)
		{
			string text = string.Concat(new string[]
			{
				"<request>\n\t<agentID>",
				AgentID,
				"</agentID>\n\t<cmdType>",
				Cmd,
				"</cmdType>\n\t<targetAgentGroupName>",
				agentGroupNameLstStr,
				"</targetAgentGroupName>\n\t<targetAgentGroupId>",
				agentGroupIdLstStr,
				"</targetAgentGroupId>\n\t<targetStatus>",
				targetStatus,
				"</targetStatus>\n\t<statusMaxNum>",
				maxStatusNum,
				"</statusMaxNum>\n\t</request>"
			});
			return this.MakeCommandHead(text.Length) + text;
		}

		private string MakeCommandHead(int xml_Len)
		{
			return "<<<length=" + xml_Len + ">>>";
		}

		private long PL_SendXML(string strXML)
		{
			this.log.Debug("enter PL_SendXML.strXML:\n" + strXML);
			long result;
			if (this.client == null || !this.client.Connected)
			{
				this.log.Debug("client is Null or client.Connected is false!");
				result = 206L;
			}
			else
			{
				try
				{
					long num = (long)clsSocket.SendData(this.client, strXML);
					if (num != 0L)
					{
						this.log.Debug("发送失败" + strXML);
					}
					else
					{
						this.blnResponsed = false;
						this.mIntCountResTime = 0;
					}
					long num2 = num;
					if (num2 <= 0L)
					{
						if (num2 >= -5L)
						{
							switch ((int)(num2 - -5L))
							{
							case 0:
								num = 205L;
								break;
							case 1:
								num = 203L;
								break;
							case 2:
								num = 1000L;
								break;
							case 3:
								num = 207L;
								break;
							case 4:
								num = 206L;
								break;
							case 5:
								num = 0L;
								break;
							}
						}
					}
					this.log.Debug("PL_SendXML is finished!ret=" + num);
					result = num;
				}
				catch (Exception ex)
				{
					this.log.Error(ex.Message);
					result = 1000L;
				}
			}
			return result;
		}

		private void ReceiveEvents()
		{
			this.log.Debug("enter ReceiveEvents.");
			while (!this.mIsStop_ThrReceive)
			{
				try
				{
					byte[] array = new byte[4096];
					if (this.client == null)
					{
						this.mIsAlive_ThrReceive = false;
						return;
					}
					this.mIsAlive_ThrReceive = true;
					int num = this.client.Receive(array, array.Length, SocketFlags.None);
					if (num <= 0)
					{
						this.log.Debug("my socket is disconnected !");
						this.add_event_to_queue(this.create_xml_event("socket_disconnect", "disconnect", "-1001"));
						break;
					}
					this.blnResponsed = true;
					this.mIntCountResTime = 0;
					if (num >= 4096)
					{
						this.event_buffer_arraylst.AddRange(array);
					}
					else if (this.event_buffer_arraylst.Count > 0)
					{
						this.event_buffer_arraylst.AddRange(array);
						byte[] bytes = this.event_buffer_arraylst.ToArray(typeof(byte)) as byte[];
						string @string = Encoding.UTF8.GetString(bytes);
						if (@string.IndexOf("\0") > 0)
						{
							num = @string.IndexOf("\0");
						}
						else
						{
							num = @string.Length;
						}
						this.log.Debug("接收到一个新事件：\n" + @string.Substring(0, num));
						this.add_event_to_queue(@string.Substring(0, num));
						this.event_buffer_arraylst.Clear();
					}
					else
					{
						string @string = Encoding.UTF8.GetString(array);
						if (@string.IndexOf("\0") > 0)
						{
							num = @string.IndexOf("\0");
						}
						else
						{
							num = @string.Length;
						}
						this.log.Debug("接收到一个新事件：\n" + @string.Substring(0, num));
						this.add_event_to_queue(@string.Substring(0, num));
					}
				}
				catch (SocketException ex)
				{
					this.log.Error("ReceiveEvents is throw SocketException ! reason:" + ex.Message);
					this.add_event_to_queue(this.create_xml_event("socket_exception", "socket Exception", "-1002"));
					break;
				}
				catch (Exception ex2)
				{
					this.log.Debug("ReceiveEvents is throw Exception ! reason:" + ex2.Message);
					this.add_event_to_queue(this.create_xml_event("exception", "ReceiveEvents throw Exception!", "-2000"));
					break;
				}
			}
			this.blnConnect = false;
			this.mIsAlive_ThrReceive = false;
		}

		private void ReceiveCallback(IAsyncResult ar)
		{
			try
			{
				StateObject stateObject = (StateObject)ar.AsyncState;
				Socket workSocket = stateObject.workSocket;
				int num = workSocket.EndReceive(ar);
				if (num > 0)
				{
					stateObject.sb.Append(Encoding.UTF8.GetString(stateObject.buffer, 0, num));
					this.blnResponsed = true;
					this.mIntCountResTime = 0;
					this.log.Debug("\n***************\n" + stateObject.sb.ToString(0, stateObject.sb.Length));
					this.add_event_to_queue(stateObject.sb.ToString(0, stateObject.sb.Length));
					workSocket.BeginReceive(stateObject.buffer, 0, 1024, SocketFlags.None, new AsyncCallback(this.ReceiveCallback), stateObject);
				}
				else if (stateObject.sb.Length > 1)
				{
					this.blnResponsed = true;
					this.mIntCountResTime = 0;
					this.log.Debug("\n***************\n" + stateObject.sb.ToString(0, stateObject.sb.Length));
					this.add_event_to_queue(stateObject.sb.ToString(0, stateObject.sb.Length));
				}
			}
			catch (Exception var_3_140)
			{
			}
		}

		private void add_event_to_queue(string strEvent)
		{
			this.log.Debug("enter add_event_to_queue.strEvent:\n" + strEvent + "-----------");
			if (!("" == strEvent))
			{
				string text = "";
				try
				{
					text = this.strQueueEventBuffer + strEvent;
					string text2;
					while (true)
					{
						text = this.GetXMLBody(text, this.LocalIpAddress, out text2);
						if (text == "")
						{
							break;
						}
						this.strQueueEventBuffer = "";
						this.log.Debug(string.Concat(new string[]
						{
							"xml event to queue prase success! Xml:",
							text,
							",leave_Xml:",
							text2,
							"-----------"
						}));
						bool flag = text.Contains("<cmdType>heartbeat</cmdType>");
						if (flag)
						{
							lock (this.QueueHeartbeatEvents)
							{
								this.QueueHeartbeatEvents.Enqueue(text);
							}
						}
						else
						{
							lock (this.QueueEvents)
							{
								this.QueueEvents.Enqueue(text);
							}
						}
						AgentEvent agentEvent = XmlControl.ReadOneXml(text);
						if ((agentEvent.eEventQualifier == EventQualifier.SignOut_Status && agentEvent.deAgentEventType == AgentEventType.AGENT_EVENT_EVENT) || (agentEvent.eEventQualifier == EventQualifier.Sys_KickOut && agentEvent.deAgentEventType == AgentEventType.AGENT_EVENT_EVENT))
						{
							text2 = "";
						}
						text = text2;
						if (this.sem.Release() > 1000)
						{
							Thread.Sleep(50);
						}
						if (this.semHeartBeat.Release() > 1000)
						{
							Thread.Sleep(50);
						}
						if (!(text != ""))
						{
							goto IL_1E6;
						}
					}
					this.strQueueEventBuffer = text2;
					IL_1E6:;
				}
				catch (Exception ex)
				{
					this.log.Error(string.Concat(new string[]
					{
						"prase xml is error!",
						text,
						ex.Source,
						",信息:",
						ex.Message,
						",堆栈:",
						ex.StackTrace
					}));
				}
			}
		}

		private string create_xml_event(string eventType, string reason, string retCode)
		{
			this.log.Debug(string.Concat(new string[]
			{
				"enter create_xml_event.eventType:",
				eventType,
				",reason:",
				reason,
				",retCode",
				retCode
			}));
			string text = "";
			if (eventType != null)
			{
				if (!(eventType == "socket_disconnect"))
				{
					if (!(eventType == "socket_exception"))
					{
						if (eventType == "exception")
						{
							text = this.create_xml_exception(reason, retCode);
						}
					}
					else
					{
						text = this.create_xml_socket_disconnect(reason, retCode);
					}
				}
				else
				{
					text = this.create_xml_socket_disconnect(reason, retCode);
				}
			}
			return string.Concat(new object[]
			{
				"<<<ip&port=127.0.0.1,length=",
				text.Length,
				">>>",
				text
			});
		}

		private string create_xml_by_map(string rootName, ref Dictionary<string, string> xmlMap)
		{
			this.log.Debug("enter create_xml_by_map.rootName:" + rootName);
			string text = "<" + rootName + ">\n";
			foreach (KeyValuePair<string, string> current in xmlMap)
			{
				text = string.Concat(new string[]
				{
					text,
					"\t<",
					current.Key,
					">",
					current.Value,
					"</",
					current.Key,
					">\n"
				});
			}
			text = text + "</" + rootName + ">\n";
			return text;
		}

		private string create_xml_signout(string agentnum, string reason, string retCode)
		{
			this.log.Debug(string.Concat(new string[]
			{
				"enter create_xml_signout.agentnum:",
				agentnum,
				",reason:",
				reason,
				",retCode",
				retCode
			}));
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			dictionary["cmdType"] = "signout";
			dictionary["agentID"] = agentnum;
			dictionary["reason"] = reason;
			dictionary["retCode"] = retCode;
			return this.create_xml_by_map("event", ref dictionary);
		}

		private string create_xml_response_timeout(string reason, string retCode)
		{
			this.log.Debug("enter create_xml_signout.reason:" + reason + ",retCode:" + retCode);
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			dictionary["cmdType"] = "response_timeout";
			dictionary["reason"] = reason;
			dictionary["retCode"] = retCode;
			return this.create_xml_by_map("define", ref dictionary);
		}

		private string create_xml_exception(string reason, string retCode)
		{
			this.log.Debug("enter create_xml_exception .reason:" + reason + ",retCode:" + retCode);
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			dictionary["cmdType"] = "exception";
			dictionary["exception_reason"] = reason;
			dictionary["exception_type"] = retCode;
			return this.create_xml_by_map("define", ref dictionary);
		}

		private string create_xml_socket_disconnect(string reason, string retCode)
		{
			this.log.Debug("enter create_xml_socket_disconnect .reason:" + reason + ",retCode:" + retCode);
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			dictionary["cmdType"] = "disconnect";
			dictionary["reason"] = reason;
			dictionary["retCode"] = retCode;
			return this.create_xml_by_map("define", ref dictionary);
		}

		private void SendMyEvent(AgentEvent myEvent)
		{
			this.log.Debug("enter SendMyEvent.");
			if (this.AgentEvents != null)
			{
				this.AgentEvents(myEvent);
			}
		}

		private void SendMyHeartbeatEvent(AgentEvent myEvent)
		{
			this.log.Debug("enter SendMyHeartbeatEvent.");
			if (this.AgentHeartbeatEvents != null)
			{
				this.AgentHeartbeatEvents(myEvent);
			}
		}

		private void HandleEvents()
		{
			this.log.Debug("enter HandleEvents.");
			string text = "";
			while (true)
			{
				try
				{
					this.sem.WaitOne();
					lock (this.QueueEvents)
					{
						if (this.QueueEvents.Count > 0)
						{
							text = this.QueueEvents.Dequeue();
						}
					}
					if (text != "")
					{
						this.RaiseEvents(text);
					}
					text = "";
				}
				catch (Exception ex)
				{
					this.log.Debug("HandleEvents error :" + ex.Message);
				}
			}
		}

		private void HandleHeartBeatEvents()
		{
			this.log.Debug("enter HandleHeartBeatEvents.");
			string text = "";
			while (true)
			{
				try
				{
					this.semHeartBeat.WaitOne();
					this.log.Debug("semHeartBeat.WaitOne success");
					lock (this.QueueHeartbeatEvents)
					{
						if (this.QueueHeartbeatEvents.Count > 0)
						{
							text = this.QueueHeartbeatEvents.Dequeue();
						}
					}
					if (text != "")
					{
						this.RaiseHeartbeatEvents(text);
					}
					text = "";
				}
				catch (Exception ex)
				{
					this.log.Debug("HandleHeartBeatEvents error :" + ex.Message);
				}
			}
		}

		private void RaiseEvents(object param)
		{
			string text = (string)param;
			this.log.Debug("enter RaiseEvents. strXml:" + text + "----------");
			AgentEvent myEvent = XmlControl.ReadOneXml(text);
			this.SendMyEvent(myEvent);
			this.log.Debug("SendMyEvent success!");
		}

		private void RaiseHeartbeatEvents(object param)
		{
			string text = (string)param;
			this.log.Debug("enter RaiseHeartbeatEvents. strXml:" + text + "----------");
			AgentEvent myEvent = XmlControl.ReadOneXml(text);
			this.SendMyHeartbeatEvent(myEvent);
			this.log.Debug("SendMyHeartbeatEvent success!");
		}

		public string GetXMLBody(string strXml, string strLocalIp, out string strRemain)
		{
			this.log.Debug("enter GetXMLBody.strXml:" + strXml + ",strLocalIp:" + strLocalIp);
			string result;
			try
			{
				strRemain = "";
				string someStr = this.GetSomeStr(strXml, "<<<", ">>>");
				if (someStr == "")
				{
					if (strXml != "")
					{
						strRemain += strXml;
					}
					result = "";
				}
				else
				{
					int num = strXml.IndexOf("<<<");
					if (num != 0)
					{
						strXml = this.mySubStr(strXml, num, AgentDll.get_str_len(strXml));
					}
					string someStr2 = this.GetSomeStr(someStr, "ip&port=", ",");
					int num2 = someStr.IndexOf("length=");
					int num3 = Convert.ToInt32(this.mySubStr(someStr, num2 + 7, AgentDll.get_str_len(someStr) - num2 - 7));
					int num4 = AgentDll.get_str_len(strXml) - AgentDll.get_str_len(someStr) - 6;
					if (num4 < num3)
					{
						strRemain += strXml;
						result = "";
					}
					else if (num4 == num3)
					{
						string text = strXml.Substring(AgentDll.get_str_len(someStr) + 6);
						strRemain = "";
						result = text;
					}
					else
					{
						string text = this.mySubStr(strXml, AgentDll.get_str_len(someStr) + 6, num3);
						strRemain = this.mySubStr(strXml, AgentDll.get_str_len(text) + AgentDll.get_str_len(someStr) + 6, AgentDll.get_str_len(strXml));
						result = text;
					}
				}
			}
			catch (Exception ex)
			{
				this.log.Error(string.Concat(new string[]
				{
					"GetXMLBody is error! strXml:",
					strXml,
					ex.Source,
					",信息:",
					ex.Message,
					",堆栈:",
					ex.StackTrace
				}));
				strRemain = "";
				result = "";
			}
			return result;
		}

		public static int get_str_len(string str)
		{
			byte[] bytes = Encoding.UTF8.GetBytes(str);
			return bytes.Length;
		}

		private string mySubStr(string a_SrcStr, int a_StartIndex, int a_Cnt)
		{
			byte[] bytes = Encoding.UTF8.GetBytes(a_SrcStr);
			string result;
			if (a_Cnt <= 0)
			{
				result = "";
			}
			else if (a_StartIndex + 1 > bytes.Length)
			{
				result = "";
			}
			else
			{
				if (a_StartIndex + a_Cnt > bytes.Length)
				{
					a_Cnt = bytes.Length - a_StartIndex;
				}
				result = Encoding.UTF8.GetString(bytes, a_StartIndex, a_Cnt);
			}
			return result;
		}

		private string GetSomeStr(string istr, string startString, string endString)
		{
			this.log.Debug("enter GetSomeStr.");
			string result;
			try
			{
				int num = istr.IndexOf(startString, 0);
				if (num < 0)
				{
					result = "";
				}
				else
				{
					num += startString.Length;
					int num2 = istr.IndexOf(endString, num);
					if (num < 0)
					{
						result = "";
					}
					else
					{
						string text = istr.Substring(num, num2 - num);
						result = text;
					}
				}
			}
			catch (Exception ex)
			{
				this.log.Error(string.Concat(new string[]
				{
					ex.Source,
					",信息:",
					ex.Message,
					",堆栈:",
					ex.StackTrace
				}));
				result = "";
			}
			return result;
		}

		public void PLCloseAll()
		{
			if (null != this.client)
			{
				this.client.Close();
				this.client = null;
			}
			this.blnConnect = false;
		}
	}
}
