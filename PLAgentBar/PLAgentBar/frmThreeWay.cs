using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace PLAgentBar
{
	public class frmThreeWay : Form
	{
		private IContainer components = null;

		private Button btnOk;

		private Button btnCacncel;

		public ComboBox cboAgent;

		public frmThreeWay()
		{
			this.InitializeComponent();
		}

		private void btnOk_Click(object sender, EventArgs e)
		{
			if (this.cboAgent.Text == "")
			{
				MessageBox.Show("请选择一个坐席工号！", "三方通话失败", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
				base.DialogResult = DialogResult.No;
			}
			else
			{
				base.DialogResult = DialogResult.OK;
			}
		}

		private void frmCallOut_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (base.DialogResult == DialogResult.No)
			{
				e.Cancel = true;
			}
		}

		private void button1_Click(object sender, EventArgs e)
		{
			base.Close();
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
			this.btnOk = new Button();
			this.btnCacncel = new Button();
			this.cboAgent = new ComboBox();
			base.SuspendLayout();
			this.btnOk.DialogResult = DialogResult.Cancel;
			this.btnOk.Location = new Point(100, 169);
			this.btnOk.Name = "btnOk";
			this.btnOk.Size = new Size(105, 33);
			this.btnOk.TabIndex = 7;
			this.btnOk.Text = "确定";
			this.btnOk.UseVisualStyleBackColor = true;
			this.btnOk.Click += new EventHandler(this.btnOk_Click);
			this.btnCacncel.DialogResult = DialogResult.Cancel;
			this.btnCacncel.Location = new Point(279, 179);
			this.btnCacncel.Name = "btnCacncel";
			this.btnCacncel.Size = new Size(75, 23);
			this.btnCacncel.TabIndex = 10;
			this.btnCacncel.Text = "取消";
			this.btnCacncel.UseVisualStyleBackColor = true;
			this.btnCacncel.Visible = false;
			this.btnCacncel.Click += new EventHandler(this.button1_Click);
			this.cboAgent.FormattingEnabled = true;
			this.cboAgent.Location = new Point(50, 58);
			this.cboAgent.Name = "cboAgent";
			this.cboAgent.Size = new Size(222, 20);
			this.cboAgent.TabIndex = 11;
			base.AcceptButton = this.btnOk;
			base.AutoScaleDimensions = new SizeF(6f, 12f);
			base.AutoScaleMode = AutoScaleMode.Font;
			base.CancelButton = this.btnCacncel;
			base.ClientSize = new Size(320, 153);
			base.Controls.Add(this.cboAgent);
			base.Controls.Add(this.btnCacncel);
			base.Controls.Add(this.btnOk);
			base.FormBorderStyle = FormBorderStyle.FixedSingle;
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "frmThreeWay";
			base.StartPosition = FormStartPosition.CenterParent;
			this.Text = "请选择一个坐席";
			base.FormClosing += new FormClosingEventHandler(this.frmCallOut_FormClosing);
			base.ResumeLayout(false);
		}
	}
}
