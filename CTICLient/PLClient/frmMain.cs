using log4net;
using PLAgentBar;
using PLAgentDll;
using PLClient.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using UserCtrlSplitContainer;

namespace PLClient
{
	public class frmMain : Form
	{
		private delegate void EvtSignInDelegate(string AgentID, bool blnSuccess, string strReason);

		private delegate void DelegateShowTip(object myObjec);

		private delegate void BeginInvokeDelegate();

		private delegate void InvokeDelegate();

		public delegate void AgentStatusHotKeySettingEventHandler(string hotKeyIdle, string hotKeyBusy, string hotKeyLeave, string hotKeyCallOut, string hotKeyMonitor);

		public enum falshType : uint
		{
			FLASHW_STOP,
			FALSHW_CAPTION,
			FLASHW_TRAY,
			FLASHW_ALL,
			FLASHW_PARAM1,
			FLASHW_PARAM2 = 12u,
			FLASHW_TIMER = 6u,
			FLASHW_TIMERNOFG = 14u
		}

		private enum EnumLoginResult
		{
			LoginProcess,
			LoginSuccess,
			LoginFail
		}

		private enum EnumPhoneType
		{
			InternalPhone,
			ExternalPhone,
			NoPhone,
			ControlOtherPhone
		}

		private struct ShowTipInfoStruct
		{
			public string info;

			public bool autoClose;

			public int sleepTimeOut;

			public bool btnVisible;

			public string openBtnName;
		}

		private const uint MAX_ACWTIMEOUT = 3600u;

		private IContainer components = null;

		private ContextMenuStrip cmsWB;

		private ToolStripMenuItem 新建ToolStripMenuItem;

		private ToolStripMenuItem tsmi_close;

		private MenuStrip msMenu;

		private ToolStripMenuItem 文件ToolStripMenuItem;

		private ToolStripMenuItem 关于ToolStripMenuItem;

		private ToolStripMenuItem tsmiLogoff;

		private ToolStripMenuItem tsmiExit;

		private ToolStripMenuItem 帮助ToolStripMenuItem;

		private ToolStripMenuItem 关于ToolStripMenuItem1;

		private StatusStrip statusStrip1;

		private ToolStripStatusLabel tsslAgentID;

		private SplitContainer splitContainer1;

		private SplitContainer splitContainer3;

		private StatusStrip ssWBstatus;

		private ToolStripStatusLabel tsbWBstatus;

		private ToolStripProgressBar tspWebProcess;

		private ToolStrip toolStrip1;

		private ToolStripButton tsbAdd;

		private ToolStripButton tsbGoback;

		private ToolStripButton tsbGoForward;

		private ToolStripButton tsbRefresh;

		private ToolStripComboBox tscURL;

		private ToolStripButton tsbSearch;

		public AgentBar agentBar1;

		private ImageList imageList1;

		private ListView lvwAgent;

		private ListView lvwQueue;

		private SplitContainerEx splitContainer2;

		private System.Windows.Forms.Timer tmrCheckLogin;

		private ImageList imgLstMonitor;

		private ContextMenuStrip cmsMonitor;

		private ToolStripMenuItem mnuListen;

		private ToolStripMenuItem mnuInterrupt;

		private ToolStripMenuItem mnuForceDisconnect;

		private ToolStripMenuItem mnuIntercept;

		private ToolStripStatusLabel tsslAgentName;

		private ToolStripStatusLabel tsslAgentExten;

		private ToolStripStatusLabel tsslRole;

		private ToolStripStatusLabel tsslGroup;

		private ToolStripMenuItem tsmTool;

		private ToolStripMenuItem tsmiSoftPhone;

		private TabControl tabControl1;

		private TabPage tabPage1;

		private TabPage tabPage3;

		private WebBrowser wb;

		private TabPage add;

		private ToolStripMenuItem tsmWebsite;

		private ToolStripSeparator toolStripSeparator1;

		private ToolStripMenuItem tsmi_close_other;

		private ToolStripMenuItem tsmi_close_all;

		private System.Windows.Forms.Timer tmrChkACWTimeOut;

		private ToolStripMenuItem toolStripMenuItem1;

		private ToolStripMenuItem tsmConfig;

		private ToolStripMenuItem tsmi_system;

		private Label lblTelNumber;

		private Label lblAccessNum;

		private ToolStripStatusLabel tsslQueueStatis;

		private Panel palMarquee;

		private System.Windows.Forms.Timer tmrMarquee;

		private Label lblMarquee;

		private NotifyIcon notifyIcon1;

		private ContextMenuStrip cmsNotify;

		private ToolStripMenuItem tsmiLogout;

		private ToolStripMenuItem tsmiClose;

		private ToolStripMenuItem tsmi_Personal;

		private PictureBox picDown;

		private ToolStripMenuItem tsmi_controls;

		public bool BLN_TCPCONNECTED = false;

		private bool IsSearch = false;

		public byte[] Sendbytes = new byte[1024];

		private frmMain.EnumLoginResult LoginResult;

		private volatile bool blnLoginSuccess;

		private static ILog Log;

		private Point pt;

		private FrmTip newFrmTip;

		private FrmSysInfo newFrmSysTip;

		private FrmSysInfo newFrmQueueInfoTip;

		private frmPersonalConfig newPersonalConfig;

		private Point startPoint;

		private int intCount;

		private FrmLogin newLoginForm;

		private volatile bool blnIsSignOut = true;

		private bool blnIsFirstLogin = true;

		private volatile bool IsLoging = false;

		private volatile bool IsExitWithNoPrompt = false;

		private bool IsFirstRun = false;

		private volatile bool IsResigning = false;

		private volatile bool IsManualLogOff = false;

		private bool IsAlertedForEditInfo;

		private Dictionary<string, string> agent_website_dic;

		private List<string> callHistoryLst;

		private int ACWTimeCount = 0;

		private string g_username;

		private string g_agentname;

		private string g_password;

		private string g_exten;

		private string g_exten_password;

		private string g_caller_id;

		private string g_called_id;

		private string g_access_num_name;

		private string g_group_id;

		private string g_group_name;

		private string g_role_name;

		private string g_task_id;

		private string g_queue_num;

		private string g_call_type;

		private string g_area_id;

		private string g_area_country = "";

		private string g_area_province = "";

		private string g_area_city = "";

		private string g_area_name = "";

		private string g_salt_key;

		private string g_cust_grade;

		private ProcessStartInfo process_info;

		private int padL = 20;

		private int padR = 20;

		private System.Threading.Timer queueWaitTime;

		private System.Windows.Forms.Timer notifyTimer = new System.Windows.Forms.Timer();

		private Icon ico_notify;

		private Icon ico_blank;

		private static bool Is_Blink = false;

		private static string marqueeStr = "";

		private static FormWindowState LastWindowsState;

		private bool urlStretch = false;

		private bool isRememberPwd = false;

		private string mDataFilePath;

		private static Dictionary<string, List<Customer_Info_Struct>> g_my_customer_of_queue;

		private frmMain.EnumPhoneType mBindPhoneType;

		private DateTime keyPressTime;

		public bool SignInSuccess = false;

		public frmLoginConfig newLoginConfig = new frmLoginConfig();

		public frmControlsConfig newControlsConfig = new frmControlsConfig();

		public AgentBar.ControlsInfo controlsinfoTemp;

