using Microsoft.VisualBasic.PowerPacks;
using PLAgentDll;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace PLAgentBar
{
	public class FrmMonitorScreen : Form
	{
		private string mMyAgentNum;

		private AgentBar agentBar1;

		private int agent_idle_amount = 0;

		private int agent_busy_amount = 0;

		private int agent_talking_amount = 0;

		private int agent_leave_amount = 0;

		private int agent_online_amount = 0;

		private int queue_wait_people_amount = 0;

		private int max_wait_time_amount = 0;

		private IContainer components = null;

		private TableLayoutPanel tableLayoutPanel1;

		private Label lblCallin;

		private Label lblEnterQueue;

		private Label lblCallinAnswerPer;

		private Label lblCallinAnswer;

		private Panel panel10;

		private Panel panel8;

		private Panel panel6;

		private Panel panel5;

		private Panel panel4;

		private Panel panel3;

		private Panel panel2;

		private Panel panel1;

		private Label lblCustomersOfQueue;

		private Label lblCallAbandon;

		private Label lblMaxWaitTime;

		private Label lblLeaves;

		private Label lblBusys;

		private Label lblTalks;

		private Label lblIdles;

		private Label lblAgents;

		private Panel panel9;

		private Label lblCalloutAnswer;

		private Panel panel7;

		private Label lblCallOut;

		private Timer tmrGetReportStatisInfo;

		private ShapeContainer shapeContainer1;

		private LineShape lineShape1;

		private ShapeContainer shapeContainer2;

		private LineShape lineShape2;

		private Timer tmrWaitTime;

		public string MyAgentNum
		{
			set
			{
				this.mMyAgentNum = value;
			}
		}

		public AgentBar AgentBar
		{
			set
			{
				this.agentBar1 = value;
			}
		}

		public FrmMonitorScreen()
		{
			this.InitializeComponent();
		}

		public void Evt_Get_All_Queue_Statis(string current_time, List<Queue_Statis_Struct> queue_statis_lst)
		{
			int agent_idle = 0;
			int agent_busy = 0;
			int agent_talking = 0;
			int agent_leave = 0;
			int agent_online = 0;
			int agent_hold = 0;
			int agent_mute = 0;
			int wait_people = 0;
			this.agent_idle_amount = 0;
			this.agent_busy_amount = 0;
			this.agent_talking_amount = 0;
			this.agent_leave_amount = 0;
			this.agent_online_amount = 0;
			this.queue_wait_people_amount = 0;
			this.max_wait_time_amount = 0;
			if (!string.IsNullOrEmpty(queue_statis_lst[0].agent_idle_amount))
			{
				agent_idle = Convert.ToInt32(queue_statis_lst[0].agent_idle_amount);
			}
			if (!string.IsNullOrEmpty(queue_statis_lst[0].agent_busy_amount))
			{
				agent_busy = Convert.ToInt32(queue_statis_lst[0].agent_busy_amount);
			}
			if (!string.IsNullOrEmpty(queue_statis_lst[0].agent_talking_amount))
			{
				agent_talking = Convert.ToInt32(queue_statis_lst[0].agent_talking_amount);
			}
			if (!string.IsNullOrEmpty(queue_statis_lst[0].agent_leave_amount))
			{
				agent_leave = Convert.ToInt32(queue_statis_lst[0].agent_leave_amount);
			}
			if (!string.IsNullOrEmpty(queue_statis_lst[0].agent_online_amount))
			{
				agent_online = Convert.ToInt32(queue_statis_lst[0].agent_online_amount);
			}
			if (!string.IsNullOrEmpty(queue_statis_lst[0].agent_callout_amount))
			{
				int agent_callout = Convert.ToInt32(queue_statis_lst[0].agent_callout_amount);
			}
			if (!string.IsNullOrEmpty(queue_statis_lst[0].agent_hold_amount))
			{
				agent_hold = Convert.ToInt32(queue_statis_lst[0].agent_hold_amount);
			}
			if (!string.IsNullOrEmpty(queue_statis_lst[0].agent_mute_amount))
			{
				agent_mute = Convert.ToInt32(queue_statis_lst[0].agent_mute_amount);
			}
			if (!string.IsNullOrEmpty(queue_statis_lst[0].agent_no_exten_amount))
			{
				int agent_no_exten = Convert.ToInt32(queue_statis_lst[0].agent_no_exten_amount);
			}
			if (!string.IsNullOrEmpty(queue_statis_lst[0].agent_post_treatment))
			{
				int agent_post_treatment = Convert.ToInt32(queue_statis_lst[0].agent_post_treatment);
			}
			if (!string.IsNullOrEmpty(queue_statis_lst[0].agent_ring_amount))
			{
				int agent_ring = Convert.ToInt32(queue_statis_lst[0].agent_ring_amount);
			}
			if (!string.IsNullOrEmpty(queue_statis_lst[0].queue_wait_people_amount))
			{
				wait_people = Convert.ToInt32(queue_statis_lst[0].queue_wait_people_amount);
			}
			this.update_info_real_time(agent_online, agent_idle, agent_busy, agent_talking + agent_hold + agent_mute, agent_leave, wait_people, ComFunc.get_time_interval_sec(current_time, queue_statis_lst[0].early_queue_enter_time));
		}

		private void init_UI()
		{
			this.lblAgents.Text = "座席数：0";
			this.lblIdles.Text = "空  闲：0";
			this.lblBusys.Text = "忙  碌：0";
			this.lblTalks.Text = "通话中：0";
			this.lblLeaves.Text = "离  开：0";
			this.lblCustomersOfQueue.Text = "当前排队数：0";
			this.lblMaxWaitTime.Text = "排队最长等待时间：0秒";
			this.lblCallin.Text = "呼入总量：0";
			this.lblCallinAnswer.Text = "呼入接通量：0";
			this.lblCallOut.Text = "外呼总量：0";
			this.lblCalloutAnswer.Text = "外呼接通量：0";
			this.lblCallAbandon.Text = "呼叫放弃量：0";
			this.lblEnterQueue.Text = "入队量：0";
			this.lblCallinAnswerPer.Text = "呼入接通率：0.00%";
		}

		private void update_info_real_time(int agent_online, int agent_idle, int agent_busy, int agent_talking, int agent_leave, int wai_people, int max_wait_time)
		{
			if (agent_online != 0)
			{
				this.agent_online_amount += agent_online;
				this.lblAgents.Text = "坐席数：" + this.agent_online_amount;
			}
			if (agent_idle != 0)
			{
				this.agent_idle_amount += agent_idle;
				this.lblIdles.Text = "空  闲：" + this.agent_idle_amount;
			}
			if (agent_busy != 0)
			{
				this.agent_busy_amount += agent_busy;
				this.lblBusys.Text = "忙  碌：" + this.agent_busy_amount;
			}
			if (agent_talking != 0)
			{
				this.agent_talking_amount += agent_talking;
				this.lblTalks.Text = "通话中：" + this.agent_talking_amount;
			}
			if (agent_leave != 0)
			{
				this.agent_leave_amount += agent_leave;
				this.lblLeaves.Text = "离  开：" + this.agent_leave_amount;
			}
			if (wai_people != 0)
			{
				this.queue_wait_people_amount += wai_people;
				this.lblCustomersOfQueue.Text = "当前排队数：" + this.queue_wait_people_amount;
				if (this.queue_wait_people_amount != 0)
				{
					this.lblCustomersOfQueue.ForeColor = Color.Red;
				}
				else
				{
					this.lblCustomersOfQueue.ForeColor = Color.Black;
				}
				if (this.queue_wait_people_amount == 0)
				{
					this.max_wait_time_amount = 0;
					this.lblMaxWaitTime.Text = "排队最长等待时间：0秒";
				}
			}
			if (max_wait_time != 0 && this.queue_wait_people_amount != 0)
			{
				this.max_wait_time_amount += max_wait_time;
				this.lblMaxWaitTime.Text = "排队最长等待时间：" + this.max_wait_time_amount + "秒";
			}
		}

		public void Evt_Agent_Status_Change(string status_change_agent_num, string status_change_before, string status_change_after, bool is_bind_exten, string customer_enter_channle, string current_time, string start_talking_time)
		{
			if (is_bind_exten)
			{
				AgentBar.Agent_Status agent_status_before = AgentBar.Str2AgentStatus(status_change_before);
				if (AgentBar.ChkIsTalking(status_change_before) == 0)
				{
					this.update_info_real_time(0, 0, 0, -1, 0, 0, 0);
				}
				else if (agent_status_before == AgentBar.Agent_Status.AGENT_STATUS_IDLE)
				{
					this.update_info_real_time(0, -1, 0, 0, 0, 0, 0);
				}
				else if (agent_status_before == AgentBar.Agent_Status.AGENT_STATUS_BUSY)
				{
					this.update_info_real_time(0, 0, -1, 0, 0, 0, 0);
				}
				else if (agent_status_before == AgentBar.Agent_Status.AGENT_STATUS_LEAVE)
				{
					this.update_info_real_time(0, 0, 0, 0, -1, 0, 0);
				}
				else if (agent_status_before == AgentBar.Agent_Status.AGENT_STATUS_OFFLINE)
				{
					this.update_info_real_time(1, 0, 0, 0, 0, 0, 0);
				}
				else if (Convert.ToInt32(agent_status_before) >= 100)
				{
					this.update_info_real_time(0, 0, 0, 0, -1, 0, 0);
				}
				AgentBar.Agent_Status agent_status_after = AgentBar.Str2AgentStatus(status_change_after);
				if (AgentBar.ChkIsTalking(status_change_after) == 0)
				{
					this.update_info_real_time(0, 0, 0, 1, 0, 0, 0);
				}
				else if (agent_status_after == AgentBar.Agent_Status.AGENT_STATUS_IDLE)
				{
					this.update_info_real_time(0, 1, 0, 0, 0, 0, 0);
				}
				else if (agent_status_after == AgentBar.Agent_Status.AGENT_STATUS_BUSY)
				{
					this.update_info_real_time(0, 0, 1, 0, 0, 0, 0);
				}
				else if (agent_status_after == AgentBar.Agent_Status.AGENT_STATUS_LEAVE)
				{
					this.update_info_real_time(0, 0, 0, 0, 1, 0, 0);
				}
				else if (agent_status_after == AgentBar.Agent_Status.AGENT_STATUS_OFFLINE)
				{
					this.update_info_real_time(-1, 0, 0, 0, 0, 0, 0);
				}
				else if (Convert.ToInt32(agent_status_after) >= 100)
				{
					this.update_info_real_time(0, 0, 0, 0, 1, 0, 0);
				}
			}
		}

		public void Evt_Add_Customer_To_Queue(string call_type, string callcenter_num, string customer_num, string customer_status, string customer_type, string customer_uuid, string enter_queue_time, string exclusive_agent_num, string exclusive_queue_num, string queue_num, string current_time, string queue_name, string queue_customer_amount, string early_queue_enter_time, string early_queue_enter_time_all, string customer_enter_channel)
		{
			if (this.queue_wait_people_amount == 0)
			{
				this.max_wait_time_amount = ComFunc.get_time_interval_sec(current_time, early_queue_enter_time_all);
				this.update_info_real_time(0, 0, 0, 0, 0, 1, 1);
			}
			else
			{
				this.update_info_real_time(0, 0, 0, 0, 0, 1, 0);
			}
		}

		public void Evt_Del_Customer_From_Queue(string customer_uuid, string queue_num, string current_time, string early_queue_enter_time, string early_queue_enter_time_all, string reason)
		{
			int wait_time = ComFunc.get_time_interval_sec(current_time, early_queue_enter_time_all);
			this.max_wait_time_amount = wait_time;
			this.update_info_real_time(0, 0, 0, 0, 0, -1, 1);
		}

		public void Evt_GetReportStatisInfoEvent(string agentID, int retCode, string reason, Dictionary<string, string> report_statis_info)
		{
			string strInQueue = "0";
			string strCallinAnswer = "0";
			if (report_statis_info.ContainsKey("todaycallin"))
			{
				this.lblCallin.Text = "呼入总量：" + report_statis_info["todaycallin"];
			}
			else
			{
				this.lblCallin.Text = "呼入总量：NULL";
			}
			if (report_statis_info.ContainsKey("todaycallinanswer"))
			{
				strCallinAnswer = report_statis_info["todaycallinanswer"];
				this.lblCallinAnswer.Text = "呼入接通量：" + strCallinAnswer;
			}
			else
			{
				this.lblCallinAnswer.Text = "呼入接通量：NULL";
			}
			if (report_statis_info.ContainsKey("todaycallout"))
			{
				this.lblCallOut.Text = "外呼总量：" + report_statis_info["todaycallout"];
			}
			else
			{
				this.lblCallOut.Text = "外呼总量：NULL";
			}
			if (report_statis_info.ContainsKey("todaycalloutanswer"))
			{
				this.lblCalloutAnswer.Text = "外呼接通量：" + report_statis_info["todaycalloutanswer"];
			}
			else
			{
				this.lblCalloutAnswer.Text = "外呼接通量：NULL";
			}
			if (report_statis_info.ContainsKey("todayabandon"))
			{
				this.lblCallAbandon.Text = "呼叫放弃量：" + report_statis_info["todayabandon"];
			}
			else
			{
				this.lblCallAbandon.Text = "呼叫放弃量：NULL";
			}
			if (report_statis_info.ContainsKey("todayenterqueue") && report_statis_info.ContainsKey("todayenterqueueother"))
			{
				int todayenterqueue = Convert.ToInt32(report_statis_info["todayenterqueue"]);
				int todayenterqueueother = Convert.ToInt32(report_statis_info["todayenterqueueother"]);
				strInQueue = (todayenterqueue + todayenterqueueother).ToString();
				this.lblEnterQueue.Text = "入队量：" + strInQueue;
			}
			else
			{
				this.lblEnterQueue.Text = "入队量：NULL";
			}
			if (strInQueue == "" || strInQueue == "0" || strCallinAnswer == "" || strCallinAnswer == "0")
			{
				this.lblCallinAnswerPer.Text = "呼入接通率：0.00%";
			}
			else
			{
				double callinAnswerPer = Convert.ToDouble(strCallinAnswer) / Convert.ToDouble(strInQueue) * 100.0;
				this.lblCallinAnswerPer.Text = "呼入接通率：" + Convert.ToDouble(callinAnswerPer).ToString("0.00") + "%";
			}
		}

		private void FrmMonitorScreen_Load(object sender, EventArgs e)
		{
			base.WindowState = FormWindowState.Maximized;
			this.lineShape1.BringToFront();
			this.lineShape2.BringToFront();
			this.tmrGetReportStatisInfo.Enabled = true;
			this.tmrGetReportStatisInfo.Interval = this.agentBar1.RefreshReportStatisInterval * 1000;
			this.tmrGetReportStatisInfo.Start();
			this.tmrWaitTime.Enabled = true;
			this.tmrWaitTime.Interval = 1000;
			this.tmrWaitTime.Start();
			this.init_UI();
		}

		private void tmrGetReportStatisInfo_Tick(object sender, EventArgs e)
		{
			this.agentBar1.DoGetReportStatisInfo();
		}

		private void FrmMonitorScreen_Resize(object sender, EventArgs e)
		{
			this.lineShape1.BringToFront();
			this.lineShape1.Y1 = this.lineShape1.Y2;
			this.lineShape2.BringToFront();
			this.lineShape2.Y1 = this.lineShape2.Y2;
		}

		private void tmrWaitTime_Tick(object sender, EventArgs e)
		{
			if (this.queue_wait_people_amount > 0)
			{
				this.update_info_real_time(0, 0, 0, 0, 0, 0, 1);
			}
			else
			{
				this.max_wait_time_amount = 0;
				this.lblMaxWaitTime.Text = "排队最长等待时间：0秒";
			}
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
			this.tableLayoutPanel1 = new TableLayoutPanel();
			this.panel9 = new Panel();
			this.lblCalloutAnswer = new Label();
			this.panel7 = new Panel();
			this.lblCallOut = new Label();
			this.panel10 = new Panel();
			this.lblLeaves = new Label();
			this.panel8 = new Panel();
			this.lblBusys = new Label();
			this.panel6 = new Panel();
			this.lblTalks = new Label();
			this.panel5 = new Panel();
			this.lblCallAbandon = new Label();
			this.panel4 = new Panel();
			this.lblIdles = new Label();
			this.panel3 = new Panel();
			this.lblMaxWaitTime = new Label();
			this.panel2 = new Panel();
			this.lblAgents = new Label();
			this.shapeContainer2 = new ShapeContainer();
			this.lineShape2 = new LineShape();
			this.lblCallinAnswerPer = new Label();
			this.lblCallinAnswer = new Label();
			this.lblEnterQueue = new Label();
			this.lblCallin = new Label();
			this.panel1 = new Panel();
			this.lblCustomersOfQueue = new Label();
			this.shapeContainer1 = new ShapeContainer();
			this.lineShape1 = new LineShape();
			this.tmrGetReportStatisInfo = new Timer(this.components);
			this.tmrWaitTime = new Timer(this.components);
			this.tableLayoutPanel1.SuspendLayout();
			this.panel9.SuspendLayout();
			this.panel7.SuspendLayout();
			this.panel10.SuspendLayout();
			this.panel8.SuspendLayout();
			this.panel6.SuspendLayout();
			this.panel5.SuspendLayout();
			this.panel4.SuspendLayout();
			this.panel3.SuspendLayout();
			this.panel2.SuspendLayout();
			this.panel1.SuspendLayout();
			base.SuspendLayout();
			this.tableLayoutPanel1.Anchor = (AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right);
			this.tableLayoutPanel1.AutoSizeMode = AutoSizeMode.GrowAndShrink;
			this.tableLayoutPanel1.BackColor = Color.Transparent;
			this.tableLayoutPanel1.ColumnCount = 4;
			this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25f));
			this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25f));
			this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25f));
			this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25f));
			this.tableLayoutPanel1.Controls.Add(this.panel9, 0, 5);
			this.tableLayoutPanel1.Controls.Add(this.panel7, 0, 4);
			this.tableLayoutPanel1.Controls.Add(this.panel10, 2, 5);
			this.tableLayoutPanel1.Controls.Add(this.panel8, 2, 4);
			this.tableLayoutPanel1.Controls.Add(this.panel6, 2, 3);
			this.tableLayoutPanel1.Controls.Add(this.panel5, 0, 3);
			this.tableLayoutPanel1.Controls.Add(this.panel4, 2, 2);
			this.tableLayoutPanel1.Controls.Add(this.panel3, 0, 2);
			this.tableLayoutPanel1.Controls.Add(this.panel2, 2, 1);
			this.tableLayoutPanel1.Controls.Add(this.lblCallinAnswerPer, 3, 0);
			this.tableLayoutPanel1.Controls.Add(this.lblCallinAnswer, 2, 0);
			this.tableLayoutPanel1.Controls.Add(this.lblEnterQueue, 1, 0);
			this.tableLayoutPanel1.Controls.Add(this.lblCallin, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 1);
			this.tableLayoutPanel1.Location = new Point(149, 90);
			this.tableLayoutPanel1.Margin = new Padding(0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 6;
			this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 20f));
			this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 16f));
			this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 16f));
			this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 16f));
			this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 16f));
			this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 16f));
			this.tableLayoutPanel1.Size = new Size(665, 569);
			this.tableLayoutPanel1.TabIndex = 1;
			this.tableLayoutPanel1.SetColumnSpan(this.panel9, 2);
			this.panel9.Controls.Add(this.lblCalloutAnswer);
			this.panel9.Dock = DockStyle.Fill;
			this.panel9.Location = new Point(3, 480);
			this.panel9.Name = "panel9";
			this.panel9.Size = new Size(326, 86);
			this.panel9.TabIndex = 15;
			this.lblCalloutAnswer.Anchor = AnchorStyles.None;
			this.lblCalloutAnswer.AutoSize = true;
			this.lblCalloutAnswer.Font = new Font("黑体", 18f, FontStyle.Bold, GraphicsUnit.Point, 134);
			this.lblCalloutAnswer.Location = new Point(87, 32);
			this.lblCalloutAnswer.Name = "lblCalloutAnswer";
			this.lblCalloutAnswer.Size = new Size(186, 24);
			this.lblCalloutAnswer.TabIndex = 2;
			this.lblCalloutAnswer.Text = "外呼接通量：12";
			this.lblCalloutAnswer.TextAlign = ContentAlignment.MiddleCenter;
			this.tableLayoutPanel1.SetColumnSpan(this.panel7, 2);
			this.panel7.Controls.Add(this.lblCallOut);
			this.panel7.Dock = DockStyle.Fill;
			this.panel7.Location = new Point(3, 389);
			this.panel7.Name = "panel7";
			this.panel7.Size = new Size(326, 85);
			this.panel7.TabIndex = 14;
			this.lblCallOut.Anchor = AnchorStyles.None;
			this.lblCallOut.AutoSize = true;
			this.lblCallOut.Font = new Font("黑体", 18f, FontStyle.Bold, GraphicsUnit.Point, 134);
			this.lblCallOut.Location = new Point(112, 32);
			this.lblCallOut.Name = "lblCallOut";
			this.lblCallOut.Size = new Size(161, 24);
			this.lblCallOut.TabIndex = 2;
			this.lblCallOut.Text = "外呼总量：12";
			this.lblCallOut.TextAlign = ContentAlignment.MiddleCenter;
			this.tableLayoutPanel1.SetColumnSpan(this.panel10, 2);
			this.panel10.Controls.Add(this.lblLeaves);
			this.panel10.Dock = DockStyle.Fill;
			this.panel10.Location = new Point(335, 480);
			this.panel10.Name = "panel10";
			this.panel10.Size = new Size(327, 86);
			this.panel10.TabIndex = 13;
			this.lblLeaves.Anchor = AnchorStyles.None;
			this.lblLeaves.AutoSize = true;
			this.lblLeaves.Font = new Font("黑体", 18f, FontStyle.Bold, GraphicsUnit.Point, 134);
			this.lblLeaves.Location = new Point(101, 32);
			this.lblLeaves.Name = "lblLeaves";
			this.lblLeaves.Size = new Size(137, 24);
			this.lblLeaves.TabIndex = 3;
			this.lblLeaves.Text = "离  开：12";
			this.lblLeaves.TextAlign = ContentAlignment.MiddleCenter;
			this.tableLayoutPanel1.SetColumnSpan(this.panel8, 2);
			this.panel8.Controls.Add(this.lblBusys);
			this.panel8.Dock = DockStyle.Fill;
			this.panel8.Location = new Point(335, 389);
			this.panel8.Name = "panel8";
			this.panel8.Size = new Size(327, 85);
			this.panel8.TabIndex = 11;
			this.lblBusys.Anchor = AnchorStyles.None;
			this.lblBusys.AutoSize = true;
			this.lblBusys.Font = new Font("黑体", 18f, FontStyle.Bold, GraphicsUnit.Point, 134);
			this.lblBusys.Location = new Point(101, 32);
			this.lblBusys.Name = "lblBusys";
			this.lblBusys.Size = new Size(137, 24);
			this.lblBusys.TabIndex = 3;
			this.lblBusys.Text = "忙  碌：12";
			this.lblBusys.TextAlign = ContentAlignment.MiddleCenter;
			this.tableLayoutPanel1.SetColumnSpan(this.panel6, 2);
			this.panel6.Controls.Add(this.lblTalks);
			this.panel6.Dock = DockStyle.Fill;
			this.panel6.Location = new Point(335, 298);
			this.panel6.Name = "panel6";
			this.panel6.Size = new Size(327, 85);
			this.panel6.TabIndex = 9;
			this.lblTalks.Anchor = AnchorStyles.None;
			this.lblTalks.AutoSize = true;
			this.lblTalks.Font = new Font("黑体", 18f, FontStyle.Bold, GraphicsUnit.Point, 134);
			this.lblTalks.Location = new Point(101, 32);
			this.lblTalks.Name = "lblTalks";
			this.lblTalks.Size = new Size(136, 24);
			this.lblTalks.TabIndex = 3;
			this.lblTalks.Text = "通话中：12";
			this.lblTalks.TextAlign = ContentAlignment.MiddleCenter;
			this.tableLayoutPanel1.SetColumnSpan(this.panel5, 2);
			this.panel5.Controls.Add(this.lblCallAbandon);
			this.panel5.Dock = DockStyle.Fill;
			this.panel5.Location = new Point(3, 298);
			this.panel5.Name = "panel5";
			this.panel5.Size = new Size(326, 85);
			this.panel5.TabIndex = 8;
			this.lblCallAbandon.Anchor = AnchorStyles.None;
			this.lblCallAbandon.AutoSize = true;
			this.lblCallAbandon.Font = new Font("黑体", 18f, FontStyle.Bold, GraphicsUnit.Point, 134);
			this.lblCallAbandon.Location = new Point(87, 32);
			this.lblCallAbandon.Name = "lblCallAbandon";
			this.lblCallAbandon.Size = new Size(186, 24);
			this.lblCallAbandon.TabIndex = 2;
			this.lblCallAbandon.Text = "呼叫放弃量：12";
			this.lblCallAbandon.TextAlign = ContentAlignment.MiddleCenter;
			this.tableLayoutPanel1.SetColumnSpan(this.panel4, 2);
			this.panel4.Controls.Add(this.lblIdles);
			this.panel4.Dock = DockStyle.Fill;
			this.panel4.Location = new Point(335, 207);
			this.panel4.Name = "panel4";
			this.panel4.Size = new Size(327, 85);
			this.panel4.TabIndex = 7;
			this.lblIdles.Anchor = AnchorStyles.None;
			this.lblIdles.AutoSize = true;
			this.lblIdles.Font = new Font("黑体", 18f, FontStyle.Bold, GraphicsUnit.Point, 134);
			this.lblIdles.Location = new Point(101, 31);
			this.lblIdles.Name = "lblIdles";
			this.lblIdles.Size = new Size(137, 24);
			this.lblIdles.TabIndex = 3;
			this.lblIdles.Text = "空  闲：12";
			this.lblIdles.TextAlign = ContentAlignment.MiddleCenter;
			this.tableLayoutPanel1.SetColumnSpan(this.panel3, 2);
			this.panel3.Controls.Add(this.lblMaxWaitTime);
			this.panel3.Dock = DockStyle.Fill;
			this.panel3.Location = new Point(3, 207);
			this.panel3.Name = "panel3";
			this.panel3.Size = new Size(326, 85);
			this.panel3.TabIndex = 6;
			this.lblMaxWaitTime.Anchor = AnchorStyles.None;
			this.lblMaxWaitTime.AutoSize = true;
			this.lblMaxWaitTime.Font = new Font("黑体", 18f, FontStyle.Bold, GraphicsUnit.Point, 134);
			this.lblMaxWaitTime.Location = new Point(12, 31);
			this.lblMaxWaitTime.Name = "lblMaxWaitTime";
			this.lblMaxWaitTime.Size = new Size(261, 24);
			this.lblMaxWaitTime.TabIndex = 2;
			this.lblMaxWaitTime.Text = "排队最长等待时间：12";
			this.lblMaxWaitTime.TextAlign = ContentAlignment.MiddleCenter;
			this.tableLayoutPanel1.SetColumnSpan(this.panel2, 2);
			this.panel2.Controls.Add(this.lblAgents);
			this.panel2.Controls.Add(this.shapeContainer2);
			this.panel2.Dock = DockStyle.Fill;
			this.panel2.Location = new Point(335, 116);
			this.panel2.Name = "panel2";
			this.panel2.Size = new Size(327, 85);
			this.panel2.TabIndex = 5;
			this.lblAgents.Anchor = AnchorStyles.None;
			this.lblAgents.AutoSize = true;
			this.lblAgents.Font = new Font("黑体", 18f, FontStyle.Bold, GraphicsUnit.Point, 134);
			this.lblAgents.Location = new Point(101, 34);
			this.lblAgents.Name = "lblAgents";
			this.lblAgents.Size = new Size(136, 24);
			this.lblAgents.TabIndex = 3;
			this.lblAgents.Text = "坐席数：12";
			this.lblAgents.TextAlign = ContentAlignment.MiddleCenter;
			this.shapeContainer2.Location = new Point(0, 0);
			this.shapeContainer2.Margin = new Padding(0);
			this.shapeContainer2.Name = "shapeContainer2";
			this.shapeContainer2.Shapes.AddRange(new Shape[]
			{
				this.lineShape2
			});
			this.shapeContainer2.Size = new Size(327, 85);
			this.shapeContainer2.TabIndex = 4;
			this.shapeContainer2.TabStop = false;
			this.lineShape2.Anchor = (AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right);
			this.lineShape2.BorderWidth = 3;
			this.lineShape2.Name = "lineShape2";
			this.lineShape2.X1 = 0;
			this.lineShape2.X2 = 341;
			this.lineShape2.Y1 = 6;
			this.lineShape2.Y2 = 6;
			this.lblCallinAnswerPer.AutoSize = true;
			this.lblCallinAnswerPer.Dock = DockStyle.Fill;
			this.lblCallinAnswerPer.Font = new Font("黑体", 18f, FontStyle.Bold, GraphicsUnit.Point, 134);
			this.lblCallinAnswerPer.Location = new Point(501, 0);
			this.lblCallinAnswerPer.Name = "lblCallinAnswerPer";
			this.lblCallinAnswerPer.Size = new Size(161, 113);
			this.lblCallinAnswerPer.TabIndex = 3;
			this.lblCallinAnswerPer.Text = "呼入接通率：100%";
			this.lblCallinAnswerPer.TextAlign = ContentAlignment.MiddleCenter;
			this.lblCallinAnswer.AutoSize = true;
			this.lblCallinAnswer.Dock = DockStyle.Fill;
			this.lblCallinAnswer.Font = new Font("黑体", 18f, FontStyle.Bold, GraphicsUnit.Point, 134);
			this.lblCallinAnswer.Location = new Point(335, 0);
			this.lblCallinAnswer.Name = "lblCallinAnswer";
			this.lblCallinAnswer.Size = new Size(160, 113);
			this.lblCallinAnswer.TabIndex = 2;
			this.lblCallinAnswer.Text = "呼入接通量：125";
			this.lblCallinAnswer.TextAlign = ContentAlignment.MiddleCenter;
			this.lblEnterQueue.AutoSize = true;
			this.lblEnterQueue.Dock = DockStyle.Fill;
			this.lblEnterQueue.Font = new Font("黑体", 18f, FontStyle.Bold, GraphicsUnit.Point, 134);
			this.lblEnterQueue.Location = new Point(169, 0);
			this.lblEnterQueue.Name = "lblEnterQueue";
			this.lblEnterQueue.Size = new Size(160, 113);
			this.lblEnterQueue.TabIndex = 1;
			this.lblEnterQueue.Text = "入队量：125";
			this.lblEnterQueue.TextAlign = ContentAlignment.MiddleCenter;
			this.lblCallin.AutoSize = true;
			this.lblCallin.Dock = DockStyle.Fill;
			this.lblCallin.Font = new Font("黑体", 18f, FontStyle.Bold, GraphicsUnit.Point, 134);
			this.lblCallin.Location = new Point(3, 0);
			this.lblCallin.Name = "lblCallin";
			this.lblCallin.Size = new Size(160, 113);
			this.lblCallin.TabIndex = 0;
			this.lblCallin.Text = "呼入总量：12";
			this.lblCallin.TextAlign = ContentAlignment.MiddleCenter;
			this.tableLayoutPanel1.SetColumnSpan(this.panel1, 2);
			this.panel1.Controls.Add(this.lblCustomersOfQueue);
			this.panel1.Controls.Add(this.shapeContainer1);
			this.panel1.Dock = DockStyle.Fill;
			this.panel1.Location = new Point(3, 116);
			this.panel1.Name = "panel1";
			this.panel1.Size = new Size(326, 85);
			this.panel1.TabIndex = 4;
			this.lblCustomersOfQueue.Anchor = AnchorStyles.None;
			this.lblCustomersOfQueue.AutoSize = true;
			this.lblCustomersOfQueue.Font = new Font("黑体", 18f, FontStyle.Bold, GraphicsUnit.Point, 134);
			this.lblCustomersOfQueue.Location = new Point(87, 33);
			this.lblCustomersOfQueue.Name = "lblCustomersOfQueue";
			this.lblCustomersOfQueue.Size = new Size(186, 24);
			this.lblCustomersOfQueue.TabIndex = 1;
			this.lblCustomersOfQueue.Text = "当前排队数：12";
			this.lblCustomersOfQueue.TextAlign = ContentAlignment.MiddleCenter;
			this.shapeContainer1.Location = new Point(0, 0);
			this.shapeContainer1.Margin = new Padding(0);
			this.shapeContainer1.Name = "shapeContainer1";
			this.shapeContainer1.Shapes.AddRange(new Shape[]
			{
				this.lineShape1
			});
			this.shapeContainer1.Size = new Size(326, 85);
			this.shapeContainer1.TabIndex = 2;
			this.shapeContainer1.TabStop = false;
			this.lineShape1.Anchor = (AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right);
			this.lineShape1.BorderWidth = 3;
			this.lineShape1.Name = "lineShape1";
			this.lineShape1.X1 = 3;
			this.lineShape1.X2 = 340;
			this.lineShape1.Y1 = 6;
			this.lineShape1.Y2 = 6;
			this.tmrGetReportStatisInfo.Tick += new EventHandler(this.tmrGetReportStatisInfo_Tick);
			this.tmrWaitTime.Tick += new EventHandler(this.tmrWaitTime_Tick);
			base.AutoScaleDimensions = new SizeF(6f, 12f);
			base.AutoScaleMode = AutoScaleMode.Font;
			this.BackColor = Color.White;
			base.ClientSize = new Size(980, 718);
			base.Controls.Add(this.tableLayoutPanel1);
			base.Name = "FrmMonitorScreen";
			this.Text = "监控屏";
			base.Load += new EventHandler(this.FrmMonitorScreen_Load);
			base.Resize += new EventHandler(this.FrmMonitorScreen_Resize);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			this.panel9.ResumeLayout(false);
			this.panel9.PerformLayout();
			this.panel7.ResumeLayout(false);
			this.panel7.PerformLayout();
			this.panel10.ResumeLayout(false);
			this.panel10.PerformLayout();
			this.panel8.ResumeLayout(false);
			this.panel8.PerformLayout();
			this.panel6.ResumeLayout(false);
			this.panel6.PerformLayout();
			this.panel5.ResumeLayout(false);
			this.panel5.PerformLayout();
			this.panel4.ResumeLayout(false);
			this.panel4.PerformLayout();
			this.panel3.ResumeLayout(false);
			this.panel3.PerformLayout();
			this.panel2.ResumeLayout(false);
			this.panel2.PerformLayout();
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			base.ResumeLayout(false);
		}
	}
}
