using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace PLAgentBar
{
	public class FrmTransferQueue : Form
	{
		private Dictionary<string, string> mQueueList;

		private IContainer components = null;

		public TextBox txtQueueNum;

		private Button btnOK;

		private ListView lvwQueue;

		private ImageList imgLstMonitor;

		private Button btnCancel;

		public FrmTransferQueue()
		{
			this.InitializeComponent();
		}

		private void initLvwAgent()
		{
			this.lvwQueue.Columns.Add("队列号码", 80, HorizontalAlignment.Left);
			this.lvwQueue.Columns.Add("队列名称", 80, HorizontalAlignment.Left);
		}

		private int FindAgentID(string strAgentID, ListView lvw)
		{
			ListViewItem foundItem = lvw.FindItemWithText(strAgentID);
			int result;
			if (foundItem != null)
			{
				result = foundItem.Index;
			}
			else
			{
				result = -1;
			}
			return result;
		}

		public void Evt_UpdateLvwQueue(Dictionary<string, string> queue_list)
		{
			this.lvwQueue.BeginUpdate();
			this.lvwQueue.Items.Clear();
			this.mQueueList = queue_list;
			foreach (KeyValuePair<string, string> queue in queue_list)
			{
				if (queue.Key.ToString().StartsWith(this.txtQueueNum.Text) || !this.txtQueueNum.Focused)
				{
					ListViewItem lvi = new ListViewItem();
					lvi.Text = queue.Key;
					lvi.SubItems.Add(queue.Value);
					this.lvwQueue.Items.Add(lvi);
				}
			}
			this.lvwQueue.EndUpdate();
			this.txtQueueNum.Focus();
		}

		private void FrmTransfer_Load(object sender, EventArgs e)
		{
			this.btnOK.Enabled = false;
			this.initLvwAgent();
		}

		private void lvwAgent_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (this.lvwQueue.SelectedItems.Count > 0)
			{
				this.txtQueueNum.Text = this.lvwQueue.SelectedItems[0].Text.ToString();
			}
			this.btnOK.Enabled = true;
		}

		private void btnCancel_Click(object sender, EventArgs e)
		{
			base.Close();
		}

		private void btnTransfer_Click(object sender, EventArgs e)
		{
			if (this.txtQueueNum.Text == "" || this.lvwQueue.Items.Count == 0)
			{
				base.DialogResult = DialogResult.No;
			}
			else if (!ComFunc.checkNumIsLegal(this.txtQueueNum.Text.Trim()))
			{
				MessageBox.Show("队列号码非法！", "转接失败", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
				base.DialogResult = DialogResult.No;
			}
			else
			{
				base.DialogResult = DialogResult.OK;
			}
		}

		private void txtQueueNum_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (!char.IsNumber(e.KeyChar) && e.KeyChar != '\r' && e.KeyChar != '\b' && e.KeyChar != '\u0001' && e.KeyChar != '\u0003' && e.KeyChar != '\u0016' && e.KeyChar != '\u0018')
			{
				e.Handled = true;
			}
			else if (e.KeyChar == '\u0001')
			{
				this.txtQueueNum.SelectAll();
				this.txtQueueNum.Focus();
			}
		}

		private void txtQueueNum_TextChanged(object sender, EventArgs e)
		{
			if (this.txtQueueNum.Focused)
			{
				this.btnOK.Enabled = false;
				this.Evt_UpdateLvwQueue(this.mQueueList);
				for (int i = 0; i < this.lvwQueue.Items.Count; i++)
				{
					if (this.lvwQueue.Items[i].Text == this.txtQueueNum.Text)
					{
						this.btnOK.Enabled = true;
					}
					else
					{
						this.btnOK.Enabled = false;
					}
					if (!this.lvwQueue.Items[i].Text.StartsWith(this.txtQueueNum.Text))
					{
						this.lvwQueue.Items.RemoveAt(i);
					}
				}
			}
		}

		private void lvwQueue_DoubleClick(object sender, EventArgs e)
		{
			if (this.lvwQueue.SelectedItems.Count > 0)
			{
				this.txtQueueNum.Text = this.lvwQueue.SelectedItems[0].Text;
			}
			this.btnOK.Enabled = true;
			this.btnOK.PerformClick();
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
			this.components = new Container();
			ComponentResourceManager resources = new ComponentResourceManager(typeof(FrmTransferQueue));
			this.txtQueueNum = new TextBox();
			this.btnOK = new Button();
			this.lvwQueue = new ListView();
			this.imgLstMonitor = new ImageList(this.components);
			this.btnCancel = new Button();
			base.SuspendLayout();
			this.txtQueueNum.Location = new Point(16, 12);
			this.txtQueueNum.Name = "txtQueueNum";
			this.txtQueueNum.Size = new Size(206, 21);
			this.txtQueueNum.TabIndex = 8;
			this.txtQueueNum.TextChanged += new EventHandler(this.txtQueueNum_TextChanged);
			this.txtQueueNum.KeyPress += new KeyPressEventHandler(this.txtQueueNum_KeyPress);
			this.btnOK.DialogResult = DialogResult.OK;
			this.btnOK.Location = new Point(241, 9);
			this.btnOK.Name = "btnOK";
			this.btnOK.Size = new Size(69, 26);
			this.btnOK.TabIndex = 7;
			this.btnOK.Text = "确定";
			this.btnOK.UseVisualStyleBackColor = true;
			this.btnOK.Click += new EventHandler(this.btnTransfer_Click);
			this.lvwQueue.Alignment = ListViewAlignment.Left;
			this.lvwQueue.FullRowSelect = true;
			this.lvwQueue.GridLines = true;
			this.lvwQueue.LargeImageList = this.imgLstMonitor;
			this.lvwQueue.Location = new Point(16, 41);
			this.lvwQueue.Name = "lvwQueue";
			this.lvwQueue.Size = new Size(295, 171);
			this.lvwQueue.SmallImageList = this.imgLstMonitor;
			this.lvwQueue.TabIndex = 6;
			this.lvwQueue.UseCompatibleStateImageBehavior = false;
			this.lvwQueue.View = View.Details;
			this.lvwQueue.SelectedIndexChanged += new EventHandler(this.lvwAgent_SelectedIndexChanged);
			this.lvwQueue.DoubleClick += new EventHandler(this.lvwQueue_DoubleClick);
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
			base.Controls.Add(this.txtQueueNum);
			base.Controls.Add(this.btnOK);
			base.Controls.Add(this.lvwQueue);
			base.FormBorderStyle = FormBorderStyle.FixedSingle;
			base.Icon = (Icon)resources.GetObject("$this.Icon");
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "FrmTransferQueue";
			base.StartPosition = FormStartPosition.CenterParent;
			this.Text = "转接队列";
			base.Load += new EventHandler(this.FrmTransfer_Load);
			base.ResumeLayout(false);
			base.PerformLayout();
		}
	}
}