		public event frmMain.AgentStatusHotKeySettingEventHandler AgentStatusHotKeySettingEvent
		{
			[MethodImpl(MethodImplOptions.Synchronized)]
			add
			{
				this.AgentStatusHotKeySettingEvent = (frmMain.AgentStatusHotKeySettingEventHandler)Delegate.Combine(this.AgentStatusHotKeySettingEvent, value);
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			remove
			{
				this.AgentStatusHotKeySettingEvent = (frmMain.AgentStatusHotKeySettingEventHandler)Delegate.Remove(this.AgentStatusHotKeySettingEvent, value);
			}
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
			this.components = new Container();
			ComponentResourceManager resources = new ComponentResourceManager(typeof(frmMain));
			this.cmsWB = new ContextMenuStrip(this.components);
			this.新建ToolStripMenuItem = new ToolStripMenuItem();
			this.tsmi_close = new ToolStripMenuItem();
			this.tsmi_close_other = new ToolStripMenuItem();
			this.tsmi_close_all = new ToolStripMenuItem();
			this.msMenu = new MenuStrip();
			this.文件ToolStripMenuItem = new ToolStripMenuItem();
			this.tsmiLogoff = new ToolStripMenuItem();
			this.tsmiExit = new ToolStripMenuItem();
			this.tsmWebsite = new ToolStripMenuItem();
			this.tsmTool = new ToolStripMenuItem();
			this.tsmiSoftPhone = new ToolStripMenuItem();
			this.tsmConfig = new ToolStripMenuItem();
			this.tsmi_system = new ToolStripMenuItem();
			this.tsmi_Personal = new ToolStripMenuItem();
			this.tsmi_controls = new ToolStripMenuItem();
			this.关于ToolStripMenuItem = new ToolStripMenuItem();
			this.帮助ToolStripMenuItem = new ToolStripMenuItem();
			this.关于ToolStripMenuItem1 = new ToolStripMenuItem();
			this.toolStripSeparator1 = new ToolStripSeparator();
			this.statusStrip1 = new StatusStrip();
			this.tsslAgentID = new ToolStripStatusLabel();
			this.tsslAgentName = new ToolStripStatusLabel();
			this.tsslAgentExten = new ToolStripStatusLabel();
			this.tsslRole = new ToolStripStatusLabel();
			this.tsslGroup = new ToolStripStatusLabel();
			this.tsslQueueStatis = new ToolStripStatusLabel();
			this.imageList1 = new ImageList(this.components);
			this.imgLstMonitor = new ImageList(this.components);
			this.tmrCheckLogin = new System.Windows.Forms.Timer(this.components);
			this.cmsMonitor = new ContextMenuStrip(this.components);
			this.mnuListen = new ToolStripMenuItem();
			this.mnuInterrupt = new ToolStripMenuItem();
			this.mnuForceDisconnect = new ToolStripMenuItem();
			this.mnuIntercept = new ToolStripMenuItem();
			this.splitContainer1 = new SplitContainer();
			this.picDown = new PictureBox();
			this.lblAccessNum = new Label();
			this.lblTelNumber = new Label();
			this.toolStrip1 = new ToolStrip();
			this.tsbAdd = new ToolStripButton();
			this.tsbGoback = new ToolStripButton();
			this.tsbGoForward = new ToolStripButton();
			this.tsbRefresh = new ToolStripButton();
			this.tsbSearch = new ToolStripButton();
			this.tscURL = new ToolStripComboBox();
			this.agentBar1 = new AgentBar();
			this.splitContainer2 = new SplitContainerEx();
			this.splitContainer3 = new SplitContainer();
			this.lvwAgent = new ListView();
			this.lvwQueue = new ListView();
			this.tabControl1 = new TabControl();
			this.tabPage1 = new TabPage();
			this.wb = new WebBrowser();
			this.add = new TabPage();
			this.ssWBstatus = new StatusStrip();
			this.tsbWBstatus = new ToolStripStatusLabel();
			this.tspWebProcess = new ToolStripProgressBar();
			this.tmrChkACWTimeOut = new System.Windows.Forms.Timer(this.components);
			this.toolStripMenuItem1 = new ToolStripMenuItem();
			this.tmrMarquee = new System.Windows.Forms.Timer(this.components);
			this.palMarquee = new Panel();
			this.lblMarquee = new Label();
			this.notifyIcon1 = new NotifyIcon(this.components);
			this.cmsNotify = new ContextMenuStrip(this.components);
			this.tsmiLogout = new ToolStripMenuItem();
			this.tsmiClose = new ToolStripMenuItem();
			this.cmsWB.SuspendLayout();
			this.msMenu.SuspendLayout();
			this.statusStrip1.SuspendLayout();
			this.cmsMonitor.SuspendLayout();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			((ISupportInitialize)this.picDown).BeginInit();
			this.toolStrip1.SuspendLayout();
			this.splitContainer2.Panel1.SuspendLayout();
			this.splitContainer2.Panel2.SuspendLayout();
			this.splitContainer2.SuspendLayout();
			this.splitContainer3.Panel1.SuspendLayout();
			this.splitContainer3.Panel2.SuspendLayout();
			this.splitContainer3.SuspendLayout();
			this.tabControl1.SuspendLayout();
			this.tabPage1.SuspendLayout();
			this.ssWBstatus.SuspendLayout();
			this.palMarquee.SuspendLayout();
			this.cmsNotify.SuspendLayout();
			base.SuspendLayout();
			this.cmsWB.Items.AddRange(new ToolStripItem[]
			{
				this.新建ToolStripMenuItem,
				this.tsmi_close,
				this.tsmi_close_other,
				this.tsmi_close_all
			});
			this.cmsWB.Name = "cmsWB";
			this.cmsWB.Size = new Size(221, 92);
			this.新建ToolStripMenuItem.Name = "新建ToolStripMenuItem";
			this.新建ToolStripMenuItem.Size = new Size(220, 22);
			this.新建ToolStripMenuItem.Text = "新建";
			this.新建ToolStripMenuItem.Click += new EventHandler(this.NewToolStripMenuItem_Click);
			this.tsmi_close.Name = "tsmi_close";
			this.tsmi_close.Size = new Size(220, 22);
			this.tsmi_close.Text = "关闭当前页";
			this.tsmi_close.Click += new EventHandler(this.CloseToolStripMenuItem_Click);
			this.tsmi_close_other.Name = "tsmi_close_other";
			this.tsmi_close_other.Size = new Size(220, 22);
			this.tsmi_close_other.Text = "关闭除当前页外其他所有页";
			this.tsmi_close_other.Click += new EventHandler(this.tsmi_close_other_Click);
			this.tsmi_close_all.Name = "tsmi_close_all";
			this.tsmi_close_all.Size = new Size(220, 22);
			this.tsmi_close_all.Text = "关闭所有页";
			this.tsmi_close_all.Click += new EventHandler(this.tsmi_close_all_Click);
			this.msMenu.Items.AddRange(new ToolStripItem[]
			{
				this.文件ToolStripMenuItem,
				this.tsmWebsite,
				this.tsmTool,
				this.tsmConfig,
				this.关于ToolStripMenuItem
			});
			this.msMenu.Location = new Point(0, 0);
			this.msMenu.Name = "msMenu";
			this.msMenu.Size = new Size(1380, 25);
			this.msMenu.TabIndex = 7;
			this.msMenu.Text = "menuStrip1";
			this.文件ToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[]
			{
				this.tsmiLogoff,
				this.tsmiExit
			});
			this.文件ToolStripMenuItem.Name = "文件ToolStripMenuItem";
			this.文件ToolStripMenuItem.Size = new Size(58, 21);
			this.文件ToolStripMenuItem.Text = "文件(&F)";
			this.tsmiLogoff.Name = "tsmiLogoff";
			this.tsmiLogoff.Size = new Size(100, 22);
			this.tsmiLogoff.Text = "注销";
			this.tsmiLogoff.Click += new EventHandler(this.tsmiLogoff_Click);
			this.tsmiExit.Name = "tsmiExit";
			this.tsmiExit.Size = new Size(100, 22);
			this.tsmiExit.Text = "退出";
			this.tsmiExit.Click += new EventHandler(this.tsmiExit_Click);
			this.tsmWebsite.Name = "tsmWebsite";
			this.tsmWebsite.Size = new Size(56, 21);
			this.tsmWebsite.Text = "收藏夹";
			this.tsmWebsite.Visible = false;
			this.tsmWebsite.DropDownItemClicked += new ToolStripItemClickedEventHandler(this.tsmWebsite_DropDownItemClicked);
			this.tsmTool.DropDownItems.AddRange(new ToolStripItem[]
			{
				this.tsmiSoftPhone
			});
			this.tsmTool.Name = "tsmTool";
			this.tsmTool.Size = new Size(44, 21);
			this.tsmTool.Text = "工具";
			this.tsmTool.Visible = false;
			this.tsmiSoftPhone.Name = "tsmiSoftPhone";
			this.tsmiSoftPhone.Size = new Size(112, 22);
			this.tsmiSoftPhone.Text = "软电话";
			this.tsmiSoftPhone.Click += new EventHandler(this.tsmiSoftPhone_Click);
			this.tsmConfig.DropDownItems.AddRange(new ToolStripItem[]
			{
				this.tsmi_system,
				this.tsmi_Personal,
				this.tsmi_controls
			});
			this.tsmConfig.Name = "tsmConfig";
			this.tsmConfig.Size = new Size(44, 21);
			this.tsmConfig.Text = "设置";
			this.tsmi_system.Name = "tsmi_system";
			this.tsmi_system.Size = new Size(124, 22);
			this.tsmi_system.Text = "系统配置";
			this.tsmi_system.Click += new EventHandler(this.tsmi_system_Click);
			this.tsmi_Personal.Name = "tsmi_Personal";
			this.tsmi_Personal.Size = new Size(124, 22);
			this.tsmi_Personal.Text = "个人设置";
			this.tsmi_Personal.Click += new EventHandler(this.tsmi_Personal_Click);
			this.tsmi_controls.Name = "tsmi_controls";
			this.tsmi_controls.Size = new Size(124, 22);
			this.tsmi_controls.Text = "控件设置";
			this.tsmi_controls.Click += new EventHandler(this.tsmi_controls_Click);
			this.关于ToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[]
			{
				this.帮助ToolStripMenuItem,
				this.关于ToolStripMenuItem1,
				this.toolStripSeparator1
			});
			this.关于ToolStripMenuItem.Name = "关于ToolStripMenuItem";
			this.关于ToolStripMenuItem.Size = new Size(61, 21);
			this.关于ToolStripMenuItem.Text = "帮助(&H)";
			this.帮助ToolStripMenuItem.Name = "帮助ToolStripMenuItem";
			this.帮助ToolStripMenuItem.Size = new Size(100, 22);
			this.帮助ToolStripMenuItem.Text = "帮助";
			this.帮助ToolStripMenuItem.Visible = false;
			this.关于ToolStripMenuItem1.Name = "关于ToolStripMenuItem1";
			this.关于ToolStripMenuItem1.Size = new Size(100, 22);
			this.关于ToolStripMenuItem1.Text = "关于";
			this.关于ToolStripMenuItem1.Click += new EventHandler(this.关于ToolStripMenuItem1_Click);
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new Size(97, 6);
			this.statusStrip1.Items.AddRange(new ToolStripItem[]
			{
				this.tsslAgentID,
				this.tsslAgentName,
				this.tsslAgentExten,
				this.tsslRole,
				this.tsslGroup,
				this.tsslQueueStatis
			});
			this.statusStrip1.Location = new Point(0, 652);
			this.statusStrip1.Name = "statusStrip1";
			this.statusStrip1.Size = new Size(1380, 22);
			this.statusStrip1.TabIndex = 9;
			this.statusStrip1.Text = "statusStrip1";
			this.tsslAgentID.Name = "tsslAgentID";
			this.tsslAgentID.Padding = new Padding(5, 0, 5, 0);
			this.tsslAgentID.Size = new Size(84, 17);
			this.tsslAgentID.Text = "tsslAgentID";
			this.tsslAgentName.Name = "tsslAgentName";
			this.tsslAgentName.Padding = new Padding(20, 0, 20, 0);
			this.tsslAgentName.Size = new Size(136, 17);
			this.tsslAgentName.Text = "tsslAgentName";
			this.tsslAgentExten.Name = "tsslAgentExten";
			this.tsslAgentExten.Padding = new Padding(20, 0, 20, 0);
			this.tsslAgentExten.Size = new Size(132, 17);
			this.tsslAgentExten.Text = "tsslAgentExten";
			this.tsslRole.Name = "tsslRole";
			this.tsslRole.Padding = new Padding(20, 0, 20, 0);
			this.tsslRole.Size = new Size(93, 17);
			this.tsslRole.Text = "tsslRole";
			this.tsslGroup.Name = "tsslGroup";
			this.tsslGroup.Padding = new Padding(20, 0, 20, 0);
			this.tsslGroup.Size = new Size(104, 17);
			this.tsslGroup.Text = "tsslGroup";
			this.tsslQueueStatis.Margin = new Padding(630, 3, 0, 2);
			this.tsslQueueStatis.Name = "tsslQueueStatis";
			this.tsslQueueStatis.Overflow = ToolStripItemOverflow.Never;
			this.tsslQueueStatis.Size = new Size(149, 17);
			this.tsslQueueStatis.Text = "队列:1001 最大等待人数:3";
			this.tsslQueueStatis.TextAlign = ContentAlignment.MiddleLeft;
			this.imageList1.ImageStream = (ImageListStreamer)resources.GetObject("imageList1.ImageStream");
			this.imageList1.TransparentColor = Color.Transparent;
			this.imageList1.Images.SetKeyName(0, "NetByte Design Studio - 1162.png");
			this.imageList1.Images.SetKeyName(1, "NetByte Design Studio - 0156.png");
			this.imageList1.Images.SetKeyName(2, "NetByte Design Studio - 0167.png");
			this.imageList1.Images.SetKeyName(3, "0026.ico");
			this.imgLstMonitor.ImageStream = (ImageListStreamer)resources.GetObject("imgLstMonitor.ImageStream");
			this.imgLstMonitor.TransparentColor = Color.Transparent;
			this.imgLstMonitor.Images.SetKeyName(0, "hold");
			this.imgLstMonitor.Images.SetKeyName(1, "talk");
			this.imgLstMonitor.Images.SetKeyName(2, "ring");
			this.imgLstMonitor.Images.SetKeyName(3, "offline");
			this.tmrCheckLogin.Tick += new EventHandler(this.WaitForCtiCallBack);
			this.cmsMonitor.Items.AddRange(new ToolStripItem[]
			{
				this.mnuListen,
				this.mnuInterrupt,
				this.mnuForceDisconnect,
				this.mnuIntercept
			});
			this.cmsMonitor.Name = "cmsMonitor";
			this.cmsMonitor.Size = new Size(101, 92);
			this.mnuListen.Name = "mnuListen";
			this.mnuListen.Size = new Size(100, 22);
			this.mnuListen.Text = "监听";
			this.mnuListen.Click += new EventHandler(this.mnuListen_Click);
			this.mnuInterrupt.Name = "mnuInterrupt";
			this.mnuInterrupt.Size = new Size(100, 22);
			this.mnuInterrupt.Text = "插话";
			this.mnuInterrupt.Click += new EventHandler(this.mnuInterrupt_Click);
			this.mnuForceDisconnect.Name = "mnuForceDisconnect";
			this.mnuForceDisconnect.Size = new Size(100, 22);
			this.mnuForceDisconnect.Text = "强拆";
			this.mnuForceDisconnect.Click += new EventHandler(this.mnuForceDisconnect_Click);
			this.mnuIntercept.Name = "mnuIntercept";
			this.mnuIntercept.Size = new Size(100, 22);
			this.mnuIntercept.Text = "拦截";
			this.mnuIntercept.Click += new EventHandler(this.mnuIntercept_Click);
			this.splitContainer1.Dock = DockStyle.Fill;
			this.splitContainer1.FixedPanel = FixedPanel.Panel1;
			this.splitContainer1.IsSplitterFixed = true;
			this.splitContainer1.Location = new Point(0, 25);
			this.splitContainer1.Name = "splitContainer1";
			this.splitContainer1.Orientation = Orientation.Horizontal;
			this.splitContainer1.Panel1.Controls.Add(this.picDown);
			this.splitContainer1.Panel1.Controls.Add(this.lblAccessNum);
			this.splitContainer1.Panel1.Controls.Add(this.lblTelNumber);
			this.splitContainer1.Panel1.Controls.Add(this.toolStrip1);
			this.splitContainer1.Panel1.Controls.Add(this.agentBar1);
			this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
			this.splitContainer1.Size = new Size(1380, 627);
			this.splitContainer1.SplitterDistance = 86;
			this.splitContainer1.TabIndex = 10;
			this.picDown.BackColor = Color.Transparent;
			this.picDown.Image = (Image)resources.GetObject("picDown.Image");
			this.picDown.Location = new Point(1364, 26);
			this.picDown.Name = "picDown";
			this.picDown.Size = new Size(16, 16);
			this.picDown.SizeMode = PictureBoxSizeMode.AutoSize;
			this.picDown.TabIndex = 13;
			this.picDown.TabStop = false;
			this.picDown.Click += new EventHandler(this.picDown_Click);
			this.lblAccessNum.AutoSize = true;
			this.lblAccessNum.Font = new Font("宋体", 9f, FontStyle.Bold, GraphicsUnit.Point, 134);
			this.lblAccessNum.Location = new Point(1178, 55);
			this.lblAccessNum.Name = "lblAccessNum";
			this.lblAccessNum.Size = new Size(77, 12);
			this.lblAccessNum.TabIndex = 12;
			this.lblAccessNum.Text = "接入号名称:";
			this.lblAccessNum.Visible = false;
			this.lblTelNumber.AutoSize = true;
			this.lblTelNumber.Font = new Font("宋体", 9f, FontStyle.Bold, GraphicsUnit.Point, 134);
			this.lblTelNumber.Location = new Point(835, 55);
			this.lblTelNumber.Name = "lblTelNumber";
			this.lblTelNumber.Size = new Size(64, 12);
			this.lblTelNumber.TabIndex = 11;
			this.lblTelNumber.Text = "电话号码:";
			this.lblTelNumber.Visible = false;
			this.toolStrip1.AutoSize = false;
			this.toolStrip1.BackColor = SystemColors.Control;
			this.toolStrip1.BackgroundImageLayout = ImageLayout.None;
			this.toolStrip1.Dock = DockStyle.None;
			this.toolStrip1.GripStyle = ToolStripGripStyle.Hidden;
			this.toolStrip1.ImageScalingSize = new Size(32, 32);
			this.toolStrip1.Items.AddRange(new ToolStripItem[]
			{
				this.tsbAdd,
				this.tsbGoback,
				this.tsbGoForward,
				this.tsbRefresh,
				this.tsbSearch,
				this.tscURL
			});
			this.toolStrip1.Location = new Point(5, 45);
			this.toolStrip1.Name = "toolStrip1";
			this.toolStrip1.Padding = new Padding(0, 1, 1, 0);
			this.toolStrip1.Size = new Size(1437, 40);
			this.toolStrip1.TabIndex = 5;
			this.toolStrip1.TabStop = true;
			this.toolStrip1.Text = "toolStrip1";
			this.tsbAdd.DisplayStyle = ToolStripItemDisplayStyle.Image;
			this.tsbAdd.Image = (Image)resources.GetObject("tsbAdd.Image");
			this.tsbAdd.ImageTransparentColor = Color.Magenta;
			this.tsbAdd.Name = "tsbAdd";
			this.tsbAdd.Size = new Size(36, 36);
			this.tsbAdd.Text = "新建";
			this.tsbAdd.Visible = false;
			this.tsbAdd.Click += new EventHandler(this.tsbAdd_Click);
			this.tsbGoback.DisplayStyle = ToolStripItemDisplayStyle.Image;
			this.tsbGoback.Image = Resources.back;
			this.tsbGoback.ImageTransparentColor = Color.Magenta;
			this.tsbGoback.Name = "tsbGoback";
			this.tsbGoback.Size = new Size(36, 36);
			this.tsbGoback.Text = "后退";
			this.tsbGoback.Click += new EventHandler(this.tsbGoback_Click);
			this.tsbGoForward.DisplayStyle = ToolStripItemDisplayStyle.Image;
			this.tsbGoForward.Image = Resources.forward;
			this.tsbGoForward.ImageTransparentColor = Color.Magenta;
			this.tsbGoForward.Name = "tsbGoForward";
			this.tsbGoForward.Size = new Size(36, 36);
			this.tsbGoForward.Text = "前进";
			this.tsbGoForward.Click += new EventHandler(this.tsbGoForward_Click);
			this.tsbRefresh.DisplayStyle = ToolStripItemDisplayStyle.Image;
			this.tsbRefresh.Image = Resources.refresh;
			this.tsbRefresh.ImageTransparentColor = Color.Magenta;
			this.tsbRefresh.Name = "tsbRefresh";
			this.tsbRefresh.Size = new Size(36, 36);
			this.tsbRefresh.Text = "刷新";
			this.tsbRefresh.Click += new EventHandler(this.tsbRefresh_Click);
			this.tsbSearch.Alignment = ToolStripItemAlignment.Right;
			this.tsbSearch.DisplayStyle = ToolStripItemDisplayStyle.Image;
			this.tsbSearch.Image = (Image)resources.GetObject("tsbSearch.Image");
			this.tsbSearch.ImageTransparentColor = Color.Magenta;
			this.tsbSearch.Name = "tsbSearch";
			this.tsbSearch.Size = new Size(36, 36);
			this.tsbSearch.Text = "转到";
			this.tsbSearch.Visible = false;
			this.tsbSearch.Click += new EventHandler(this.tsbSearch_Click);
			this.tscURL.DropDownHeight = 500;
			this.tscURL.FlatStyle = FlatStyle.Standard;
			this.tscURL.Font = new Font("微软雅黑", 12f, FontStyle.Regular, GraphicsUnit.Point, 134);
			this.tscURL.IntegralHeight = false;
			this.tscURL.Items.AddRange(new object[]
			{
				"http://www.polylink.net",
				"http://www.baidu.com",
				"http://yahoo.com.cn",
				"http://www.sohu.com"
			});
			this.tscURL.Margin = new Padding(1, 2, 1, 0);
			this.tscURL.Name = "tscURL";
			this.tscURL.Overflow = ToolStripItemOverflow.Never;
			this.tscURL.Size = new Size(710, 37);
			this.tscURL.SelectedIndexChanged += new EventHandler(this.tscURL_SelectedIndexChanged);
			this.tscURL.KeyDown += new KeyEventHandler(this.tscURL_KeyDown);
			this.tscURL.DropDown += new EventHandler(this.tscURL_DropDown);
			this.tscURL.KeyPress += new KeyPressEventHandler(this.tscURL_KeyPress);
			this.agentBar1.AgentExten = null;
			this.agentBar1.AgentID = null;
			this.agentBar1.AgentName = null;
			this.agentBar1.AgentPwd = null;
			this.agentBar1.AgentState = AgentBar.Agent_State.AGENT_STATUS_UNKNOWN;
			this.agentBar1.AgentStateAfterHangup = AgentBar.Agent_Status.AGENT_STATUS_ACW;
			this.agentBar1.AgentStatus = null;
			this.agentBar1.ApplyChangeStatusApprovalHistory = (List<Apply_Change_Status>)resources.GetObject("agentBar1.ApplyChangeStatusApprovalHistory");
			this.agentBar1.AutoSize = true;
			this.agentBar1.BackColor = Color.Transparent;
			this.agentBar1.BindExten = false;
			this.agentBar1.CalloutHistory = (List<string>)resources.GetObject("agentBar1.CalloutHistory");
			this.agentBar1.CuID = "";
			this.agentBar1.DefaultAccessNum = null;
			this.agentBar1.Dock = DockStyle.Top;
			this.agentBar1.EavesDropAgentNum = null;
			this.agentBar1.ExtenIsOutbound = false;
			this.agentBar1.ExtenMode = null;
			this.agentBar1.GradeSwitch = false;
			this.agentBar1.HeartBeatTimeout = 30;
			this.agentBar1.HotKeyBusy = null;
			this.agentBar1.hotKeyCallOut = null;
			this.agentBar1.HotKeyIdle = null;
			this.agentBar1.HotKeyLeave = null;
			this.agentBar1.hotKeyMonitor = null;
			this.agentBar1.IsMonitorOfflineAgent = false;
			this.agentBar1.Location = new Point(0, 0);
			this.agentBar1.Name = "agentBar1";
			this.agentBar1.NoAnswerCallsURL = "";
			this.agentBar1.RefreshReportStatisInterval = 60;
			this.agentBar1.SaltKey = null;
			this.agentBar1.ServerIP = null;
			this.agentBar1.ServerPort = 0;
			this.agentBar1.SigninIntervalAfterSoftPhoneRegisted = 3000;
			this.agentBar1.SipAutoAnswer = false;
			this.agentBar1.SipAutoSignIn = false;
			this.agentBar1.SipLocalNum = null;
			this.agentBar1.SipNum = null;
			this.agentBar1.SipPhoneOnLineWarning = false;
			this.agentBar1.SipPort = 0;
			this.agentBar1.SipPwd = null;
			this.agentBar1.SipRegistTime = 0;
			this.agentBar1.SipServer = null;
			this.agentBar1.Size = new Size(1380, 39);
			this.agentBar1.SoftPhone_Dtmf_0_Cmd = 200;
			this.agentBar1.SoftPhone_dtmf_1_cmd = 201;
			this.agentBar1.SoftPhone_dtmf_2_cmd = 202;
			this.agentBar1.SoftPhone_dtmf_3_cmd = 203;
			this.agentBar1.SoftPhone_dtmf_4_cmd = 204;
			this.agentBar1.SoftPhone_dtmf_5_cmd = 205;
			this.agentBar1.SoftPhone_dtmf_6_cmd = 206;
			this.agentBar1.SoftPhone_dtmf_7_cmd = 207;
			this.agentBar1.SoftPhone_dtmf_8_cmd = 208;
			this.agentBar1.SoftPhone_dtmf_9_cmd = 209;
			this.agentBar1.SoftPhone_dtmf_pound_cmd = 211;
			this.agentBar1.SoftPhone_dtmf_star_cmd = 210;
			this.agentBar1.SoftPhoneAnswerCmd = 80;
			this.agentBar1.SoftPhoneAppClassName = "Qt5QwindowIcon";
			this.agentBar1.SoftPhoneAppName = "视频通话";
			this.agentBar1.SoftPhoneAppProcessName = "VideoTelephone";
			this.agentBar1.SoftphoneAutoAnswer = false;
			this.agentBar1.SoftPhoneEnable = false;
			this.agentBar1.SoftPhoneEnable2 = false;
			this.agentBar1.SoftPhoneLogoffCmd = 90;
			this.agentBar1.SoftPhoneMsgValue = 256;
			this.agentBar1.TabIndex = 14;
			this.agentBar1.WebUrl = null;
			this.splitContainer2.BackColor = SystemColors.Control;
			this.splitContainer2.BorderStyle = BorderStyle.Fixed3D;
			this.splitContainer2.CollpasePanel = SplitContainerEx.SplitterPanelEnum.Panel2;
			this.splitContainer2.Cursor = Cursors.Default;
			this.splitContainer2.Dock = DockStyle.Fill;
			this.splitContainer2.IsSplitterFixed = true;
			this.splitContainer2.Location = new Point(0, 0);
			this.splitContainer2.Name = "splitContainer2";
			this.splitContainer2.Panel1.Controls.Add(this.splitContainer3);
			this.splitContainer2.Panel1MinSize = 0;
			this.splitContainer2.Panel2.Controls.Add(this.tabControl1);
			this.splitContainer2.Panel2.Controls.Add(this.ssWBstatus);
			this.splitContainer2.Panel2MinSize = 140;
			this.splitContainer2.Size = new Size(1380, 537);
			this.splitContainer2.SplitterDistance = 0;
			this.splitContainer2.SplitterIncrement = 10;
			this.splitContainer2.SplitterWidth = 9;
			this.splitContainer2.TabIndex = 9;
			this.splitContainer3.BorderStyle = BorderStyle.Fixed3D;
			this.splitContainer3.Dock = DockStyle.Fill;
			this.splitContainer3.Location = new Point(0, 0);
			this.splitContainer3.Name = "splitContainer3";
			this.splitContainer3.Orientation = Orientation.Horizontal;
			this.splitContainer3.Panel1.Controls.Add(this.lvwAgent);
			this.splitContainer3.Panel2.Controls.Add(this.lvwQueue);
			this.splitContainer3.Size = new Size(0, 537);
			this.splitContainer3.SplitterDistance = 260;
			this.splitContainer3.TabIndex = 0;
			this.lvwAgent.Alignment = ListViewAlignment.Left;
			this.lvwAgent.AllowColumnReorder = true;
			this.lvwAgent.Dock = DockStyle.Fill;
			this.lvwAgent.FullRowSelect = true;
			this.lvwAgent.GridLines = true;
			this.lvwAgent.LargeImageList = this.imgLstMonitor;
			this.lvwAgent.Location = new Point(0, 0);
			this.lvwAgent.MultiSelect = false;
			this.lvwAgent.Name = "lvwAgent";
			this.lvwAgent.Size = new Size(0, 260);
			this.lvwAgent.SmallImageList = this.imgLstMonitor;
			this.lvwAgent.StateImageList = this.imgLstMonitor;
			this.lvwAgent.TabIndex = 0;
			this.lvwAgent.UseCompatibleStateImageBehavior = false;
			this.lvwAgent.View = View.Details;
			this.lvwAgent.MouseUp += new MouseEventHandler(this.lvwAgent_MouseUp);
			this.lvwQueue.AllowColumnReorder = true;
			this.lvwQueue.Dock = DockStyle.Fill;
			this.lvwQueue.FullRowSelect = true;
			this.lvwQueue.GridLines = true;
			this.lvwQueue.Location = new Point(0, 0);
			this.lvwQueue.MultiSelect = false;
			this.lvwQueue.Name = "lvwQueue";
			this.lvwQueue.Size = new Size(0, 273);
			this.lvwQueue.TabIndex = 0;
			this.lvwQueue.UseCompatibleStateImageBehavior = false;
			this.lvwQueue.View = View.Details;
			this.tabControl1.Controls.Add(this.tabPage1);
			this.tabControl1.Controls.Add(this.add);
			this.tabControl1.Dock = DockStyle.Fill;
			this.tabControl1.ImageList = this.imageList1;
			this.tabControl1.ItemSize = new Size(100, 28);
			this.tabControl1.Location = new Point(0, 0);
			this.tabControl1.Multiline = true;
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.Padding = new Point(3, 3);
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new Size(1367, 511);
			this.tabControl1.TabIndex = 0;
			this.tabControl1.MouseDoubleClick += new MouseEventHandler(this.tabControl1_MouseDoubleClick);
			this.tabControl1.MouseUp += new MouseEventHandler(this.tabControl1_MouseUp);
			this.tabControl1.SelectedIndexChanged += new EventHandler(this.tabControl1_SelectedIndexChanged);
			this.tabPage1.Controls.Add(this.wb);
			this.tabPage1.Location = new Point(4, 32);
			this.tabPage1.Name = "tabPage1";
			this.tabPage1.Padding = new Padding(3);
			this.tabPage1.Size = new Size(1359, 475);
			this.tabPage1.TabIndex = 0;
			this.tabPage1.Text = "新标签页";
			this.tabPage1.UseVisualStyleBackColor = true;
			this.wb.Dock = DockStyle.Fill;
			this.wb.Location = new Point(3, 3);
			this.wb.MinimumSize = new Size(20, 20);
			this.wb.Name = "wb";
			this.wb.ScriptErrorsSuppressed = true;
			this.wb.Size = new Size(1353, 469);
			this.wb.TabIndex = 0;
			this.wb.ProgressChanged += new WebBrowserProgressChangedEventHandler(this.wb_ProgressChanged);
			this.wb.Navigating += new WebBrowserNavigatingEventHandler(this.wb_Navigating);
			this.wb.NewWindow += new CancelEventHandler(this.wb_NewWindow);
			this.wb.PreviewKeyDown += new PreviewKeyDownEventHandler(this.wb_PreviewKeyDown);
			this.wb.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(this.wb_DocumentCompleted);
			this.wb.Navigated += new WebBrowserNavigatedEventHandler(this.wb_Navigated);
			this.add.Font = new Font("宋体", 9f, FontStyle.Regular, GraphicsUnit.Point, 134);
			this.add.ImageIndex = 3;
			this.add.Location = new Point(4, 32);
			this.add.Name = "add";
			this.add.Padding = new Padding(10, 5, 5, 5);
			this.add.Size = new Size(1359, 475);
			this.add.TabIndex = 1;
			this.add.Tag = "add";
			this.add.ToolTipText = "添加选项卡";
			this.add.UseVisualStyleBackColor = true;
			this.ssWBstatus.Items.AddRange(new ToolStripItem[]
			{
				this.tsbWBstatus,
				this.tspWebProcess
			});
			this.ssWBstatus.Location = new Point(0, 511);
			this.ssWBstatus.Name = "ssWBstatus";
			this.ssWBstatus.Size = new Size(1367, 22);
			this.ssWBstatus.SizingGrip = false;
			this.ssWBstatus.TabIndex = 5;
			this.tsbWBstatus.AutoSize = false;
			this.tsbWBstatus.Name = "tsbWBstatus";
			this.tsbWBstatus.Size = new Size(493, 17);
			this.tsbWBstatus.Text = "就绪                                            ";
			this.tsbWBstatus.TextAlign = ContentAlignment.MiddleLeft;
			this.tspWebProcess.Name = "tspWebProcess";
			this.tspWebProcess.Size = new Size(200, 16);
			this.tspWebProcess.Visible = false;
			this.tmrChkACWTimeOut.Interval = 1000;
			this.tmrChkACWTimeOut.Tick += new EventHandler(this.tmrChkACWTimeOut_Tick);
			this.toolStripMenuItem1.Name = "toolStripMenuItem1";
			this.toolStripMenuItem1.Size = new Size(94, 21);
			this.tmrMarquee.Interval = 200;
			this.tmrMarquee.Tick += new EventHandler(this.tmrMarquee_Tick);
			this.palMarquee.Controls.Add(this.lblMarquee);
			this.palMarquee.Location = new Point(543, 655);
			this.palMarquee.Name = "palMarquee";
			this.palMarquee.Size = new Size(258, 15);
			this.palMarquee.TabIndex = 1;
			this.lblMarquee.AutoSize = true;
			this.lblMarquee.Font = new Font("宋体", 9f, FontStyle.Bold, GraphicsUnit.Point, 134);
			this.lblMarquee.Location = new Point(6, 2);
			this.lblMarquee.Name = "lblMarquee";
			this.lblMarquee.Size = new Size(75, 12);
			this.lblMarquee.TabIndex = 2;
			this.lblMarquee.Text = "palMarquee";
			this.notifyIcon1.BalloonTipIcon = ToolTipIcon.Info;
			this.notifyIcon1.BalloonTipText = "CTI Client";
			this.notifyIcon1.BalloonTipTitle = "Title";
			this.notifyIcon1.ContextMenuStrip = this.cmsNotify;
			this.notifyIcon1.Icon = (Icon)resources.GetObject("notifyIcon1.Icon");
			this.notifyIcon1.Text = "CTI Client";
			this.notifyIcon1.MouseUp += new MouseEventHandler(this.notifyIcon1_MouseUp);
			this.cmsNotify.Items.AddRange(new ToolStripItem[]
			{
				this.tsmiLogout,
				this.tsmiClose
			});
			this.cmsNotify.Name = "cmsNotify";
			this.cmsNotify.Size = new Size(101, 48);
			this.tsmiLogout.Name = "tsmiLogout";
			this.tsmiLogout.Size = new Size(100, 22);
			this.tsmiLogout.Text = "注销";
			this.tsmiLogout.Click += new EventHandler(this.tsmiLogout_Click);
			this.tsmiClose.Name = "tsmiClose";
			this.tsmiClose.Size = new Size(100, 22);
			this.tsmiClose.Text = "退出";
			this.tsmiClose.Click += new EventHandler(this.tsmiClose_Click);
			this.AllowDrop = true;
			base.AutoScaleDimensions = new SizeF(6f, 12f);
			base.AutoScaleMode = AutoScaleMode.Font;
			this.BackColor = SystemColors.Control;
			base.ClientSize = new Size(1380, 674);
			base.Controls.Add(this.palMarquee);
			base.Controls.Add(this.splitContainer1);
			base.Controls.Add(this.msMenu);
			base.Controls.Add(this.statusStrip1);
			base.Icon = (Icon)resources.GetObject("$this.Icon");
			base.KeyPreview = true;
			base.Name = "frmMain";
			base.Opacity = 0.0;
			base.StartPosition = FormStartPosition.CenterScreen;
			this.Text = "Polylink CTI Client";
			base.Load += new EventHandler(this.FrmMain_Load);
			base.SizeChanged += new EventHandler(this.frmMain_SizeChanged);
			base.Activated += new EventHandler(this.frmMain_Activated);
			base.FormClosed += new FormClosedEventHandler(this.frmMain_FormClosed);
			base.FormClosing += new FormClosingEventHandler(this.frmMain_FormClosing);
			base.Resize += new EventHandler(this.frmMain_Resize);
			base.KeyDown += new KeyEventHandler(this.frmMain_KeyDown);
			this.cmsWB.ResumeLayout(false);
			this.msMenu.ResumeLayout(false);
			this.msMenu.PerformLayout();
			this.statusStrip1.ResumeLayout(false);
			this.statusStrip1.PerformLayout();
			this.cmsMonitor.ResumeLayout(false);
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel1.PerformLayout();
			this.splitContainer1.Panel2.ResumeLayout(false);
			this.splitContainer1.ResumeLayout(false);
			((ISupportInitialize)this.picDown).EndInit();
			this.toolStrip1.ResumeLayout(false);
			this.toolStrip1.PerformLayout();
			this.splitContainer2.Panel1.ResumeLayout(false);
			this.splitContainer2.Panel2.ResumeLayout(false);
			this.splitContainer2.Panel2.PerformLayout();
			this.splitContainer2.ResumeLayout(false);
			this.splitContainer3.Panel1.ResumeLayout(false);
			this.splitContainer3.Panel2.ResumeLayout(false);
			this.splitContainer3.ResumeLayout(false);
			this.tabControl1.ResumeLayout(false);
			this.tabPage1.ResumeLayout(false);
			this.ssWBstatus.ResumeLayout(false);
			this.ssWBstatus.PerformLayout();
			this.palMarquee.ResumeLayout(false);
			this.palMarquee.PerformLayout();
			this.cmsNotify.ResumeLayout(false);
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		[DllImport("user32.dll")]
		public static extern IntPtr FindWindow(string class_name, string app_name);

