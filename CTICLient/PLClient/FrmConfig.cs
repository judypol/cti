using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace PLClient
{
	public class FrmConfig : Form
	{
		private bool mConfigSoftphoneEnable;

		public string strSoftPhone;

		public string strExternalPhone;

		public string strNotBind;

		public string strIdle;

		public string strBusy;

		public string strManualCallOut;

		private IContainer components = null;

		private TextBox txtPort;

		private Label lblPort;

		private GroupBox groupBox2;

		private TextBox txtServerIp;

		private Label lblServer;

		private TabPage tabPage1;

		private TabControl tabSetting;

		private TabPage tabPage2;

		private Label label7;

		private Label label8;

		private Label label9;

		private Label label10;

		private Label label11;

		private Label label12;

		private Button button2;

		private Button button3;

		private CheckBox checkBox1;

		private GroupBox groupBox3;

		private Label label1;

		private TextBox textBox1;

		private Label label2;

		private TextBox textBox2;

		private GroupBox groupBox4;

		private TextBox textBox3;

		private Label label3;

		private TextBox textBox4;

		private Label label4;

		private TextBox textBox5;

		private Label label5;

		private Label label6;

		private TextBox textBox6;

		private Button btnCancel;

		private Button btnOk;

		public GroupBox grp2;

		private NumericUpDown numRegistTime;

		public CheckBox chkAutoAnswer;

		public TextBox txtSipLocalNum;

		public GroupBox grp1;

		public TextBox txtSipPort;

		public TextBox txtSipServer;

		public TextBox txtSipPwd;

		public TextBox txtSipNum;

		private Label lblinfo3;

		private GroupBox groupBox1;

		private GroupBox groupBox5;

		private CheckBox chkNotBind;

		private CheckBox chkExternalPhone;

		private CheckBox chkSoftPhone;

		private CheckBox chkManualCallOut;

		private CheckBox chkBusy;

		private CheckBox chkIdle;

		public bool ConfigSoftPhoneEnable
		{
			get
			{
				return this.mConfigSoftphoneEnable;
			}
			set
			{
				this.mConfigSoftphoneEnable = value;
			}
		}

		public FrmConfig()
		{
			this.InitializeComponent();
		}

		private void button5_Click(object sender, EventArgs e)
		{
		}

		private void btnOk_Click(object sender, EventArgs e)
		{
			try
			{
				if (this.txtPort.Text.Trim() == "" || this.txtServerIp.Text.Trim() == "")
				{
					MessageBox.Show("登录帐号信息填写不完整！", "保存", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
				}
				else if (!ComFunc.checkIp4IsLegal(this.txtServerIp.Text.Trim()))
				{
					MessageBox.Show("服务器IP填写不合法！", "保存", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
				}
				else if (!ComFunc.checkPortIsLegal(this.txtPort.Text.Trim()))
				{
					MessageBox.Show("服务器端口号填写不合法，端口号输入范围(0到65535)！", "保存", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
				}
				else
				{
					if (this.txtSipPort.Text == "")
					{
						this.txtSipPort.Text = "5060";
					}
					if (this.chkSoftPhone.Checked)
					{
						this.strSoftPhone = "1";
					}
					else
					{
						this.strSoftPhone = "0";
					}
					if (this.chkExternalPhone.Checked)
					{
						this.strExternalPhone = "1";
					}
					else
					{
						this.strExternalPhone = "0";
					}
					if (this.chkNotBind.Checked)
					{
						this.strNotBind = "1";
					}
					else
					{
						this.strNotBind = "0";
					}
					if (this.chkIdle.Checked)
					{
						this.strIdle = "1";
					}
					else
					{
						this.strIdle = "0";
					}
					if (this.chkBusy.Checked)
					{
						this.strBusy = "1";
					}
					else
					{
						this.strBusy = "0";
					}
					if (this.chkManualCallOut.Checked)
					{
						this.strManualCallOut = "1";
					}
					else
					{
						this.strManualCallOut = "0";
					}
					if (!this.chkSoftPhone.Checked && !this.chkExternalPhone.Checked && !this.chkNotBind.Checked)
					{
						MessageBox.Show("绑定分机类型至少选一个！");
					}
					else if (!this.chkIdle.Checked && !this.chkBusy.Checked && !this.chkManualCallOut.Checked)
					{
						MessageBox.Show("登陆后状态至少选一个！");
					}
					else
					{
						Helper helper = new Helper(this.txtServerIp.Text, Convert.ToInt32(this.txtPort.Text), this.txtSipNum.Text, this.txtSipPwd.Text, this.txtSipServer.Text, Convert.ToInt32(this.txtSipPort.Text), this.txtSipLocalNum.Text, this.chkAutoAnswer.Checked, Convert.ToInt32(this.numRegistTime.Value), this.strSoftPhone, this.strExternalPhone, this.strNotBind, this.strIdle, this.strBusy, this.strManualCallOut);
						Helper.NoAnswerCallsURL = this.txtServerIp.Text + ":8080";
						Helper.write_SystemConfig_to_file();
						string strAutoAnswer = "0";
						if (this.chkAutoAnswer.Checked)
						{
							strAutoAnswer = "1";
						}
						string softphone_path = Environment.GetEnvironmentVariable("APPDATA") + "\\wonderUsers\\" + this.txtSipNum.Text + "\\config.xml";
						if (File.Exists(softphone_path))
						{
							if (!Helper.write_conf_to_softphone(softphone_path, strAutoAnswer, this.numRegistTime.Value.ToString()))
							{
								MessageBox.Show("保存文件失败！", "保存失败", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
							}
						}
						else
						{
							softphone_path = Environment.GetEnvironmentVariable("APPDATA") + "\\wonderUsers\\config.xml";
							if (File.Exists(softphone_path))
							{
								if (!Helper.write_conf_to_softphone(softphone_path, strAutoAnswer, this.numRegistTime.Value.ToString()))
								{
									MessageBox.Show("保存文件失败！", "保存失败", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
								}
							}
							else
							{
								MessageBox.Show("文件不存在，保存失败！", "保存失败", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
							}
						}
						base.DialogResult = DialogResult.OK;
					}
				}
			}
			catch (Exception)
			{
			}
		}

		private void sipPwd_TextChanged(object sender, EventArgs e)
		{
		}

		private void load_config()
		{
			if (!Helper.load_sysAgentconfig())
			{
				MessageBox.Show("读取配置文件失败！  ", "读取配置文件", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			}
			else
			{
				this.txtServerIp.Text = Helper.ServerIP;
				this.txtPort.Text = Helper.Port.ToString();
				this.txtSipNum.Text = Helper.SipNum;
				this.txtSipPwd.Text = Helper.SipPwd;
				this.txtSipServer.Text = Helper.SipServer;
				this.txtSipPort.Text = Helper.SipPort.ToString();
				this.numRegistTime.Value = Convert.ToInt32(Helper.SipRegistTime.ToString());
				this.chkAutoAnswer.Checked = Helper.SipAutoAnswer;
				this.txtSipLocalNum.Text = this.txtSipNum.Text;
				this.chkSoftPhone.Checked = (Helper.ChkSoftPhone == "1");
				this.chkExternalPhone.Checked = (Helper.ChkExternalPhone == "1");
				this.chkNotBind.Checked = (Helper.ChkNotBind == "1");
				this.chkIdle.Checked = (Helper.ChkIdle == "1");
				this.chkBusy.Checked = (Helper.ChkBusy == "1");
				this.chkManualCallOut.Checked = (Helper.ChkManualCallOut == "1");
			}
		}

		private void FrmConfig_Load(object sender, EventArgs e)
		{
			ToolTip newToolTip = new ToolTip();
			newToolTip.SetToolTip(this.numRegistTime, "注册间隔时间必须在30秒和3600秒之间");
			this.txtSipLocalNum.Enabled = false;
			this.load_config();
		}

		private void cboStatus_SelectedIndexChanged(object sender, EventArgs e)
		{
		}

		private void label13_Click(object sender, EventArgs e)
		{
		}

		private void txtSipLocalNum_TextChanged(object sender, EventArgs e)
		{
		}

		private void txtRegistTimeOut_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (!char.IsNumber(e.KeyChar) && e.KeyChar != '\r' && e.KeyChar != '\b')
			{
				e.Handled = true;
			}
		}

		private void txtSipNum_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (!char.IsNumber(e.KeyChar) && e.KeyChar != '\r' && e.KeyChar != '\b' && e.KeyChar != '\u0001' && e.KeyChar != '\u0003' && e.KeyChar != '\u0016' && e.KeyChar != '\u0018')
			{
				e.Handled = true;
			}
			else if (e.KeyChar == '\u0001')
			{
				this.txtSipNum.SelectAll();
				this.txtSipNum.Focus();
			}
		}

		private void txtSipServer_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (!char.IsNumber(e.KeyChar) && e.KeyChar != '\r' && e.KeyChar != '\b' && e.KeyChar != '.' && e.KeyChar != '\u0001' && e.KeyChar != '\u0003' && e.KeyChar != '\u0016' && e.KeyChar != '\u0018')
			{
				e.Handled = true;
			}
			else if (e.KeyChar == '\u0001')
			{
				this.txtSipServer.SelectAll();
				this.txtSipServer.Focus();
			}
		}

		private void txtSipPort_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (!char.IsNumber(e.KeyChar) && e.KeyChar != '\r' && e.KeyChar != '\b' && e.KeyChar != '\u0001' && e.KeyChar != '\u0003' && e.KeyChar != '\u0016' && e.KeyChar != '\u0018')
			{
				e.Handled = true;
			}
			else if (e.KeyChar == '\u0001')
			{
				this.txtSipPort.SelectAll();
				this.txtSipPort.Focus();
			}
		}

		private void txtServerIp_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (!char.IsNumber(e.KeyChar) && e.KeyChar != '\r' && e.KeyChar != '\b' && e.KeyChar != '.' && e.KeyChar != '\u0001' && e.KeyChar != '\u0003' && e.KeyChar != '\u0016' && e.KeyChar != '\u0018')
			{
				e.Handled = true;
			}
			else if (e.KeyChar == '\u0001')
			{
				this.txtServerIp.SelectAll();
				this.txtServerIp.Focus();
			}
		}

		private void txtPort_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (!char.IsNumber(e.KeyChar) && e.KeyChar != '\r' && e.KeyChar != '\b' && e.KeyChar != '\u0001' && e.KeyChar != '\u0003' && e.KeyChar != '\u0016' && e.KeyChar != '\u0018')
			{
				e.Handled = true;
			}
			else if (e.KeyChar == '\u0001')
			{
				this.txtPort.SelectAll();
				this.txtPort.Focus();
			}
		}

		private void txtSipNum_TextChanged(object sender, EventArgs e)
		{
			this.txtSipLocalNum.Text = this.txtSipNum.Text;
		}

		private void btnCancel_Click(object sender, EventArgs e)
		{
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
			ComponentResourceManager resources = new ComponentResourceManager(typeof(FrmConfig));
			this.txtPort = new TextBox();
			this.lblPort = new Label();
			this.groupBox2 = new GroupBox();
			this.txtServerIp = new TextBox();
			this.lblServer = new Label();
			this.tabPage1 = new TabPage();
			this.groupBox1 = new GroupBox();
			this.chkNotBind = new CheckBox();
			this.chkExternalPhone = new CheckBox();
			this.chkSoftPhone = new CheckBox();
			this.groupBox5 = new GroupBox();
			this.chkManualCallOut = new CheckBox();
			this.chkBusy = new CheckBox();
			this.chkIdle = new CheckBox();
			this.lblinfo3 = new Label();
			this.tabSetting = new TabControl();
			this.tabPage2 = new TabPage();
			this.grp2 = new GroupBox();
			this.numRegistTime = new NumericUpDown();
			this.chkAutoAnswer = new CheckBox();
			this.label7 = new Label();
			this.txtSipLocalNum = new TextBox();
			this.label8 = new Label();
			this.grp1 = new GroupBox();
			this.txtSipPort = new TextBox();
			this.label9 = new Label();
			this.txtSipServer = new TextBox();
			this.label10 = new Label();
			this.txtSipPwd = new TextBox();
			this.label11 = new Label();
			this.label12 = new Label();
			this.txtSipNum = new TextBox();
			this.button2 = new Button();
			this.button3 = new Button();
			this.checkBox1 = new CheckBox();
			this.groupBox3 = new GroupBox();
			this.label1 = new Label();
			this.textBox1 = new TextBox();
			this.label2 = new Label();
			this.textBox2 = new TextBox();
			this.groupBox4 = new GroupBox();
			this.textBox3 = new TextBox();
			this.label3 = new Label();
			this.textBox4 = new TextBox();
			this.label4 = new Label();
			this.textBox5 = new TextBox();
			this.label5 = new Label();
			this.label6 = new Label();
			this.textBox6 = new TextBox();
			this.btnCancel = new Button();
			this.btnOk = new Button();
			this.groupBox2.SuspendLayout();
			this.tabPage1.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.groupBox5.SuspendLayout();
			this.tabSetting.SuspendLayout();
			this.tabPage2.SuspendLayout();
			this.grp2.SuspendLayout();
			((ISupportInitialize)this.numRegistTime).BeginInit();
			this.grp1.SuspendLayout();
			this.groupBox3.SuspendLayout();
			this.groupBox4.SuspendLayout();
			base.SuspendLayout();
			this.txtPort.Location = new Point(101, 54);
			this.txtPort.Name = "txtPort";
			this.txtPort.Size = new Size(187, 21);
			this.txtPort.TabIndex = 36;
			this.txtPort.KeyPress += new KeyPressEventHandler(this.txtPort_KeyPress);
			this.lblPort.AutoSize = true;
			this.lblPort.Location = new Point(28, 58);
			this.lblPort.Name = "lblPort";
			this.lblPort.Size = new Size(65, 12);
			this.lblPort.TabIndex = 35;
			this.lblPort.Text = "端 口 号：";
			this.lblPort.TextAlign = ContentAlignment.MiddleRight;
			this.groupBox2.Controls.Add(this.txtPort);
			this.groupBox2.Controls.Add(this.lblPort);
			this.groupBox2.Controls.Add(this.txtServerIp);
			this.groupBox2.Controls.Add(this.lblServer);
			this.groupBox2.Location = new Point(6, 6);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new Size(309, 91);
			this.groupBox2.TabIndex = 31;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "连接服务器";
			this.txtServerIp.Location = new Point(101, 22);
			this.txtServerIp.Name = "txtServerIp";
			this.txtServerIp.Size = new Size(187, 21);
			this.txtServerIp.TabIndex = 34;
			this.txtServerIp.KeyPress += new KeyPressEventHandler(this.txtServerIp_KeyPress);
			this.lblServer.AutoSize = true;
			this.lblServer.Location = new Point(28, 27);
			this.lblServer.Name = "lblServer";
			this.lblServer.Size = new Size(65, 12);
			this.lblServer.TabIndex = 33;
			this.lblServer.Text = "服务器IP：";
			this.lblServer.TextAlign = ContentAlignment.MiddleRight;
			this.tabPage1.Controls.Add(this.groupBox1);
			this.tabPage1.Controls.Add(this.groupBox5);
			this.tabPage1.Controls.Add(this.groupBox2);
			this.tabPage1.Location = new Point(4, 22);
			this.tabPage1.Name = "tabPage1";
			this.tabPage1.Padding = new Padding(3);
			this.tabPage1.Size = new Size(337, 335);
			this.tabPage1.TabIndex = 0;
			this.tabPage1.Text = "登录帐号";
			this.tabPage1.UseVisualStyleBackColor = true;
			this.groupBox1.Controls.Add(this.chkNotBind);
			this.groupBox1.Controls.Add(this.chkExternalPhone);
			this.groupBox1.Controls.Add(this.chkSoftPhone);
			this.groupBox1.Location = new Point(6, 104);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new Size(309, 84);
			this.groupBox1.TabIndex = 34;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "绑定分机类型";
			this.chkNotBind.AutoSize = true;
			this.chkNotBind.Location = new Point(8, 55);
			this.chkNotBind.Name = "chkNotBind";
			this.chkNotBind.Size = new Size(60, 16);
			this.chkNotBind.TabIndex = 2;
			this.chkNotBind.Text = "不绑定";
			this.chkNotBind.UseVisualStyleBackColor = true;
			this.chkExternalPhone.AutoSize = true;
			this.chkExternalPhone.Location = new Point(140, 21);
			this.chkExternalPhone.Name = "chkExternalPhone";
			this.chkExternalPhone.Size = new Size(72, 16);
			this.chkExternalPhone.TabIndex = 1;
			this.chkExternalPhone.Text = "外置电话";
			this.chkExternalPhone.UseVisualStyleBackColor = true;
			this.chkSoftPhone.AutoSize = true;
			this.chkSoftPhone.Location = new Point(8, 21);
			this.chkSoftPhone.Name = "chkSoftPhone";
			this.chkSoftPhone.Size = new Size(84, 16);
			this.chkSoftPhone.TabIndex = 0;
			this.chkSoftPhone.Text = "内置软电话";
			this.chkSoftPhone.UseVisualStyleBackColor = true;
			this.groupBox5.Controls.Add(this.chkManualCallOut);
			this.groupBox5.Controls.Add(this.chkBusy);
			this.groupBox5.Controls.Add(this.chkIdle);
			this.groupBox5.Location = new Point(6, 206);
			this.groupBox5.Name = "groupBox5";
			this.groupBox5.Size = new Size(318, 85);
			this.groupBox5.TabIndex = 33;
			this.groupBox5.TabStop = false;
			this.groupBox5.Text = "登陆后状态";
			this.chkManualCallOut.AutoSize = true;
			this.chkManualCallOut.Location = new Point(8, 52);
			this.chkManualCallOut.Name = "chkManualCallOut";
			this.chkManualCallOut.Size = new Size(84, 16);
			this.chkManualCallOut.TabIndex = 2;
			this.chkManualCallOut.Text = "手动外呼中";
			this.chkManualCallOut.UseVisualStyleBackColor = true;
			this.chkBusy.AutoSize = true;
			this.chkBusy.Location = new Point(140, 19);
			this.chkBusy.Name = "chkBusy";
			this.chkBusy.Size = new Size(48, 16);
			this.chkBusy.TabIndex = 1;
			this.chkBusy.Text = "忙碌";
			this.chkBusy.UseVisualStyleBackColor = true;
			this.chkIdle.AutoSize = true;
			this.chkIdle.Location = new Point(8, 20);
			this.chkIdle.Name = "chkIdle";
			this.chkIdle.Size = new Size(48, 16);
			this.chkIdle.TabIndex = 0;
			this.chkIdle.Text = "空闲";
			this.chkIdle.UseVisualStyleBackColor = true;
			this.lblinfo3.AutoSize = true;
			this.lblinfo3.Location = new Point(10, 291);
			this.lblinfo3.Name = "lblinfo3";
			this.lblinfo3.Size = new Size(197, 12);
			this.lblinfo3.TabIndex = 42;
			this.lblinfo3.Text = "注意：可根据情况只使用部分参数。";
			this.lblinfo3.Visible = false;
			this.tabSetting.Controls.Add(this.tabPage1);
			this.tabSetting.Controls.Add(this.tabPage2);
			this.tabSetting.Location = new Point(-3, 2);
			this.tabSetting.Name = "tabSetting";
			this.tabSetting.SelectedIndex = 0;
			this.tabSetting.Size = new Size(345, 361);
			this.tabSetting.TabIndex = 1;
			this.tabPage2.Controls.Add(this.grp2);
			this.tabPage2.Controls.Add(this.grp1);
			this.tabPage2.Location = new Point(4, 22);
			this.tabPage2.Name = "tabPage2";
			this.tabPage2.Padding = new Padding(3);
			this.tabPage2.Size = new Size(337, 335);
			this.tabPage2.TabIndex = 1;
			this.tabPage2.Text = "软电话配置";
			this.tabPage2.UseVisualStyleBackColor = true;
			this.grp2.Controls.Add(this.numRegistTime);
			this.grp2.Controls.Add(this.chkAutoAnswer);
			this.grp2.Controls.Add(this.label7);
			this.grp2.Controls.Add(this.txtSipLocalNum);
			this.grp2.Controls.Add(this.label8);
			this.grp2.Location = new Point(10, 171);
			this.grp2.Name = "grp2";
			this.grp2.Size = new Size(306, 108);
			this.grp2.TabIndex = 45;
			this.grp2.TabStop = false;
			this.grp2.Text = "基本设置";
			this.numRegistTime.Location = new Point(101, 23);
			NumericUpDown arg_CD5_0 = this.numRegistTime;
			int[] array = new int[4];
			array[0] = 3600;
			arg_CD5_0.Maximum = new decimal(array);
			NumericUpDown arg_CF3_0 = this.numRegistTime;
			array = new int[4];
			array[0] = 30;
			arg_CF3_0.Minimum = new decimal(array);
			this.numRegistTime.Name = "numRegistTime";
			this.numRegistTime.Size = new Size(187, 21);
			this.numRegistTime.TabIndex = 52;
			NumericUpDown arg_D48_0 = this.numRegistTime;
			array = new int[4];
			array[0] = 60;
			arg_D48_0.Value = new decimal(array);
			this.chkAutoAnswer.AutoSize = true;
			this.chkAutoAnswer.Location = new Point(7, 68);
			this.chkAutoAnswer.Name = "chkAutoAnswer";
			this.chkAutoAnswer.Size = new Size(72, 16);
			this.chkAutoAnswer.TabIndex = 47;
			this.chkAutoAnswer.Text = "自动接听";
			this.chkAutoAnswer.UseVisualStyleBackColor = true;
			this.label7.AutoSize = true;
			this.label7.Location = new Point(29, 53);
			this.label7.Name = "label7";
			this.label7.Size = new Size(65, 12);
			this.label7.TabIndex = 44;
			this.label7.Text = "本机号码：";
			this.label7.TextAlign = ContentAlignment.MiddleRight;
			this.label7.Visible = false;
			this.txtSipLocalNum.Location = new Point(101, 50);
			this.txtSipLocalNum.Name = "txtSipLocalNum";
			this.txtSipLocalNum.Size = new Size(187, 21);
			this.txtSipLocalNum.TabIndex = 43;
			this.txtSipLocalNum.Visible = false;
			this.txtSipLocalNum.TextChanged += new EventHandler(this.txtSipLocalNum_TextChanged);
			this.label8.AutoSize = true;
			this.label8.Location = new Point(5, 27);
			this.label8.Name = "label8";
			this.label8.Size = new Size(89, 12);
			this.label8.TabIndex = 42;
			this.label8.Text = "注册间隔(秒)：";
			this.label8.TextAlign = ContentAlignment.MiddleRight;
			this.grp1.Controls.Add(this.txtSipPort);
			this.grp1.Controls.Add(this.label9);
			this.grp1.Controls.Add(this.txtSipServer);
			this.grp1.Controls.Add(this.label10);
			this.grp1.Controls.Add(this.txtSipPwd);
			this.grp1.Controls.Add(this.label11);
			this.grp1.Controls.Add(this.label12);
			this.grp1.Controls.Add(this.txtSipNum);
			this.grp1.Location = new Point(10, 10);
			this.grp1.Name = "grp1";
			this.grp1.Size = new Size(306, 145);
			this.grp1.TabIndex = 44;
			this.grp1.TabStop = false;
			this.grp1.Text = "帐户";
			this.txtSipPort.Location = new Point(101, 111);
			this.txtSipPort.Name = "txtSipPort";
			this.txtSipPort.Size = new Size(187, 21);
			this.txtSipPort.TabIndex = 36;
			this.txtSipPort.KeyPress += new KeyPressEventHandler(this.txtSipPort_KeyPress);
			this.label9.AutoSize = true;
			this.label9.Location = new Point(50, 114);
			this.label9.Name = "label9";
			this.label9.Size = new Size(41, 12);
			this.label9.TabIndex = 35;
			this.label9.Text = "端口：";
			this.label9.TextAlign = ContentAlignment.MiddleRight;
			this.txtSipServer.Location = new Point(101, 82);
			this.txtSipServer.Name = "txtSipServer";
			this.txtSipServer.Size = new Size(187, 21);
			this.txtSipServer.TabIndex = 34;
			this.txtSipServer.KeyPress += new KeyPressEventHandler(this.txtSipServer_KeyPress);
			this.label10.AutoSize = true;
			this.label10.Location = new Point(14, 85);
			this.label10.Name = "label10";
			this.label10.Size = new Size(77, 12);
			this.label10.TabIndex = 33;
			this.label10.Text = "服务器地址：";
			this.label10.TextAlign = ContentAlignment.MiddleRight;
			this.txtSipPwd.Location = new Point(101, 50);
			this.txtSipPwd.Name = "txtSipPwd";
			this.txtSipPwd.PasswordChar = '*';
			this.txtSipPwd.Size = new Size(187, 21);
			this.txtSipPwd.TabIndex = 32;
			this.txtSipPwd.TextChanged += new EventHandler(this.sipPwd_TextChanged);
			this.label11.AutoSize = true;
			this.label11.Location = new Point(38, 53);
			this.label11.Name = "label11";
			this.label11.Size = new Size(53, 12);
			this.label11.TabIndex = 31;
			this.label11.Text = "密  码：";
			this.label11.TextAlign = ContentAlignment.MiddleRight;
			this.label12.AutoSize = true;
			this.label12.Location = new Point(38, 21);
			this.label12.Name = "label12";
			this.label12.Size = new Size(53, 12);
			this.label12.TabIndex = 30;
			this.label12.Text = "帐  号：";
			this.label12.TextAlign = ContentAlignment.MiddleRight;
			this.txtSipNum.Location = new Point(101, 18);
			this.txtSipNum.Name = "txtSipNum";
			this.txtSipNum.Size = new Size(187, 21);
			this.txtSipNum.TabIndex = 29;
			this.txtSipNum.TextChanged += new EventHandler(this.txtSipNum_TextChanged);
			this.txtSipNum.KeyPress += new KeyPressEventHandler(this.txtSipNum_KeyPress);
			this.button2.Location = new Point(231, 340);
			this.button2.Name = "button2";
			this.button2.Size = new Size(80, 27);
			this.button2.TabIndex = 43;
			this.button2.Text = "确定";
			this.button2.UseVisualStyleBackColor = true;
			this.button3.Location = new Point(145, 340);
			this.button3.Name = "button3";
			this.button3.Size = new Size(80, 27);
			this.button3.TabIndex = 43;
			this.button3.Text = "确定";
			this.button3.UseVisualStyleBackColor = true;
			this.checkBox1.AutoSize = true;
			this.checkBox1.Location = new Point(24, 293);
			this.checkBox1.Name = "checkBox1";
			this.checkBox1.Size = new Size(72, 16);
			this.checkBox1.TabIndex = 42;
			this.checkBox1.Text = "自动接听";
			this.checkBox1.UseVisualStyleBackColor = true;
			this.groupBox3.Controls.Add(this.label1);
			this.groupBox3.Controls.Add(this.textBox1);
			this.groupBox3.Controls.Add(this.label2);
			this.groupBox3.Controls.Add(this.textBox2);
			this.groupBox3.Location = new Point(6, 167);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Size = new Size(306, 92);
			this.groupBox3.TabIndex = 41;
			this.groupBox3.TabStop = false;
			this.groupBox3.Text = "基本设置";
			this.label1.AutoSize = true;
			this.label1.Location = new Point(16, 50);
			this.label1.Name = "label1";
			this.label1.Size = new Size(65, 12);
			this.label1.TabIndex = 44;
			this.label1.Text = "本机号码：";
			this.label1.TextAlign = ContentAlignment.MiddleRight;
			this.textBox1.Location = new Point(101, 50);
			this.textBox1.Name = "textBox1";
			this.textBox1.Size = new Size(187, 21);
			this.textBox1.TabIndex = 43;
			this.label2.AutoSize = true;
			this.label2.Location = new Point(16, 26);
			this.label2.Name = "label2";
			this.label2.Size = new Size(65, 12);
			this.label2.TabIndex = 42;
			this.label2.Text = "最大超时：";
			this.label2.TextAlign = ContentAlignment.MiddleRight;
			this.textBox2.Location = new Point(101, 23);
			this.textBox2.Name = "textBox2";
			this.textBox2.Size = new Size(187, 21);
			this.textBox2.TabIndex = 41;
			this.groupBox4.Controls.Add(this.textBox3);
			this.groupBox4.Controls.Add(this.label3);
			this.groupBox4.Controls.Add(this.textBox4);
			this.groupBox4.Controls.Add(this.label4);
			this.groupBox4.Controls.Add(this.textBox5);
			this.groupBox4.Controls.Add(this.label5);
			this.groupBox4.Controls.Add(this.label6);
			this.groupBox4.Controls.Add(this.textBox6);
			this.groupBox4.Location = new Point(6, 6);
			this.groupBox4.Name = "groupBox4";
			this.groupBox4.Size = new Size(306, 145);
			this.groupBox4.TabIndex = 31;
			this.groupBox4.TabStop = false;
			this.groupBox4.Text = "帐户";
			this.textBox3.Location = new Point(101, 111);
			this.textBox3.Name = "textBox3";
			this.textBox3.Size = new Size(187, 21);
			this.textBox3.TabIndex = 36;
			this.label3.AutoSize = true;
			this.label3.Location = new Point(28, 114);
			this.label3.Name = "label3";
			this.label3.Size = new Size(53, 12);
			this.label3.TabIndex = 35;
			this.label3.Text = "端口号：";
			this.label3.TextAlign = ContentAlignment.MiddleRight;
			this.textBox4.Location = new Point(101, 82);
			this.textBox4.Name = "textBox4";
			this.textBox4.Size = new Size(187, 21);
			this.textBox4.TabIndex = 34;
			this.label4.AutoSize = true;
			this.label4.Location = new Point(28, 85);
			this.label4.Name = "label4";
			this.label4.Size = new Size(53, 12);
			this.label4.TabIndex = 33;
			this.label4.Text = "服务器：";
			this.label4.TextAlign = ContentAlignment.MiddleRight;
			this.textBox5.Location = new Point(101, 50);
			this.textBox5.Name = "textBox5";
			this.textBox5.PasswordChar = '*';
			this.textBox5.Size = new Size(187, 21);
			this.textBox5.TabIndex = 32;
			this.label5.AutoSize = true;
			this.label5.Location = new Point(28, 53);
			this.label5.Name = "label5";
			this.label5.Size = new Size(53, 12);
			this.label5.TabIndex = 31;
			this.label5.Text = "密  码：";
			this.label5.TextAlign = ContentAlignment.MiddleRight;
			this.label6.AutoSize = true;
			this.label6.Location = new Point(28, 21);
			this.label6.Name = "label6";
			this.label6.Size = new Size(53, 12);
			this.label6.TabIndex = 30;
			this.label6.Text = "帐  号：";
			this.label6.TextAlign = ContentAlignment.MiddleRight;
			this.textBox6.Location = new Point(101, 18);
			this.textBox6.Name = "textBox6";
			this.textBox6.Size = new Size(187, 21);
			this.textBox6.TabIndex = 29;
			this.btnCancel.DialogResult = DialogResult.Cancel;
			this.btnCancel.Location = new Point(249, 369);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new Size(80, 27);
			this.btnCancel.TabIndex = 49;
			this.btnCancel.Text = "取消";
			this.btnCancel.UseVisualStyleBackColor = true;
			this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
			this.btnOk.Location = new Point(147, 369);
			this.btnOk.Name = "btnOk";
			this.btnOk.Size = new Size(80, 27);
			this.btnOk.TabIndex = 50;
			this.btnOk.Text = "确定";
			this.btnOk.UseVisualStyleBackColor = true;
			this.btnOk.Click += new EventHandler(this.btnOk_Click);
			base.AcceptButton = this.btnOk;
			base.AutoScaleDimensions = new SizeF(6f, 12f);
			base.AutoScaleMode = AutoScaleMode.Font;
			base.CancelButton = this.btnCancel;
			base.ClientSize = new Size(338, 405);
			base.Controls.Add(this.btnCancel);
			base.Controls.Add(this.btnOk);
			base.Controls.Add(this.tabSetting);
			base.FormBorderStyle = FormBorderStyle.FixedDialog;
			base.Icon = (Icon)resources.GetObject("$this.Icon");
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "FrmConfig";
			this.Text = "系统设置";
			base.Load += new EventHandler(this.FrmConfig_Load);
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			this.tabPage1.ResumeLayout(false);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.groupBox5.ResumeLayout(false);
			this.groupBox5.PerformLayout();
			this.tabSetting.ResumeLayout(false);
			this.tabPage2.ResumeLayout(false);
			this.grp2.ResumeLayout(false);
			this.grp2.PerformLayout();
			((ISupportInitialize)this.numRegistTime).EndInit();
			this.grp1.ResumeLayout(false);
			this.grp1.PerformLayout();
			this.groupBox3.ResumeLayout(false);
			this.groupBox3.PerformLayout();
			this.groupBox4.ResumeLayout(false);
			this.groupBox4.PerformLayout();
			base.ResumeLayout(false);
		}
	}
}
