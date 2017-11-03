using log4net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Net;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Forms;

namespace PLAgentBar
{
	public class FrmNoAnswerCalls : Form
	{
		public struct NoAnswerCallInfo
		{
			public string caller_num;

			public string caller_type;

			public string call_type;

			public string caller_action;

			public string ring_sec;

			public string start_time;
		}

		public delegate bool DoCallBackOuterEventHandler(string callBack_num, string access_num);

		public delegate bool DoCallBackInterEventHandler(string callBack_num);

		private const string DATE_TIME_FORMAT = "yyyy-MM-dd HH:mm:ss";

		public static ILog Log;

		private string m_agent_ID = string.Empty;

		private string m_agent_pswd = string.Empty;

		private string m_callBack_num = string.Empty;

		private string m_noAnswer_Calls_URL = string.Empty;

		private string m_start_time = string.Empty;

		private string m_end_time = string.Empty;

		private string m_caller_type = string.Empty;

		private string m_caller_agent_num = string.Empty;

		private string m_call_action = string.Empty;

		private int m_page_size = 20;

		private int m_total_num = 0;

		private int m_current_page = 0;

		private string m_strUrl = "http://{noAnswerCallsURL}/cti/getUnAnsweCallsByAgentID.action?User={agent_Id}&Password={pswd}&PageNum={page_num}&PageSize={page_size}&StartTime={start_time}&EndTime={end_time}&Order={Order}";

		private string m_agent_status = string.Empty;

		private IContainer components = null;

		private Label label1;

		private Label label2;

		private DateTimePicker dtp_end;

		private Button btn_searcch;

		private DateTimePicker dtp_start;

		private Button btn_downPage;

		private Button btn_upPage;

		private TextBox txt_pageInfo;

		private ListView lv_NoAnswerCalls;

		private Label lb_total_num;

		private Button btn_callBack;

        public event FrmNoAnswerCalls.DoCallBackOuterEventHandler DoCallBackOuterEvent;

        public event FrmNoAnswerCalls.DoCallBackInterEventHandler DoCallBackInterEvent;

		public string CallBack_num
		{
			get
			{
				return this.m_callBack_num;
			}
			set
			{
				this.m_callBack_num = value;
			}
		}

		public string Caller_type
		{
			get
			{
				return this.m_caller_type;
			}
			set
			{
				this.m_caller_type = value;
			}
		}

		public string Caller_agent_num
		{
			get
			{
				return this.m_caller_agent_num;
			}
			set
			{
				this.m_caller_agent_num = value;
			}
		}

		public string NoAnswer_Calls_URL
		{
			get
			{
				return this.m_noAnswer_Calls_URL;
			}
			set
			{
				this.m_noAnswer_Calls_URL = value;
			}
		}

		public string Agent_pswd
		{
			get
			{
				return this.m_agent_pswd;
			}
			set
			{
				this.m_agent_pswd = value;
			}
		}

		public int Page_size
		{
			get
			{
				return this.m_page_size;
			}
			set
			{
				this.m_page_size = value;
			}
		}

		public string Call_action
		{
			get
			{
				return this.m_call_action;
			}
			set
			{
				this.m_call_action = value;
			}
		}

		public string Agent_status
		{
			get
			{
				return this.m_agent_status;
			}
			set
			{
				this.m_agent_status = value;
			}
		}

		public FrmNoAnswerCalls(string agent_ID, string agent_pswd)
		{
			this.InitializeComponent();
			FrmNoAnswerCalls.Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
			this.m_agent_ID = agent_ID;
			this.m_agent_pswd = agent_pswd;
		}

		private void clean_member_var()
		{
			this.m_callBack_num = string.Empty;
			this.m_caller_type = string.Empty;
			this.m_caller_agent_num = string.Empty;
			this.m_call_action = string.Empty;
		}

		public void update_agent_status(string agent_num, string old_status, string new_status, bool is_bind_exten, string customer_enter_channel, string current_time, string start_talking_time)
		{
			if (agent_num == this.m_agent_ID)
			{
				this.m_agent_status = new_status;
			}
		}

