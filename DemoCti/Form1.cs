using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PLAgentBar;
using static PLAgentBar.AgentBar;

namespace DemoCti
{
    public partial class Form1 : Form
    {
        private EnumPhoneType mBindPhoneType;

        public Form1()
        {
            InitializeComponent();

        }
        private enum EnumPhoneType
        {
            InternalPhone,
            ExternalPhone,
            NoPhone,
            ControlOtherPhone
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.agentBar1.AgentID = "5005";
            this.agentBar1.AgentExten = "5005";
            agentBar1.AgentPwd = "0000";
            agentBar1.AgentExten = "5005";
            agentBar1.ServerIP = "192.168.60.41";
            agentBar1.ServerPort = 12345;
            agentBar1.SipServer = "192.168.60.41";
            agentBar1.SipPort = 5061;
            agentBar1.SipPwd = "7857368089216850";
            agentBar1.AgentStatus = "0";
            agentBar1.SoftPhoneAnswerCmd = 80;
            agentBar1.SoftPhoneLogoffCmd = 90;
            agentBar1.SoftPhoneAppName = "视频通话";
            agentBar1.SoftPhoneAppClassName = "Qt5QwindowIcon";
            agentBar1.SoftPhoneAppProcessName = "VideoTelephone";
            agentBar1.SoftPhoneMsgValue = 256;
            //agentBar1.BindExten = true;
            agentBar1.SipAutoSignIn = true;
            agentBar1.SoftPhoneEnable2 = true;
            agentBar1.SipNum = "5005";
            
            agentBar1.CuID = "";
            agentBar1.bindSoftPhoneLogin = true;
            agentBar1.SipPhoneOnLineWarning = true;
            agentBar1.NoAnswerCallsURL = "192.168.60.41:8080";
            //this.agentBar1.SoftPhoneAnswerCmd = Helper.SipSoftPhone_Answer_Cmd;
            //this.agentBar1.SoftPhoneLogoffCmd = Helper.SipSoftPhone_Logoff_Cmd;
            //this.agentBar1.SoftPhoneAppName = Helper.SipSoftPhone_App_Name;
            //this.agentBar1.SoftPhoneAppClassName = Helper.SipSoftPhone_App_Class_Name;
            //this.agentBar1.SoftPhoneAppProcessName = Helper.SoftPhoneAppProcessName;
            this.agentBar1.SoftPhoneMsgValue = Helper.SipSoftPhone_Msg_Value;
            //this.agentBar1.SipServer = Helper.SipServer;
            //this.agentBar1.SipPort = Helper.SipPort;
            //this.agentBar1.SipNum = Helper.SipNum;
            //this.agentBar1.SipPwd = Helper.SipPwd;
            this.agentBar1.SipRegistTime = Helper.SipRegistTime;
            this.agentBar1.SipAutoAnswer = Helper.SipAutoAnswer;
            this.agentBar1.SipLocalNum = Helper.SipCallid;
            this.agentBar1.RefreshReportStatisInterval = Helper.ReportStatisInterval;
            this.agentBar1.AgentStateAfterHangup = AgentBar.Str2AgentStatus(Helper.Default_Agent_State_After_Hangup.ToString());
            this.agentBar1.HeartBeatTimeout = Helper.HeartTimeout;
            this.agentBar1.SoftPhoneEnable = false;
            this.agentBar1.SoftphoneAutoAnswer = true;
            bool flag=this.agentBar1.DoSignInWithConnect();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            bool flag=this.agentBar1.DoSignOut();
            var agentState=agentBar1.AgentState;
            var agentStatus = agentBar1.AgentStatus;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            bool flag = this.agentBar1.DoSignIn();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            bool flag = agentBar1.DoCallOut("13818894125", "51550628");
        }

        private void agentBar1_Load(object sender, EventArgs e)
        {

        }

        private void agentBar1_CallOutRingEvent(string customerNum, string callcenterNum, string accessNumName, string callType, string areaID, string areaName, string makeStr)
        {

        }
    }
}
