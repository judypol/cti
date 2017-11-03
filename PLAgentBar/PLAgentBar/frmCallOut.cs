using log4net;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace PLAgentBar
{
	public class frmCallOut : Form
	{
		private bool bCtrlDown = false;

		public static ILog Log;

		public string mAgent_DID_Num = "";

		private IContainer components = null;

		private GroupBox groupBox1;

		private Button btnOk;

		private GroupBox groupBox2;

		public ComboBox cboRoute;

		private Button btnCancel;

		private Label label1;

		public ComboBox cboCallID;

		public frmCallOut(string agent_DID_Num)
		{
			this.InitializeComponent();
			frmCallOut.Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
			this.mAgent_DID_Num = agent_DID_Num;
		}

		private void btnOk_Click(object sender, EventArgs e)
		{
			frmCallOut.Log.Debug("enter " + MethodBase.GetCurrentMethod().Name);
			if (this.cboCallID.Text == "")
			{
				MessageBox.Show("呼出号码不能为空！", "呼出失败", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
				base.DialogResult = DialogResult.No;
			}
			else if (!ComFunc.checkNumIsLegal(this.cboCallID.Text.Trim()))
			{
				MessageBox.Show("呼出号码非法！", "呼出失败", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
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

		private void btnCancel_Click(object sender, EventArgs e)
		{
			base.Close();
		}

		public void Evt_GetAccessNumber(string[] accessNumbers, string default_access_num, List<string> callHistoryList)
		{
			frmCallOut.Log.Debug("enter " + MethodBase.GetCurrentMethod().Name);
			int i = 0;
			int chooseID = -1;
			this.cboRoute.Items.Clear();
			this.cboRoute.Items.Add("不设置");
			while (i < accessNumbers.Count<string>())
			{
				if (accessNumbers[i] == default_access_num)
				{
					chooseID = i + 1;
				}
				this.cboRoute.Items.Add(accessNumbers[i++]);
			}
			if (string.IsNullOrEmpty(this.mAgent_DID_Num))
			{
				if (chooseID >= 0)
				{
					this.cboRoute.SelectedIndex = chooseID;
				}
				else
				{
					this.cboRoute.SelectedIndex = 0;
				}
			}
			else
			{
				this.cboRoute.SelectedItem = this.mAgent_DID_Num;
			}
			this.cboCallID.Items.Clear();
			if (callHistoryList != null)
			{
				for (int j = 0; j < callHistoryList.Count; j++)
				{
					this.cboCallID.Items.Add(callHistoryList[j].ToString());
				}
			}
			this.cboCallID.Focus();
		}

		private void cboCallID_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (!char.IsNumber(e.KeyChar) && e.KeyChar != '\r' && e.KeyChar != '\b' && e.KeyChar != '\u0001' && e.KeyChar != '\u0003' && e.KeyChar != '\u0016' && e.KeyChar != '\u0018')
			{
				e.Handled = true;
			}
			else if (e.KeyChar == '\u0001')
			{
				this.cboCallID.SelectAll();
				this.cboCallID.Focus();
			}
		}

		private void frmCallOut_Load(object sender, EventArgs e)
		{
			this.cboCallID.MaxLength = 50;
			this.cboCallID.Text = "";
			this.cboCallID.Focus();
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
			ComponentResourceManager resources = new ComponentResourceManager(typeof(frmCallOut));
			this.groupBox1 = new GroupBox();
			this.cboRoute = new ComboBox();
			this.btnOk = new Button();
			this.groupBox2 = new GroupBox();
			this.label1 = new Label();
			this.cboCallID = new ComboBox();
			this.btnCancel = new Button();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			base.SuspendLayout();
			this.groupBox1.Controls.Add(this.cboRoute);
			this.groupBox1.Location = new Point(9, 12);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new Size(309, 76);
			this.groupBox1.TabIndex = 4;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "请选择要使用的号码(不选则用默认)";
			this.cboRoute.DropDownStyle = ComboBoxStyle.DropDownList;
			this.cboRoute.FormattingEnabled = true;
			this.cboRoute.Location = new Point(58, 33);
			this.cboRoute.Name = "cboRoute";
			this.cboRoute.Size = new Size(208, 20);
			this.cboRoute.TabIndex = 1;
			this.btnOk.DialogResult = DialogResult.Cancel;
			this.btnOk.Location = new Point(107, 253);
			this.btnOk.Name = "btnOk";
			this.btnOk.Size = new Size(113, 28);
			this.btnOk.TabIndex = 2;
			this.btnOk.Text = "确定";
			this.btnOk.UseVisualStyleBackColor = true;
			this.btnOk.Click += new EventHandler(this.btnOk_Click);
			this.groupBox2.Controls.Add(this.label1);
			this.groupBox2.Controls.Add(this.cboCallID);
			this.groupBox2.Location = new Point(9, 117);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new Size(309, 117);
			this.groupBox2.TabIndex = 5;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "请输入要拨打的电话号码";
			this.label1.AutoSize = true;
			this.label1.ForeColor = Color.Red;
			this.label1.Location = new Point(272, 57);
			this.label1.Name = "label1";
			this.label1.Size = new Size(11, 12);
			this.label1.TabIndex = 2;
			this.label1.Text = "*";
			this.cboCallID.FormattingEnabled = true;
			this.cboCallID.Location = new Point(58, 54);
			this.cboCallID.Name = "cboCallID";
			this.cboCallID.Size = new Size(208, 20);
			this.cboCallID.TabIndex = 0;
			this.cboCallID.KeyPress += new KeyPressEventHandler(this.cboCallID_KeyPress);
			this.btnCancel.DialogResult = DialogResult.Cancel;
			this.btnCancel.Location = new Point(441, 195);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new Size(113, 28);
			this.btnCancel.TabIndex = 7;
			this.btnCancel.Text = "取消";
			this.btnCancel.UseVisualStyleBackColor = true;
			this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
			base.AcceptButton = this.btnOk;
			base.AutoScaleDimensions = new SizeF(6f, 12f);
			base.AutoScaleMode = AutoScaleMode.Font;
			base.CancelButton = this.btnCancel;
			base.ClientSize = new Size(340, 301);
			base.Controls.Add(this.groupBox2);
			base.Controls.Add(this.btnCancel);
			base.Controls.Add(this.btnOk);
			base.Controls.Add(this.groupBox1);
			base.FormBorderStyle = FormBorderStyle.FixedSingle;
			base.Icon = (Icon)resources.GetObject("$this.Icon");
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "frmCallOut";
			base.StartPosition = FormStartPosition.CenterParent;
			this.Text = "拨打外线";
			base.Load += new EventHandler(this.frmCallOut_Load);
			base.FormClosing += new FormClosingEventHandler(this.frmCallOut_FormClosing);
			this.groupBox1.ResumeLayout(false);
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			base.ResumeLayout(false);
		}
	}
}