		private string callType2String(string call_type)
		{
			string call_type_str = string.Empty;
			if (call_type != null)
			{
				if (call_type == "1")
				{
					call_type_str = "客户主动呼入";
					return call_type_str;
				}
				if (call_type == "2")
				{
					call_type_str = "预览/手动外呼";
					return call_type_str;
				}
				if (call_type == "3")
				{
					call_type_str = "预测式外呼";
					return call_type_str;
				}
				if (call_type == "4")
				{
					call_type_str = "坐席互拨";
					return call_type_str;
				}
				if (call_type == "5")
				{
					call_type_str = "环回测试";
					return call_type_str;
				}
			}
			call_type_str = "未知";
			return call_type_str;
		}

		private string callerType2String(string caller_type)
		{
			string caller_type_str = string.Empty;
			switch (caller_type)
			{
			case "1":
				caller_type_str = "坐席分机";
				return caller_type_str;
			case "2":
				caller_type_str = "队列";
				return caller_type_str;
			case "3":
				caller_type_str = "接入号码";
				return caller_type_str;
			case "4":
				caller_type_str = "IVR";
				return caller_type_str;
			case "5":
				caller_type_str = "客户号码";
				return caller_type_str;
			case "6":
				caller_type_str = "外线号码";
				return caller_type_str;
			case "7":
				caller_type_str = "坐席工号";
				return caller_type_str;
			}
			caller_type_str = "未知";
			return caller_type_str;
		}

		private string result2String(string result)
		{
			string resultMsg = string.Empty;
			if (result != null)
			{
				if (result == "0")
				{
					resultMsg = "无数据！";
					return resultMsg;
				}
				if (result == "-1")
				{
					resultMsg = "获取数据失败，入参的参数不合法！ ";
					return resultMsg;
				}
				if (result == "-2")
				{
					resultMsg = "获取数据失败，密码错误 ";
					return resultMsg;
				}
				if (result == "-3")
				{
					resultMsg = "获取数据失败，系统错误";
					return resultMsg;
				}
				if (result == "-4")
				{
					resultMsg = "获取数据失败，URL错误";
					return resultMsg;
				}
			}
			resultMsg = "获取数据失败，发生未知错误";
			return resultMsg;
		}

		private bool check_Is_reCallable(string caller_type, string action)
		{
			FrmNoAnswerCalls.Log.Debug("enter " + MethodBase.GetCurrentMethod().Name);
			return "1" == caller_type || "3" == caller_type || "5" == caller_type || "6" == caller_type || "7" == caller_type;
		}

		private bool check_agent_status_is_reCallable(string agent_status)
		{
			return AgentBar.Str2AgentStatus(agent_status) != AgentBar.Agent_Status.AGENT_STATUS_OFFLINE && AgentBar.Str2AgentStatus(agent_status) != AgentBar.Agent_Status.AGENT_STATUS_RING && AgentBar.Str2AgentStatus(agent_status) != AgentBar.Agent_Status.AGENT_STATUS_TALKING && AgentBar.Str2AgentStatus(agent_status) != AgentBar.Agent_Status.AGENT_STATUS_HOLD && AgentBar.Str2AgentStatus(agent_status) != AgentBar.Agent_Status.AGENT_STATUS_CAMP_ON && AgentBar.Str2AgentStatus(agent_status) != AgentBar.Agent_Status.AGENT_STATUS_MUTE && AgentBar.Str2AgentStatus(agent_status) != AgentBar.Agent_Status.AGENT_STATUS_CALLING_OUT;
		}

		public int SetHttpServerInfo(string noAnswerCallsURL)
		{
			FrmNoAnswerCalls.Log.Debug("enter " + MethodBase.GetCurrentMethod().Name);
			int result;
			if (string.IsNullOrEmpty(noAnswerCallsURL))
			{
				result = -1;
			}
			else
			{
				this.m_noAnswer_Calls_URL = noAnswerCallsURL;
				result = 0;
			}
			return result;
		}

		private void initListViewColumns()
		{
			FrmNoAnswerCalls.Log.Debug("enter " + MethodBase.GetCurrentMethod().Name);
			this.lv_NoAnswerCalls.FullRowSelect = true;
			this.lv_NoAnswerCalls.Columns.Add("序号", 40);
			this.lv_NoAnswerCalls.Columns.Add("来电号码(工号)", 100);
			this.lv_NoAnswerCalls.Columns.Add("号码类型", 90);
			this.lv_NoAnswerCalls.Columns.Add("呼叫类型", 90);
			this.lv_NoAnswerCalls.Columns.Add("振铃时长", 110);
			this.lv_NoAnswerCalls.Columns.Add("来电时间", 140);
		}

