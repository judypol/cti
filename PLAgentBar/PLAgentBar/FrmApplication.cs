using PLAgentDll;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace PLAgentBar
{
	public class FrmApplication : Form
	{
		private IContainer components = null;

		private TabControl applicationManage;

		private TabPage tabPage1;

		private TabPage tabPage2;

		private ListView lvwCurrent;

		private ListView lvwHistory;

		private TabPage tabPage3;

		private Label label3;

		private ComboBox cboAgentGroup;

		private Label label1;

		private Label label2;

		private Button btnOk;

		private TextBox txtMaxNum;

		private ToolStripButton tsbPass;

		private ToolStripButton tsbNopass;

		private ToolStrip toolStrip1;

		private NumericUpDown upMaxNum;

		public AgentBar agentBar1;

		public Dictionary<string, string> dicAgentGroupName;

		private bool HasLoaded_Agent_Group_List = false;

		public FormWindowState LastWindowsState = FormWindowState.Normal;

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
			ComponentResourceManager resources = new ComponentResourceManager(typeof(FrmApplication));
			this.applicationManage = new TabControl();
			this.tabPage1 = new TabPage();
			this.lvwCurrent = new ListView();
			this.tabPage2 = new TabPage();
			this.lvwHistory = new ListView();
			this.tabPage3 = new TabPage();
			this.upMaxNum = new NumericUpDown();
			this.txtMaxNum = new TextBox();
			this.label3 = new Label();
			this.cboAgentGroup = new ComboBox();
			this.label1 = new Label();
			this.label2 = new Label();
			this.btnOk = new Button();
			this.tsbPass = new ToolStripButton();
			this.tsbNopass = new ToolStripButton();
			this.toolStrip1 = new ToolStrip();
			this.applicationManage.SuspendLayout();
			this.tabPage1.SuspendLayout();
			this.tabPage2.SuspendLayout();
			this.tabPage3.SuspendLayout();
			((ISupportInitialize)this.upMaxNum).BeginInit();
			this.toolStrip1.SuspendLayout();
			base.SuspendLayout();
			this.applicationManage.AccessibleName = "";
			this.applicationManage.Anchor = (AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right);
			this.applicationManage.Controls.Add(this.tabPage1);
			this.applicationManage.Controls.Add(this.tabPage2);
			this.applicationManage.Controls.Add(this.tabPage3);
			this.applicationManage.Location = new Point(0, 42);
			this.applicationManage.Name = "applicationManage";
			this.applicationManage.SelectedIndex = 0;
			this.applicationManage.Size = new Size(811, 374);
			this.applicationManage.TabIndex = 0;
			this.applicationManage.SelectedIndexChanged += new EventHandler(this.applicationManage_SelectedIndexChanged);
			this.tabPage1.AccessibleName = "currentApply";
			this.tabPage1.Controls.Add(this.lvwCurrent);
			this.tabPage1.Location = new Point(4, 22);
			this.tabPage1.Name = "tabPage1";
			this.tabPage1.Padding = new Padding(3);
			this.tabPage1.Size = new Size(803, 348);
			this.tabPage1.TabIndex = 0;
			this.tabPage1.Text = "当前申请";
			this.tabPage1.UseVisualStyleBackColor = true;
			this.lvwCurrent.Dock = DockStyle.Fill;
			this.lvwCurrent.Location = new Point(3, 3);
			this.lvwCurrent.Name = "lvwCurrent";
			this.lvwCurrent.Size = new Size(797, 342);
			this.lvwCurrent.TabIndex = 0;
			this.lvwCurrent.UseCompatibleStateImageBehavior = false;
			this.lvwCurrent.View = View.Details;
			this.tabPage2.AccessibleName = "historyApply";
			this.tabPage2.Controls.Add(this.lvwHistory);
			this.tabPage2.Location = new Point(4, 22);
			this.tabPage2.Name = "tabPage2";
			this.tabPage2.Padding = new Padding(3);
			this.tabPage2.Size = new Size(803, 348);
			this.tabPage2.TabIndex = 1;
			this.tabPage2.Text = "历史申请";
			this.tabPage2.UseVisualStyleBackColor = true;
			this.lvwHistory.Dock = DockStyle.Fill;
			this.lvwHistory.Location = new Point(3, 3);
			this.lvwHistory.Name = "lvwHistory";
			this.lvwHistory.Size = new Size(797, 342);
			this.lvwHistory.TabIndex = 0;
			this.lvwHistory.UseCompatibleStateImageBehavior = false;
			this.tabPage3.AccessibleName = "approve";
			this.tabPage3.Controls.Add(this.upMaxNum);
			this.tabPage3.Controls.Add(this.txtMaxNum);
			this.tabPage3.Controls.Add(this.label3);
			this.tabPage3.Controls.Add(this.cboAgentGroup);
			this.tabPage3.Controls.Add(this.label1);
			this.tabPage3.Controls.Add(this.label2);
			this.tabPage3.Controls.Add(this.btnOk);
			this.tabPage3.Location = new Point(4, 22);
			this.tabPage3.Name = "tabPage3";
			this.tabPage3.Padding = new Padding(3);
			this.tabPage3.Size = new Size(803, 348);
			this.tabPage3.TabIndex = 2;
			this.tabPage3.Text = "审批设置";
			this.tabPage3.UseVisualStyleBackColor = true;
			this.upMaxNum.Location = new Point(135, 87);
			NumericUpDown arg_56B_0 = this.upMaxNum;
			int[] array = new int[4];
			array[0] = 1000;
			arg_56B_0.Maximum = new decimal(array);
			this.upMaxNum.Name = "upMaxNum";
			this.upMaxNum.Size = new Size(121, 21);
			this.upMaxNum.TabIndex = 12;
			this.txtMaxNum.Enabled = false;
			this.txtMaxNum.Location = new Point(135, 51);
			this.txtMaxNum.Name = "txtMaxNum";
			this.txtMaxNum.Size = new Size(121, 21);
			this.txtMaxNum.TabIndex = 3;
			this.label3.AutoSize = true;
			this.label3.Location = new Point(25, 22);
			this.label3.Name = "label3";
			this.label3.Size = new Size(89, 12);
			this.label3.TabIndex = 11;
			this.label3.Text = "请选择坐席组：";
			this.cboAgentGroup.DropDownStyle = ComboBoxStyle.DropDownList;
			this.cboAgentGroup.FormattingEnabled = true;
			this.cboAgentGroup.Location = new Point(135, 19);
			this.cboAgentGroup.Name = "cboAgentGroup";
			this.cboAgentGroup.Size = new Size(121, 20);
			this.cboAgentGroup.TabIndex = 2;
			this.cboAgentGroup.SelectedIndexChanged += new EventHandler(this.cboAgentGroup_SelectedIndexChanged);
			this.label1.AutoSize = true;
			this.label1.Location = new Point(25, 54);
			this.label1.Name = "label1";
			this.label1.Size = new Size(101, 12);
			this.label1.TabIndex = 8;
			this.label1.Text = "当前组离开阀值：";
			this.label2.AutoSize = true;
			this.label2.Location = new Point(25, 89);
			this.label2.Name = "label2";
			this.label2.Size = new Size(89, 12);
			this.label2.TabIndex = 7;
			this.label2.Text = "离席最大人数：";
			this.btnOk.Location = new Point(27, 130);
			this.btnOk.Name = "btnOk";
			this.btnOk.Size = new Size(67, 28);
			this.btnOk.TabIndex = 1;
			this.btnOk.Text = "确定";
			this.btnOk.UseVisualStyleBackColor = true;
			this.btnOk.Click += new EventHandler(this.btnOk_Click);
			this.tsbPass.DisplayStyle = ToolStripItemDisplayStyle.Image;
			this.tsbPass.Image = (Image)resources.GetObject("tsbPass.Image");
			this.tsbPass.ImageTransparentColor = Color.Magenta;
			this.tsbPass.Name = "tsbPass";
			this.tsbPass.Size = new Size(36, 36);
			this.tsbPass.Text = "审批通过";
			this.tsbPass.Click += new EventHandler(this.tsbPass_Click);
			this.tsbNopass.DisplayStyle = ToolStripItemDisplayStyle.Image;
			this.tsbNopass.Image = (Image)resources.GetObject("tsbNopass.Image");
			this.tsbNopass.ImageTransparentColor = Color.Magenta;
			this.tsbNopass.Name = "tsbNopass";
			this.tsbNopass.Size = new Size(36, 36);
			this.tsbNopass.Text = "审批不通过";
			this.tsbNopass.Click += new EventHandler(this.tsbNopass_Click);
			this.toolStrip1.ImageScalingSize = new Size(32, 32);
			this.toolStrip1.Items.AddRange(new ToolStripItem[]
			{
				this.tsbPass,
				this.tsbNopass
			});
			this.toolStrip1.Location = new Point(0, 0);
			this.toolStrip1.Name = "toolStrip1";
			this.toolStrip1.Size = new Size(811, 39);
			this.toolStrip1.TabIndex = 1;
			this.toolStrip1.Text = "toolStrip1";
			base.AutoScaleDimensions = new SizeF(6f, 12f);
			base.AutoScaleMode = AutoScaleMode.Font;
			base.ClientSize = new Size(811, 416);
			base.Controls.Add(this.toolStrip1);
			base.Controls.Add(this.applicationManage);
			base.FormBorderStyle = FormBorderStyle.FixedSingle;
			base.Name = "FrmApplication";
			base.StartPosition = FormStartPosition.CenterScreen;
			this.Text = "坐席离席申请管理";
			base.Load += new EventHandler(this.FrmApplication_Load);
			base.Resize += new EventHandler(this.FrmApplication_Resize);
			this.applicationManage.ResumeLayout(false);
			this.tabPage1.ResumeLayout(false);
			this.tabPage2.ResumeLayout(false);
			this.tabPage3.ResumeLayout(false);
			this.tabPage3.PerformLayout();
			((ISupportInitialize)this.upMaxNum).EndInit();
			this.toolStrip1.ResumeLayout(false);
			this.toolStrip1.PerformLayout();
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		public FrmApplication()
		{
			this.InitializeComponent();
		}

		private void initCurrentApplicationInfo()
		{
			this.lvwCurrent.AccessibleName = "current_application_info";
			this.lvwCurrent.FullRowSelect = true;
			this.lvwCurrent.Columns.Add("申请人工号");
			this.lvwCurrent.Columns[0].Width = 100;
			this.lvwCurrent.Columns.Add("申请人姓名");
			this.lvwCurrent.Columns[1].Width = 100;
			this.lvwCurrent.Columns.Add("申请人坐席组名称");
			this.lvwCurrent.Columns[2].Width = 120;
			this.lvwCurrent.Columns.Add("状态");
			this.lvwCurrent.Columns[3].Width = 100;
			this.lvwCurrent.Columns.Add("申请时间");
			this.lvwCurrent.Columns[4].Width = 120;
			this.lvwCurrent.Columns.Add("申请者类型");
			this.lvwCurrent.Columns[5].Width = 100;
			this.lvwCurrent.Items.Clear();
		}

		private void initHistoryApplicationInfo()
		{
			this.lvwHistory.AccessibleName = "history_application_info";
			this.lvwHistory.FullRowSelect = true;
			this.lvwHistory.Columns.Add("申请人工号");
			this.lvwHistory.Columns[0].Width = 100;
			this.lvwHistory.Columns.Add("申请人姓名");
			this.lvwHistory.Columns[1].Width = 100;
			this.lvwHistory.Columns.Add("申请人坐席组名称");
			this.lvwHistory.Columns[2].Width = 120;
			this.lvwHistory.Columns.Add("状态");
			this.lvwHistory.Columns[3].Width = 100;
			this.lvwHistory.Columns.Add("申请时间");
			this.lvwHistory.Columns[4].Width = 120;
			this.lvwHistory.Columns.Add("审批时间");
			this.lvwHistory.Columns[5].Width = 120;
			this.lvwHistory.Columns.Add("申请者类型");
			this.lvwHistory.Columns[6].Width = 100;
			this.lvwHistory.Columns.Add("原因");
			this.lvwHistory.Columns[7].Width = 120;
			this.lvwHistory.Items.Clear();
		}

		private void FrmApplication_Load(object sender, EventArgs e)
		{
			this.lvwCurrent.View = View.Details;
			this.lvwHistory.View = View.Details;
			this.initCurrentApplicationInfo();
			this.initHistoryApplicationInfo();
			this.initToolStrip();
			this.refreshForm();
			this.lvwCurrent.MultiSelect = false;
		}

		private void initToolStrip()
		{
			this.tsbPass.Enabled = this.agentBar1.AgentRoleAndRight.group_role_right1;
			this.tsbNopass.Enabled = this.agentBar1.AgentRoleAndRight.group_role_right1;
			if (!this.agentBar1.AgentRoleAndRight.group_role_right2)
			{
				this.applicationManage.TabPages.RemoveByKey("tabPage3");
			}
		}

		private void tsbPass_Click(object sender, EventArgs e)
		{
			this.doApprove("1");
		}

		private void doApprove(string approveResult)
		{
			if (this.lvwCurrent.SelectedItems.Count != 0)
			{
				int strStatusOfLeave = 7;
				if (!this.agentBar1.DoApplyApproval(this.lvwCurrent.SelectedItems[0].Text, strStatusOfLeave.ToString(), approveResult))
				{
					MessageBox.Show("审批失败！", "审批", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
				}
			}
		}

		private void showCurrentApplyRecord()
		{
			this.lvwCurrent.Items.Clear();
			for (int i = 0; i < this.agentBar1.GetApplyChangeStatusApprovalLst.Count; i++)
			{
				Apply_Change_Status new_item = this.agentBar1.GetApplyChangeStatusApprovalLst[i];
				this.addToCurrentApplyListView(new_item.applyAgentID, new_item.agentName, new_item.applyAgentGroupName, AgentBar.ApplyStatus2Str(new_item.applyState), new_item.applyTime, new_item.approveTime, new_item.applyType);
			}
			if (this.lvwCurrent.Items.Count > 0)
			{
				this.lvwCurrent.Items[0].Selected = true;
			}
		}

		private void showHistoryApplyRecord()
		{
			this.lvwHistory.Items.Clear();
			for (int i = 0; i < this.agentBar1.ApplyChangeStatusApprovalHistory.Count; i++)
			{
				Apply_Change_Status new_item = this.agentBar1.ApplyChangeStatusApprovalHistory[i];
				this.addToHistoryApplyListView(new_item.applyAgentID, new_item.agentName, new_item.applyAgentGroupName, AgentBar.ApplyStatus2Str(new_item.applyState), new_item.applyTime, new_item.approveTime, new_item.applyType, new_item.reason);
			}
		}

		private void applicationManage_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (this.applicationManage.SelectedTab.AccessibleName == "approve")
			{
				this.upMaxNum.Focus();
				if (!this.agentBar1.DoGetAgentGroupList(""))
				{
					MessageBox.Show("获取坐席组列表失败！", "获取失败", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				}
			}
			else
			{
				this.refreshForm();
			}
		}

		private void refreshForm()
		{
			this.lvwCurrent.SelectedItems.Clear();
			string accessibleName = this.applicationManage.SelectedTab.AccessibleName;
			if (accessibleName != null)
			{
				if (!(accessibleName == "currentApply"))
				{
					if (accessibleName == "historyApply")
					{
						this.showHistoryApplyRecord();
					}
				}
				else
				{
					this.showCurrentApplyRecord();
				}
			}
		}

		private void addToCurrentApplyListView(string agentID, string agentName, string agentGroupName, string agentStatus, string applyTime, string approveTime, string applyType)
		{
			ListViewItem new_lvwItem = new ListViewItem(agentID);
			new_lvwItem.SubItems.Add(agentName);
			new_lvwItem.SubItems.Add(agentGroupName);
			new_lvwItem.SubItems.Add(agentStatus);
			new_lvwItem.SubItems.Add(applyTime);
			new_lvwItem.SubItems.Add(applyType);
			this.lvwCurrent.Items.Add(new_lvwItem);
		}

		private void addToHistoryApplyListView(string agentID, string agentName, string agentGroupName, string agentStatus, string applyTime, string approveTime, string applyType, string reason)
		{
			ListViewItem new_lvwItem = new ListViewItem(agentID);
			new_lvwItem.SubItems.Add(agentName);
			new_lvwItem.SubItems.Add(agentGroupName);
			new_lvwItem.SubItems.Add(agentStatus);
			new_lvwItem.SubItems.Add(applyTime);
			new_lvwItem.SubItems.Add(approveTime);
			new_lvwItem.SubItems.Add(applyType);
			new_lvwItem.SubItems.Add(reason);
			this.lvwHistory.Items.Add(new_lvwItem);
		}

		public void AgentBar_GetAgentGroupListEvent(Dictionary<string, string> agent_group_list)
		{
			if (!this.HasLoaded_Agent_Group_List)
			{
				this.cboAgentGroup.Items.Clear();
				this.cboAgentGroup.Items.Add("请选择");
				this.cboAgentGroup.SelectedIndex = 0;
				this.upMaxNum.Text = "";
				this.txtMaxNum.Text = "";
				this.dicAgentGroupName = agent_group_list;
				foreach (KeyValuePair<string, string> groupItem in this.dicAgentGroupName)
				{
					this.cboAgentGroup.Items.Add(groupItem.Value);
				}
				this.HasLoaded_Agent_Group_List = true;
			}
		}

		private void cboAgentGroup_SelectedIndexChanged(object sender, EventArgs e)
		{
			try
			{
				string groupIdStr = "";
				if (this.cboAgentGroup.SelectedIndex == 0)
				{
					this.txtMaxNum.Text = "";
				}
				else
				{
					foreach (KeyValuePair<string, string> kvp in this.dicAgentGroupName)
					{
						if (kvp.Value.Equals(this.cboAgentGroup.SelectedItem.ToString()))
						{
							groupIdStr = kvp.Key;
							break;
						}
					}
					int strStatusOfLeave = 7;
					if (!this.agentBar1.DoGetAgentGroupStatusMaxNum("", groupIdStr, strStatusOfLeave.ToString()))
					{
						MessageBox.Show("获取坐席组最大离开人数失败！", "获取失败", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					}
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

		public void AgentBar_GetAgentGroupStatusMaxNumEvent(string AgentID, string agentGroupNameLstStr, Dictionary<string, string> dicAgentGroupStatusMaxNum)
		{
			if (this.cboAgentGroup.SelectedIndex == 0)
			{
				this.txtMaxNum.Text = "";
			}
			else if (dicAgentGroupStatusMaxNum.Count == 1)
			{
				using (Dictionary<string, string>.Enumerator enumerator = dicAgentGroupStatusMaxNum.GetEnumerator())
				{
					if (enumerator.MoveNext())
					{
						KeyValuePair<string, string> item = enumerator.Current;
						this.txtMaxNum.Text = item.Value;
					}
				}
			}
			else
			{
				this.txtMaxNum.Text = "";
			}
		}

		public void AgentBar_ApplyChangeStatusDistributeEvent(string AgentID, string apply_agentid, string apply_agentName, string apply_agent_groupID, string targetStatus, string approveResult, string approve_time, string applyType, int retCode, string reason)
		{
			this.refreshForm();
		}

		public void AgentBar_ApproveChangeStatusDistributeEvent(string AgentID, string apply_agentid, string apply_agent_groupID, string targetStatus, string approveResult, string approve_time, int retCode, string reason)
		{
			this.refreshForm();
		}

		public void AgentBar_ApplyChangeStatusCancelEvent(string AgentID, string apply_agentid, string targetStatus, int retCode, string reason)
		{
			this.refreshForm();
		}

		public void AgentBar_GetChangeStatusApplyListEvent(string AgentID, List<Apply_Change_Status> apply_agent_list, int retCode, string reason)
		{
			this.refreshForm();
		}

		public void AgentBar_SignInEvent(string AgentID, int retCode, string strReason)
		{
			this.refreshForm();
		}

		public void AgentBar_AgentStatusChangeEvent(string agent_num, string old_status, string new_status, bool is_bind_exten, string enter_channel, string current_time, string start_talking_time)
		{
			this.refreshForm();
		}

		public void AgentBar_ApproveChangeStatusTimeoutDistributeEvent(string AgentID, string apply_agentid, string apply_agent_groupID, string targetStatus, string timeoutType)
		{
			this.refreshForm();
		}

		private void btnOk_Click(object sender, EventArgs e)
		{
			string groupIdStr = "";
			if (this.cboAgentGroup.SelectedIndex <= 0)
			{
				MessageBox.Show("请选择一个坐席组！", "设置失败", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			}
			else if (!ComFunc.IsInteger(this.upMaxNum.Text))
			{
				MessageBox.Show("输入的值不合法！！", "设置失败", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			}
			else
			{
				foreach (KeyValuePair<string, string> kvp in this.dicAgentGroupName)
				{
					if (kvp.Value.Equals(this.cboAgentGroup.SelectedItem.ToString()))
					{
						groupIdStr = kvp.Key;
						break;
					}
				}
				int strStatusOfLeave = 7;
				if (!this.agentBar1.DoSetAgentGroupStatusMaxNum("", groupIdStr, strStatusOfLeave.ToString(), this.upMaxNum.Text.ToString()))
				{
					MessageBox.Show("设置坐席组最大离开人数失败！", "设置失败", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				}
			}
		}

		private void tsbNopass_Click(object sender, EventArgs e)
		{
			this.doApprove("0");
		}

		private void FrmApplication_Resize(object sender, EventArgs e)
		{
			if (base.WindowState != FormWindowState.Minimized)
			{
				this.LastWindowsState = base.WindowState;
			}
		}
	}
}
