using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PLAgent
{
    internal class PLSoftPhone
    {
        private delegate void showMsg(int channel, int newState, string message);

        public delegate void SoftPhoneCallInEventHandle(string callerid);

        public delegate void SoftPhoneRegistResultEventHandle(int retCode);

        public delegate void SoftPhoneLogoutResultEventHandle(int retCode);

        public delegate void SoftPhoneHangupEventHandle();

        public delegate void SoftPhoneAnswerEventHandle();

        private enum CallState
        {
            UnRegister,
            Idle,
            Talk,
            Ring,
            Dial,
            HangUp
        }

        private string UserName;

        private string Pwd;

        private string ServerIp;

        private int Port;

        private string CallID;

        private bool AutoAnswer;

        private int RegistTime;

        private PLSoftPhone.CallState callState;

        private AxWonderRtcActivex cSipPhone;

        private IConnectionPoint icp;

        private int cookie;

        private string strTel;

        private static bool blnInitSoftPhone = false;

        private static ILog Log;

        private IConnectionPointContainer icpc;

        private Guid IID_IMyEvents;

        public event PLSoftPhone.SoftPhoneCallInEventHandle SoftPhoneCallInEvent;

        public event PLSoftPhone.SoftPhoneRegistResultEventHandle SoftPhoneRegistResultEvent;

        public event PLSoftPhone.SoftPhoneLogoutResultEventHandle SoftPhoneLogoutResultEvent;

        public event PLSoftPhone.SoftPhoneHangupEventHandle SoftPhoneHangupEvent;

        public event PLSoftPhone.SoftPhoneAnswerEventHandle SoftPhoneAnswerEvent;

        public AxWonderRtcActivex mySoftPhone
        {
            get
            {
                return this.cSipPhone;
            }
            set
            {
                this.cSipPhone = value;
            }
        }

        public PLSoftPhone(string _userName, string _pwd, string _serverip, int _port, bool _autoAnswer, 
            int _registTime, string _callid)
        {
            XmlConfigurator.Configure();
            PLSoftPhone.Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
            PLSoftPhone.Log.Debug("PLSoftPhone is Create.");
            this.UserName = _userName;
            this.Pwd = _pwd;
            this.ServerIp = _serverip;
            this.Port = _port;
            this.AutoAnswer = _autoAnswer;
            this.RegistTime = _registTime;
            this.CallID = _callid;
        }

        ~PLSoftPhone()
        {
            if (this.callState == PLSoftPhone.CallState.Talk)
            {
                this.PL_HangUp();
            }
            PLSoftPhone.Log.Debug("PLSoftPhone is destruct.");
        }

        public bool PL_Login()
        {
            bool result;
            try
            {
                if (!this.init())
                {
                    result = false;
                }
                else if (!this.ConfigSys())
                {
                    result = false;
                }
                else
                {
                    result = true;
                }
            }
            catch (Exception e)
            {
                PLSoftPhone.Log.Error("softPhone PL_Login error!reason=" + e.Message);
                result = false;
            }
            return result;
        }

        private bool init()
        {
            bool result;
            if (PLSoftPhone.blnInitSoftPhone)
            {
                result = true;
            }
            else if (!this.initSipPhone())
            {
                MessageBox.Show("软电话初始化失败！", "初始化软电话", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                result = false;
            }
            else
            {
                PLSoftPhone.Log.Debug("软电话初始化成功！");
                PLSoftPhone.blnInitSoftPhone = true;
                result = true;
            }
            return result;
        }

        public bool ConfigSys()
        {
            bool result;
            if (!this.PL_Regist())
            {
                result = false;
            }
            else if (!this.PL_SetCallID(this.CallID))
            {
                result = false;
            }
            else if (!this.PL_SetAutoAnswer(this.AutoAnswer))
            {
                result = false;
            }
            else if (!this.PL_SetRegistExpire(this.RegistTime))
            {
                result = false;
            }
            else
            {
                this.callState = PLSoftPhone.CallState.UnRegister;
                this.strTel = "";
                result = true;
            }
            return result;
        }

        private bool initSipPhone()
        {
            bool result;
            if (!PLSoftPhone.blnInitSoftPhone)
            {
                if (0 != this.cSipPhone.Init())
                {
                    result = false;
                    return result;
                }
                PLSoftPhone.blnInitSoftPhone = true;
            }
            this.cSipPhone.CallconnectedMessage += new _DWonderRtcActivexEvents_CallconnectedMessageEventHandler(this.cSipPhone_CallconnectedMessage);
            this.cSipPhone.CallendMessage += new _DWonderRtcActivexEvents_CallendMessageEventHandler(this.cSipPhone_CallendMessage);
            this.cSipPhone.CallErrorMessage += new _DWonderRtcActivexEvents_CallErrorMessageEventHandler(this.cSipPhone_CallError);
            this.cSipPhone.CallinMessage += new _DWonderRtcActivexEvents_CallinMessageEventHandler(this.cSipPhone_CallinMessage);
            this.cSipPhone.RegistMessgage += new _DWonderRtcActivexEvents_RegistMessgageEventHandler(this.cSipPhone_RegistMessgage);
            this.cSipPhone.RegistOffMessage += new _DWonderRtcActivexEvents_RegistOffMessageEventHandler(this.cSipPhone_RegistOffMessage);
            result = true;
            return result;
        }

        private bool PL_Regist()
        {
            bool result;
            if (this.ServerIp == "" || this.Port < 0 || this.UserName == "")
            {
                result = false;
            }
            else
            {
                try
                {
                    if (this.cSipPhone == null)
                    {
                        result = false;
                    }
                    else
                    {
                        PLSoftPhone.Log.Debug("软电话注册中....");
                        int rt = this.cSipPhone.Login(this.Pwd, this.ServerIp + ":" + this.Port, this.UserName);
                        if (0 != rt)
                        {
                            result = false;
                        }
                        else
                        {
                            result = true;
                        }
                    }
                }
                catch (Exception e)
                {
                    PLSoftPhone.Log.Error("软电话注册时发生异常！原因:" + e.Message);
                    result = false;
                }
            }
            return result;
        }

        public bool PL_LogOut()
        {
            bool result;
            try
            {
                if (this.cSipPhone == null)
                {
                    result = false;
                }
                else
                {
                    this.cSipPhone.Logout();
                    result = true;
                }
            }
            catch (Exception e)
            {
                PLSoftPhone.Log.Error("软电话注销时发生异常!" + e.Message);
                result = false;
            }
            return result;
        }

        private bool PL_SetCallID(string callID)
        {
            bool result;
            if (callID == null)
            {
                result = false;
            }
            else
            {
                try
                {
                    if (this.cSipPhone == null)
                    {
                        result = false;
                    }
                    else
                    {
                        result = true;
                    }
                }
                catch (Exception e)
                {
                    PLSoftPhone.Log.Error("softPhone PL_SetCallID error!reason=" + e.Message);
                    result = false;
                }
            }
            return result;
        }

        public bool PL_Answer()
        {
            bool result;
            try
            {
                if (this.cSipPhone == null)
                {
                    result = false;
                }
                else if (0 != this.cSipPhone.Answercall())
                {
                    result = false;
                }
                else
                {
                    result = true;
                }
            }
            catch (Exception e)
            {
                PLSoftPhone.Log.Error("softPhone PL_Answer error!reason=" + e.Message);
                result = false;
            }
            return result;
        }

        public bool PL_Hold()
        {
            bool result;
            if (this.callState != PLSoftPhone.CallState.Talk)
            {
                result = false;
            }
            else
            {
                try
                {
                    result = true;
                }
                catch (Exception e)
                {
                    PLSoftPhone.Log.Error("softPhone PL_Hold error!reason=" + e.Message);
                    result = false;
                }
            }
            return result;
        }

        public bool PL_SetBusy()
        {
            bool result;
            try
            {
                if (this.cSipPhone == null)
                {
                    result = false;
                }
                else
                {
                    result = true;
                }
            }
            catch (Exception e)
            {
                PLSoftPhone.Log.Error("softPhone PL_SetBusy error!reason=" + e.Message);
                result = false;
            }
            return result;
        }

        public bool PL_SetAutoAnswer(bool autoAnswer)
        {
            bool result;
            try
            {
                if (this.cSipPhone == null)
                {
                    result = false;
                }
                else
                {
                    this.cSipPhone.SetAutoAnswerStatus(autoAnswer);
                    result = true;
                }
            }
            catch (Exception e)
            {
                PLSoftPhone.Log.Error("softPhone PL_SetAutoAnswer error!reason=" + e.Message);
                result = false;
            }
            return result;
        }

        public bool PL_SetRegistExpire(int registTimeLen)
        {
            bool result;
            try
            {
                if (this.cSipPhone == null)
                {
                    result = false;
                }
                else
                {
                    this.cSipPhone.SetRegistExpires(registTimeLen);
                    result = true;
                }
            }
            catch (Exception e)
            {
                PLSoftPhone.Log.Error("softPhone PL_SetRegistExpire error!reason=" + e.Message);
                result = false;
            }
            return result;
        }

        public bool PL_Call(string callNum)
        {
            bool result;
            try
            {
                if (this.cSipPhone == null)
                {
                    result = false;
                }
                else if (0 != this.cSipPhone.Callout(callNum))
                {
                    result = false;
                }
                else
                {
                    result = true;
                }
            }
            catch (Exception e)
            {
                PLSoftPhone.Log.Error("softPhone PL_Call error!reason=" + e.Message);
                result = false;
            }
            return result;
        }

        public bool PL_HangUp()
        {
            bool result;
            try
            {
                if (this.cSipPhone == null)
                {
                    result = false;
                }
                else if (0 != this.cSipPhone.Hangup())
                {
                    result = false;
                }
                else
                {
                    result = true;
                }
            }
            catch (Exception e)
            {
                PLSoftPhone.Log.Error("softPhone PL_HangUp error!reason=" + e.Message);
                result = false;
            }
            return result;
        }

        public bool PL_SendKey(string strkey)
        {
            bool result;
            try
            {
                if (this.cSipPhone == null)
                {
                    result = false;
                }
                else
                {
                    result = true;
                }
            }
            catch (Exception e)
            {
                PLSoftPhone.Log.Error("softPhone PL_SendKey error!reason=" + e.Message);
                result = false;
            }
            return result;
        }

        private void cSipPhone_CallconnectedMessage(object sender, _DWonderRtcActivexEvents_CallconnectedMessageEvent e)
        {
            this.callState = PLSoftPhone.CallState.Talk;
            if (this.SoftPhoneAnswerEvent != null)
            {
                this.SoftPhoneAnswerEvent();
            }
        }

        private void cSipPhone_CallendMessage(object sender, _DWonderRtcActivexEvents_CallendMessageEvent e)
        {
            this.callState = PLSoftPhone.CallState.Idle;
            if (this.SoftPhoneHangupEvent != null)
            {
                this.SoftPhoneHangupEvent();
            }
        }

        private void cSipPhone_CallError(object sender, _DWonderRtcActivexEvents_CallErrorMessageEvent e)
        {
            PLSoftPhone.Log.Error("softPhone cSipPhone_CallError error!reason=" + e.message);
        }

        private void cSipPhone_CallinMessage(object sender, _DWonderRtcActivexEvents_CallinMessageEvent e)
        {
            this.callState = PLSoftPhone.CallState.Ring;
            if (this.SoftPhoneCallInEvent != null)
            {
                this.SoftPhoneCallInEvent(e.mstr);
            }
        }

        private void cSipPhone_RegistMessgage(object sender, _DWonderRtcActivexEvents_RegistMessgageEvent e)
        {
            if (e.message == "regist failed")
            {
                if (this.SoftPhoneRegistResultEvent != null)
                {
                    this.SoftPhoneRegistResultEvent(-1);
                }
            }
            else
            {
                if (this.callState == PLSoftPhone.CallState.UnRegister)
                {
                    this.callState = PLSoftPhone.CallState.Idle;
                }
                if (this.SoftPhoneRegistResultEvent != null)
                {
                    this.SoftPhoneRegistResultEvent(0);
                }
            }
        }

        private void cSipPhone_RegistOffMessage(object sender, _DWonderRtcActivexEvents_RegistOffMessageEvent e)
        {
            if (e.message == "Registed Off")
            {
                if (this.SoftPhoneLogoutResultEvent != null)
                {
                    this.SoftPhoneLogoutResultEvent(0);
                }
            }
            else if (this.SoftPhoneLogoutResultEvent != null)
            {
                this.SoftPhoneLogoutResultEvent(-1);
            }
        }

        private void ReceiveEvents(int ch, int state, string msg)
        {
            try
            {
                PLSoftPhone.Log.Debug(string.Concat(new object[]
                {
                    "收到软电话事件：ch=",
                    ch,
                    ",state=",
                    state,
                    ",msg=",
                    msg
                }));
                switch (state)
                {
                    case -1:
                        PLSoftPhone.Log.Error(string.Concat(new object[]
                        {
                        "收到软电话异常事件：ch=",
                        ch,
                        ",state=",
                        state,
                        ",msg=",
                        msg
                        }));
                        break;
                    case 0:
                        this.callState = PLSoftPhone.CallState.UnRegister;
                        if (this.SoftPhoneRegistResultEvent != null)
                        {
                            this.SoftPhoneRegistResultEvent(-1);
                        }
                        break;
                    case 1:
                        if (this.callState == PLSoftPhone.CallState.UnRegister)
                        {
                            this.callState = PLSoftPhone.CallState.Idle;
                        }
                        if (msg == "注册成功")
                        {
                            if (this.SoftPhoneRegistResultEvent != null)
                            {
                                this.SoftPhoneRegistResultEvent(0);
                            }
                        }
                        else if (msg == "注销成功")
                        {
                            if (PLSoftPhone.blnInitSoftPhone)
                            {
                                this.icp.Unadvise(this.cookie);
                                PLSoftPhone.blnInitSoftPhone = false;
                                PLSoftPhone.Log.Debug("软电话断开事件连接！");
                            }
                            if (this.SoftPhoneLogoutResultEvent != null)
                            {
                                this.SoftPhoneLogoutResultEvent(0);
                            }
                        }
                        break;
                    case 2:
                        this.callState = PLSoftPhone.CallState.Ring;
                        if (this.SoftPhoneCallInEvent != null)
                        {
                            this.SoftPhoneCallInEvent(msg);
                        }
                        break;
                    case 3:
                        this.callState = PLSoftPhone.CallState.Dial;
                        break;
                    case 4:
                        this.callState = PLSoftPhone.CallState.Talk;
                        if (this.SoftPhoneAnswerEvent != null)
                        {
                            this.SoftPhoneAnswerEvent();
                        }
                        break;
                    case 5:
                        this.callState = PLSoftPhone.CallState.Idle;
                        this.strTel = "";
                        if (this.SoftPhoneHangupEvent != null)
                        {
                            this.SoftPhoneHangupEvent();
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                PLSoftPhone.Log.Debug("softPhone ReceiveEvents error!reason=" + ex.Message);
            }
        }
    }
}