		private string setHttpUrlParam(string m_noAnswer_Calls_URL, string agent_Id, string pswd, string page_num, string page_size, string start_time, string end_time)
		{
			FrmNoAnswerCalls.Log.Debug("enter " + MethodBase.GetCurrentMethod().Name);
			string strUrl = this.m_strUrl;
			string md5_str = ComFunc.make_md5_str(pswd) + DateTime.Now.ToString("yyyyMMdd");
			strUrl = strUrl.Replace("{noAnswerCallsURL}", m_noAnswer_Calls_URL);
			strUrl = strUrl.Replace("{agent_Id}", agent_Id);
			strUrl = strUrl.Replace("{pswd}", ComFunc.make_md5_str(md5_str));
			strUrl = strUrl.Replace("{page_num}", page_num);
			strUrl = strUrl.Replace("{page_size}", page_size);
			strUrl = strUrl.Replace("{start_time}", start_time);
			strUrl = strUrl.Replace("{end_time}", end_time);
			return strUrl.Replace("{Order}", "desc");
		}

		public string HttpGet(string page_num, string start_time, string end_time)
		{
			FrmNoAnswerCalls.Log.Debug("enter " + MethodBase.GetCurrentMethod().Name);
			HttpWebResponse response = null;
			Stream myResponseStream = null;
			StreamReader myStreamReader = null;
			string retString = "-1";
			string strUrl = string.Empty;
			try
			{
				strUrl = this.setHttpUrlParam(this.m_noAnswer_Calls_URL, this.m_agent_ID, this.m_agent_pswd, page_num, this.m_page_size.ToString(), start_time, end_time);
				HttpWebRequest request = (HttpWebRequest)WebRequest.Create(strUrl);
				request.Method = "GET";
				request.ContentType = "text/html;charset=UTF-8";
				response = (HttpWebResponse)request.GetResponse();
				if (null != response)
				{
					myResponseStream = response.GetResponseStream();
					if (null != myResponseStream)
					{
						myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
						if (null != myStreamReader)
						{
							retString = myStreamReader.ReadToEnd();
						}
					}
				}
			}
			catch (Exception ex_D7)
			{
			}
			finally
			{
				if (null != myStreamReader)
				{
					myStreamReader.Close();
				}
				if (null != myResponseStream)
				{
					myResponseStream.Close();
				}
				if (null != response)
				{
					response.Close();
				}
			}
			return retString;
		}

		private void do_fill_data(JArray calls_info_jarr, int current_page, int page_size)
		{
			FrmNoAnswerCalls.Log.Debug("enter " + MethodBase.GetCurrentMethod().Name);
			if (null != calls_info_jarr)
			{
				try
				{
					if (calls_info_jarr.Count != 0 && 0 != this.lv_NoAnswerCalls.Items.Count)
					{
						this.lv_NoAnswerCalls.Items.Clear();
					}
					for (int i = 0; i < calls_info_jarr.Count; i++)
					{
						ListViewItem temItem = new ListViewItem((i + 1 + (current_page - 1) * page_size).ToString());
						if ("1" == calls_info_jarr[i]["caller_type"].ToString())
						{
							temItem.SubItems.Add(calls_info_jarr[i]["caller_agent_num"].ToString());
						}
						else
						{
							temItem.SubItems.Add(calls_info_jarr[i]["caller_num"].ToString());
						}
						temItem.SubItems[1].Tag = calls_info_jarr[i]["caller_agent_num"].ToString();
						temItem.SubItems.Add(this.callerType2String(calls_info_jarr[i]["caller_type"].ToString()));
						temItem.SubItems[2].Tag = calls_info_jarr[i]["caller_type"].ToString();
						temItem.SubItems.Add(this.callType2String(calls_info_jarr[i]["call_type"].ToString()));
						temItem.SubItems[3].Tag = calls_info_jarr[i]["action"].ToString();
						temItem.SubItems.Add(calls_info_jarr[i]["ring_sec"].ToString() + "秒");
						temItem.SubItems[4].Tag = Convert.ToInt32(calls_info_jarr[i]["ring_sec"].ToString());
						temItem.SubItems.Add(calls_info_jarr[i]["start_time"].ToString());
						this.lv_NoAnswerCalls.Items.Add(temItem);
					}
				}
				catch (Exception ex_26F)
				{
					MessageBox.Show("加载数据异常！");
				}
			}
		}

