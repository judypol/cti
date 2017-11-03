using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace PLAgentBar
{
	public class FrmTransferIvrProfile : Form
	{
		private IContainer components = null;

		public TextBox txtIvrProfileNum;

		private Button btnOK;

		private ListView lvwIvrProfile;

		private ImageList imgLstMonitor;

		private Button btnCancel;

		private Dictionary<string, string> mIvrProfileList;

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
			this.components = new Container();
			ComponentResourceManager resources = new ComponentResourceManager(typeof(FrmTransferIvrProfile));
			this.txtIvrProfileNum = new TextBox();
			this.btnOK = new Button();
			this.lvwIvrProfile = new ListView();
			this.imgLstMonitor = new ImageList(this.components);
			this.btnCancel = new Button();
			base.SuspendLayout();
			this.txtIvrProfileNum.Location = new Point(16, 12);
			this.txtIvrProfileNum.Name = "txtIvrProfileNum";
			this.txtIvrProfileNum.Size = new Size(206, 21);
			this.txtIvrProfileNum.TabIndex = 8;
			this.txtIvrProfileNum.TextChanged += new EventHandler(this.txtIvrProfileNum_TextChanged);
			this.txtIvrProfileNum.KeyPress += new KeyPressEventHandler(this.txtIvrProfileNum_KeyPress);
			this.btnOK.DialogResult = DialogResult.OK;
			this.btnOK.Location = new Point(241, 9);
			this.btnOK.Name = "btnOK";
			this.btnOK.Size = new Size(69, 26);
			this.btnOK.TabIndex = 7;
			this.btnOK.Text = "确定";
			this.btnOK.UseVisualStyleBackColor = true;
			this.btnOK.Click += new EventHandler(this.btnTransfer_Click);
			this.lvwIvrProfile.FullRowSelect = true;
			this.lvwIvrProfile.GridLines = true;
			this.lvwIvrProfile.LargeImageList = this.imgLstMonitor;
			this.lvwIvrProfile.Location = new Point(16, 41);
			this.lvwIvrProfile.Name = "lvwIvrProfile";
			this.lvwIvrProfile.Size = new Size(295, 171);
			this.lvwIvrProfile.SmallImageList = this.imgLstMonitor;
			this.lvwIvrProfile.TabIndex = 6;
			this.lvwIvrProfile.UseCompatibleStateImageBehavior = false;
			this.lvwIvrProfile.View = View.Details;
			this.lvwIvrProfile.SelectedIndexChanged += new EventHandler(this.lvwAgent_SelectedIndexChanged);
			this.lvwIvrProfile.DoubleClick += new EventHandler(this.lvwIvrProfile_DoubleClick);
			this.imgLstMonitor.ImageStream = (ImageListStreamer)resources.GetObject("imgLstMonitor.ImageStream");
			this.imgLstMonitor.TransparentColor = Color.Transparent;
			this.imgLstMonitor.Images.SetKeyName(0, "hold");
			this.imgLstMonitor.Images.SetKeyName(1, "talk");
			this.imgLstMonitor.Images.SetKeyName(2, "ring");
			this.imgLstMonitor.Images.SetKeyName(3, "offline");
			this.btnCancel.DialogResult = DialogResult.Cancel;
			this.btnCancel.Location = new Point(413, 103);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new Size(75, 23);
			this.btnCancel.TabIndex = 9;
			this.btnCancel.Text = "取消";
			this.btnCancel.UseVisualStyleBackColor = true;
			this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
			base.AcceptButton = this.btnOK;
			base.AutoScaleDimensions = new SizeF(6f, 12f);
			base.AutoScaleMode = AutoScaleMode.Font;
			base.CancelButton = this.btnCancel;
			base.ClientSize = new Size(325, 226);
			base.Controls.Add(this.btnCancel);
			base.Controls.Add(this.txtIvrProfileNum);
			base.Controls.Add(this.btnOK);
			base.Controls.Add(this.lvwIvrProfile);
			base.FormBorderStyle = FormBorderStyle.FixedSingle;
			base.Icon = (Icon)resources.GetObject("$this.Icon");
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "FrmTransferIvrProfile";
			base.StartPosition = FormStartPosition.CenterParent;
			this.Text = "转接IVR Profile";
			base.Load += new EventHandler(this.FrmTransfer_Load);
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		public FrmTransferIvrProfile()
		{
			this.InitializeComponent();
		}

		private void FrmTransfer_Load(object sender, EventArgs e)
		{
			this.btnOK.Enabled = false;
			this.initLvwAgent();
		}

		private void initLvwAgent()
		{
			this.lvwIvrProfile.Columns.Add("IVR Profile号码", 150, HorizontalAlignment.Left);
			this.lvwIvrProfile.Columns.Add("IVR Profile名称", 150, HorizontalAlignment.Left);
		}

		public void Evt_UpdateLvwIvrProfile(Dictionary<string, string> ivrProfile_list)
		{
			if (null != ivrProfile_list)
			{
				this.lvwIvrProfile.BeginUpdate();
				this.lvwIvrProfile.Items.Clear();
				this.mIvrProfileList = ivrProfile_list;
				foreach (KeyValuePair<string, string> ivrProfle in ivrProfile_list)
				{
					if (ivrProfle.Key.ToString().StartsWith(this.txtIvrProfileNum.Text) || !this.txtIvrProfileNum.Focused)
					{
						ListViewItem lvi = new ListViewItem();
						lvi.Text = ivrProfle.Key;
						lvi.SubItems.Add(ivrProfle.Value);
						this.lvwIvrProfile.Items.Add(lvi);
					}
				}
				this.lvwIvrProfile.EndUpdate();
				this.txtIvrProfileNum.Focus();
			}
		}

		private void lvwAgent_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (this.lvwIvrProfile.SelectedItems.Count > 0)
			{
				this.txtIvrProfileNum.Text = this.lvwIvrProfile.SelectedItems[0].Text.ToString();
			}
			this.btnOK.Enabled = true;
		}

		private void lvwIvrProfile_DoubleClick(object sender, EventArgs e)
		{
			if (this.lvwIvrProfile.SelectedItems.Count > 0)
			{
				this.txtIvrProfileNum.Text = this.lvwIvrProfile.SelectedItems[0].Text;
			}
			this.btnOK.Enabled = true;
			this.btnOK.PerformClick();
		}

		private void txtIvrProfileNum_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (!char.IsNumber(e.KeyChar) && e.KeyChar != '\r' && e.KeyChar != '\b' && e.KeyChar != '\u0001' && e.KeyChar != '\u0003' && e.KeyChar != '\u0016' && e.KeyChar != '\u0018')
			{
				e.Handled = true;
			}
			else if (e.KeyChar == '\u0001')
			{
				this.txtIvrProfileNum.SelectAll();
				this.txtIvrProfileNum.Focus();
			}
		}

		private void txtIvrProfileNum_TextChanged(object sender, EventArgs e)
		{
			if (this.txtIvrProfileNum.Focused)
			{
				this.btnOK.Enabled = false;
				this.Evt_UpdateLvwIvrProfile(this.mIvrProfileList);
				for (int i = 0; i < this.lvwIvrProfile.Items.Count; i++)
				{
					if (this.lvwIvrProfile.Items[i].Text == this.txtIvrProfileNum.Text)
					{
						this.btnOK.Enabled = true;
					}
					else
					{
						this.btnOK.Enabled = false;
					}
					if (!this.lvwIvrProfile.Items[i].Text.StartsWith(this.txtIvrProfileNum.Text))
					{
						this.lvwIvrProfile.Items.RemoveAt(i);
					}
				}
			}
		}

		private void btnCancel_Click(object sender, EventArgs e)
		{
			base.Close();
		}

		private void btnTransfer_Click(object sender, EventArgs e)
		{
			if (this.txtIvrProfileNum.Text == "")
			{
				base.DialogResult = DialogResult.No;
			}
			else if (!ComFunc.checkNumIsLegal(this.txtIvrProfileNum.Text.Trim()))
			{
				MessageBox.Show("IvrProfile号码非法！", "转接失败", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
				base.DialogResult = DialogResult.No;
			}
			else
			{
				base.DialogResult = DialogResult.OK;
			}
		}
	}
}
