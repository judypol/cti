using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace PLAgentBar
{
	public class FrmTransferIvr : Form
	{
		private IContainer components = null;

		public TextBox txtIvrNum;

		private Button btnOK;

		private ListView lvwIVR;

		private ImageList imgLstMonitor;

		private Button btnCancel;

		private Dictionary<string, string> mIvrList;

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
			ComponentResourceManager resources = new ComponentResourceManager(typeof(FrmTransferIvr));
			this.txtIvrNum = new TextBox();
			this.btnOK = new Button();
			this.lvwIVR = new ListView();
			this.imgLstMonitor = new ImageList(this.components);
			this.btnCancel = new Button();
			base.SuspendLayout();
			this.txtIvrNum.Location = new Point(16, 12);
			this.txtIvrNum.Name = "txtIvrNum";
			this.txtIvrNum.Size = new Size(206, 21);
			this.txtIvrNum.TabIndex = 8;
			this.txtIvrNum.TextChanged += new EventHandler(this.txtIvrNum_TextChanged);
			this.txtIvrNum.KeyPress += new KeyPressEventHandler(this.txtIvrNum_KeyPress);
			this.btnOK.DialogResult = DialogResult.OK;
			this.btnOK.Location = new Point(241, 9);
			this.btnOK.Name = "btnOK";
			this.btnOK.Size = new Size(69, 26);
			this.btnOK.TabIndex = 7;
			this.btnOK.Text = "确定";
			this.btnOK.UseVisualStyleBackColor = true;
			this.btnOK.Click += new EventHandler(this.btnTransfer_Click);
			this.lvwIVR.FullRowSelect = true;
			this.lvwIVR.GridLines = true;
			this.lvwIVR.LargeImageList = this.imgLstMonitor;
			this.lvwIVR.Location = new Point(16, 41);
			this.lvwIVR.Name = "lvwIVR";
			this.lvwIVR.Size = new Size(295, 171);
			this.lvwIVR.SmallImageList = this.imgLstMonitor;
			this.lvwIVR.TabIndex = 6;
			this.lvwIVR.UseCompatibleStateImageBehavior = false;
			this.lvwIVR.View = View.Details;
			this.lvwIVR.SelectedIndexChanged += new EventHandler(this.lvwAgent_SelectedIndexChanged);
			this.lvwIVR.DoubleClick += new EventHandler(this.lvwIVR_DoubleClick);
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
			base.Controls.Add(this.txtIvrNum);
			base.Controls.Add(this.btnOK);
			base.Controls.Add(this.lvwIVR);
			base.FormBorderStyle = FormBorderStyle.FixedSingle;
			base.Icon = (Icon)resources.GetObject("$this.Icon");
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "FrmTransferIvr";
			base.StartPosition = FormStartPosition.CenterParent;
			this.Text = "转接IVR";
			base.Load += new EventHandler(this.FrmTransfer_Load);
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		public FrmTransferIvr()
		{
			this.InitializeComponent();
		}

		private void initLvwAgent()
		{
			this.lvwIVR.Columns.Add("IVR号码", 80, HorizontalAlignment.Left);
			this.lvwIVR.Columns.Add("IVR名称", 80, HorizontalAlignment.Left);
		}

		public void Evt_UpdateLvwIvr(Dictionary<string, string> ivr_list)
		{
			this.lvwIVR.BeginUpdate();
			this.lvwIVR.Items.Clear();
			this.mIvrList = ivr_list;
			foreach (KeyValuePair<string, string> ivr in ivr_list)
			{
				if (ivr.Key.ToString().StartsWith(this.txtIvrNum.Text) || !this.txtIvrNum.Focused)
				{
					ListViewItem lvi = new ListViewItem();
					lvi.Text = ivr.Key;
					lvi.SubItems.Add(ivr.Value);
					this.lvwIVR.Items.Add(lvi);
				}
			}
			this.lvwIVR.EndUpdate();
			this.txtIvrNum.Focus();
		}

		private void FrmTransfer_Load(object sender, EventArgs e)
		{
			this.btnOK.Enabled = false;
			this.initLvwAgent();
		}

		private void lvwAgent_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (this.lvwIVR.SelectedItems.Count > 0)
			{
				this.txtIvrNum.Text = this.lvwIVR.SelectedItems[0].Text.ToString();
			}
			this.btnOK.Enabled = true;
		}

		private void btnCancel_Click(object sender, EventArgs e)
		{
			base.Close();
		}

		private void btnTransfer_Click(object sender, EventArgs e)
		{
			if (this.txtIvrNum.Text == "")
			{
				base.DialogResult = DialogResult.No;
			}
			else if (!ComFunc.checkNumIsLegal(this.txtIvrNum.Text.Trim()))
			{
				MessageBox.Show("Ivr号码非法！", "转接失败", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
				base.DialogResult = DialogResult.No;
			}
			else
			{
				base.DialogResult = DialogResult.OK;
			}
		}

		private void txtIvrNum_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (!char.IsNumber(e.KeyChar) && e.KeyChar != '\r' && e.KeyChar != '\b' && e.KeyChar != '\u0001' && e.KeyChar != '\u0003' && e.KeyChar != '\u0016' && e.KeyChar != '\u0018')
			{
				e.Handled = true;
			}
			else if (e.KeyChar == '\u0001')
			{
				this.txtIvrNum.SelectAll();
				this.txtIvrNum.Focus();
			}
		}

		private void txtIvrNum_TextChanged(object sender, EventArgs e)
		{
			if (this.txtIvrNum.Focused)
			{
				this.btnOK.Enabled = false;
				this.Evt_UpdateLvwIvr(this.mIvrList);
				for (int i = 0; i < this.lvwIVR.Items.Count; i++)
				{
					if (this.lvwIVR.Items[i].Text == this.txtIvrNum.Text)
					{
						this.btnOK.Enabled = true;
					}
					else
					{
						this.btnOK.Enabled = false;
					}
					if (!this.lvwIVR.Items[i].Text.StartsWith(this.txtIvrNum.Text))
					{
						this.lvwIVR.Items.RemoveAt(i);
					}
				}
			}
		}

		private void lvwIVR_DoubleClick(object sender, EventArgs e)
		{
			if (this.lvwIVR.SelectedItems.Count > 0)
			{
				this.txtIvrNum.Text = this.lvwIVR.SelectedItems[0].Text;
			}
			this.btnOK.Enabled = true;
			this.btnOK.PerformClick();
		}
	}
}