		private void get_NoAnswer_calls(int page_num, string start_time, string end_time)
		{
			FrmNoAnswerCalls.Log.Debug("enter " + MethodBase.GetCurrentMethod().Name);
			string noAnswerCallJsonstr = string.Empty;
			string result = string.Empty;
			string total_num = string.Empty;
			string current_page = string.Empty;
			string page_size = string.Empty;
			string rows = string.Empty;
			try
			{
				noAnswerCallJsonstr = this.HttpGet(page_num.ToString(), start_time, end_time);
				if ("-1" == noAnswerCallJsonstr)
				{
					MessageBox.Show("获取数据失败!");
				}
				else
				{
					JObject jobj = (JObject)JsonConvert.DeserializeObject(noAnswerCallJsonstr);
					if (null != jobj)
					{
						result = jobj["result"].ToString();
						if ("1" != result)
						{
							MessageBox.Show(this.result2String(result));
							if ("0" == result)
							{
								this.txt_pageInfo.Text = "";
								this.lb_total_num.Text = "共0条数据";
								this.m_total_num = 0;
								this.m_current_page = 0;
								if (this.lv_NoAnswerCalls.Items.Count > 0)
								{
									this.lv_NoAnswerCalls.Items.Clear();
								}
							}
						}
						else
						{
							total_num = jobj["total_num"].ToString();
							current_page = jobj["current_page"].ToString();
							page_size = jobj["PageSize"].ToString();
							rows = jobj["rows"].ToString();
							this.m_total_num = Convert.ToInt32(total_num);
							this.m_current_page = Convert.ToInt32(current_page);
							int total_page = this.m_total_num / this.m_page_size + ((this.m_total_num % this.m_page_size == 0) ? 0 : 1);
							this.txt_pageInfo.Text = current_page + "/" + total_page.ToString();
							this.lb_total_num.Text = "共" + total_num + "条数据";
							JArray calls_info_jarr = (JArray)JsonConvert.DeserializeObject(rows);
							this.do_fill_data(calls_info_jarr, this.m_current_page, this.m_page_size);
						}
					}
				}
			}
			catch (Exception ex_235)
			{
				MessageBox.Show("获取数据失败,请确认获取未接来电信息的服务器IP和端口号是否正确!");
			}
		}

		private void FrmNoAnswerCalls_Load(object sender, EventArgs e)
		{
			FrmNoAnswerCalls.Log.Debug("enter " + MethodBase.GetCurrentMethod().Name);
			this.dtp_start.CustomFormat = "yyyy-MM-dd HH:mm:ss";
			this.dtp_start.Format = DateTimePickerFormat.Custom;
			this.dtp_start.ShowUpDown = true;
			this.dtp_end.CustomFormat = "yyyy-MM-dd HH:mm:ss";
			this.dtp_end.Format = DateTimePickerFormat.Custom;
			this.dtp_end.ShowUpDown = true;
			this.dtp_start.Focus();
			this.initListViewColumns();
			this.dtp_start.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
			this.m_start_time = this.dtp_start.Value.ToString("yyyy-MM-dd HH:mm:ss");
			this.m_end_time = this.dtp_end.Value.ToString("yyyy-MM-dd HH:mm:ss");
			this.get_NoAnswer_calls(1, this.m_start_time, this.m_end_time);
		}

		private void lv_NoAnswerCalls_SelectedIndexChanged(object sender, EventArgs e)
		{
			FrmNoAnswerCalls.Log.Debug("enter " + MethodBase.GetCurrentMethod().Name);
			if (0 != this.lv_NoAnswerCalls.SelectedItems.Count)
			{
				this.m_callBack_num = this.lv_NoAnswerCalls.SelectedItems[0].SubItems[1].Text;
				this.m_caller_type = this.lv_NoAnswerCalls.SelectedItems[0].SubItems[2].Tag.ToString();
				this.m_call_action = this.lv_NoAnswerCalls.SelectedItems[0].SubItems[3].Tag.ToString();
				if ("1" == this.m_caller_type && null != this.lv_NoAnswerCalls.SelectedItems[0].SubItems[1].Tag)
				{
					this.m_caller_agent_num = this.lv_NoAnswerCalls.SelectedItems[0].SubItems[1].Tag.ToString();
				}
			}
		}