		[DllImport("User32.dll", EntryPoint = "SendMessage")]
		private static extern int PostMessage(IntPtr wnd, int msg, IntPtr wP, IntPtr lP);

		[DllImport("user32")]
		private static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

		[DllImport("user32.dll")]
		public static extern bool FlashWindowEx(ref FLASHWINFO pwfi);

		public frmMain()
		{
			this.InitializeComponent();
			if (null == frmMain.Log)
			{
				frmMain.Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
				frmMain.Log.Info("client is init success！");
			}
			this.initMainForm();
		}

		private void initMainForm()
		{
			this.agentBar1.CallInEvent += new AgentBar.CallInEventHandler(this.Evt_CallIn);
			this.agentBar1.PredictCallOutBridgeRingEvent += new AgentBar.PredictCallOutBridgeRingEventHandler(this.Evt_CallIn);
			this.agentBar1.SignInResponse += new AgentBar.SignInResponseEventHandler(this.Res_SignIn);
			this.agentBar1.SignInEvent += new AgentBar.SignInEventHandler(this.Evt_SignInEvent);
			this.agentBar1.SignOutEvent += new AgentBar.SignOutEventHandler(this.Evt_SignOutEvent);
			this.agentBar1.KickOutEvent += new AgentBar.KickOutEventHandler(this.Evt_KickOut);
			this.agentBar1.SockDisconnectEvent += new AgentBar.SockDisconnectEventHandler(this.Evt_SocketDisconnect);
			this.agentBar1.WarnAgentResigninEvent += new AgentBar.WarnAgentResigninEventHandler(this.Evt_WarnAgentResignin);
			this.agentBar1.WarnAgentForceChangeStatusEvent += new AgentBar.WarnAgentForceChangeStatusEventHandler(this.Evt_WarnAgentForceChangeStatus);
			this.agentBar1.HangupEvent += new AgentBar.HangupEventHandler(this.Evt_Hangup);
			this.agentBar1.AnswerEvent += new AgentBar.AnswerEventHandler(this.Evt_Answer);
			this.agentBar1.AnswerEvent += new AgentBar.AnswerEventHandler(this.Evt_Bridge);
			this.agentBar1.CallOutRingEvent += new AgentBar.CallOutRingEventHandler(this.Evt_CallOutRing);
			this.agentBar1.CheckExtenEvent += new AgentBar.CheckExtenEventHandler(this.Evt_CheckExtenEventRing);
			this.agentBar1.InternalCallerRingEvent += new AgentBar.InternalCallerRingEventHandler(this.Evt_InternalCallerRingEvent);
			this.agentBar1.InternalCall_CallInEvent += new AgentBar.InternalCall_CallInEventHandler(this.Evt_InternalCalledRingEvent);
			this.agentBar1.ThreeWayCallRingEvent += new AgentBar.ThreeWayCallRingEventHandler(this.Evt_ThreeWayRingEvent);
			this.agentBar1.ConsultCallInEvent += new AgentBar.Consult_CallInEventHandler(this.Evt_ConsultCallInEvent);
			this.agentBar1.EavesdropCallRingEvent += new AgentBar.EavesdropCallRingEventHandler(this.Evt_EavesdropRingEvent);
			this.agentBar1.WhisperCallRingEvent += new AgentBar.WhisperCallRingEventHandler(this.Evt_WhisperRingEvent);
			this.agentBar1.BargeinCallRingEvent += new AgentBar.BargeinCallRingEventHandler(this.Evt_BargeinRingEvent);
			this.agentBar1.ForceHangupCallRingEvent += new AgentBar.ForceHangupCallRingEventHandler(this.Evt_ForceHangupRingEvent);
			this.agentBar1.TransferBlindCallInEvent += new AgentBar.Transfer_Blind_CallInEventHandler(this.Evt_TransferBlindCallInEvent);
			this.agentBar1.GetRoleNameEvent += new AgentBar.GetRoleNameEventHandler(this.Evt_GetRoleNameEvent);
			this.agentBar1.ServerResponse += new AgentBar.ServerResponseEventHandler(this.Evt_Response);
			this.agentBar1.SysThrowExceptionEvent += new AgentBar.SysThrowExceptionEventHandler(this.Evt_SysThrowException);
			this.agentBar1.GetWebSiteInfoEvent += new AgentBar.GetWebSiteInfoEventHandler(this.Evt_GetWebSiteInfo);
			this.agentBar1.AddCustomerToQueueEvent += new AgentBar.AddCustomerToQueueEventHandler(this.Evt_Customer_Enter_Queue);
			this.agentBar1.DelCustomerFromQueueEvent += new AgentBar.DelCustomerFromQueueEventHandler(this.Evt_Customer_Leave_Queue);
			this.agentBar1.GetCustomerOfMyQueueEvent += new AgentBar.GetCustomerOfMyQueueEventHandler(this.Evt_Get_Customer_Of_My_Queue);
			this.agentBar1.ApplyChangeStatusDistributeEvent += new AgentBar.ApplyChangeStatusDistributeEventHandler(this.Evt_ApplyChangeStatusDistribute);
			this.agentBar1.ApproveChangeStatusTimeoutDistributeEvent += new AgentBar.ApproveChangeStatusTimeoutEventDistributeHandler(this.Evt_ApproveChangeStatusTimeoutDistribute);
			this.agentBar1.ApplyChangeStatusCancelEvent += new AgentBar.ApplyChangeStatusCancelEventHandler(this.Evt_ApplyChangeStatusCancel);
			this.agentBar1.ApproveChangeStatusDistributeEvent += new AgentBar.ApproveChangeStatusDistributeEventHandler(this.Evt_ApproveChangeStatusDistribute);
			this.agentBar1.GetChangeStatusApplyListEvent += new AgentBar.GetChangeStatusApplyListEventHandler(this.Evt_GetChangeStatusApplyList);
			this.AgentStatusHotKeySettingEvent = (frmMain.AgentStatusHotKeySettingEventHandler)Delegate.Combine(this.AgentStatusHotKeySettingEvent, new frmMain.AgentStatusHotKeySettingEventHandler(this.agentBar1.Evt_SetAgentStatusHotKeyString));
			if (null != this.AgentStatusHotKeySettingEvent)
			{
				this.AgentStatusHotKeySettingEvent(Helper.Hot_Key_Setting_Idle, Helper.Hot_Key_Setting_Busy, Helper.Hot_Key_Setting_Leave, Helper.Hot_Key_Setting_CallOut, Helper.Hot_Key_Setting_Monitor);
			}
			this.agentBar1.ForceChangeStatusEvent += new AgentBar.ForceChangeStatusEventHandler(this.Evt_ForceChangeStatus);
		}

		private void Evt_GetChangeStatusApplyList(string agentID, List<Apply_Change_Status> apply_agent_list, int retCode, string reason)
		{
			this.initApplyHistoryLst();
		}

		private void FrmMain_Load(object sender, EventArgs e)
		{
			this.startPoint = new Point(0, 0);
			base.BeginInvoke(new ThreadStart(this.hideMySelf));
			this.reLogin();
			this.tmrChkACWTimeOut.Interval = 1000;
			this.initMarqueePannelAndLabel();
			this.tmrMarquee.Enabled = true;
			this.tmrMarquee.Interval = 100;
			this.tmrMarquee.Start();
			string appPath = Application.StartupPath;
			string notify_path = appPath + "\\images\\notify.ico";
			string blank_path = appPath + "\\images\\blank.ico";
			if (File.Exists(notify_path) && File.Exists(blank_path))
			{
				this.ico_notify = new Icon(notify_path);
				this.ico_blank = new Icon(blank_path);
				this.notifyTimer.Tick += new EventHandler(this.notifyTimer_Tick);
				this.notifyTimer.Interval = 500;
				this.notifyTimer.Enabled = true;
				this.notifyTimer.Stop();
			}
			this.notifyIcon1.Visible = false;
			this.picDown.BackColor = this.agentBar1.BackColor;
			this.picDown.Image = Resources.Arrow_003;
			this.keyPressTime = DateTime.Now;
			this.frmMainLoadControlsCheak();
			this.newLoginConfig.ScreenPhoneFormat = Helper.getScreenPhoneFormat;
			this.agentBar1.controls_info = this.controlsinfoTemp;
			this.agentBar1.GetAgentLocationInfo(this.agentBar1.controls_count_previous);
			this.agentBar1.ChangeControls();
			this.agentBar1.changetsddbAfterHangupSize();
			this.agentBar1.transferControlsVisible += new AgentBar.transferControlsVisibleHandler(this.ControlsVisible);
			this.agentBar1.SoftphoneAutoAnswer = Helper.IsSoftphoneAutoAnswer;
		}

		public void ControlsVisible()
		{
			this.newControlsConfig.Controls_Visible = this.agentBar1.controlsVisible;
			this.newControlsConfig.addNodes();
		}

