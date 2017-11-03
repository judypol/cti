using PLAgentBar;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace PLClient
{
	public class frmControlsConfig : Form
	{
		private IContainer components = null;

		private TreeView ControlsCheckTreeView;

		private Button ButtonOk;

		private Button buttonCancel;

		private GroupBox groupBox1;

		private Panel panel1;

		public AgentBar.ControlsInfo controlsInfo;

		public AgentBar.ControlsVisible Controls_Visible;

		public TreeNode SetControls;

		public TreeNode Basic;

		public TreeNode Advanced;

		public TreeNode Others;

		private bool chkValue = true;

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
			TreeNode treeNode18 = new TreeNode("保持");
			TreeNode treeNode19 = new TreeNode("静音");
			TreeNode treeNode20 = new TreeNode("三方");
			TreeNode treeNode21 = new TreeNode("询问");
			TreeNode treeNode22 = new TreeNode("取消询问");
			TreeNode treeNode23 = new TreeNode("询问转");
			TreeNode treeNode24 = new TreeNode("转接");
			TreeNode treeNode25 = new TreeNode("评分");
			TreeNode treeNode26 = new TreeNode("基础设置", new TreeNode[]
			{
				treeNode18,
				treeNode19,
				treeNode20,
				treeNode21,
				treeNode22,
				treeNode23,
				treeNode24,
				treeNode25
			});
			TreeNode treeNode27 = new TreeNode("呼叫坐席");
			TreeNode treeNode28 = new TreeNode("环回测试");
			TreeNode treeNode29 = new TreeNode("高级设置", new TreeNode[]
			{
				treeNode27,
				treeNode28
			});
			TreeNode treeNode30 = new TreeNode("取消申请");
			TreeNode treeNode31 = new TreeNode("挂断后状态设置");
			TreeNode treeNode32 = new TreeNode("未接来电");
			TreeNode treeNode33 = new TreeNode("其他设置", new TreeNode[]
			{
				treeNode30,
				treeNode31,
				treeNode32
			});
			TreeNode treeNode34 = new TreeNode("控件设置", new TreeNode[]
			{
				treeNode26,
				treeNode29,
				treeNode33
			});
			this.ControlsCheckTreeView = new TreeView();
			this.ButtonOk = new Button();
			this.buttonCancel = new Button();
			this.groupBox1 = new GroupBox();
			this.panel1 = new Panel();
			this.groupBox1.SuspendLayout();
			this.panel1.SuspendLayout();
			base.SuspendLayout();
			this.ControlsCheckTreeView.BackColor = SystemColors.ControlLightLight;
			this.ControlsCheckTreeView.BorderStyle = BorderStyle.None;
			this.ControlsCheckTreeView.CheckBoxes = true;
			this.ControlsCheckTreeView.Location = new Point(0, 5);
			this.ControlsCheckTreeView.Name = "ControlsCheckTreeView";
			treeNode18.Name = "chkHold";
			treeNode18.Text = "保持";
			treeNode19.Name = "chkMute";
			treeNode19.Text = "静音";
			treeNode20.Name = "chkThreeWay";
			treeNode20.Text = "三方";
			treeNode21.Name = "chkConsult";
			treeNode21.Text = "询问";
			treeNode22.Name = "chkStopConsult";
			treeNode22.Text = "取消询问";
			treeNode23.Name = "chkConsultTransfer";
			treeNode23.Text = "询问转";
			treeNode24.Name = "chkTransfer";
			treeNode24.Text = "转接";
			treeNode25.Name = "chkGrade";
			treeNode25.Text = "评分";
			treeNode26.Name = "chkControlsBasic";
			treeNode26.Text = "基础设置";
			treeNode27.Name = "chkCallAgent";
			treeNode27.Text = "呼叫坐席";
			treeNode28.Name = "chkCheck";
			treeNode28.Text = "环回测试";
			treeNode29.Name = "chkControlsAdvanced";
			treeNode29.Text = "高级设置";
			treeNode30.Name = "chkCancelApplication";
			treeNode30.Text = "取消申请";
			treeNode31.Name = "chkdbAfterHangup";
			treeNode31.Text = "挂断后状态设置";
			treeNode32.Name = "chkNoAnswerCalls";
			treeNode32.Text = "未接来电";
			treeNode33.Name = "chkControlsOther";
			treeNode33.Text = "其他设置";
			treeNode34.Name = "chkSetControls";
			treeNode34.Text = "控件设置";
			this.ControlsCheckTreeView.Nodes.AddRange(new TreeNode[]
			{
				treeNode34
			});
			this.ControlsCheckTreeView.PathSeparator = "";
			this.ControlsCheckTreeView.Size = new Size(313, 350);
			this.ControlsCheckTreeView.TabIndex = 0;
			this.ControlsCheckTreeView.AfterCheck += new TreeViewEventHandler(this.frmControlsConfig_AfterCheck);
			this.ButtonOk.Location = new Point(144, 389);
			this.ButtonOk.Name = "ButtonOk";
			this.ButtonOk.Size = new Size(75, 23);
			this.ButtonOk.TabIndex = 1;
			this.ButtonOk.Text = "确定";
			this.ButtonOk.UseVisualStyleBackColor = true;
			this.ButtonOk.Click += new EventHandler(this.buttonOk_Click);
			this.buttonCancel.Location = new Point(247, 389);
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.Size = new Size(75, 23);
			this.buttonCancel.TabIndex = 2;
			this.buttonCancel.Text = "取消";
			this.buttonCancel.UseVisualStyleBackColor = true;
			this.buttonCancel.Click += new EventHandler(this.buttonCancel_Click);
			this.groupBox1.Controls.Add(this.panel1);
			this.groupBox1.Location = new Point(12, 0);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new Size(321, 371);
			this.groupBox1.TabIndex = 3;
			this.groupBox1.TabStop = false;
			this.panel1.BackColor = SystemColors.ControlLightLight;
			this.panel1.Controls.Add(this.ControlsCheckTreeView);
			this.panel1.Location = new Point(2, 9);
			this.panel1.Name = "panel1";
			this.panel1.Size = new Size(317, 360);
			this.panel1.TabIndex = 1;
			base.AutoScaleDimensions = new SizeF(6f, 12f);
			base.AutoScaleMode = AutoScaleMode.Font;
			base.ClientSize = new Size(345, 425);
			base.Controls.Add(this.groupBox1);
			base.Controls.Add(this.buttonCancel);
			base.Controls.Add(this.ButtonOk);
			base.FormBorderStyle = FormBorderStyle.FixedDialog;
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "frmControlsConfig";
			base.StartPosition = FormStartPosition.CenterParent;
			this.Text = "控件配置";
			base.Load += new EventHandler(this.frmControlsConfig_Load);
			this.groupBox1.ResumeLayout(false);
			this.panel1.ResumeLayout(false);
			base.ResumeLayout(false);
			base.MaximizeBox = false;
			base.MinimizeBox = false;
		}

		public frmControlsConfig()
		{
			this.InitializeComponent();
			this.SetControls = this.ControlsCheckTreeView.Nodes.Find("chkSetControls", true)[0];
			this.Basic = this.ControlsCheckTreeView.Nodes.Find("chkControlsBasic", true)[0];
			this.Advanced = this.ControlsCheckTreeView.Nodes.Find("chkControlsAdvanced", true)[0];
			this.Others = this.ControlsCheckTreeView.Nodes.Find("chkControlsOther", true)[0];
		}

		private void SetChildChecked(TreeNode parentNode)
		{
			for (int i = 0; i < parentNode.GetNodeCount(false); i++)
			{
				TreeNode node = parentNode.Nodes[i];
				node.Checked = parentNode.Checked;
				if (node.GetNodeCount(false) > 0)
				{
					this.SetChildChecked(node);
				}
			}
		}

		private void SetParentChecked(TreeNode childNode)
		{
			TreeNode parentNode = childNode.Parent;
			if (!parentNode.Checked && childNode.Checked)
			{
				int ichecks = 0;
				for (int i = 0; i < parentNode.GetNodeCount(false); i++)
				{
					TreeNode node = parentNode.Nodes[i];
					if (node.Checked)
					{
						ichecks++;
					}
				}
				if (ichecks == parentNode.GetNodeCount(false))
				{
					parentNode.Checked = true;
					if (parentNode.Parent != null)
					{
						this.SetParentChecked(parentNode);
					}
				}
			}
			else if (parentNode.Checked && !childNode.Checked)
			{
				if (parentNode != null)
				{
					this.setParentNodeCheckedState(childNode, false);
				}
			}
		}

		private void setParentNodeCheckedState(TreeNode childNode, bool state)
		{
			TreeNode parentNode = childNode.Parent;
			this.chkValue = false;
			parentNode.Checked = state;
			if (parentNode.Parent != null)
			{
				this.setParentNodeCheckedState(childNode.Parent, state);
			}
			this.chkValue = true;
		}

		private void frmControlsConfig_AfterCheck(object sender, TreeViewEventArgs e)
		{
			if (this.chkValue)
			{
				this.SetChildChecked(e.Node);
				if (e.Node.Parent != null)
				{
					this.SetParentChecked(e.Node);
				}
				this.ControlsCheckTreeView.SelectedNode = e.Node;
			}
		}

		public void addNodes()
		{
			TreeNode CallOut = new TreeNode();
			CallOut.Name = "chkCallOut";
			CallOut.Text = "拨外线";
			TreeNode Listen = new TreeNode();
			Listen.Name = "chkListen";
			Listen.Text = "监听";
			TreeNode Whisper = new TreeNode();
			Whisper.Name = "chkWhisper";
			Whisper.Text = "密语";
			TreeNode Bargein = new TreeNode();
			Bargein.Name = "chkBargein";
			Bargein.Text = "强插";
			TreeNode ForceHangup = new TreeNode();
			ForceHangup.Name = "chkForceHangup";
			ForceHangup.Text = "强拆";
			TreeNode Monitor = new TreeNode();
			Monitor.Name = "chkMonitor";
			Monitor.Text = "监控";
			TreeNode Approve = new TreeNode();
			Approve.Name = "chkApprove";
			Approve.Text = "离开审批";
			if (this.ContainsNodes("chkCallOut"))
			{
				if (!this.Controls_Visible.CallOutVisible)
				{
					this.Advanced.Nodes.RemoveByKey("chkCallOut");
				}
			}
			else if (this.Controls_Visible.CallOutVisible)
			{
				this.Advanced.Nodes.Add(CallOut);
			}
			if (this.ContainsNodes("chkListen"))
			{
				if (!this.Controls_Visible.ListenVisible)
				{
					this.Advanced.Nodes.RemoveByKey("chkListen");
				}
			}
			else if (this.Controls_Visible.ListenVisible)
			{
				this.Advanced.Nodes.Add(Listen);
			}
			if (this.ContainsNodes("chkWhisper"))
			{
				if (!this.Controls_Visible.WhisperVisible)
				{
					this.Advanced.Nodes.RemoveByKey("chkWhisper");
				}
			}
			else if (this.Controls_Visible.WhisperVisible)
			{
				this.Advanced.Nodes.Add(Whisper);
			}
			if (this.ContainsNodes("chkBargein"))
			{
				if (!this.Controls_Visible.BargeinVisible)
				{
					this.Advanced.Nodes.RemoveByKey("chkBargein");
				}
			}
			else if (this.Controls_Visible.BargeinVisible)
			{
				this.Advanced.Nodes.Add(Bargein);
			}
			if (this.ContainsNodes("chkForceHangup"))
			{
				if (!this.Controls_Visible.ForceHangupVisible)
				{
					this.Advanced.Nodes.RemoveByKey("chkForceHangup");
				}
			}
			else if (this.Controls_Visible.ForceHangupVisible)
			{
				this.Advanced.Nodes.Add(ForceHangup);
			}
			if (this.ContainsNodes("chkMonitor"))
			{
				if (!this.Controls_Visible.MonitorVisible)
				{
					this.Others.Nodes.RemoveByKey("chkMonitor");
				}
			}
			else if (this.Controls_Visible.MonitorVisible)
			{
				this.Others.Nodes.Add(Monitor);
			}
			if (this.ContainsNodes("chkApprove"))
			{
				if (!this.Controls_Visible.ApproveVisible)
				{
					this.Others.Nodes.RemoveByKey("chkApprove");
				}
			}
			else if (this.Controls_Visible.ApproveVisible)
			{
				this.Others.Nodes.Add(Approve);
			}
		}

		public bool ContainsNodes(string ControlsName)
		{
			bool result;
			foreach (TreeNode node in this.ControlsCheckTreeView.Nodes)
			{
				foreach (TreeNode node2 in node.Nodes)
				{
					foreach (TreeNode node3 in node2.Nodes)
					{
						if (node3.Name == ControlsName)
						{
							result = true;
							return result;
						}
					}
				}
			}
			result = false;
			return result;
		}

		public void ControlsCheak()
		{
			this.controlsInfo.chkHoldInfo = (Helper.isHold == "1");
			this.controlsInfo.chkMuteInfo = (Helper.isMute == "1");
			this.controlsInfo.chkThreeWayInfo = (Helper.isThreeWay == "1");
			this.controlsInfo.chkConsultInfo = (Helper.isConsult == "1");
			this.controlsInfo.chkStopConsultInfo = (Helper.isStopConsult == "1");
			this.controlsInfo.chkConsultTransferInfo = (Helper.isConsultTransfer == "1");
			this.controlsInfo.chkTransferInfo = (Helper.isTransfer == "1");
			this.controlsInfo.chkGradeInfo = (Helper.isGrade == "1");
			this.controlsInfo.chkCallAgentInfo = (Helper.isCallAgent == "1");
			this.controlsInfo.chkCallOutInfo = (Helper.isCallOut == "1");
			this.controlsInfo.chkListenInfo = (Helper.isListen == "1");
			this.controlsInfo.chkWhisperInfo = (Helper.isWhisper == "1");
			this.controlsInfo.chkBargeinInfo = (Helper.isBargein == "1");
			this.controlsInfo.chkForceHangupInfo = (Helper.isForceHangup == "1");
			this.controlsInfo.chkCheckInfo = (Helper.isCheck == "1");
			this.controlsInfo.chkMonitorInfo = (Helper.isMonitor == "1");
			this.controlsInfo.chkCancelApplicationInfo = (Helper.isCancelApplication == "1");
			this.controlsInfo.chkdbAfterHangupInfo = (Helper.isdbAfterHangup == "1");
			this.controlsInfo.chkApproveInfo = (Helper.isApprove == "1");
			this.controlsInfo.chkNoAnswerCallsInfo = (Helper.isNoAnswerCalls == "1");
		}

		private void buttonOk_Click(object sender, EventArgs e)
		{
			this.readCheck();
			this.ControlsCheak();
			Helper.write_ControlsConfig_to_file();
			base.DialogResult = DialogResult.OK;
		}

		private void readCheck()
		{
			Helper.isHold = (this.ControlsCheckTreeView.Nodes.Find("chkHold", true)[0].Checked ? "1" : "0");
			Helper.isMute = (this.ControlsCheckTreeView.Nodes.Find("chkMute", true)[0].Checked ? "1" : "0");
			Helper.isThreeWay = (this.ControlsCheckTreeView.Nodes.Find("chkThreeWay", true)[0].Checked ? "1" : "0");
			Helper.isConsult = (this.ControlsCheckTreeView.Nodes.Find("chkConsult", true)[0].Checked ? "1" : "0");
			Helper.isStopConsult = (this.ControlsCheckTreeView.Nodes.Find("chkStopConsult", true)[0].Checked ? "1" : "0");
			Helper.isConsultTransfer = (this.ControlsCheckTreeView.Nodes.Find("chkConsultTransfer", true)[0].Checked ? "1" : "0");
			Helper.isTransfer = (this.ControlsCheckTreeView.Nodes.Find("chkTransfer", true)[0].Checked ? "1" : "0");
			Helper.isGrade = (this.ControlsCheckTreeView.Nodes.Find("chkGrade", true)[0].Checked ? "1" : "0");
			Helper.isCallAgent = (this.ControlsCheckTreeView.Nodes.Find("chkCallAgent", true)[0].Checked ? "1" : "0");
			if (this.Advanced.Nodes.ContainsKey("chkCallOut"))
			{
				Helper.isCallOut = (this.ControlsCheckTreeView.Nodes.Find("chkCallOut", true)[0].Checked ? "1" : "0");
			}
			if (this.Advanced.Nodes.ContainsKey("chkListen"))
			{
				Helper.isListen = (this.ControlsCheckTreeView.Nodes.Find("chkListen", true)[0].Checked ? "1" : "0");
			}
			if (this.Advanced.Nodes.ContainsKey("chkWhisper"))
			{
				Helper.isWhisper = (this.ControlsCheckTreeView.Nodes.Find("chkWhisper", true)[0].Checked ? "1" : "0");
			}
			if (this.Advanced.Nodes.ContainsKey("chkBargein"))
			{
				Helper.isBargein = (this.ControlsCheckTreeView.Nodes.Find("chkBargein", true)[0].Checked ? "1" : "0");
			}
			if (this.Advanced.Nodes.ContainsKey("chkForceHangup"))
			{
				Helper.isForceHangup = (this.ControlsCheckTreeView.Nodes.Find("chkForceHangup", true)[0].Checked ? "1" : "0");
			}
			if (this.Others.Nodes.ContainsKey("chkMonitor"))
			{
				Helper.isMonitor = (this.ControlsCheckTreeView.Nodes.Find("chkMonitor", true)[0].Checked ? "1" : "0");
			}
			if (this.Others.Nodes.ContainsKey("chkApprove"))
			{
				Helper.isApprove = (this.ControlsCheckTreeView.Nodes.Find("chkApprove", true)[0].Checked ? "1" : "0");
			}
			Helper.isCheck = (this.ControlsCheckTreeView.Nodes.Find("chkCheck", true)[0].Checked ? "1" : "0");
			Helper.isCancelApplication = (this.ControlsCheckTreeView.Nodes.Find("chkCancelApplication", true)[0].Checked ? "1" : "0");
			Helper.isdbAfterHangup = (this.ControlsCheckTreeView.Nodes.Find("chkdbAfterHangup", true)[0].Checked ? "1" : "0");
			Helper.isNoAnswerCalls = (this.ControlsCheckTreeView.Nodes.Find("chkNoAnswerCalls", true)[0].Checked ? "1" : "0");
			Helper.isControls = (this.ControlsCheckTreeView.Nodes.Find("chkSetControls", true)[0].Checked ? "1" : "0");
			Helper.isBasic = (this.ControlsCheckTreeView.Nodes.Find("chkControlsBasic", true)[0].Checked ? "1" : "0");
			Helper.isAdvanced = (this.ControlsCheckTreeView.Nodes.Find("chkControlsAdvanced", true)[0].Checked ? "1" : "0");
			Helper.isOthers = (this.ControlsCheckTreeView.Nodes.Find("chkControlsOther", true)[0].Checked ? "1" : "0");
		}

		public void recheck()
		{
			this.ControlsCheckTreeView.Nodes.Find("chkHold", true)[0].Checked = (Helper.isHold == "1");
			this.ControlsCheckTreeView.Nodes.Find("chkMute", true)[0].Checked = (Helper.isMute == "1");
			this.ControlsCheckTreeView.Nodes.Find("chkThreeWay", true)[0].Checked = (Helper.isThreeWay == "1");
			this.ControlsCheckTreeView.Nodes.Find("chkConsult", true)[0].Checked = (Helper.isConsult == "1");
			this.ControlsCheckTreeView.Nodes.Find("chkStopConsult", true)[0].Checked = (Helper.isStopConsult == "1");
			this.ControlsCheckTreeView.Nodes.Find("chkConsultTransfer", true)[0].Checked = (Helper.isConsultTransfer == "1");
			this.ControlsCheckTreeView.Nodes.Find("chkTransfer", true)[0].Checked = (Helper.isTransfer == "1");
			this.ControlsCheckTreeView.Nodes.Find("chkGrade", true)[0].Checked = (Helper.isGrade == "1");
			this.ControlsCheckTreeView.Nodes.Find("chkCallAgent", true)[0].Checked = (Helper.isCallAgent == "1");
			if (this.Advanced.Nodes.ContainsKey("chkCallOut"))
			{
				this.ControlsCheckTreeView.Nodes.Find("chkCallOut", true)[0].Checked = (Helper.isCallOut == "1");
			}
			if (this.Advanced.Nodes.ContainsKey("chkListen"))
			{
				this.ControlsCheckTreeView.Nodes.Find("chkListen", true)[0].Checked = (Helper.isListen == "1");
			}
			if (this.Advanced.Nodes.ContainsKey("chkWhisper"))
			{
				this.ControlsCheckTreeView.Nodes.Find("chkWhisper", true)[0].Checked = (Helper.isWhisper == "1");
			}
			if (this.Advanced.Nodes.ContainsKey("chkBargein"))
			{
				this.ControlsCheckTreeView.Nodes.Find("chkBargein", true)[0].Checked = (Helper.isBargein == "1");
			}
			if (this.Advanced.Nodes.ContainsKey("chkForceHangup"))
			{
				this.ControlsCheckTreeView.Nodes.Find("chkForceHangup", true)[0].Checked = (Helper.isForceHangup == "1");
			}
			if (this.Others.Nodes.ContainsKey("chkMonitor"))
			{
				this.ControlsCheckTreeView.Nodes.Find("chkMonitor", true)[0].Checked = (Helper.isMonitor == "1");
			}
			if (this.Others.Nodes.ContainsKey("chkApprove"))
			{
				this.ControlsCheckTreeView.Nodes.Find("chkApprove", true)[0].Checked = (Helper.isApprove == "1");
			}
			this.ControlsCheckTreeView.Nodes.Find("chkCheck", true)[0].Checked = (Helper.isCheck == "1");
			this.ControlsCheckTreeView.Nodes.Find("chkCancelApplication", true)[0].Checked = (Helper.isCancelApplication == "1");
			this.ControlsCheckTreeView.Nodes.Find("chkdbAfterHangup", true)[0].Checked = (Helper.isdbAfterHangup == "1");
			this.ControlsCheckTreeView.Nodes.Find("chkNoAnswerCalls", true)[0].Checked = (Helper.isNoAnswerCalls == "1");
			this.ControlsCheckTreeView.Nodes.Find("chkSetControls", true)[0].Checked = (Helper.isControls == "1");
			this.ControlsCheckTreeView.Nodes.Find("chkControlsBasic", true)[0].Checked = (Helper.isBasic == "1");
			this.ControlsCheckTreeView.Nodes.Find("chkControlsAdvanced", true)[0].Checked = (Helper.isAdvanced == "1");
			this.ControlsCheckTreeView.Nodes.Find("chkControlsOther", true)[0].Checked = (Helper.isOthers == "1");
		}

		public void CheckParentNode(TreeNode ParentNode)
		{
			int ichecks = 0;
			for (int i = 0; i < ParentNode.GetNodeCount(false); i++)
			{
				TreeNode node = ParentNode.Nodes[i];
				if (node.Checked)
				{
					ichecks++;
				}
			}
			if (ichecks == ParentNode.GetNodeCount(false))
			{
				ParentNode.Checked = true;
			}
			else
			{
				ParentNode.Checked = false;
			}
		}

		private void frmControlsConfig_Load(object sender, EventArgs e)
		{
			this.chkValue = false;
			Helper.Controlsload_sys_config();
			this.ControlsCheckTreeView.ExpandAll();
			this.recheck();
			this.CheckParentNode(this.Advanced);
			this.CheckParentNode(this.Others);
			if (this.Basic.Checked && this.Advanced.Checked && this.Others.Checked)
			{
				this.SetControls.Checked = true;
			}
			else
			{
				this.SetControls.Checked = false;
			}
			this.chkValue = true;
		}

		private void buttonCancel_Click(object sender, EventArgs e)
		{
			base.Close();
		}
	}
}
