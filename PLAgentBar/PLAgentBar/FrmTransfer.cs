using PLAgentDll;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace PLAgentBar
{
	public class FrmTransfer : Form
	{
		public delegate void DoGetAgentOnlineEventHandler(string specific, int numType);

		private IContainer components = null;

		public TextBox txtAgentID;

		private Button btnTransfer;

		private ImageList imgLstMonitor;

		private Timer tmrGetOnlineAgent;

		private Button btnCancel;

		public RadioButton chkTransferAgent;

		public RadioButton chkOutbound;

		private ListView lvwAgent;

		private TreeView tvwAgentGroup;

		private ImageList imageList1;

		private string mAgentNum;

		private Dictionary<string, string> mAgentDefineStatus;

		private string mLastSelectAgentNum = "";

		private List<Agent_Online_Struct> mAgentOnline;

		private AgentBar agentBar1;

		private string strOutboundFlag;

		private Color back_color;

		private Color fore_color;

		private bool HasLoaded_Agent_Group_List = false;

		private string mControledAgentGroupLstStr;

		private AgentBar.Agent_Or_AgentGroup mAgentOrAgentGroup;

		private bool isRootNode = false;

        public event FrmTransfer.DoGetAgentOnlineEventHandler DoGetAgentOnlineEvent;

		public string ControledAgentGroupLstStr
		{
			get
			{
				return this.mControledAgentGroupLstStr;
			}
			set
			{
				this.mControledAgentGroupLstStr = value;
			}
		}

		public AgentBar.Agent_Or_AgentGroup AgentOrAgentGroup
		{
			get
			{
				return this.mAgentOrAgentGroup;
			}
			set
			{
				this.mAgentOrAgentGroup = value;
			}
		}

		public string OutboundFlag
		{
			get
			{
				return this.strOutboundFlag;
			}
			set
			{
				this.strOutboundFlag = value;
			}
		}

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

		public Dictionary<string, string> AgentDefineStatus
		{
			set
			{
				this.mAgentDefineStatus = value;
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
			ComponentResourceManager resources = new ComponentResourceManager(typeof(FrmTransfer));
			this.txtAgentID = new TextBox();
			this.btnTransfer = new Button();
			this.imgLstMonitor = new ImageList(this.components);
			this.tmrGetOnlineAgent = new Timer(this.components);
			this.btnCancel = new Button();
			this.chkTransferAgent = new RadioButton();
			this.chkOutbound = new RadioButton();
			this.lvwAgent = new ListView();
			this.tvwAgentGroup = new TreeView();
			this.imageList1 = new ImageList(this.components);
			base.SuspendLayout();
			this.txtAgentID.Anchor = (AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left);
			this.txtAgentID.Location = new Point(12, 33);
			this.txtAgentID.Name = "txtAgentID";
			this.txtAgentID.Size = new Size(231, 21);
			this.txtAgentID.TabIndex = 0;
			this.txtAgentID.TextChanged += new EventHandler(this.txtAgentID_TextChanged);
			this.txtAgentID.KeyPress += new KeyPressEventHandler(this.txtAgentID_KeyPress);
			this.btnTransfer.DialogResult = DialogResult.OK;
			this.btnTransfer.Location = new Point(250, 30);
			this.btnTransfer.Name = "btnTransfer";
			this.btnTransfer.Size = new Size(75, 26);
			this.btnTransfer.TabIndex = 3;
			this.btnTransfer.Text = "确定";
			this.btnTransfer.UseVisualStyleBackColor = true;
			this.btnTransfer.Click += new EventHandler(this.btnTransfer_Click);
			this.imgLstMonitor.ImageStream = (ImageListStreamer)resources.GetObject("imgLstMonitor.ImageStream");
			this.imgLstMonitor.TransparentColor = Color.Transparent;
			this.imgLstMonitor.Images.SetKeyName(0, "meal");
			this.imgLstMonitor.Images.SetKeyName(1, "occupy");
			this.imgLstMonitor.Images.SetKeyName(2, "offline");
			this.imgLstMonitor.Images.SetKeyName(3, "acw");
			this.imgLstMonitor.Images.SetKeyName(4, "hold");
			this.imgLstMonitor.Images.SetKeyName(5, "leave");
			this.imgLstMonitor.Images.SetKeyName(6, "idle");
			this.imgLstMonitor.Images.SetKeyName(7, "busy");
			this.imgLstMonitor.Images.SetKeyName(8, "talk");
			this.imgLstMonitor.Images.SetKeyName(9, "ring");
			this.imgLstMonitor.Images.SetKeyName(10, "manage");
			this.imgLstMonitor.Images.SetKeyName(11, "calling");
			this.imgLstMonitor.Images.SetKeyName(12, "mute");
			this.tmrGetOnlineAgent.Tick += new EventHandler(this.tmrGetOnlineAgent_Tick);
			this.btnCancel.DialogResult = DialogResult.Cancel;
			this.btnCancel.Location = new Point(331, 30);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new Size(75, 26);
			this.btnCancel.TabIndex = 4;
			this.btnCancel.Text = "取消";
			this.btnCancel.UseVisualStyleBackColor = true;
			this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
			this.chkTransferAgent.AutoSize = true;
			this.chkTransferAgent.Checked = true;
			this.chkTransferAgent.Location = new Point(26, 12);
			this.chkTransferAgent.Name = "chkTransferAgent";
			this.chkTransferAgent.Size = new Size(47, 16);
			this.chkTransferAgent.TabIndex = 1;
			this.chkTransferAgent.TabStop = true;
			this.chkTransferAgent.Text = "坐席";
			this.chkTransferAgent.UseVisualStyleBackColor = true;
			this.chkTransferAgent.CheckedChanged += new EventHandler(this.chkTransferAgent_CheckedChanged);
			this.chkOutbound.AutoSize = true;
			this.chkOutbound.Location = new Point(132, 12);
			this.chkOutbound.Name = "chkOutbound";
			this.chkOutbound.Size = new Size(47, 16);
			this.chkOutbound.TabIndex = 2;
			this.chkOutbound.Text = "外线";
			this.chkOutbound.UseVisualStyleBackColor = true;
			this.chkOutbound.CheckedChanged += new EventHandler(this.chkOutbound_CheckedChanged);
			this.lvwAgent.Anchor = (AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right);
			this.lvwAgent.FullRowSelect = true;
			this.lvwAgent.GridLines = true;
			this.lvwAgent.LargeImageList = this.imgLstMonitor;
			this.lvwAgent.Location = new Point(249, 62);
			this.lvwAgent.Name = "lvwAgent";
			this.lvwAgent.Size = new Size(442, 337);
			this.lvwAgent.SmallImageList = this.imgLstMonitor;
			this.lvwAgent.TabIndex = 6;
			this.lvwAgent.UseCompatibleStateImageBehavior = false;
			this.lvwAgent.View = View.Details;
			this.lvwAgent.SelectedIndexChanged += new EventHandler(this.lvwAgent_SelectedIndexChanged);
			this.lvwAgent.DoubleClick += new EventHandler(this.lvwAgent_DoubleClick);
			this.lvwAgent.Leave += new EventHandler(this.lvwAgent_Leave);
			this.tvwAgentGroup.Anchor = (AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left);
			this.tvwAgentGroup.ImageIndex = 0;
			this.tvwAgentGroup.ImageList = this.imageList1;
			this.tvwAgentGroup.Location = new Point(12, 64);
			this.tvwAgentGroup.Name = "tvwAgentGroup";
			this.tvwAgentGroup.SelectedImageIndex = 0;
			this.tvwAgentGroup.Size = new Size(231, 335);
			this.tvwAgentGroup.TabIndex = 5;
			this.tvwAgentGroup.AfterSelect += new TreeViewEventHandler(this.tvwAgentGroup_AfterSelect);
			this.tvwAgentGroup.Leave += new EventHandler(this.tvwAgentGroup_Leave);
			this.tvwAgentGroup.BeforeSelect += new TreeViewCancelEventHandler(this.tvwAgentGroup_BeforeSelect);
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
			base.AcceptButton = this.btnTransfer;
			base.AutoScaleDimensions = new SizeF(6f, 12f);
			base.AutoScaleMode = AutoScaleMode.Font;
			base.CancelButton = this.btnCancel;
			base.ClientSize = new Size(707, 411);
			base.Controls.Add(this.tvwAgentGroup);
			base.Controls.Add(this.chkOutbound);
			base.Controls.Add(this.chkTransferAgent);
			base.Controls.Add(this.btnCancel);
			base.Controls.Add(this.txtAgentID);
			base.Controls.Add(this.btnTransfer);
			base.Controls.Add(this.lvwAgent);
			base.Icon = (Icon)resources.GetObject("$this.Icon");
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "FrmTransfer";
			base.StartPosition = FormStartPosition.CenterParent;
			this.Text = "转接";
			base.Load += new EventHandler(this.FrmTransfer_Load);
			base.FormClosed += new FormClosedEventHandler(this.FrmTransfer_FormClosed);
			base.FormClosing += new FormClosingEventHandler(this.FrmTransfer_FormClosing);
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		public FrmTransfer()
		{
			this.InitializeComponent();
			this.OutboundFlag = "false";
		}

		private void initLvwAgent()
		{
			this.lvwAgent.Items.Clear();
			this.lvwAgent.Columns.Add("状态", 100, HorizontalAlignment.Left);
			this.lvwAgent.Columns.Add("坐席号", 80, HorizontalAlignment.Left);
			this.lvwAgent.Columns.Add("坐席姓名", 120, HorizontalAlignment.Left);
			this.lvwAgent.Columns.Add("坐席组名称", 120, HorizontalAlignment.Left);
		}

		private void initLvwAgentGroupList()
		{
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

		public void Evt_Get_Agent_Group_List(Dictionary<string, string> agent_group_list)
		{
			if (!this.HasLoaded_Agent_Group_List)
			{
				TreeNode root = new TreeNode("坐席组");
				root.ImageKey = "agentGroup";
				root.ImageKey = (root.SelectedImageKey = "agentGroup");
				this.back_color = root.BackColor;
				this.fore_color = root.ForeColor;
				foreach (KeyValuePair<string, string> agent_group in agent_group_list)
				{
					TreeNode new_group = new TreeNode(agent_group.Value);
					new_group.Name = agent_group.Key;
					new_group.ImageKey = "group_agent";
					new_group.Tag = "group";
					new_group.ImageKey = (new_group.SelectedImageKey = "agentGroup");
					root.Nodes.Add(new_group);
				}
				this.tvwAgentGroup.Nodes.Add(root);
				this.tvwAgentGroup.SelectedNode = root;
				this.HasLoaded_Agent_Group_List = true;
				this.txtAgentID.Focus();
			}
		}

		public void Evt_GetOnlineAgent(List<Agent_Online_Struct> agent_online)
		{
			this.lvwAgent.BeginUpdate();
			this.lvwAgent.Items.Clear();
			this.mAgentOnline = agent_online;
			if (agent_online != null)
			{
				for (int i = 0; i < agent_online.Count; i++)
				{
					if (agent_online[i].agentNum.ToString().StartsWith(this.txtAgentID.Text) || !this.txtAgentID.Focused)
					{
						ListViewItem lvi = new ListViewItem();
						AgentBar.Agent_Status agent_status = AgentBar.Str2AgentStatus(agent_online[i].agentStatus);
						string agent_status_name = AgentBar.AgentStatus2Str(agent_status);
						if ("" == agent_status_name)
						{
							if (this.mAgentDefineStatus.ContainsKey(agent_online[i].agentStatus))
							{
								agent_status_name = this.mAgentDefineStatus[agent_online[i].agentStatus];
							}
							else
							{
								agent_status_name = "未知";
							}
							lvi.ImageKey = "busy";
						}
						else
						{
							agent_status_name = AgentBar.AgentStatus2Str(agent_status);
							lvi.ImageKey = AgentBar.IcoName_AgentStatus2Str(agent_status);
						}
						lvi.Text = agent_status_name;
						lvi.SubItems.Add(agent_online[i].agentNum.ToString());
						lvi.SubItems.Add(agent_online[i].agentName.ToString());
						lvi.SubItems.Add(agent_online[i].agentgroup_name.ToString());
						this.lvwAgent.Items.Add(lvi);
					}
				}
				this.lvwAgent.EndUpdate();
				this.selectLastChoose();
			}
		}

		private void selectLastChoose()
		{
			if (!(this.mLastSelectAgentNum == ""))
			{
				for (int i = 0; i < this.lvwAgent.Items.Count; i++)
				{
					if (this.lvwAgent.Items[i].SubItems[1].Text == this.mLastSelectAgentNum)
					{
						this.lvwAgent.Items[i].Selected = true;
						break;
					}
				}
			}
		}

		private void FrmTransfer_Load(object sender, EventArgs e)
		{
			this.tmrGetOnlineAgent.Interval = 3000;
			this.tmrGetOnlineAgent.Start();
			this.chkTransferAgent.Checked = true;
			this.btnTransfer.Enabled = false;
			this.initLvwAgent();
			this.initLvwAgentGroupList();
			if (string.IsNullOrEmpty(this.mControledAgentGroupLstStr) && this.mAgentOrAgentGroup == AgentBar.Agent_Or_AgentGroup.AGENTGROUPNUM)
			{
				MessageBox.Show("没有可以管理的坐席组！", "获取信息失败", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
			}
			else
			{
				this.getAgentsOnline(this.mControledAgentGroupLstStr, (int)this.mAgentOrAgentGroup);
			}
		}

		private void lvwAgent_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (this.lvwAgent.SelectedItems.Count > 0)
			{
				this.txtAgentID.Text = this.lvwAgent.SelectedItems[0].SubItems[1].Text;
				this.mLastSelectAgentNum = this.lvwAgent.SelectedItems[0].SubItems[1].Text;
			}
			if (this.mAgentNum == this.txtAgentID.Text)
			{
				this.btnTransfer.Enabled = false;
			}
			else
			{
				this.btnTransfer.Enabled = true;
			}
		}

		private void btnCancel_Click(object sender, EventArgs e)
		{
			base.Close();
		}

		private void btnTransfer_Click(object sender, EventArgs e)
		{
			if (this.txtAgentID.Text == "")
			{
				base.DialogResult = DialogResult.No;
			}
			else if (!ComFunc.checkNumIsLegal(this.txtAgentID.Text.Trim()))
			{
				MessageBox.Show("选择的号码非法！", "操作失败", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
				base.DialogResult = DialogResult.No;
			}
			else
			{
				base.DialogResult = DialogResult.OK;
			}
		}

		private void FrmTransfer_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (base.DialogResult == DialogResult.No)
			{
				e.Cancel = true;
			}
		}

		private void tmrGetOnlineAgent_Tick(object sender, EventArgs e)
		{
			if (this.tvwAgentGroup.SelectedNode != null)
			{
				if (this.tvwAgentGroup.SelectedNode.Text == "坐席组" && this.isRootNode)
				{
					this.getAgentsOnline(this.mControledAgentGroupLstStr, (int)this.mAgentOrAgentGroup);
				}
				else
				{
					this.getAgentsOnline(this.tvwAgentGroup.SelectedNode.Name, 2);
				}
			}
		}

		private void getAgentsOnline(string specificNum, int numType)
		{
			if (this.agentBar1.IsConnected)
			{
				if (this.DoGetAgentOnlineEvent != null)
				{
					this.DoGetAgentOnlineEvent(specificNum, numType);
				}
			}
		}

		private void FrmTransfer_FormClosed(object sender, FormClosedEventArgs e)
		{
			this.tmrGetOnlineAgent.Stop();
		}

		private void txtAgentID_TextChanged(object sender, EventArgs e)
		{
			if (this.txtAgentID.Focused)
			{
				if (!this.chkTransferAgent.Checked)
				{
					if (this.chkOutbound.Checked && this.txtAgentID.Text != "")
					{
						this.btnTransfer.Enabled = true;
					}
					else
					{
						this.btnTransfer.Enabled = false;
					}
				}
				else
				{
					this.btnTransfer.Enabled = false;
					this.Evt_GetOnlineAgent(this.mAgentOnline);
					if (this.txtAgentID.Text == this.mAgentNum)
					{
						this.btnTransfer.Enabled = false;
					}
					else
					{
						for (int i = 0; i < this.lvwAgent.Items.Count; i++)
						{
							if (this.lvwAgent.Items[i].SubItems[1].Text == this.txtAgentID.Text)
							{
								this.btnTransfer.Enabled = true;
							}
							else
							{
								this.btnTransfer.Enabled = false;
							}
							if (!this.lvwAgent.Items[i].SubItems[1].Text.StartsWith(this.txtAgentID.Text))
							{
								this.lvwAgent.Items.RemoveAt(i);
							}
						}
					}
				}
			}
		}

		private void txtAgentID_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (!char.IsNumber(e.KeyChar) && e.KeyChar != '\r' && e.KeyChar != '\b' && e.KeyChar != '\u0001' && e.KeyChar != '\u0003' && e.KeyChar != '\u0016' && e.KeyChar != '\u0018')
			{
				e.Handled = true;
			}
			else if (e.KeyChar == '\u0001')
			{
				this.txtAgentID.SelectAll();
				this.txtAgentID.Focus();
			}
		}

		private void lvwAgent_Leave(object sender, EventArgs e)
		{
			this.mLastSelectAgentNum = "";
		}

		private void lvwAgent_DoubleClick(object sender, EventArgs e)
		{
			if (this.lvwAgent.SelectedItems.Count > 0)
			{
				this.txtAgentID.Text = this.lvwAgent.SelectedItems[0].SubItems[1].Text;
				this.mLastSelectAgentNum = this.lvwAgent.SelectedItems[0].SubItems[1].Text;
			}
			this.btnTransfer.Enabled = true;
			this.btnTransfer.PerformClick();
		}

		private void chkTransferAgent_CheckedChanged(object sender, EventArgs e)
		{
			if (this.chkTransferAgent.Checked)
			{
				this.tvwAgentGroup.Enabled = true;
				if (this.tvwAgentGroup.SelectedNode != null)
				{
					this.tvwAgentGroup.SelectedNode.BackColor = Color.Empty;
					this.tvwAgentGroup.SelectedNode.ForeColor = Color.Black;
				}
				this.lvwAgent.Enabled = true;
				this.OutboundFlag = "false";
				this.txtAgentID.Text = "";
				this.btnTransfer.Enabled = false;
				this.getAgentsOnline(this.mControledAgentGroupLstStr, (int)this.mAgentOrAgentGroup);
				this.tmrGetOnlineAgent.Start();
				if (this.tvwAgentGroup.SelectedNode != null)
				{
					Color c = Color.FromArgb(255, 49, 106, 197);
					this.tvwAgentGroup.SelectedNode.BackColor = c;
					this.tvwAgentGroup.SelectedNode.ForeColor = Color.White;
				}
				this.tvwAgentGroup.BackColor = this.lvwAgent.BackColor;
			}
		}

		private void initAllNodesForeColorAndBackColor(TreeNodeCollection aNodes)
		{
			foreach (TreeNode tn in aNodes)
			{
				tn.ForeColor = this.fore_color;
				tn.BackColor = this.back_color;
				if (tn.Nodes.Count > 0)
				{
					this.initAllNodesForeColorAndBackColor(tn.Nodes);
				}
			}
		}

		private void chkOutbound_CheckedChanged(object sender, EventArgs e)
		{
			if (this.chkOutbound.Checked)
			{
				this.OutboundFlag = "true";
				this.txtAgentID.Text = "";
				this.btnTransfer.Enabled = false;
				this.tmrGetOnlineAgent.Stop();
				this.lvwAgent.Items.Clear();
				this.tvwAgentGroup.Enabled = false;
				this.lvwAgent.Enabled = false;
				this.initAllNodesForeColorAndBackColor(this.tvwAgentGroup.Nodes);
				this.tvwAgentGroup.BackColor = this.btnTransfer.BackColor;
			}
		}

		private void tvwAgentGroup_AfterSelect(object sender, TreeViewEventArgs e)
		{
			if (e.Node.Parent == null)
			{
				this.isRootNode = true;
			}
			else
			{
				this.isRootNode = false;
			}
			if (this.tvwAgentGroup.SelectedNode != null)
			{
				if (this.tvwAgentGroup.SelectedNode.Text == "坐席组" && this.isRootNode)
				{
					if (!this.agentBar1.DoGetOnlineAgent(this.mControledAgentGroupLstStr, (int)this.mAgentOrAgentGroup))
					{
						MessageBox.Show("获得在线坐席失败！", "获得在线坐席", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
					}
				}
				else if (!this.agentBar1.DoGetOnlineAgent(this.tvwAgentGroup.SelectedNode.Name, 2))
				{
					MessageBox.Show("获得在线坐席失败！", "获得在线坐席", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
				}
			}
		}

		private void tvwAgentGroup_BeforeSelect(object sender, TreeViewCancelEventArgs e)
		{
			if (this.tvwAgentGroup.SelectedNode != null)
			{
				this.tvwAgentGroup.SelectedNode.BackColor = Color.Empty;
				this.tvwAgentGroup.SelectedNode.ForeColor = Color.Black;
			}
		}

		private void tvwAgentGroup_Leave(object sender, EventArgs e)
		{
			if (this.tvwAgentGroup.SelectedNode != null)
			{
				Color c = Color.FromArgb(255, 49, 106, 197);
				this.tvwAgentGroup.SelectedNode.BackColor = c;
				this.tvwAgentGroup.SelectedNode.ForeColor = Color.White;
			}
		}
	}
}
