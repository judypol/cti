using PLAgentBar;
using PLClient.Properties;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace PLClient
{
	public class FrmSysInfo : Form
	{
		public const int WM_SYSCOMMAND = 274;

		public const int SC_MOVE = 61456;

		public const int HTCAPTION = 2;

		public AgentBar agentBar1;

		private bool mAutoClose = false;

		private int mStayTime = 5000;

		private string mFrmTitle = "";

		private string mMsgInfo = "";

		private int mScreenWidth;

		private int mScreenHeight;

		private bool mBlnStop;

		private bool mHaveDelay = false;

		private bool mReachMaxHeigh = false;

		private Point mMaxpoint;

		private Point mStartPos;

		private Point mEndPos;

		private bool mBlnClose = false;

		private int mStayTimeCount = 0;

		private bool mBtnViewVisible = false;

		private string mAgentBarBtnName;

		private IContainer components = null;

		private Label lblInfo;

		private Timer tmrMove;

		private PictureBox pictureBox2;

		private Button btnView;

		public bool AutoClose
		{
			get
			{
				return this.mAutoClose;
			}
			set
			{
				this.mAutoClose = value;
			}
		}

		public bool ButtonVisible
		{
			get
			{
				return this.mBtnViewVisible;
			}
			set
			{
				this.mBtnViewVisible = value;
			}
		}

		public Point EndPoint
		{
			get
			{
				return this.mEndPos;
			}
			set
			{
				this.mEndPos = value;
			}
		}

		public string AgentBarBtnName
		{
			get
			{
				return this.mAgentBarBtnName;
			}
			set
			{
				this.mAgentBarBtnName = value;
			}
		}

		[DllImport("user32")]
		private static extern bool ReleaseCapture();

		[DllImport("user32")]
		private static extern bool SendMessage(IntPtr hwnd, int wMsg, int wParam, int lParam);

		public FrmSysInfo()
		{
			this.InitializeComponent();
		}

		public void Start(string Title, string MsgInfo, bool AutoClose, int StayTime, Point StartPos)
		{
			this.btnView.Visible = this.mBtnViewVisible;
			this.mAutoClose = AutoClose;
			this.mStartPos = StartPos;
			this.mStayTime = StayTime;
			this.tmrMove.Interval = 10;
			this.mFrmTitle = Title;
			this.mMsgInfo = MsgInfo;
			this.mBlnStop = false;
			this.mMaxpoint = new Point(0, 0);
			this.ShowForm();
		}

		public void refreshForm(string title, string MsgInfo)
		{
			MessageBox.Show("msginfo:" + MsgInfo);
			this.lblInfo.Text = this.mMsgInfo;
			this.Text = title;
		}

		public void Stop()
		{
			this.mBlnStop = true;
		}

		private void ShowForm()
		{
			this.mScreenWidth = Screen.PrimaryScreen.WorkingArea.Width;
			this.mScreenHeight = Screen.PrimaryScreen.WorkingArea.Height;
			Point p = new Point(this.mScreenWidth - base.Width - 5, this.mScreenHeight - 20);
			base.PointToClient(p);
			base.Location = p;
			if (this.mStartPos.Y - base.Height < 0)
			{
				this.mStartPos.Y = this.mScreenHeight;
			}
			this.mEndPos.X = this.mStartPos.X;
			this.mEndPos.Y = this.mStartPos.Y - base.Height;
			base.ShowInTaskbar = false;
			this.lblInfo.Text = this.mMsgInfo;
			this.Text = this.mFrmTitle;
			base.TopMost = true;
			base.Show();
			this.tmrMove.Tick += new EventHandler(this.tmrMove_Tick);
			this.tmrMove.Start();
		}

		private void tmrMove_Tick(object sender, EventArgs e)
		{
			if (!this.mBlnStop)
			{
				if (this.mEndPos.Y < base.Location.Y && !this.mReachMaxHeigh)
				{
					base.Location = new Point(base.Location.X, base.Location.Y - 13);
				}
				else if (!this.IsMouseEnter())
				{
					this.mReachMaxHeigh = true;
					if (this.mAutoClose)
					{
						if (this.mStayTimeCount < this.mStayTime / 10)
						{
							this.mStayTimeCount++;
						}
						else
						{
							base.Close();
							this.mBlnClose = true;
						}
					}
				}
			}
			else if (this.mReachMaxHeigh)
			{
				base.Close();
				this.mBlnClose = true;
			}
			else
			{
				this.mAutoClose = true;
				this.mBlnStop = false;
			}
		}

		private bool IsMouseEnter()
		{
			bool result;
			if (this.mBlnClose)
			{
				result = false;
			}
			else
			{
				Point p = default(Point);
				p = base.PointToClient(Control.MousePosition);
				result = (p.X >= -4 && p.X <= base.Width - 6 && p.Y >= -21 && p.Y <= base.Height - 21);
			}
			return result;
		}

		private void FrmSysInfo_MouseDown(object sender, MouseEventArgs e)
		{
			FrmSysInfo.ReleaseCapture();
			FrmSysInfo.SendMessage(base.Handle, 274, 61458, 0);
		}

		private void FrmSysInfo_Load(object sender, EventArgs e)
		{
		}

		private void pictureBox2_Click(object sender, EventArgs e)
		{
			base.Close();
		}

		private void lblInfo_MouseDown(object sender, MouseEventArgs e)
		{
			FrmSysInfo.ReleaseCapture();
			FrmSysInfo.SendMessage(base.Handle, 274, 61458, 0);
		}

		private void btnView_Click(object sender, EventArgs e)
		{
			if (this.agentBar1 != null)
			{
				int rt = this.agentBar1.DoClickAgentBarButtonByName(this.mAgentBarBtnName);
				if (rt != 0)
				{
					MessageBox.Show("打开窗口失败！", "查看", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				}
			}
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
			this.components = new Container();
			this.lblInfo = new Label();
			this.tmrMove = new Timer(this.components);
			this.pictureBox2 = new PictureBox();
			this.btnView = new Button();
			((ISupportInitialize)this.pictureBox2).BeginInit();
			base.SuspendLayout();
			this.lblInfo.BackColor = SystemColors.Window;
			this.lblInfo.Location = new Point(33, 50);
			this.lblInfo.Name = "lblInfo";
			this.lblInfo.Size = new Size(196, 88);
			this.lblInfo.TabIndex = 0;
			this.lblInfo.Text = "提醒消息";
			this.lblInfo.MouseDown += new MouseEventHandler(this.lblInfo_MouseDown);
			this.tmrMove.Tick += new EventHandler(this.tmrMove_Tick);
			this.pictureBox2.Image = Resources.Close2;
			this.pictureBox2.Location = new Point(247, 4);
			this.pictureBox2.Name = "pictureBox2";
			this.pictureBox2.Size = new Size(17, 17);
			this.pictureBox2.SizeMode = PictureBoxSizeMode.AutoSize;
			this.pictureBox2.TabIndex = 6;
			this.pictureBox2.TabStop = false;
			this.pictureBox2.Click += new EventHandler(this.pictureBox2_Click);
			this.btnView.Location = new Point(181, 127);
			this.btnView.Name = "btnView";
			this.btnView.Size = new Size(75, 23);
			this.btnView.TabIndex = 7;
			this.btnView.Text = "查看";
			this.btnView.UseVisualStyleBackColor = true;
			this.btnView.Click += new EventHandler(this.btnView_Click);
			base.AutoScaleDimensions = new SizeF(6f, 12f);
			base.AutoScaleMode = AutoScaleMode.Font;
			this.BackgroundImage = Resources._1;
			base.ClientSize = new Size(268, 162);
			base.Controls.Add(this.btnView);
			base.Controls.Add(this.pictureBox2);
			base.Controls.Add(this.lblInfo);
			base.FormBorderStyle = FormBorderStyle.None;
			base.Name = "FrmSysInfo";
			this.Text = "FrmSysInfo";
			base.Load += new EventHandler(this.FrmSysInfo_Load);
			base.MouseDown += new MouseEventHandler(this.FrmSysInfo_MouseDown);
			((ISupportInitialize)this.pictureBox2).EndInit();
			base.ResumeLayout(false);
			base.PerformLayout();
		}
	}
}
