using PLAgentBar.Properties;
using PLAgentDll;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace PLAgentBar
{
	public class FrmDetailCallInfo : Form
	{
		public delegate void DoGetDetailCallInfoEventHandler(string targetAgentNum);

		private const int WM_NCLBUTTONDOWN = 161;

		private const int HTCAPTION = 2;

		private IContainer components = null;

		private TreeListView tlvDetailCallInfo;

		private ColumnHeader columnHeader1;

		private ColumnHeader columnHeader2;

		private ColumnHeader columnHeader3;

		private ColumnHeader columnHeader4;

		private ColumnHeader columnHeader5;

		private ColumnHeader columnHeader6;

		private ImageList imageList1;

		private Timer tmrDoGetDetail;

		private PictureBox pictureBox2;

		private string mAgentNum;

        public event FrmDetailCallInfo.DoGetDetailCallInfoEventHandler DoGetDetailCallInfoEvent;

		public string AgentNum
		{
			get
			{
				return this.mAgentNum;
			}
			set
			{
				this.mAgentNum = value;
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
			TreeListViewItemCollection.TreeListViewItemCollectionComparer treeListViewItemCollectionComparer = new TreeListViewItemCollection.TreeListViewItemCollectionComparer();
			ComponentResourceManager resources = new ComponentResourceManager(typeof(FrmDetailCallInfo));
			this.tlvDetailCallInfo = new TreeListView();
			this.columnHeader1 = new ColumnHeader();
			this.columnHeader2 = new ColumnHeader();
			this.columnHeader3 = new ColumnHeader();
			this.columnHeader4 = new ColumnHeader();
			this.columnHeader5 = new ColumnHeader();
			this.columnHeader6 = new ColumnHeader();
			this.imageList1 = new ImageList(this.components);
			this.tmrDoGetDetail = new Timer(this.components);
			this.pictureBox2 = new PictureBox();
			((ISupportInitialize)this.pictureBox2).BeginInit();
			base.SuspendLayout();
			this.tlvDetailCallInfo.Columns.AddRange(new ColumnHeader[]
			{
				this.columnHeader1,
				this.columnHeader2,
				this.columnHeader3,
				this.columnHeader4,
				this.columnHeader5,
				this.columnHeader6
			});
			treeListViewItemCollectionComparer.Column = 0;
			treeListViewItemCollectionComparer.SortOrder = SortOrder.Ascending;
			this.tlvDetailCallInfo.Comparer = treeListViewItemCollectionComparer;
			this.tlvDetailCallInfo.Dock = DockStyle.Fill;
			this.tlvDetailCallInfo.Location = new Point(10, 10);
			this.tlvDetailCallInfo.Name = "tlvDetailCallInfo";
			this.tlvDetailCallInfo.ShowGroups = false;
			this.tlvDetailCallInfo.Size = new Size(564, 201);
			this.tlvDetailCallInfo.SmallImageList = this.imageList1;
			this.tlvDetailCallInfo.TabIndex = 0;
			this.tlvDetailCallInfo.UseCompatibleStateImageBehavior = false;
			this.tlvDetailCallInfo.BeforeExpand += new TreeListViewCancelEventHandler(this.treeListView1_BeforeExpand);
			this.tlvDetailCallInfo.SelectedIndexChanged += new EventHandler(this.treeListView1_SelectedIndexChanged);
			this.tlvDetailCallInfo.BeforeCollapse += new TreeListViewCancelEventHandler(this.treeListView1_BeforeCollapse);
			this.tlvDetailCallInfo.MouseEnter += new EventHandler(this.treeListView1_MouseEnter);
			this.tlvDetailCallInfo.Leave += new EventHandler(this.treeListView1_MouseEnter);
			this.tlvDetailCallInfo.MouseMove += new MouseEventHandler(this.treeListView1_MouseMove);
			this.tlvDetailCallInfo.MouseDown += new MouseEventHandler(this.treeListView1_MouseDown);
			this.columnHeader1.Text = "通话类型/状态";
			this.columnHeader1.Width = 119;
			this.columnHeader2.Text = "工号";
			this.columnHeader2.Width = 51;
			this.columnHeader3.Text = "外线/分机";
			this.columnHeader3.Width = 71;
			this.columnHeader4.Text = "号码";
			this.columnHeader4.Width = 141;
			this.columnHeader5.Text = "状态";
			this.columnHeader5.Width = 68;
			this.columnHeader6.Text = "产生原因";
			this.columnHeader6.Width = 106;
			this.imageList1.ImageStream = (ImageListStreamer)resources.GetObject("imageList1.ImageStream");
			this.imageList1.TransparentColor = Color.Transparent;
			this.imageList1.Images.SetKeyName(0, "525FLOP1.bmp");
			this.imageList1.Images.SetKeyName(1, "CLSDFOLD.bmp");
			this.imageList1.Images.SetKeyName(2, "OPENFOLD.bmp");
			this.imageList1.Images.SetKeyName(3, "NOTE12.bmp");
			this.imageList1.Images.SetKeyName(4, "talk");
			this.imageList1.Images.SetKeyName(5, "eavesdrop");
			this.imageList1.Images.SetKeyName(6, "group");
			this.imageList1.Images.SetKeyName(7, "callin");
			this.imageList1.Images.SetKeyName(8, "Close.bmp");
			this.imageList1.Images.SetKeyName(9, "calling");
			this.imageList1.Images.SetKeyName(10, "hold");
			this.imageList1.Images.SetKeyName(11, "mute");
			this.tmrDoGetDetail.Tick += new EventHandler(this.tmrDoGetDetail_Tick);
			this.pictureBox2.ErrorImage = Resources.Close;
			this.pictureBox2.Image = Resources.Close;
			this.pictureBox2.Location = new Point(565, 0);
			this.pictureBox2.Name = "pictureBox2";
			this.pictureBox2.Size = new Size(17, 17);
			this.pictureBox2.SizeMode = PictureBoxSizeMode.AutoSize;
			this.pictureBox2.TabIndex = 7;
			this.pictureBox2.TabStop = false;
			this.pictureBox2.MouseMove += new MouseEventHandler(this.pictureBox2_MouseMove);
			this.pictureBox2.Click += new EventHandler(this.pictureBox2_Click);
			this.pictureBox2.MouseEnter += new EventHandler(this.pictureBox2_MouseEnter);
			base.AutoScaleDimensions = new SizeF(6f, 12f);
			base.AutoScaleMode = AutoScaleMode.Font;
			base.ClientSize = new Size(584, 221);
			base.Controls.Add(this.pictureBox2);
			base.Controls.Add(this.tlvDetailCallInfo);
			base.FormBorderStyle = FormBorderStyle.None;
			base.Name = "FrmDetailCallInfo";
			base.Padding = new Padding(10);
			base.ShowInTaskbar = false;
			base.StartPosition = FormStartPosition.Manual;
			this.Text = "通话明细";
			base.Deactivate += new EventHandler(this.Form2_Deactivate);
			base.Load += new EventHandler(this.Form2_Load);
			base.ControlAdded += new ControlEventHandler(this.Form2_ControlAdded);
			base.Leave += new EventHandler(this.Form2_Leave);
			base.MouseDown += new MouseEventHandler(this.Form2_MouseDown);
			base.Validating += new CancelEventHandler(this.Form2_Validating);
			base.MouseMove += new MouseEventHandler(this.Form2_MouseMove);
			((ISupportInitialize)this.pictureBox2).EndInit();
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		[DllImport("user32.dll", EntryPoint = "SendMessageA")]
		private static extern int SendMessage(int hwnd, int wMsg, int wParam, int lParam);

		[DllImport("user32.dll", EntryPoint = "ReleaseCapture")]
		private static extern int FreeCursor();

		public FrmDetailCallInfo()
		{
			this.InitializeComponent();
		}

		private void treeListView1_SelectedIndexChanged(object sender, EventArgs e)
		{
		}

		private void Form2_Load(object sender, EventArgs e)
		{
			this.initTlvw();
		}

		private bool IsInFrm(MouseEventArgs e)
		{
			int eX = e.Location.X;
			int eY = e.Location.Y;
			int XL = 0;
			int XR = base.Width;
			int YT = 0;
			int YB = base.Height;
			int bound = 10;
			bool result;
			if (eX < XL + bound || eX > XR - bound || eY < YT + bound || eY > YB - bound)
			{
				this.Cursor = Cursors.SizeAll;
				result = true;
			}
			else
			{
				this.Cursor = Cursors.Default;
				result = false;
			}
			return result;
		}

		private void initTlvw()
		{
			this.tlvDetailCallInfo.Items.Clear();
			this.tmrDoGetDetail.Interval = 3000;
			this.tmrDoGetDetail.Start();
			this.DoGetDetailCallInfo(this.mAgentNum);
		}

		private void AddItems()
		{
			TreeListViewItem itemA = new TreeListViewItem("普通呼入", 7);
			itemA.Expand();
			TreeListViewItem itemA2 = new TreeListViewItem("监听", 5);
			itemA.Items.Add(itemA2);
			TreeListViewItem itemA3 = new TreeListViewItem("通话", 4);
			itemA.Items.Add(itemA3);
			TreeListViewItem itemA4 = new TreeListViewItem("", 4);
			itemA4.SubItems.Add("1001");
			itemA4.SubItems.Add("分机");
			itemA4.SubItems.Add("1001");
			itemA4.SubItems.Add("通话");
			itemA4.SubItems.Add("普通呼入");
			itemA2.Items.Add(itemA4);
			TreeListViewItem itemA5 = new TreeListViewItem("", 4);
			itemA5.SubItems.Add("2001");
			itemA5.SubItems.Add("分机");
			itemA5.SubItems.Add("2001");
			itemA5.SubItems.Add("通话");
			itemA5.SubItems.Add("普通呼入");
			itemA2.Items.Add(itemA5);
			TreeListViewItem itemB0 = new TreeListViewItem("", 4);
			itemB0.SubItems.Add("1001");
			itemB0.SubItems.Add("分机");
			itemB0.SubItems.Add("1001");
			itemB0.SubItems.Add("通话");
			itemB0.SubItems.Add("普通呼入");
			itemA3.Items.Add(itemB0);
			TreeListViewItem itemB = new TreeListViewItem("", 4);
			itemB.SubItems.Add("10086");
			itemB.SubItems.Add("外线");
			itemB.SubItems.Add("10086");
			itemB.SubItems.Add("通话");
			itemB.SubItems.Add("普通呼入");
			itemA3.Items.Add(itemB);
			this.tlvDetailCallInfo.Items.Add(itemA);
		}

		private void treeListView1_BeforeCollapse(object sender, TreeListViewCancelEventArgs e)
		{
			if (e.Item.ImageIndex == 2)
			{
				e.Item.ImageIndex = 1;
			}
		}

		private void treeListView1_BeforeExpand(object sender, TreeListViewCancelEventArgs e)
		{
			if (e.Item.ImageIndex == 1)
			{
				e.Item.ImageIndex = 2;
			}
		}

		private void treeListView1_MouseMove(object sender, MouseEventArgs e)
		{
		}

		private void treeListView1_MouseDown(object sender, MouseEventArgs e)
		{
		}

		private void Form2_MouseMove(object sender, MouseEventArgs e)
		{
			this.IsInFrm(e);
		}

		private void Form2_MouseDown(object sender, MouseEventArgs e)
		{
			if (this.IsInFrm(e))
			{
				FrmDetailCallInfo.FreeCursor();
				FrmDetailCallInfo.SendMessage((int)base.Handle, 161, 2, 0);
			}
			else
			{
				base.Close();
			}
		}

		private void Form2_Leave(object sender, EventArgs e)
		{
		}

		private void Form2_ControlAdded(object sender, ControlEventArgs e)
		{
		}

		private void Form2_Deactivate(object sender, EventArgs e)
		{
			base.Close();
		}

		private void Form2_Validating(object sender, CancelEventArgs e)
		{
		}

		private void treeListView1_MouseEnter(object sender, EventArgs e)
		{
			this.Cursor = Cursors.Default;
		}

		private bool DoGetDetailCallInfo(string targetAgentNum)
		{
			bool result;
			if (targetAgentNum == null || targetAgentNum == "")
			{
				result = false;
			}
			else
			{
				if (this.DoGetDetailCallInfoEvent != null)
				{
					this.DoGetDetailCallInfoEvent(targetAgentNum);
				}
				result = true;
			}
			return result;
		}

		public void Evt_Get_Detail_Call_Info(string targetAgentNum, string call_type, List<Leg_Info_Struct> leg_list, List<Relation_Info_Struct> relation_list)
		{
			this.tlvDetailCallInfo.Items.Clear();
			TreeListViewItem itemCallType = new TreeListViewItem(FrmDetailCallInfo.CallType2Str(call_type), 7);
			itemCallType.Expand();
			for (int i = 0; i < relation_list.Count; i++)
			{
				TreeListViewItem new_relation_item = new TreeListViewItem(FrmDetailCallInfo.RelationName2Str(relation_list[i].relation_name), 5);
				new_relation_item.Expand();
				for (int j = 0; j < leg_list.Count; j++)
				{
					if (leg_list[j].leg_uuid == relation_list[i].callee_uuid || leg_list[j].leg_uuid == relation_list[i].caller_uuid)
					{
						TreeListViewItem new_leg_item = new TreeListViewItem("", 4);
						new_leg_item.SubItems.Add(leg_list[j].agentNum);
						if (leg_list[j].end_is_outbound)
						{
							new_leg_item.SubItems.Add("外部电话");
						}
						else
						{
							new_leg_item.SubItems.Add("内部电话");
						}
						new_leg_item.SubItems.Add(leg_list[j].end_num);
						new_leg_item.SubItems.Add(FrmDetailCallInfo.LegStatus2Str(leg_list[j].leg_status));
						new_leg_item.SubItems.Add(FrmDetailCallInfo.Lgr2Str(leg_list[j].lgr));
						new_relation_item.Items.Add(new_leg_item);
					}
				}
				itemCallType.Items.Add(new_relation_item);
			}
			this.tlvDetailCallInfo.Items.Add(itemCallType);
		}

		private static string CallType2Str(string call_type)
		{
			string result;
			if (call_type != null)
			{
				if (call_type == "CTI_CALL_TYPE_COMMON_CALL_IN")
				{
					result = "普通呼入";
					return result;
				}
				if (call_type == "CTI_CALL_TYPE_MANUAL_CALL_OUT")
				{
					result = "手动呼出";
					return result;
				}
				if (call_type == "CTI_CALL_TYPE_PREDICT_CALL_OUT")
				{
					result = "预测式呼出";
					return result;
				}
				if (call_type == "CTI_CALL_TYPE_AGENT_INTERNAL_CALL")
				{
					result = "坐席互打";
					return result;
				}
				if (call_type == "CTI_CALL_TYPE_ECHO_TEST")
				{
					result = "环回测试";
					return result;
				}
			}
			result = "未知";
			return result;
		}

		private static string RelationName2Str(string relationName)
		{
			string result;
			switch (relationName)
			{
			case "CALL_LEG_RELATION_TYPE_BRIDGE":
				result = "通话";
				return result;
			case "CALL_LEG_RELATION_TYPE_EAVESDROP":
				result = "监听";
				return result;
			case "CALL_LEG_RELATION_TYPE_WHISPER":
				result = "密语";
				return result;
			case "CALL_LEG_RELATION_TYPE_BARGEIN":
				result = "强插";
				return result;
			case "CALL_LEG_RELATION_TYPE_THREEWAY":
				result = "三方通话";
				return result;
			case "CALL_LEG_RELATION_TYPE_CONSULT":
				result = "询问";
				return result;
			}
			result = "未知";
			return result;
		}

		private static string LegStatus2Str(string legStatus)
		{
			string result;
			switch (legStatus)
			{
			case "LEG_STATUS_INIT":
				result = "初始化";
				return result;
			case "LEG_STATUS_RING":
				result = "响铃";
				return result;
			case "LEG_STATUS_PARK":
				result = "通话";
				return result;
			case "LEG_STATUS_PLAY":
				result = "放音";
				return result;
			case "LEG_STATUS_TALK":
				result = "通话";
				return result;
			case "LEG_STATUS_HOLD":
				result = "保持";
				return result;
			case "LEG_STATUS_MUTE":
				result = "静音";
				return result;
			}
			result = "未知";
			return result;
		}

		private static string Lgr2Str(string lgr)
		{
			string result;
			switch (lgr)
			{
			case "LEG_GENERATE_REASON_COMMON_CALLIN_ACTIVE":
				result = "普通呼入-主叫";
				return result;
			case "LEG_GENERATE_REASON_MANUAL_CALLOUT_ACTIVE":
				result = "手动呼出-主叫";
				return result;
			case "LEG_GENERATE_REASON_EAVESDROP_ACTIVE":
				result = "监听发起者";
				return result;
			case "LEG_GENERATE_REASON_WHISPER_ACTIVE":
				result = "密语发起者";
				return result;
			case "LEG_GENERATE_REASON_BARGEIN_ACTIVE":
				result = "强插发起者";
				return result;
			case "LEG_GENERATE_REASON_FORCE_HANGUP_ACTIVE":
				result = "强拆发起者";
				return result;
			case "LEG_GENERATE_REASON_ECHO_TEST_ACTIVE":
				result = "环回测试";
				return result;
			case "LEG_GENERATE_REASON_BRIDGE_PASSIVE":
				result = "应答的坐席";
				return result;
			case "LEG_GENERATE_REASON_MANUAL_CALLOUT_PASSIVE":
			case "LEG_GENERATE_REASON_PREVIEW_CALLOUT_PASSIVE":
				result = "手动呼出-被叫";
				return result;
			case "LEG_GENERATE_REASON_PREDICT_CALLOUT_PASSIVE":
				result = "预测式外呼-被叫";
				return result;
			case "LEG_GENERATE_REASON_BLIND_TRANSFER_PASSIVE":
				result = "盲转-被叫";
				return result;
			case "LEG_GENERATE_REASON_CONSULT_PASSIVE":
				result = "询问-被叫";
				return result;
			case "LEG_GENERATE_REASON_THREEWAY_PASSIVE":
				result = "三方通话-第三方";
				return result;
			}
			result = "未知";
			return result;
		}

		private void tmrDoGetDetail_Tick(object sender, EventArgs e)
		{
			this.DoGetDetailCallInfo(this.mAgentNum);
		}

		private void pictureBox2_Click(object sender, EventArgs e)
		{
			base.Close();
		}

		private void pictureBox2_MouseEnter(object sender, EventArgs e)
		{
		}

		private void pictureBox2_MouseMove(object sender, MouseEventArgs e)
		{
			Cursor.Current = Cursors.Default;
		}
	}
}
