using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace PLClient
{
	public class frmPersonalConfig : Form
	{
		public delegate void DoGetPersonalInfoEventHandler(string strAgentNum);

		public delegate void DoChangePswdEventHandler(string strAgentNum, string strOldPswd, string strNewPswd);

		public delegate void DoSetPersonalInfoEventHandler(string strAgentNum, string strAgentMobile, string strAgentEmail);

		private string m_agent_num = string.Empty;

		private string m_agent_exten_num = string.Empty;

		private string m_agent_mobile = string.Empty;

		private string m_agent_email = string.Empty;

		private bool m_bPswdSaved = true;

		private bool m_bPersonalInfoSaved = true;

		private IContainer components = null;

		private TabControl tabPersonset;

		private TabPage tabPagePswd;

		private TabPage tabPageInfo;

		private Button btnPswdSubmit;

		private Button btnPswdReset;

		private GroupBox groupBox1;

		private Label label7;

		private TextBox txtEmail;

		private TextBox txtMobile;

		private Label label8;

		private Button btnInfoReset;

		private Button btnInfoSubmit;

		private GroupBox groupBox2;

		private TextBox txtConfirmPswd;

		private TextBox txtNewPswd;

		private TextBox txtOldPswd;

		private Label label3;

		private Label label2;

		private Label label1;

		public event frmPersonalConfig.DoGetPersonalInfoEventHandler DoGetPersonalInfoEvent
		{
			[MethodImpl(MethodImplOptions.Synchronized)]
			add
			{
				this.DoGetPersonalInfoEvent = (frmPersonalConfig.DoGetPersonalInfoEventHandler)Delegate.Combine(this.DoGetPersonalInfoEvent, value);
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			remove
			{
				this.DoGetPersonalInfoEvent = (frmPersonalConfig.DoGetPersonalInfoEventHandler)Delegate.Remove(this.DoGetPersonalInfoEvent, value);
			}
		}

		public event frmPersonalConfig.DoChangePswdEventHandler DoChangePswdEvent
		{
			[MethodImpl(MethodImplOptions.Synchronized)]
			add
			{
				this.DoChangePswdEvent = (frmPersonalConfig.DoChangePswdEventHandler)Delegate.Combine(this.DoChangePswdEvent, value);
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			remove
			{
				this.DoChangePswdEvent = (frmPersonalConfig.DoChangePswdEventHandler)Delegate.Remove(this.DoChangePswdEvent, value);
			}
		}

		public event frmPersonalConfig.DoSetPersonalInfoEventHandler DoSetPersonalInfoEvent
		{
			[MethodImpl(MethodImplOptions.Synchronized)]
			add
			{
				this.DoSetPersonalInfoEvent = (frmPersonalConfig.DoSetPersonalInfoEventHandler)Delegate.Combine(this.DoSetPersonalInfoEvent, value);
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			remove
			{
				this.DoSetPersonalInfoEvent = (frmPersonalConfig.DoSetPersonalInfoEventHandler)Delegate.Remove(this.DoSetPersonalInfoEvent, value);
			}
		}

		public frmPersonalConfig(string strAgentNum)
		{
			this.InitializeComponent();
			this.m_agent_num = strAgentNum;
		}

		private void frmPersonalConfig_Load(object sender, EventArgs e)
		{
			this.load_personal_info();
		}

		private void load_personal_info()
		{
			if (null != this.DoGetPersonalInfoEvent)
			{
				this.DoGetPersonalInfoEvent(this.m_agent_num);
			}
			this.txtOldPswd.Focus();
		}

		private void clear_pswd_info()
		{
			this.txtOldPswd.Text = string.Empty;
			this.txtNewPswd.Text = string.Empty;
			this.txtConfirmPswd.Text = string.Empty;
		}

		private void reset_personal_info()
		{
			this.txtMobile.Text = this.m_agent_mobile;
			this.txtEmail.Text = this.m_agent_email;
		}

		private bool checkPswdIsLegal(string strOldPswd, string strNewPswd, string strConfrimPswd)
		{
			bool result;
			if (string.IsNullOrEmpty(strOldPswd))
			{
				MessageBox.Show("请输入旧密码！", "密码修改", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				this.txtOldPswd.Focus();
				result = false;
			}
			else if (string.IsNullOrEmpty(strNewPswd))
			{
				MessageBox.Show("请输入新密码！", "密码修改", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				this.txtNewPswd.Focus();
				result = false;
			}
			else if (strNewPswd.Length < 4 || strNewPswd.Length > 40)
			{
				MessageBox.Show("密码长度必须介于4和40之间！", "密码修改", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				this.txtNewPswd.SelectAll();
				this.txtNewPswd.Focus();
				result = false;
			}
			else if (string.IsNullOrEmpty(strConfrimPswd))
			{
				MessageBox.Show("请再次输入新密码！", "密码修改", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				this.txtConfirmPswd.Focus();
				result = false;
			}
			else if (!strNewPswd.Equals(strConfrimPswd))
			{
				MessageBox.Show("两次输入的新密码不一致！", "密码修改", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				this.txtNewPswd.SelectAll();
				this.txtNewPswd.Focus();
				result = false;
			}
			else
			{
				result = true;
			}
			return result;
		}

		private bool checkPersonalInfoIsLegal(string strMobile, string strEmail)
		{
			bool result;
			if (string.IsNullOrEmpty(strMobile))
			{
				MessageBox.Show("请输入电话号码！", "个人信息设置", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				this.txtMobile.Focus();
				result = false;
			}
			else if (string.IsNullOrEmpty(strEmail))
			{
				MessageBox.Show("请输入邮箱地址！", "个人信息设置", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				this.txtEmail.Focus();
				result = false;
			}
			else if (!ComFunc.checkNumIsLegal(strMobile))
			{
				MessageBox.Show("请输入正确的电话号码！", "个人信息设置", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				this.txtMobile.SelectAll();
				this.txtMobile.Focus();
				result = false;
			}
			else if (!ComFunc.checkEmailAddrIsLegal(strEmail))
			{
				MessageBox.Show("请输入正确的邮箱地址！", "个人信息设置", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				this.txtEmail.SelectAll();
				this.txtEmail.Focus();
				result = false;
			}
			else
			{
				result = true;
			}
			return result;
		}

		public void OnEvent_GetAgentPersonalInfo(string agent_num, string agent_mobile, string agent_email)
		{
			if (!string.IsNullOrEmpty(agent_num) && agent_num.EndsWith(this.m_agent_num))
			{
				this.m_agent_mobile = agent_mobile;
				this.m_agent_email = agent_email;
				this.txtMobile.Text = agent_mobile;
				this.txtEmail.Text = agent_email;
				this.m_bPersonalInfoSaved = true;
				this.txtOldPswd.Focus();
			}
		}

		public void OnEvent_SetAgentPersonalInfo(string agent_num, int retCode, string reason)
		{
			if (!string.IsNullOrEmpty(agent_num) && agent_num.EndsWith(this.m_agent_num))
			{
				if (0 == retCode)
				{
					this.m_bPersonalInfoSaved = true;
					this.m_agent_mobile = this.txtMobile.Text.Trim();
					this.m_agent_email = this.txtEmail.Text.Trim();
					MessageBox.Show("个人信息修改成功！", "个人信息设置", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				}
				else
				{
					MessageBox.Show("个人信息修改失败！", "个人信息设置", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				}
			}
		}

		public void OnEvent_ChangePswd(string agent_num, int retCode, string reason)
		{
			if (!string.IsNullOrEmpty(agent_num) && agent_num.EndsWith(this.m_agent_num))
			{
				if (0 == retCode)
				{
					this.m_bPswdSaved = true;
					MessageBox.Show("密码修改成功！", "密码修改", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				}
				else if (Helper.ErrorCodeDic.ContainsKey(retCode.ToString()))
				{
					string info = Helper.ErrorCodeDic[retCode.ToString()];
					string title = "提示";
					MessageBox.Show(info, title, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
				}
				else
				{
					MessageBox.Show("无此错误码:" + retCode + " 请更新错误码！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
				}
			}
		}

		private void btnPswdSubmit_Click(object sender, EventArgs e)
		{
			if (this.checkPswdIsLegal(this.txtOldPswd.Text.Trim(), this.txtNewPswd.Text.Trim(), this.txtConfirmPswd.Text.Trim()))
			{
				if (null != this.DoChangePswdEvent)
				{
					this.DoChangePswdEvent(this.m_agent_num, this.txtOldPswd.Text.Trim(), this.txtNewPswd.Text.Trim());
				}
			}
		}

		private void btnPswdReset_Click(object sender, EventArgs e)
		{
			this.clear_pswd_info();
			this.m_bPswdSaved = true;
		}

		private void btnInfoSubmit_Click(object sender, EventArgs e)
		{
			if (this.checkPersonalInfoIsLegal(this.txtMobile.Text.Trim(), this.txtEmail.Text.Trim()))
			{
				if (null != this.DoSetPersonalInfoEvent)
				{
					this.DoSetPersonalInfoEvent(this.m_agent_num, this.txtMobile.Text.Trim(), this.txtEmail.Text.Trim());
				}
			}
		}

		private void btnInfoReset_Click(object sender, EventArgs e)
		{
			this.reset_personal_info();
			this.m_bPersonalInfoSaved = true;
		}

		private void txtChangePswd_TextChanged(object sender, EventArgs e)
		{
			this.m_bPswdSaved = false;
		}

		private void txtPersonalInfo_TextChanged(object sender, EventArgs e)
		{
			this.m_bPersonalInfoSaved = false;
		}

		private void frmPersonalConfig_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (!this.m_bPswdSaved)
			{
				if (DialogResult.Yes != MessageBox.Show("密码修改尚未保存，是否放弃修改！", "密码修改", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Exclamation))
				{
					e.Cancel = true;
					return;
				}
			}
			if (!this.m_bPersonalInfoSaved)
			{
				if (DialogResult.Yes != MessageBox.Show("个人信息尚未保存，是否放弃修改！", "个人信息设置", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Exclamation))
				{
					e.Cancel = true;
				}
			}
		}

		private void tabPersonset_SelectedIndexChanged(object sender, EventArgs e)
		{
			string accessibleName = this.tabPersonset.SelectedTab.AccessibleName;
			if (accessibleName != null)
			{
				if (!(accessibleName == "resetPwd"))
				{
					if (accessibleName == "personInfo")
					{
						this.txtMobile.Focus();
					}
				}
				else
				{
					this.txtOldPswd.Focus();
				}
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
			this.tabPersonset = new TabControl();
			this.tabPagePswd = new TabPage();
			this.groupBox2 = new GroupBox();
			this.txtConfirmPswd = new TextBox();
			this.btnPswdReset = new Button();
			this.txtNewPswd = new TextBox();
			this.btnPswdSubmit = new Button();
			this.txtOldPswd = new TextBox();
			this.label3 = new Label();
			this.label2 = new Label();
			this.label1 = new Label();
			this.tabPageInfo = new TabPage();
			this.groupBox1 = new GroupBox();
			this.btnInfoReset = new Button();
			this.btnInfoSubmit = new Button();
			this.txtEmail = new TextBox();
			this.txtMobile = new TextBox();
			this.label8 = new Label();
			this.label7 = new Label();
			this.tabPersonset.SuspendLayout();
			this.tabPagePswd.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.tabPageInfo.SuspendLayout();
			this.groupBox1.SuspendLayout();
			base.SuspendLayout();
			this.tabPersonset.Controls.Add(this.tabPagePswd);
			this.tabPersonset.Controls.Add(this.tabPageInfo);
			this.tabPersonset.Location = new Point(2, 1);
			this.tabPersonset.Name = "tabPersonset";
			this.tabPersonset.SelectedIndex = 0;
			this.tabPersonset.Size = new Size(342, 393);
			this.tabPersonset.TabIndex = 0;
			this.tabPersonset.SelectedIndexChanged += new EventHandler(this.tabPersonset_SelectedIndexChanged);
			this.tabPagePswd.AccessibleName = "resetPwd";
			this.tabPagePswd.Controls.Add(this.groupBox2);
			this.tabPagePswd.Location = new Point(4, 22);
			this.tabPagePswd.Name = "tabPagePswd";
			this.tabPagePswd.Padding = new Padding(3);
			this.tabPagePswd.Size = new Size(334, 367);
			this.tabPagePswd.TabIndex = 0;
			this.tabPagePswd.Text = "修改密码";
			this.tabPagePswd.UseVisualStyleBackColor = true;
			this.groupBox2.Controls.Add(this.txtConfirmPswd);
			this.groupBox2.Controls.Add(this.btnPswdReset);
			this.groupBox2.Controls.Add(this.txtNewPswd);
			this.groupBox2.Controls.Add(this.btnPswdSubmit);
			this.groupBox2.Controls.Add(this.txtOldPswd);
			this.groupBox2.Controls.Add(this.label3);
			this.groupBox2.Controls.Add(this.label2);
			this.groupBox2.Controls.Add(this.label1);
			this.groupBox2.Location = new Point(6, 6);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new Size(325, 361);
			this.groupBox2.TabIndex = 8;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "密码修改";
			this.txtConfirmPswd.Location = new Point(92, 90);
			this.txtConfirmPswd.MaxLength = 40;
			this.txtConfirmPswd.Name = "txtConfirmPswd";
			this.txtConfirmPswd.PasswordChar = '*';
			this.txtConfirmPswd.Size = new Size(204, 21);
			this.txtConfirmPswd.TabIndex = 12;
			this.txtConfirmPswd.TextChanged += new EventHandler(this.txtChangePswd_TextChanged);
			this.btnPswdReset.Location = new Point(245, 330);
			this.btnPswdReset.Name = "btnPswdReset";
			this.btnPswdReset.Size = new Size(70, 25);
			this.btnPswdReset.TabIndex = 7;
			this.btnPswdReset.Text = "重置";
			this.btnPswdReset.UseVisualStyleBackColor = true;
			this.btnPswdReset.Click += new EventHandler(this.btnPswdReset_Click);
			this.txtNewPswd.Location = new Point(92, 55);
			this.txtNewPswd.MaxLength = 40;
			this.txtNewPswd.Name = "txtNewPswd";
			this.txtNewPswd.PasswordChar = '*';
			this.txtNewPswd.Size = new Size(204, 21);
			this.txtNewPswd.TabIndex = 11;
			this.txtNewPswd.TextChanged += new EventHandler(this.txtChangePswd_TextChanged);
			this.btnPswdSubmit.Location = new Point(169, 330);
			this.btnPswdSubmit.Name = "btnPswdSubmit";
			this.btnPswdSubmit.Size = new Size(70, 25);
			this.btnPswdSubmit.TabIndex = 3;
			this.btnPswdSubmit.Text = "提交";
			this.btnPswdSubmit.UseVisualStyleBackColor = true;
			this.btnPswdSubmit.Click += new EventHandler(this.btnPswdSubmit_Click);
			this.txtOldPswd.Location = new Point(92, 20);
			this.txtOldPswd.MaxLength = 40;
			this.txtOldPswd.Name = "txtOldPswd";
			this.txtOldPswd.PasswordChar = '*';
			this.txtOldPswd.Size = new Size(204, 21);
			this.txtOldPswd.TabIndex = 10;
			this.txtOldPswd.TextChanged += new EventHandler(this.txtChangePswd_TextChanged);
			this.label3.AutoSize = true;
			this.label3.Location = new Point(6, 93);
			this.label3.Name = "label3";
			this.label3.Size = new Size(65, 12);
			this.label3.TabIndex = 9;
			this.label3.Text = "确认新密码";
			this.label2.AutoSize = true;
			this.label2.Location = new Point(6, 58);
			this.label2.Name = "label2";
			this.label2.Size = new Size(41, 12);
			this.label2.TabIndex = 8;
			this.label2.Text = "新密码";
			this.label1.AutoSize = true;
			this.label1.Location = new Point(6, 23);
			this.label1.Name = "label1";
			this.label1.Size = new Size(41, 12);
			this.label1.TabIndex = 7;
			this.label1.Text = "旧密码";
			this.tabPageInfo.AccessibleName = "personInfo";
			this.tabPageInfo.Controls.Add(this.groupBox1);
			this.tabPageInfo.Location = new Point(4, 22);
			this.tabPageInfo.Name = "tabPageInfo";
			this.tabPageInfo.Padding = new Padding(3);
			this.tabPageInfo.Size = new Size(334, 367);
			this.tabPageInfo.TabIndex = 1;
			this.tabPageInfo.Text = "个人信息";
			this.tabPageInfo.UseVisualStyleBackColor = true;
			this.groupBox1.Controls.Add(this.btnInfoReset);
			this.groupBox1.Controls.Add(this.btnInfoSubmit);
			this.groupBox1.Controls.Add(this.txtEmail);
			this.groupBox1.Controls.Add(this.txtMobile);
			this.groupBox1.Controls.Add(this.label8);
			this.groupBox1.Controls.Add(this.label7);
			this.groupBox1.Location = new Point(6, 6);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new Size(325, 361);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "联系方式";
			this.btnInfoReset.Location = new Point(245, 330);
			this.btnInfoReset.Name = "btnInfoReset";
			this.btnInfoReset.Size = new Size(70, 25);
			this.btnInfoReset.TabIndex = 5;
			this.btnInfoReset.Text = "重置";
			this.btnInfoReset.UseVisualStyleBackColor = true;
			this.btnInfoReset.Click += new EventHandler(this.btnInfoReset_Click);
			this.btnInfoSubmit.Location = new Point(169, 330);
			this.btnInfoSubmit.Name = "btnInfoSubmit";
			this.btnInfoSubmit.Size = new Size(70, 25);
			this.btnInfoSubmit.TabIndex = 4;
			this.btnInfoSubmit.Text = "提交";
			this.btnInfoSubmit.UseVisualStyleBackColor = true;
			this.btnInfoSubmit.Click += new EventHandler(this.btnInfoSubmit_Click);
			this.txtEmail.ImeMode = ImeMode.On;
			this.txtEmail.Location = new Point(92, 55);
			this.txtEmail.MaxLength = 50;
			this.txtEmail.Name = "txtEmail";
			this.txtEmail.Size = new Size(204, 21);
			this.txtEmail.TabIndex = 3;
			this.txtEmail.TextChanged += new EventHandler(this.txtPersonalInfo_TextChanged);
			this.txtMobile.ImeMode = ImeMode.On;
			this.txtMobile.Location = new Point(92, 20);
			this.txtMobile.MaxLength = 50;
			this.txtMobile.Name = "txtMobile";
			this.txtMobile.Size = new Size(204, 21);
			this.txtMobile.TabIndex = 2;
			this.txtMobile.TextChanged += new EventHandler(this.txtPersonalInfo_TextChanged);
			this.label8.AutoSize = true;
			this.label8.Location = new Point(6, 58);
			this.label8.Name = "label8";
			this.label8.Size = new Size(29, 12);
			this.label8.TabIndex = 1;
			this.label8.Text = "邮箱";
			this.label7.AutoSize = true;
			this.label7.Location = new Point(6, 23);
			this.label7.Name = "label7";
			this.label7.Size = new Size(29, 12);
			this.label7.TabIndex = 0;
			this.label7.Text = "电话";
			base.AutoScaleDimensions = new SizeF(6f, 12f);
			base.AutoScaleMode = AutoScaleMode.Font;
			base.ClientSize = new Size(345, 399);
			base.Controls.Add(this.tabPersonset);
			base.FormBorderStyle = FormBorderStyle.FixedDialog;
			base.ImeMode = ImeMode.On;
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "frmPersonalConfig";
			base.StartPosition = FormStartPosition.CenterParent;
			this.Text = " 个人设置";
			base.Load += new EventHandler(this.frmPersonalConfig_Load);
			base.FormClosing += new FormClosingEventHandler(this.frmPersonalConfig_FormClosing);
			this.tabPersonset.ResumeLayout(false);
			this.tabPagePswd.ResumeLayout(false);
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			this.tabPageInfo.ResumeLayout(false);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			base.ResumeLayout(false);
		}
	}
}
