using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PLAgent;
using System.Threading;

namespace PLAgentUnit
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            PLAgent.PLAgent pLAgent = new PLAgent.PLAgent
            {
                ServerIP = "192.168.60.41",
                ServerPort = 12345,
                AgentID = "5005",
                AgentExten="5005",
                AgentPwd = "0000",
                SipServer = "192.168.60.41",
                SipPort = 5061,
                SipPwd = "7857368089216850",
                SoftPhoneAnswerCmd = 80,
                SoftPhoneLogoffCmd = 90,
                SoftPhoneAppName = "视频通话",
                SoftPhoneAppClassName = "Qt5QwindowIcon",
                SoftPhoneMsgValue = 256,
                //agentBar.BindExten = true;
                SipAutoSignIn = true,
                SoftPhoneEnable2 = true,
                SipNum = "5005",//"5005";
                CuID = "",
                bindSoftPhoneLogin = true,
                SoftphoneAutoAnswer = true,
                SipAutoAnswer = true,
                AgentStatus="0",
            };
            var aflag = pLAgent.DoConnect();
            var flag= pLAgent.DoSignIn();
            if (flag)
            {
                var bflag = pLAgent.DoCallOut("13818894125", "51550628");

                pLAgent.ResponseEvent += PLAgent_ResponseEvent;
            }

            Thread.Sleep(500000);
        }

        private void PLAgent_ResponseEvent(string EventType, string agentID, int retCode, string strReason)
        {
            //throw new NotImplementedException();
        }
    }
}
