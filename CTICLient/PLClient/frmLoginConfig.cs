using PLAgentBar;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace PLClient
{
	public class frmLoginConfig : Form
	{
		private IContainer components = null;

		private TabControl tabSetting;

		private TabPage tabPage1;

		private GroupBox groupBox6;

		private Label label14;

		private Label label16;

		private TextBox txtWebCallOutUrl;

		private GroupBox groupBox1;

		private Label lblinfo2;

		private Label lblInfo;

		private TextBox txtWebUrl;

		private Label label15;

		private Label label13;

		private NumericUpDown numACWtime;

		private CheckBox chkSetIdle;

		private Button btnCancel;

		private Button btnOk;

		private CheckBox chkMoniotrOfflineAgent;

		private GroupBox groupBox2;

		private Label label1;

		private Label label2;

		private TextBox txtHomePage;

		private TabPage tabPage2;

		private TreeView treeView1;

		private GroupBox groupBox3;

		private Label label4;

		private Label label5;

		private TextBox txt_noAnswerCallsUrl;

		private TabPage tabPage4;

		private GroupBox groupBox8;

		private CheckBox chkQueueMonitor;

		private CheckBox chkWhenForceHangup;

		private CheckBox chkWhenBargin;

		private CheckBox chkWhenWhisper;

		private CheckBox chkWhenEavesDrop;

		private CheckBox chkWhenThreeWay;

		private CheckBox chkWhenConsult;

		private CheckBox chkSoftphoneAutoAnswer;

		private Label label3;

		private ComboBox cbxPhoneScreen;

		private bool mIsMonitorOfflineAgent;

		private string mNoAnswerCallsURL;

		public string ScreenPhoneFormat = string.Empty;

		public AgentBar.ControlsInfo controlsinfo;

		public bool IsMonitorOfflineAgent
		{
			get
			{
				return this.mIsMonitorOfflineAgent;
			}
			set
			{
				this.mIsMonitorOfflineAgent = value;
			}
		}

		public string NoAnswerCallsURL
		{
			get
			{
				return this.mNoAnswerCallsURL;
			}
			set
			{
				this.mNoAnswerCallsURL = value;
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
			TreeNode treeNode19 = new TreeNode("转接ivr");
			TreeNode treeNode20 = new TreeNode("转接ivr profile");
			TreeNode treeNode21 = new TreeNode("转接", new TreeNode[]
			{
				treeNode19,
				treeNode20
			});
			this.tabSetting = new TabControl();
			this.tabPage1 = new TabPage();
			this.chkSoftphoneAutoAnswer = new CheckBox();
			this.groupBox3 = new GroupBox();
			this.label4 = new Label();
			this.label5 = new Label();
			this.txt_noAnswerCallsUrl = new TextBox();
			this.groupBox2 = new GroupBox();
			this.label1 = new Label();
			this.label2 = new Label();
			this.txtHomePage = new TextBox();
			this.chkMoniotrOfflineAgent = new CheckBox();
			this.label15 = new Label();
			this.label13 = new Label();
			this.numACWtime = new NumericUpDown();
			this.chkSetIdle = new CheckBox();
			this.groupBox6 = new GroupBox();
			this.label14 = new Label();
			this.label16 = new Label();
			this.txtWebCallOutUrl = new TextBox();
			this.groupBox1 = new GroupBox();
			this.lblinfo2 = new Label();
			this.lblInfo = new Label();
			this.txtWebUrl = new TextBox();
			this.tabPage2 = new TabPage();
			this.treeView1 = new TreeView();
			this.tabPage4 = new TabPage();
			this.groupBox8 = new GroupBox();
			this.chkQueueMonitor = new CheckBox();
			this.chkWhenForceHangup = new CheckBox();
			this.chkWhenBargin = new CheckBox();
			this.chkWhenWhisper = new CheckBox();
			this.chkWhenEavesDrop = new CheckBox();
			this.chkWhenThreeWay = new CheckBox();
			this.chkWhenConsult = new CheckBox();
			this.btnCancel = new Button();
			this.btnOk = new Button();
			this.cbxPhoneScreen = new ComboBox();
			this.label3 = new Label();
			this.tabSetting.SuspendLayout();
			this.tabPage1.SuspendLayout();
			this.groupBox3.SuspendLayout();
			this.groupBox2.SuspendLayout();
			((ISupportInitialize)this.numACWtime).BeginInit();
			this.groupBox6.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.tabPage2.SuspendLayout();
			this.tabPage4.SuspendLayout();
			this.groupBox8.SuspendLayout();
			base.SuspendLayout();
			this.tabSetting.Controls.Add(this.tabPage1);
			this.tabSetting.Controls.Add(this.tabPage2);
			this.tabSetting.Controls.Add(this.tabPage4);
			this.tabSetting.Location = new Point(2, 1);
			this.tabSetting.Name = "tabSetting";
			this.tabSetting.SelectedIndex = 0;
			this.tabSetting.Size = new Size(345, 383);
			this.tabSetting.TabIndex = 2;
			this.tabPage1.Controls.Add(this.label3);
			this.tabPage1.Controls.Add(this.cbxPhoneScreen);
			this.tabPage1.Controls.Add(this.chkSoftphoneAutoAnswer);
			this.tabPage1.Controls.Add(this.groupBox3);
			this.tabPage1.Controls.Add(this.groupBox2);
			this.tabPage1.Controls.Add(this.chkMoniotrOfflineAgent);
			this.tabPage1.Controls.Add(this.label15);
			this.tabPage1.Controls.Add(this.label13);
			this.tabPage1.Controls.Add(this.numACWtime);
			this.tabPage1.Controls.Add(this.chkSetIdle);
			this.tabPage1.Controls.Add(this.groupBox6);
			this.tabPage1.Controls.Add(this.groupBox1);
			this.tabPage1.Location = new Point(4, 22);
			this.tabPage1.Name = "tabPage1";
			this.tabPage1.Padding = new Padding(3);
			this.tabPage1.Size = new Size(337, 357);
			this.tabPage1.TabIndex = 0;
			this.tabPage1.Text = "基础配置";
			this.tabPage1.UseVisualStyleBackColor = true;
			this.chkSoftphoneAutoAnswer.AutoSize = true;
			this.chkSoftphoneAutoAnswer.Location = new Point(132, 41);
			this.chkSoftphoneAutoAnswer.Name = "chkSoftphoneAutoAnswer";
			this.chkSoftphoneAutoAnswer.Size = new Size(108, 16);
			this.chkSoftphoneAutoAnswer.TabIndex = 50;
			this.chkSoftphoneAutoAnswer.Text = "软电话自动接听";
			this.chkSoftphoneAutoAnswer.UseVisualStyleBackColor = true;
			this.groupBox3.Controls.Add(this.label4);
			this.groupBox3.Controls.Add(this.label5);
			this.groupBox3.Controls.Add(this.txt_noAnswerCallsUrl);
			this.groupBox3.Location = new Point(6, 301);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Size = new Size(313, 51);
			this.groupBox3.TabIndex = 49;
			this.groupBox3.TabStop = false;
			this.groupBox3.Text = "未接来电接口地址";
			this.label4.Location = new Point(7, 116);
			this.label4.Name = "label4";
			this.label4.Size = new Size(298, 68);
			this.label4.TabIndex = 45;
			this.label4.Text = "弹屏网址+?+customernum={customernum} & callcenternum={callcenternum} & taskid={taskid} & queuenum={queuenum} & agentnum={agentnum} & targetAgentNum={targetAgentNum} & calltype={calltype}";
			this.label4.Visible = false;
			this.label5.Font = new Font("微软雅黑", 9f, FontStyle.Regular, GraphicsUnit.Point, 134);
			this.label5.ForeColor = Color.Red;
			this.label5.Location = new Point(6, 47);
			this.label5.Name = "label5";
			this.label5.Size = new Size(297, 76);
			this.label5.TabIndex = 44;
			this.label5.Visible = false;
			this.txt_noAnswerCallsUrl.Location = new Point(6, 19);
			this.txt_noAnswerCallsUrl.Name = "txt_noAnswerCallsUrl";
			this.txt_noAnswerCallsUrl.Size = new Size(297, 21);
			this.txt_noAnswerCallsUrl.TabIndex = 6;
			this.txt_noAnswerCallsUrl.Text = "http://";
			this.groupBox2.Controls.Add(this.label1);
			this.groupBox2.Controls.Add(this.label2);
			this.groupBox2.Controls.Add(this.txtHomePage);
			this.groupBox2.Location = new Point(6, 247);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new Size(313, 48);
			this.groupBox2.TabIndex = 48;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "首页地址";
			this.label1.Location = new Point(7, 116);
			this.label1.Name = "label1";
			this.label1.Size = new Size(298, 68);
			this.label1.TabIndex = 45;
			this.label1.Text = "弹屏网址+?+customernum={customernum} & callcenternum={callcenternum} & taskid={taskid} & queuenum={queuenum} & agentnum={agentnum} & targetAgentNum={targetAgentNum} & calltype={calltype}";
			this.label1.Visible = false;
			this.label2.Font = new Font("微软雅黑", 9f, FontStyle.Regular, GraphicsUnit.Point, 134);
			this.label2.ForeColor = Color.Red;
			this.label2.Location = new Point(6, 47);
			this.label2.Name = "label2";
			this.label2.Size = new Size(297, 76);
			this.label2.TabIndex = 44;
			this.label2.Text = "说明：弹屏网址请填写CRM系统提供的链接地址，如果要在网址中附带参数请添加\"?\"，然后再添加需要的参数，多个参数之间使用&符号分隔开，格式如下：";
			this.label2.Visible = false;
			this.txtHomePage.Location = new Point(6, 19);
			this.txtHomePage.Name = "txtHomePage";
			this.txtHomePage.Size = new Size(297, 21);
			this.txtHomePage.TabIndex = 6;
			this.txtHomePage.Text = "http://";
			this.chkMoniotrOfflineAgent.AutoSize = true;
			this.chkMoniotrOfflineAgent.Location = new Point(11, 41);
			this.chkMoniotrOfflineAgent.Name = "chkMoniotrOfflineAgent";
			this.chkMoniotrOfflineAgent.Size = new Size(108, 16);
			this.chkMoniotrOfflineAgent.TabIndex = 3;
			this.chkMoniotrOfflineAgent.Text = "监控不在线坐席";
			this.chkMoniotrOfflineAgent.UseVisualStyleBackColor = true;
			this.label15.AutoSize = true;
			this.label15.Location = new Point(184, 14);
			this.label15.Name = "label15";
			this.label15.Size = new Size(77, 12);
			this.label15.TabIndex = 46;
			this.label15.Text = "秒后自动置闲";
			this.label13.AutoSize = true;
			this.label13.Location = new Point(28, 17);
			this.label13.Name = "label13";
			this.label13.Size = new Size(0, 12);
			this.label13.TabIndex = 1;
			this.numACWtime.Location = new Point(132, 8);
			this.numACWtime.Name = "numACWtime";
			this.numACWtime.Size = new Size(46, 21);
			this.numACWtime.TabIndex = 2;
			this.chkSetIdle.AutoSize = true;
			this.chkSetIdle.Location = new Point(12, 13);
			this.chkSetIdle.Name = "chkSetIdle";
			this.chkSetIdle.Size = new Size(120, 16);
			this.chkSetIdle.TabIndex = 1;
			this.chkSetIdle.Text = "当后处理状态超过";
			this.chkSetIdle.UseVisualStyleBackColor = true;
			this.groupBox6.Controls.Add(this.label14);
			this.groupBox6.Controls.Add(this.label16);
			this.groupBox6.Controls.Add(this.txtWebCallOutUrl);
			this.groupBox6.Location = new Point(6, 176);
			this.groupBox6.Name = "groupBox6";
			this.groupBox6.Size = new Size(313, 63);
			this.groupBox6.TabIndex = 42;
			this.groupBox6.TabStop = false;
			this.groupBox6.Text = "呼出弹屏地址";
			this.label14.Location = new Point(7, 116);
			this.label14.Name = "label14";
			this.label14.Size = new Size(298, 68);
			this.label14.TabIndex = 45;
			this.label14.Text = "弹屏网址+?+customernum={customernum} & callcenternum={callcenternum} & taskid={taskid} & queuenum={queuenum} & agentnum={agentnum} & targetAgentNum={targetAgentNum} & calltype={calltype}";
			this.label14.Visible = false;
			this.label16.Font = new Font("微软雅黑", 9f, FontStyle.Regular, GraphicsUnit.Point, 134);
			this.label16.ForeColor = Color.Red;
			this.label16.Location = new Point(6, 44);
			this.label16.Name = "label16";
			this.label16.Size = new Size(297, 64);
			this.label16.TabIndex = 44;
			this.label16.Text = "说明：弹屏网址请填写CRM系统提供的链接地址，如果要在网址中附带参数请添加\"?\"，然后再添加需要的参数，多个参数之间使用&符号分隔开，格式如下：";
			this.label16.Visible = false;
			this.txtWebCallOutUrl.Location = new Point(6, 16);
			this.txtWebCallOutUrl.Name = "txtWebCallOutUrl";
			this.txtWebCallOutUrl.Size = new Size(297, 21);
			this.txtWebCallOutUrl.TabIndex = 5;
			this.txtWebCallOutUrl.Text = "http://";
			this.groupBox1.Controls.Add(this.lblinfo2);
			this.groupBox1.Controls.Add(this.lblInfo);
			this.groupBox1.Controls.Add(this.txtWebUrl);
			this.groupBox1.Location = new Point(6, 104);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new Size(313, 57);
			this.groupBox1.TabIndex = 41;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "来电弹屏地址";
			this.lblinfo2.Location = new Point(7, 116);
			this.lblinfo2.Name = "lblinfo2";
			this.lblinfo2.Size = new Size(298, 68);
			this.lblinfo2.TabIndex = 45;
			this.lblinfo2.Text = "弹屏网址+?+customernum={customernum} & callcenternum={callcenternum} & taskid={taskid} & queuenum={queuenum} & agentnum={agentnum} & targetAgentNum={targetAgentNum} & calltype={calltype}";
			this.lblinfo2.Visible = false;
			this.lblInfo.Font = new Font("微软雅黑", 9f, FontStyle.Regular, GraphicsUnit.Point, 134);
			this.lblInfo.ForeColor = Color.Red;
			this.lblInfo.Location = new Point(6, 40);
			this.lblInfo.Name = "lblInfo";
			this.lblInfo.Size = new Size(297, 23);
			this.lblInfo.TabIndex = 44;
			this.lblInfo.Text = "说明：弹屏网址请填写CRM系统提供的链接地址，如果要在网址中附带参数请添加\"?\"，然后再添加需要的参数，多个参数之间使用&符号分隔开，格式如下：";
			this.lblInfo.Visible = false;
			this.txtWebUrl.Location = new Point(6, 19);
			this.txtWebUrl.Name = "txtWebUrl";
			this.txtWebUrl.Size = new Size(297, 21);
			this.txtWebUrl.TabIndex = 4;
			this.txtWebUrl.Text = "http://";
			this.tabPage2.Controls.Add(this.treeView1);
			this.tabPage2.Location = new Point(4, 22);
			this.tabPage2.Name = "tabPage2";
			this.tabPage2.Size = new Size(337, 357);
			this.tabPage2.TabIndex = 1;
			this.tabPage2.Text = "功能键配置";
			this.tabPage2.UseVisualStyleBackColor = true;
			this.treeView1.CheckBoxes = true;
			this.treeView1.Location = new Point(5, 3);
			this.treeView1.Name = "treeView1";
			treeNode19.Name = "节点4";
			treeNode19.Text = "转接ivr";
			treeNode20.Name = "节点5";
			treeNode20.Text = "转接ivr profile";
			treeNode21.Checked = true;
			treeNode21.Name = "节点3";
			treeNode21.Text = "转接";
			this.treeView1.Nodes.AddRange(new TreeNode[]
			{
				treeNode21
			});
			this.treeView1.Size = new Size(329, 346);
			this.treeView1.TabIndex = 0;
			this.tabPage4.Controls.Add(this.groupBox8);
			this.tabPage4.Location = new Point(4, 22);
			this.tabPage4.Name = "tabPage4";
			this.tabPage4.Size = new Size(337, 357);
			this.tabPage4.TabIndex = 3;
			this.tabPage4.Text = "提示功能配置";
			this.tabPage4.UseVisualStyleBackColor = true;
			this.groupBox8.Controls.Add(this.chkQueueMonitor);
			this.groupBox8.Controls.Add(this.chkWhenForceHangup);
			this.groupBox8.Controls.Add(this.chkWhenBargin);
			this.groupBox8.Controls.Add(this.chkWhenWhisper);
			this.groupBox8.Controls.Add(this.chkWhenEavesDrop);
			this.groupBox8.Controls.Add(this.chkWhenThreeWay);
			this.groupBox8.Controls.Add(this.chkWhenConsult);
			this.groupBox8.Location = new Point(3, 8);
			this.groupBox8.Name = "groupBox8";
			this.groupBox8.Size = new Size(331, 132);
			this.groupBox8.TabIndex = 0;
			this.groupBox8.TabStop = false;
			this.groupBox8.Text = "选择是否提醒";
			this.chkQueueMonitor.AutoSize = true;
			this.chkQueueMonitor.Location = new Point(21, 94);
			this.chkQueueMonitor.Name = "chkQueueMonitor";
			this.chkQueueMonitor.Size = new Size(72, 16);
			this.chkQueueMonitor.TabIndex = 6;
			this.chkQueueMonitor.Text = "客户入队";
			this.chkQueueMonitor.UseVisualStyleBackColor = true;
			this.chkWhenForceHangup.AutoSize = true;
			this.chkWhenForceHangup.Location = new Point(221, 61);
			this.chkWhenForceHangup.Name = "chkWhenForceHangup";
			this.chkWhenForceHangup.Size = new Size(72, 16);
			this.chkWhenForceHangup.TabIndex = 5;
			this.chkWhenForceHangup.Text = "发起强拆";
			this.chkWhenForceHangup.UseVisualStyleBackColor = true;
			this.chkWhenBargin.AutoSize = true;
			this.chkWhenBargin.Location = new Point(120, 61);
			this.chkWhenBargin.Name = "chkWhenBargin";
			this.chkWhenBargin.Size = new Size(72, 16);
			this.chkWhenBargin.TabIndex = 4;
			this.chkWhenBargin.Text = "发起强插";
			this.chkWhenBargin.UseVisualStyleBackColor = true;
			this.chkWhenWhisper.AutoSize = true;
			this.chkWhenWhisper.Location = new Point(221, 29);
			this.chkWhenWhisper.Name = "chkWhenWhisper";
			this.chkWhenWhisper.Size = new Size(72, 16);
			this.chkWhenWhisper.TabIndex = 3;
			this.chkWhenWhisper.Text = "发起密语";
			this.chkWhenWhisper.UseVisualStyleBackColor = true;
			this.chkWhenEavesDrop.AutoSize = true;
			this.chkWhenEavesDrop.Location = new Point(21, 61);
			this.chkWhenEavesDrop.Name = "chkWhenEavesDrop";
			this.chkWhenEavesDrop.Size = new Size(60, 16);
			this.chkWhenEavesDrop.TabIndex = 2;
			this.chkWhenEavesDrop.Text = "监听时";
			this.chkWhenEavesDrop.UseVisualStyleBackColor = true;
			this.chkWhenThreeWay.AutoSize = true;
			this.chkWhenThreeWay.Location = new Point(120, 29);
			this.chkWhenThreeWay.Name = "chkWhenThreeWay";
			this.chkWhenThreeWay.Size = new Size(72, 16);
			this.chkWhenThreeWay.TabIndex = 1;
			this.chkWhenThreeWay.Text = "三方通话";
			this.chkWhenThreeWay.UseVisualStyleBackColor = true;
			this.chkWhenConsult.AutoSize = true;
			this.chkWhenConsult.Location = new Point(21, 29);
			this.chkWhenConsult.Name = "chkWhenConsult";
			this.chkWhenConsult.Size = new Size(72, 16);
			this.chkWhenConsult.TabIndex = 0;
			this.chkWhenConsult.Text = "咨询来电";
			this.chkWhenConsult.UseVisualStyleBackColor = true;
			this.btnCancel.DialogResult = DialogResult.Cancel;
			this.btnCancel.Location = new Point(253, 388);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new Size(80, 27);
			this.btnCancel.TabIndex = 8;
			this.btnCancel.Text = "取消";
			this.btnCancel.UseVisualStyleBackColor = true;
			this.btnOk.Location = new Point(156, 388);
			this.btnOk.Name = "btnOk";
			this.btnOk.Size = new Size(80, 27);
			this.btnOk.TabIndex = 7;
			this.btnOk.Text = "确定";
			this.btnOk.UseVisualStyleBackColor = true;
			this.btnOk.Click += new EventHandler(this.btnOk_Click);
			this.cbxPhoneScreen.DropDownStyle = ComboBoxStyle.DropDownList;
			this.cbxPhoneScreen.FormattingEnabled = true;
			this.cbxPhoneScreen.Location = new Point(98, 71);
			this.cbxPhoneScreen.Name = "cbxPhoneScreen";
			this.cbxPhoneScreen.Size = new Size(211, 20);
			this.cbxPhoneScreen.TabIndex = 0;
			this.label3.AutoSize = true;
			this.label3.Location = new Point(9, 74);
			this.label3.Name = "label3";
			this.label3.Size = new Size(89, 12);
			this.label3.TabIndex = 51;
			this.label3.Text = "弹屏电话格式：";
			base.AcceptButton = this.btnOk;
			base.AutoScaleDimensions = new SizeF(6f, 12f);
			base.AutoScaleMode = AutoScaleMode.Font;
			base.CancelButton = this.btnCancel;
			base.ClientSize = new Size(345, 425);
			base.Controls.Add(this.btnCancel);
			base.Controls.Add(this.btnOk);
			base.Controls.Add(this.tabSetting);
			base.FormBorderStyle = FormBorderStyle.FixedDialog;
			base.ImeMode = ImeMode.On;
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "frmLoginConfig";
			base.StartPosition = FormStartPosition.CenterParent;
			this.Text = "系统配置";
			base.Load += new EventHandler(this.frmLoginConfig_Load);
			this.tabSetting.ResumeLayout(false);
			this.tabPage1.ResumeLayout(false);
			this.tabPage1.PerformLayout();
			this.groupBox3.ResumeLayout(false);
			this.groupBox3.PerformLayout();
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			((ISupportInitialize)this.numACWtime).EndInit();
			this.groupBox6.ResumeLayout(false);
			this.groupBox6.PerformLayout();
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.tabPage2.ResumeLayout(false);
			this.tabPage4.ResumeLayout(false);
			this.groupBox8.ResumeLayout(false);
			this.groupBox8.PerformLayout();
			base.ResumeLayout(false);
		}

		public frmLoginConfig()
		{
			this.InitializeComponent();
			this.tabSetting.TabPages.RemoveByKey("tabPage2");
		}

		public void set_tabSetting(int TabPageNum)
		{
			this.tabSetting.SelectedIndex = TabPageNum;
		}

		private void readCheck()
		{
			Helper.OpenUrlWhenThreeWayCallin = this.chkWhenThreeWay.Checked;
			Helper.OpenUrlWhenConsultCallin = this.chkWhenConsult.Checked;
			Helper.OpenUrlWhenEavesDropCallin = this.chkWhenEavesDrop.Checked;
			Helper.OpenUrlWhenWhisperCallin = this.chkWhenWhisper.Checked;
			Helper.OpenUrlWhenBargeinCallin = this.chkWhenBargin.Checked;
			Helper.OpenUrlWhenForceHangupCallin = this.chkWhenForceHangup.Checked;
			Helper.Queue_Monitor_Enable = this.chkQueueMonitor.Checked;
		}

		private void LoginCheck()
		{
			this.chkWhenThreeWay.Checked = Helper.OpenUrlWhenThreeWayCallin;
			this.chkWhenConsult.Checked = Helper.OpenUrlWhenConsultCallin;
			this.chkWhenEavesDrop.Checked = Helper.OpenUrlWhenEavesDropCallin;
			this.chkWhenWhisper.Checked = Helper.OpenUrlWhenWhisperCallin;
			this.chkWhenBargin.Checked = Helper.OpenUrlWhenBargeinCallin;
			this.chkWhenForceHangup.Checked = Helper.OpenUrlWhenForceHangupCallin;
			this.chkQueueMonitor.Checked = Helper.Queue_Monitor_Enable;
		}

		private void btnOk_Click(object sender, EventArgs e)
		{
			if (this.chkSetIdle.Checked)
			{
				Helper.AutoSetIdleFromACKTime = Convert.ToInt32(this.numACWtime.Value);
			}
			else
			{
				Helper.AutoSetIdleFromACKTime = 0;
			}
			if (this.chkMoniotrOfflineAgent.Checked)
			{
				Helper.IsMonitorOfflineAgent = true;
			}
			else
			{
				Helper.IsMonitorOfflineAgent = false;
			}
			if (this.chkSoftphoneAutoAnswer.Checked)
			{
				Helper.IsSoftphoneAutoAnswer = true;
			}
			else
			{
				Helper.IsSoftphoneAutoAnswer = false;
			}
			Helper.CallInWebURL = this.txtWebUrl.Text;
			Helper.CallOutWebURL = this.txtWebCallOutUrl.Text;
			Helper.HomePageWebURL = this.txtHomePage.Text;
			Helper.NoAnswerCallsURL = this.txt_noAnswerCallsUrl.Text.Trim();
			this.readCheck();
			Helper.getScreenPhoneFormat = this.cbxPhoneScreen.SelectedIndex.ToString();
			this.ScreenPhoneFormat = this.cbxPhoneScreen.SelectedIndex.ToString();
			Helper.write_all_config_to_file();
			this.mIsMonitorOfflineAgent = Helper.IsMonitorOfflineAgent;
			this.mNoAnswerCallsURL = Helper.NoAnswerCallsURL;
			base.DialogResult = DialogResult.OK;
		}

		private void frmLoginConfig_Load(object sender, EventArgs e)
		{
			ToolTip newToolTip_ACW = new ToolTip();
			newToolTip_ACW.SetToolTip(this.numACWtime, "后处理超时时间必须在30秒和3600秒之间");
			this.numACWtime.Minimum = 30m;
			this.numACWtime.Maximum = 3600m;
			this.numACWtime.Value = 90m;
			this.chkSetIdle.Checked = false;
			this.cbxPhoneScreen.Items.Clear();
			this.cbxPhoneScreen.Items.Add("原始号码");
			this.cbxPhoneScreen.Items.Add("本地电话");
			this.cbxPhoneScreen.Items.Add("区号 + 本地电话");
			this.cbxPhoneScreen.Items.Add("国家码 + 区号 + 本地电话");
			this.load_config();
			this.txtWebUrl.Focus();
			this.LoginCheck();
		}

		private void load_config()
		{
			if (!Helper.load_sys_config())
			{
				MessageBox.Show("读取配置文件失败！  ", "读取配置文件", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			}
			else
			{
				this.txtWebUrl.Text = Helper.CallInWebURL;
				this.txtWebCallOutUrl.Text = Helper.CallOutWebURL;
				this.txtHomePage.Text = Helper.HomePageWebURL;
				if (string.IsNullOrEmpty(Helper.NoAnswerCallsURL))
				{
					this.txt_noAnswerCallsUrl.Text = Helper.ServerIP + ":8080";
				}
				else
				{
					this.txt_noAnswerCallsUrl.Text = Helper.NoAnswerCallsURL;
				}
				int ackTimeout = Helper.AutoSetIdleFromACKTime;
				if (ackTimeout >= 30 && ackTimeout <= 3600)
				{
					this.chkSetIdle.Checked = true;
					this.numACWtime.Value = ackTimeout;
				}
				this.chkSoftphoneAutoAnswer.Checked = Helper.IsSoftphoneAutoAnswer;
				this.chkMoniotrOfflineAgent.Checked = Helper.IsMonitorOfflineAgent;
				this.cbxPhoneScreen.SelectedIndex = int.Parse(Helper.getScreenPhoneFormat);
			}
		}
	}
}