		private void btn_searcch_Click(object sender, EventArgs e)
		{
			FrmNoAnswerCalls.Log.Debug("enter " + MethodBase.GetCurrentMethod().Name);
			if (this.dtp_start.Value.CompareTo(this.dtp_end.Value) > 0)
			{
				MessageBox.Show("开始时间大于结束时间！");
			}
			else
			{
				this.m_start_time = this.dtp_start.Value.ToString("yyyy-MM-dd HH:mm:ss");
				this.m_end_time = this.dtp_end.Value.ToString("yyyy-MM-dd HH:mm:ss");
				this.get_NoAnswer_calls(1, this.m_start_time, this.m_end_time);
				this.clean_member_var();
			}
		}

		private void btn_callBack_Click(object sender, EventArgs e)
		{
			FrmNoAnswerCalls.Log.Debug("enter " + MethodBase.GetCurrentMethod().Name);
			if (string.IsNullOrEmpty(this.m_callBack_num))
			{
				MessageBox.Show("请选择需要回拨的号码！");
			}
			else if (!this.check_agent_status_is_reCallable(this.m_agent_status))
			{
				MessageBox.Show("坐席状态错误，无法进行回拨！");
			}
			else if (!this.check_Is_reCallable(this.m_caller_type, this.m_call_action))
			{
				MessageBox.Show("该号码类型无法回拨！");
			}
			else
			{
				if ("1" == this.m_caller_type || "7" == this.m_caller_type)
				{
					if (null != this.DoCallBackInterEvent)
					{
						this.DoCallBackInterEvent(this.m_callBack_num);
					}
				}
				else if ("3" == this.m_caller_type || "5" == this.m_caller_type || "6" == this.m_caller_type)
				{
					if (null != this.DoCallBackOuterEvent)
					{
						this.DoCallBackOuterEvent(this.m_callBack_num, "");
					}
				}
				this.clean_member_var();
			}
		}

		private void btn_upPage_Click(object sender, EventArgs e)
		{
			FrmNoAnswerCalls.Log.Debug("enter " + MethodBase.GetCurrentMethod().Name);
			if (this.m_current_page > 1)
			{
				this.get_NoAnswer_calls(this.m_current_page - 1, this.m_start_time, this.m_end_time);
			}
			this.clean_member_var();
		}

