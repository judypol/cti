using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace PLAgentBar
{
	public class FrmMessageBox : Form
	{
		private IContainer components = null;

		private Button btnOk;

		private Button btnCancel;

		public Label lblMsg;

		private PictureBox pictureBox1;

		public AgentBar agentbar1;

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
			ComponentResourceManager resources = new ComponentResourceManager(typeof(FrmMessageBox));
			this.lblMsg = new Label();
			this.btnOk = new Button();
			this.btnCancel = new Button();
			this.pictureBox1 = new PictureBox();
			((ISupportInitialize)this.pictureBox1).BeginInit();
			base.SuspendLayout();
			this.lblMsg.BackColor = SystemColors.Window;
			this.lblMsg.Font = new Font("宋体", 9f, FontStyle.Regular, GraphicsUnit.Point, 134);
			this.lblMsg.Location = new Point(3, 0);
			this.lblMsg.Name = "lblMsg";
			this.lblMsg.Size = new Size(314, 96);
			this.lblMsg.TabIndex = 0;
			this.lblMsg.Text = "label1";
			this.lblMsg.TextAlign = ContentAlignment.MiddleCenter;
			this.btnOk.Location = new Point(135, 109);
			this.btnOk.Name = "btnOk";
			this.btnOk.Size = new Size(80, 28);
			this.btnOk.TabIndex = 1;
			this.btnOk.Text = "是";
			this.btnOk.UseVisualStyleBackColor = true;
			this.btnOk.Click += new EventHandler(this.btnOk_Click);
			this.btnCancel.Location = new Point(221, 109);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new Size(80, 28);
			this.btnCancel.TabIndex = 2;
			this.btnCancel.Text = "否";
			this.btnCancel.UseVisualStyleBackColor = true;
			this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
			this.pictureBox1.BackColor = SystemColors.Window;
			this.pictureBox1.Image = (Image)resources.GetObject("pictureBox1.Image");
			this.pictureBox1.Location = new Point(9, 35);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new Size(32, 33);
			this.pictureBox1.TabIndex = 3;
			this.pictureBox1.TabStop = false;
			base.AutoScaleDimensions = new SizeF(6f, 12f);
			base.AutoScaleMode = AutoScaleMode.Font;
			base.ClientSize = new Size(317, 145);
			base.Controls.Add(this.pictureBox1);
			base.Controls.Add(this.btnCancel);
			base.Controls.Add(this.btnOk);
			base.Controls.Add(this.lblMsg);
			this.ForeColor = SystemColors.InfoText;
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "FrmMessageBox";
			base.StartPosition = FormStartPosition.CenterScreen;
			this.Text = "FrmMessageBox";
			base.TopMost = true;
			((ISupportInitialize)this.pictureBox1).EndInit();
			base.ResumeLayout(false);
		}

		public FrmMessageBox()
		{
			this.InitializeComponent();
		}

		private void btnCancel_Click(object sender, EventArgs e)
		{
			base.Close();
		}

		private void btnOk_Click(object sender, EventArgs e)
		{
			if (this.agentbar1 == null)
			{
				MessageBox.Show("发起申请失败！");
				base.Close();
			}
			else
			{
				int strStatusOfLeave = 7;
				if (!this.agentbar1.DoApplyChangeStatus(strStatusOfLeave.ToString()))
				{
					MessageBox.Show("发起申请失败！");
				}
				base.Close();
			}
		}
	}
}
