using PLClient.Properties;
using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace PLClient
{
	internal class AboutBox : Form
	{
		private IContainer components = null;

		private TableLayoutPanel tableLayoutPanel;

		private PictureBox logoPictureBox;

		private TextBox textBoxDescription;

		private Button okButton;

		private Label labelProductName;

		private Label labelVersion;

		private Label labelCopyright;

		private Label labelCompanyName;

		private Button btnCancel;

		public string AssemblyTitle
		{
			get
			{
				object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyTitleAttribute), false);
				string result;
				if (attributes.Length > 0)
				{
					AssemblyTitleAttribute titleAttribute = (AssemblyTitleAttribute)attributes[0];
					if (titleAttribute.Title != "")
					{
						result = titleAttribute.Title;
						return result;
					}
				}
				result = Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().CodeBase);
				return result;
			}
		}

		public string AssemblyVersion
		{
			get
			{
				return Assembly.GetExecutingAssembly().GetName().Version.ToString();
			}
		}

		public string AssemblyDescription
		{
			get
			{
				object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false);
				string result;
				if (attributes.Length == 0)
				{
					result = "";
				}
				else
				{
					result = ((AssemblyDescriptionAttribute)attributes[0]).Description;
				}
				return result;
			}
		}

		public string AssemblyProduct
		{
			get
			{
				object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyProductAttribute), false);
				string result;
				if (attributes.Length == 0)
				{
					result = "";
				}
				else
				{
					result = ((AssemblyProductAttribute)attributes[0]).Product;
				}
				return result;
			}
		}

		public string AssemblyCopyright
		{
			get
			{
				object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);
				string result;
				if (attributes.Length == 0)
				{
					result = "";
				}
				else
				{
					result = ((AssemblyCopyrightAttribute)attributes[0]).Copyright;
				}
				return result;
			}
		}

		public string AssemblyCompany
		{
			get
			{
				object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCompanyAttribute), false);
				string result;
				if (attributes.Length == 0)
				{
					result = "";
				}
				else
				{
					result = ((AssemblyCompanyAttribute)attributes[0]).Company;
				}
				return result;
			}
		}

		public AboutBox()
		{
			this.InitializeComponent();
			this.Text = string.Format("关于 {0}", "");
			this.labelProductName.Text = "软件名称:" + Helper.ApplicationTitle;
			this.labelVersion.Text = string.Format("版本 {0} ", this.AssemblyVersion);
			this.labelCopyright.Text = Helper.CompanyName;
			this.labelCompanyName.Text = Helper.CompanyName;
			this.textBoxDescription.Text = this.AssemblyDescription;
		}

		private void okButton_Click(object sender, EventArgs e)
		{
			base.Close();
		}

		private void AboutBox_Deactivate(object sender, EventArgs e)
		{
			base.Close();
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
			this.tableLayoutPanel = new TableLayoutPanel();
			this.logoPictureBox = new PictureBox();
			this.labelProductName = new Label();
			this.labelVersion = new Label();
			this.labelCopyright = new Label();
			this.labelCompanyName = new Label();
			this.textBoxDescription = new TextBox();
			this.okButton = new Button();
			this.btnCancel = new Button();
			this.tableLayoutPanel.SuspendLayout();
			((ISupportInitialize)this.logoPictureBox).BeginInit();
			base.SuspendLayout();
			this.tableLayoutPanel.ColumnCount = 2;
			this.tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33f));
			this.tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 67f));
			this.tableLayoutPanel.Controls.Add(this.logoPictureBox, 0, 0);
			this.tableLayoutPanel.Controls.Add(this.labelProductName, 1, 0);
			this.tableLayoutPanel.Controls.Add(this.labelVersion, 1, 1);
			this.tableLayoutPanel.Controls.Add(this.labelCopyright, 1, 2);
			this.tableLayoutPanel.Controls.Add(this.labelCompanyName, 1, 3);
			this.tableLayoutPanel.Controls.Add(this.textBoxDescription, 1, 4);
			this.tableLayoutPanel.Controls.Add(this.okButton, 1, 5);
			this.tableLayoutPanel.Dock = DockStyle.Fill;
			this.tableLayoutPanel.Location = new Point(9, 8);
			this.tableLayoutPanel.Name = "tableLayoutPanel";
			this.tableLayoutPanel.RowCount = 6;
			this.tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 10f));
			this.tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 10f));
			this.tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 10f));
			this.tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 10f));
			this.tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 50f));
			this.tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 10f));
			this.tableLayoutPanel.Size = new Size(558, 236);
			this.tableLayoutPanel.TabIndex = 0;
			this.logoPictureBox.Dock = DockStyle.Fill;
			this.logoPictureBox.Image = Resources._131u0622055l0_101b61;
			this.logoPictureBox.Location = new Point(3, 3);
			this.logoPictureBox.Name = "logoPictureBox";
			this.tableLayoutPanel.SetRowSpan(this.logoPictureBox, 6);
			this.logoPictureBox.Size = new Size(178, 230);
			this.logoPictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
			this.logoPictureBox.TabIndex = 12;
			this.logoPictureBox.TabStop = false;
			this.labelProductName.Dock = DockStyle.Fill;
			this.labelProductName.Location = new Point(190, 0);
			this.labelProductName.Margin = new Padding(6, 0, 3, 0);
			this.labelProductName.MaximumSize = new Size(0, 16);
			this.labelProductName.Name = "labelProductName";
			this.labelProductName.Size = new Size(365, 16);
			this.labelProductName.TabIndex = 19;
			this.labelProductName.Text = "产品名称";
			this.labelProductName.TextAlign = ContentAlignment.MiddleLeft;
			this.labelVersion.Dock = DockStyle.Fill;
			this.labelVersion.Location = new Point(190, 23);
			this.labelVersion.Margin = new Padding(6, 0, 3, 0);
			this.labelVersion.MaximumSize = new Size(0, 16);
			this.labelVersion.Name = "labelVersion";
			this.labelVersion.Size = new Size(365, 16);
			this.labelVersion.TabIndex = 0;
			this.labelVersion.Text = "版本";
			this.labelVersion.TextAlign = ContentAlignment.MiddleLeft;
			this.labelCopyright.Dock = DockStyle.Fill;
			this.labelCopyright.Location = new Point(190, 46);
			this.labelCopyright.Margin = new Padding(6, 0, 3, 0);
			this.labelCopyright.MaximumSize = new Size(0, 16);
			this.labelCopyright.Name = "labelCopyright";
			this.labelCopyright.Size = new Size(365, 16);
			this.labelCopyright.TabIndex = 21;
			this.labelCopyright.Text = "版权(C) Polylink";
			this.labelCopyright.TextAlign = ContentAlignment.MiddleLeft;
			this.labelCompanyName.Dock = DockStyle.Fill;
			this.labelCompanyName.Location = new Point(190, 69);
			this.labelCompanyName.Margin = new Padding(6, 0, 3, 0);
			this.labelCompanyName.MaximumSize = new Size(0, 16);
			this.labelCompanyName.Name = "labelCompanyName";
			this.labelCompanyName.Size = new Size(365, 16);
			this.labelCompanyName.TabIndex = 22;
			this.labelCompanyName.Text = "公司名称";
			this.labelCompanyName.TextAlign = ContentAlignment.MiddleLeft;
			this.textBoxDescription.Dock = DockStyle.Fill;
			this.textBoxDescription.Location = new Point(190, 95);
			this.textBoxDescription.Margin = new Padding(6, 3, 3, 3);
			this.textBoxDescription.Multiline = true;
			this.textBoxDescription.Name = "textBoxDescription";
			this.textBoxDescription.ReadOnly = true;
			this.textBoxDescription.ScrollBars = ScrollBars.Both;
			this.textBoxDescription.Size = new Size(365, 112);
			this.textBoxDescription.TabIndex = 23;
			this.textBoxDescription.TabStop = false;
			this.textBoxDescription.Text = "CTI 客户端程序";
			this.okButton.Anchor = (AnchorStyles.Bottom | AnchorStyles.Right);
			this.okButton.DialogResult = DialogResult.Cancel;
			this.okButton.Location = new Point(480, 213);
			this.okButton.Name = "okButton";
			this.okButton.Size = new Size(75, 20);
			this.okButton.TabIndex = 24;
			this.okButton.Text = "确定(&O)";
			this.okButton.Click += new EventHandler(this.okButton_Click);
			this.btnCancel.DialogResult = DialogResult.Cancel;
			this.btnCancel.Location = new Point(18, 26);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new Size(75, 23);
			this.btnCancel.TabIndex = 1;
			this.btnCancel.Text = "button1";
			this.btnCancel.UseVisualStyleBackColor = true;
			this.btnCancel.Click += new EventHandler(this.button1_Click);
			base.AcceptButton = this.okButton;
			base.AutoScaleDimensions = new SizeF(6f, 12f);
			base.AutoScaleMode = AutoScaleMode.Font;
			base.CancelButton = this.btnCancel;
			base.ClientSize = new Size(576, 252);
			base.Controls.Add(this.tableLayoutPanel);
			base.Controls.Add(this.btnCancel);
			base.FormBorderStyle = FormBorderStyle.FixedDialog;
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "AboutBox";
			base.Padding = new Padding(9, 8, 9, 8);
			base.ShowIcon = false;
			base.ShowInTaskbar = false;
			base.StartPosition = FormStartPosition.CenterScreen;
			this.Text = "关于";
			base.Deactivate += new EventHandler(this.AboutBox_Deactivate);
			this.tableLayoutPanel.ResumeLayout(false);
			this.tableLayoutPanel.PerformLayout();
			((ISupportInitialize)this.logoPictureBox).EndInit();
			base.ResumeLayout(false);
		}
	}
}