		private void btn_downPage_Click(object sender, EventArgs e)
		{
			FrmNoAnswerCalls.Log.Debug("enter " + MethodBase.GetCurrentMethod().Name);
			int total_page = this.m_total_num / this.m_page_size + ((this.m_total_num % this.m_page_size == 0) ? 0 : 1);
			if (this.m_current_page < total_page)
			{
				this.get_NoAnswer_calls(this.m_current_page + 1, this.m_start_time, this.m_end_time);
			}
			this.clean_member_var();
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		private void InitializeComponent()
		{
			ComponentResourceManager resources = new ComponentResourceManager(typeof(FrmNoAnswerCalls));
			this.label1 = new Label();
			this.label2 = new Label();
			this.dtp_end = new DateTimePicker();
			this.btn_searcch = new Button();
			this.dtp_start = new DateTimePicker();
			this.btn_downPage = new Button();
			this.btn_upPage = new Button();
			this.txt_pageInfo = new TextBox();
			this.lv_NoAnswerCalls = new ListView();
			this.lb_total_num = new Label();
			this.btn_callBack = new Button();
			base.SuspendLayout();
			this.label1.AutoSize = true;
			this.label1.Location = new Point(12, 23);
			this.label1.Name = "label1";
			this.label1.Size = new Size(53, 12);
			this.label1.TabIndex = 2;
			this.label1.Text = "开始时间";
			this.label2.AutoSize = true;
			this.label2.Location = new Point(230, 23);
			this.label2.Name = "label2";
			this.label2.Size = new Size(53, 12);
			this.label2.TabIndex = 3;
			this.label2.Text = "结束时间";
			this.dtp_end.Location = new Point(285, 19);
			this.dtp_end.Name = "dtp_end";
			this.dtp_end.Size = new Size(150, 21);
			this.dtp_end.TabIndex = 2;
			this.btn_searcch.Location = new Point(449, 17);
			this.btn_searcch.Name = "btn_searcch";
			this.btn_searcch.Size = new Size(58, 23);
			this.btn_searcch.TabIndex = 3;
			this.btn_searcch.Text = "检索";
			this.btn_searcch.UseVisualStyleBackColor = true;
			this.btn_searcch.Click += new EventHandler(this.btn_searcch_Click);
			this.dtp_start.Location = new Point(74, 19);
			this.dtp_start.Name = "dtp_start";
			this.dtp_start.Size = new Size(150, 21);
			this.dtp_start.TabIndex = 1;
			this.btn_downPage.Location = new Point(94, 423);
			this.btn_downPage.Name = "btn_downPage";
			this.btn_downPage.Size = new Size(25, 20);
			this.btn_downPage.TabIndex = 8;
			this.btn_downPage.Text = ">";
			this.btn_downPage.UseVisualStyleBackColor = true;
			this.btn_downPage.Click += new EventHandler(this.btn_downPage_Click);
			this.btn_upPage.Location = new Point(12, 423);
			this.btn_upPage.Name = "btn_upPage";
			this.btn_upPage.Size = new Size(25, 20);
			this.btn_upPage.TabIndex = 7;
			this.btn_upPage.Text = "<";
			this.btn_upPage.UseVisualStyleBackColor = true;
			this.btn_upPage.Click += new EventHandler(this.btn_upPage_Click);
			this.txt_pageInfo.Enabled = false;
			this.txt_pageInfo.Location = new Point(48, 422);
			this.txt_pageInfo.Name = "txt_pageInfo";
			this.txt_pageInfo.Size = new Size(37, 21);
			this.txt_pageInfo.TabIndex = 6;
			this.lv_NoAnswerCalls.Location = new Point(2, 59);
			this.lv_NoAnswerCalls.Name = "lv_NoAnswerCalls";
			this.lv_NoAnswerCalls.Size = new Size(600, 350);
			this.lv_NoAnswerCalls.TabIndex = 5;
			this.lv_NoAnswerCalls.UseCompatibleStateImageBehavior = false;
			this.lv_NoAnswerCalls.View = View.Details;
			this.lv_NoAnswerCalls.SelectedIndexChanged += new EventHandler(this.lv_NoAnswerCalls_SelectedIndexChanged);
			this.lb_total_num.AutoSize = true;
			this.lb_total_num.Location = new Point(132, 427);
			this.lb_total_num.Name = "lb_total_num";
			this.lb_total_num.Size = new Size(59, 12);
			this.lb_total_num.TabIndex = 9;
			this.lb_total_num.Text = "共0条记录";
			this.btn_callBack.Location = new Point(522, 17);
			this.btn_callBack.Name = "btn_callBack";
			this.btn_callBack.Size = new Size(58, 23);
			this.btn_callBack.TabIndex = 4;
			this.btn_callBack.Text = "回拨";
			this.btn_callBack.UseVisualStyleBackColor = true;
			this.btn_callBack.Click += new EventHandler(this.btn_callBack_Click);
			base.AutoScaleDimensions = new SizeF(6f, 12f);
			base.AutoScaleMode = AutoScaleMode.Font;
			base.ClientSize = new Size(607, 452);
			base.Controls.Add(this.btn_callBack);
			base.Controls.Add(this.lb_total_num);
			base.Controls.Add(this.btn_downPage);
			base.Controls.Add(this.btn_upPage);
			base.Controls.Add(this.txt_pageInfo);
			base.Controls.Add(this.lv_NoAnswerCalls);
			base.Controls.Add(this.dtp_start);
			base.Controls.Add(this.btn_searcch);
			base.Controls.Add(this.dtp_end);
			base.Controls.Add(this.label2);
			base.Controls.Add(this.label1);
			base.FormBorderStyle = FormBorderStyle.FixedSingle;
			base.Icon = (Icon)resources.GetObject("$this.Icon");
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "FrmNoAnswerCalls";
			base.StartPosition = FormStartPosition.CenterScreen;
			this.Text = "未接来电";
			base.Load += new EventHandler(this.FrmNoAnswerCalls_Load);
			base.ResumeLayout(false);
			base.PerformLayout();
		}
	}
}