		public void frmMainLoadControlsCheak()
		{
			if (!Helper.load_sys_config())
			{
				MessageBox.Show("读取配置文件失败！  ", "读取配置文件", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			}
			else
			{
				this.controlsinfoTemp.chkHoldInfo = (Helper.isHold == "1");
				this.controlsinfoTemp.chkMuteInfo = (Helper.isMute == "1");
				this.controlsinfoTemp.chkThreeWayInfo = (Helper.isThreeWay == "1");
				this.controlsinfoTemp.chkConsultInfo = (Helper.isConsult == "1");
				this.controlsinfoTemp.chkStopConsultInfo = (Helper.isStopConsult == "1");
				this.controlsinfoTemp.chkConsultTransferInfo = (Helper.isConsultTransfer == "1");
				this.controlsinfoTemp.chkTransferInfo = (Helper.isTransfer == "1");
				this.controlsinfoTemp.chkGradeInfo = (Helper.isGrade == "1");
				this.controlsinfoTemp.chkCallAgentInfo = (Helper.isCallAgent == "1");
				this.controlsinfoTemp.chkCallOutInfo = (Helper.isCallOut == "1");
				this.controlsinfoTemp.chkListenInfo = (Helper.isListen == "1");
				this.controlsinfoTemp.chkWhisperInfo = (Helper.isWhisper == "1");
				this.controlsinfoTemp.chkBargeinInfo = (Helper.isBargein == "1");
				this.controlsinfoTemp.chkForceHangupInfo = (Helper.isForceHangup == "1");
				this.controlsinfoTemp.chkCheckInfo = (Helper.isCheck == "1");
				this.controlsinfoTemp.chkMonitorInfo = (Helper.isMonitor == "1");
				this.controlsinfoTemp.chkCancelApplicationInfo = (Helper.isCancelApplication == "1");
				this.controlsinfoTemp.chkdbAfterHangupInfo = (Helper.isdbAfterHangup == "1");
				this.controlsinfoTemp.chkApproveInfo = (Helper.isApprove == "1");
				this.controlsinfoTemp.chkNoAnswerCallsInfo = (Helper.isNoAnswerCalls == "1");
			}
		}

		public static bool flashTaskBar(IntPtr hWnd, frmMain.falshType type)
		{
			FLASHWINFO fInfo = default(FLASHWINFO);
			fInfo.cbSize = Convert.ToUInt32(Marshal.SizeOf(fInfo));
			fInfo.hwnd = hWnd;
			fInfo.dwFlags = (uint)type;
			fInfo.uCount = 4294967295u;
			fInfo.dwTimeout = 0u;
			return frmMain.FlashWindowEx(ref fInfo);
		}

		private void notifyTimer_Tick(object sender, EventArgs e)
		{
			if (this.ico_notify != null && this.ico_blank != null)
			{
				if (!frmMain.Is_Blink)
				{
					this.notifyIcon1.Icon = this.ico_notify;
				}
				else
				{
					this.notifyIcon1.Icon = this.ico_blank;
				}
				frmMain.Is_Blink = !frmMain.Is_Blink;
			}
		}

		private void initMarqueePannelAndLabel()
		{
			int all_tssl_length = this.tsslAgentExten.Width + this.tsslAgentID.Width + this.tsslAgentName.Width + this.tsslGroup.Width + this.tsslRole.Width;
			this.palMarquee.Width = this.statusStrip1.Width - all_tssl_length - 25;
			Point p = new Point(this.palMarquee.Width, 5);
			this.lblMarquee.Location = p;
			Point pannel_pos = new Point(all_tssl_length + 100, this.statusStrip1.Location.Y + 2);
			this.palMarquee.Location = pannel_pos;
			this.palMarquee.Height = this.statusStrip1.Height - 5;
		}

		private void initCallHistoryLst()
		{
			this.callHistoryLst = new List<string>();
			this.mDataFilePath = Environment.GetEnvironmentVariable("APPDATA") + "\\" + Helper.App_Directory_Name + "\\data.xml";
			this.ReadFromXml(this.mDataFilePath, "ID", ref this.callHistoryLst);
			this.agentBar1.CalloutHistory = this.callHistoryLst;
		}

		private void initApplyHistoryLst()
		{
			this.mDataFilePath = Environment.GetEnvironmentVariable("APPDATA") + "\\" + Helper.App_Directory_Name + "\\data.xml";
			if (this.agentBar1.AgentRoleAndRight.rights_of_view_agent_group_info)
			{
				List<Apply_Change_Status> apply_history = new List<Apply_Change_Status>();
				this.ReadFromXmlOfApplyHistory(this.mDataFilePath, ref apply_history);
				this.agentBar1.ApplyChangeStatusApprovalHistory = apply_history;
			}
		}

		private void initStatus()
		{
			this.agentBar1.Enabled = true;
			this.Text = Helper.ApplicationTitle + "(" + this.agentBar1.AgentID + ")";
			this.tsslAgentID.Text = "坐席号:" + this.agentBar1.AgentID;
			this.g_username = this.agentBar1.AgentID;
			this.g_password = this.agentBar1.AgentPwd;
			this.tsslAgentName.Text = "姓名:" + this.agentBar1.AgentName;
			this.g_agentname = this.agentBar1.AgentName;
			this.tsslAgentExten.Text = "分机号:" + this.agentBar1.AgentExten;
			this.g_exten = this.agentBar1.AgentExten;
			this.tsslRole.Text = "角色名称:";
			this.tsslGroup.Text = "坐席组名:" + this.agentBar1.AgentGroupName;
			this.g_group_name = this.agentBar1.AgentGroupName;
			this.g_group_id = this.agentBar1.AgentGroupID;
			this.agentBar1.DefaultAccessNum = Helper.Default_Access_Number;
			this.agentBar1.IsMonitorOfflineAgent = Helper.IsMonitorOfflineAgent;
			this.initCallHistoryLst();
		}

		private void hideMySelf()
		{
			base.Visible = false;
		}

		private void reLogin()
		{
			frmMain.Log.Debug("enter relogin.IsFirstRun:" + this.IsFirstRun);
			if (!this.IsFirstRun)
			{
				this.IsFirstRun = true;
				this.checkLogin();
			}
			else
			{
				this.IsExitWithNoPrompt = true;
				this.agentBar1.DoDisconnect();
				this.blnIsSignOut = true;
				frmMain.Log.Debug("restart application now..........");
				Program.run.Close();
				Application.Restart();
				frmMain.Log.Debug("restart application finished...........");
			}
			this.IsLoging = false;
		}

		private void checkLogin()
		{
			this.intCount = 0;
			this.newLoginForm = new FrmLogin();
			this.LoginResult = frmMain.EnumLoginResult.LoginProcess;
			if (this.newLoginForm.ShowDialog() == DialogResult.OK)
			{
				if (this.newLoginForm.chkRemember.Checked)
				{
					this.isRememberPwd = true;
				}
				else
				{
					this.isRememberPwd = false;
				}
				Helper.SigninID = this.newLoginForm.txtUserName.Text;
				this.IsLoging = false;
				Helper.SelectStatus = this.newLoginForm.cboStatus.SelectedIndex;
				this.newLoginForm.btnOk.Enabled = false;
				if (!Helper.load_sys_config() || !Helper.load_error_code())
				{
					MessageBox.Show("读取配置文件失败！  ", "读取配置文件", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				}
				else
				{
					if (this.newLoginForm.cboPhoneType.Text == "内置软电话2")
					{
						this.agentBar1.SoftPhoneEnable = true;
						this.agentBar1.SipServer = Helper.SipServer;
						this.agentBar1.SipPort = Helper.SipPort;
						this.agentBar1.SipNum = Helper.SipNum;
						this.agentBar1.SipPwd = Helper.SipPwd;
						this.agentBar1.SipRegistTime = Helper.SipRegistTime;
						this.agentBar1.SipAutoAnswer = Helper.SipAutoAnswer;
						this.agentBar1.SipLocalNum = Helper.SipCallid;
						this.mBindPhoneType = frmMain.EnumPhoneType.InternalPhone;
					}
					else
					{
						this.agentBar1.SoftPhoneEnable = false;
					}
					this.agentBar1.ServerIP = Helper.ServerIP;
					this.agentBar1.ServerPort = Helper.Port;
					this.agentBar1.HeartBeatTimeout = Helper.HeartTimeout;
					if (string.IsNullOrEmpty(Helper.NoAnswerCallsURL))
					{
						this.agentBar1.NoAnswerCallsURL = Helper.ServerIP + ":8080";
					}
					else
					{
						this.agentBar1.NoAnswerCallsURL = Helper.NoAnswerCallsURL;
					}
					this.agentBar1.AgentID = this.newLoginForm.txtUserName.Text;
					this.agentBar1.AgentPwd = this.newLoginForm.txtPwd.Text;
					this.agentBar1.AgentExten = this.newLoginForm.txtExten.Text;
					this.agentBar1.AgentStatus = this.newLoginForm.str2AgentStatus(this.newLoginForm.cboStatus.Text);
					this.agentBar1.RefreshReportStatisInterval = Helper.ReportStatisInterval;
					this.agentBar1.AgentStateAfterHangup = AgentBar.Str2AgentStatus(Helper.Default_Agent_State_After_Hangup.ToString());
					Helper.LoginStatus = this.agentBar1.AgentStatus;
					if (this.newLoginForm.txtExten.Text == "")
					{
						this.agentBar1.BindExten = false;
					}
					else
					{
						this.agentBar1.BindExten = true;
					}
					if (this.newLoginForm.cboPhoneType.Text == "内置软电话")
					{
						this.mBindPhoneType = frmMain.EnumPhoneType.ControlOtherPhone;
						this.agentBar1.bindSoftPhoneLogin = true;
						this.agentBar1.SoftPhoneEnable2 = true;
						this.agentBar1.SipAutoSignIn = true;
						this.agentBar1.SipPhoneOnLineWarning = true;
						this.agentBar1.SoftPhoneAnswerCmd = Helper.SipSoftPhone_Answer_Cmd;
						this.agentBar1.SoftPhoneLogoffCmd = Helper.SipSoftPhone_Logoff_Cmd;
						this.agentBar1.SoftPhoneAppName = Helper.SipSoftPhone_App_Name;
						this.agentBar1.SoftPhoneAppClassName = Helper.SipSoftPhone_App_Class_Name;
						this.agentBar1.SoftPhoneAppProcessName = Helper.SoftPhoneAppProcessName;
						this.agentBar1.SoftPhoneMsgValue = Helper.SipSoftPhone_Msg_Value;
						this.agentBar1.SipServer = Helper.SipServer;
						this.agentBar1.SipPort = Helper.SipPort;
						this.agentBar1.SipNum = Helper.SipNum;
						this.agentBar1.SipPwd = Helper.SipPwd;
						this.agentBar1.SipRegistTime = Helper.SipRegistTime;
						this.agentBar1.SipAutoAnswer = Helper.SipAutoAnswer;
						this.agentBar1.SipLocalNum = Helper.SipCallid;
					}
					frmMain.Log.Debug("开始连接CtiSever.... ");
					int rt = this.agentBar1.DoConnect();
					if (rt != 0)
					{
						if (rt == -2)
						{
							MessageBox.Show("内置软电话注册失败！  ", "登录", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
						}
						else
						{
							MessageBox.Show("连接服务器失败！  ", "登录", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
						}
						this.reLogin();
					}
					else
					{
						Thread.Sleep(Helper.SigninIntervalAfterSoftPhoneRegisted);
						frmMain.Log.Debug("开始签入.... ");
						if (this.agentBar1.DoSignIn())
						{
							this.newLoginForm.lblStatus.Text = "正在签入,请稍候.....";
						}
						else
						{
							this.newLoginForm.lblStatus.Text = "签入失败！";
							this.newLoginForm.btnOk.Enabled = true;
						}
						frmMain.Log.Debug("DoSignIn() is execute ok!will wait reply...");
						this.tmrCheckLogin.Start();
					}
				}
			}
			else
			{
				Environment.Exit(0);
			}
		}

		private void start_softphone_app()
		{
			int first_login_delay_time = 0;
			if (this.mBindPhoneType == frmMain.EnumPhoneType.ControlOtherPhone)
			{
				Process[] soft_phone_app_process = Process.GetProcessesByName(this.agentBar1.SoftPhoneAppProcessName);
				if (0 == soft_phone_app_process.Count<Process>())
				{
					string arg = string.Concat(new object[]
					{
						"-u",
						Helper.SipNum,
						" -p",
						Helper.SipPwd,
						" -s",
						Helper.SipServer,
						":",
						Helper.SipPort
					});
					string process_path = Application.StartupPath + "\\视频通话\\VideoTelephone.exe";
					string work_dir = Application.StartupPath + "\\视频通话\\";
					if (!Directory.Exists(Environment.GetEnvironmentVariable("APPDATA") + "\\wonderUsers\\" + this.newLoginForm.txtExten.Text))
					{
						first_login_delay_time = 8000;
					}
					if (!this.start_process(work_dir, process_path, arg))
					{
						MessageBox.Show("启动内置软电话失败！  ", "启动内置软电话", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
						this.reLogin();
					}
					Thread.Sleep(Helper.SipSoftPhone_after_login_delay_time + first_login_delay_time);
				}
				else
				{
					if (this.agentBar1.SipPhoneOnLineWarning)
					{
						if (MessageBox.Show("内置软电话程序正在运行中，如果正在通话则电话将被挂断，您是否确定要关闭它？", "退出内置软电话", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
						{
							return;
						}
					}
					Process[] array = soft_phone_app_process;
					for (int i = 0; i < array.Length; i++)
					{
						Process p = array[i];
						p.Kill();
					}
					Thread.Sleep(3000);
					this.start_softphone_app();
				}
			}
		}

		public static IntPtr GetProcessWindowHandle(string className, string strProcTitle)
		{
			IntPtr hwnd = IntPtr.Zero;
			return frmMain.FindWindow(className, strProcTitle);
		}

		private bool start_process(string work_dir, string path, string args)
		{
			this.process_info = new ProcessStartInfo();
			this.process_info.WorkingDirectory = work_dir;
			this.process_info.FileName = path;
			this.process_info.Arguments = args;
			this.process_info.CreateNoWindow = true;
			this.process_info.UseShellExecute = true;
			bool result;
			try
			{
				Process.Start(this.process_info);
				result = true;
			}
			catch (Win32Exception we)
			{
				MessageBox.Show(this, we.Message);
				result = false;
			}
			return result;
		}

		private void WaitForCtiCallBack(object sender, EventArgs e)
		{
			if (this.LoginResult == frmMain.EnumLoginResult.LoginProcess)
			{
				if (this.intCount > 50)
				{
					this.tmrCheckLogin.Stop();
					Thread.Sleep(100);
					this.intCount = 0;
					MessageBox.Show("  登录已超时 ！  ", "登录", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					this.reLogin();
					this.IsExitWithNoPrompt = true;
					if (!this.agentBar1.DoDisconnect())
					{
						frmMain.Log.Error("断开连接失败！");
					}
				}
				else
				{
					Thread.Sleep(100);
					this.intCount++;
				}
			}
			else if (this.LoginResult == frmMain.EnumLoginResult.LoginSuccess)
			{
				this.initSignIn();
			}
			else
			{
				this.tmrCheckLogin.Stop();
				this.intCount = 0;
			}
		}

		private void initSignIn()
		{
			this.tmrCheckLogin.Stop();
			this.newLoginForm.Close();
			this.Text = Helper.ApplicationTitle + "(工号:" + this.agentBar1.AgentID + ")";
			base.Show();
			base.Opacity = 100.0;
			base.Visible = true;
		}

		private void Evt_Hangup(string hangupReason, int retCode, string makeStr)
		{
			this.lblAccessNum.Text = "";
			this.lblTelNumber.Text = "";
			this.lblAccessNum.Visible = false;
			this.lblTelNumber.Visible = false;
			this.Clear_Global_Call_Variable();
			if (this.newFrmTip != null)
			{
				this.newFrmTip.Stop();
			}
		}

		private void Clear_Global_Call_Variable()
		{
			this.g_access_num_name = "";
			this.g_area_name = "";
			this.g_area_id = "";
			this.g_area_country = "";
			this.g_area_province = "";
			this.g_area_city = "";
			this.g_cust_grade = "";
			this.g_called_id = "";
			this.g_caller_id = "";
		}

		private void Evt_Answer(string makeStr)
		{
			if (this.newFrmTip != null)
			{
				this.newFrmTip.Stop();
			}
		}

		private void Evt_Bridge(string makeStr)
		{
			if (this.newFrmTip != null)
			{
				this.newFrmTip.Stop();
			}
		}

		private void Evt_CallOutRing(string customerNum, string callcenterNum, string accessNumName, string callType, string areaID, string areaName, string makeStr)
		{
			this.g_area_id = areaID;
			string customerNumBack = string.Empty;
			string customer_num_format_local = frmMain.get_variable_value(makeStr, "customer_num_format_local");
			string customer_num_format_national = frmMain.get_variable_value(makeStr, "customer_num_format_national");
			string customer_num_format_e164 = frmMain.get_variable_value(makeStr, "customer_num_format_e164");
			string customer_num_phone_type = frmMain.get_variable_value(makeStr, "customer_num_phone_type");
			if ("-2".Equals(this.g_area_id))
			{
				areaName = "未知归属地";
			}
			else if ("-1".Equals(this.g_area_id))
			{
				areaName = "内部号码";
			}
			string[] area_str = areaName.Split(new char[]
			{
				'-'
			});
			if (area_str.Length == 3)
			{
				this.g_area_country = area_str[0];
				this.g_area_province = area_str[1];
				this.g_area_city = area_str[2];
			}
			else if (area_str.Length == 2)
			{
				this.g_area_country = area_str[0];
				this.g_area_province = area_str[1];
				this.g_area_city = area_str[1];
			}
			if (areaName.Substring(0, 1) == "-")
			{
				this.g_area_name = areaName.Substring(1);
			}
			else
			{
				this.g_area_name = areaName;
			}
			this.g_cust_grade = "0";
			string customer_grade_str;
			if (this.g_cust_grade == "0")
			{
				customer_grade_str = "普通客户";
			}
			else if (this.g_cust_grade == "1")
			{
				customer_grade_str = "VIP客户";
			}
			else
			{
				customer_grade_str = "普通客户";
			}
			string showMsg = customerNum + "\r\n" + this.g_area_name;
			this.showTipInfo(showMsg);
			string showAccessNumName;
			if (string.IsNullOrEmpty(accessNumName))
			{
				showAccessNumName = "主叫号码:" + callcenterNum;
			}
			else if (accessNumName.Length > 15)
			{
				showAccessNumName = string.Concat(new string[]
				{
					"主叫号码:",
					callcenterNum,
					"(",
					accessNumName.Substring(0, 15),
					"..)"
				});
			}
			else
			{
				showAccessNumName = string.Concat(new string[]
				{
					"主叫号码:",
					callcenterNum,
					"(",
					accessNumName,
					")"
				});
			}
			if (areaName.Length > 8)
			{
				this.lblTelNumber.Text = string.Concat(new string[]
				{
					"去电号码:",
					customerNum,
					"(",
					this.g_area_name.Substring(0, 8),
					"..) ",
					customer_grade_str
				});
			}
			else
			{
				this.lblTelNumber.Text = string.Concat(new string[]
				{
					"去电号码:",
					customerNum,
					"(",
					this.g_area_name,
					") ",
					customer_grade_str
				});
			}
			this.lblAccessNum.Text = showAccessNumName;
			this.lblTelNumber.Visible = true;
			this.lblAccessNum.Visible = true;
			string strUrl = Helper.CallOutWebURL;
			if (!(strUrl == ""))
			{
				this.g_call_type = callType;
				customerNumBack = customerNum;
				if (this.newLoginConfig.ScreenPhoneFormat == "1")
				{
					if (!string.IsNullOrEmpty(customer_num_format_local))
					{
						customerNumBack = customer_num_format_local;
					}
				}
				if (this.newLoginConfig.ScreenPhoneFormat == "2")
				{
					if (customer_num_phone_type == "mobile")
					{
						if (!string.IsNullOrEmpty(customer_num_format_local))
						{
							customerNumBack = customer_num_format_local;
						}
					}
					else if (!string.IsNullOrEmpty(customer_num_format_national))
					{
						customerNumBack = customer_num_format_national;
					}
				}
				if (this.newLoginConfig.ScreenPhoneFormat == "3")
				{
					if (!string.IsNullOrEmpty(customer_num_format_e164))
					{
						customerNumBack = customer_num_format_e164;
					}
				}
				strUrl = strUrl.Replace("{customernum}", customerNumBack);
				strUrl = strUrl.Replace("{callcenternum}", callcenterNum);
				strUrl = strUrl.Replace("{accessnumname}", accessNumName);
				strUrl = strUrl.Replace("{agentnum}", this.g_username);
				strUrl = strUrl.Replace("{exten}", this.g_exten);
				strUrl = strUrl.Replace("{calltype}", callType);
				strUrl = strUrl.Replace("{PWD}", this.g_password);
				strUrl = strUrl.Replace("{MD5PWD}", Helper.Md5.make_md5_str(this.g_password));
				string md5_str = Helper.Md5.make_md5_str(this.g_password) + DateTime.Now.ToString("yyyyMMdd");
				strUrl = strUrl.Replace("{MD5PWD2}", Helper.Md5.make_md5_str(md5_str));
				strUrl = strUrl.Replace("{agentGroup}", this.g_group_name);
				strUrl = strUrl.Replace("{areaid}", this.g_area_id);
				strUrl = strUrl.Replace("{areaname}", this.g_area_name);
				string signature = this.makeSignature(customerNum, this.g_username);
				strUrl = strUrl.Replace("{sign}", signature);
				strUrl = strUrl.Replace("{customergrade}", this.g_cust_grade);
				strUrl = strUrl.Replace("{areaCountry}", this.g_area_country);
				strUrl = strUrl.Replace("{areaProvince}", this.g_area_province);
				strUrl = strUrl.Replace("{areaCity}", this.g_area_city);
				if (this.tabControl1.SelectedTab.Text == "新标签页")
				{
					WebBrowser currentWebBrowser = (WebBrowser)this.tabControl1.SelectedTab.Controls[0];
					this.tabControl1.SelectedTab.Text = "呼出弹屏";
					currentWebBrowser.Navigate(strUrl);
				}
				else
				{
					this.NewWebForm("呼出弹屏", strUrl);
				}
			}
		}

		private void Evt_CheckExtenEventRing(string AgentID, string makeStr)
		{
			this.showTipInfo(AgentID);
		}

		private void Evt_InternalCallerRingEvent(string AgentID, string CallerID, string CallType, string makeStr)
		{
			this.showTipInfo(CallerID);
		}

		private void Evt_InternalCalledRingEvent(string AgentID, string CallerID, string CallType, string makeStr)
		{
			this.showTipInfo(CallerID);
		}

		private void Evt_Response(string agentid, int retCode, string reason)
		{
			if (0 != retCode)
			{
				if (retCode != -69)
				{
					this.LoginResult = frmMain.EnumLoginResult.LoginFail;
					this.showMessageBox("", "请求失败", retCode);
				}
			}
		}

		private void Evt_SysThrowException(string reason, int retCode)
		{
			frmMain.Log.Debug("enter Evt_SysThrowException.");
			this.blnIsSignOut = true;
			if (this.blnLoginSuccess)
			{
				if (!this.IsExitWithNoPrompt)
				{
					if (retCode == -1002)
					{
						this.agentBar1.Enabled = false;
						this.reSignin("已与服务器断开连接！", "断开连接");
					}
				}
			}
		}

		private void Evt_GetRoleNameEvent(string roleName)
		{
			this.tsslRole.Text = "角色名:" + roleName;
		}

		private void Evt_ThreeWayRingEvent(string AgentID, string customerNum, string callcenterNum, string accessNumName, string callerAgentNum, string callType, string areaID, string areaName, string custGrade, string outExtraParamsFromIvr, string makeStr)
		{
			if (!Helper.OpenUrlWhenThreeWayCallin)
			{
				this.showTipInfo(callerAgentNum);
			}
			else
			{
				string mak_str = string.Concat(new string[]
				{
					"customer_num=",
					customerNum,
					"#callcenter_num=",
					callcenterNum,
					"#access_num_name=",
					accessNumName,
					"#agent_num=",
					AgentID,
					"#target_agent_num=",
					callerAgentNum,
					"#call_type=",
					callType,
					"#area_id=",
					areaID,
					"#area_name=",
					areaName,
					"#cust_grade=",
					custGrade,
					makeStr
				});
				this.Evt_CallIn(callerAgentNum, mak_str, callType, false, outExtraParamsFromIvr);
			}
		}

		private void Evt_ConsultCallInEvent(string AgentID, string customerNum, string callcenterNum, string accessNumName, string consulterAgentNum, string callType, string areaID, string areaName, string custGrade, string outExtraParamsFromIvr, string makeStr)
		{
			if (!Helper.OpenUrlWhenConsultCallin)
			{
				this.showTipInfo(consulterAgentNum);
			}
			else
			{
				string mak_str = string.Concat(new string[]
				{
					"customer_num=",
					customerNum,
					"#callcenter_num=",
					callcenterNum,
					"#access_num_name=",
					accessNumName,
					"#agent_num=",
					AgentID,
					"#target_agent_num=",
					consulterAgentNum,
					"#call_type=",
					callType,
					"#area_id=",
					areaID,
					"#area_name=",
					areaName,
					"#cust_grade=",
					custGrade,
					makeStr
				});
				this.Evt_CallIn(consulterAgentNum, mak_str, callType, false, outExtraParamsFromIvr);
			}
		}

		private void Evt_TransferBlindCallInEvent(string AgentID, string customerNum, string callcenterNum, string accessNumName, string callerAgentNum, string callType, string areaID, string areaName, string custGrade, string outExtraParamsFromIvr, string makeStr)
		{
			string mak_str = string.Concat(new string[]
			{
				"customer_num=",
				customerNum,
				"#callcenter_num=",
				callcenterNum,
				"#access_num_name=",
				accessNumName,
				"#agent_num=",
				AgentID,
				"#target_agent_num=",
				callerAgentNum,
				"#call_type=",
				callType,
				"#area_id=",
				areaID,
				"#area_name=",
				areaName,
				"#cust_grade=",
				custGrade,
				makeStr
			});
			this.Evt_CallIn(customerNum, mak_str, callType, true, outExtraParamsFromIvr);
		}

		private void Evt_EavesdropRingEvent(string AgentID, string customerNum, string callcenterNum, string accessNumName, string desAgentID, string callType, string areaID, string areaName, string custGrade, string outExtraParamsFromIvr, string makeStr)
		{
			if (!Helper.OpenUrlWhenEavesDropCallin)
			{
				this.showTipInfo(desAgentID);
			}
			else
			{
				string mak_str = string.Concat(new string[]
				{
					"customer_num=",
					customerNum,
					"#callcenter_num=",
					callcenterNum,
					"#access_num_name=",
					accessNumName,
					"#agent_num=",
					AgentID,
					"#target_agent_num=",
					desAgentID,
					"#call_type=",
					callType,
					"#area_id=",
					areaID,
					"#area_name=",
					areaName,
					"#cust_grade=",
					custGrade,
					makeStr
				});
				this.Evt_CallIn(customerNum, mak_str, callType, true, outExtraParamsFromIvr);
			}
		}

		private void Evt_WhisperRingEvent(string AgentID, string customerNum, string callcenterNum, string accessNumName, string desAgentID, string callType, string areaID, string areaName, string custGrade, string outExtraParamsFromIvr, string makeStr)
		{
			if (!Helper.OpenUrlWhenWhisperCallin)
			{
				this.showTipInfo(desAgentID);
			}
			else
			{
				string mak_str = string.Concat(new string[]
				{
					"customer_num=",
					customerNum,
					"#callcenter_num=",
					callcenterNum,
					"#access_num_name=",
					accessNumName,
					"#agent_num=",
					AgentID,
					"#target_agent_num=",
					desAgentID,
					"#call_type=",
					callType,
					"#area_id=",
					areaID,
					"#area_name=",
					areaName,
					"#cust_grade=",
					custGrade,
					makeStr
				});
				this.Evt_CallIn(customerNum, mak_str, callType, true, outExtraParamsFromIvr);
			}
		}

		private void Evt_BargeinRingEvent(string AgentID, string customerNum, string callcenterNum, string accessNumName, string desAgentID, string callType, string areaID, string areaName, string custGrade, string outExtraParamsFromIvr, string makeStr)
		{
			if (!Helper.OpenUrlWhenBargeinCallin)
			{
				this.showTipInfo(desAgentID);
			}
			else
			{
				string mak_str = string.Concat(new string[]
				{
					"customer_num=",
					customerNum,
					"#callcenter_num=",
					callcenterNum,
					"#access_num_name=",
					accessNumName,
					"#agent_num=",
					AgentID,
					"#target_agent_num=",
					desAgentID,
					"#call_type=",
					callType,
					"#area_id=",
					areaID,
					"#area_name=",
					areaName,
					"#cust_grade=",
					custGrade,
					makeStr
				});
				this.Evt_CallIn(customerNum, mak_str, callType, true, outExtraParamsFromIvr);
			}
		}

		private void Evt_ForceHangupRingEvent(string AgentID, string customerNum, string callcenterNum, string accessNumName, string desAgentID, string callType, string areaID, string areaName, string custGrade, string outExtraParamsFromIvr, string makeStr)
		{
			if (!Helper.OpenUrlWhenForceHangupCallin)
			{
				this.showTipInfo(desAgentID);
			}
			else
			{
				string mak_str = string.Concat(new string[]
				{
					"customer_num=",
					customerNum,
					"#callcenter_num=",
					callcenterNum,
					"#access_num_name=",
					accessNumName,
					"#agent_num=",
					AgentID,
					"#target_agent_num=",
					desAgentID,
					"#call_type=",
					callType,
					"#area_id=",
					areaID,
					"#area_name=",
					areaName,
					"#cust_grade=",
					custGrade,
					makeStr
				});
				this.Evt_CallIn(customerNum, mak_str, callType, true, outExtraParamsFromIvr);
			}
		}

		private void Res_SignIn(string AgentID, int retCode, string strReason)
		{
			if (0 != retCode)
			{
				this.tmrCheckLogin.Stop();
				if (this.IsResigning)
				{
					this.IsResigning = false;
					this.reSignin("", "重新签入");
				}
				else
				{
					this.reLogin();
				}
			}
		}

		private void Evt_Customer_Enter_Queue(string call_type, string callcenter_num, string customer_num, string customer_status, string customer_type, string customer_uuid, string enter_queue_time, string exclusive_agent_num, string exclusive_queue_num, string queue_num, string current_time, string queue_name, string queue_customer_amount, string early_queue_enter_time, string early_queue_enter_time_all, string customer_enter_channel)
		{
			frmMain.Log.Debug("enter Evt_Customer_Enter_Queue.");
			string ret_str = "\r\n\n";
			string queue_info = string.Concat(new string[]
			{
				"电话号码：",
				customer_num,
				ret_str,
				"队列号码：",
				queue_num,
				ret_str,
				"队列名称：",
				queue_name,
				ret_str,
				"所在队列排队人数：",
				queue_customer_amount
			});
			lock (frmMain.g_my_customer_of_queue)
			{
				if (frmMain.g_my_customer_of_queue != null)
				{
					Customer_Info_Struct cust = default(Customer_Info_Struct);
					cust.queue_num = queue_num;
					cust.enter_queue_time = enter_queue_time;
					cust.customer_uuid = customer_uuid;
					cust.wait_time = ComFunc.CalculateTimeLengthToSec(current_time, cust.enter_queue_time);
					if (frmMain.g_my_customer_of_queue.ContainsKey(queue_num))
					{
						List<Customer_Info_Struct> cust_lst = frmMain.g_my_customer_of_queue[queue_num];
						cust_lst.Add(cust);
						frmMain.g_my_customer_of_queue[cust.queue_num] = cust_lst;
						this.show_customer_statis_of_my_queue();
					}
				}
			}
			this.showTip_QueueInfo(queue_info);
		}

		private void Evt_Customer_Leave_Queue(string customer_uuid, string queue_num, string current_time, string early_queue_enter_time, string early_queue_enter_time_all, string reason)
		{
			frmMain.Log.Debug("enter Evt_Customer_Leave_Queue.");
			lock (frmMain.g_my_customer_of_queue)
			{
				if (frmMain.g_my_customer_of_queue != null)
				{
					if (frmMain.g_my_customer_of_queue.ContainsKey(queue_num))
					{
						List<Customer_Info_Struct> cust_lst = frmMain.g_my_customer_of_queue[queue_num];
						for (int i = 0; i < cust_lst.Count; i++)
						{
							if (cust_lst[i].customer_uuid == customer_uuid)
							{
								cust_lst.RemoveAt(i);
								break;
							}
						}
						frmMain.g_my_customer_of_queue[queue_num] = cust_lst;
						this.show_customer_statis_of_my_queue();
					}
				}
			}
		}

		private void show_customer_statis_of_my_queue()
		{
			string showinfo = "";
			if (frmMain.g_my_customer_of_queue != null)
			{
				foreach (string queue_num in frmMain.g_my_customer_of_queue.Keys)
				{
					ulong max_wait_time = 0uL;
					List<Customer_Info_Struct> cust_lst = frmMain.g_my_customer_of_queue[queue_num];
					for (int i = 0; i < cust_lst.Count; i++)
					{
						if (max_wait_time == 0uL)
						{
							max_wait_time = cust_lst[i].wait_time;
						}
						else if (max_wait_time < cust_lst[i].wait_time)
						{
							max_wait_time = cust_lst[i].wait_time;
						}
					}
					if (showinfo != "")
					{
						showinfo += "   ";
					}
					showinfo = string.Concat(new object[]
					{
						showinfo,
						" 队列号码：",
						queue_num,
						" 等待人数：",
						cust_lst.Count,
						" 最长等待时间：",
						ComFunc.ConverToFormatTimeLength(Convert.ToInt32(max_wait_time))
					});
				}
				frmMain.marqueeStr = showinfo;
			}
		}

		private void Evt_Get_Customer_Of_My_Queue(string queueNumLstStr, string current_time, List<Customer_Info_Struct> customer_list)
		{
			frmMain.Log.Debug("enter Evt_Get_Customer_Of_My_Queue.");
			frmMain.g_my_customer_of_queue = new Dictionary<string, List<Customer_Info_Struct>>();
			lock (frmMain.g_my_customer_of_queue)
			{
				string[] queueNumLst = queueNumLstStr.Split(new char[]
				{
					','
				});
				string[] array = queueNumLst;
				for (int j = 0; j < array.Length; j++)
				{
					string queueNum = array[j];
					List<Customer_Info_Struct> cust_lst = new List<Customer_Info_Struct>();
					frmMain.g_my_customer_of_queue[queueNum] = cust_lst;
				}
				for (int i = 0; i < customer_list.Count; i++)
				{
					Customer_Info_Struct cust = customer_list[i];
					cust.wait_time = ComFunc.CalculateTimeLengthToSec(current_time, cust.enter_queue_time);
					if (frmMain.g_my_customer_of_queue.ContainsKey(cust.queue_num))
					{
						List<Customer_Info_Struct> cust_lst = frmMain.g_my_customer_of_queue[cust.queue_num];
						cust_lst.Add(cust);
						frmMain.g_my_customer_of_queue[cust.queue_num] = cust_lst;
					}
					else
					{
						List<Customer_Info_Struct> cust_lst = new List<Customer_Info_Struct>();
						cust_lst.Add(cust);
						frmMain.g_my_customer_of_queue[cust.queue_num] = cust_lst;
					}
				}
			}
			this.show_customer_statis_of_my_queue();
			this.queueWaitTime = new System.Threading.Timer(new TimerCallback(this.countWaitTimeLength), null, 1000, 1000);
		}

		private void Evt_ApplyChangeStatusDistribute(string AgentID, string apply_agentid, string targetStatus, string apply_agentName, string apply_agent_groupID, string apply_agent_groupName, string apply_time, string apply_type, int retCode, string reason)
		{
			if (this.agentBar1.AgentRoleAndRight.group_role_right1 && this.agentBar1.AgentRoleAndRight.controled_agent_group_lst != null)
			{
				if (this.agentBar1.AgentRoleAndRight.controled_agent_group_lst.IndexOf(apply_agent_groupID) >= 0)
				{
					this.showTipInfo2(new frmMain.ShowTipInfoStruct
					{
						btnVisible = true,
						openBtnName = "tsbApprove",
						autoClose = true,
						info = string.Concat(new string[]
						{
							"收到一个",
							AgentBar.str2ApplyType(apply_type),
							"的离开申请：\r\n工号：",
							apply_agentid,
							"\r\n时间：",
							ComFunc.TotalSecondToDateTime(apply_time),
							"\r\n请及时审批！"
						})
					});
				}
			}
		}

		private void Evt_ApproveChangeStatusTimeoutDistribute(string AgentID, string apply_agentid, string apply_agent_groupID, string targetStatus, string timeoutType)
		{
			if (!(AgentID != apply_agentid))
			{
				this.showTipInfo2(new frmMain.ShowTipInfoStruct
				{
					autoClose = true,
					info = "您的离开申请已经超时，请重新申请！"
				});
			}
		}

		private void Evt_ApproveChangeStatusDistribute(string AgentID, string apply_agentid, string apply_agent_groupID, string targetStatus, string approveResult, string approve_time, int retCode, string reason)
		{
			frmMain.ShowTipInfoStruct tip = default(frmMain.ShowTipInfoStruct);
			tip.autoClose = true;
			if (AgentID == apply_agentid)
			{
				if (approveResult == "1")
				{
					tip.info = "您的离开申请已经批准通过！";
				}
				else
				{
					tip.info = "您的离开申请未批准通过，请稍候再申请！";
				}
				this.showTipInfo2(tip);
			}
		}

		private void Evt_ApplyChangeStatusCancel(string AgentID, string apply_agentid, string targetStatus, int retCode, string reason)
		{
			frmMain.ShowTipInfoStruct tip = default(frmMain.ShowTipInfoStruct);
			tip.autoClose = true;
			if (AgentID == apply_agentid)
			{
				if (retCode == 0)
				{
					tip.info = "您已成功取消申请离开！";
				}
				else
				{
					tip.info = "取消申请离开失败！";
				}
				this.showTipInfo2(tip);
			}
		}

		private void countWaitTimeLength(object a)
		{
			lock (frmMain.g_my_customer_of_queue)
			{
				if (frmMain.g_my_customer_of_queue != null)
				{
					foreach (string queue_num in frmMain.g_my_customer_of_queue.Keys)
					{
						List<Customer_Info_Struct> cust_lst = frmMain.g_my_customer_of_queue[queue_num];
						for (int i = 0; i < cust_lst.Count; i++)
						{
							Customer_Info_Struct cust = cust_lst[i];
							cust.wait_time += 1uL;
							cust_lst[i] = cust;
						}
					}
					this.show_customer_statis_of_my_queue();
				}
			}
		}

		private void marqueeOfQueueWaitInfo()
		{
			try
			{
				if (frmMain.marqueeStr == "")
				{
					this.lblMarquee.Text = "";
				}
				else
				{
					this.lblMarquee.Text = frmMain.marqueeStr;
					if (this.lblMarquee.Left <= -this.lblMarquee.Width)
					{
						this.lblMarquee.Left = this.palMarquee.Width;
					}
					else
					{
						this.lblMarquee.Left = this.lblMarquee.Left - 5;
					}
				}
			}
			catch
			{
			}
		}

		private void flashIcon()
		{
			if (this.ico_notify != null && this.ico_blank != null)
			{
				this.notifyTimer.Start();
			}
			if (base.ShowInTaskbar)
			{
				frmMain.flashTaskBar(base.Handle, frmMain.falshType.FLASHW_TIMERNOFG);
			}
		}

		private void Evt_CallIn(string callerID, string makeStr, string call_type, bool bNeedArea, string outExtraParamsFromIvr)
		{
			string customerNumBack = string.Empty;
			this.flashIcon();
			string customerNum = frmMain.get_variable_value(makeStr, "customer_num");
			this.g_caller_id = customerNum;
			string callcenterNum = frmMain.get_variable_value(makeStr, "callcenter_num");
			this.g_called_id = callcenterNum;
			string accessNumName = frmMain.get_variable_value(makeStr, "access_num_name");
			this.g_access_num_name = accessNumName;
			string areaID = frmMain.get_variable_value(makeStr, "area_id");
			this.g_area_id = areaID;
			string cust_grade = frmMain.get_variable_value(makeStr, "cust_grade");
			this.g_cust_grade = cust_grade;
			string customer_num_format_local = frmMain.get_variable_value(makeStr, "customer_num_format_local");
			string customer_num_format_national = frmMain.get_variable_value(makeStr, "customer_num_format_national");
			string customer_num_format_e164 = frmMain.get_variable_value(makeStr, "customer_num_format_e164");
			string customer_num_phone_type = frmMain.get_variable_value(makeStr, "customer_num_phone_type");
			string customer_grade_str;
			if (this.g_cust_grade == "0")
			{
				customer_grade_str = "普通客户";
			}
			else if (this.g_cust_grade == "1")
			{
				customer_grade_str = "VIP客户";
			}
			else
			{
				customer_grade_str = "普通客户";
			}
			if ("-2".Equals(this.g_area_id))
			{
				string areaName = "未知归属地";
				this.g_area_name = areaName;
			}
			else if ("-1".Equals(this.g_area_id))
			{
				string areaName = "内部号码";
				this.g_area_name = areaName;
			}
			else
			{
				string areaName = frmMain.get_variable_value(makeStr, "area_name");
				string[] area_str = areaName.Split(new char[]
				{
					'-'
				});
				if (area_str.Length == 3)
				{
					this.g_area_country = area_str[0];
					this.g_area_province = area_str[1];
					this.g_area_city = area_str[2];
				}
				else if (area_str.Length == 2)
				{
					this.g_area_country = area_str[0];
					this.g_area_province = area_str[1];
					this.g_area_city = area_str[1];
				}
				if (areaName.Substring(0, 1) == "-")
				{
					this.g_area_name = areaName.Substring(1);
				}
				else
				{
					this.g_area_name = areaName;
				}
			}
			string showMsg;
			if (bNeedArea)
			{
				showMsg = callerID + "\r\n" + this.g_area_name;
			}
			else
			{
				showMsg = callerID;
			}
			if (string.IsNullOrEmpty(this.g_area_name))
			{
				this.lblTelNumber.Text = "来电号码:" + customerNum + customer_grade_str;
			}
			else if (this.g_area_name.Length > 8)
			{
				this.lblTelNumber.Text = string.Concat(new string[]
				{
					"来电号码:",
					customerNum,
					"(",
					this.g_area_name.Substring(0, 8),
					"..) ",
					customer_grade_str
				});
			}
			else
			{
				this.lblTelNumber.Text = string.Concat(new string[]
				{
					"来电号码:",
					customerNum,
					"(",
					this.g_area_name,
					") ",
					customer_grade_str
				});
			}
			string showAccessNumName;
			if (string.IsNullOrEmpty(accessNumName))
			{
				showAccessNumName = "接入号:" + callcenterNum;
			}
			else if (accessNumName.Length > 15)
			{
				showAccessNumName = string.Concat(new string[]
				{
					"接入号名称:",
					accessNumName.Substring(0, 15),
					"..(",
					callcenterNum,
					")"
				});
			}
			else
			{
				showAccessNumName = string.Concat(new string[]
				{
					"接入号名称:",
					accessNumName,
					"(",
					callcenterNum,
					")"
				});
			}
			this.lblAccessNum.Text = showAccessNumName;
			this.lblAccessNum.Visible = true;
			this.lblTelNumber.Visible = true;
			this.showTipInfo(showMsg);
			string strUrl = Helper.CallInWebURL;
			if (!(strUrl == ""))
			{
				string taskID = frmMain.get_variable_value(makeStr, "task_id");
				this.g_task_id = taskID;
				string queueNum = frmMain.get_variable_value(makeStr, "queue_num");
				this.g_queue_num = queueNum;
				string agentNum = frmMain.get_variable_value(makeStr, "agent_num");
				string callType = frmMain.get_variable_value(makeStr, "call_type");
				string relation_uuid = frmMain.get_variable_value(makeStr, "relation_uuid");
				if (relation_uuid == "")
				{
					relation_uuid = "undefine";
				}
				this.g_call_type = callType;
				if (callType == "")
				{
					callType = call_type;
				}
				string targetAgentNum = frmMain.get_variable_value(makeStr, "target_agent_num");
				customerNumBack = customerNum;
				if (this.newLoginConfig.ScreenPhoneFormat == "1")
				{
					if (!string.IsNullOrEmpty(customer_num_format_local))
					{
						customerNumBack = customer_num_format_local;
					}
				}
				if (this.newLoginConfig.ScreenPhoneFormat == "2")
				{
					if (customer_num_phone_type == "mobile")
					{
						if (!string.IsNullOrEmpty(customer_num_format_local))
						{
							customerNumBack = customer_num_format_local;
						}
					}
					else if (!string.IsNullOrEmpty(customer_num_format_national))
					{
						customerNumBack = customer_num_format_national;
					}
				}
				if (this.newLoginConfig.ScreenPhoneFormat == "3")
				{
					if (!string.IsNullOrEmpty(customer_num_format_e164))
					{
						customerNumBack = customer_num_format_e164;
					}
				}
				strUrl = strUrl.Replace("{customernum}", customerNumBack);
				strUrl = strUrl.Replace("{callcenternum}", callcenterNum);
				strUrl = strUrl.Replace("{accessnumname}", accessNumName);
				strUrl = strUrl.Replace("{taskid}", taskID);
				strUrl = strUrl.Replace("{queuenum}", queueNum);
				strUrl = strUrl.Replace("{agentnum}", agentNum);
				strUrl = strUrl.Replace("{calltype}", callType);
				strUrl = strUrl.Replace("{targetAgentNum}", targetAgentNum);
				strUrl = strUrl.Replace("{Uniqueid}", relation_uuid);
				strUrl = strUrl.Replace("{PWD}", this.g_password);
				strUrl = strUrl.Replace("{MD5PWD}", Helper.Md5.make_md5_str(this.g_password));
				string md5_str = Helper.Md5.make_md5_str(this.g_password) + DateTime.Now.ToString("yyyyMMdd");
				strUrl = strUrl.Replace("{MD5PWD2}", Helper.Md5.make_md5_str(md5_str));
				strUrl = strUrl.Replace("{agentGroup}", this.g_group_name);
				strUrl = strUrl.Replace("{areaid}", this.g_area_id);
				strUrl = strUrl.Replace("{areaname}", this.g_area_name);
				strUrl = strUrl.Replace("{areaCountry}", this.g_area_country);
				strUrl = strUrl.Replace("{areaProvince}", this.g_area_province);
				strUrl = strUrl.Replace("{areaCity}", this.g_area_city);
				string[] ivrExtraParamsLst = outExtraParamsFromIvr.Split(new char[]
				{
					'|'
				});
				for (int i = 0; i < ivrExtraParamsLst.Length; i++)
				{
					int pos = ivrExtraParamsLst[i].IndexOf('=');
					if (pos > 0)
					{
						string key = ivrExtraParamsLst[i].Substring(0, pos);
						string value = ivrExtraParamsLst[i].Substring(pos + 1);
						strUrl = strUrl.Replace("{" + key + "}", value);
					}
				}
				string signature = this.makeSignature(customerNum, this.g_username);
				strUrl = strUrl.Replace("{sign}", signature);
				if (this.g_cust_grade == "")
				{
					this.g_cust_grade = "0";
				}
				strUrl = strUrl.Replace("{customergrade}", this.g_cust_grade);
				if (this.tabControl1.SelectedTab.Text == "新标签页")
				{
					WebBrowser currentWebBrowser = (WebBrowser)this.tabControl1.SelectedTab.Controls[0];
					this.tabControl1.SelectedTab.Text = "来电弹屏";
					currentWebBrowser.Navigate(strUrl);
				}
				else
				{
					this.NewWebForm("来电弹屏", strUrl);
				}
			}
		}

		private string makeSignature(string callerID, string agentNum)
		{
			string orgstr = string.Empty;
			string signature = string.Empty;
			string result;
			if (string.Empty == callerID || string.Empty == agentNum)
			{
				result = signature;
			}
			else
			{
				orgstr = string.Concat(new string[]
				{
					callerID,
					"_",
					agentNum,
					"_",
					this.g_salt_key
				});
				signature = Helper.Md5.make_md5_str(orgstr);
				result = signature;
			}
			return result;
		}

		private void showTipInfo(string callerID)
		{
			this.threadFun(callerID);
		}

		private void showTipInfo2(frmMain.ShowTipInfoStruct showInfo)
		{
			frmMain.Log.Debug("enter showTipInfo2.");
			object myObject = showInfo;
			this.threadFun2(myObject);
		}

		private void showTip_QueueInfo(string strInfo)
		{
			frmMain.Log.Debug("enter showTipInfo2.");
			this.threadFun_queueInfo(strInfo);
		}

		private static string get_variable_value(string strSource, string find_str)
		{
			int variable_pos = strSource.IndexOf(find_str);
			string result;
			if (variable_pos < 0)
			{
				result = "";
			}
			else
			{
				int equal = strSource.IndexOf("=", variable_pos);
				if (equal < 0)
				{
					result = "";
				}
				else
				{
					int comma = strSource.IndexOf("#", equal);
					string get_value = strSource.Substring(equal + 1, comma - equal - 1);
					result = get_value;
				}
			}
			return result;
		}

		private void threadFun(object myObject)
		{
			this.showCallinTip(myObject);
		}

		private void showCallinTip(object myObject)
		{
			string strCallerID = (string)myObject;
			if (this.newFrmTip != null && !this.newFrmTip.IsDisposed)
			{
				this.startPoint = this.newFrmTip.EndPoint;
			}
			else
			{
				this.startPoint = new Point(Screen.PrimaryScreen.WorkingArea.Width, Screen.PrimaryScreen.WorkingArea.Height);
			}
			this.newFrmTip = new FrmTip();
			this.newFrmTip.Start("来电", strCallerID, false, 2000, this.startPoint);
		}

		private void threadFun2(object myObject)
		{
			frmMain.Log.Debug("enter threadFun2.");
			this.showCallinTip2(myObject);
		}

		private void threadFun_queueInfo(object myObject)
		{
			frmMain.Log.Debug("enter threadFun_queueInfo.");
			this.showQueueInfoTip(myObject);
		}

		private void showCallinTip2(object myObject)
		{
			frmMain.Log.Debug("enter showCallinTip2.");
			frmMain.ShowTipInfoStruct tipInfo = (frmMain.ShowTipInfoStruct)myObject;
			if (this.newFrmSysTip != null && !this.newFrmSysTip.IsDisposed)
			{
				this.startPoint = this.newFrmSysTip.EndPoint;
			}
			else
			{
				this.startPoint = new Point(Screen.PrimaryScreen.WorkingArea.Width, Screen.PrimaryScreen.WorkingArea.Height);
			}
			this.newFrmSysTip = new FrmSysInfo();
			this.newFrmSysTip.AgentBarBtnName = tipInfo.openBtnName;
			this.newFrmSysTip.agentBar1 = this.agentBar1;
			this.newFrmSysTip.ButtonVisible = tipInfo.btnVisible;
			this.newFrmSysTip.Start("消息", tipInfo.info, tipInfo.autoClose, 5000, this.startPoint);
		}

		private void showQueueInfoTip(object myObject)
		{
			frmMain.Log.Debug("enter showQueueInfoTip.");
			if (Helper.Queue_Monitor_Enable)
			{
				string strInfo = (string)myObject;
				this.startPoint = new Point(Screen.PrimaryScreen.WorkingArea.Width, Screen.PrimaryScreen.WorkingArea.Height);
				if (this.newFrmQueueInfoTip == null || this.newFrmQueueInfoTip.IsDisposed)
				{
					this.newFrmQueueInfoTip = new FrmSysInfo();
					this.newFrmQueueInfoTip.Start("有客户进入排队", strInfo, true, 30000, this.startPoint);
				}
				else
				{
					this.newFrmQueueInfoTip.Close();
					this.newFrmQueueInfoTip = new FrmSysInfo();
					this.newFrmQueueInfoTip.Start("有客户进入排队", strInfo, true, 30000, this.startPoint);
				}
			}
		}

		private void Evt_KickOut(string agentID, int retCode, string reason)
		{
			if (retCode != 0)
			{
				this.showMessageBox("", "踢出", retCode);
			}
			this.reSignin("系统检测到你已经在其他地方登陆了！", "强制踢出");
		}

		private void Evt_SocketDisconnect(string reason, int retCode)
		{
			frmMain.Log.Debug("enter Evt_SocketDisconnect.IsExitWithNoPrompt:" + this.IsExitWithNoPrompt);
			this.flashIcon();
			if (!this.IsExitWithNoPrompt)
			{
				this.blnIsSignOut = true;
				if (this.IsManualLogOff)
				{
					this.IsManualLogOff = false;
					this.reLogin();
				}
				else
				{
					this.reSignin("与服务器断开连接!", "断开连接");
				}
				this.Clear_Global_Call_Variable();
			}
		}

		private void Evt_WarnAgentResignin()
		{
			this.showTipInfo2(new frmMain.ShowTipInfoStruct
			{
				autoClose = false,
				info = "您的帐号信息或权限已被修改，建议您重新登陆！"
			});
		}

		private void Evt_WarnAgentForceChangeStatus(string executorAgentID)
		{
			this.showTipInfo2(new frmMain.ShowTipInfoStruct
			{
				autoClose = false,
				info = "请注意！您的坐席状态已被坐席工号：" + executorAgentID + "强制改变。"
			});
		}

		private void Evt_ForceChangeStatus(string agentID, string reason, int retCode)
		{
			if (retCode != 0)
			{
				this.showMessageBox("", "强制改变坐席状态", retCode);
			}
		}

		private void Evt_GetWebSiteInfo(string agentID, int retCode, string reason, Dictionary<string, string> web_site_info)
		{
			this.agent_website_dic = web_site_info;
			this.tsmWebsite.DropDownItems.Clear();
			int i = 0;
			foreach (KeyValuePair<string, string> strPair in web_site_info)
			{
				if (i++ > 3)
				{
					this.tsmWebsite.DropDownItems.Add(new ToolStripSeparator());
					i = 0;
				}
				this.tsmWebsite.DropDownItems.Add(strPair.Key);
			}
			if (this.tsmWebsite.DropDownItems.Count > 0)
			{
				this.tsmWebsite.Visible = true;
			}
			else
			{
				this.tsmWebsite.Visible = false;
			}
		}

		private void initLvwAgent()
		{
			this.lvwAgent.Columns.Add("坐席号", 80, HorizontalAlignment.Left);
			this.lvwAgent.Columns.Add("坐席姓名", 80, HorizontalAlignment.Left);
			this.lvwAgent.Columns.Add("状态", 80, HorizontalAlignment.Left);
			this.lvwAgent.Columns.Add("主叫号码", 80, HorizontalAlignment.Left);
			this.lvwAgent.Columns.Add("队列名称", 80, HorizontalAlignment.Left);
			this.lvwAgent.Columns.Add("持续时长", 80, HorizontalAlignment.Left);
			this.lvwAgent.Columns.Add("通话ID", 80, HorizontalAlignment.Left);
		}

		private void initLvwQueue()
		{
			this.lvwQueue.Columns.Add("通话ID", 80, HorizontalAlignment.Left);
			this.lvwQueue.Columns.Add("主叫号码", 80, HorizontalAlignment.Left);
			this.lvwQueue.Columns.Add("坐席号", 80, HorizontalAlignment.Left);
			this.lvwQueue.Columns.Add("坐席姓名", 80, HorizontalAlignment.Left);
			this.lvwQueue.Columns.Add("持续时长", 80, HorizontalAlignment.Left);
			this.lvwQueue.Columns.Add("队列名称", 80, HorizontalAlignment.Left);
			this.lvwQueue.Columns.Add("被叫号码", 80, HorizontalAlignment.Left);
		}

		private void UpdateAgentMonitor(List<string[]> newMonitorData)
		{
			this.lvwAgent.BeginUpdate();
			for (int i = 0; i < newMonitorData.Count; i++)
			{
				int foundItemIndex = this.FindAgentID(newMonitorData[i].ElementAt(0), this.lvwAgent);
				if (foundItemIndex == -1)
				{
					ListViewItem lvi = new ListViewItem();
					lvi.Text = newMonitorData[i].ElementAt(0).ToString();
					lvi.SubItems.Add(newMonitorData[i].ElementAt(1).ToString());
					lvi.SubItems.Add(newMonitorData[i].ElementAt(2).ToString());
					lvi.SubItems.Add(newMonitorData[i].ElementAt(3).ToString());
					lvi.SubItems.Add(newMonitorData[i].ElementAt(4).ToString());
					lvi.SubItems.Add(newMonitorData[i].ElementAt(5).ToString());
					lvi.SubItems.Add(newMonitorData[i].ElementAt(6).ToString());
					string text = newMonitorData[i].ElementAt(2).ToLower();
					if (text != null)
					{
						if (!(text == "talk"))
						{
							if (!(text == "idle"))
							{
								if (!(text == "hold"))
								{
									if (!(text == "ring"))
									{
										if (text == "offline")
										{
											lvi.ImageKey = "offline";
										}
									}
									else
									{
										lvi.ImageKey = "ring";
									}
								}
								else
								{
									lvi.ImageKey = "hold";
								}
							}
							else
							{
								lvi.ImageKey = "idle";
							}
						}
						else
						{
							lvi.ImageKey = "talk";
						}
					}
					this.lvwAgent.Items.Add(lvi);
				}
				else
				{
					for (int j = 0; j < 6; j++)
					{
						if (this.lvwAgent.Items[foundItemIndex].SubItems[j + 1].Text != newMonitorData[i].ElementAt(j + 1).ToString())
						{
							this.lvwAgent.Items[foundItemIndex].SubItems[j + 1].Text = newMonitorData[i].ElementAt(j + 1).ToString();
							if (j == 1)
							{
								string text = newMonitorData[i].ElementAt(2).ToLower();
								if (text != null)
								{
									if (!(text == "talk"))
									{
										if (!(text == "idle"))
										{
											if (!(text == "hold"))
											{
												if (!(text == "ring"))
												{
													if (text == "offline")
													{
														this.lvwAgent.Items[foundItemIndex].ImageKey = "offline";
													}
												}
												else
												{
													this.lvwAgent.Items[foundItemIndex].ImageKey = "ring";
												}
											}
											else
											{
												this.lvwAgent.Items[foundItemIndex].ImageKey = "hold";
											}
										}
										else
										{
											this.lvwAgent.Items[foundItemIndex].ImageKey = "idle";
										}
									}
									else
									{
										this.lvwAgent.Items[foundItemIndex].ImageKey = "talk";
									}
								}
							}
						}
					}
				}
			}
			this.lvwAgent.EndUpdate();
		}

		private void UpdateQueueMonitor(List<string[]> newMonitorData)
		{
			this.lvwQueue.BeginUpdate();
			for (int i = 0; i < newMonitorData.Count; i++)
			{
				int foundItemIndex = this.FindAgentID(newMonitorData[i].ElementAt(0), this.lvwQueue);
				if (foundItemIndex == -1)
				{
					ListViewItem lvi = new ListViewItem();
					lvi.Text = newMonitorData[i].ElementAt(0).ToString();
					lvi.SubItems.Add(newMonitorData[i].ElementAt(1).ToString());
					lvi.SubItems.Add(newMonitorData[i].ElementAt(2).ToString());
					lvi.SubItems.Add(newMonitorData[i].ElementAt(3).ToString());
					lvi.SubItems.Add(newMonitorData[i].ElementAt(4).ToString());
					lvi.SubItems.Add(newMonitorData[i].ElementAt(5).ToString());
					lvi.SubItems.Add(newMonitorData[i].ElementAt(6).ToString());
					this.lvwQueue.Items.Add(lvi);
				}
				else
				{
					for (int j = 0; j < 6; j++)
					{
						if (this.lvwQueue.Items[foundItemIndex].SubItems[j + 1].Text != newMonitorData[i].ElementAt(j + 1).ToString())
						{
							this.lvwQueue.Items[foundItemIndex].SubItems[j + 1].Text = newMonitorData[i].ElementAt(j + 1).ToString();
						}
					}
				}
			}
			this.lvwQueue.EndUpdate();
		}

		private int FindAgentID(string strAgentID, ListView lvw)
		{
			ListViewItem foundItem = lvw.FindItemWithText(strAgentID);
			int result;
			if (foundItem != null)
			{
				result = foundItem.Index;
			}
			else
			{
				result = -1;
			}
			return result;
		}

		private void Evt_SignInEvent(string AgentID, int retCode, string strReason)
		{
			if (0 == retCode)
			{
				this.blnIsSignOut = false;
				this.SignInSuccess = true;
				this.IsExitWithNoPrompt = false;
				if (this.agentBar1.SoftPhoneEnable || this.agentBar1.SoftPhoneEnable2)
				{
					Helper.SoftPhoneType = 0;
				}
				else if (this.agentBar1.AgentExten != "" && this.agentBar1.AgentExten != null)
				{
					Helper.SoftPhoneType = 1;
				}
				else
				{
					Helper.SoftPhoneType = 2;
				}
				Helper.write_config("softPhoneType", Helper.SoftPhoneType.ToString());
				Helper.write_config("defaultAgentNum", this.agentBar1.AgentID);
				if (Helper.SoftPhoneType == 1)
				{
					Helper.write_config("defaultExten", this.agentBar1.AgentExten);
				}
				Helper.write_config("defaultStatus", Helper.SelectStatus.ToString());
				if (this.isRememberPwd)
				{
					Helper.write_config("IsRemembered", "1");
					Helper.write_config("agentPwd", this.agentBar1.AgentPwd);
				}
				else
				{
					Helper.write_config("IsRemembered", "0");
					Helper.write_config("agentPwd", "");
				}
				this.LoginResult = frmMain.EnumLoginResult.LoginSuccess;
				this.blnLoginSuccess = true;
				this.initStatus();
				if (this.agentBar1.BindExten)
				{
					this.tmrChkACWTimeOut.Start();
				}
				this.ACWTimeCount = 0;
				this.g_salt_key = this.agentBar1.SaltKey;
				this.notifyIcon1.Visible = true;
				if (!string.IsNullOrEmpty(Helper.HomePageWebURL))
				{
					WebBrowser currentWebBrowser = (WebBrowser)this.tabControl1.SelectedTab.Controls[0];
					currentWebBrowser.Navigate(Helper.HomePageWebURL);
				}
			}
			else
			{
				this.SignInSuccess = false;
				this.LoginResult = frmMain.EnumLoginResult.LoginFail;
				this.showMessageBox("", "登录失败", retCode);
				this.reLogin();
			}
		}

		private void Evt_SignOutEvent(string AgentID, int retCode, string strReason)
		{
			frmMain.Log.Debug(string.Concat(new object[]
			{
				"enter Evt_SignOutEvent.IsExitWithNoPrompt:",
				this.IsExitWithNoPrompt,
				",IsManualLogOff:",
				this.IsManualLogOff
			}));
			string info = "";
			if (0 == retCode)
			{
				this.blnIsSignOut = true;
				this.blnLoginSuccess = false;
				this.agentBar1.DoDisconnect();
				if (this.IsExitWithNoPrompt)
				{
					base.Close();
				}
				if (strReason == "exten unregister")
				{
					info = "分机已下线!";
				}
				else if (strReason == "agent info deleted in db")
				{
					info = "此坐席已被禁用或者删除！";
				}
				if (this.IsManualLogOff)
				{
					this.IsManualLogOff = false;
					this.reLogin();
				}
				else
				{
					this.reSignin(info + "您已经被签出，", "已签出");
				}
				this.tmrChkACWTimeOut.Stop();
			}
			else
			{
				this.showMessageBox("", "签出", retCode);
			}
			if (this.IsExitWithNoPrompt)
			{
			}
		}

		private void showMessageBox(string info, string title, int retCode)
		{
			if (Helper.ErrorCodeDic.ContainsKey(retCode.ToString()))
			{
				info += Helper.ErrorCodeDic[retCode.ToString()];
				MessageBox.Show(info, title, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
			}
			else
			{
				MessageBox.Show("无此错误码:" + retCode + " 请更新错误码！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
			}
		}

		private void reSignin(string msgInfo, string msgTitle)
		{
			bool blnSignin = false;
			if (!this.IsLoging)
			{
				this.IsLoging = true;
				while (!blnSignin)
				{
					DialogResult rt = MessageBox.Show(this, msgInfo + "是否要重新签入？ \n\n【是】重新签入 \n【否】返回到登录界面 \n【取消】关闭此对话框 ", msgTitle, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
					if (DialogResult.Yes == rt)
					{
						this.agentBar1.AgentStatus = Helper.LoginStatus;
						frmMain.Log.Debug("执行 DoSignInWithConnect()....");
						if (!this.agentBar1.DoSignInWithConnect())
						{
							frmMain.Log.Debug("执行 DoSignInWithConnect() 失败！");
							blnSignin = false;
						}
						else
						{
							blnSignin = true;
							this.lblAccessNum.Text = "";
							this.lblTelNumber.Text = "";
							this.lblAccessNum.Visible = false;
							this.lblTelNumber.Visible = false;
							this.Clear_Global_Call_Variable();
						}
						frmMain.Log.Debug("执行 DoSignInWithConnect() 成功！");
						this.IsResigning = true;
					}
					else
					{
						if (DialogResult.No == rt)
						{
							this.IsLoging = false;
							this.reLogin();
							return;
						}
						return;
					}
				}
				this.IsLoging = false;
			}
		}

		private void tsbGoback_Click(object sender, EventArgs e)
		{
			WebBrowser currentWb = (WebBrowser)this.tabControl1.SelectedTab.Controls[0];
			currentWb.GoBack();
		}

		private void tsbRefresh_Click(object sender, EventArgs e)
		{
			WebBrowser currentWb = (WebBrowser)this.tabControl1.SelectedTab.Controls[0];
			currentWb.Refresh();
		}

		private void tsbGoForward_Click(object sender, EventArgs e)
		{
			WebBrowser currentWb = (WebBrowser)this.tabControl1.SelectedTab.Controls[0];
			currentWb.GoForward();
		}

		private void tsbSearch_Click(object sender, EventArgs e)
		{
			this.WebSearch();
		}

		private void wb_ProgressChanged(object sender, WebBrowserProgressChangedEventArgs e)
		{
			try
			{
				if (((WebBrowser)sender).Handle.ToString() != ((WebBrowser)this.tabControl1.SelectedTab.Controls[0]).Handle.ToString())
				{
					this.tsbWBstatus.Text = "";
					this.tspWebProcess.Value = 0;
					this.tspWebProcess.Visible = false;
				}
				else if (e.CurrentProgress > 0L && e.MaximumProgress > 0L)
				{
					this.tspWebProcess.Maximum = Convert.ToInt32(e.MaximumProgress);
					this.tspWebProcess.Step = Convert.ToInt32(e.CurrentProgress);
					this.tspWebProcess.PerformStep();
					if (((WebBrowser)sender).StatusText != "")
					{
						this.tsbWBstatus.Text = ((WebBrowser)sender).StatusText;
					}
				}
				else if (((WebBrowser)sender).ReadyState == WebBrowserReadyState.Complete)
				{
					this.tspWebProcess.Value = 0;
					this.tspWebProcess.Visible = false;
				}
			}
			catch (Exception ex)
			{
				string logMsgInfo = string.Concat(new string[]
				{
					"系统发生未知错误！",
					string.Format("版本 {0} ", ComFunc.AssemblyVersion),
					"来源:",
					ex.Source,
					",信息:",
					ex.Message,
					",堆栈:",
					ex.StackTrace
				});
				frmMain.Log.Error(logMsgInfo);
				MessageBox.Show("系统发生未知错误！" + string.Format("版本 {0} ", ComFunc.AssemblyVersion), "警告", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			}
		}

		private void wb_Navigated(object sender, WebBrowserNavigatedEventArgs e)
		{
			try
			{
				if (!(((WebBrowser)sender).Handle.ToString() != ((WebBrowser)this.tabControl1.SelectedTab.Controls[0]).Handle.ToString()))
				{
					this.tscURL.Text = ((WebBrowser)sender).Url.ToString();
				}
			}
			catch (Exception ex)
			{
				string logMsgInfo = string.Concat(new string[]
				{
					"系统发生未知错误！",
					string.Format("版本 {0} ", ComFunc.AssemblyVersion),
					"来源:",
					ex.Source,
					",信息:",
					ex.Message,
					",堆栈:",
					ex.StackTrace
				});
				frmMain.Log.Error(logMsgInfo);
				MessageBox.Show("系统发生未知错误！" + string.Format("版本 {0} ", ComFunc.AssemblyVersion), "警告", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			}
		}

		private void wb_Navigating(object sender, WebBrowserNavigatingEventArgs e)
		{
			try
			{
				if (((WebBrowser)sender).Handle.ToString() != ((WebBrowser)this.tabControl1.SelectedTab.Controls[0]).Handle.ToString())
				{
					this.tsbWBstatus.Text = "";
					this.tspWebProcess.Value = 0;
					this.tspWebProcess.Visible = false;
				}
				else
				{
					this.tsbWBstatus.Text = "正在加载....";
					this.tspWebProcess.Visible = true;
				}
			}
			catch (Exception ex)
			{
				string logMsgInfo = string.Concat(new string[]
				{
					"系统发生未知错误！",
					string.Format("版本 {0} ", ComFunc.AssemblyVersion),
					"来源:",
					ex.Source,
					",信息:",
					ex.Message,
					",堆栈:",
					ex.StackTrace
				});
				frmMain.Log.Error(logMsgInfo);
				MessageBox.Show("系统发生未知错误！" + string.Format("版本 {0} ", ComFunc.AssemblyVersion), "警告", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			}
		}

		private void wb_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
		{
			try
			{
				string pagetitle = ((WebBrowser)sender).DocumentTitle;
				if (this.agent_website_dic != null)
				{
					foreach (KeyValuePair<string, string> website in this.agent_website_dic)
					{
						string website_url = ((WebBrowser)sender).Url.ToString();
						int startPos = website_url.IndexOf("reportApi");
						if (startPos > 0)
						{
							int startPos_question_mark = website_url.LastIndexOf(".action");
							string webUrl_prefix = ((WebBrowser)sender).Url.ToString().Substring(0, startPos_question_mark);
							if (website.Value.ToString().IndexOf(webUrl_prefix) >= 0)
							{
								pagetitle = website.Key.ToString();
								break;
							}
						}
					}
				}
				if (pagetitle.Length > 10)
				{
					pagetitle = pagetitle.Substring(0, 9) + "...";
				}
				((WebBrowser)sender).Parent.Text = pagetitle;
				if (((WebBrowser)sender).Handle.ToString() != ((WebBrowser)this.tabControl1.SelectedTab.Controls[0]).Handle.ToString())
				{
					this.tsbWBstatus.Text = "";
					this.tspWebProcess.Value = 0;
					this.tspWebProcess.Visible = false;
				}
				else
				{
					this.tsbWBstatus.Text = "完成  ";
					this.tspWebProcess.Visible = false;
				}
			}
			catch (Exception ex)
			{
				string logMsgInfo = string.Concat(new string[]
				{
					"系统发生未知错误！",
					string.Format("版本 {0} ", ComFunc.AssemblyVersion),
					"来源:",
					ex.Source,
					",信息:",
					ex.Message,
					",堆栈:",
					ex.StackTrace
				});
				frmMain.Log.Error(logMsgInfo);
				MessageBox.Show("系统发生未知错误！" + string.Format("版本 {0} ", ComFunc.AssemblyVersion), "警告", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			}
		}

		private void AddToCombo(string URL)
		{
			if (!(URL == string.Empty))
			{
				int i;
				for (i = 0; i < this.tscURL.Items.Count; i++)
				{
					if (this.tscURL.Items[i].ToString() == this.tscURL.Text.ToString())
					{
						break;
					}
				}
				if (i >= this.tscURL.Items.Count)
				{
					this.tscURL.Items.Insert(0, this.tscURL.Text);
					if (this.tscURL.Items.Count > 10)
					{
						this.tscURL.Items.RemoveAt(this.tscURL.Items.Count - 1);
					}
				}
			}
		}

		private void ConvertCombol(ref ToolStripComboBox cbo)
		{
			List<string> lstUrl = new List<string>();
			for (int i = 0; i < cbo.Items.Count; i++)
			{
				lstUrl.Add(cbo.Items[i].ToString());
			}
			int MaxUrlCount = lstUrl.Count;
			if (MaxUrlCount > 20)
			{
				MaxUrlCount = 20;
			}
			cbo.Items.Clear();
			for (int i = MaxUrlCount - 1; i >= 0; i--)
			{
				cbo.Items.Add(lstUrl[i].ToString());
			}
		}

		private void wb_NewWindow(object sender, CancelEventArgs e)
		{
			try
			{
				e.Cancel = true;
				string newUrl = ((WebBrowser)sender).StatusText;
				string pagetitle = ((WebBrowser)sender).DocumentTitle;
				if (pagetitle.Length > 10)
				{
					pagetitle = pagetitle.Substring(0, 9) + "...";
				}
				if (newUrl.IndexOf("http") != -1)
				{
					this.NewWebForm(pagetitle, newUrl);
				}
				else if (newUrl.IndexOf("javascript:") != -1)
				{
					e.Cancel = false;
				}
			}
			catch (Exception ex)
			{
				string logMsgInfo = string.Concat(new string[]
				{
					"系统发生未知错误！",
					string.Format("版本 {0} ", ComFunc.AssemblyVersion),
					"来源:",
					ex.Source,
					",信息:",
					ex.Message,
					",堆栈:",
					ex.StackTrace
				});
				frmMain.Log.Error(logMsgInfo);
				MessageBox.Show("系统发生未知错误！" + string.Format("版本 {0} ", ComFunc.AssemblyVersion), "警告", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			}
		}

		private void tscURL_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Up || e.KeyCode == Keys.Down)
			{
				this.IsSearch = false;
			}
			if (e.KeyCode == Keys.Return)
			{
				this.WebSearch();
			}
		}

		private void WebSearch()
		{
			if (!(this.tscURL.Text == ""))
			{
				WebBrowser currentWebBrowser = (WebBrowser)this.tabControl1.SelectedTab.Controls[0];
				currentWebBrowser.AccessibleName = "";
				currentWebBrowser.Navigate(this.tscURL.Text);
				this.AddToCombo(this.tscURL.Text);
			}
		}

		private void ReadFromXml(string xmlFileName, string getTypeName, ref List<string> dataLst)
		{
			dataLst.Clear();
			XmlTextReader textReader = new XmlTextReader(xmlFileName);
			try
			{
				textReader.Read();
				while (textReader.Read())
				{
					XmlNodeType nType = textReader.NodeType;
					if (nType == XmlNodeType.Element)
					{
						if (textReader.Name == getTypeName)
						{
							textReader.Read();
							if (textReader.NodeType == XmlNodeType.Text && textReader.Value != "")
							{
								dataLst.Add(textReader.Value.ToString());
							}
						}
					}
				}
				textReader.Close();
			}
			catch (Exception e)
			{
				MessageBox.Show(e.Message);
			}
		}

		private void ReadFromXmlOfApplyHistory(string xmlFileName, ref List<Apply_Change_Status> dataLst)
		{
			dataLst.Clear();
			XmlTextReader textReader = new XmlTextReader(xmlFileName);
			textReader.Read();
			try
			{
				while (textReader.Read())
				{
					XmlNodeType nType = textReader.NodeType;
					if (nType == XmlNodeType.Element)
					{
						if (textReader.Name == "ApproveInfo")
						{
							Apply_Change_Status applyInfo = default(Apply_Change_Status);
							while (textReader.Read())
							{
								if (textReader.NodeType == XmlNodeType.Element)
								{
									string name = textReader.Name;
									switch (name)
									{
									case "ApplyID":
										applyInfo.applyAgentID = textReader.ReadElementString().Trim();
										break;
									case "ApplyName":
										applyInfo.agentName = textReader.ReadElementString().Trim();
										break;
									case "ApplyGroupName":
										applyInfo.applyAgentGroupName = textReader.ReadElementString().Trim();
										break;
									case "ApplyState":
										applyInfo.applyState = AgentBar.Str2ApplyState(textReader.ReadElementString().Trim());
										break;
									case "ApplyTime":
										applyInfo.applyTime = textReader.ReadElementString().Trim();
										break;
									case "ApproveTime":
										applyInfo.approveTime = textReader.ReadElementString().Trim();
										break;
									case "ApplyType":
										applyInfo.applyType = textReader.ReadElementString().Trim();
										break;
									case "Reason":
										applyInfo.reason = textReader.ReadElementString().Trim();
										break;
									}
								}
								if (textReader.NodeType == XmlNodeType.EndElement)
								{
									if (textReader.Name == "ApproveInfo")
									{
										dataLst.Add(applyInfo);
										applyInfo = default(Apply_Change_Status);
									}
								}
							}
						}
					}
				}
				textReader.Close();
			}
			catch (Exception e_253)
			{
			}
		}

		private void SaveToXmlTextWriter()
		{
			try
			{
				XmlTextWriter xml = new XmlTextWriter(Environment.GetEnvironmentVariable("APPDATA") + "\\" + Helper.App_Directory_Name + "\\data.xml", Encoding.UTF8);
				xml.Formatting = Formatting.Indented;
				xml.WriteStartDocument();
				xml.WriteStartElement("PLAgent");
				xml.WriteStartElement("LastSigninAgent");
				xml.WriteElementString("LastSigninID", this.agentBar1.AgentID);
				xml.WriteEndElement();
				if (null != this.callHistoryLst)
				{
					xml.WriteStartElement("CallID");
					for (int i = 0; i < this.callHistoryLst.Count; i++)
					{
						xml.WriteElementString("ID", this.callHistoryLst[i].ToString());
					}
					xml.WriteEndElement();
				}
				if (this.agentBar1.AgentRoleAndRight.rights_of_view_agent_group_info)
				{
					if (null != this.agentBar1.ApplyChangeStatusApprovalHistory)
					{
						xml.WriteStartElement("ApproveList");
						for (int i = 0; i < this.agentBar1.ApplyChangeStatusApprovalHistory.Count; i++)
						{
							if (i >= 10)
							{
								break;
							}
							xml.WriteStartElement("ApproveInfo");
							Apply_Change_Status apply_info = default(Apply_Change_Status);
							apply_info = this.agentBar1.ApplyChangeStatusApprovalHistory[i];
							xml.WriteElementString("ApplyID", apply_info.applyAgentID);
							xml.WriteElementString("ApplyName", apply_info.agentName);
							xml.WriteElementString("ApplyGroupName", apply_info.applyAgentGroupName);
							xml.WriteElementString("ApplyState", AgentBar.ApplyStatus2Str(apply_info.applyState));
							xml.WriteElementString("ApplyTime", apply_info.applyTime);
							xml.WriteElementString("ApproveTime", apply_info.approveTime);
							xml.WriteElementString("ApplyType", apply_info.applyType);
							xml.WriteElementString("Reason", apply_info.reason);
							xml.WriteEndElement();
						}
						xml.WriteEndElement();
					}
				}
				xml.WriteEndElement();
				xml.WriteEndDocument();
				xml.Flush();
				xml.Close();
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
				frmMain.Log.Error(ex.Message);
			}
		}

		private void SaveToXML()
		{
			XmlDocument doc = new XmlDocument();
			doc.Load("data.xml");
			XmlElement root = doc.CreateElement("URL_History");
			root.SetAttribute("name", "URL");
			doc.AppendChild(root);
			XmlElement UrlNode = doc.CreateElement("UrlNode");
			for (int i = 0; i < this.tscURL.Items.Count; i++)
			{
				UrlNode.SetAttribute("URL", this.tscURL.Items[i].ToString());
			}
			root.AppendChild(UrlNode);
			doc.Save(Environment.GetEnvironmentVariable("APPDATA") + "\\" + Helper.App_Directory_Name + "\\data.xml");
		}

		private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (this.IsTalking())
			{
				e.Cancel = true;
			}
			else
			{
				if (!this.blnIsSignOut || !this.IsExitWithNoPrompt)
				{
					if (MessageBox.Show("你确定要退出么？", "退出", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No)
					{
						this.IsExitWithNoPrompt = false;
						e.Cancel = true;
						return;
					}
					if (this.IsTalking())
					{
						e.Cancel = true;
						return;
					}
					if (!this.blnIsSignOut)
					{
						if (!this.agentBar1.DoSignOut())
						{
							MessageBox.Show("签出失败！", "签出", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
						}
					}
					this.IsExitWithNoPrompt = true;
					if (!this.agentBar1.DoDisconnect())
					{
						frmMain.Log.Error("断开连接失败！");
					}
					base.Visible = false;
					this.softPhone_close();
				}
				if (this.SignInSuccess)
				{
					this.SaveToXmlTextWriter();
				}
				if (this.agentBar1.DefaultAccessNum != null && this.SignInSuccess)
				{
					Helper.write_config("DefaultAccessNumber", this.agentBar1.DefaultAccessNum);
					Helper.write_config("DefaultAgentStateAfterHangup", ((int)this.agentBar1.AgentStateAfterHangup).ToString());
				}
			}
		}

		private bool IsTalking()
		{
			frmMain.Log.Debug("enter IsTalking().");
			bool result;
			if (this.agentBar1.IsInUse)
			{
				MessageBox.Show("当前电话正在使用中，不能退出系统！", "退出", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				result = true;
			}
			else
			{
				result = false;
			}
			return result;
		}

		private void NewToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.NewWebForm("新标签页", "");
		}

		private void NewWebForm(string pageTitle, string strUrl)
		{
			try
			{
				frmMain.Log.Debug("enter NewWebForm.strUrl=" + strUrl);
				WebBrowser newWebbrowser = new WebBrowser();
				TabPage newpage = new TabPage(pageTitle);
				Process processes = Process.GetCurrentProcess();
				string process_name = processes.ProcessName;
				if (ComFunc.GetProcessInfomationOfHandleCount(process_name) > 10000)
				{
					MessageBox.Show("系统资源消耗过多，请关闭不使用的页面！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				}
				newpage.Controls.Add(newWebbrowser);
				this.tscURL.Text = strUrl;
				if (strUrl.IndexOf("http") != -1)
				{
					this.tabControl1.TabPages.Insert(this.tabControl1.SelectedIndex + 1, newpage);
				}
				else
				{
					this.tabControl1.TabPages.Insert(this.tabControl1.TabPages.Count - 1, newpage);
				}
				if ("" != strUrl)
				{
					newWebbrowser.Navigate(strUrl);
				}
				this.tabControl1.SelectedTab = newpage;
				newWebbrowser.Dock = DockStyle.Fill;
				newWebbrowser.ScriptErrorsSuppressed = true;
				newWebbrowser.AllowWebBrowserDrop = true;
				newWebbrowser.WebBrowserShortcutsEnabled = true;
				newWebbrowser.IsWebBrowserContextMenuEnabled = true;
				newWebbrowser.NewWindow += new CancelEventHandler(this.wb_NewWindow);
				newWebbrowser.Navigating += new WebBrowserNavigatingEventHandler(this.wb_Navigating);
				newWebbrowser.Navigated += new WebBrowserNavigatedEventHandler(this.wb_Navigated);
				newWebbrowser.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(this.wb_DocumentCompleted);
				newWebbrowser.ProgressChanged += new WebBrowserProgressChangedEventHandler(this.wb_ProgressChanged);
				newWebbrowser.PreviewKeyDown += new PreviewKeyDownEventHandler(this.wb_PreviewKeyDown);
			}
			catch (Exception ex)
			{
				string logMsgInfo = string.Concat(new string[]
				{
					"系统发生错误，请联系管理员！",
					string.Format("版本 {0} ", ComFunc.AssemblyVersion),
					"来源:",
					ex.Source,
					",信息:",
					ex.Message,
					",堆栈:",
					ex.StackTrace
				});
				frmMain.Log.Error(logMsgInfo);
				MessageBox.Show("系统发生未知错误！" + string.Format("版本 {0} ", ComFunc.AssemblyVersion), "警告", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			}
		}

		private void CloseToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (this.tabControl1.TabPages.Count > 2)
			{
				Rectangle recTab = default(Rectangle);
				for (int i = 0; i < this.tabControl1.TabCount; i++)
				{
					if (this.tabControl1.GetTabRect(i).Contains(this.pt))
					{
						this.closeTabPages(this.tabControl1.TabPages[i]);
						break;
					}
				}
			}
		}

		private void tsmiLogoff_Click(object sender, EventArgs e)
		{
			this.LogoffWindows();
		}

		private void LogoffWindows()
		{
			frmMain.Log.Debug("enter tsmiLogoff_Click.");
			if (!this.IsTalking())
			{
				if (MessageBox.Show("你确定要注销此用户么？", "注销", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) != DialogResult.No)
				{
					if (!this.IsTalking())
					{
						this.IsManualLogOff = true;
						if (!this.agentBar1.DoSignOut())
						{
							this.reLogin();
						}
						else
						{
							base.Visible = false;
						}
					}
				}
			}
		}

		private void tsmiExit_Click(object sender, EventArgs e)
		{
			this.ExitWindows();
		}

		private void ExitWindows()
		{
			this.IsExitWithNoPrompt = false;
			base.Close();
		}

		private void initForm()
		{
			this.agentBar1.DoSignOut();
			for (int i = 1; i < this.tabControl1.TabPages.Count; i++)
			{
				this.tabControl1.TabPages[i].Dispose();
			}
			((WebBrowser)this.tabControl1.TabPages[0].Controls[0]).Url = null;
		}

		private void tsbAdd_Click(object sender, EventArgs e)
		{
			this.NewWebForm("新标签页", "");
		}

		private void tabControl1_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left && this.tabControl1.TabPages.Count > 2)
			{
				this.closeTabPages(this.tabControl1.SelectedTab);
			}
		}

		private void closeTabPages(TabPage tp)
		{
			for (int i = 0; i < this.tabControl1.TabCount; i++)
			{
				if (this.tabControl1.TabPages[i].Handle == tp.Handle)
				{
					if (i > 0)
					{
						this.tabControl1.SelectedTab = this.tabControl1.TabPages[i - 1];
					}
					else
					{
						this.tabControl1.SelectedTab = this.tabControl1.TabPages[i + 1];
					}
					tp.Dispose();
					this.tsbWBstatus.Text = "";
					this.tspWebProcess.Value = 0;
					this.tspWebProcess.Visible = false;
					break;
				}
			}
		}

		private void tabControl1_MouseUp(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Right)
			{
				for (int i = 0; i < this.tabControl1.TabPages.Count - 1; i++)
				{
					if (this.tabControl1.GetTabRect(i).Contains(e.Location))
					{
						if (this.tabControl1.TabPages.Count <= 2)
						{
							this.cmsWB.Items["tsmi_close"].Enabled = false;
							this.cmsWB.Items["tsmi_close_other"].Enabled = false;
							this.cmsWB.Items["tsmi_close_all"].Enabled = false;
						}
						else
						{
							this.cmsWB.Items["tsmi_close"].Enabled = true;
							this.cmsWB.Items["tsmi_close_other"].Enabled = true;
							this.cmsWB.Items["tsmi_close_all"].Enabled = true;
						}
						this.cmsWB.Show(this.tabControl1, e.X, e.Y);
						this.pt = new Point(e.X, e.Y);
						break;
					}
				}
			}
		}

		private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (this.tabControl1.SelectedTab.Tag != null)
			{
				if (this.tabControl1.SelectedTab.Tag.ToString() == "add")
				{
					this.NewWebForm("新标签页", "");
					return;
				}
			}
			if (this.tabControl1.SelectedTab != null)
			{
				if (((WebBrowser)this.tabControl1.SelectedTab.Controls[0]).Url != null)
				{
					this.tscURL.Text = ((WebBrowser)this.tabControl1.SelectedTab.Controls[0]).Url.ToString();
				}
				else
				{
					this.tscURL.Text = "";
				}
			}
		}

		private void tscURL_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (this.IsSearch)
			{
				this.WebSearch();
			}
		}

		private void tscURL_DropDown(object sender, EventArgs e)
		{
			this.IsSearch = true;
		}

		private void lvwAgent_MouseUp(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Right && this.lvwAgent.SelectedItems.Count >= 1)
			{
				string text = this.lvwAgent.SelectedItems[0].SubItems[2].Text.ToLower();
				if (text != null)
				{
					if (!(text == "idle"))
					{
						if (!(text == "talk") && !(text == "hold"))
						{
							if (!(text == "ring"))
							{
								if (text == "offline")
								{
									this.cmsMonitor.Items["mnuListen"].Enabled = false;
									this.cmsMonitor.Items["mnuInterrupt"].Enabled = false;
									this.cmsMonitor.Items["mnuForceDisconnect"].Enabled = false;
									this.cmsMonitor.Items["mnuIntercept"].Enabled = false;
								}
							}
							else
							{
								this.cmsMonitor.Items["mnuListen"].Enabled = false;
								this.cmsMonitor.Items["mnuInterrupt"].Enabled = false;
								this.cmsMonitor.Items["mnuForceDisconnect"].Enabled = false;
								this.cmsMonitor.Items["mnuIntercept"].Enabled = true;
							}
						}
						else
						{
							this.cmsMonitor.Items["mnuListen"].Enabled = true;
							this.cmsMonitor.Items["mnuInterrupt"].Enabled = true;
							this.cmsMonitor.Items["mnuForceDisconnect"].Enabled = true;
							this.cmsMonitor.Items["mnuIntercept"].Enabled = false;
						}
					}
					else
					{
						this.cmsMonitor.Items["mnuListen"].Enabled = false;
						this.cmsMonitor.Items["mnuInterrupt"].Enabled = false;
						this.cmsMonitor.Items["mnuForceDisconnect"].Enabled = false;
						this.cmsMonitor.Items["mnuIntercept"].Enabled = false;
					}
				}
				this.cmsMonitor.Show(this.lvwAgent, e.X, e.Y);
			}
		}

		private void mnuListen_Click(object sender, EventArgs e)
		{
			if (!this.agentBar1.DoListen(this.lvwAgent.SelectedItems[0].SubItems[6].Text))
			{
				MessageBox.Show("监听失败！", "监听", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			}
		}

		private void mnuInterrupt_Click(object sender, EventArgs e)
		{
			if (!this.agentBar1.DoBargein(this.lvwAgent.SelectedItems[0].SubItems[6].Text))
			{
				MessageBox.Show("插话失败！", "插话", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			}
		}

		private void mnuForceDisconnect_Click(object sender, EventArgs e)
		{
			if (!this.agentBar1.DoForceHangup(this.lvwAgent.SelectedItems[0].SubItems[6].Text))
			{
				MessageBox.Show("强拆失败！", "强拆", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			}
		}

		private void mnuIntercept_Click(object sender, EventArgs e)
		{
			if (!this.agentBar1.DoIntercept(this.lvwAgent.SelectedItems[0].SubItems[6].Text))
			{
				MessageBox.Show("拦截失败！", "拦截", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			}
		}

		private void tsmiSoftPhone_Click(object sender, EventArgs e)
		{
			FrmSoftPhone frmSoftPhone = new FrmSoftPhone();
			frmSoftPhone.Show();
		}

		private void frmMain_FormClosed(object sender, FormClosedEventArgs e)
		{
			base.Visible = false;
		}

		public bool softPhone_close()
		{
			bool result;
			if (Helper.SipSoftPhone_App_Name != "")
			{
				Process[] soft_phone_app_process = Process.GetProcessesByName(this.agentBar1.SoftPhoneAppProcessName);
				if (0 != soft_phone_app_process.Count<Process>())
				{
					if (this.blnIsFirstLogin)
					{
						if (MessageBox.Show("内置软电话程序正在运行中，如果正在通话则电话将被挂断，您是否确定要关闭它？", "退出内置软电话", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
						{
							result = false;
							return result;
						}
					}
					Process[] array = soft_phone_app_process;
					for (int i = 0; i < array.Length; i++)
					{
						Process p = array[i];
						p.Kill();
					}
					Thread.Sleep(3000);
				}
			}
			this.blnIsFirstLogin = false;
			result = true;
			return result;
		}

		public static bool PostMessageApi(IntPtr hwnd, int msg_value, int wP, int lP)
		{
			bool result;
			try
			{
				if (hwnd == IntPtr.Zero || msg_value == 0 || wP == 0 || lP == 0)
				{
					result = false;
				}
				else
				{
					frmMain.PostMessage(hwnd, msg_value, (IntPtr)wP, (IntPtr)lP);
					result = true;
				}
			}
			catch (Exception e_3F)
			{
				result = false;
			}
			return result;
		}

		protected override void WndProc(ref Message msg)
		{
			if (msg.Msg == 274 && (int)msg.WParam == 61536)
			{
				this.IsExitWithNoPrompt = false;
			}
			base.WndProc(ref msg);
		}

		private void 关于ToolStripMenuItem1_Click(object sender, EventArgs e)
		{
			AboutBox frmAbout = new AboutBox();
			frmAbout.Show();
		}

		private void tsmWebsite_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
		{
			string website_name = e.ClickedItem.Text.ToString();
			if (this.agent_website_dic.ContainsKey(website_name))
			{
				string website_url = this.agent_website_dic[e.ClickedItem.Text.ToString()];
				website_url = website_url.Replace("{customernum}", this.g_caller_id);
				website_url = website_url.Replace("{callcenternum}", this.g_called_id);
				website_url = website_url.Replace("{accessnumname}", this.g_access_num_name);
				website_url = website_url.Replace("{taskid}", this.g_task_id);
				website_url = website_url.Replace("{queuenum}", this.g_queue_num);
				website_url = website_url.Replace("{agentnum}", this.g_username);
				website_url = website_url.Replace("{agentpwd}", this.g_password);
				website_url = website_url.Replace("{groupid}", this.g_group_id);
				website_url = website_url.Replace("{agentname}", this.g_agentname);
				website_url = website_url.Replace("{PWD}", this.g_password);
				website_url = website_url.Replace("{MD5PWD}", Helper.Md5.make_md5_str(this.g_password));
				string md5_str = Helper.Md5.make_md5_str(this.g_password) + DateTime.Now.ToString("yyyyMMdd");
				website_url = website_url.Replace("{MD5PWD2}", Helper.Md5.make_md5_str(md5_str));
				string url_encode_group_name = frmMain.UrlEncode(this.g_group_name);
				website_url = website_url.Replace("{agentGroup}", url_encode_group_name);
				website_url = website_url.Replace("{areaid}", this.g_area_id);
				website_url = website_url.Replace("{areaname}", this.g_area_name);
				string signature = this.makeSignature(this.g_caller_id, this.g_username);
				website_url = website_url.Replace("{sign}", signature);
				if (this.tabControl1.SelectedTab.Text == "新标签页")
				{
					WebBrowser currentWebBrowser = (WebBrowser)this.tabControl1.SelectedTab.Controls[0];
					this.tabControl1.SelectedTab.Text = e.ClickedItem.Text.ToString();
					currentWebBrowser.Navigate(website_url);
				}
				else
				{
					this.NewWebForm(website_name, website_url);
				}
			}
		}

		public static string UrlEncode(string str)
		{
			StringBuilder sb = new StringBuilder();
			byte[] byStr = Encoding.UTF8.GetBytes(str);
			for (int i = 0; i < byStr.Length; i++)
			{
				sb.Append("%" + Convert.ToString(byStr[i], 16));
			}
			return sb.ToString();
		}

		private void tsmi_close_other_Click(object sender, EventArgs e)
		{
			if (this.tabControl1.TabPages.Count > 2)
			{
				object found = "no delete";
				Rectangle recTab = default(Rectangle);
				int tabCount = this.tabControl1.TabCount - 1;
				for (int i = 0; i < tabCount; i++)
				{
					if (this.tabControl1.GetTabRect(i).Contains(this.pt))
					{
						this.tabControl1.TabPages[i].Tag = found;
						break;
					}
				}
				int j = 0;
				while (j < this.tabControl1.TabCount)
				{
					if (this.tabControl1.TabPages[j].Tag != found && this.tabControl1.TabPages[j].Name != "add")
					{
						this.closeTabPages(this.tabControl1.TabPages[j]);
					}
					else
					{
						if (this.tabControl1.TabPages[j].Tag == found)
						{
							this.tabControl1.TabPages[j].Tag = null;
						}
						j++;
					}
				}
			}
		}

		private void tsmi_close_all_Click(object sender, EventArgs e)
		{
			if (this.tabControl1.TabPages.Count > 2)
			{
				int i = 0;
				while (i < this.tabControl1.TabCount - 2)
				{
					if (this.tabControl1.TabPages[i].Name != "add")
					{
						this.closeTabPages(this.tabControl1.TabPages[i]);
					}
					else
					{
						i++;
					}
				}
			}
		}

		private void frmMain_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Control)
			{
				this.doChangAgentDefineStatus((char)e.KeyCode);
			}
		}

		private void wb_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
		{
			try
			{
				if (e.Control)
				{
					this.doChangAgentDefineStatus((char)e.KeyCode);
				}
			}
			catch (Exception ex)
			{
				string logMsgInfo = string.Concat(new string[]
				{
					"系统发生未知错误！",
					string.Format("版本 {0} ", ComFunc.AssemblyVersion),
					"来源:",
					ex.Source,
					",信息:",
					ex.Message,
					",堆栈:",
					ex.StackTrace
				});
				frmMain.Log.Error(logMsgInfo);
				MessageBox.Show("系统发生未知错误！" + string.Format("版本 {0} ", ComFunc.AssemblyVersion), "警告", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			}
		}

		private void doChangAgentDefineStatus(char hotKey)
		{
			if (!AgentBar.Agent_Status.AGENT_STATUS_IDLE.Equals(AgentBar.Str2AgentStatus(this.agentBar1.AgentStatus)) && Helper.Hot_Key_Setting_Idle.IndexOf(hotKey, Helper.Hot_Key_Setting_Idle.IndexOf('+')) >= 0)
			{
				this.agentBar1.DoSetAgentDefineStatus(0, 1);
			}
			else if (!AgentBar.Agent_Status.AGENT_STATUS_BUSY.Equals(AgentBar.Str2AgentStatus(this.agentBar1.AgentStatus)) && Helper.Hot_Key_Setting_Busy.IndexOf(hotKey, Helper.Hot_Key_Setting_Idle.IndexOf('+')) >= 0)
			{
				this.agentBar1.DoSetAgentDefineStatus(6, 1);
			}
			else if (!AgentBar.Agent_Status.AGENT_STATUS_LEAVE.Equals(AgentBar.Str2AgentStatus(this.agentBar1.AgentStatus)) && Helper.Hot_Key_Setting_Leave.IndexOf(hotKey, Helper.Hot_Key_Setting_Idle.IndexOf('+')) >= 0)
			{
				if ((DateTime.Now - this.keyPressTime).Seconds >= 1)
				{
					this.agentBar1.DoSetAgentDefineStatus(7, 1);
					this.keyPressTime = DateTime.Now;
				}
			}
			else if (!AgentBar.Agent_Status.AGENT_STATUS_CALL_OUT.Equals(AgentBar.Str2AgentStatus(this.agentBar1.AgentStatus)) && Helper.Hot_Key_Setting_CallOut.IndexOf(hotKey, Helper.Hot_Key_Setting_Idle.IndexOf('+')) >= 0)
			{
				this.agentBar1.DoSetAgentDefineStatus(8, 1);
			}
			else if (!AgentBar.Agent_Status.AGENT_STATUS_MONITOR.Equals(AgentBar.Str2AgentStatus(this.agentBar1.AgentStatus)) && Helper.Hot_Key_Setting_Monitor.IndexOf(hotKey, Helper.Hot_Key_Setting_Monitor.IndexOf('+')) >= 0)
			{
				this.agentBar1.DoSetAgentDefineStatus(9, 1);
			}
		}

		private void tscURL_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (e.KeyChar == '\u0001')
			{
				this.tscURL.SelectAll();
				this.tscURL.Focus();
			}
		}

		private void tmrChkACWTimeOut_Tick(object sender, EventArgs e)
		{
			if (this.blnIsSignOut)
			{
				this.tmrChkACWTimeOut.Stop();
				this.ACWTimeCount = 0;
			}
			else
			{
				if (AgentBar.Agent_Status.AGENT_STATUS_ACW.Equals(AgentBar.Str2AgentStatus(this.agentBar1.AgentStatus)))
				{
					this.ACWTimeCount++;
				}
				else if (this.ACWTimeCount != 0)
				{
					this.ACWTimeCount = 0;
				}
				if (this.ACWTimeCount >= Helper.AutoSetIdleFromACKTime && Helper.AutoSetIdleFromACKTime != 0)
				{
					this.agentBar1.DoSetAgentDefineStatus(0, 1);
					this.ACWTimeCount = 0;
				}
				else if ((long)this.ACWTimeCount >= 3600L)
				{
					this.agentBar1.DoSetAgentDefineStatus(6, 0);
					this.ACWTimeCount = 0;
				}
			}
		}

		private void tsmi_system_Click(object sender, EventArgs e)
		{
			this.newLoginConfig.set_tabSetting(0);
			if (this.newLoginConfig.ShowDialog() == DialogResult.OK)
			{
				if (this.agentBar1.BindExten)
				{
					this.tmrChkACWTimeOut.Start();
					this.ACWTimeCount = 0;
				}
				this.agentBar1.IsMonitorOfflineAgent = this.newLoginConfig.IsMonitorOfflineAgent;
				this.agentBar1.NoAnswerCallsURL = this.newLoginConfig.NoAnswerCallsURL;
				this.agentBar1.SoftphoneAutoAnswer = Helper.IsSoftphoneAutoAnswer;
			}
		}

		private void tsmi_Personal_Click(object sender, EventArgs e)
		{
			this.newPersonalConfig = new frmPersonalConfig(this.agentBar1.AgentID);
			this.newPersonalConfig.DoGetPersonalInfoEvent += new frmPersonalConfig.DoGetPersonalInfoEventHandler(this.newPersonalConfig_DoGetPersonalInfoEvent);
			this.newPersonalConfig.DoSetPersonalInfoEvent += new frmPersonalConfig.DoSetPersonalInfoEventHandler(this.newPersonalConfig_DoSetPersonalInfoEvent);
			this.newPersonalConfig.DoChangePswdEvent += new frmPersonalConfig.DoChangePswdEventHandler(this.newPersonalConfig_DoChangePswdEvent);
			this.agentBar1.GetAgentPersonalInfoEvent += new AgentBar.GetAgentPersonalInfoEventHandler(this.newPersonalConfig.OnEvent_GetAgentPersonalInfo);
			this.agentBar1.SetAgentPersonalInfoEvent += new AgentBar.SetAgentPersonalInfoEventHandler(this.newPersonalConfig.OnEvent_SetAgentPersonalInfo);
			this.agentBar1.ChangePswdEvent += new AgentBar.ChangePswdEventHandler(this.newPersonalConfig.OnEvent_ChangePswd);
			this.newPersonalConfig.ShowDialog();
			this.newPersonalConfig.DoGetPersonalInfoEvent -= new frmPersonalConfig.DoGetPersonalInfoEventHandler(this.newPersonalConfig_DoGetPersonalInfoEvent);
			this.newPersonalConfig.DoSetPersonalInfoEvent -= new frmPersonalConfig.DoSetPersonalInfoEventHandler(this.newPersonalConfig_DoSetPersonalInfoEvent);
			this.newPersonalConfig.DoChangePswdEvent -= new frmPersonalConfig.DoChangePswdEventHandler(this.newPersonalConfig_DoChangePswdEvent);
			this.agentBar1.GetAgentPersonalInfoEvent -= new AgentBar.GetAgentPersonalInfoEventHandler(this.newPersonalConfig.OnEvent_GetAgentPersonalInfo);
			this.agentBar1.SetAgentPersonalInfoEvent -= new AgentBar.SetAgentPersonalInfoEventHandler(this.newPersonalConfig.OnEvent_SetAgentPersonalInfo);
			this.agentBar1.ChangePswdEvent -= new AgentBar.ChangePswdEventHandler(this.newPersonalConfig.OnEvent_ChangePswd);
		}

		private void tmrMarquee_Tick(object sender, EventArgs e)
		{
			this.marqueeOfQueueWaitInfo();
		}

		private void frmMain_Resize(object sender, EventArgs e)
		{
			if (base.WindowState != FormWindowState.Minimized)
			{
				frmMain.LastWindowsState = base.WindowState;
			}
			this.initMarqueePannelAndLabel();
			this.picDown.Left = this.agentBar1.Width - 30;
		}

		private void tsmiLogout_Click(object sender, EventArgs e)
		{
			this.LogoffWindows();
		}

		private void tsmiClose_Click(object sender, EventArgs e)
		{
			this.ExitWindows();
		}

		private void notifyIcon1_MouseUp(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Right)
			{
				this.cmsNotify.Show(Control.MousePosition.X + 12, Control.MousePosition.Y - 20);
			}
			else if (e.Button == MouseButtons.Left)
			{
				this.notifyTimer.Stop();
				this.notifyIcon1.Icon = this.ico_notify;
				base.WindowState = frmMain.LastWindowsState;
				base.Activate();
				base.ShowInTaskbar = true;
			}
		}

		private void frmMain_SizeChanged(object sender, EventArgs e)
		{
			if (base.WindowState == FormWindowState.Minimized)
			{
				base.ShowInTaskbar = true;
				this.notifyIcon1.Visible = true;
			}
		}

		private void frmMain_Activated(object sender, EventArgs e)
		{
			this.notifyTimer.Stop();
			this.notifyIcon1.Icon = this.ico_notify;
		}

		private void newPersonalConfig_DoGetPersonalInfoEvent(string strAgentNum)
		{
			if (!string.IsNullOrEmpty(strAgentNum))
			{
				if (!this.agentBar1.DoGetPersonalInfo(strAgentNum))
				{
					MessageBox.Show("获得坐席个人信息失败！", "获得坐席个人信息", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
				}
			}
		}

		private void newPersonalConfig_DoSetPersonalInfoEvent(string strAgentNum, string strMobile, string strEmail)
		{
			if (!string.IsNullOrEmpty(strAgentNum) && !string.IsNullOrEmpty(strMobile) && !string.IsNullOrEmpty(strEmail))
			{
				if (!this.agentBar1.DoSetPersonalInfo(strAgentNum, strMobile, strEmail))
				{
					MessageBox.Show("修改坐席个人信息失败！", "坐席个人信息", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
				}
			}
		}

		private void newPersonalConfig_DoChangePswdEvent(string strAgentNum, string strOldPswd, string strNewPswd)
		{
			if (!string.IsNullOrEmpty(strAgentNum))
			{
				if (!this.agentBar1.DoChangePswd(strAgentNum, strOldPswd, strNewPswd))
				{
					MessageBox.Show("修改坐席密码失败！", "修改坐席密码", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
				}
			}
		}

		private void picDown_Click(object sender, EventArgs e)
		{
			if (this.urlStretch)
			{
				this.picDown.Image = Resources.Arrow_003;
				this.splitContainer1.SplitterDistance = 86;
			}
			else
			{
				this.picDown.Image = Resources.Arrow_004;
				this.splitContainer1.SplitterDistance = 40;
			}
			this.urlStretch = !this.urlStretch;
		}

		private void tsmi_controls_Click(object sender, EventArgs e)
		{
			if (this.newControlsConfig.ShowDialog() == DialogResult.OK)
			{
				this.agentBar1.controls_info = this.newControlsConfig.controlsInfo;
				this.agentBar1.GetAgentLocationInfo(this.agentBar1.controls_count_current);
				this.agentBar1.ChangeControls();
			}
		}
	}
}
