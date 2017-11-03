using log4net;
using PLAgentBar.Properties;
using PLAgentDll;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace PLAgentBar
{
	public class FrmMonitor : Form
	{
		public delegate void GetAgentsOfGroupEventHandler(string agentGroupName);

		public delegate void GetAgentsOfQueueEventHandler(string queueName);

		public delegate void GetCustomersOfQueueEventHandler(string queueName);

		public delegate void GetDetailCallInfoEventHandler(string targetAgentNum, string call_type, List<Leg_Info_Struct> leg_info, List<Relation_Info_Struct> relation_info);

		public delegate void DoGetDetailCallInfoEventHandler(string targetAgentNum);

		public delegate void GetAgentStateEventHandler();

		public delegate void DoEavesDropEventHandler(string agentNum);

		public delegate void DoWhisperEventHandler(string agentNum);

		public delegate void DoBargeinEventHandler(string agentNum);

		public delegate void DoForceHangupEventHandler(string agentNum);

		public delegate void DoForceChangeStatusEventHandler(string agentNum, string status);

		private enum Event_Type
		{
			INITE_TOOLBAR,
			EAVESDROP_SUCCESS,
			EAVESDROP_FAIL,
			EAVESDROP_CANCEL_SUCCESS,
			EAVESDROP_CANCEL_FAIL,
			EAVESDROP_RING_MYSELF,
			WHISPER_RING_MYSELF,
			BARGEIN_RING_MYSELF,
			FORCE_HANGUP_RING_MYSELF,
			WHISPER_SUCCESS,
			WHISPER_FAIL,
			BARGE_IN_SUCCESS,
			BARGE_IN_FAIL,
			FORCE_HANGUP_SUCCESS,
			FORCE_HANGUP_FAIL
		}

		public class ComparableOrderIDInc : IComparer<Customer_Info_Struct>
		{
			public int Compare(Customer_Info_Struct b1, Customer_Info_Struct b2)
			{
				return b1.orderID.CompareTo(b2.orderID);
			}
		}

		public class ComparableEnterQueueTimeInc : IComparer<Customer_Info_Struct>
		{
			public int Compare(Customer_Info_Struct b1, Customer_Info_Struct b2)
			{
				return b1.enter_queue_time.CompareTo(b2.enter_queue_time);
			}
		}

		private IContainer components = null;

		private ImageList imageList1;

		private SplitContainer splitContainer1;

		private TreeView tvwGroupAndQueue;

		private Timer tmrUpdateTime;

		private ContextMenuStrip cmsMonitor;

		private ToolStripMenuItem mnuListen;

		private ToolStripMenuItem mnuInterrupt;

		private ToolStripMenuItem mnuForceDisconnect;

		private ToolStripMenuItem mnuWhisper;

		private ToolStripMenuItem mnuOnIdleByForce;

		private ToolStripMenuItem mnuOnBusyByForce;

		private ToolStripMenuItem mnuOnleaveByForce;

		private ToolStripSeparator toolStripSeparator1;

		private PictureBox picReturn;

		private ContextMenuStrip cmsViewDetail;

		private ToolStripMenuItem mnuQueue;

		private ToolStripMenuItem mnuAgent;

		private PictureBox picDown;

		private SplitContainer splitContainer2;

		private ToolStrip tsMonitor;

		private ToolStripLabel tslStatus;

		private ToolStripButton tsbListen;

		private ToolStripButton tsbWhisper;

		private ToolStripButton tsbBargein;

		private ToolStripButton tsbForceHangup;

		private ToolStripButton tsbHangUp;

		private ListView lvwMonitorInfo;

		private ColumnHeader columnHeader6;

		private ColumnHeader agentid;

		private ColumnHeader columnHeader2;

		private ColumnHeader columnHeader3;

		private ColumnHeader columnHeader4;

		private ColumnHeader columnHeader1;

		private ColumnHeader columnHeader5;

		private Dictionary<string, int> agent_status_len;

		private Dictionary<string, int> customer_status_len;

		private Dictionary<string, int> queue_max_wait_len;

		private Dictionary<string, string> agent_num_lst_of_queue;

		private List<Customer_Info_Struct> lvw_customer_list;

		private static FrmDetailCallInfo frmDetail;

		private ListViewColumnSorter lvwColumnSorter;

		private ListViewColumnTimeSorter lvwColumnTimeSorter;

		private ListViewColumnSorter_Agent lvwColumnSorter_agent;

		private ListViewColumnSorter_AgentState lvwColumnSorter_agentState;

		private ListViewColumnSorter_Agent_Status lvwColumnSorter_agent_status;

		private Dictionary<string, string> mAgentDefineStatus;

		private string mMyAgentNum;

		private AgentBar agentBar1;

		private Agent_Role_And_Right_Struct mMyRoleAndRight;

		private string Form_Title_Name;

		private List<Agent_Online_Struct> all_agent_monitor_info;

		private Dictionary<string, string> all_group_map;

		private Dictionary<string, string> all_role_map;

		private bool HasLoaded_Agent_Group_List = false;

		private bool HasLoaded_Queue_List = false;

		private bool HasClickQueueStatis = false;

		public FormWindowState LastWindowsState = FormWindowState.Normal;

		public static ILog Log;

        public event FrmMonitor.GetAgentsOfGroupEventHandler GetAgentsOfGroupEvent;

        public event FrmMonitor.GetAgentsOfQueueEventHandler GetAgentsOfQueueEvent;

        public event FrmMonitor.GetCustomersOfQueueEventHandler GetCustomersOfQueueEvent;

        public event FrmMonitor.GetDetailCallInfoEventHandler GetDetailCallInfoEvent;

        public event FrmMonitor.DoGetDetailCallInfoEventHandler DoGetDetailCallInfoEvent;

        public event FrmMonitor.GetAgentStateEventHandler GetAgentStateEvent;

        public event FrmMonitor.DoEavesDropEventHandler DoEavesDropEvent;

        public event FrmMonitor.DoWhisperEventHandler DoWhisperEvent;

        public event FrmMonitor.DoBargeinEventHandler DoBargeinEvent;

        public event FrmMonitor.DoForceHangupEventHandler DoForceHangupEvent;

        public event FrmMonitor.DoForceChangeStatusEventHandler DoForceChangeStatusEvent;

		public Dictionary<string, string> AgentDefineStatus
		{
			set
			{
				this.mAgentDefineStatus = value;
			}
		}

		public string MyAgentNum
		{
			set
			{
				this.mMyAgentNum = value;
			}
		}

		public AgentBar AgentBar
		{
			set
			{
				this.agentBar1 = value;
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
			ComponentResourceManager resources = new ComponentResourceManager(typeof(FrmMonitor));
			TreeNode treeNode = new TreeNode("坐席组A");
			TreeNode treeNode2 = new TreeNode("坐席组B");
			TreeNode treeNode3 = new TreeNode("坐席组C");
			TreeNode treeNode4 = new TreeNode("坐席组D");
			TreeNode treeNode5 = new TreeNode("坐席组E");
			TreeNode treeNode6 = new TreeNode("坐席组", 16, -2, new TreeNode[]
			{
				treeNode,
				treeNode2,
				treeNode3,
				treeNode4,
				treeNode5
			});
			TreeNode treeNode7 = new TreeNode("客户等待队列");
			TreeNode treeNode8 = new TreeNode("坐席监控");
			TreeNode treeNode9 = new TreeNode("队列A", new TreeNode[]
			{
				treeNode7,
				treeNode8
			});
			TreeNode treeNode10 = new TreeNode("队列B", -2, -2);
			TreeNode treeNode11 = new TreeNode("队列C");
			TreeNode treeNode12 = new TreeNode("队列D");
			TreeNode treeNode13 = new TreeNode("队列", 17, -2, new TreeNode[]
			{
				treeNode9,
				treeNode10,
				treeNode11,
				treeNode12
			});
			TreeNode treeNode14 = new TreeNode("队列统计");
			ListViewItem listViewItem = new ListViewItem(new string[]
			{
				"点击查看",
				"1001",
				"张三",
				"通话",
				"00:01:05",
				"坐席组A",
				"坐席"
			}, 0);
			ListViewItem listViewItem2 = new ListViewItem(new string[]
			{
				"",
				"1002",
				"李四",
				"忙碌",
				"00:00:05",
				"坐席组A",
				"组长"
			}, "busy.ico");
			ListViewItem listViewItem3 = new ListViewItem(new string[]
			{
				"",
				"1003",
				"王五",
				"离开",
				"00:01:50",
				"坐席组A",
				"坐席"
			}, "leave.ico");
			this.imageList1 = new ImageList(this.components);
			this.splitContainer1 = new SplitContainer();
			this.tvwGroupAndQueue = new TreeView();
			this.splitContainer2 = new SplitContainer();
			this.tsMonitor = new ToolStrip();
			this.tslStatus = new ToolStripLabel();
			this.tsbListen = new ToolStripButton();
			this.tsbWhisper = new ToolStripButton();
			this.tsbBargein = new ToolStripButton();
			this.tsbForceHangup = new ToolStripButton();
			this.tsbHangUp = new ToolStripButton();
			this.picReturn = new PictureBox();
			this.lvwMonitorInfo = new ListView();
			this.columnHeader6 = new ColumnHeader("(无)");
			this.agentid = new ColumnHeader();
			this.columnHeader2 = new ColumnHeader();
			this.columnHeader3 = new ColumnHeader("(无)");
			this.columnHeader4 = new ColumnHeader();
			this.columnHeader1 = new ColumnHeader();
			this.columnHeader5 = new ColumnHeader();
			this.picDown = new PictureBox();
			this.tmrUpdateTime = new Timer(this.components);
			this.cmsMonitor = new ContextMenuStrip(this.components);
			this.mnuListen = new ToolStripMenuItem();
			this.mnuWhisper = new ToolStripMenuItem();
			this.mnuInterrupt = new ToolStripMenuItem();
			this.mnuForceDisconnect = new ToolStripMenuItem();
			this.toolStripSeparator1 = new ToolStripSeparator();
			this.mnuOnIdleByForce = new ToolStripMenuItem();
			this.mnuOnBusyByForce = new ToolStripMenuItem();
			this.mnuOnleaveByForce = new ToolStripMenuItem();
			this.cmsViewDetail = new ContextMenuStrip(this.components);
			this.mnuQueue = new ToolStripMenuItem();
			this.mnuAgent = new ToolStripMenuItem();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			this.splitContainer2.Panel1.SuspendLayout();
			this.splitContainer2.Panel2.SuspendLayout();
			this.splitContainer2.SuspendLayout();
			this.tsMonitor.SuspendLayout();
			((ISupportInitialize)this.picReturn).BeginInit();
			((ISupportInitialize)this.picDown).BeginInit();
			this.cmsMonitor.SuspendLayout();
			this.cmsViewDetail.SuspendLayout();
			base.SuspendLayout();
			this.imageList1.ImageStream = (ImageListStreamer)resources.GetObject("imageList1.ImageStream");
			this.imageList1.TransparentColor = Color.Transparent;
			this.imageList1.Images.SetKeyName(0, "occupy");
			this.imageList1.Images.SetKeyName(1, "queue");
			this.imageList1.Images.SetKeyName(2, "unknow");
			this.imageList1.Images.SetKeyName(3, "acw");
			this.imageList1.Images.SetKeyName(4, "agentGroup");
			this.imageList1.Images.SetKeyName(5, "queue_agent");
			this.imageList1.Images.SetKeyName(6, "talk");
			this.imageList1.Images.SetKeyName(7, "busy");
			this.imageList1.Images.SetKeyName(8, "group_agent");
			this.imageList1.Images.SetKeyName(9, "leave");
			this.imageList1.Images.SetKeyName(10, "idle");
			this.imageList1.Images.SetKeyName(11, "ring");
			this.imageList1.Images.SetKeyName(12, "try");
			this.imageList1.Images.SetKeyName(13, "wait");
			this.imageList1.Images.SetKeyName(14, "offline");
			this.imageList1.Images.SetKeyName(15, "hold");
			this.imageList1.Images.SetKeyName(16, "queue_statis");
			this.imageList1.Images.SetKeyName(17, "manage");
			this.imageList1.Images.SetKeyName(18, "calling");
			this.imageList1.Images.SetKeyName(19, "restore");
			this.imageList1.Images.SetKeyName(20, "mute");
			this.splitContainer1.Dock = DockStyle.Fill;
			this.splitContainer1.Location = new Point(0, 0);
			this.splitContainer1.Name = "splitContainer1";
			this.splitContainer1.Panel1.Controls.Add(this.tvwGroupAndQueue);
			this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
			this.splitContainer1.Panel2.Controls.Add(this.picDown);
			this.splitContainer1.Size = new Size(780, 432);
			this.splitContainer1.SplitterDistance = 148;
			this.splitContainer1.TabIndex = 0;
			this.tvwGroupAndQueue.Dock = DockStyle.Fill;
			this.tvwGroupAndQueue.ImageIndex = 0;
			this.tvwGroupAndQueue.ImageList = this.imageList1;
			this.tvwGroupAndQueue.Location = new Point(0, 0);
			this.tvwGroupAndQueue.Name = "tvwGroupAndQueue";
			treeNode.Name = "节点3";
			treeNode.SelectedImageIndex = 16;
			treeNode.Text = "坐席组A";
			treeNode2.Name = "节点6";
			treeNode2.Text = "坐席组B";
			treeNode3.Name = "节点7";
			treeNode3.Text = "坐席组C";
			treeNode4.Name = "节点8";
			treeNode4.Text = "坐席组D";
			treeNode5.Name = "节点9";
			treeNode5.Text = "坐席组E";
			treeNode6.ImageIndex = 16;
			treeNode6.Name = "节点0";
			treeNode6.SelectedImageIndex = -2;
			treeNode6.Text = "坐席组";
			treeNode7.Name = "节点2";
			treeNode7.Text = "客户等待队列";
			treeNode8.Name = "节点0";
			treeNode8.Text = "坐席监控";
			treeNode9.Name = "节点10";
			treeNode9.SelectedImageIndex = -2;
			treeNode9.Text = "队列A";
			treeNode10.ImageIndex = -2;
			treeNode10.Name = "节点11";
			treeNode10.SelectedImageIndex = -2;
			treeNode10.Text = "队列B";
			treeNode11.Name = "节点12";
			treeNode11.SelectedImageIndex = -2;
			treeNode11.Text = "队列C";
			treeNode12.Name = "节点13";
			treeNode12.Text = "队列D";
			treeNode13.ImageIndex = 17;
			treeNode13.Name = "节点1";
			treeNode13.SelectedImageIndex = -2;
			treeNode13.Text = "队列";
			treeNode14.Name = "节点0";
			treeNode14.Text = "队列统计";
			this.tvwGroupAndQueue.Nodes.AddRange(new TreeNode[]
			{
				treeNode6,
				treeNode13,
				treeNode14
			});
			this.tvwGroupAndQueue.PathSeparator = "/";
			this.tvwGroupAndQueue.SelectedImageIndex = 2;
			this.tvwGroupAndQueue.Size = new Size(148, 432);
			this.tvwGroupAndQueue.TabIndex = 10;
			this.tvwGroupAndQueue.AfterSelect += new TreeViewEventHandler(this.tvwGroupAndQueue_AfterSelect);
			this.tvwGroupAndQueue.Leave += new EventHandler(this.tvwGroupAndQueue_Leave);
			this.tvwGroupAndQueue.BeforeSelect += new TreeViewCancelEventHandler(this.tvwGroupAndQueue_BeforeSelect);
			this.tvwGroupAndQueue.Click += new EventHandler(this.tvwGroupAndQueue_Click);
			this.splitContainer2.Dock = DockStyle.Fill;
			this.splitContainer2.FixedPanel = FixedPanel.Panel1;
			this.splitContainer2.IsSplitterFixed = true;
			this.splitContainer2.Location = new Point(0, 0);
			this.splitContainer2.Name = "splitContainer2";
			this.splitContainer2.Orientation = Orientation.Horizontal;
			this.splitContainer2.Panel1.Controls.Add(this.tsMonitor);
			this.splitContainer2.Panel1MinSize = 0;
			this.splitContainer2.Panel2.Controls.Add(this.picReturn);
			this.splitContainer2.Panel2.Controls.Add(this.lvwMonitorInfo);
			this.splitContainer2.Size = new Size(628, 432);
			this.splitContainer2.SplitterDistance = 39;
			this.splitContainer2.TabIndex = 19;
			this.tsMonitor.GripStyle = ToolStripGripStyle.Hidden;
			this.tsMonitor.ImageScalingSize = new Size(32, 32);
			this.tsMonitor.Items.AddRange(new ToolStripItem[]
			{
				this.tslStatus,
				this.tsbListen,
				this.tsbWhisper,
				this.tsbBargein,
				this.tsbForceHangup,
				this.tsbHangUp
			});
			this.tsMonitor.LayoutStyle = ToolStripLayoutStyle.HorizontalStackWithOverflow;
			this.tsMonitor.Location = new Point(0, 0);
			this.tsMonitor.Name = "tsMonitor";
			this.tsMonitor.Size = new Size(628, 39);
			this.tsMonitor.TabIndex = 20;
			this.tsMonitor.Text = "toolStrip1";
			this.tslStatus.AutoSize = false;
			this.tslStatus.Image = Resources.info;
			this.tslStatus.ImageAlign = ContentAlignment.MiddleLeft;
			this.tslStatus.Name = "tslStatus";
			this.tslStatus.Size = new Size(140, 36);
			this.tslStatus.Text = "状态:正在通话";
			this.tslStatus.TextAlign = ContentAlignment.MiddleLeft;
			this.tsbListen.AutoSize = false;
			this.tsbListen.DisplayStyle = ToolStripItemDisplayStyle.Image;
			this.tsbListen.Image = Resources.eavesdrop1;
			this.tsbListen.ImageTransparentColor = Color.Magenta;
			this.tsbListen.Name = "tsbListen";
			this.tsbListen.Size = new Size(36, 36);
			this.tsbListen.Text = "监听";
			this.tsbListen.Click += new EventHandler(this.tsbListen_Click);
			this.tsbWhisper.AutoSize = false;
			this.tsbWhisper.DisplayStyle = ToolStripItemDisplayStyle.Image;
			this.tsbWhisper.Image = Resources.whisper;
			this.tsbWhisper.ImageTransparentColor = Color.Magenta;
			this.tsbWhisper.Name = "tsbWhisper";
			this.tsbWhisper.Size = new Size(36, 36);
			this.tsbWhisper.Text = "密语";
			this.tsbWhisper.TextAlign = ContentAlignment.BottomLeft;
			this.tsbWhisper.Click += new EventHandler(this.tsbWhisper_Click);
			this.tsbBargein.DisplayStyle = ToolStripItemDisplayStyle.Image;
			this.tsbBargein.Image = Resources._0004;
			this.tsbBargein.ImageTransparentColor = Color.Magenta;
			this.tsbBargein.Name = "tsbBargein";
			this.tsbBargein.Size = new Size(36, 36);
			this.tsbBargein.Text = "插话";
			this.tsbBargein.Click += new EventHandler(this.tsbBargein_Click);
			this.tsbForceHangup.DisplayStyle = ToolStripItemDisplayStyle.Image;
			this.tsbForceHangup.Image = Resources._0005;
			this.tsbForceHangup.ImageTransparentColor = Color.Magenta;
			this.tsbForceHangup.Name = "tsbForceHangup";
			this.tsbForceHangup.Size = new Size(36, 36);
			this.tsbForceHangup.Text = "强拆";
			this.tsbForceHangup.Click += new EventHandler(this.tsbForceHangup_Click);
			this.tsbHangUp.DisplayStyle = ToolStripItemDisplayStyle.Image;
			this.tsbHangUp.Image = Resources.hangup;
			this.tsbHangUp.ImageTransparentColor = Color.Magenta;
			this.tsbHangUp.Name = "tsbHangUp";
			this.tsbHangUp.Size = new Size(36, 36);
			this.tsbHangUp.Text = "挂断";
			this.tsbHangUp.Click += new EventHandler(this.tsbHangup_Click);
			this.picReturn.BackColor = Color.Transparent;
			this.picReturn.BackgroundImageLayout = ImageLayout.Center;
			this.picReturn.Image = (Image)resources.GetObject("picReturn.Image");
			this.picReturn.Location = new Point(3, 3);
			this.picReturn.Name = "picReturn";
			this.picReturn.Size = new Size(25, 19);
			this.picReturn.SizeMode = PictureBoxSizeMode.StretchImage;
			this.picReturn.TabIndex = 11;
			this.picReturn.TabStop = false;
			this.picReturn.MouseLeave += new EventHandler(this.picReturn_MouseLeave);
			this.picReturn.Click += new EventHandler(this.picReturn_Click);
			this.picReturn.MouseEnter += new EventHandler(this.picReturn_MouseEnter);
			this.lvwMonitorInfo.AllowColumnReorder = true;
			this.lvwMonitorInfo.Columns.AddRange(new ColumnHeader[]
			{
				this.columnHeader6,
				this.agentid,
				this.columnHeader2,
				this.columnHeader3,
				this.columnHeader4,
				this.columnHeader1,
				this.columnHeader5
			});
			this.lvwMonitorInfo.Dock = DockStyle.Fill;
			this.lvwMonitorInfo.FullRowSelect = true;
			listViewItem.StateImageIndex = 0;
			listViewItem2.StateImageIndex = 0;
			listViewItem3.StateImageIndex = 0;
			this.lvwMonitorInfo.Items.AddRange(new ListViewItem[]
			{
				listViewItem,
				listViewItem2,
				listViewItem3
			});
			this.lvwMonitorInfo.Location = new Point(0, 0);
			this.lvwMonitorInfo.MultiSelect = false;
			this.lvwMonitorInfo.Name = "lvwMonitorInfo";
			this.lvwMonitorInfo.ShowItemToolTips = true;
			this.lvwMonitorInfo.Size = new Size(628, 389);
			this.lvwMonitorInfo.SmallImageList = this.imageList1;
			this.lvwMonitorInfo.TabIndex = 11;
			this.lvwMonitorInfo.UseCompatibleStateImageBehavior = false;
			this.lvwMonitorInfo.View = View.Details;
			this.lvwMonitorInfo.Resize += new EventHandler(this.lvwMonitorInfo_Resize);
			this.lvwMonitorInfo.SelectedIndexChanged += new EventHandler(this.lvwMonitorInfo_SelectedIndexChanged);
			this.lvwMonitorInfo.DoubleClick += new EventHandler(this.lvwMonitorInfo_DoubleClick);
			this.lvwMonitorInfo.MouseUp += new MouseEventHandler(this.lvwMonitorInfo_MouseUp);
			this.lvwMonitorInfo.ColumnClick += new ColumnClickEventHandler(this.lvwMonitorInfo_ColumnClick);
			this.columnHeader6.Text = "通话明细";
			this.columnHeader6.Width = 91;
			this.agentid.Text = "工号";
			this.agentid.Width = 76;
			this.columnHeader2.Text = "姓名";
			this.columnHeader3.Text = "状态";
			this.columnHeader4.Text = "时长";
			this.columnHeader1.Text = "坐席组";
			this.columnHeader5.Text = "角色名称";
			this.columnHeader5.Width = 103;
			this.picDown.BackColor = Color.Transparent;
			this.picDown.Image = Resources.Arrow_003;
			this.picDown.Location = new Point(815, 45);
			this.picDown.Name = "picDown";
			this.picDown.Size = new Size(16, 16);
			this.picDown.SizeMode = PictureBoxSizeMode.AutoSize;
			this.picDown.TabIndex = 17;
			this.picDown.TabStop = false;
			this.tmrUpdateTime.Tick += new EventHandler(this.tmrUpdateTime_Tick);
			this.cmsMonitor.Items.AddRange(new ToolStripItem[]
			{
				this.mnuListen,
				this.mnuWhisper,
				this.mnuInterrupt,
				this.mnuForceDisconnect,
				this.toolStripSeparator1,
				this.mnuOnIdleByForce,
				this.mnuOnBusyByForce,
				this.mnuOnleaveByForce
			});
			this.cmsMonitor.Name = "cmsMonitor";
			this.cmsMonitor.Size = new Size(125, 164);
			this.mnuListen.Name = "mnuListen";
			this.mnuListen.Size = new Size(124, 22);
			this.mnuListen.Text = "监听";
			this.mnuListen.Click += new EventHandler(this.mnuListen_Click);
			this.mnuWhisper.Name = "mnuWhisper";
			this.mnuWhisper.Size = new Size(124, 22);
			this.mnuWhisper.Text = "密语";
			this.mnuWhisper.Click += new EventHandler(this.mnuWhisper_Click);
			this.mnuInterrupt.Name = "mnuInterrupt";
			this.mnuInterrupt.Size = new Size(124, 22);
			this.mnuInterrupt.Text = "插话";
			this.mnuInterrupt.Click += new EventHandler(this.mnuInterrupt_Click);
			this.mnuForceDisconnect.Name = "mnuForceDisconnect";
			this.mnuForceDisconnect.Size = new Size(124, 22);
			this.mnuForceDisconnect.Text = "强拆";
			this.mnuForceDisconnect.Click += new EventHandler(this.mnuForceDisconnect_Click);
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new Size(121, 6);
			this.mnuOnIdleByForce.Name = "mnuOnIdleByForce";
			this.mnuOnIdleByForce.Size = new Size(124, 22);
			this.mnuOnIdleByForce.Text = "强制置闲";
			this.mnuOnIdleByForce.Click += new EventHandler(this.mnuOnIdleByForce_Click);
			this.mnuOnBusyByForce.Name = "mnuOnBusyByForce";
			this.mnuOnBusyByForce.Size = new Size(124, 22);
			this.mnuOnBusyByForce.Text = "强制置忙";
			this.mnuOnBusyByForce.Click += new EventHandler(this.mnuOnBusyByForce_Click);
			this.mnuOnleaveByForce.Name = "mnuOnleaveByForce";
			this.mnuOnleaveByForce.Size = new Size(124, 22);
			this.mnuOnleaveByForce.Text = "强制离开";
			this.mnuOnleaveByForce.Click += new EventHandler(this.mnuOnleaveByForce_Click);
			this.cmsViewDetail.Items.AddRange(new ToolStripItem[]
			{
				this.mnuQueue,
				this.mnuAgent
			});
			this.cmsViewDetail.Name = "cmsViewDetail";
			this.cmsViewDetail.Size = new Size(149, 48);
			this.cmsViewDetail.Opening += new CancelEventHandler(this.cmsViewDetail_Opening);
			this.mnuQueue.Name = "mnuQueue";
			this.mnuQueue.Size = new Size(148, 22);
			this.mnuQueue.Text = "查看队列明细";
			this.mnuQueue.Click += new EventHandler(this.mnuQueue_Click);
			this.mnuAgent.Name = "mnuAgent";
			this.mnuAgent.Size = new Size(148, 22);
			this.mnuAgent.Text = "查看坐席明细";
			this.mnuAgent.Click += new EventHandler(this.mnuAgent_Click);
			base.AutoScaleDimensions = new SizeF(6f, 12f);
			base.AutoScaleMode = AutoScaleMode.Font;
			base.ClientSize = new Size(780, 432);
			base.Controls.Add(this.splitContainer1);
			base.Icon = (Icon)resources.GetObject("$this.Icon");
			base.Name = "FrmMonitor";
			base.StartPosition = FormStartPosition.CenterScreen;
			this.Text = "话务监控";
			base.Load += new EventHandler(this.FrmMonitor_Load);
			base.FormClosed += new FormClosedEventHandler(this.FrmMonitor_FormClosed);
			base.FormClosing += new FormClosingEventHandler(this.FrmMonitor_FormClosing);
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel2.ResumeLayout(false);
			this.splitContainer1.Panel2.PerformLayout();
			this.splitContainer1.ResumeLayout(false);
			this.splitContainer2.Panel1.ResumeLayout(false);
			this.splitContainer2.Panel1.PerformLayout();
			this.splitContainer2.Panel2.ResumeLayout(false);
			this.splitContainer2.ResumeLayout(false);
			this.tsMonitor.ResumeLayout(false);
			this.tsMonitor.PerformLayout();
			((ISupportInitialize)this.picReturn).EndInit();
			((ISupportInitialize)this.picDown).EndInit();
			this.cmsMonitor.ResumeLayout(false);
			this.cmsViewDetail.ResumeLayout(false);
			base.ResumeLayout(false);
		}

		public FrmMonitor()
		{
			this.InitializeComponent();
			FrmMonitor.Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
		}

		private void listView1_SelectedIndexChanged(object sender, EventArgs e)
		{
			FrmDetailCallInfo frm = new FrmDetailCallInfo();
			frm.ShowDialog();
		}

		public void Evt_Do_Get_Detail_Call_info(string targetAgentNum)
		{
			if (this.DoGetDetailCallInfoEvent != null)
			{
				this.DoGetDetailCallInfoEvent(targetAgentNum);
			}
		}

		public void Evt_Get_Agent_Group_List(Dictionary<string, string> agent_group_list)
		{
			FrmMonitor.Log.Debug("enter " + MethodBase.GetCurrentMethod().Name);
			if (!this.HasLoaded_Agent_Group_List)
			{
				TreeNode root = new TreeNode("坐席组");
				root.ImageKey = "agentGroup";
				root.ImageKey = (root.SelectedImageKey = "agentGroup");
				foreach (KeyValuePair<string, string> agent_group in agent_group_list)
				{
					TreeNode new_group = new TreeNode(agent_group.Value);
					new_group.Name = agent_group.Key;
					new_group.ImageKey = "group_agent";
					new_group.Tag = "group";
					new_group.ImageKey = (new_group.SelectedImageKey = "agentGroup");
					root.Nodes.Add(new_group);
				}
				this.tvwGroupAndQueue.Nodes.Add(root);
				this.HasLoaded_Agent_Group_List = true;
			}
		}

		public void Evt_Get_Agents_Monitor_Info(List<Agent_Online_Struct> agent_info, string current_time, Dictionary<string, string> group_map, Dictionary<string, string> role_map)
		{
			FrmMonitor.Log.Debug("enter " + MethodBase.GetCurrentMethod().Name);
			this.lvwMonitorInfo.Clear();
			this.initMonitorAgentInfo();
			this.agent_status_len = new Dictionary<string, int>();
			this.all_agent_monitor_info = agent_info;
			this.all_group_map = group_map;
			this.all_role_map = role_map;
			for (int i = 0; i < agent_info.Count; i++)
			{
				if (agent_info[i].flag == "-99")
				{
					AgentBar.Agent_Status agent_status = AgentBar.Str2AgentStatus(agent_info[i].agentStatus);
					ListViewItem new_lvwItem = new ListViewItem(AgentBar.AgentStatus2Str(agent_status));
					new_lvwItem.ImageKey = "occupy";
					new_lvwItem.SubItems.Add(agent_info[i].agentNum);
					new_lvwItem.SubItems.Add("");
					new_lvwItem.SubItems.Add("");
					new_lvwItem.SubItems.Add("");
					new_lvwItem.SubItems.Add("");
					new_lvwItem.SubItems.Add("");
				}
				else if (!(agent_info[i].flag == "-1") || this.agentBar1.IsMonitorOfflineAgent)
				{
					AgentBar.Agent_Status agent_status = AgentBar.Str2AgentStatus(agent_info[i].agentStatus);
					ListViewItem new_lvwItem = new ListViewItem();
					string agent_status_name = AgentBar.AgentStatus2Str(agent_status);
					if ("" == agent_status_name)
					{
						if (this.mAgentDefineStatus.ContainsKey(agent_info[i].agentStatus))
						{
							agent_status_name = this.mAgentDefineStatus[agent_info[i].agentStatus];
							new_lvwItem.Text = agent_status_name;
							new_lvwItem.ImageKey = "busy";
						}
						else
						{
							new_lvwItem.Text = "未知";
							new_lvwItem.ImageKey = "busy";
						}
					}
					else
					{
						new_lvwItem.Text = AgentBar.AgentStatus2Str(agent_status);
						new_lvwItem.ImageKey = AgentBar.IcoName_AgentStatus2Str(agent_status);
					}
					if (AgentBar.ChkIsTalking(agent_info[i].agentStatus) == 0)
					{
						new_lvwItem.ToolTipText = "点击查看详情";
					}
					new_lvwItem.SubItems.Add(agent_info[i].agentNum);
					new_lvwItem.SubItems.Add(agent_info[i].agentName);
					if (agent_info[i].flag == "0")
					{
						new_lvwItem.SubItems.Add(ComFunc.countStatusLength(current_time, agent_info[i].status_change_time));
						if (agent_info[i].agentStatus == "2")
						{
							this.agent_status_len.Add(agent_info[i].agentNum, ComFunc.get_time_interval_sec(current_time, agent_info[i].start_talking_time));
						}
						else
						{
							this.agent_status_len.Add(agent_info[i].agentNum, ComFunc.get_time_interval_sec(current_time, agent_info[i].status_change_time));
						}
						new_lvwItem.SubItems[3].Tag = ComFunc.get_time_interval_sec(current_time, agent_info[i].status_change_time);
					}
					else if (agent_info[i].flag == "-1")
					{
						new_lvwItem.SubItems.Add("");
					}
					if (group_map.ContainsKey(agent_info[i].agentgroup_num))
					{
						new_lvwItem.SubItems.Add(group_map[agent_info[i].agentgroup_num]);
					}
					else
					{
						new_lvwItem.SubItems.Add("未知");
					}
					if (role_map.ContainsKey(agent_info[i].roleNum))
					{
						new_lvwItem.SubItems.Add(role_map[agent_info[i].roleNum]);
					}
					else
					{
						new_lvwItem.SubItems.Add("未知");
					}
					new_lvwItem.SubItems.Add(agent_info[i].customer_enter_channel);
					this.lvwMonitorInfo.Items.Add(new_lvwItem);
				}
			}
			this.lvwMonitorInfo.ListViewItemSorter = this.lvwColumnSorter_agent_status;
			this.lvwMonitorInfo.Sort();
		}

		public void Evt_Get_Customer_Monitor_Info(string queueNumLstStr, string current_time, List<Customer_Info_Struct> customer_info)
		{
			FrmMonitor.Log.Debug("enter " + MethodBase.GetCurrentMethod().Name);
			this.lvwMonitorInfo.Clear();
			this.initMonitorCustomerInfo();
			this.customer_status_len = new Dictionary<string, int>();
			customer_info.Sort(new FrmMonitor.ComparableEnterQueueTimeInc());
			for (int i = 0; i < customer_info.Count; i++)
			{
				this.Evt_Add_Customer_To_Queue(customer_info[i].call_type, customer_info[i].callcenter_num, customer_info[i].customer_num, customer_info[i].customer_status, customer_info[i].customer_type, customer_info[i].customer_uuid, customer_info[i].enter_queue_time, customer_info[i].exclusive_agent_num, customer_info[i].exclusive_queue_num, customer_info[i].queue_num, current_time, customer_info[i].queue_name, customer_info[i].queue_customer_amount, "", "", customer_info[i].customer_enter_channel);
			}
		}

		public void Evt_Get_Queue_Statis_List(string queueNumLstStr, string current_time, List<Queue_Statis_Struct> queue_statis_lst)
		{
			FrmMonitor.Log.Debug("enter " + MethodBase.GetCurrentMethod().Name);
			if (!(this.lvwMonitorInfo.AccessibleName != "queue_statis_info"))
			{
				if (!(this.tvwGroupAndQueue.SelectedNode.Text != "队列统计"))
				{
					if (null == this.queue_max_wait_len)
					{
						this.queue_max_wait_len = new Dictionary<string, int>();
					}
					try
					{
						for (int i = 0; i < queue_statis_lst.Count; i++)
						{
							if (!this.agent_num_lst_of_queue.ContainsKey(queue_statis_lst[i].queue_num))
							{
								this.agent_num_lst_of_queue.Add(queue_statis_lst[i].queue_num, queue_statis_lst[i].agentNumLstStr);
							}
							ListViewItem new_lvwItem = new ListViewItem(queue_statis_lst[i].queue_name);
							new_lvwItem.Name = queue_statis_lst[i].queue_num;
							new_lvwItem.ImageKey = "queue_statis";
							new_lvwItem.SubItems.Add(queue_statis_lst[i].queue_num);
							new_lvwItem.SubItems.Add(queue_statis_lst[i].queue_wait_people_amount);
							if ("0" == queue_statis_lst[i].early_queue_enter_time)
							{
								new_lvwItem.SubItems.Add(ComFunc.countStatusLength(current_time, current_time));
								if (this.queue_max_wait_len.ContainsKey(queue_statis_lst[i].queue_num))
								{
									int tempTime = this.queue_max_wait_len[queue_statis_lst[i].queue_num];
									tempTime = -1;
									this.queue_max_wait_len[queue_statis_lst[i].queue_num] = tempTime;
								}
								else
								{
									this.queue_max_wait_len.Add(queue_statis_lst[i].queue_num, -1);
								}
							}
							else
							{
								new_lvwItem.SubItems.Add(ComFunc.countStatusLength(current_time, queue_statis_lst[i].early_queue_enter_time));
								if (this.queue_max_wait_len.ContainsKey(queue_statis_lst[i].queue_num))
								{
									int tempTime = this.queue_max_wait_len[queue_statis_lst[i].queue_num];
									tempTime = ComFunc.get_time_interval_sec(current_time, queue_statis_lst[i].early_queue_enter_time);
									this.queue_max_wait_len[queue_statis_lst[i].queue_num] = tempTime;
								}
								else
								{
									this.queue_max_wait_len.Add(queue_statis_lst[i].queue_num, ComFunc.get_time_interval_sec(current_time, queue_statis_lst[i].early_queue_enter_time));
								}
							}
							new_lvwItem.SubItems.Add(queue_statis_lst[i].agent_idle_amount);
							int talking_amount = Convert.ToInt32(queue_statis_lst[i].agent_talking_amount) + Convert.ToInt32(queue_statis_lst[i].agent_hold_amount) + Convert.ToInt32(queue_statis_lst[i].agent_mute_amount);
							new_lvwItem.SubItems.Add(talking_amount.ToString());
							int agent_leave_amount = Convert.ToInt32(queue_statis_lst[i].agent_leave_amount) + Convert.ToInt32(queue_statis_lst[i].agent_busy_amount);
							new_lvwItem.SubItems.Add(agent_leave_amount.ToString());
							new_lvwItem.SubItems.Add(queue_statis_lst[i].agent_offline_amount);
							this.lvwMonitorInfo.Items.Add(new_lvwItem);
						}
					}
					catch (Exception e)
					{
						FrmMonitor.Log.Error(string.Concat(new string[]
						{
							e.Source,
							",信息:",
							e.Message,
							",堆栈:",
							e.StackTrace
						}));
					}
				}
			}
		}

		private void update_queue_statis_info(string queueNumLstStr, int wait_people_amount, int agent_idle_amount, int agent_talking_amount, int agent_leave_amount, int agent_offline_amount)
		{
			FrmMonitor.Log.Debug("enter " + MethodBase.GetCurrentMethod().Name);
			if (!(this.lvwMonitorInfo.AccessibleName != "queue_statis_info") && !(queueNumLstStr == ""))
			{
				for (int i = 0; i < this.lvwMonitorInfo.Items.Count; i++)
				{
					if (queueNumLstStr.IndexOf(this.lvwMonitorInfo.Items[i].Name) != -1)
					{
						if (wait_people_amount != 0)
						{
							this.lvwMonitorInfo.Items[i].SubItems[2].Text = Convert.ToString(Convert.ToInt32(this.lvwMonitorInfo.Items[i].SubItems[2].Text) + wait_people_amount);
						}
						if (this.queue_max_wait_len.ContainsKey(this.lvwMonitorInfo.Items[i].Name))
						{
							int tempTime = this.queue_max_wait_len[this.lvwMonitorInfo.Items[i].Name];
							this.lvwMonitorInfo.Items[i].SubItems[3].Tag = tempTime;
						}
						else
						{
							this.lvwMonitorInfo.Items[i].SubItems[3].Tag = 0;
						}
						if (agent_idle_amount != 0)
						{
							this.lvwMonitorInfo.Items[i].SubItems[4].Text = Convert.ToString(Convert.ToInt32(this.lvwMonitorInfo.Items[i].SubItems[4].Text) + agent_idle_amount);
						}
						if (agent_talking_amount != 0)
						{
							this.lvwMonitorInfo.Items[i].SubItems[5].Text = Convert.ToString(Convert.ToInt32(this.lvwMonitorInfo.Items[i].SubItems[5].Text) + agent_talking_amount);
						}
						if (agent_leave_amount != 0)
						{
							this.lvwMonitorInfo.Items[i].SubItems[6].Text = Convert.ToString(Convert.ToInt32(this.lvwMonitorInfo.Items[i].SubItems[6].Text) + agent_leave_amount);
						}
						if (agent_offline_amount != 0)
						{
							this.lvwMonitorInfo.Items[i].SubItems[7].Text = Convert.ToString(Convert.ToInt32(this.lvwMonitorInfo.Items[i].SubItems[7].Text) + agent_offline_amount);
						}
					}
				}
			}
		}

		public void Evt_Agent_Status_Change(string status_change_agent_num, string status_change_before, string status_change_after, bool is_bind_exten, string customer_enter_channel, string current_time, string start_talking_time)
		{
			FrmMonitor.Log.Debug("enter " + MethodBase.GetCurrentMethod().Name);
			bool is_agent_find = false;
			if (this.lvwMonitorInfo.AccessibleName == "monitor_agent_info")
			{
				for (int i = 0; i < this.lvwMonitorInfo.Items.Count; i++)
				{
					if (this.lvwMonitorInfo.Items[i].SubItems[1].Text == status_change_agent_num)
					{
						AgentBar.Agent_Status agent_status = AgentBar.Str2AgentStatus(status_change_after);
						if (AgentBar.ChkIsTalking(status_change_after) == 0)
						{
							if (FrmMonitor.frmDetail != null)
							{
								if (FrmMonitor.frmDetail.AgentNum == status_change_agent_num)
								{
									if (FrmMonitor.frmDetail != null && !FrmMonitor.frmDetail.IsDisposed)
									{
										FrmMonitor.frmDetail.Close();
									}
									this.lvwMonitorInfo.Items[i].ToolTipText = "";
								}
							}
						}
						else
						{
							this.lvwMonitorInfo.Items[i].ToolTipText = "双击查看通话详情";
						}
						string agent_status_name = AgentBar.AgentStatus2Str(agent_status);
						if ("" == agent_status_name)
						{
							if (this.mAgentDefineStatus.ContainsKey(status_change_after))
							{
								agent_status_name = this.mAgentDefineStatus[status_change_after];
								this.lvwMonitorInfo.Items[i].Text = agent_status_name;
								this.lvwMonitorInfo.Items[i].ImageKey = "busy";
							}
							else
							{
								this.lvwMonitorInfo.Items[i].Text = "未知";
								this.lvwMonitorInfo.Items[i].ImageKey = "busy";
							}
						}
						else
						{
							this.lvwMonitorInfo.Items[i].Text = AgentBar.AgentStatus2Str(agent_status);
							this.lvwMonitorInfo.Items[i].ImageKey = AgentBar.IcoName_AgentStatus2Str(agent_status);
						}
						this.lvwMonitorInfo.Items[i].SubItems[6].Text = customer_enter_channel;
						if (status_change_after == "2")
						{
							this.agent_status_len[status_change_agent_num] = ComFunc.get_time_interval_sec(current_time, start_talking_time);
						}
						else
						{
							this.agent_status_len[status_change_agent_num] = 0;
						}
						this.lvwMonitorInfo.Items[i].SubItems[3].Tag = this.agent_status_len[status_change_agent_num];
						is_agent_find = true;
						if (agent_status == AgentBar.Agent_Status.AGENT_STATUS_OFFLINE && !this.agentBar1.IsMonitorOfflineAgent)
						{
							this.lvwMonitorInfo.Items[i].Remove();
						}
						break;
					}
				}
				if (!is_agent_find)
				{
					for (int i = 0; i < this.all_agent_monitor_info.Count; i++)
					{
						if (this.all_agent_monitor_info[i].agentNum == status_change_agent_num)
						{
							AgentBar.Agent_Status agent_status = AgentBar.Str2AgentStatus(status_change_after);
							if (agent_status != AgentBar.Agent_Status.AGENT_STATUS_OFFLINE || this.agentBar1.IsMonitorOfflineAgent)
							{
								ListViewItem new_lvwItem = new ListViewItem();
								string agent_status_name = AgentBar.AgentStatus2Str(agent_status);
								if ("" == agent_status_name)
								{
									if (this.mAgentDefineStatus.ContainsKey(agent_status_name))
									{
										agent_status_name = this.mAgentDefineStatus[agent_status_name];
										new_lvwItem.Text = agent_status_name;
										new_lvwItem.ImageKey = "busy";
									}
									else
									{
										new_lvwItem.Text = "未知";
										new_lvwItem.ImageKey = "busy";
									}
								}
								else
								{
									new_lvwItem.Text = AgentBar.AgentStatus2Str(agent_status);
									new_lvwItem.ImageKey = AgentBar.IcoName_AgentStatus2Str(agent_status);
								}
								if (AgentBar.ChkIsTalking(status_change_after) == 0)
								{
									new_lvwItem.ToolTipText = "双击查看详情";
								}
								new_lvwItem.SubItems.Add(this.all_agent_monitor_info[i].agentNum);
								new_lvwItem.SubItems.Add(this.all_agent_monitor_info[i].agentName);
								new_lvwItem.SubItems.Add("0秒");
								new_lvwItem.SubItems[3].Tag = 0;
								this.agent_status_len[status_change_agent_num] = 0;
								if (this.all_group_map.ContainsKey(this.all_agent_monitor_info[i].agentgroup_num))
								{
									new_lvwItem.SubItems.Add(this.all_group_map[this.all_agent_monitor_info[i].agentgroup_num]);
								}
								else
								{
									new_lvwItem.SubItems.Add("未知");
								}
								if (this.all_role_map.ContainsKey(this.all_agent_monitor_info[i].roleNum))
								{
									new_lvwItem.SubItems.Add(this.all_role_map[this.all_agent_monitor_info[i].roleNum]);
								}
								else
								{
									new_lvwItem.SubItems.Add("未知");
								}
								new_lvwItem.SubItems.Add(this.all_agent_monitor_info[i].customer_enter_channel);
								this.lvwMonitorInfo.Items.Add(new_lvwItem);
								this.lvwMonitorInfo.Sort();
								break;
							}
						}
					}
				}
			}
			else if (this.lvwMonitorInfo.AccessibleName == "queue_statis_info")
			{
				string tmpQueueNumLstStr = "";
				foreach (KeyValuePair<string, string> kv in this.agent_num_lst_of_queue)
				{
					if (kv.Value != null)
					{
						if (kv.Value.IndexOf(status_change_agent_num) != -1)
						{
							tmpQueueNumLstStr += kv.Key;
						}
					}
				}
				if (!(tmpQueueNumLstStr == ""))
				{
					AgentBar.Agent_Status agent_status_before = AgentBar.Str2AgentStatus(status_change_before);
					if (AgentBar.ChkIsTalking(status_change_before) == 0)
					{
						this.update_queue_statis_info(tmpQueueNumLstStr, 0, 0, -1, 0, 0);
					}
					else if (agent_status_before == AgentBar.Agent_Status.AGENT_STATUS_IDLE && is_bind_exten)
					{
						this.update_queue_statis_info(tmpQueueNumLstStr, 0, -1, 0, 0, 0);
					}
					else if (agent_status_before == AgentBar.Agent_Status.AGENT_STATUS_OFFLINE)
					{
						this.update_queue_statis_info(tmpQueueNumLstStr, 0, 0, 0, 0, -1);
					}
					else if (agent_status_before == AgentBar.Agent_Status.AGENT_STATUS_LEAVE && is_bind_exten)
					{
						this.update_queue_statis_info(tmpQueueNumLstStr, 0, 0, 0, -1, 0);
					}
					else if (Convert.ToInt32(agent_status_before) >= 100)
					{
						this.update_queue_statis_info(tmpQueueNumLstStr, 0, 0, 0, -1, 0);
					}
					AgentBar.Agent_Status agent_status_after = AgentBar.Str2AgentStatus(status_change_after);
					if (AgentBar.ChkIsTalking(status_change_after) == 0)
					{
						this.update_queue_statis_info(tmpQueueNumLstStr, 0, 0, 1, 0, 0);
					}
					else if (agent_status_after == AgentBar.Agent_Status.AGENT_STATUS_IDLE && is_bind_exten)
					{
						this.update_queue_statis_info(tmpQueueNumLstStr, 0, 1, 0, 0, 0);
					}
					else if (agent_status_after == AgentBar.Agent_Status.AGENT_STATUS_OFFLINE)
					{
						this.update_queue_statis_info(tmpQueueNumLstStr, 0, 0, 0, 0, 1);
					}
					else if (agent_status_after == AgentBar.Agent_Status.AGENT_STATUS_LEAVE && is_bind_exten)
					{
						this.update_queue_statis_info(tmpQueueNumLstStr, 0, 0, 0, 1, 0);
					}
					else if (Convert.ToInt32(agent_status_after) >= 100)
					{
						this.update_queue_statis_info(tmpQueueNumLstStr, 0, 0, 0, 1, 0);
					}
				}
			}
		}

		private void initMonitorAgentInfo()
		{
			this.lvwMonitorInfo.AccessibleName = "monitor_agent_info";
			this.lvwMonitorInfo.FullRowSelect = true;
			if (this.picReturn.Visible)
			{
				this.lvwMonitorInfo.Columns.Add("     状态");
			}
			else
			{
				this.lvwMonitorInfo.Columns.Add("状态");
			}
			this.lvwMonitorInfo.Columns[0].Width = 100;
			this.lvwMonitorInfo.Columns.Add("工号");
			this.lvwMonitorInfo.Columns[1].Width = 50;
			this.lvwMonitorInfo.Columns.Add("姓名");
			this.lvwMonitorInfo.Columns[2].Width = 100;
			this.lvwMonitorInfo.Columns.Add("时长");
			this.lvwMonitorInfo.Columns[3].Width = 120;
			this.lvwMonitorInfo.Columns.Add("坐席组");
			this.lvwMonitorInfo.Columns[4].Width = 100;
			this.lvwMonitorInfo.Columns.Add("角色");
			this.lvwMonitorInfo.Columns[5].Width = 100;
			this.lvwMonitorInfo.Columns.Add("进线通道");
			this.lvwMonitorInfo.Columns[6].Width = 200;
			this.lvwMonitorInfo.Items.Clear();
			this.splitContainer2.Panel1Collapsed = false;
			this.update_Toolbar_UI(FrmMonitor.Event_Type.INITE_TOOLBAR, "");
		}

		private void initMonitorCustomerInfo()
		{
			this.lvwMonitorInfo.AccessibleName = "monitor_customer_info";
			this.lvwMonitorInfo.FullRowSelect = true;
			if (this.picReturn.Visible)
			{
				this.lvwMonitorInfo.Columns.Add("     状态");
			}
			else
			{
				this.lvwMonitorInfo.Columns.Add("状态");
			}
			this.lvwMonitorInfo.Columns[0].Width = 100;
			this.lvwMonitorInfo.Columns.Add("序号");
			this.lvwMonitorInfo.Columns[1].Width = 50;
			this.lvwMonitorInfo.Columns.Add("主叫号码");
			this.lvwMonitorInfo.Columns[2].Width = 100;
			this.lvwMonitorInfo.Columns.Add("被叫号码");
			this.lvwMonitorInfo.Columns[3].Width = 100;
			this.lvwMonitorInfo.Columns.Add("呼叫类型");
			this.lvwMonitorInfo.Columns[4].Width = 100;
			this.lvwMonitorInfo.Columns.Add("客户类型");
			this.lvwMonitorInfo.Columns[5].Width = 100;
			this.lvwMonitorInfo.Columns.Add("专属坐席");
			this.lvwMonitorInfo.Columns[6].Width = 100;
			this.lvwMonitorInfo.Columns.Add("队列号");
			this.lvwMonitorInfo.Columns[7].Width = 60;
			this.lvwMonitorInfo.Columns.Add("等待时长");
			this.lvwMonitorInfo.Columns[8].Width = 120;
			this.lvwMonitorInfo.Columns.Add("进线通道");
			this.lvwMonitorInfo.Columns[9].Width = 120;
			this.lvwMonitorInfo.Items.Clear();
			this.splitContainer2.Panel1Collapsed = true;
		}

		private void initMonitorQueueStatisInfo()
		{
			this.lvwMonitorInfo.AccessibleName = "queue_statis_info";
			this.lvwMonitorInfo.FullRowSelect = true;
			this.lvwMonitorInfo.Columns.Add("队列名称");
			this.lvwMonitorInfo.Columns[0].Width = 100;
			this.lvwMonitorInfo.Columns.Add("队列号码");
			this.lvwMonitorInfo.Columns[1].Width = 100;
			this.lvwMonitorInfo.Columns.Add("排队人数");
			this.lvwMonitorInfo.Columns[2].Width = 100;
			this.lvwMonitorInfo.Columns.Add("最长等待时间");
			this.lvwMonitorInfo.Columns[3].Width = 100;
			this.lvwMonitorInfo.Columns.Add("坐席空闲数");
			this.lvwMonitorInfo.Columns[4].Width = 100;
			this.lvwMonitorInfo.Columns.Add("坐席通话数");
			this.lvwMonitorInfo.Columns[5].Width = 100;
			this.lvwMonitorInfo.Columns.Add("坐席离开数");
			this.lvwMonitorInfo.Columns[6].Width = 100;
			this.lvwMonitorInfo.Columns.Add("坐席不在线数");
			this.lvwMonitorInfo.Columns[7].Width = 100;
			this.lvwMonitorInfo.Items.Clear();
			this.splitContainer2.Panel1Collapsed = true;
		}

		public void Evt_Get_Agents_Of_AgentGroup(string agent_list, string agent_group_num)
		{
			if (agent_list == "" || agent_list == null)
			{
				this.lvwMonitorInfo.Clear();
				this.initMonitorAgentInfo();
			}
		}

		public void Evt_Get_Agents_Of_Queue(string agent_list, string queue_num)
		{
			if (agent_list == "" || agent_list == null)
			{
				this.lvwMonitorInfo.Clear();
				this.initMonitorAgentInfo();
			}
		}

		public void Evt_Get_Agent_Queue_List(Dictionary<string, string> queue_list)
		{
			FrmMonitor.Log.Debug("enter " + MethodBase.GetCurrentMethod().Name);
			if (this.agentBar1.AgentRoleAndRight.controled_queue_lst == null || this.agentBar1.AgentRoleAndRight.controled_queue_lst == "")
			{
				this.HasClickQueueStatis = false;
			}
			else
			{
				string[] sArray = this.agentBar1.AgentRoleAndRight.controled_queue_lst.Split(new char[]
				{
					','
				});
				Dictionary<string, string> new_queue_list = new Dictionary<string, string>();
				string[] array = sArray;
				for (int j = 0; j < array.Length; j++)
				{
					string i = array[j];
					if (queue_list.ContainsKey(i))
					{
						new_queue_list.Add(i, queue_list[i]);
					}
				}
				if (this.HasClickQueueStatis)
				{
					this.show_queue_statis_info(new_queue_list);
					this.HasClickQueueStatis = false;
				}
				if (!this.HasLoaded_Queue_List)
				{
					if (this.agentBar1.AgentRoleAndRight.rights_of_view_queue_info)
					{
						foreach (TreeNode node in this.tvwGroupAndQueue.Nodes)
						{
							if (node.Text == "队列" && node.Parent == null)
							{
								return;
							}
						}
						TreeNode root = new TreeNode("队列");
						root.ImageKey = (root.SelectedImageKey = "queue_agent");
						root.Tag = "queue";
						foreach (KeyValuePair<string, string> queue in new_queue_list)
						{
							TreeNode new_queue = new TreeNode(queue.Value);
							new_queue.Name = queue.Key;
							new_queue.ToolTipText = queue.Key;
							new_queue.ImageKey = "queue_agent";
							new_queue.Tag = "queue";
							new_queue.ImageKey = (new_queue.SelectedImageKey = "queue_agent");
							TreeNode new_queue_customer = new TreeNode("客户排队监控");
							new_queue_customer.ImageKey = "wait";
							new_queue_customer.Tag = "queue";
							new_queue_customer.ImageKey = (new_queue_customer.SelectedImageKey = "wait");
							TreeNode new_queue_agent = new TreeNode("坐席监控");
							new_queue_agent.ImageKey = "idle";
							new_queue_agent.Tag = "queue";
							new_queue_agent.ImageKey = (new_queue_agent.SelectedImageKey = "idle");
							new_queue.Nodes.Add(new_queue_customer);
							new_queue.Nodes.Add(new_queue_agent);
							root.Nodes.Add(new_queue);
						}
						this.tvwGroupAndQueue.Nodes.Add(root);
						TreeNode root_queue_statis = new TreeNode("队列统计");
						root_queue_statis.ImageKey = (root_queue_statis.SelectedImageKey = "queue_statis");
						root_queue_statis.Name = "queue_statis";
						root_queue_statis.Tag = "queue_statis";
						this.tvwGroupAndQueue.Nodes.Add(root_queue_statis);
						this.HasLoaded_Queue_List = true;
					}
				}
			}
		}

		public void Evt_Add_Customer_To_Queue(string call_type, string callcenter_num, string customer_num, string customer_status, string customer_type, string customer_uuid, string enter_queue_time, string exclusive_agent_num, string exclusive_queue_num, string queue_num, string current_time, string queue_name, string queue_customer_amount, string early_queue_enter_time, string early_queue_enter_time_all, string customer_enter_channel)
		{
			FrmMonitor.Log.Debug("enter " + MethodBase.GetCurrentMethod().Name + ",customer_enter_channel:" + customer_enter_channel);
			if (!(this.lvwMonitorInfo.AccessibleName != "monitor_customer_info") || !(this.lvwMonitorInfo.AccessibleName != "queue_statis_info"))
			{
				try
				{
					if (this.lvwMonitorInfo.AccessibleName == "monitor_customer_info")
					{
						if (this.tvwGroupAndQueue.SelectedNode != null)
						{
							if (this.tvwGroupAndQueue.SelectedNode.Parent != null)
							{
								if (this.tvwGroupAndQueue.SelectedNode.Parent.Name != queue_num)
								{
									return;
								}
							}
						}
						else if (this.tvwGroupAndQueue.Tag.ToString() != queue_num)
						{
							return;
						}
						this.customer_status_len.Add(customer_uuid, ComFunc.get_time_interval_sec(current_time, enter_queue_time));
						ListViewItem new_lvwItem = new ListViewItem(AgentBar.CustomerStatus2Chinese(customer_status));
						new_lvwItem.Name = customer_uuid;
						new_lvwItem.ImageKey = AgentBar.IcoName_CustomerStatus2Str(customer_status);
						new_lvwItem.SubItems.Add("");
						new_lvwItem.SubItems[1].Tag = this.formula_define_customer_order(customer_type, enter_queue_time);
						new_lvwItem.SubItems.Add(customer_num);
						new_lvwItem.SubItems.Add(callcenter_num);
						new_lvwItem.SubItems.Add(this.GetCallType(call_type));
						new_lvwItem.SubItems.Add(this.GetCustomerType(customer_type));
						new_lvwItem.SubItems.Add(exclusive_agent_num);
						new_lvwItem.SubItems.Add(queue_num);
						new_lvwItem.SubItems.Add(ComFunc.countStatusLength(current_time, enter_queue_time));
						new_lvwItem.SubItems[8].Tag = ComFunc.get_time_interval_sec(current_time, enter_queue_time);
						new_lvwItem.SubItems.Add(customer_enter_channel);
						this.lvwMonitorInfo.Items.Add(new_lvwItem);
						this.lvwMonitorInfo.Sort();
						this.updateCustomerOrder();
					}
					else if (this.lvwMonitorInfo.AccessibleName == "queue_statis_info")
					{
						if ("0" == early_queue_enter_time)
						{
							if (this.queue_max_wait_len.ContainsKey(queue_num))
							{
								int tempTime = this.queue_max_wait_len[queue_num];
								tempTime = -1;
								this.queue_max_wait_len[queue_num] = tempTime;
							}
							else
							{
								this.queue_max_wait_len.Add(queue_num, -1);
							}
						}
						else if (this.queue_max_wait_len.ContainsKey(queue_num))
						{
							int tempTime = this.queue_max_wait_len[queue_num];
							tempTime = ComFunc.get_time_interval_sec(current_time, early_queue_enter_time);
							this.queue_max_wait_len[queue_num] = tempTime;
						}
						else
						{
							this.queue_max_wait_len.Add(queue_num, ComFunc.get_time_interval_sec(current_time, early_queue_enter_time));
						}
						this.update_queue_statis_info(queue_num, 1, 0, 0, 0, 0);
					}
				}
				catch (Exception e)
				{
					FrmMonitor.Log.Error(string.Concat(new string[]
					{
						e.Source,
						",信息:",
						e.Message,
						",堆栈:",
						e.StackTrace
					}));
				}
			}
		}

		private void AddToCustomerList(string call_type, string callcenter_num, string customer_num, string customer_status, string customer_type, string customer_uuid, string enter_queue_time, string queue_num, string current_time)
		{
			Customer_Info_Struct cust_info = default(Customer_Info_Struct);
			cust_info.call_type = call_type;
			cust_info.callcenter_num = callcenter_num;
			cust_info.customer_num = customer_num;
			cust_info.customer_status = customer_status;
			cust_info.customer_type = customer_type;
			cust_info.customer_uuid = customer_uuid;
			cust_info.queue_num = queue_num;
			this.lvw_customer_list.Add(cust_info);
		}

		private void UpdateCustomer(string call_type, string callcenter_num, string customer_num, string customer_status, string customer_type, string customer_uuid, string enter_queue_time, string queue_num, string current_time)
		{
			for (int i = 0; i < this.lvw_customer_list.Count; i++)
			{
				if (this.lvw_customer_list[i].customer_uuid == customer_uuid)
				{
					break;
				}
			}
		}

		private List<Customer_Info_Struct> Sort_By_Customer(List<Customer_Info_Struct> cust_list)
		{
			List<Customer_Info_Struct> new_list = new List<Customer_Info_Struct>();
			List<Customer_Info_Struct> new_list2 = new List<Customer_Info_Struct>();
			List<Customer_Info_Struct> new_list3 = new List<Customer_Info_Struct>();
			List<Customer_Info_Struct> new_list4 = new List<Customer_Info_Struct>();
			for (int i = 0; i < cust_list.Count; i++)
			{
				string customer_type = cust_list[i].customer_type;
				if (customer_type != null)
				{
					if (!(customer_type == "3"))
					{
						if (!(customer_type == "2"))
						{
							if (customer_type == "1")
							{
								new_list2.Add(cust_list[i]);
								new_list2.Sort(new FrmMonitor.ComparableOrderIDInc());
							}
						}
						else
						{
							new_list3.Add(cust_list[i]);
							new_list3.Sort(new FrmMonitor.ComparableOrderIDInc());
						}
					}
					else
					{
						new_list4.Add(cust_list[i]);
					}
				}
			}
			if (new_list4.Count > 0)
			{
				new_list = this.AddToCustomerList(ref new_list, new_list4);
			}
			if (new_list3.Count > 0)
			{
				new_list = this.AddToCustomerList(ref new_list, new_list3);
			}
			if (new_list2.Count > 0)
			{
				new_list = this.AddToCustomerList(ref new_list, new_list2);
			}
			return new_list;
		}

		private List<Customer_Info_Struct> AddToCustomerList(ref List<Customer_Info_Struct> cust_list, List<Customer_Info_Struct> new_cust_list)
		{
			for (int i = 0; i < new_cust_list.Count; i++)
			{
				cust_list.Add(new_cust_list[i]);
			}
			return cust_list;
		}

		public void Evt_Del_Customer_From_Queue(string customer_uuid, string queue_num, string current_time, string early_queue_enter_time, string early_queue_enter_time_all, string reason)
		{
			FrmMonitor.Log.Debug("enter " + MethodBase.GetCurrentMethod().Name);
			if ((!(this.lvwMonitorInfo.AccessibleName != "monitor_customer_info") || !(this.lvwMonitorInfo.AccessibleName != "queue_statis_info")) && this.lvwMonitorInfo.Items.Count != 0)
			{
				if (this.lvwMonitorInfo.AccessibleName == "monitor_customer_info")
				{
					for (int i = 0; i < this.lvwMonitorInfo.Items.Count; i++)
					{
						if (this.lvwMonitorInfo.Items[i].Name == customer_uuid)
						{
							int j = this.lvwMonitorInfo.Items[customer_uuid].Index;
							this.lvwMonitorInfo.Items.RemoveAt(j);
							this.customer_status_len.Remove(customer_uuid);
							this.lvwMonitorInfo.Sort();
							this.updateCustomerOrder();
							break;
						}
					}
				}
				else if (this.lvwMonitorInfo.AccessibleName == "queue_statis_info")
				{
					this.update_queue_statis_info(queue_num, -1, 0, 0, 0, 0);
					if ("0" == early_queue_enter_time)
					{
						if (this.queue_max_wait_len.ContainsKey(queue_num))
						{
							int tempTime = this.queue_max_wait_len[queue_num];
							tempTime = -1;
							this.queue_max_wait_len[queue_num] = tempTime;
						}
						else
						{
							this.queue_max_wait_len.Add(queue_num, -1);
						}
					}
					else if (this.queue_max_wait_len.ContainsKey(queue_num))
					{
						int tempTime = this.queue_max_wait_len[queue_num];
						tempTime = ComFunc.get_time_interval_sec(current_time, early_queue_enter_time);
						this.queue_max_wait_len[queue_num] = tempTime;
					}
					else
					{
						this.queue_max_wait_len.Add(queue_num, ComFunc.get_time_interval_sec(current_time, early_queue_enter_time));
					}
				}
			}
		}

		public void Evt_Update_Customer_Of_Queue(string call_type, string callcenter_num, string customer_num, string customer_status, string customer_type, string customer_uuid, string enter_queue_time, string exclusive_agent_num, string exclusive_queue_num, string queue_num)
		{
			FrmMonitor.Log.Debug("enter " + MethodBase.GetCurrentMethod().Name);
			if (!(this.lvwMonitorInfo.AccessibleName != "monitor_customer_info") && this.lvwMonitorInfo.Items.Count != 0)
			{
				try
				{
					int i = this.lvwMonitorInfo.Items[customer_uuid].Index;
					this.lvwMonitorInfo.Items[i].Text = AgentBar.CustomerStatus2Chinese(customer_status);
					this.lvwMonitorInfo.Items[i].ImageKey = AgentBar.IcoName_CustomerStatus2Str(customer_status);
					this.lvwMonitorInfo.Items[i].SubItems[1].Text = "";
					this.lvwMonitorInfo.Items[i].SubItems[1].Tag = this.formula_define_customer_order(customer_type, enter_queue_time);
					this.lvwMonitorInfo.Items[i].SubItems[2].Text = customer_num;
					this.lvwMonitorInfo.Items[i].SubItems[3].Text = callcenter_num;
					this.lvwMonitorInfo.Items[i].SubItems[4].Text = this.GetCallType(call_type);
					this.lvwMonitorInfo.Items[i].SubItems[5].Text = this.GetCustomerType(customer_type);
					this.lvwMonitorInfo.Items[i].SubItems[6].Text = exclusive_agent_num;
					this.lvwMonitorInfo.Items[i].SubItems[7].Text = queue_num;
					this.updateCustomerOrder();
				}
				catch (Exception e)
				{
					FrmMonitor.Log.Error(string.Concat(new string[]
					{
						e.Source,
						",信息:",
						e.Message,
						",堆栈:",
						e.StackTrace
					}));
				}
			}
		}

		private string formula_define_customer_order(string customer_type, string enter_queue_time)
		{
			long intType = (long)Convert.ToInt32(customer_type);
			return (Convert.ToInt64(enter_queue_time) / (10L * intType)).ToString();
		}

		public void Evt_Get_Detail_Call_Info(string targetAgentNum, string call_type, List<Leg_Info_Struct> leg_info, List<Relation_Info_Struct> relation_info)
		{
			if (this.GetDetailCallInfoEvent != null)
			{
				this.GetDetailCallInfoEvent(targetAgentNum, call_type, leg_info, relation_info);
			}
		}

		public void Update_CmsMonitor_Item()
		{
			FrmMonitor.Log.Debug("enter " + MethodBase.GetCurrentMethod().Name);
			AgentBar.Agent_State agentState = this.agentBar1.AgentState;
			if (agentState != AgentBar.Agent_State.AGENT_STATUS_IDLE)
			{
				switch (agentState)
				{
				case AgentBar.Agent_State.AGENT_STATUS_TALKING_EAVESDROP:
					if (this.agentBar1.EavesDropAgentNum == this.lvwMonitorInfo.SelectedItems[0].SubItems[1].Text)
					{
						this.cmsMonitor.Items["mnuListen"].Enabled = false;
						this.cmsMonitor.Items["mnuWhisper"].Enabled = true;
						this.cmsMonitor.Items["mnuInterrupt"].Enabled = true;
						this.cmsMonitor.Items["mnuForceDisconnect"].Enabled = true;
					}
					else
					{
						this.cmsMonitor.Items["mnuListen"].Enabled = false;
						this.cmsMonitor.Items["mnuWhisper"].Enabled = false;
						this.cmsMonitor.Items["mnuInterrupt"].Enabled = false;
						this.cmsMonitor.Items["mnuForceDisconnect"].Enabled = false;
					}
					break;
				case AgentBar.Agent_State.AGENT_STATUS_TALKING_WHISPER:
					if (this.agentBar1.EavesDropAgentNum == this.lvwMonitorInfo.SelectedItems[0].SubItems[1].Text)
					{
						this.cmsMonitor.Items["mnuListen"].Enabled = true;
						this.cmsMonitor.Items["mnuWhisper"].Enabled = false;
						this.cmsMonitor.Items["mnuInterrupt"].Enabled = true;
						this.cmsMonitor.Items["mnuForceDisconnect"].Enabled = true;
					}
					else
					{
						this.cmsMonitor.Items["mnuListen"].Enabled = false;
						this.cmsMonitor.Items["mnuWhisper"].Enabled = false;
						this.cmsMonitor.Items["mnuInterrupt"].Enabled = false;
						this.cmsMonitor.Items["mnuForceDisconnect"].Enabled = false;
					}
					break;
				case AgentBar.Agent_State.AGENT_STATUS_TALKING_BARGEIN:
					if (this.agentBar1.EavesDropAgentNum == this.lvwMonitorInfo.SelectedItems[0].SubItems[1].Text)
					{
						this.cmsMonitor.Items["mnuListen"].Enabled = true;
						this.cmsMonitor.Items["mnuWhisper"].Enabled = true;
						this.cmsMonitor.Items["mnuInterrupt"].Enabled = false;
						this.cmsMonitor.Items["mnuForceDisconnect"].Enabled = true;
					}
					else
					{
						this.cmsMonitor.Items["mnuListen"].Enabled = false;
						this.cmsMonitor.Items["mnuWhisper"].Enabled = false;
						this.cmsMonitor.Items["mnuInterrupt"].Enabled = false;
						this.cmsMonitor.Items["mnuForceDisconnect"].Enabled = false;
					}
					break;
				default:
					this.cmsMonitor.Items["mnuListen"].Enabled = false;
					this.cmsMonitor.Items["mnuWhisper"].Enabled = false;
					this.cmsMonitor.Items["mnuInterrupt"].Enabled = false;
					this.cmsMonitor.Items["mnuForceDisconnect"].Enabled = false;
					break;
				}
			}
			else
			{
				this.cmsMonitor.Items["mnuListen"].Enabled = this.mMyRoleAndRight.rights_of_eavesdrop;
				this.cmsMonitor.Items["mnuWhisper"].Enabled = this.mMyRoleAndRight.rights_of_whisper;
				this.cmsMonitor.Items["mnuInterrupt"].Enabled = this.mMyRoleAndRight.rights_of_bargein;
				this.cmsMonitor.Items["mnuForceDisconnect"].Enabled = this.mMyRoleAndRight.rights_of_forcehangup;
			}
		}

		private string GetCallType(string call_type)
		{
			string result;
			if (call_type != null)
			{
				if (call_type == "0")
				{
					result = "呼入";
					return result;
				}
				if (call_type == "1")
				{
					result = "预测式呼出";
					return result;
				}
				if (call_type == "2")
				{
					result = "呼出";
					return result;
				}
			}
			result = "未知";
			return result;
		}

		private string GetCustomerType(string cust_type)
		{
			string result;
			if (cust_type != null)
			{
				if (cust_type == "1")
				{
					result = "普通客户";
					return result;
				}
				if (cust_type == "2")
				{
					result = "VIP客户";
					return result;
				}
				if (cust_type == "3")
				{
					result = "重进入客户";
					return result;
				}
			}
			result = "未知";
			return result;
		}

		private void FrmMonitor_Load(object sender, EventArgs e)
		{
			this.tvwGroupAndQueue.Nodes.Clear();
			this.lvwMonitorInfo.Clear();
			this.tmrUpdateTime.Interval = 1000;
			this.tmrUpdateTime.Start();
			this.splitContainer2.Panel1Collapsed = true;
			this.lvwColumnSorter = new ListViewColumnSorter();
			this.lvwMonitorInfo.ListViewItemSorter = this.lvwColumnSorter;
			this.lvwColumnSorter_agent = new ListViewColumnSorter_Agent();
			this.lvwColumnTimeSorter = new ListViewColumnTimeSorter();
			this.lvwColumnSorter_agentState = new ListViewColumnSorter_AgentState();
			this.lvwColumnSorter_agent_status = new ListViewColumnSorter_Agent_Status();
			this.agent_num_lst_of_queue = new Dictionary<string, string>();
			this.picReturn.Visible = false;
			this.Form_Title_Name = "话务监控";
		}

		private void ThdUpdateTimeLength()
		{
			if (!(this.lvwMonitorInfo.AccessibleName != "monitor_agent_info") || !(this.lvwMonitorInfo.AccessibleName != "monitor_customer_info") || !(this.lvwMonitorInfo.AccessibleName != "queue_statis_info"))
			{
				for (int i = 0; i < this.lvwMonitorInfo.Items.Count; i++)
				{
					try
					{
						if (this.lvwMonitorInfo.AccessibleName == "monitor_agent_info")
						{
							if (this.lvwMonitorInfo.Items[i].SubItems[3].Text != "")
							{
								string agent_num = this.lvwMonitorInfo.Items[i].SubItems[1].Text;
								this.agent_status_len[agent_num] = this.agent_status_len[agent_num] + 1;
								this.lvwMonitorInfo.Items[i].SubItems[3].Text = ComFunc.converToTimeLength(this.agent_status_len[agent_num].ToString());
								this.lvwMonitorInfo.Items[i].SubItems[3].Tag = this.agent_status_len[agent_num];
							}
						}
						if (this.lvwMonitorInfo.AccessibleName == "monitor_customer_info")
						{
							if (this.lvwMonitorInfo.Items[i].SubItems[8].Text != "")
							{
								string item_name = this.lvwMonitorInfo.Items[i].Name;
								this.customer_status_len[item_name] = this.customer_status_len[item_name] + 1;
								this.lvwMonitorInfo.Items[i].SubItems[8].Text = ComFunc.converToTimeLength(this.customer_status_len[item_name].ToString());
								this.lvwMonitorInfo.Items[i].SubItems[8].Tag = this.customer_status_len[item_name];
							}
						}
						if (this.lvwMonitorInfo.AccessibleName == "queue_statis_info")
						{
							if (this.lvwMonitorInfo.Items[i].SubItems[2].Text != "0")
							{
								string item_name = this.lvwMonitorInfo.Items[i].Name;
								this.queue_max_wait_len[item_name] = this.queue_max_wait_len[item_name] + 1;
								this.lvwMonitorInfo.Items[i].SubItems[3].Text = ComFunc.converToTimeLength(this.queue_max_wait_len[item_name].ToString());
								this.lvwMonitorInfo.Items[i].SubItems[3].Tag = this.queue_max_wait_len[item_name];
							}
							else
							{
								this.lvwMonitorInfo.Items[i].SubItems[3].Text = "0秒";
								this.lvwMonitorInfo.Items[i].SubItems[3].Tag = 0;
							}
						}
					}
					catch (Exception e)
					{
						FrmMonitor.Log.Error(string.Concat(new string[]
						{
							e.Source,
							",信息:",
							e.Message,
							",堆栈:",
							e.StackTrace
						}));
					}
				}
			}
		}

		private void updateCustomerOrder()
		{
			List<long> list_OrderID2 = new List<long>();
			for (int i = 0; i < this.lvwMonitorInfo.Items.Count; i++)
			{
				list_OrderID2.Add(Convert.ToInt64(this.lvwMonitorInfo.Items[i].SubItems[1].Tag));
			}
			list_OrderID2.Sort();
			for (int i = 0; i < this.lvwMonitorInfo.Items.Count; i++)
			{
				long orderid = Convert.ToInt64(this.lvwMonitorInfo.Items[i].SubItems[1].Tag);
				long orderid2 = (long)list_OrderID2.FindIndex((long num) => num == orderid);
				this.lvwMonitorInfo.Items[i].SubItems[1].Text = (orderid2 + 1L).ToString();
			}
			this.lvwMonitorInfo.ListViewItemSorter = this.lvwColumnTimeSorter;
			this.lvwColumnTimeSorter.SortColumn = 1;
			this.lvwColumnTimeSorter.Order = SortOrder.Ascending;
			this.lvwMonitorInfo.Sort();
		}

		private void tvwGroupAndQueue_AfterSelect(object sender, TreeViewEventArgs e)
		{
			FrmMonitor.Log.Debug("enter " + MethodBase.GetCurrentMethod().Name);
			string form_title_name = "";
			this.Text = this.Form_Title_Name;
			this.picReturn.Visible = false;
			if (this.tvwGroupAndQueue.SelectedNode.Nodes.Count != 0)
			{
				this.splitContainer2.Panel1Collapsed = true;
				if (this.lvwMonitorInfo != null)
				{
					this.lvwMonitorInfo.Clear();
				}
			}
			else
			{
				if (this.tvwGroupAndQueue.SelectedNode.Parent != null)
				{
					if (this.tvwGroupAndQueue.SelectedNode.Parent.Text == "坐席组")
					{
						this.GetAgentsOfGroupEvent(this.tvwGroupAndQueue.SelectedNode.Name);
						form_title_name = "坐席组监控 - 坐席组名称：" + this.tvwGroupAndQueue.SelectedNode.Text;
					}
					else if (this.tvwGroupAndQueue.SelectedNode.Parent.Parent.Text == "队列")
					{
						if (this.tvwGroupAndQueue.SelectedNode.Text == "客户排队监控")
						{
							this.tvwGroupAndQueue.SelectedNode.ImageKey = "wait";
							this.show_customer_of_queue(this.tvwGroupAndQueue.SelectedNode.Parent.Name);
							form_title_name = "客户排队监控 - 队列号：" + this.tvwGroupAndQueue.SelectedNode.Parent.Name;
						}
						else
						{
							this.tvwGroupAndQueue.SelectedNode.ImageKey = "idle";
							this.show_agent_of_queue(this.tvwGroupAndQueue.SelectedNode.Parent.Name);
							form_title_name = "队列坐席监控 - 队列号：" + this.tvwGroupAndQueue.SelectedNode.Parent.Name;
						}
					}
				}
				if (this.tvwGroupAndQueue.SelectedNode.Text == "队列统计")
				{
					this.HasClickQueueStatis = true;
					this.agentBar1.DoGetQueueList();
					form_title_name = "队列统计";
				}
				this.Text = this.Text + "(" + form_title_name + ")";
			}
		}

		private void show_customer_of_queue(string queue_num)
		{
			this.GetCustomersOfQueueEvent(queue_num);
		}

		private void show_agent_of_queue(string queue_num)
		{
			this.GetAgentsOfQueueEvent(queue_num);
		}

		private void show_queue_statis_info(Dictionary<string, string> now_queue_list)
		{
			bool blnRet = false;
			if (now_queue_list != null && now_queue_list.Count > 0)
			{
				this.lvwMonitorInfo.Clear();
				this.initMonitorQueueStatisInfo();
				foreach (KeyValuePair<string, string> kv in now_queue_list)
				{
					blnRet = this.agentBar1.DoGetQueueStatis(kv.Key);
				}
				if (!blnRet)
				{
					MessageBox.Show("获取队列中的客户失败！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
				}
			}
		}

		private void tmrUpdateTime_Tick(object sender, EventArgs e)
		{
			this.ThdUpdateTimeLength();
		}

		private void FrmMonitor_FormClosing(object sender, FormClosingEventArgs e)
		{
			this.lvwMonitorInfo.AccessibleName = "";
			this.tmrUpdateTime.Stop();
		}

		private void lvwMonitorInfo_ColumnWidthChanged(object sender, ColumnWidthChangedEventArgs e)
		{
		}

		private void lvwMonitorInfo_ColumnWidthChanging(object sender, ColumnWidthChangingEventArgs e)
		{
			if (this.lvwMonitorInfo.AccessibleName == "monitor_customer_info")
			{
				if (e.ColumnIndex == 1)
				{
					e.Cancel = true;
					e.NewWidth = this.lvwMonitorInfo.Columns[e.ColumnIndex].Width;
				}
			}
		}

		private void lvwMonitorInfo_ColumnClick(object sender, ColumnClickEventArgs e)
		{
			if (this.lvwMonitorInfo.AccessibleName == "monitor_agent_info")
			{
				if (e.Column == 0)
				{
					this.lvwMonitorInfo.ListViewItemSorter = this.lvwColumnSorter_agentState;
					this.lvwColumnSorter_agentState.SortColumn = e.Column;
					this.lvwColumnSorter_agentState.Order = SortOrder.Descending;
					this.lvwMonitorInfo.Sort();
				}
				else
				{
					if (e.Column == 3)
					{
						this.lvwMonitorInfo.ListViewItemSorter = this.lvwColumnTimeSorter;
						this.lvwColumnTimeSorter.SortColumn = e.Column;
						if (this.lvwColumnTimeSorter.Order == SortOrder.Ascending)
						{
							this.lvwColumnTimeSorter.Order = SortOrder.Descending;
						}
						else
						{
							this.lvwColumnTimeSorter.Order = SortOrder.Ascending;
						}
					}
					else
					{
						this.lvwMonitorInfo.ListViewItemSorter = this.lvwColumnSorter_agent;
						this.lvwColumnSorter_agent.SortColumn = e.Column;
						if (this.lvwColumnSorter_agent.Order == SortOrder.Ascending)
						{
							this.lvwColumnSorter_agent.Order = SortOrder.Descending;
						}
						else
						{
							this.lvwColumnSorter_agent.Order = SortOrder.Ascending;
						}
					}
					this.lvwMonitorInfo.Sort();
				}
			}
			else if (this.lvwMonitorInfo.AccessibleName == "monitor_customer_info")
			{
				if (e.Column == 8 || e.Column == 1)
				{
					this.lvwMonitorInfo.ListViewItemSorter = this.lvwColumnTimeSorter;
					this.lvwColumnTimeSorter.SortColumn = e.Column;
					if (this.lvwColumnTimeSorter.Order == SortOrder.Ascending)
					{
						this.lvwColumnTimeSorter.Order = SortOrder.Descending;
					}
					else
					{
						this.lvwColumnTimeSorter.Order = SortOrder.Ascending;
					}
				}
				else
				{
					this.lvwMonitorInfo.ListViewItemSorter = this.lvwColumnSorter_agent;
					this.lvwColumnSorter_agent.SortColumn = e.Column;
					if (this.lvwColumnSorter_agent.Order == SortOrder.Ascending)
					{
						this.lvwColumnSorter_agent.Order = SortOrder.Descending;
					}
					else
					{
						this.lvwColumnSorter_agent.Order = SortOrder.Ascending;
					}
				}
				this.lvwMonitorInfo.Sort();
			}
			else if (this.lvwMonitorInfo.AccessibleName == "queue_statis_info")
			{
				if (e.Column == 3)
				{
					this.lvwMonitorInfo.ListViewItemSorter = this.lvwColumnTimeSorter;
					this.lvwColumnTimeSorter.SortColumn = e.Column;
					if (this.lvwColumnTimeSorter.Order == SortOrder.Ascending)
					{
						this.lvwColumnTimeSorter.Order = SortOrder.Descending;
					}
					else
					{
						this.lvwColumnTimeSorter.Order = SortOrder.Ascending;
					}
				}
				else
				{
					this.lvwMonitorInfo.ListViewItemSorter = this.lvwColumnSorter_agent;
					this.lvwColumnSorter_agent.SortColumn = e.Column;
					if (this.lvwColumnSorter_agent.Order == SortOrder.Ascending)
					{
						this.lvwColumnSorter_agent.Order = SortOrder.Descending;
					}
					else
					{
						this.lvwColumnSorter_agent.Order = SortOrder.Ascending;
					}
				}
				this.lvwMonitorInfo.Sort();
			}
		}

		private void FrmMonitor_FormClosed(object sender, FormClosedEventArgs e)
		{
			base.Dispose();
		}

		private void lvwMonitorInfo_MouseUp(object sender, MouseEventArgs e)
		{
			FrmMonitor.Log.Debug("enter " + MethodBase.GetCurrentMethod().Name);
			if (e.Button == MouseButtons.Right && this.lvwMonitorInfo.SelectedItems.Count >= 1)
			{
				if (this.lvwMonitorInfo.AccessibleName == "queue_statis_info")
				{
					this.cmsViewDetail.Show(this.lvwMonitorInfo, e.X, e.Y);
				}
				else if (!(this.lvwMonitorInfo.AccessibleName != "monitor_agent_info"))
				{
					string strAgentStatus = this.lvwMonitorInfo.SelectedItems[0].Text;
					if (this.mMyAgentNum == this.lvwMonitorInfo.SelectedItems[0].SubItems[1].Text)
					{
						this.cmsMonitor.Items["mnuListen"].Enabled = false;
						this.cmsMonitor.Items["mnuWhisper"].Enabled = false;
						this.cmsMonitor.Items["mnuInterrupt"].Enabled = false;
						this.cmsMonitor.Items["mnuForceDisconnect"].Enabled = false;
						this.cmsMonitor.Items["mnuOnIdleByForce"].Enabled = false;
						this.cmsMonitor.Items["mnuOnBusyByForce"].Enabled = false;
						this.cmsMonitor.Items["mnuOnleaveByForce"].Enabled = false;
					}
					else
					{
						this.mMyRoleAndRight = this.agentBar1.AgentRoleAndRight;
						if (AgentBar.AgentStatus2Str(AgentBar.Agent_Status.AGENT_STATUS_TALKING) == strAgentStatus)
						{
							if (this.agentBar1.BindExten)
							{
								this.Update_CmsMonitor_Item();
							}
							else
							{
								this.cmsMonitor.Items["mnuListen"].Enabled = false;
								this.cmsMonitor.Items["mnuWhisper"].Enabled = false;
								this.cmsMonitor.Items["mnuInterrupt"].Enabled = false;
								this.cmsMonitor.Items["mnuForceDisconnect"].Enabled = false;
							}
						}
						else
						{
							this.cmsMonitor.Items["mnuListen"].Enabled = false;
							this.cmsMonitor.Items["mnuWhisper"].Enabled = false;
							this.cmsMonitor.Items["mnuInterrupt"].Enabled = false;
							this.cmsMonitor.Items["mnuForceDisconnect"].Enabled = false;
						}
						if (AgentBar.AgentStatus2Str(AgentBar.Agent_Status.AGENT_STATUS_OFFLINE) == strAgentStatus || AgentBar.AgentStatus2Str(AgentBar.Agent_Status.AGENT_STATUS_RING) == strAgentStatus || AgentBar.AgentStatus2Str(AgentBar.Agent_Status.AGENT_STATUS_TALKING) == strAgentStatus || AgentBar.AgentStatus2Str(AgentBar.Agent_Status.AGENT_STATUS_HOLD) == strAgentStatus || AgentBar.AgentStatus2Str(AgentBar.Agent_Status.AGENT_STATUS_CAMP_ON) == strAgentStatus || AgentBar.AgentStatus2Str(AgentBar.Agent_Status.AGENT_STATUS_MUTE) == strAgentStatus || AgentBar.AgentStatus2Str(AgentBar.Agent_Status.AGENT_STATUS_CALLING_OUT) == strAgentStatus)
						{
							this.cmsMonitor.Items["mnuOnIdleByForce"].Enabled = false;
							this.cmsMonitor.Items["mnuOnBusyByForce"].Enabled = false;
							this.cmsMonitor.Items["mnuOnleaveByForce"].Enabled = false;
						}
						else if (AgentBar.AgentStatus2Str(AgentBar.Agent_Status.AGENT_STATUS_IDLE) == strAgentStatus)
						{
							this.cmsMonitor.Items["mnuOnIdleByForce"].Enabled = false;
							this.cmsMonitor.Items["mnuOnBusyByForce"].Enabled = this.mMyRoleAndRight.rights_of_force_change_status_busy;
							this.cmsMonitor.Items["mnuOnleaveByForce"].Enabled = this.mMyRoleAndRight.rights_of_force_change_status_leave;
						}
						else if (AgentBar.AgentStatus2Str(AgentBar.Agent_Status.AGENT_STATUS_BUSY) == strAgentStatus)
						{
							this.cmsMonitor.Items["mnuOnIdleByForce"].Enabled = this.mMyRoleAndRight.rights_of_force_change_status_idle;
							this.cmsMonitor.Items["mnuOnBusyByForce"].Enabled = false;
							this.cmsMonitor.Items["mnuOnleaveByForce"].Enabled = this.mMyRoleAndRight.rights_of_force_change_status_leave;
						}
						else if (AgentBar.AgentStatus2Str(AgentBar.Agent_Status.AGENT_STATUS_LEAVE) == strAgentStatus)
						{
							this.cmsMonitor.Items["mnuOnIdleByForce"].Enabled = this.mMyRoleAndRight.rights_of_force_change_status_idle;
							this.cmsMonitor.Items["mnuOnBusyByForce"].Enabled = this.mMyRoleAndRight.rights_of_force_change_status_busy;
							this.cmsMonitor.Items["mnuOnleaveByForce"].Enabled = false;
						}
						else
						{
							this.cmsMonitor.Items["mnuOnIdleByForce"].Enabled = this.mMyRoleAndRight.rights_of_force_change_status_idle;
							this.cmsMonitor.Items["mnuOnBusyByForce"].Enabled = this.mMyRoleAndRight.rights_of_force_change_status_busy;
							this.cmsMonitor.Items["mnuOnleaveByForce"].Enabled = this.mMyRoleAndRight.rights_of_force_change_status_leave;
						}
					}
					this.cmsMonitor.Show(this.lvwMonitorInfo, e.X, e.Y);
				}
			}
		}

		private void mnuListen_Click(object sender, EventArgs e)
		{
			if (this.DoEavesDropEvent != null)
			{
				this.DoEavesDropEvent(this.lvwMonitorInfo.SelectedItems[0].SubItems[1].Text);
			}
		}

		private void mnuWhisper_Click(object sender, EventArgs e)
		{
			if (this.DoWhisperEvent != null)
			{
				this.DoWhisperEvent(this.lvwMonitorInfo.SelectedItems[0].SubItems[1].Text);
			}
		}

		private void mnuInterrupt_Click(object sender, EventArgs e)
		{
			if (this.DoBargeinEvent != null)
			{
				this.DoBargeinEvent(this.lvwMonitorInfo.SelectedItems[0].SubItems[1].Text);
			}
		}

		private void mnuForceDisconnect_Click(object sender, EventArgs e)
		{
			if (this.DoForceHangupEvent != null)
			{
				this.DoForceHangupEvent(this.lvwMonitorInfo.SelectedItems[0].SubItems[1].Text);
			}
		}

		private void mnuOnIdleByForce_Click(object sender, EventArgs e)
		{
			if (this.DoForceChangeStatusEvent != null)
			{
				this.DoForceChangeStatusEvent(this.lvwMonitorInfo.SelectedItems[0].SubItems[1].Text, 0.ToString());
			}
		}

		private void mnuOnBusyByForce_Click(object sender, EventArgs e)
		{
			if (this.DoForceChangeStatusEvent != null)
			{
				this.DoForceChangeStatusEvent(this.lvwMonitorInfo.SelectedItems[0].SubItems[1].Text, 6.ToString());
			}
		}

		private void mnuOnleaveByForce_Click(object sender, EventArgs e)
		{
			if (this.DoForceChangeStatusEvent != null)
			{
				this.DoForceChangeStatusEvent(this.lvwMonitorInfo.SelectedItems[0].SubItems[1].Text, 7.ToString());
			}
		}

		private void lvwMonitorInfo_ItemActivate(object sender, EventArgs e)
		{
			this.showCallDetailInfo();
		}

		private void showCallDetailInfo()
		{
			if (this.lvwMonitorInfo.SelectedItems.Count >= 1)
			{
				string agent_status = this.lvwMonitorInfo.SelectedItems[0].Text;
				if (agent_status == AgentBar.AgentStatus2Str(AgentBar.Agent_Status.AGENT_STATUS_HOLD) || agent_status == AgentBar.AgentStatus2Str(AgentBar.Agent_Status.AGENT_STATUS_TALKING) || agent_status == AgentBar.AgentStatus2Str(AgentBar.Agent_Status.AGENT_STATUS_MUTE))
				{
					FrmMonitor.frmDetail = new FrmDetailCallInfo();
					FrmMonitor.frmDetail.AgentNum = this.lvwMonitorInfo.SelectedItems[0].SubItems[1].Text;
					FrmMonitor.frmDetail.Location = new Point(this.lvwMonitorInfo.PointToScreen(this.lvwMonitorInfo.SelectedItems[0].Position).X + 30, this.lvwMonitorInfo.PointToScreen(this.lvwMonitorInfo.SelectedItems[0].Position).Y + 10);
					this.GetDetailCallInfoEvent = (FrmMonitor.GetDetailCallInfoEventHandler)Delegate.Combine(this.GetDetailCallInfoEvent, new FrmMonitor.GetDetailCallInfoEventHandler(FrmMonitor.frmDetail.Evt_Get_Detail_Call_Info));
					FrmMonitor.frmDetail.DoGetDetailCallInfoEvent += new FrmDetailCallInfo.DoGetDetailCallInfoEventHandler(this.Evt_Do_Get_Detail_Call_info);
					FrmMonitor.frmDetail.Show();
				}
			}
		}

		private void tvwGroupAndQueue_Leave(object sender, EventArgs e)
		{
			if (this.tvwGroupAndQueue.SelectedNode != null)
			{
				Color c = Color.FromArgb(255, 49, 106, 197);
				this.tvwGroupAndQueue.SelectedNode.BackColor = c;
				this.tvwGroupAndQueue.SelectedNode.ForeColor = Color.White;
			}
		}

		private void tvwGroupAndQueue_BeforeSelect(object sender, TreeViewCancelEventArgs e)
		{
			if (this.tvwGroupAndQueue.SelectedNode != null)
			{
				this.tvwGroupAndQueue.SelectedNode.BackColor = Color.Empty;
				this.tvwGroupAndQueue.SelectedNode.ForeColor = Color.Black;
			}
		}

		private void tvwGroupAndQueue_Click(object sender, EventArgs e)
		{
		}

		private void lvwMonitorInfo_SelectedIndexChanged(object sender, EventArgs e)
		{
			FrmMonitor.Log.Debug("enter " + MethodBase.GetCurrentMethod().Name);
			if (this.lvwMonitorInfo.SelectedItems.Count == 0)
			{
				this.update_Toolbar_UI(FrmMonitor.Event_Type.INITE_TOOLBAR, "");
			}
			else
			{
				string strAgentStatus = this.lvwMonitorInfo.SelectedItems[0].SubItems[0].Text;
				int intListen = this.agentBar1.GetAgentBarItemEnableAndVisible("tsbListen");
				int intWhisper = this.agentBar1.GetAgentBarItemEnableAndVisible("tsbWhisper");
				int intBargein = this.agentBar1.GetAgentBarItemEnableAndVisible("tsbBargein");
				int intForcehangup = this.agentBar1.GetAgentBarItemEnableAndVisible("tsbForceHangup");
				int intHangup = this.agentBar1.GetAgentBarItemEnableAndVisible("tsbHangUp");
				if (this.mMyAgentNum == this.lvwMonitorInfo.SelectedItems[0].SubItems[1].Text)
				{
					this.update_Toostrip_Monitor(intListen & 2, intWhisper & 2, intBargein & 2, intForcehangup & 2, intHangup);
				}
				else if (this.agentBar1.IsInUse)
				{
					if (this.agentBar1.EavesDropAgentNum == this.lvwMonitorInfo.SelectedItems[0].SubItems[1].Text)
					{
						this.update_Toolbar_UI(FrmMonitor.Event_Type.INITE_TOOLBAR, "");
					}
					else
					{
						this.update_Toostrip_Monitor(intListen & 2, intWhisper & 2, intBargein & 2, intForcehangup & 2, intHangup);
					}
				}
				else if (AgentBar.AgentStatus2Str(AgentBar.Agent_Status.AGENT_STATUS_TALKING) == strAgentStatus || AgentBar.AgentStatus2Str(AgentBar.Agent_Status.AGENT_STATUS_HOLD) == strAgentStatus || AgentBar.AgentStatus2Str(AgentBar.Agent_Status.AGENT_STATUS_MUTE) == strAgentStatus)
				{
					this.update_Toolbar_UI(FrmMonitor.Event_Type.INITE_TOOLBAR, "");
				}
				else
				{
					this.update_Toostrip_Monitor(intListen & 2, intWhisper & 2, intBargein & 2, intForcehangup & 2, intHangup);
				}
			}
		}

		private void lvwMonitorInfo_MouseClick(object sender, MouseEventArgs e)
		{
		}

		private void picReturn_Click(object sender, EventArgs e)
		{
			this.picReturn.Visible = false;
			this.tvwGroupAndQueue.SelectedNode = this.tvwGroupAndQueue.Nodes["queue_statis"];
			string form_title_name = "队列统计";
			this.Text = this.Form_Title_Name + " (" + form_title_name + ")";
		}

		private void picReturn_MouseEnter(object sender, EventArgs e)
		{
		}

		private void picReturn_MouseLeave(object sender, EventArgs e)
		{
		}

		private void mnuQueue_Click(object sender, EventArgs e)
		{
			this.Text = this.Form_Title_Name;
			if (this.lvwMonitorInfo.SelectedItems.Count >= 1)
			{
				string queue_num = this.lvwMonitorInfo.SelectedItems[0].Name;
				if (!string.IsNullOrEmpty(queue_num))
				{
					this.show_customer_of_queue(queue_num);
					this.picReturn.Visible = true;
					string form_title_name = "客户排队监控 - 队列号：" + queue_num;
					this.Text = this.Text + " (" + form_title_name + ")";
					this.tvwGroupAndQueue.SelectedNode.BackColor = Color.Empty;
					this.tvwGroupAndQueue.SelectedNode.ForeColor = Color.Black;
					this.tvwGroupAndQueue.SelectedNode = null;
					this.tvwGroupAndQueue.Tag = queue_num;
				}
			}
		}

		private void mnuAgent_Click(object sender, EventArgs e)
		{
			this.Text = this.Form_Title_Name;
			if (this.lvwMonitorInfo.SelectedItems.Count >= 1)
			{
				string queue_num = this.lvwMonitorInfo.SelectedItems[0].Name;
				if (!string.IsNullOrEmpty(queue_num))
				{
					this.show_agent_of_queue(queue_num);
					this.picReturn.Visible = true;
					string form_title_name = "队列坐席监控 - 队列号：" + queue_num;
					this.Text = this.Text + " (" + form_title_name + ")";
					this.tvwGroupAndQueue.SelectedNode.BackColor = Color.Empty;
					this.tvwGroupAndQueue.SelectedNode.ForeColor = Color.Black;
					this.tvwGroupAndQueue.SelectedNode = null;
				}
			}
		}

		private void cmsViewDetail_Opening(object sender, CancelEventArgs e)
		{
		}

		private void tsbHangup_Click(object sender, EventArgs e)
		{
			FrmMonitor.Log.Debug("enter " + MethodBase.GetCurrentMethod().Name);
			if (MessageBox.Show("您确定要挂断电话么？", "挂断电话", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
			{
				if (!this.agentBar1.DoHangUp())
				{
					this.tsbHangUp.Enabled = true;
					MessageBox.Show("挂断失败！");
				}
				else
				{
					this.tsbHangUp.Enabled = false;
				}
			}
		}

		private void update_Toolbar_UI(FrmMonitor.Event_Type eventType, string info)
		{
			FrmMonitor.Log.Debug("enter " + MethodBase.GetCurrentMethod().Name);
			if (eventType == FrmMonitor.Event_Type.INITE_TOOLBAR)
			{
				int intListen = this.agentBar1.GetAgentBarItemEnableAndVisible("tsbListen");
				int intWhisper = this.agentBar1.GetAgentBarItemEnableAndVisible("tsbWhisper");
				int intBargein = this.agentBar1.GetAgentBarItemEnableAndVisible("tsbBargein");
				int intForcehangup = this.agentBar1.GetAgentBarItemEnableAndVisible("tsbForceHangup");
				int intHangup = this.agentBar1.GetAgentBarItemEnableAndVisible("tsbHangUp");
				this.update_Toostrip_Monitor(intListen, intWhisper, intBargein, intForcehangup, intHangup);
				this.tslStatus.Text = this.agentBar1.GetAgentBarStatusInfo();
				this.tslStatus.ToolTipText = "";
			}
		}

		public void Evt_AgentBar_AgentBarUIChangedEvent(AgentBar.Event_Type event_type, string statusInfo)
		{
			FrmMonitor.Log.Debug("enter " + MethodBase.GetCurrentMethod().Name);
			try
			{
				if (!(this.lvwMonitorInfo.AccessibleName != "monitor_agent_info"))
				{
					this.tslStatus.Text = this.agentBar1.GetAgentBarStatusInfo();
					this.tslStatus.ToolTipText = "";
					if (this.lvwMonitorInfo.SelectedItems.Count == 0)
					{
						this.update_Toolbar_UI(FrmMonitor.Event_Type.INITE_TOOLBAR, statusInfo);
					}
					else
					{
						string strAgentStatus = this.lvwMonitorInfo.SelectedItems[0].SubItems[0].Text;
						int intListen = this.agentBar1.GetAgentBarItemEnableAndVisible("tsbListen");
						int intWhisper = this.agentBar1.GetAgentBarItemEnableAndVisible("tsbWhisper");
						int intBargein = this.agentBar1.GetAgentBarItemEnableAndVisible("tsbBargein");
						int intForcehangup = this.agentBar1.GetAgentBarItemEnableAndVisible("tsbForceHangup");
						int intHangup = this.agentBar1.GetAgentBarItemEnableAndVisible("tsbHangUp");
						if (this.mMyAgentNum == this.lvwMonitorInfo.SelectedItems[0].SubItems[1].Text)
						{
							this.update_Toostrip_Monitor(intListen & 2, intWhisper & 2, intBargein & 2, intForcehangup & 2, intHangup);
						}
						else if (this.agentBar1.IsInUse)
						{
							if (this.agentBar1.EavesDropAgentNum == this.lvwMonitorInfo.SelectedItems[0].SubItems[1].Text)
							{
								this.update_Toolbar_UI(FrmMonitor.Event_Type.INITE_TOOLBAR, "");
							}
							else
							{
								this.update_Toostrip_Monitor(intListen & 2, intWhisper & 2, intBargein & 2, intForcehangup & 2, intHangup);
							}
						}
						else if (AgentBar.AgentStatus2Str(AgentBar.Agent_Status.AGENT_STATUS_TALKING) == strAgentStatus || AgentBar.AgentStatus2Str(AgentBar.Agent_Status.AGENT_STATUS_HOLD) == strAgentStatus || AgentBar.AgentStatus2Str(AgentBar.Agent_Status.AGENT_STATUS_MUTE) == strAgentStatus)
						{
							this.update_Toolbar_UI(FrmMonitor.Event_Type.INITE_TOOLBAR, "");
						}
						else
						{
							this.update_Toostrip_Monitor(intListen & 2, intWhisper & 2, intBargein & 2, intForcehangup & 2, intHangup);
						}
					}
				}
			}
			catch (Exception ex)
			{
				FrmMonitor.Log.Error(string.Concat(new string[]
				{
					ex.Source,
					",信息:",
					ex.Message,
					",堆栈:",
					ex.StackTrace
				}));
			}
		}

		private void update_Toostrip_Monitor(int _tsbListen, int _tsbWhisper, int _tsbBargein, int _tsbForceHangup, int _tsbHangup)
		{
			FrmMonitor.Log.Debug("enter " + MethodBase.GetCurrentMethod().Name);
			if ((_tsbListen & 2) == 2)
			{
				this.tsbListen.Visible = true;
				this.tsbListen.Tag = true;
				if ((_tsbListen & 1) == 1)
				{
					this.tsbListen.Enabled = true;
				}
				else
				{
					this.tsbListen.Enabled = false;
				}
			}
			else
			{
				this.tsbListen.Visible = false;
				this.tsbListen.Tag = false;
			}
			if ((_tsbWhisper & 2) == 2)
			{
				this.tsbWhisper.Visible = true;
				this.tsbWhisper.Tag = true;
				if ((_tsbWhisper & 1) == 1)
				{
					this.tsbWhisper.Enabled = true;
				}
				else
				{
					this.tsbWhisper.Enabled = false;
				}
			}
			else
			{
				this.tsbWhisper.Visible = false;
				this.tsbWhisper.Tag = false;
			}
			if ((_tsbBargein & 2) == 2)
			{
				this.tsbBargein.Visible = true;
				this.tsbBargein.Tag = true;
				if ((_tsbBargein & 1) == 1)
				{
					this.tsbBargein.Enabled = true;
				}
				else
				{
					this.tsbBargein.Enabled = false;
				}
			}
			else
			{
				this.tsbBargein.Visible = false;
				this.tsbBargein.Tag = false;
			}
			if ((_tsbForceHangup & 2) == 2)
			{
				this.tsbForceHangup.Visible = true;
				this.tsbForceHangup.Tag = true;
				if ((_tsbForceHangup & 1) == 1)
				{
					this.tsbForceHangup.Enabled = true;
				}
				else
				{
					this.tsbForceHangup.Enabled = false;
				}
			}
			else
			{
				this.tsbForceHangup.Visible = false;
				this.tsbForceHangup.Tag = false;
			}
			if ((_tsbHangup & 2) == 2)
			{
				this.tsbHangUp.Visible = true;
				this.tsbHangUp.Tag = true;
				if ((_tsbHangup & 1) == 1)
				{
					this.tsbHangUp.Enabled = true;
				}
				else
				{
					this.tsbHangUp.Enabled = false;
				}
			}
			else
			{
				this.tsbHangUp.Visible = false;
				this.tsbHangUp.Tag = false;
			}
		}

		private void tsbListen_Click(object sender, EventArgs e)
		{
			FrmMonitor.Log.Debug("enter " + MethodBase.GetCurrentMethod().Name);
			if (this.agentBar1.AgentState == AgentBar.Agent_State.AGENT_STATUS_TALKING_BARGEIN || this.agentBar1.AgentState == AgentBar.Agent_State.AGENT_STATUS_TALKING_WHISPER)
			{
				bool blnRet = this.agentBar1.DoListen(this.agentBar1.EavesDropAgentNum);
				if (blnRet)
				{
					this.tsbListen.Enabled = false;
				}
				else
				{
					MessageBox.Show("监听失败！", "监听坐席", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
					this.tsbListen.Enabled = true;
				}
			}
			else if (this.lvwMonitorInfo.SelectedItems.Count == 0)
			{
				MessageBox.Show("请选择一个坐席！", "监听失败", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
			}
			else if (this.lvwMonitorInfo.SelectedItems[0].SubItems[1].Text != this.agentBar1.AgentID)
			{
				if (this.agentBar1.AgentStatus != Convert.ToString(9))
				{
					int new_state = 9;
					if (!this.agentBar1.DoSetAgentDefineStatus(new_state, 1))
					{
						MessageBox.Show("更改坐席状态为：监控 失败！", "更改坐席状态", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
						return;
					}
				}
				bool blnRet = this.agentBar1.DoListen(this.lvwMonitorInfo.SelectedItems[0].SubItems[1].Text);
				if (blnRet)
				{
					this.tsbListen.Enabled = false;
				}
				else
				{
					MessageBox.Show("监听失败！", "监听坐席", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
					this.tsbListen.Enabled = true;
				}
			}
			else
			{
				MessageBox.Show("不能呼叫自己！", "监听坐席", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
			}
		}

		private void tsbWhisper_Click(object sender, EventArgs e)
		{
			FrmMonitor.Log.Debug("enter " + MethodBase.GetCurrentMethod().Name);
			if (this.agentBar1.AgentState == AgentBar.Agent_State.AGENT_STATUS_TALKING_EAVESDROP || this.agentBar1.AgentState == AgentBar.Agent_State.AGENT_STATUS_TALKING_BARGEIN)
			{
				bool blnRet = this.agentBar1.DoWhisper(this.agentBar1.EavesDropAgentNum);
				if (blnRet)
				{
					this.tsbWhisper.Enabled = false;
				}
				else
				{
					MessageBox.Show("密语失败！", "密语坐席", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
					this.tsbWhisper.Enabled = true;
				}
			}
			else if (this.lvwMonitorInfo.SelectedItems.Count == 0)
			{
				MessageBox.Show("请选择一个坐席！", "密语失败", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
			}
			else if (this.lvwMonitorInfo.SelectedItems[0].SubItems[1].Text != this.agentBar1.AgentID)
			{
				if (this.agentBar1.AgentStatus != Convert.ToString(9))
				{
					int new_state = 9;
					if (!this.agentBar1.DoSetAgentDefineStatus(new_state, 1))
					{
						MessageBox.Show("更改坐席状态为：监控 失败！", "更改坐席状态", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
						return;
					}
				}
				bool blnRet = this.agentBar1.DoWhisper(this.lvwMonitorInfo.SelectedItems[0].SubItems[1].Text);
				if (blnRet)
				{
					this.tsbWhisper.Enabled = false;
				}
				else
				{
					MessageBox.Show("密语失败！", "密语坐席", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
					this.tsbWhisper.Enabled = true;
				}
			}
			else
			{
				MessageBox.Show("不能呼叫自己！", "密语坐席", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
			}
		}

		private void tsbBargein_Click(object sender, EventArgs e)
		{
			FrmMonitor.Log.Debug("enter " + MethodBase.GetCurrentMethod().Name);
			if (this.agentBar1.AgentState == AgentBar.Agent_State.AGENT_STATUS_TALKING_EAVESDROP || this.agentBar1.AgentState == AgentBar.Agent_State.AGENT_STATUS_TALKING_WHISPER)
			{
				bool blnRet = this.agentBar1.DoBargein(this.agentBar1.EavesDropAgentNum);
				if (blnRet)
				{
					this.tsbBargein.Enabled = false;
				}
				else
				{
					MessageBox.Show("插话失败！", "插话坐席", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
					this.tsbBargein.Enabled = true;
				}
			}
			else if (this.lvwMonitorInfo.SelectedItems.Count == 0)
			{
				MessageBox.Show("请选择一个坐席！", "插话失败", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
			}
			else if (this.lvwMonitorInfo.SelectedItems[0].SubItems[1].Text != this.agentBar1.AgentID)
			{
				if (this.agentBar1.AgentStatus != Convert.ToString(9))
				{
					int new_state = 9;
					if (!this.agentBar1.DoSetAgentDefineStatus(new_state, 1))
					{
						MessageBox.Show("更改坐席状态为：监控 失败！", "更改坐席状态", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
						return;
					}
				}
				bool blnRet = this.agentBar1.DoBargein(this.lvwMonitorInfo.SelectedItems[0].SubItems[1].Text);
				if (blnRet)
				{
					this.tsbBargein.Enabled = false;
				}
				else
				{
					MessageBox.Show("插话失败！", "插话坐席", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
					this.tsbBargein.Enabled = true;
				}
			}
			else
			{
				MessageBox.Show("不能呼叫自己！", "插话坐席", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
			}
		}

		private void tsbForceHangup_Click(object sender, EventArgs e)
		{
			FrmMonitor.Log.Debug("enter " + MethodBase.GetCurrentMethod().Name);
			if (this.agentBar1.AgentState == AgentBar.Agent_State.AGENT_STATUS_TALKING_EAVESDROP || this.agentBar1.AgentState == AgentBar.Agent_State.AGENT_STATUS_TALKING_WHISPER || this.agentBar1.AgentState == AgentBar.Agent_State.AGENT_STATUS_TALKING_BARGEIN)
			{
				bool blnRet = this.agentBar1.DoForceHangup(this.agentBar1.EavesDropAgentNum);
				if (blnRet)
				{
					this.tsbForceHangup.Enabled = false;
				}
				else
				{
					MessageBox.Show("强拆失败！", "强拆坐席", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
					this.tsbForceHangup.Enabled = true;
				}
			}
			else if (this.lvwMonitorInfo.SelectedItems.Count == 0)
			{
				MessageBox.Show("请选择一个坐席！", "强拆失败", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
			}
			else if (this.lvwMonitorInfo.SelectedItems[0].SubItems[1].Text != this.agentBar1.AgentID)
			{
				if (this.agentBar1.AgentStatus != Convert.ToString(9))
				{
					int new_state = 9;
					if (!this.agentBar1.DoSetAgentDefineStatus(new_state, 1))
					{
						MessageBox.Show("更改坐席状态为：监控 失败！", "更改坐席状态", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
						return;
					}
				}
				bool blnRet = this.agentBar1.DoForceHangup(this.lvwMonitorInfo.SelectedItems[0].SubItems[1].Text);
				if (blnRet)
				{
					this.tsbForceHangup.Enabled = false;
				}
				else
				{
					MessageBox.Show("强拆失败！", "强拆坐席", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
					this.tsbForceHangup.Enabled = true;
				}
			}
			else
			{
				MessageBox.Show("不能强拆自己！", "强拆坐席", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
			}
		}

		private void lvwMonitorInfo_Resize(object sender, EventArgs e)
		{
			if (base.WindowState != FormWindowState.Minimized)
			{
				this.LastWindowsState = base.WindowState;
			}
		}

		private void lvwMonitorInfo_DoubleClick(object sender, EventArgs e)
		{
			this.showCallDetailInfo();
		}
	}
}
