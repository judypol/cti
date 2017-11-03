namespace DemoCti
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.agentBar1 = new PLAgentBar.AgentBar();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(356, 141);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "Single_Out";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(506, 141);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 1;
            this.button2.Text = "Single_in";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(621, 141);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 1;
            this.button3.Text = "拨打";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // agentBar1
            // 
            this.agentBar1.AgentExten = null;
            this.agentBar1.AgentID = null;
            this.agentBar1.AgentName = null;
            this.agentBar1.AgentPwd = null;
            this.agentBar1.AgentState = PLAgentBar.AgentBar.Agent_State.AGENT_STATUS_UNKNOWN;
            this.agentBar1.AgentStateAfterHangup = PLAgentBar.AgentBar.Agent_Status.AGENT_STATUS_ACW;
            this.agentBar1.AgentStatus = null;
            this.agentBar1.ApplyChangeStatusApprovalHistory = ((System.Collections.Generic.List<PLAgentDll.Apply_Change_Status>)(resources.GetObject("agentBar1.ApplyChangeStatusApprovalHistory")));
            this.agentBar1.AutoSize = true;
            this.agentBar1.BackColor = System.Drawing.Color.Transparent;
            this.agentBar1.BindExten = false;
            this.agentBar1.CalloutHistory = ((System.Collections.Generic.List<string>)(resources.GetObject("agentBar1.CalloutHistory")));
            this.agentBar1.CuID = "";
            this.agentBar1.DefaultAccessNum = null;
            this.agentBar1.EavesDropAgentNum = null;
            this.agentBar1.ExtenIsOutbound = false;
            this.agentBar1.ExtenMode = null;
            this.agentBar1.GradeSwitch = false;
            this.agentBar1.HeartBeatTimeout = 30;
            this.agentBar1.HotKeyBusy = null;
            this.agentBar1.hotKeyCallOut = null;
            this.agentBar1.HotKeyIdle = null;
            this.agentBar1.HotKeyLeave = null;
            this.agentBar1.hotKeyMonitor = null;
            this.agentBar1.IsMonitorOfflineAgent = false;
            this.agentBar1.Location = new System.Drawing.Point(-1, 1);
            this.agentBar1.Name = "agentBar1";
            this.agentBar1.NoAnswerCallsURL = "";
            this.agentBar1.RefreshReportStatisInterval = 60;
            this.agentBar1.SaltKey = null;
            this.agentBar1.ServerIP = null;
            this.agentBar1.ServerPort = 0;
            this.agentBar1.SigninIntervalAfterSoftPhoneRegisted = 3000;
            this.agentBar1.SipAutoAnswer = false;
            this.agentBar1.SipAutoSignIn = false;
            this.agentBar1.SipLocalNum = null;
            this.agentBar1.SipNum = null;
            this.agentBar1.SipPhoneOnLineWarning = false;
            this.agentBar1.SipPort = 0;
            this.agentBar1.SipPwd = null;
            this.agentBar1.SipRegistTime = 0;
            this.agentBar1.SipServer = null;
            this.agentBar1.Size = new System.Drawing.Size(1110, 39);
            this.agentBar1.SoftPhone_Dtmf_0_Cmd = 200;
            this.agentBar1.SoftPhone_dtmf_1_cmd = 201;
            this.agentBar1.SoftPhone_dtmf_2_cmd = 202;
            this.agentBar1.SoftPhone_dtmf_3_cmd = 203;
            this.agentBar1.SoftPhone_dtmf_4_cmd = 204;
            this.agentBar1.SoftPhone_dtmf_5_cmd = 205;
            this.agentBar1.SoftPhone_dtmf_6_cmd = 206;
            this.agentBar1.SoftPhone_dtmf_7_cmd = 207;
            this.agentBar1.SoftPhone_dtmf_8_cmd = 208;
            this.agentBar1.SoftPhone_dtmf_9_cmd = 209;
            this.agentBar1.SoftPhone_dtmf_pound_cmd = 211;
            this.agentBar1.SoftPhone_dtmf_star_cmd = 210;
            this.agentBar1.SoftPhoneAnswerCmd = 80;
            this.agentBar1.SoftPhoneAppClassName = "Qt5QwindowIcon";
            this.agentBar1.SoftPhoneAppName = "视频通话";
            this.agentBar1.SoftPhoneAppProcessName = "VideoTelephone";
            this.agentBar1.SoftphoneAutoAnswer = false;
            this.agentBar1.SoftPhoneEnable = false;
            this.agentBar1.SoftPhoneEnable2 = false;
            this.agentBar1.SoftPhoneLogoffCmd = 90;
            this.agentBar1.SoftPhoneMsgValue = 256;
// TODO: “”的代码生成失败，原因是出现异常“无效的基元类型: System.IntPtr。请考虑使用 CodeObjectCreateExpression。”。
            this.agentBar1.TabIndex = 2;
            this.agentBar1.WebUrl = null;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1287, 261);
            this.Controls.Add(this.agentBar1);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private PLAgentBar.AgentBar agentBar1;
    }
}

