using PLAgentBar.Properties;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace PLAgentBar
{
	public class FrmKeyPad : Form
	{
		public const int WM_SYSCOMMAND = 274;

		public const int SC_MOVE = 61456;

		public const int HTCAPTION = 2;

		public AgentBar agentBar1;

		private string strTel;

		private IContainer components = null;

		private Panel panel1;

		private Label lblLCD;

		private PictureBox pic1;

		private PictureBox pic2;

		private PictureBox pic11;

		private PictureBox pic0;

		private PictureBox pic9;

		private PictureBox pic8;

		private PictureBox pic6;

		private PictureBox pic10;

		private PictureBox pic5;

		private PictureBox pic7;

		private PictureBox pic3;

		private PictureBox pic4;

		private Button btnClose;

		private PictureBox picClose;

		[DllImport("user32")]
		private static extern bool ReleaseCapture();

		[DllImport("user32")]
		private static extern bool SendMessage(IntPtr hwnd, int wMsg, int wParam, int lParam);

		public FrmKeyPad()
		{
			this.InitializeComponent();
		}

		private void btnClose_Click(object sender, EventArgs e)
		{
			base.Close();
		}

		private void pictClose_Click(object sender, EventArgs e)
		{
			base.Close();
		}

		private void FrmKeyPad_Load(object sender, EventArgs e)
		{
			this.picClose.BackColor = Color.Transparent;
		}

		private void pic_Click(object sender, EventArgs e)
		{
			string dtmfValue = (sender as PictureBox).AccessibleName;
			this.ShowFromLCD(dtmfValue);
			string text = dtmfValue;
			int dtmfMsg;
			switch (text)
			{
			case "0":
				dtmfMsg = this.agentBar1.SoftPhone_Dtmf_0_Cmd;
				goto IL_1CE;
			case "1":
				dtmfMsg = this.agentBar1.SoftPhone_dtmf_1_cmd;
				goto IL_1CE;
			case "2":
				dtmfMsg = this.agentBar1.SoftPhone_dtmf_2_cmd;
				goto IL_1CE;
			case "3":
				dtmfMsg = this.agentBar1.SoftPhone_dtmf_3_cmd;
				goto IL_1CE;
			case "4":
				dtmfMsg = this.agentBar1.SoftPhone_dtmf_4_cmd;
				goto IL_1CE;
			case "5":
				dtmfMsg = this.agentBar1.SoftPhone_dtmf_5_cmd;
				goto IL_1CE;
			case "6":
				dtmfMsg = this.agentBar1.SoftPhone_dtmf_6_cmd;
				goto IL_1CE;
			case "7":
				dtmfMsg = this.agentBar1.SoftPhone_dtmf_7_cmd;
				goto IL_1CE;
			case "8":
				dtmfMsg = this.agentBar1.SoftPhone_dtmf_8_cmd;
				goto IL_1CE;
			case "9":
				dtmfMsg = this.agentBar1.SoftPhone_dtmf_9_cmd;
				goto IL_1CE;
			case "*":
				dtmfMsg = this.agentBar1.SoftPhone_dtmf_star_cmd;
				goto IL_1CE;
			case "#":
				dtmfMsg = this.agentBar1.SoftPhone_dtmf_pound_cmd;
				goto IL_1CE;
			}
			dtmfMsg = -1;
			IL_1CE:
			if (dtmfMsg != -1)
			{
				this.sendDtmf(dtmfMsg);
			}
			else
			{
				MessageBox.Show("按键不存在！", "按键失败", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			}
		}

		private void ShowFromLCD(string strKey)
		{
			this.strTel += strKey;
			Label expr_19 = this.lblLCD;
			expr_19.Text += strKey;
			if (this.lblLCD.Text.Length > 13)
			{
				this.lblLCD.Text = this.lblLCD.Text.Substring(this.lblLCD.Text.Length - 13, this.lblLCD.Text.Length - 1);
			}
			else
			{
				this.lblLCD.Text = this.lblLCD.Text.Substring(0, this.lblLCD.Text.Length);
			}
		}

		private void sendDtmf(int dtmfValue)
		{
			if (this.agentBar1 != null)
			{
				int rt = AgentBar.PostMsgToSoftPhone(this.agentBar1.SoftPhoneWindowHandle, this.agentBar1.SoftPhoneAppClassName, this.agentBar1.SoftPhoneAppName, this.agentBar1.SoftPhoneMsgValue, dtmfValue, dtmfValue);
				if (rt != 0)
				{
					MessageBox.Show("按键发送失败！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
				}
			}
		}

		private void MyBackSpace()
		{
			if (this.strTel.Length > 0)
			{
				this.strTel = this.strTel.Substring(0, this.strTel.Length - 1);
				if (this.strTel.Length > 13)
				{
					this.lblLCD.Text = this.strTel.Substring(this.strTel.Length - 13, 13);
				}
				else
				{
					this.lblLCD.Text = this.strTel;
				}
			}
		}

		private void lblLCD_MouseDown(object sender, MouseEventArgs e)
		{
			FrmKeyPad.ReleaseCapture();
			FrmKeyPad.SendMessage(base.Handle, 274, 61458, 0);
		}

		private void panel1_MouseDown(object sender, MouseEventArgs e)
		{
			FrmKeyPad.ReleaseCapture();
			FrmKeyPad.SendMessage(base.Handle, 274, 61458, 0);
		}

		private void FrmKeyPad_KeyDown(object sender, KeyEventArgs e)
		{
			string strKey = this.PressBtnChar(e.KeyValue, e.Shift);
			if (strKey.Length == 1)
			{
				this.ShowFromLCD(strKey);
			}
			else if (strKey == "backspace")
			{
				this.MyBackSpace();
			}
		}

		private string PressBtnChar(int intValue, bool blnShift)
		{
			string strKey = "";
			if (intValue != 8)
			{
				switch (intValue)
				{
				case 48:
					break;
				case 49:
					goto IL_86;
				case 50:
					goto IL_91;
				case 51:
					if (blnShift)
					{
						strKey = "#";
					}
					else
					{
						strKey = "3";
					}
					return strKey;
				case 52:
					goto IL_C0;
				case 53:
					goto IL_C8;
				case 54:
					goto IL_D0;
				case 55:
					goto IL_D8;
				case 56:
					if (blnShift)
					{
						this.ShowFromLCD("*");
						strKey = "*";
					}
					else
					{
						this.ShowFromLCD("8");
						strKey = "8";
					}
					return strKey;
				case 57:
					goto IL_11C;
				default:
					switch (intValue)
					{
					case 96:
						break;
					case 97:
						goto IL_86;
					case 98:
						goto IL_91;
					case 99:
						strKey = "3";
						return strKey;
					case 100:
						goto IL_C0;
					case 101:
						goto IL_C8;
					case 102:
						goto IL_D0;
					case 103:
						goto IL_D8;
					case 104:
						strKey = "8";
						return strKey;
					case 105:
						goto IL_11C;
					case 106:
						strKey = "*";
						return strKey;
					default:
						return strKey;
					}
					break;
				}
				strKey = "0";
				return strKey;
				IL_86:
				strKey = "1";
				return strKey;
				IL_91:
				strKey = "2";
				return strKey;
				IL_C0:
				strKey = "4";
				return strKey;
				IL_C8:
				strKey = "5";
				return strKey;
				IL_D0:
				strKey = "6";
				return strKey;
				IL_D8:
				strKey = "7";
				return strKey;
				IL_11C:
				strKey = "9";
			}
			else
			{
				strKey = "backspace";
			}
			return strKey;
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
			ComponentResourceManager resources = new ComponentResourceManager(typeof(FrmKeyPad));
			this.btnClose = new Button();
			this.panel1 = new Panel();
			this.picClose = new PictureBox();
			this.lblLCD = new Label();
			this.pic1 = new PictureBox();
			this.pic2 = new PictureBox();
			this.pic11 = new PictureBox();
			this.pic0 = new PictureBox();
			this.pic9 = new PictureBox();
			this.pic8 = new PictureBox();
			this.pic6 = new PictureBox();
			this.pic10 = new PictureBox();
			this.pic5 = new PictureBox();
			this.pic7 = new PictureBox();
			this.pic3 = new PictureBox();
			this.pic4 = new PictureBox();
			this.panel1.SuspendLayout();
			((ISupportInitialize)this.picClose).BeginInit();
			((ISupportInitialize)this.pic1).BeginInit();
			((ISupportInitialize)this.pic2).BeginInit();
			((ISupportInitialize)this.pic11).BeginInit();
			((ISupportInitialize)this.pic0).BeginInit();
			((ISupportInitialize)this.pic9).BeginInit();
			((ISupportInitialize)this.pic8).BeginInit();
			((ISupportInitialize)this.pic6).BeginInit();
			((ISupportInitialize)this.pic10).BeginInit();
			((ISupportInitialize)this.pic5).BeginInit();
			((ISupportInitialize)this.pic7).BeginInit();
			((ISupportInitialize)this.pic3).BeginInit();
			((ISupportInitialize)this.pic4).BeginInit();
			base.SuspendLayout();
			this.btnClose.DialogResult = DialogResult.Cancel;
			this.btnClose.Location = new Point(65, 254);
			this.btnClose.Name = "btnClose";
			this.btnClose.Size = new Size(75, 23);
			this.btnClose.TabIndex = 4;
			this.btnClose.Text = "关闭";
			this.btnClose.UseVisualStyleBackColor = true;
			this.btnClose.Click += new EventHandler(this.btnClose_Click);
			this.panel1.BackgroundImage = (Image)resources.GetObject("panel1.BackgroundImage");
			this.panel1.BackgroundImageLayout = ImageLayout.Stretch;
			this.panel1.Controls.Add(this.picClose);
			this.panel1.Controls.Add(this.lblLCD);
			this.panel1.Controls.Add(this.pic1);
			this.panel1.Controls.Add(this.pic2);
			this.panel1.Controls.Add(this.pic11);
			this.panel1.Controls.Add(this.pic0);
			this.panel1.Controls.Add(this.pic9);
			this.panel1.Controls.Add(this.pic8);
			this.panel1.Controls.Add(this.pic6);
			this.panel1.Controls.Add(this.pic10);
			this.panel1.Controls.Add(this.pic5);
			this.panel1.Controls.Add(this.pic7);
			this.panel1.Controls.Add(this.pic3);
			this.panel1.Controls.Add(this.pic4);
			this.panel1.Location = new Point(2, 2);
			this.panel1.Name = "panel1";
			this.panel1.Size = new Size(189, 220);
			this.panel1.TabIndex = 3;
			this.panel1.MouseDown += new MouseEventHandler(this.panel1_MouseDown);
			this.picClose.Location = new Point(168, 0);
			this.picClose.Name = "picClose";
			this.picClose.Size = new Size(21, 19);
			this.picClose.TabIndex = 2;
			this.picClose.TabStop = false;
			this.picClose.Click += new EventHandler(this.pictClose_Click);
			this.lblLCD.Anchor = AnchorStyles.Left;
			this.lblLCD.BackColor = Color.Transparent;
			this.lblLCD.Font = new Font("Arial Unicode MS", 15.75f, FontStyle.Regular, GraphicsUnit.Point, 134);
			this.lblLCD.ForeColor = Color.Snow;
			this.lblLCD.Location = new Point(3, 1);
			this.lblLCD.Name = "lblLCD";
			this.lblLCD.Size = new Size(180, 38);
			this.lblLCD.TabIndex = 0;
			this.lblLCD.TextAlign = ContentAlignment.MiddleCenter;
			this.lblLCD.MouseDown += new MouseEventHandler(this.lblLCD_MouseDown);
			this.pic1.AccessibleName = "1";
			this.pic1.Anchor = AnchorStyles.Left;
			this.pic1.BackgroundImageLayout = ImageLayout.Stretch;
			this.pic1.Image = Resources._1_00x00;
			this.pic1.Location = new Point(2, 42);
			this.pic1.Name = "pic1";
			this.pic1.Size = new Size(61, 40);
			this.pic1.SizeMode = PictureBoxSizeMode.StretchImage;
			this.pic1.TabIndex = 1;
			this.pic1.TabStop = false;
			this.pic1.Click += new EventHandler(this.pic_Click);
			this.pic2.AccessibleName = "2";
			this.pic2.Anchor = AnchorStyles.Left;
			this.pic2.BackgroundImageLayout = ImageLayout.Stretch;
			this.pic2.Image = Resources._1_00x01;
			this.pic2.Location = new Point(63, 42);
			this.pic2.Name = "pic2";
			this.pic2.Size = new Size(61, 40);
			this.pic2.SizeMode = PictureBoxSizeMode.StretchImage;
			this.pic2.TabIndex = 1;
			this.pic2.TabStop = false;
			this.pic2.Click += new EventHandler(this.pic_Click);
			this.pic11.AccessibleName = "#";
			this.pic11.Anchor = AnchorStyles.Left;
			this.pic11.BackgroundImageLayout = ImageLayout.Stretch;
			this.pic11.Image = Resources._1_03x02;
			this.pic11.Location = new Point(124, 162);
			this.pic11.Name = "pic11";
			this.pic11.Size = new Size(61, 40);
			this.pic11.SizeMode = PictureBoxSizeMode.StretchImage;
			this.pic11.TabIndex = 1;
			this.pic11.TabStop = false;
			this.pic11.Click += new EventHandler(this.pic_Click);
			this.pic0.AccessibleName = "0";
			this.pic0.Anchor = AnchorStyles.Left;
			this.pic0.BackgroundImageLayout = ImageLayout.Stretch;
			this.pic0.Image = Resources._1_03x01;
			this.pic0.Location = new Point(63, 162);
			this.pic0.Name = "pic0";
			this.pic0.Size = new Size(61, 40);
			this.pic0.SizeMode = PictureBoxSizeMode.StretchImage;
			this.pic0.TabIndex = 1;
			this.pic0.TabStop = false;
			this.pic0.Click += new EventHandler(this.pic_Click);
			this.pic9.AccessibleName = "9";
			this.pic9.Anchor = AnchorStyles.Left;
			this.pic9.BackgroundImageLayout = ImageLayout.Stretch;
			this.pic9.Image = Resources._1_02x02;
			this.pic9.Location = new Point(124, 122);
			this.pic9.Name = "pic9";
			this.pic9.Size = new Size(61, 40);
			this.pic9.SizeMode = PictureBoxSizeMode.StretchImage;
			this.pic9.TabIndex = 1;
			this.pic9.TabStop = false;
			this.pic9.Click += new EventHandler(this.pic_Click);
			this.pic8.AccessibleName = "8";
			this.pic8.Anchor = AnchorStyles.Left;
			this.pic8.BackgroundImageLayout = ImageLayout.Stretch;
			this.pic8.Image = Resources._1_02x01;
			this.pic8.Location = new Point(63, 122);
			this.pic8.Name = "pic8";
			this.pic8.Size = new Size(61, 40);
			this.pic8.SizeMode = PictureBoxSizeMode.StretchImage;
			this.pic8.TabIndex = 1;
			this.pic8.TabStop = false;
			this.pic8.Click += new EventHandler(this.pic_Click);
			this.pic6.AccessibleName = "6";
			this.pic6.Anchor = AnchorStyles.Left;
			this.pic6.BackgroundImageLayout = ImageLayout.Stretch;
			this.pic6.Image = Resources._1_01x02;
			this.pic6.Location = new Point(124, 82);
			this.pic6.Name = "pic6";
			this.pic6.Size = new Size(61, 40);
			this.pic6.SizeMode = PictureBoxSizeMode.StretchImage;
			this.pic6.TabIndex = 1;
			this.pic6.TabStop = false;
			this.pic6.Click += new EventHandler(this.pic_Click);
			this.pic10.AccessibleName = "*";
			this.pic10.Anchor = AnchorStyles.Left;
			this.pic10.BackgroundImageLayout = ImageLayout.Stretch;
			this.pic10.Image = Resources._1_03x00;
			this.pic10.Location = new Point(2, 162);
			this.pic10.Name = "pic10";
			this.pic10.Size = new Size(61, 40);
			this.pic10.SizeMode = PictureBoxSizeMode.StretchImage;
			this.pic10.TabIndex = 1;
			this.pic10.TabStop = false;
			this.pic10.Click += new EventHandler(this.pic_Click);
			this.pic5.AccessibleName = "5";
			this.pic5.Anchor = AnchorStyles.Left;
			this.pic5.BackgroundImageLayout = ImageLayout.Stretch;
			this.pic5.Image = Resources._1_01x01;
			this.pic5.Location = new Point(63, 82);
			this.pic5.Name = "pic5";
			this.pic5.Size = new Size(61, 40);
			this.pic5.SizeMode = PictureBoxSizeMode.StretchImage;
			this.pic5.TabIndex = 1;
			this.pic5.TabStop = false;
			this.pic5.Click += new EventHandler(this.pic_Click);
			this.pic7.AccessibleName = "7";
			this.pic7.Anchor = AnchorStyles.Left;
			this.pic7.BackgroundImageLayout = ImageLayout.Stretch;
			this.pic7.Image = Resources._1_02x00;
			this.pic7.Location = new Point(2, 122);
			this.pic7.Name = "pic7";
			this.pic7.Size = new Size(61, 40);
			this.pic7.SizeMode = PictureBoxSizeMode.StretchImage;
			this.pic7.TabIndex = 1;
			this.pic7.TabStop = false;
			this.pic7.Click += new EventHandler(this.pic_Click);
			this.pic3.AccessibleName = "3";
			this.pic3.Anchor = AnchorStyles.Left;
			this.pic3.BackgroundImageLayout = ImageLayout.Stretch;
			this.pic3.Image = Resources._1_00x02;
			this.pic3.Location = new Point(124, 42);
			this.pic3.Name = "pic3";
			this.pic3.Size = new Size(61, 40);
			this.pic3.SizeMode = PictureBoxSizeMode.StretchImage;
			this.pic3.TabIndex = 1;
			this.pic3.TabStop = false;
			this.pic3.Click += new EventHandler(this.pic_Click);
			this.pic4.AccessibleName = "4";
			this.pic4.Anchor = AnchorStyles.Left;
			this.pic4.BackgroundImageLayout = ImageLayout.Stretch;
			this.pic4.Image = Resources._1_01x00;
			this.pic4.Location = new Point(2, 82);
			this.pic4.Name = "pic4";
			this.pic4.Size = new Size(61, 40);
			this.pic4.SizeMode = PictureBoxSizeMode.StretchImage;
			this.pic4.TabIndex = 1;
			this.pic4.TabStop = false;
			this.pic4.Click += new EventHandler(this.pic_Click);
			base.AutoScaleDimensions = new SizeF(6f, 12f);
			base.AutoScaleMode = AutoScaleMode.Font;
			base.CancelButton = this.btnClose;
			base.ClientSize = new Size(193, 224);
			base.Controls.Add(this.btnClose);
			base.Controls.Add(this.panel1);
			base.FormBorderStyle = FormBorderStyle.None;
			base.KeyPreview = true;
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "FrmKeyPad";
			base.StartPosition = FormStartPosition.CenterScreen;
			this.Text = "拨号盘";
			base.Load += new EventHandler(this.FrmKeyPad_Load);
			base.KeyDown += new KeyEventHandler(this.FrmKeyPad_KeyDown);
			this.panel1.ResumeLayout(false);
			((ISupportInitialize)this.picClose).EndInit();
			((ISupportInitialize)this.pic1).EndInit();
			((ISupportInitialize)this.pic2).EndInit();
			((ISupportInitialize)this.pic11).EndInit();
			((ISupportInitialize)this.pic0).EndInit();
			((ISupportInitialize)this.pic9).EndInit();
			((ISupportInitialize)this.pic8).EndInit();
			((ISupportInitialize)this.pic6).EndInit();
			((ISupportInitialize)this.pic10).EndInit();
			((ISupportInitialize)this.pic5).EndInit();
			((ISupportInitialize)this.pic7).EndInit();
			((ISupportInitialize)this.pic3).EndInit();
			((ISupportInitialize)this.pic4).EndInit();
			base.ResumeLayout(false);
		}
	}
}
