using PLClient.Properties;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace PLClient
{
	public class FrmTip : Form
	{
		public const int WM_SYSCOMMAND = 274;

		public const int SC_MOVE = 61456;

		public const int HTCAPTION = 2;

		private bool mAutoClose = false;

		private int mStayTime = 2000;

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

		private bool blnClose = false;

		private IContainer components = null;

		private PictureBox pictureBox1;

		private Button btnHangup;

		private Label lblInfo;

		private Button btnAnswer;

		private System.Windows.Forms.Timer tmrMove;

		private PictureBox pictureBox2;

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

		[DllImport("user32")]
		private static extern bool ReleaseCapture();

		[DllImport("user32")]
		private static extern bool SendMessage(IntPtr hwnd, int wMsg, int wParam, int lParam);

		public FrmTip()
		{
			this.InitializeComponent();
		}

		public void Start(string Title, string MsgInfo, bool AutoClose, int StayTime, Point StartPos)
		{
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
			if (!this.IsMouseEnter())
			{
				if (!this.mBlnStop)
				{
					if (this.mEndPos.Y < base.Location.Y && !this.mReachMaxHeigh)
					{
						base.Location = new Point(base.Location.X, base.Location.Y - 13);
					}
					else
					{
						this.mReachMaxHeigh = true;
						if (this.mAutoClose)
						{
							if (base.Location.Y < this.mScreenHeight || this.mMaxpoint.Y < base.Height)
							{
								if (this.mHaveDelay)
								{
									base.Location = new Point(base.Location.X, base.Location.Y + 3);
									this.mMaxpoint.Y = this.mMaxpoint.Y + 3;
								}
								else
								{
									Thread.Sleep(this.mStayTime);
									this.mHaveDelay = true;
								}
							}
							else
							{
								base.Close();
								this.blnClose = true;
							}
						}
					}
				}
				else if (this.mReachMaxHeigh)
				{
					base.Close();
					this.blnClose = true;
				}
				else
				{
					this.mAutoClose = true;
					this.mBlnStop = false;
				}
			}
		}

		private bool IsMouseEnter()
		{
			return false;
		}

		private void btnAnswer_Click(object sender, EventArgs e)
		{
		}

		private void btnHangup_Click(object sender, EventArgs e)
		{
			Program.newFrmMain.agentBar1.DoHangUp();
			this.tmrMove.Stop();
			base.Close();
		}

		private void FrmTip_MouseDown(object sender, MouseEventArgs e)
		{
			FrmTip.ReleaseCapture();
			FrmTip.SendMessage(base.Handle, 274, 61458, 0);
		}

		private void FrmTip_Load(object sender, EventArgs e)
		{
		}

		private void pictureBox2_Click(object sender, EventArgs e)
		{
			base.Close();
		}

		private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
		{
			FrmTip.ReleaseCapture();
			FrmTip.SendMessage(base.Handle, 274, 61458, 0);
		}

		private void lblInfo_MouseDown(object sender, MouseEventArgs e)
		{
			FrmTip.ReleaseCapture();
			FrmTip.SendMessage(base.Handle, 274, 61458, 0);
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
			ComponentResourceManager resources = new ComponentResourceManager(typeof(FrmTip));
			this.pictureBox1 = new PictureBox();
			this.btnHangup = new Button();
			this.lblInfo = new Label();
			this.btnAnswer = new Button();
			this.tmrMove = new System.Windows.Forms.Timer(this.components);
			this.pictureBox2 = new PictureBox();
			((ISupportInitialize)this.pictureBox1).BeginInit();
			((ISupportInitialize)this.pictureBox2).BeginInit();
			base.SuspendLayout();
			this.pictureBox1.BackColor = Color.White;
			this.pictureBox1.Image = Resources.callin;
			this.pictureBox1.Location = new Point(12, 56);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new Size(92, 81);
			this.pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
			this.pictureBox1.TabIndex = 2;
			this.pictureBox1.TabStop = false;
			this.pictureBox1.MouseDown += new MouseEventHandler(this.pictureBox1_MouseDown);
			this.btnHangup.FlatStyle = FlatStyle.Flat;
			this.btnHangup.ImageAlign = ContentAlignment.MiddleLeft;
			this.btnHangup.Location = new Point(173, 121);
			this.btnHangup.Name = "btnHangup";
			this.btnHangup.Size = new Size(84, 35);
			this.btnHangup.TabIndex = 1;
			this.btnHangup.Text = "挂断";
			this.btnHangup.TextAlign = ContentAlignment.MiddleRight;
			this.btnHangup.UseVisualStyleBackColor = true;
			this.btnHangup.Visible = false;
			this.btnHangup.Click += new EventHandler(this.btnHangup_Click);
			this.lblInfo.AutoSize = true;
			this.lblInfo.BackColor = Color.White;
			this.lblInfo.FlatStyle = FlatStyle.Flat;
			this.lblInfo.Font = new Font("黑体", 12f, FontStyle.Bold, GraphicsUnit.Point, 134);
			this.lblInfo.Location = new Point(125, 65);
			this.lblInfo.Name = "lblInfo";
			this.lblInfo.Size = new Size(107, 16);
			this.lblInfo.TabIndex = 3;
			this.lblInfo.Text = "13816153452";
			this.lblInfo.MouseDown += new MouseEventHandler(this.lblInfo_MouseDown);
			this.btnAnswer.FlatStyle = FlatStyle.Flat;
			this.btnAnswer.ImageAlign = ContentAlignment.MiddleLeft;
			this.btnAnswer.Location = new Point(85, 121);
			this.btnAnswer.Name = "btnAnswer";
			this.btnAnswer.Size = new Size(82, 35);
			this.btnAnswer.TabIndex = 4;
			this.btnAnswer.Text = "接听";
			this.btnAnswer.TextAlign = ContentAlignment.MiddleRight;
			this.btnAnswer.UseVisualStyleBackColor = true;
			this.btnAnswer.Visible = false;
			this.btnAnswer.Click += new EventHandler(this.btnAnswer_Click);
			this.tmrMove.Tick += new EventHandler(this.tmrMove_Tick);
			this.pictureBox2.Image = Resources.Close2;
			this.pictureBox2.Location = new Point(247, 5);
			this.pictureBox2.Name = "pictureBox2";
			this.pictureBox2.Size = new Size(17, 17);
			this.pictureBox2.SizeMode = PictureBoxSizeMode.AutoSize;
			this.pictureBox2.TabIndex = 5;
			this.pictureBox2.TabStop = false;
			this.pictureBox2.Click += new EventHandler(this.pictureBox2_Click);
			base.AutoScaleDimensions = new SizeF(6f, 12f);
			base.AutoScaleMode = AutoScaleMode.Font;
			this.BackColor = Color.FromArgb(227, 239, 255);
			this.BackgroundImage = Resources._1;
			base.ClientSize = new Size(268, 162);
			base.Controls.Add(this.pictureBox2);
			base.Controls.Add(this.btnAnswer);
			base.Controls.Add(this.lblInfo);
			base.Controls.Add(this.pictureBox1);
			base.Controls.Add(this.btnHangup);
			base.FormBorderStyle = FormBorderStyle.None;
			base.Icon = (Icon)resources.GetObject("$this.Icon");
			base.Name = "FrmTip";
			base.ShowInTaskbar = false;
			this.Text = "FrmTip";
			base.Load += new EventHandler(this.FrmTip_Load);
			base.MouseDown += new MouseEventHandler(this.FrmTip_MouseDown);
			((ISupportInitialize)this.pictureBox1).EndInit();
			((ISupportInitialize)this.pictureBox2).EndInit();
			base.ResumeLayout(false);
			base.PerformLayout();
		}
	}
}
