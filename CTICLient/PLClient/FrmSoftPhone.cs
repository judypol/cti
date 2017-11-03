using PLSoftPhone;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace PLClient
{
	public class FrmSoftPhone : Form
	{
		public const int WM_SYSCOMMAND = 274;

		public const int SC_MOVE = 61456;

		public const int HTCAPTION = 2;

		private IContainer components = null;

		private PLSoftPhone plSoftPhone1;

		private Button btnClose;

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
			this.plSoftPhone1 = new PLSoftPhone();
			this.btnClose = new Button();
			base.SuspendLayout();
			this.plSoftPhone1.BackColor = Color.Transparent;
			this.plSoftPhone1.Location = new Point(7, 4);
			this.plSoftPhone1.Name = "plSoftPhone1";
			this.plSoftPhone1.Size = new Size(224, 392);
			this.plSoftPhone1.TabIndex = 4;
			this.btnClose.DialogResult = DialogResult.Cancel;
			this.btnClose.Location = new Point(381, 174);
			this.btnClose.Name = "btnClose";
			this.btnClose.Size = new Size(75, 23);
			this.btnClose.TabIndex = 5;
			this.btnClose.Text = "关闭";
			this.btnClose.UseVisualStyleBackColor = true;
			this.btnClose.Click += new EventHandler(this.btnClose_Click);
			base.AutoScaleDimensions = new SizeF(6f, 12f);
			base.AutoScaleMode = AutoScaleMode.Font;
			this.BackColor = Color.Gainsboro;
			base.CancelButton = this.btnClose;
			base.ClientSize = new Size(261, 417);
			base.Controls.Add(this.btnClose);
			base.Controls.Add(this.plSoftPhone1);
			base.FormBorderStyle = FormBorderStyle.None;
			base.KeyPreview = true;
			base.Name = "FrmSoftPhone";
			base.StartPosition = FormStartPosition.CenterScreen;
			this.Text = "FrmSoftPhone";
			base.TransparencyKey = Color.Gainsboro;
			base.Load += new EventHandler(this.FrmSoftPhone_Load);
			base.KeyDown += new KeyEventHandler(this.FrmSoftPhone_KeyDown);
			base.ResumeLayout(false);
		}

		[DllImport("user32")]
		private static extern bool ReleaseCapture();

		[DllImport("user32")]
		private static extern bool SendMessage(IntPtr hwnd, int wMsg, int wParam, int lParam);

		public FrmSoftPhone()
		{
			this.InitializeComponent();
		}

		private void FrmSoftPhone_Load(object sender, EventArgs e)
		{
			try
			{
				if (!this.plSoftPhone1.start())
				{
					MessageBox.Show("软电话启动失败！", "启动", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

		private void plSoftPhone1_MouseDown(object sender, MouseEventArgs e)
		{
			FrmSoftPhone.ReleaseCapture();
			FrmSoftPhone.SendMessage(base.Handle, 274, 61458, 0);
		}

		private void FrmSoftPhone_KeyDown(object sender, KeyEventArgs e)
		{
		}

		private void closeForm()
		{
			this.plSoftPhone1.SysClose();
		}

		private void btnClose_Click(object sender, EventArgs e)
		{
			this.closeForm();
		}
	}
}
