using log4net;
using PLClient.Properties;
using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace PLClient
{
	public class FrmLogin : Form
	{
		private enum AgentStatus
		{
			AGENT_STATUS_OFFLINE = -1,
			AGENT_STATUS_IDLE,
			AGENT_STATUS_BUSY = 5,
			AGENT_STATUS_LEAVE,
			AGENT_STATUS_PREDICT_CALL_OUT
		}

		private bool is_build_in_softphone_login = false;

		private string ExtenValue = string.Empty;

		private string StatusValue = string.Empty;

		private string PhoneTypeValue = string.Empty;

		private static ILog exLog;

		private IContainer components = null;

		private BackgroundWorker backgroundWorker1;

		private GroupBox grpLogin;

		public TextBox txtExten;

		public TextBox txtPwd;

		private Label lblExten;

		public TextBox txtUserName;

		private Label lblPwd;

		private Label lblAgentID;

		private Button btnCancel;

		public Button btnOk;

		private Label lblAgentIdReq;

		private Label lblPwdReq;

		private Button btnConfig;

		public Label lblStatus;

		public ComboBox cboStatus;

		private PictureBox pictureBox1;

		private Label label1;

		public ComboBox cboPhoneType;

		private Label label2;

		private Label lblRemember;

		public CheckBox chkRemember;

		public FrmLogin()
		{
			this.InitializeComponent();
		}

		private void FrmLogin_Load(object sender, EventArgs e)
		{
			this.load_AgentStatusAndPhoneType();
			this.load_default_login_info();
			base.Activate();
			this.load_logo();
			this.txtUserName.Focus();
		}

		private void load_AgentStatusAndPhoneType()
		{
			if (!Helper.load_sys_AgentStatueAndPhoneType())
			{
				MessageBox.Show("读取配置文件失败！  ", "读取配置文件", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			}
			else
			{
				this.setAgentStatusAndPhoneType();
			}
		}

		private void reload_AgentStatusAndPhoneType()
		{
			if (!Helper.reload_sys_AgentStatueAndPhoneType())
			{
				MessageBox.Show("读取配置文件失败！  ", "读取配置文件", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			}
			else
			{
				this.setAgentStatusAndPhoneType();
			}
		}

		private void setAgentStatusAndPhoneType()
		{
			this.cboStatus.Items.Clear();
			this.cboPhoneType.Items.Clear();
			if (Helper.ChkSoftPhone == "1")
			{
				this.cboPhoneType.Items.Add("内置软电话");
			}
			if (Helper.ChkExternalPhone == "1")
			{
				this.cboPhoneType.Items.Add("外置电话");
			}
			if (Helper.ChkNotBind == "1")
			{
				this.cboPhoneType.Items.Add("不绑定");
			}
			if (Helper.ChkIdle == "1")
			{
				this.cboStatus.Items.Add("空闲");
			}
			if (Helper.ChkBusy == "1")
			{
				this.cboStatus.Items.Add("忙碌");
			}
			if (Helper.ChkManualCallOut == "1")
			{
				this.cboStatus.Items.Add("手动外呼中");
			}
		}

		private void load_logo()
		{
			this.Text = Helper.ApplicationTitle;
			string path = Application.StartupPath;
			if (File.Exists(path + "\\logo\\logo.jpg"))
			{
				this.pictureBox1.Load(path + "\\logo\\logo.jpg");
			}
		}

		private void load_default_login_info()
		{
			string softTypeStr = string.Empty;
			string agentStatus = string.Empty;
			if (!Helper.load_sys_LastAgentconfig())
			{
				MessageBox.Show("读取配置文件失败！  ", "读取配置文件", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			}
			else
			{
				this.txtUserName.Text = Helper.DefaultAgentNum;
				this.txtPwd.Text = Helper.AgentPwd;
				string defaultStatus = Helper.DefaultStatus;
				if (defaultStatus != null)
				{
					if (defaultStatus == "0")
					{
						agentStatus = "空闲";
						goto IL_AA;
					}
					if (defaultStatus == "1")
					{
						agentStatus = "忙碌";
						goto IL_AA;
					}
					if (defaultStatus == "2")
					{
						agentStatus = "手动外呼中";
						goto IL_AA;
					}
				}
				agentStatus = "空闲";
				IL_AA:
				int i;
				for (i = 0; i < this.cboStatus.Items.Count; i++)
				{
					if (this.cboStatus.Items[i].ToString() == agentStatus)
					{
						this.cboStatus.SelectedIndex = i;
						break;
					}
				}
				if (i == this.cboStatus.Items.Count)
				{
					this.cboStatus.SelectedIndex = 0;
				}
				switch (Helper.SoftPhoneType)
				{
				case 0:
					softTypeStr = "内置软电话";
					this.txtExten.Text = Helper.SipNum;
					break;
				case 1:
					softTypeStr = "外置电话";
					this.txtExten.Text = Helper.DefaultExten;
					this.ExtenValue = Helper.DefaultExten;
					break;
				case 2:
					softTypeStr = "不绑定";
					break;
				default:
					softTypeStr = "不绑定";
					break;
				}
				int j;
				for (j = 0; j < this.cboPhoneType.Items.Count; j++)
				{
					if (this.cboPhoneType.Items[j].ToString() == softTypeStr)
					{
						this.cboPhoneType.SelectedIndex = j;
						break;
					}
				}
				if (j == this.cboPhoneType.Items.Count)
				{
					this.cboPhoneType.SelectedIndex = 0;
				}
				if (Helper.isRemembered)
				{
					this.chkRemember.Checked = true;
				}
				else
				{
					this.chkRemember.Checked = false;
				}
			}
		}

		private void reload_default_login_info()
		{
			string softTypeStr = string.Empty;
			string agentStatus = string.Empty;
			if (!Helper.load_sys_LastAgentconfig())
			{
				MessageBox.Show("读取配置文件失败！  ", "读取配置文件", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			}
			else
			{
				int i;
				for (i = 0; i < this.cboStatus.Items.Count; i++)
				{
					if (this.cboStatus.Items[i].ToString() == this.StatusValue)
					{
						this.cboStatus.SelectedIndex = i;
						break;
					}
				}
				if (i == this.cboStatus.Items.Count)
				{
					this.cboStatus.SelectedIndex = 0;
				}
				int j;
				for (j = 0; j < this.cboPhoneType.Items.Count; j++)
				{
					if (this.cboPhoneType.Items[j].ToString() == this.PhoneTypeValue)
					{
						this.cboPhoneType.SelectedIndex = j;
						break;
					}
				}
				if (j == this.cboPhoneType.Items.Count)
				{
					this.cboPhoneType.SelectedIndex = 0;
				}
			}
		}

		public string str2AgentStatus(string str)
		{
			string result;
			if (str != null)
			{
				if (str == "离线")
				{
					result = "-1";
					return result;
				}
				if (str == "空闲")
				{
					result = "0";
					return result;
				}
				if (str == "忙碌")
				{
					result = "6";
					return result;
				}
				if (str == "离开")
				{
					result = "7";
					return result;
				}
				if (str == "手动外呼中")
				{
					result = "8";
					return result;
				}
			}
			result = "-2";
			return result;
		}

		private void showMainForm(bool blnSuccess)
		{
		}

		private void Event_receiveData(int eventType, int eventQulify, string CallerID, string CalledID, string reason, long retCode, string msg, string cuID)
		{
			MessageBox.Show(string.Concat(new object[]
			{
				"eventType=",
				eventType,
				",eventQulify=",
				eventQulify,
				",callerID=",
				CallerID,
				",strCalledID=",
				CalledID,
				",reason=",
				reason,
				",retCode=",
				retCode,
				",msg=",
				msg,
				",cuID=",
				cuID
			}));
		}

		private void Event_disconnect()
		{
			MessageBox.Show("与服务器断开连接！！");
		}

		private void btnOk_Click(object sender, EventArgs e)
		{
			if (this.txtUserName.Text == "" || this.txtPwd.Text == "")
			{
				MessageBox.Show("用户名和密码不能为空！", "登录", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
			}
			else if (!ComFunc.checkNumIsLegal(this.txtUserName.Text.Trim()))
			{
				MessageBox.Show("用户名填写不合法！", "登录", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
			}
			else if (this.cboPhoneType.Text == "内置软电话" && this.txtExten.Text == "")
			{
				MessageBox.Show("请先到“系统配置”中配置软电话！", "登录", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
			}
			else if (this.cboPhoneType.Text == "外置电话" && this.txtExten.Text == "")
			{
				MessageBox.Show("请先输入分机号！", "登录", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
			}
			else
			{
				if (this.cboPhoneType.Text == "内置软电话")
				{
					this.is_build_in_softphone_login = true;
					if (string.IsNullOrEmpty(Helper.SipNum) || string.IsNullOrEmpty(Helper.SipServer))
					{
						MessageBox.Show("软电话配置填写不完整！", "保存", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
						return;
					}
					if (!ComFunc.checkNumIsLegal(Helper.SipNum) || !ComFunc.checkIp4IsLegal(Helper.SipServer) || !ComFunc.checkPortIsLegal(Helper.SipPort.ToString()))
					{
						MessageBox.Show("软电话号码或服务器地址或端口号不合法！", "保存", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
						return;
					}
				}
				base.DialogResult = DialogResult.OK;
			}
		}

		private void btnCancel_Click(object sender, EventArgs e)
		{
			base.Visible = false;
			base.Close();
			Environment.Exit(0);
		}

		private void btnConfig_Click(object sender, EventArgs e)
		{
			FrmConfig newFrmConfig = new FrmConfig();
			Helper.AgentID = this.txtUserName.Text;
			if (this.cboPhoneType.Text == "内置软电话")
			{
				newFrmConfig.ConfigSoftPhoneEnable = true;
			}
			else
			{
				newFrmConfig.ConfigSoftPhoneEnable = false;
			}
			if (newFrmConfig.ShowDialog() == DialogResult.OK)
			{
				this.refresh_loginForm();
				this.reload_AgentStatusAndPhoneType();
				this.reload_default_login_info();
			}
		}

		private void refresh_loginForm()
		{
			if (this.cboPhoneType.Text == "内置软电话")
			{
				this.txtExten.Text = Helper.SipNum;
			}
		}

		private void FrmLogin_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (this.is_build_in_softphone_login)
			{
				this.is_build_in_softphone_login = false;
				if (!Program.newFrmMain.softPhone_close())
				{
					e.Cancel = true;
				}
			}
		}

		protected override void WndProc(ref Message msg)
		{
			if (msg.Msg == 274 && (int)msg.WParam == 61536)
			{
				base.Visible = false;
				base.Close();
				Environment.Exit(0);
			}
			base.WndProc(ref msg);
		}

		private void cboPhoneType_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (this.cboPhoneType.Text == "内置软电话")
			{
				this.PhoneTypeValue = "内置软电话";
				Helper.AgentID = this.txtUserName.Text;
				Helper.load_sys_AndPhoneTypeNum();
				this.txtExten.Text = Helper.SipNum;
				this.txtExten.Enabled = false;
			}
			else if (this.cboPhoneType.Text == "外置电话")
			{
				this.PhoneTypeValue = "外置电话";
				this.txtExten.Text = this.ExtenValue;
				this.txtExten.Enabled = true;
			}
			else
			{
				this.PhoneTypeValue = "不绑定";
				this.txtExten.Text = "";
				this.txtExten.Enabled = false;
			}
		}

		private void txtUserName_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (!char.IsNumber(e.KeyChar) && e.KeyChar != '\r' && e.KeyChar != '\b' && e.KeyChar != '\u0001' && e.KeyChar != '\u0003' && e.KeyChar != '\u0016' && e.KeyChar != '\u0018')
			{
				e.Handled = true;
			}
			else if (e.KeyChar == '\u0001')
			{
				this.txtUserName.SelectAll();
				this.txtUserName.Focus();
			}
		}

		private void txtExten_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (!char.IsNumber(e.KeyChar) && e.KeyChar != '\r' && e.KeyChar != '\b' && e.KeyChar != '\u0001' && e.KeyChar != '\u0003' && e.KeyChar != '\u0016' && e.KeyChar != '\u0018')
			{
				e.Handled = true;
			}
			else if (e.KeyChar == '\u0001')
			{
				this.txtExten.SelectAll();
				this.txtExten.Focus();
			}
		}

		private void FrmLogin_FormClosed(object sender, FormClosedEventArgs e)
		{
		}

		private void txtExten_TextChanged(object sender, EventArgs e)
		{
			if (this.cboPhoneType.Text == "外置电话")
			{
				this.ExtenValue = this.txtExten.Text;
			}
		}

		private void cboStatus_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (this.cboStatus.Text == "空闲")
			{
				this.StatusValue = "空闲";
			}
			else if (this.cboStatus.Text == "忙碌")
			{
				this.StatusValue = "忙碌";
			}
			else
			{
				this.StatusValue = "手动外呼中";
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
			ComponentResourceManager resources = new ComponentResourceManager(typeof(FrmLogin));
			this.backgroundWorker1 = new BackgroundWorker();
			this.grpLogin = new GroupBox();
			this.chkRemember = new CheckBox();
			this.cboPhoneType = new ComboBox();
			this.cboStatus = new ComboBox();
			this.lblPwdReq = new Label();
			this.lblAgentIdReq = new Label();
			this.txtExten = new TextBox();
			this.txtPwd = new TextBox();
			this.label2 = new Label();
			this.lblRemember = new Label();
			this.label1 = new Label();
			this.lblExten = new Label();
			this.txtUserName = new TextBox();
			this.lblPwd = new Label();
			this.lblAgentID = new Label();
			this.lblStatus = new Label();
			this.btnCancel = new Button();
			this.btnOk = new Button();
			this.btnConfig = new Button();
			this.pictureBox1 = new PictureBox();
			this.grpLogin.SuspendLayout();
			((ISupportInitialize)this.pictureBox1).BeginInit();
			base.SuspendLayout();
			this.grpLogin.Anchor = (AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right);
			this.grpLogin.Controls.Add(this.chkRemember);
			this.grpLogin.Controls.Add(this.cboPhoneType);
			this.grpLogin.Controls.Add(this.cboStatus);
			this.grpLogin.Controls.Add(this.lblPwdReq);
			this.grpLogin.Controls.Add(this.lblAgentIdReq);
			this.grpLogin.Controls.Add(this.txtExten);
			this.grpLogin.Controls.Add(this.txtPwd);
			this.grpLogin.Controls.Add(this.label2);
			this.grpLogin.Controls.Add(this.lblRemember);
			this.grpLogin.Controls.Add(this.label1);
			this.grpLogin.Controls.Add(this.lblExten);
			this.grpLogin.Controls.Add(this.txtUserName);
			this.grpLogin.Controls.Add(this.lblPwd);
			this.grpLogin.Controls.Add(this.lblAgentID);
			this.grpLogin.Location = new Point(12, 53);
			this.grpLogin.Name = "grpLogin";
			this.grpLogin.Size = new Size(335, 208);
			this.grpLogin.TabIndex = 8;
			this.grpLogin.TabStop = false;
			this.grpLogin.Text = "登录";
			this.chkRemember.AutoSize = true;
			this.chkRemember.Location = new Point(119, 174);
			this.chkRemember.Name = "chkRemember";
			this.chkRemember.Size = new Size(15, 14);
			this.chkRemember.TabIndex = 18;
			this.chkRemember.UseVisualStyleBackColor = true;
			this.cboPhoneType.DropDownStyle = ComboBoxStyle.DropDownList;
			this.cboPhoneType.FormattingEnabled = true;
			this.cboPhoneType.Location = new Point(119, 79);
			this.cboPhoneType.Name = "cboPhoneType";
			this.cboPhoneType.Size = new Size(190, 20);
			this.cboPhoneType.TabIndex = 2;
			this.cboPhoneType.SelectedIndexChanged += new EventHandler(this.cboPhoneType_SelectedIndexChanged);
			this.cboStatus.DropDownStyle = ComboBoxStyle.DropDownList;
			this.cboStatus.FormattingEnabled = true;
			this.cboStatus.Location = new Point(119, 140);
			this.cboStatus.Name = "cboStatus";
			this.cboStatus.Size = new Size(190, 20);
			this.cboStatus.TabIndex = 4;
			this.cboStatus.SelectedIndexChanged += new EventHandler(this.cboStatus_SelectedIndexChanged);
			this.lblPwdReq.Anchor = (AnchorStyles.Top | AnchorStyles.Right);
			this.lblPwdReq.ForeColor = Color.Red;
			this.lblPwdReq.Location = new Point(319, 53);
			this.lblPwdReq.Name = "lblPwdReq";
			this.lblPwdReq.Size = new Size(8, 21);
			this.lblPwdReq.TabIndex = 17;
			this.lblPwdReq.Text = "*";
			this.lblPwdReq.TextAlign = ContentAlignment.MiddleCenter;
			this.lblAgentIdReq.Anchor = (AnchorStyles.Top | AnchorStyles.Right);
			this.lblAgentIdReq.ForeColor = Color.Red;
			this.lblAgentIdReq.Location = new Point(319, 17);
			this.lblAgentIdReq.Name = "lblAgentIdReq";
			this.lblAgentIdReq.Size = new Size(8, 21);
			this.lblAgentIdReq.TabIndex = 16;
			this.lblAgentIdReq.Text = "*";
			this.lblAgentIdReq.TextAlign = ContentAlignment.MiddleCenter;
			this.txtExten.Anchor = (AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right);
			this.txtExten.Location = new Point(119, 109);
			this.txtExten.Name = "txtExten";
			this.txtExten.Size = new Size(190, 21);
			this.txtExten.TabIndex = 3;
			this.txtExten.Text = "3009";
			this.txtExten.TextChanged += new EventHandler(this.txtExten_TextChanged);
			this.txtExten.KeyPress += new KeyPressEventHandler(this.txtExten_KeyPress);
			this.txtPwd.Anchor = (AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right);
			this.txtPwd.Location = new Point(119, 48);
			this.txtPwd.Name = "txtPwd";
			this.txtPwd.PasswordChar = '*';
			this.txtPwd.Size = new Size(190, 21);
			this.txtPwd.TabIndex = 1;
			this.txtPwd.Text = "1234";
			this.label2.Font = new Font("宋体", 9f);
			this.label2.Location = new Point(10, 79);
			this.label2.Name = "label2";
			this.label2.Size = new Size(103, 21);
			this.label2.TabIndex = 14;
			this.label2.Text = "绑定分机类型：";
			this.label2.TextAlign = ContentAlignment.MiddleRight;
			this.lblRemember.Font = new Font("宋体", 9f);
			this.lblRemember.Location = new Point(10, 171);
			this.lblRemember.Name = "lblRemember";
			this.lblRemember.Size = new Size(103, 21);
			this.lblRemember.TabIndex = 14;
			this.lblRemember.Text = "记住密码：";
			this.lblRemember.TextAlign = ContentAlignment.MiddleRight;
			this.label1.Font = new Font("宋体", 9f);
			this.label1.Location = new Point(10, 141);
			this.label1.Name = "label1";
			this.label1.Size = new Size(103, 21);
			this.label1.TabIndex = 14;
			this.label1.Text = "登录后状态：";
			this.label1.TextAlign = ContentAlignment.MiddleRight;
			this.lblExten.Font = new Font("宋体", 9f);
			this.lblExten.Location = new Point(10, 110);
			this.lblExten.Name = "lblExten";
			this.lblExten.Size = new Size(103, 21);
			this.lblExten.TabIndex = 14;
			this.lblExten.Text = "分机号：";
			this.lblExten.TextAlign = ContentAlignment.MiddleRight;
			this.txtUserName.Anchor = (AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right);
			this.txtUserName.Location = new Point(119, 17);
			this.txtUserName.Name = "txtUserName";
			this.txtUserName.Size = new Size(190, 21);
			this.txtUserName.TabIndex = 0;
			this.txtUserName.Text = "1047";
			this.txtUserName.KeyPress += new KeyPressEventHandler(this.txtUserName_KeyPress);
			this.lblPwd.Font = new Font("宋体", 9f);
			this.lblPwd.Location = new Point(8, 48);
			this.lblPwd.Name = "lblPwd";
			this.lblPwd.Size = new Size(105, 21);
			this.lblPwd.TabIndex = 15;
			this.lblPwd.Text = "密  码：";
			this.lblPwd.TextAlign = ContentAlignment.MiddleRight;
			this.lblAgentID.Font = new Font("宋体", 9f);
			this.lblAgentID.Location = new Point(6, 17);
			this.lblAgentID.Name = "lblAgentID";
			this.lblAgentID.Size = new Size(107, 21);
			this.lblAgentID.TabIndex = 9;
			this.lblAgentID.Text = "坐席号：";
			this.lblAgentID.TextAlign = ContentAlignment.MiddleRight;
			this.lblStatus.ForeColor = Color.Blue;
			this.lblStatus.Location = new Point(397, 204);
			this.lblStatus.Name = "lblStatus";
			this.lblStatus.Size = new Size(170, 32);
			this.lblStatus.TabIndex = 17;
			this.lblStatus.Visible = false;
			this.btnCancel.Anchor = (AnchorStyles.Bottom | AnchorStyles.Right);
			this.btnCancel.DialogResult = DialogResult.Cancel;
			this.btnCancel.Font = new Font("宋体", 9f, FontStyle.Regular, GraphicsUnit.Point, 134);
			this.btnCancel.Location = new Point(272, 267);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new Size(75, 25);
			this.btnCancel.TabIndex = 6;
			this.btnCancel.Text = "取消";
			this.btnCancel.UseVisualStyleBackColor = true;
			this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
			this.btnOk.Anchor = (AnchorStyles.Bottom | AnchorStyles.Right);
			this.btnOk.Font = new Font("宋体", 9f, FontStyle.Regular, GraphicsUnit.Point, 134);
			this.btnOk.Location = new Point(191, 267);
			this.btnOk.Name = "btnOk";
			this.btnOk.Size = new Size(75, 25);
			this.btnOk.TabIndex = 5;
			this.btnOk.Text = "登录(&L)";
			this.btnOk.UseVisualStyleBackColor = true;
			this.btnOk.Click += new EventHandler(this.btnOk_Click);
			this.btnConfig.Anchor = (AnchorStyles.Bottom | AnchorStyles.Left);
			this.btnConfig.Location = new Point(12, 267);
			this.btnConfig.Name = "btnConfig";
			this.btnConfig.Size = new Size(94, 25);
			this.btnConfig.TabIndex = 7;
			this.btnConfig.Text = "系统配置";
			this.btnConfig.UseVisualStyleBackColor = true;
			this.btnConfig.Click += new EventHandler(this.btnConfig_Click);
			this.pictureBox1.Image = Resources.polylink_logo;
			this.pictureBox1.Location = new Point(107, 9);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new Size(177, 41);
			this.pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
			this.pictureBox1.TabIndex = 17;
			this.pictureBox1.TabStop = false;
			base.AcceptButton = this.btnOk;
			base.AutoScaleDimensions = new SizeF(6f, 12f);
			base.AutoScaleMode = AutoScaleMode.Font;
			this.AutoSize = true;
			this.BackgroundImageLayout = ImageLayout.None;
			base.CancelButton = this.btnCancel;
			base.ClientSize = new Size(374, 304);
			base.Controls.Add(this.pictureBox1);
			base.Controls.Add(this.btnConfig);
			base.Controls.Add(this.btnCancel);
			base.Controls.Add(this.btnOk);
			base.Controls.Add(this.lblStatus);
			base.Controls.Add(this.grpLogin);
			this.DoubleBuffered = true;
			base.FormBorderStyle = FormBorderStyle.FixedSingle;
			base.Icon = (Icon)resources.GetObject("$this.Icon");
			base.MaximizeBox = false;
			base.Name = "FrmLogin";
			base.StartPosition = FormStartPosition.CenterScreen;
			this.Text = "Polylink CTI Client";
			base.Load += new EventHandler(this.FrmLogin_Load);
			base.FormClosed += new FormClosedEventHandler(this.FrmLogin_FormClosed);
			base.FormClosing += new FormClosingEventHandler(this.FrmLogin_FormClosing);
			this.grpLogin.ResumeLayout(false);
			this.grpLogin.PerformLayout();
			((ISupportInitialize)this.pictureBox1).EndInit();
			base.ResumeLayout(false);
		}
	}
}
