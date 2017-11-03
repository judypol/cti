using log4net;
using log4net.Config;
using PLAgentDll;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Media;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;

namespace PLAgent
{
    public class PLAgent:IDisposable
    {
        public delegate void AgentBarEventDelegate(string jsonArgsStr);


        public delegate void JsonEventHandler(string jsonArgsStr);

        public delegate void InternalCallerRingEventHandler(string agentID, string calledID, string callType, string makeStr);

        public delegate void AgentBarUIChangedEventHandler(Event_Type event_type, string statusInfo);

        public delegate void CallInEventHandler(string callerID, string make_str, string callType, bool bNeedArea, string outExtraParamsFromIvr);

        public delegate void PredictCallOutBridgeRingEventHandler(string callerID, string make_str, string callType, bool bNeedArea, string outExtraParamsFromIvr);

        public delegate void Consult_CallInEventHandler(string agentID, string customerNum, string callcenterNum, string accessNumName, string consulterAgentNum, string callType, string areaID, string areaName, string custGrade, string outExtraParamsFromIvr, string makeStr);

        public delegate void Transfer_Blind_CallInEventHandler(string agentID, string customerNum, string callcenterNum, string accessNumName, string consulterAgentNum, string callType, string areaID, string areaName, string custGrade, string outExtraParamsFromIvr, string makeStr);

        public delegate void ThreeWayCallRingEventHandler(string agentID, string customerNum, string callcenterNum, string accessNumName, string callerAgentNum, string callType, string areaID, string areaName, string custGrade, string outExtraParamsFromIvr, string makeStr);

        public delegate void EavesdropCallRingEventHandler(string agentID, string customerNum, string callcenterNum, string accessNumName, string desAgentID, string callType, string areaID, string areaName, string custGrade, string outExtraParamsFromIvr, string makeStr);

        public delegate void WhisperCallRingEventHandler(string agentID, string customerNum, string callcenterNum, string accessNumName, string desAgentID, string callType, string areaID, string areaName, string custGrade, string outExtraParamsFromIvr, string WhisperCallRingEvent);

        public delegate void BargeinCallRingEventHandler(string agentID, string customerNum, string callcenterNum, string accessNumName, string desAgentID, string callType, string areaID, string areaName, string custGrade, string outExtraParamsFromIvr, string makeStr);

        public delegate void ForceHangupCallRingEventHandler(string agentID, string customerNum, string callcenterNum, string accessNumName, string desAgentID, string callType, string areaID, string areaName, string custGrade, string outExtraParamsFromIvr, string makeStr);

        public delegate void EavesdropEventHandler(string agentID, string reason, int retCode);

        public delegate void WhisperEventHandler(string agentID, string reason, int retCode);

        public delegate void BargeinEventHandler(string agentID, string reason, int retCode);

        public delegate void ForceHangupEventHandler(string agentID, string reason, int retCode);

        public delegate void InternalCall_CallInEventHandler(string agentID, string callerID, string callType, string makeStr);

        public delegate void UserStateEventHandler(string userState, string strReason);

        public delegate void AgentStatusChangeEventHandler(string agent_num, string old_status, string new_status, bool is_bind_exten, string customer_enter_channel, string current_time, string start_talking_time);

        public delegate void ResponseEventHandler(string EventType, string agentID, int retCode, string strReason);

        public delegate void GetOnlineAgentEventHandler(List<Agent_Online_Struct> agent_online);

        public delegate void GetAccessNumberEventHandler(string[] accessNumbers, string default_access_number, List<string> callHistory);

        public delegate void GetIvrListEventHandler(Dictionary<string, string> ivr_list);

        public delegate void GetQueueListEventHandler(Dictionary<string, string> queue_list);

        public delegate void GetIvrProfileListEventHandler(Dictionary<string, string> ivr_profile_list);

        public delegate void GetAgentGroupListEventHandler(Dictionary<string, string> agent_group_list);

        public delegate void GetAllAgentGroupListEventHandler(Dictionary<string, string> agent_group_list);

        public delegate void GetAgentsOfQueueEventHandler(string agent_list, string queue_num);

        public delegate void GetAgentsOfAgentGroupEventHandler(string agent_list, string agent_group_num);

        public delegate void GetAgentsMonitorInfoEventHandler(List<Agent_Online_Struct> agent_monitor_info, string current_time, Dictionary<string, string> group_list, Dictionary<string, string> role_list);

        public delegate void GetDetailCallInfoEventHandler(string targetAgentNum, string call_type, List<Leg_Info_Struct> leg_info, List<Relation_Info_Struct> relation_info);

        public delegate void GetCustomerOfQueueEventHandler(string queueNumLstStr, string current_time, List<Customer_Info_Struct> customer_info);

        public delegate void GetCustomerOfMyQueueEventHandler(string queueNumLstStr, string current_time, List<Customer_Info_Struct> customer_info);

        public delegate void GetQueueStatisLstEventHandler(string queueNumLstStr, string current_time, List<Queue_Statis_Struct> queue_statis_lst);

        public delegate void GetAllQueueStatisEventHandler(string current_time, List<Queue_Statis_Struct> queue_statis_lst);

        public delegate void AddCustomerToQueueEventHandler(string call_type, string callcenter_num, string customer_num, string customer_status, string customer_type, string customer_uuid, string enter_queue_time, string exclusive_agent_num, string exclusive_queue_num, string queue_num, string current_time, string queue_name, string queue_customer_amount, string early_queue_enter_time, string early_queue_enter_time_all, string customer_enter_channel);

        public delegate void UpdateCustomerOfQueueEventHandler(string call_type, string callcenter_num, string customer_num, string customer_status, string customer_type, string customer_uuid, string enter_queue_time, string exclusive_agent_num, string exclusive_queue_num, string queue_num);

        public delegate void DelCustomerFromQueueEventHandler(string customer_uuid, string queue_num, string current_time, string early_queue_enter_time, string early_queue_enter_time_all, string reason);

        public delegate void ThreeWayeeHangupEventHandler(string agentID, string hangupReason);

        public delegate void WarnAgentResigninEventHandler();

        public delegate void WarnAgentForceChangeStatusEventHandler(string executorAgentID);

        public delegate void ConsulteeHangupEventHandler(string agentID, string hangupReason);

        public delegate void EventResultEventHandler(string EventType, string agentID, int retCode, string strReason, string strHangupReason);

        public delegate void SignInRingEventHandler();

        public delegate void SignInPlayEventHandler();

        public delegate void CallOutRingEventHandler(string customerNum, string callcenterNum, string accessNumName, string callType, string areaID, string areaName, string makeStr);

        public delegate void HoldEventHandler();

        public delegate void SockDisconnectEventHandler(string reason, int retCode);

        public delegate void ResponseTimeOutEventHandler(string reason, int retCode);

        public delegate void SysThrowExceptionEventHandler(string reason, int retCode);

        public delegate void CheckExtenEventHandler(string AgentID, string makeStr);

        public delegate void SignInEventHandler(string AgentID, int retCode, string strReason);

        public delegate void HangupEventHandler(string strReason, int retCode, string makeStr);

        public delegate void SignOutEventHandler(string AgentID, int retCode, string strReason);

        public delegate void GetWebSiteInfoEventHandler(string AgentID, int retCode, string strReason, Dictionary<string, string> website_dic);

        public delegate void KickOutEventHandler(string AgentID, int retCode, string strReason);

        public delegate void ServerResponseEventHandler(string AgentID, int retCode, string strReason);

        public delegate void SignInResponseEventHandler(string AgentID, int retCode, string strReason);

        public delegate void SignOutResponseEventHandler(string AgentID, int retCode, string strReason);

        public delegate void AnswerEventHandler(string makeStr);

        public delegate void CalleeAnswerEventHandler(string relaiton_uuid);

        public delegate void BridgeEventHandler();

        public delegate void GetRoleNameEventHandler(string roleName);

        public delegate void SendAgentStatusEventHandler(Agent_State agentState, string targetAgentNum);

        public delegate void ReceiveEventHandler(AgentEvent agent_event);

        public delegate void ReceiveHeartBeatEventHandler(AgentEvent agent_event);

        private delegate void DelegateUpdateUI(Event_Type eventType, string info);

        private delegate void SocketDisconnectEventHandler();

        public delegate void ForceChangeStatusEventHandler(string agentID, string reason, int retCode);

        public delegate void GetAgentPersonalInfoEventHandler(string agentID, string agent_mobile, string agnet_email);

        public delegate void SetAgentPersonalInfoEventHandler(string agentID, int retCode, string reason);

        public delegate void ChangePswdEventHandler(string agentID, int retCode, string reason);

        public delegate void GetReportStatisInfoEventHandler(string agentID, int retCode, string reason, Dictionary<string, string> report_statis_info);

        public delegate void ApplyChangeStatusEventHandler(string agentID, int retCode, string reason);

        public delegate void ApplyChangeStatusCancelEventHandler(string agentID, string apply_agentid, string targetStatus, int retCode, string reason);

        public delegate void ApplyChangeStatusDistributeEventHandler(string AgentID, string apply_agentid, string targetStatus, string apply_agentName, string apply_agent_groupID, string apply_agent_groupName, string apply_time, string applyType, int retCode, string reason);

        public delegate void ApproveChangeStatusDistributeEventHandler(string AgentID, string apply_agentid, string apply_agent_groupID, string targetStatus, string approveResult, string approve_time, int retCode, string reason);

        public delegate void ApproveChangeStatusTimeoutEventDistributeHandler(string AgentID, string apply_agentid, string apply_agent_groupID, string targetStatus, string timeoutType);

        public delegate void GetAgentGroupStatusMaxNumEventHandler(string AgentID, string agentGroupNameLstStr, Dictionary<string, string> dicAgentGroupStatusMaxNum);

        public delegate void GetChangeStatusApplyListEventHandler(string agentID, List<Apply_Change_Status> apply_agent_list, int retCode, string reason);

        public delegate void QueueTransferBoundEventHandler(string agentID, string customer_num, string queue_num, string transfee_num, string access_num, string tranfer_time);

        public delegate void Record_StartEventHandler(string AgentExten, string AgentID, string Agent_call_uuid, string CalleeNum, string CallerNum, string FilePath, string makeStr);

        public delegate void Record_StopEventHandler(string AgentID, string Agent_call_uuid, string FilePath, string makeStr);

        public delegate void Cust_Evaluate_Result_Handle(string AgentID, string Agent_call_uuid, string agentExten, string customerUuid, string evaluateScore, string evaluateStatus, string makeStr);

        public delegate void AllGetwaysFullEventHandler();

        public delegate void NoAnswerCallAlarmlEventHandler();

        public delegate void transferControlsVisibleHandler();


        private const int MaxSoftPhoneRegistTime = 5;

        private const string _IID_IDispatch = "{00020400-0000-0000-C000-000000000046}";

        private const string _IID_IDispatchEx = "{a6ef9860-c720-11d0-9337-00a0c90dcaa9}";

        private const string _IID_IPersistStorage = "{0000010A-0000-0000-C000-000000000046}";

        private const string _IID_IPersistStream = "{00000109-0000-0000-C000-000000000046}";

        private const string _IID_IPersistPropertyBag = "{37D84F60-42CB-11CE-8135-00AA004BB851}";

        private const int INTERFACESAFE_FOR_UNTRUSTED_CALLER = 1;

        private const int INTERFACESAFE_FOR_UNTRUSTED_DATA = 2;

        private const int S_OK = 0;

        private const int E_FAIL = -2147467259;

        private const int E_NOINTERFACE = -2147467262;

        

        private IntPtr h = IntPtr.Zero;

        private AgentDll agentDll;

        private bool blnConnect;

        private bool blnSignIn;

        private bool blnTalking;

        private bool blnHold;

        private bool blnMute;

        private bool blnListen;

        private bool blnBargein;

        private bool blnWhisper;

        private string mCuID = "";

        private string mAgentPwd;

        private string mAgentID;

        private string mAgentExten;

        private string mAgentName;

        private string mAgentGroupID;

        private string mAgentGroupName;

        private string mRoleID;

        private string mRoleName;

        private string mDisplayNum;

        private bool mGradeSwitch;

        private Agent_Status mAgentStateAfterHangup = Agent_Status.AGENT_STATUS_ACW;

        private Agent_Status mAgentStateBeforeCallinOrCallout;

        private bool mBindExten;

        private string mAgentStatus;

        private string mExtenMode;

        private string mWebUrl;

        private string mServerIP;

        private Call_Type mCallType;

        private CallStatus mCallStatus = CallStatus.NO_CALL;

        private Agent_State mAgentState;

        private int mHeartBeatTimeout = 30;

        private string mEavesdropAgent;

        private bool mExtenIsOutbound;

        private string mDefaultAccessNum;

        private List<string> mCalloutHistory;

        private bool mIsMonitorOfflineAgent = false;

        private string mSaltKey;

        private string mDID_Num;

        private string mInitStatusWhenLogin;

        private int mRefreshReportStatisInterval = 60;

        private bool mSoftPhoneEnable;

        private bool mSoftPhoneEnable2;

        private PLSoftPhone mSoftPhone;

        private string mSipNum;

        private string mSipPwd;

        private string mSipServer;

        private int mSipPort;

        private int mSipRegistTime;

        private bool mSipAutoAnswer;

        private bool mSipAutoSignIn;

        private string mSipLocalNum;

        private volatile bool mSoftPhoneRegistOk;

        private int mSigninIntervalAfterSoftPhoneRegisted = 3000;

        private bool mSipPhoneOnLineWarning = false;

        public bool bindSoftPhoneLogin;

        private Dictionary<string, string> mGroupMap;

        private Dictionary<string, string> mRoleMap;

        private Dictionary<string, string> mAgentDefineStatus;

        private Dictionary<string, string> mAgentWebSiteInfo;

        private List<Agent_Role_And_Right_Struct> mAgentRoleAndRight;

        private Agent_Role_And_Right_Struct mMyRoleAndRight;

        public int HeartBeatTimes = 0;

        private System.Threading.Timer tmrHeartBeat;

        private System.Threading.Timer tmrApplyMsgbox;

        private string mNoAnswerCallsURL = string.Empty;

        private string m_agent_current_call_uuid = string.Empty;

        private string m_customer_current_call_uuid = string.Empty;

        private bool transfercheck = false;

        private bool Consultcheck = false;

        private string tsbStateBack = string.Empty;

        private int timeCount = 1;

        private string mSoftPhone_app_process_name = "VideoTelephone";

        private string mSoftPhone_app_name = "视频通话";

        private string mSoftPhone_app_className = "Qt5QwindowIcon";

        private string mSoftPhone_app_path = "";

        private int mSoftPhone_answer_cmd = 80;

        private int mSoftPhone_logoff_cmd = 90;

        private int mSoftPhone_msg_value = 256;

        private int mSoftPhone_dtmf_0_cmd = 200;

        private int mSoftPhone_dtmf_1_cmd = 201;

        private int mSoftPhone_dtmf_2_cmd = 202;

        private int mSoftPhone_dtmf_3_cmd = 203;

        private int mSoftPhone_dtmf_4_cmd = 204;

        private int mSoftPhone_dtmf_5_cmd = 205;

        private int mSoftPhone_dtmf_6_cmd = 206;

        private int mSoftPhone_dtmf_7_cmd = 207;

        private int mSoftPhone_dtmf_8_cmd = 208;

        private int mSoftPhone_dtmf_9_cmd = 209;

        private int mSoftPhone_dtmf_star_cmd = 210;

        private int mSoftPhone_dtmf_pound_cmd = 211;

        private Apply_Change_Status my_apply_change_status;

        private List<Apply_Change_Status> apply_change_status_to_approval_lst;

        private List<Apply_Change_Status> apply_change_status_approval_history;

        private IntPtr mSoftPhoneWindowHandle = IntPtr.Zero;

        public Dictionary<string, bool> controls_count_previous = new Dictionary<string, bool>();

        public Dictionary<string, bool> controls_count_current = new Dictionary<string, bool>();

        public bool ApproveNormal = true;

        public bool NoAnswerCallNormal = true;

        public bool otherNormal = true;

        public static ILog Log;

        private int mPort;

        private static object thislock = new object();

        private string ringFileName = Application.StartupPath + "/sound/Ringback.wav";

        private string hangupRingFileName = Application.StartupPath + "/sound/dududu.wav";

        private bool CallSuccess = false;

        private bool mSoftphoneAutoAnswer = false;

        private bool needCleanUpTime = false;

        private bool isTalking = false;

        private string mHotKeyIdle;

        private string mHotKeyBusy;

        private string mHotKeyLeave;

        private string mHotKeyCallOut;

        private string mHotKeyMonitor;

        private bool _fSafeForScripting = true;

        private bool _fSafeForInitializing = true;

        public event AgentBar.InternalCallerRingEventHandler InternalCallerRingEvent;

        public event AgentBar.AgentBarUIChangedEventHandler AgentBarUIChangedEvent;

        public event AgentBar.CallInEventHandler CallInEvent;

        public event AgentBar.PredictCallOutBridgeRingEventHandler PredictCallOutBridgeRingEvent;

        public event AgentBar.Consult_CallInEventHandler ConsultCallInEvent;

        public event AgentBar.Transfer_Blind_CallInEventHandler TransferBlindCallInEvent;

        public event AgentBar.ThreeWayCallRingEventHandler ThreeWayCallRingEvent;

        public event AgentBar.EavesdropCallRingEventHandler EavesdropCallRingEvent;
        public event AgentBar.WhisperCallRingEventHandler WhisperCallRingEvent;

        public event AgentBar.BargeinCallRingEventHandler BargeinCallRingEvent;

        public event AgentBar.ForceHangupCallRingEventHandler ForceHangupCallRingEvent;

        public event AgentBar.EavesdropEventHandler EavesdropEvent;

        public event AgentBar.WhisperEventHandler WhisperEvent;

        public event AgentBar.BargeinEventHandler BargeinEvent;

        public event AgentBar.ForceHangupEventHandler ForceHangupEvent;

        public event AgentBar.InternalCall_CallInEventHandler InternalCall_CallInEvent;

        public event AgentBar.UserStateEventHandler UserStateChangeEvent;

        public event AgentBar.AgentStatusChangeEventHandler AgentStatusChangeEvent;

        public event AgentBar.ResponseEventHandler ResponseEvent;

        public event AgentBar.GetOnlineAgentEventHandler GetAgentOnlineEvent;

        public event AgentBar.JsonEventHandler JSGetAgentOnlineEvent;

        public event AgentBar.GetAccessNumberEventHandler GetAccessNumberEvent;

        public event AgentBar.JsonEventHandler JSGetAccessNumberEvent;

        public event AgentBar.GetIvrListEventHandler GetIvrListEvent;

        public event AgentBar.JsonEventHandler JSGetIvrListEvent;

        public event AgentBar.GetQueueListEventHandler GetQueueListEvent;

        public event AgentBar.JsonEventHandler JSGetQueueListEvent;

        public event AgentBar.GetIvrProfileListEventHandler GetIvrProfileListEvent;

        public event AgentBar.JsonEventHandler JSGetIvrProfileListEvent;

        public event AgentBar.GetAgentGroupListEventHandler GetAgentGroupListEvent;

        public event AgentBar.JsonEventHandler JSGetAgentGroupListEvent;

        public event AgentBar.GetAllAgentGroupListEventHandler GetAllAgentGroupListEvent;

        public event AgentBar.JsonEventHandler JSGetAllAgentGroupListEvent;

        public event AgentBar.GetAgentsOfQueueEventHandler GetAgentsOfQueueEvent;

        public event AgentBar.GetAgentsOfAgentGroupEventHandler GetAgentsOfAgentGroupEvent;

        public event AgentBar.GetAgentsMonitorInfoEventHandler GetAgentsMonitorInfoEvent;

        public event AgentBar.JsonEventHandler JSGetAgentsMonitorInfoEvent;

        public event AgentBar.GetDetailCallInfoEventHandler GetDetailCallInfoEvent;

        public event AgentBar.JsonEventHandler JSGetDetailCallInfoEvent;

        public event AgentBar.GetCustomerOfQueueEventHandler GetCustomerOfQueueEvent;

        public event AgentBar.JsonEventHandler JSGetCustomerOfQueueEvent;

        public event AgentBar.GetCustomerOfMyQueueEventHandler GetCustomerOfMyQueueEvent;

        public event AgentBar.JsonEventHandler JSGetCustomerOfMyQueueEvent;

        public event AgentBar.GetQueueStatisLstEventHandler GetQueueStatisLstEvent;

        public event AgentBar.JsonEventHandler JSGetQueueStatisLstEvent;

        public event AgentBar.GetAllQueueStatisEventHandler GetAllQueueStatisEvent;

        public event AgentBar.JsonEventHandler JSGetAllQueueStatisEvent;

        public event AgentBar.AddCustomerToQueueEventHandler AddCustomerToQueueEvent;

        public event AgentBar.UpdateCustomerOfQueueEventHandler UpdateCustomerOfQueueEvent;

        public event AgentBar.DelCustomerFromQueueEventHandler DelCustomerFromQueueEvent;

        public event AgentBar.ThreeWayeeHangupEventHandler ThreeWayeeHangupEvent;

        public event AgentBar.WarnAgentResigninEventHandler WarnAgentResigninEvent;

        public event AgentBar.WarnAgentForceChangeStatusEventHandler WarnAgentForceChangeStatusEvent;

        public event AgentBar.ConsulteeHangupEventHandler ConsulteeHangupEvent;

        public event AgentBar.EventResultEventHandler EventResultEvent;

        public event AgentBar.SignInRingEventHandler SignInRingEvent;

        public event AgentBar.SignInPlayEventHandler SignInPlayEvent;

        public event AgentBar.CallOutRingEventHandler CallOutRingEvent;

        public event AgentBar.HoldEventHandler HoldEvent;

        public event AgentBar.SockDisconnectEventHandler SockDisconnectEvent;

        public event AgentBar.ResponseTimeOutEventHandler ResponseTimeOutEvent;

        public event AgentBar.SysThrowExceptionEventHandler SysThrowExceptionEvent;

        public event AgentBar.CheckExtenEventHandler CheckExtenEvent;

        public event AgentBar.SignInEventHandler SignInEvent;

        public event AgentBar.HangupEventHandler HangupEvent;

        public event AgentBar.SignOutEventHandler SignOutEvent;

        public event AgentBar.GetWebSiteInfoEventHandler GetWebSiteInfoEvent;

        public event AgentBar.JsonEventHandler JSGetWebSiteInfoEvent;

        public event AgentBar.KickOutEventHandler KickOutEvent;

        public event AgentBar.ServerResponseEventHandler ServerResponse;

        public event AgentBar.SignInResponseEventHandler SignInResponse;

        public event AgentBar.SignOutResponseEventHandler SignOutResponse;

        public event AgentBar.AnswerEventHandler AnswerEvent;

        public event AgentBar.CalleeAnswerEventHandler CalleeAnswerEvent;

        public event AgentBar.BridgeEventHandler BridgeEvent;

        public event AgentBar.GetRoleNameEventHandler GetRoleNameEvent;

        public event AgentBar.SendAgentStatusEventHandler SendAgentStatusEvent;

        public event AgentBar.ForceChangeStatusEventHandler ForceChangeStatusEvent;

        public event AgentBar.GetAgentPersonalInfoEventHandler GetAgentPersonalInfoEvent;

        public event AgentBar.SetAgentPersonalInfoEventHandler SetAgentPersonalInfoEvent;

        public event AgentBar.ChangePswdEventHandler ChangePswdEvent;

        public event AgentBar.GetReportStatisInfoEventHandler GetReportStatisInfoEvent;

        public event AgentBar.JsonEventHandler JSGetReportStatisInfoEvent;

        public event AgentBar.ApplyChangeStatusEventHandler ApplyChangeStatusEvent;

        public event AgentBar.ApplyChangeStatusCancelEventHandler ApplyChangeStatusCancelEvent;

        public event AgentBar.ApplyChangeStatusDistributeEventHandler ApplyChangeStatusDistributeEvent;

        public event AgentBar.ApproveChangeStatusDistributeEventHandler ApproveChangeStatusDistributeEvent;

        public event AgentBar.ApproveChangeStatusTimeoutEventDistributeHandler ApproveChangeStatusTimeoutDistributeEvent;

        public event AgentBar.GetAgentGroupStatusMaxNumEventHandler GetAgentGroupStatusMaxNumEvent;

        public event AgentBar.JsonEventHandler JSGetAgentGroupStatusMaxNumEvent;

        public event AgentBar.GetChangeStatusApplyListEventHandler GetChangeStatusApplyListEvent;

        public event AgentBar.JsonEventHandler JSGetChangeStatusApplyListEvent;

        public event AgentBar.QueueTransferBoundEventHandler QueueTransferBoundEvent;

        public event AgentBar.Record_StartEventHandler RecordStart;

        public event AgentBar.Record_StopEventHandler RecordStop;

        public event AgentBar.Cust_Evaluate_Result_Handle Cust_Evaluate_Result;

        public event AgentBar.AllGetwaysFullEventHandler AllGetwaysFullEvent;

        public event AgentBar.NoAnswerCallAlarmlEventHandler NoAnswerCallAlarmlEvent;

        public event AgentBar.transferControlsVisibleHandler transferControlsVisible;


        public IntPtr SoftPhoneWindowHandle
        {
            get
            {
                return this.mSoftPhoneWindowHandle;
            }
            set
            {
                this.mSoftPhoneWindowHandle = value;
            }
        }

        public string SoftPhoneAppProcessName
        {
            get
            {
                return this.mSoftPhone_app_process_name;
            }
            set
            {
                this.mSoftPhone_app_process_name = value;
            }
        }

        public string SoftPhoneAppName
        {
            get
            {
                return this.mSoftPhone_app_name;
            }
            set
            {
                this.mSoftPhone_app_name = value;
            }
        }

        public string SoftPhoneAppClassName
        {
            get
            {
                return this.mSoftPhone_app_className;
            }
            set
            {
                this.mSoftPhone_app_className = value;
            }
        }

        public int SoftPhoneMsgValue
        {
            get
            {
                return this.mSoftPhone_msg_value;
            }
            set
            {
                this.mSoftPhone_msg_value = value;
            }
        }

        public int SoftPhoneAnswerCmd
        {
            get
            {
                return this.mSoftPhone_answer_cmd;
            }
            set
            {
                this.mSoftPhone_answer_cmd = value;
            }
        }

        public int SoftPhoneLogoffCmd
        {
            get
            {
                return this.mSoftPhone_logoff_cmd;
            }
            set
            {
                this.mSoftPhone_logoff_cmd = value;
            }
        }

        public int SoftPhone_Dtmf_0_Cmd
        {
            get
            {
                return this.mSoftPhone_dtmf_0_cmd;
            }
            set
            {
                this.mSoftPhone_dtmf_0_cmd = value;
            }
        }

        public int SoftPhone_dtmf_1_cmd
        {
            get
            {
                return this.mSoftPhone_dtmf_1_cmd;
            }
            set
            {
                this.mSoftPhone_dtmf_1_cmd = value;
            }
        }

        public int SoftPhone_dtmf_2_cmd
        {
            get
            {
                return this.mSoftPhone_dtmf_2_cmd;
            }
            set
            {
                this.mSoftPhone_dtmf_2_cmd = value;
            }
        }

        public int SoftPhone_dtmf_3_cmd
        {
            get
            {
                return this.mSoftPhone_dtmf_3_cmd;
            }
            set
            {
                this.mSoftPhone_dtmf_3_cmd = value;
            }
        }

        public int SoftPhone_dtmf_4_cmd
        {
            get
            {
                return this.mSoftPhone_dtmf_4_cmd;
            }
            set
            {
                this.mSoftPhone_dtmf_4_cmd = value;
            }
        }

        public int SoftPhone_dtmf_5_cmd
        {
            get
            {
                return this.mSoftPhone_dtmf_5_cmd;
            }
            set
            {
                this.mSoftPhone_dtmf_5_cmd = value;
            }
        }

        public int SoftPhone_dtmf_6_cmd
        {
            get
            {
                return this.mSoftPhone_dtmf_6_cmd;
            }
            set
            {
                this.mSoftPhone_dtmf_6_cmd = value;
            }
        }

        public int SoftPhone_dtmf_7_cmd
        {
            get
            {
                return this.mSoftPhone_dtmf_7_cmd;
            }
            set
            {
                this.mSoftPhone_dtmf_7_cmd = value;
            }
        }

        public int SoftPhone_dtmf_8_cmd
        {
            get
            {
                return this.mSoftPhone_dtmf_8_cmd;
            }
            set
            {
                this.mSoftPhone_dtmf_8_cmd = value;
            }
        }

        public int SoftPhone_dtmf_9_cmd
        {
            get
            {
                return this.mSoftPhone_dtmf_9_cmd;
            }
            set
            {
                this.mSoftPhone_dtmf_9_cmd = value;
            }
        }

        public int SoftPhone_dtmf_star_cmd
        {
            get
            {
                return this.mSoftPhone_dtmf_star_cmd;
            }
            set
            {
                this.mSoftPhone_dtmf_star_cmd = value;
            }
        }

        public int SoftPhone_dtmf_pound_cmd
        {
            get
            {
                return this.mSoftPhone_dtmf_pound_cmd;
            }
            set
            {
                this.mSoftPhone_dtmf_pound_cmd = value;
            }
        }

        public bool SoftPhoneEnable2
        {
            get
            {
                return this.mSoftPhoneEnable2;
            }
            set
            {
                this.mSoftPhoneEnable2 = value;
            }
        }

        public bool SoftphoneAutoAnswer
        {
            get
            {
                return this.mSoftphoneAutoAnswer;
            }
            set
            {
                this.mSoftphoneAutoAnswer = value;
            }
        }

        public bool IsConnected
        {
            get
            {
                return this.blnConnect;
            }
        }

        public bool IsSignIn
        {
            get
            {
                return this.blnSignIn;
            }
        }

        public bool IsInUse
        {
            get
            {
                return AgentBar.Str2AgentStatus(this.mAgentStatus) == AgentBar.Agent_Status.AGENT_STATUS_HOLD || AgentBar.Str2AgentStatus(this.mAgentStatus) == AgentBar.Agent_Status.AGENT_STATUS_MUTE || AgentBar.Str2AgentStatus(this.mAgentStatus) == AgentBar.Agent_Status.AGENT_STATUS_CALLING_OUT || AgentBar.Str2AgentStatus(this.mAgentStatus) == AgentBar.Agent_Status.AGENT_STATUS_TALKING || AgentBar.Str2AgentStatus(this.mAgentStatus) == AgentBar.Agent_Status.AGENT_STATUS_RING || AgentBar.Str2AgentStatus(this.mAgentStatus) == AgentBar.Agent_Status.AGENT_STATUS_CAMP_ON;
            }
        }

        public bool IsTalking
        {
            get
            {
                return AgentBar.Str2AgentStatus(this.mAgentStatus) == AgentBar.Agent_Status.AGENT_STATUS_HOLD || AgentBar.Str2AgentStatus(this.mAgentStatus) == AgentBar.Agent_Status.AGENT_STATUS_MUTE || AgentBar.Str2AgentStatus(this.mAgentStatus) == AgentBar.Agent_Status.AGENT_STATUS_TALKING;
            }
        }

        public bool IsHold
        {
            get
            {
                return this.blnHold;
            }
        }

        public bool IsMute
        {
            get
            {
                return this.blnMute;
            }
        }

        public bool IsListen
        {
            get
            {
                return this.blnListen;
            }
        }

        public bool IsWhisper
        {
            get
            {
                return this.blnWhisper;
            }
        }

        public bool IsBargein
        {
            get
            {
                return this.blnBargein;
            }
        }

        public string CuID
        {
            get
            {
                return this.mCuID;
            }
            set
            {
                this.mCuID = value;
            }
        }

        public string AgentID
        {
            get
            {
                return this.mAgentID;
            }
            set
            {
                this.mAgentID = value;
            }
        }

        public string AgentName
        {
            get
            {
                return this.mAgentName;
            }
            set
            {
                this.mAgentName = value;
            }
        }

        public string AgentExten
        {
            get
            {
                return this.mAgentExten;
            }
            set
            {
                this.mAgentExten = value;
            }
        }

        public string AgentPwd
        {
            get
            {
                return this.mAgentPwd;
            }
            set
            {
                this.mAgentPwd = value;
            }
        }

        public bool BindExten
        {
            get
            {
                return this.mBindExten;
            }
            set
            {
                this.mBindExten = value;
            }
        }

        public bool ExtenIsOutbound
        {
            get
            {
                return this.mExtenIsOutbound;
            }
            set
            {
                this.mExtenIsOutbound = value;
            }
        }

        public string AgentStatus
        {
            get
            {
                return this.mAgentStatus;
            }
            set
            {
                this.mAgentStatus = value;
            }
        }

        public bool GradeSwitch
        {
            get
            {
                return this.mGradeSwitch;
            }
            set
            {
                this.mGradeSwitch = value;
            }
        }

        public string AgentGroupID
        {
            get
            {
                return this.mAgentGroupID;
            }
        }

        public string AgentGroupName
        {
            get
            {
                return this.mAgentGroupName;
            }
        }

        public string RoleID
        {
            get
            {
                return this.mRoleID;
            }
        }

        public string SaltKey
        {
            get
            {
                return this.mSaltKey;
            }
            set
            {
                this.mSaltKey = value;
            }
        }

        public string ExtenMode
        {
            get
            {
                return this.mExtenMode;
            }
            set
            {
                this.mExtenMode = value;
            }
        }

        public string WebUrl
        {
            get
            {
                return this.mWebUrl;
            }
            set
            {
                this.mWebUrl = value;
            }
        }

        public string ServerIP
        {
            get
            {
                return this.mServerIP;
            }
            set
            {
                this.mServerIP = value;
            }
        }

        public int ServerPort
        {
            get
            {
                return this.mPort;
            }
            set
            {
                this.mPort = value;
            }
        }

        public int HeartBeatTimeout
        {
            get
            {
                return this.mHeartBeatTimeout;
            }
            set
            {
                this.mHeartBeatTimeout = value;
            }
        }

        public bool SoftPhoneEnable
        {
            get
            {
                return this.mSoftPhoneEnable;
            }
            set
            {
                this.mSoftPhoneEnable = value;
            }
        }

        public string SipNum
        {
            get
            {
                return this.mSipNum;
            }
            set
            {
                this.mSipNum = value;
            }
        }

        public string SipPwd
        {
            get
            {
                return this.mSipPwd;
            }
            set
            {
                this.mSipPwd = value;
            }
        }

        public string SipServer
        {
            get
            {
                return this.mSipServer;
            }
            set
            {
                this.mSipServer = value;
            }
        }

        public int SipPort
        {
            get
            {
                return this.mSipPort;
            }
            set
            {
                this.mSipPort = value;
            }
        }

        public int SipRegistTime
        {
            get
            {
                return this.mSipRegistTime;
            }
            set
            {
                this.mSipRegistTime = value;
            }
        }

        public bool SipAutoAnswer
        {
            get
            {
                return this.mSipAutoAnswer;
            }
            set
            {
                this.mSipAutoAnswer = value;
            }
        }

        public string SipLocalNum
        {
            get
            {
                return this.mSipLocalNum;
            }
            set
            {
                this.mSipLocalNum = value;
            }
        }

        public string EavesDropAgentNum
        {
            get
            {
                return this.mEavesdropAgent;
            }
            set
            {
                this.mEavesdropAgent = value;
            }
        }

        public Agent_State AgentState
        {
            get
            {
                return this.mAgentState;
            }
            set
            {
                this.mAgentState = value;
            }
        }

        public Agent_Role_And_Right_Struct AgentRoleAndRight
        {
            get
            {
                return this.mMyRoleAndRight;
            }
        }

        public int SigninIntervalAfterSoftPhoneRegisted
        {
            get
            {
                return this.mSigninIntervalAfterSoftPhoneRegisted;
            }
            set
            {
                this.mSigninIntervalAfterSoftPhoneRegisted = value;
            }
        }

        public Apply_Change_Status GetMyApplyChangeStatus
        {
            get
            {
                return this.my_apply_change_status;
            }
        }

        public List<Apply_Change_Status> ApplyChangeStatusApprovalHistory
        {
            get
            {
                return this.apply_change_status_approval_history;
            }
            set
            {
                this.apply_change_status_approval_history = value;
            }
        }

        public List<Apply_Change_Status> GetApplyChangeStatusApprovalLst
        {
            get
            {
                return this.apply_change_status_to_approval_lst;
            }
        }

        public string HotKeyIdle
        {
            get
            {
                return this.mHotKeyIdle;
            }
            set
            {
                this.mHotKeyIdle = value;
            }
        }

        public string HotKeyBusy
        {
            get
            {
                return this.mHotKeyBusy;
            }
            set
            {
                this.mHotKeyBusy = value;
            }
        }

        public string HotKeyLeave
        {
            get
            {
                return this.mHotKeyLeave;
            }
            set
            {
                this.mHotKeyLeave = value;
            }
        }

        public string hotKeyCallOut
        {
            get
            {
                return this.mHotKeyCallOut;
            }
            set
            {
                this.mHotKeyCallOut = value;
            }
        }

        public string hotKeyMonitor
        {
            get
            {
                return this.mHotKeyMonitor;
            }
            set
            {
                this.mHotKeyMonitor = value;
            }
        }

        public string DefaultAccessNum
        {
            get
            {
                return this.mDefaultAccessNum;
            }
            set
            {
                this.mDefaultAccessNum = value;
            }
        }

        public List<string> CalloutHistory
        {
            get
            {
                return this.mCalloutHistory;
            }
            set
            {
                this.mCalloutHistory = value;
            }
        }

        public bool IsMonitorOfflineAgent
        {
            get
            {
                return this.mIsMonitorOfflineAgent;
            }
            set
            {
                this.mIsMonitorOfflineAgent = value;
            }
        }

        public int RefreshReportStatisInterval
        {
            get
            {
                return this.mRefreshReportStatisInterval;
            }
            set
            {
                this.mRefreshReportStatisInterval = value;
            }
        }

        public Dictionary<string, string> GetControlAgentGroupInfo
        {
            get
            {
                return this.mGroupMap;
            }
        }

        public Agent_Status AgentStateAfterHangup
        {
            get
            {
                return this.mAgentStateAfterHangup;
            }
            set
            {
                this.mAgentStateAfterHangup = value;
            }
        }

        public bool SipAutoSignIn
        {
            get
            {
                return this.mSipAutoSignIn;
            }
            set
            {
                this.mSipAutoSignIn = value;
            }
        }

        public bool SipPhoneOnLineWarning
        {
            get
            {
                return this.mSipPhoneOnLineWarning;
            }
            set
            {
                this.mSipPhoneOnLineWarning = value;
            }
        }

        public string NoAnswerCallsURL
        {
            get
            {
                return this.mNoAnswerCallsURL;
            }
            set
            {
                this.mNoAnswerCallsURL = value;
            }
        }

        public CallStatus GetCallStatus
        {
            get
            {
                return this.mCallStatus;
            }
        }

        protected override void Dispose()
        {
            if (this.agentDll != null)
            {
                this.agentDll.Dispose();
            }
        }

        private void InitializeComponent()
        {
            
        }

        [DllImport("user32.dll")]
        public static extern IntPtr FindWindow(string class_name, string app_name);

        [DllImport("User32.dll", EntryPoint = "SendMessage")]
        private static extern int PostMessage(IntPtr wnd, int msg, IntPtr wP, IntPtr lP);

        public void ProcessDelegate(AgentBarEventDelegate myDelegate, string jsonArgsStr)
        {
            myDelegate(jsonArgsStr);
        }

       
        

        private string HangupFailReason2Chinese(string hangupFailedReason)
        {
            string result;
            if (string.IsNullOrEmpty(hangupFailedReason))
            {
                result = "";
            }
            else
            {
                Log.Debug("enter HangupFailReason2Chinese .hangupReason:" + hangupFailedReason);
                string text = hangupFailedReason.ToUpper();
                string strReason;
                switch (text)
                {
                    case "TELMGR_RESULT_PARAM_ERROR":
                        strReason = "挂断失败：参数错误";
                        goto IL_122;
                    case "TELMGR_RESULT_CALL_IS_LOCKED":
                        strReason = "挂断失败：电话正在处理中";
                        goto IL_122;
                    case "TELMGR_RESULT_EXTEN_NOT_EXIST":
                        strReason = "挂断失败：分机不存在";
                        goto IL_122;
                    case "TELMGR_RESULT_OPERATER_STATUS_ERROR":
                        strReason = "挂断失败：电话预占中";
                        goto IL_122;
                    case "TELMGR_RESULT_IXAPI_NOT_INITIALIZED":
                        strReason = "挂断失败：服务器未初始化";
                        goto IL_122;
                    case "TELMGR_RESULT_FATAL_ERROR":
                        strReason = "挂断失败：处理异常";
                        goto IL_122;
                    case "TELMGR_RESULT_UUID_NOT_FOUND":
                        strReason = "挂断失败：电话信息出错";
                        goto IL_122;
                }
                strReason = "挂断失败：未知错误";
                IL_122:
                result = strReason;
            }
            return result;
        }

        private string ThreeWayHangupReason2Chinese(string hangupReason)
        {
            string result;
            if (hangupReason == null)
            {
                result = "";
            }
            else
            {
                Log.Debug("enter ThreeWayHangupReason2Chinese .hangupReason:" + hangupReason);
                string text = hangupReason.ToUpper();
                string strReason;
                if (text != null)
                {
                    if (text == "NO_ANSWER")
                    {
                        strReason = "三方失败：未接";
                        goto IL_B0;
                    }
                    if (text == "ORIGINATOR_CANCEL")
                    {
                        strReason = "三方失败：对方挂断";
                        goto IL_B0;
                    }
                    if (text == "NORMAL_CLEARING")
                    {
                        strReason = "已挂断";
                        goto IL_B0;
                    }
                    if (text == "CALLOUT_FAILED")
                    {
                        strReason = "三方失败：呼叫失败";
                        goto IL_B0;
                    }
                    if (text == "ALL_GATEWAYS_FULL")
                    {
                        strReason = "三方失败：外线满线";
                        goto IL_B0;
                    }
                }
                strReason = "三方失败：拒接";
                IL_B0:
                result = strReason;
            }
            return result;
        }

        private string ConsulteeHangupReason2Chinese(string hangupReason)
        {
            string result;
            if (hangupReason == null)
            {
                result = "";
            }
            else
            {
                Log.Debug("enter ConsulteeHangupReason2Chinese .hangupReason:" + hangupReason);
                string text = hangupReason.ToUpper();
                string strReason;
                if (text != null)
                {
                    if (text == "NO_ANSWER")
                    {
                        strReason = "询问失败：被询问者未接";
                        goto IL_B0;
                    }
                    if (text == "ORIGINATOR_CANCEL")
                    {
                        strReason = "询问失败：对方挂断";
                        goto IL_B0;
                    }
                    if (text == "NORMAL_CLEARING")
                    {
                        strReason = "询问结束";
                        goto IL_B0;
                    }
                    if (text == "CALLOUT_FAILED")
                    {
                        strReason = "询问失败";
                        goto IL_B0;
                    }
                    if (text == "ALL_GATEWAYS_FULL")
                    {
                        strReason = "询问失败：外线满线";
                        goto IL_B0;
                    }
                }
                strReason = "询问失败：被询问者拒接";
                IL_B0:
                result = strReason;
            }
            return result;
        }

        

        public static Apply_State IntStr2ApplyState(string strApplyStateStr)
        {
            Apply_State result;
            try
            {
                if ("" != strApplyStateStr)
                {
                    result = (Apply_State)Enum.Parse(typeof(Apply_State), strApplyStateStr);
                }
                else
                {
                    result = Apply_State.Unknow;
                }
            }
            catch (Exception ex_32)
            {
                result = Apply_State.Unknow;
            }
            return result;
        }

        public PLAgent()
        {
            this.InitializeComponent();
            XmlConfigurator.Configure(new FileInfo(ComFunc.APPDATA_PATH + "\\CTIClient\\log4net.config"));
            Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
            Log.Info("AgentBar init success！");
            this.agentDll = new AgentDll();
            this.agentDll.AgentEvents += new AgentDll.EVT_AgentDelegate(this.ReceiveAgentEvents);
            this.agentDll.AgentHeartbeatEvents += new AgentDll.HeartBeatEVT_AgentDelegate(this.ReceiveAgentHeartBeatEvents);
            this.UserStateChangeEvent += this.OnUserStateChange; //(AgentBar.UserStateEventHandler)Delegate.Combine(this.UserStateChangeEvent, new AgentBar.UserStateEventHandler(this.OnUserStateChange));
            this.ResponseEvent += this.OnResponse; //(AgentBar.ResponseEventHandler)Delegate.Combine(this.ResponseEvent, new AgentBar.ResponseEventHandler(this.OnResponse));
            this.EventResultEvent += this.OnEventResultEvent;// (AgentBar.EventResultEventHandler)Delegate.Combine(this.EventResultEvent, new AgentBar.EventResultEventHandler(this.OnEventResultEvent));
            this.SockDisconnectEvent += this.OnSockDisconnectEvent; //(AgentBar.SockDisconnectEventHandler)Delegate.Combine(this.SockDisconnectEvent, new AgentBar.SockDisconnectEventHandler(this.OnSockDisconnectEvent));
            this.ResponseTimeOutEvent += this.OnResponseTimeOutEvent;// (AgentBar.ResponseTimeOutEventHandler)Delegate.Combine(this.ResponseTimeOutEvent, new AgentBar.ResponseTimeOutEventHandler(this.OnResponseTimeOutEvent));
            this.SignOutEvent += this.OnSignOutEvent;// (AgentBar.SignOutEventHandler)Delegate.Combine(this.SignOutEvent, new AgentBar.SignOutEventHandler(this.OnSignOutEvent));
            this.mGroupMap = new Dictionary<string, string>();
            this.mRoleMap = new Dictionary<string, string>();
            this.apply_change_status_approval_history = new List<Apply_Change_Status>();
            this.apply_change_status_to_approval_lst = new List<Apply_Change_Status>();
            this.sndPlayer = new SoundPlayer();
            this.initAgentTool();
            this.mCalloutHistory = new List<string>();
            this.initMyApplyChangeStatus();
            this.mSipAutoSignIn = false;
        }

        public static int ChkIsTalking(string agent_status)
        {
            int result;
            if (string.IsNullOrEmpty(agent_status))
            {
                result = -1;
            }
            else if (Utils.Str2AgentStatus(agent_status) == Agent_Status.AGENT_STATUS_HOLD || 
                Utils.Str2AgentStatus(agent_status) == Agent_Status.AGENT_STATUS_MUTE || 
                Utils.Str2AgentStatus(agent_status) == Agent_Status.AGENT_STATUS_TALKING)
            {
                result = 0;
            }
            else
            {
                result = -2;
            }
            return result;
        }

        public static int ChkStateIsHoldOrMute(string agent_status)
        {
            int result;
            if (string.IsNullOrEmpty(agent_status))
            {
                result = -1;
            }
            else if (Utils.Str2AgentStatus(agent_status) == Agent_Status.AGENT_STATUS_TALKING)
            {
                result = 0;
            }
            else
            {
                result = -2;
            }
            return result;
        }

        public static int ChkStateIsTalking(string agent_status)
        {
            int result;
            if (string.IsNullOrEmpty(agent_status))
            {
                result = -1;
            }
            else if (Utils.Str2AgentStatus(agent_status) == Agent_Status.AGENT_STATUS_HOLD || Utils.Str2AgentStatus(agent_status) == Agent_Status.AGENT_STATUS_MUTE)
            {
                result = 0;
            }
            else
            {
                result = -2;
            }
            return result;
        }

        

        public void Evt_SetAgentStatusHotKeyString(string hotKeyIdle, string hotKeyBusy, 
            string hotKeyLeave, string hotKeyCallOut, string hotKeyMonitor)
        {
            this.mHotKeyIdle = hotKeyIdle;
            this.mHotKeyBusy = hotKeyBusy;
            this.mHotKeyLeave = hotKeyLeave;
            this.mHotKeyCallOut = hotKeyCallOut;
            this.mHotKeyMonitor = hotKeyMonitor;
        }

        public int SetSoftphoneIsAutoAnswer(bool blnAutoAnswer)
        {
            string cfgFileName = ComFunc.SOFTPHONE_CONFIG_FOLDER_PATH + "\\" + this.SipNum + "\\config.xml";
            int result;
            if (File.Exists(cfgFileName))
            {
                string strAutoAnswer = "0";
                if (blnAutoAnswer)
                {
                    strAutoAnswer = "1";
                }
                if (!AgentBar.write_conf_to_softphone(cfgFileName, "autoanswer", strAutoAnswer))
                {
                    MessageBox.Show("保存文件失败！", "保存失败", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    AgentBar.Log.Warn("write_conf_to_softphone 执行失败!");
                    result = -1;
                }
                else
                {
                    result = 0;
                }
            }
            else
            {
                AgentBar.Log.Warn("配置文件不存在！" + cfgFileName);
                result = -2;
            }
            return result;
        }

        public int SetSoftphoneIsAutoAnswer(int registTime)
        {
            string cfgFileName = ComFunc.SOFTPHONE_CONFIG_FOLDER_PATH + "\\" + this.SipNum + "\\config.xml";
            int result;
            if (File.Exists(cfgFileName))
            {
                if (!AgentBar.write_conf_to_softphone(cfgFileName, "registtime", registTime.ToString()))
                {
                    MessageBox.Show("保存文件失败！", "保存失败", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    AgentBar.Log.Warn("write_conf_to_softphone 执行失败!");
                    result = -1;
                }
                else
                {
                    result = 0;
                }
            }
            else
            {
                AgentBar.Log.Warn("配置文件不存在！" + cfgFileName);
                result = -2;
            }
            return result;
        }

        public static bool write_conf_to_softphone(string filePath, string strKey, string strValue)
        {
            bool result;
            try
            {
                Log.Debug(string.Concat(new string[]
                {
                    "enter write_conf_to_softphone,filePath=",
                    filePath,
                    ",strKey=",
                    strKey,
                    ",strValue=",
                    strValue
                }));
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(filePath);
                XmlNode xns = xmlDoc.SelectSingleNode("TERMINAL");
                XmlNodeList xnl = xns.ChildNodes;
                foreach (XmlNode xn in xnl)
                {
                    if (xn.NodeType == XmlNodeType.Element)
                    {
                        XmlElement xe = (XmlElement)xn;
                        if (xe.Name == "GENERALCONFIG")
                        {
                            XmlNodeList xnls = xe.ChildNodes;
                            foreach (XmlNode xnls2 in xnls)
                            {
                                if (xnls2.Name.ToLower() == strKey)
                                {
                                    xnls2.InnerText = strValue;
                                    break;
                                }
                            }
                            break;
                        }
                    }
                }
                xmlDoc.Save(filePath);
            }
            catch (Exception e)
            {
                Log.Error(e.Source + e.Message + e.StackTrace);
                result = false;
                return result;
            }
            result = true;
            return result;
        }

        private void OnEvent_CommonCallIn(string agentID, string agent_call_uuid, string callerID, 
            string calledID, string accessNumName, string makeStr, string callType, 
            string relation_uuid, string areaID, string areaName, string custGrade, 
            string outExtraParamsFromIvr, string todayDate, string customer_num_format_local, 
            string customer_num_format_national, string customer_num_format_e164, 
            string customer_num_phone_type, string predictCustomerForeignId, 
            string predictCustomerName, string predictCustomerRemark)
        {
            Log.Debug(string.Concat(new string[]
            {
                "enter OnEvent_CommonCallIn.agentID:",
                agentID,
                ",agent_call_uuid:",
                agent_call_uuid,
                ",callerID",
                callerID,
                ",calledID:",
                calledID,
                ",accessNumName:",
                accessNumName,
                ",makeStr:",
                makeStr,
                ",callType:",
                callType,
                ",relation_uuid:",
                relation_uuid,
                ",areaID:",
                areaID,
                ",areaName:",
                areaName,
                ",custGrade:",
                custGrade,
                ",outExtraParamsFromIvr:",
                outExtraParamsFromIvr,
                ",todayDate:",
                todayDate,
                ",predictCustomerForeignId:",
                predictCustomerForeignId,
                ",predictCustomerName:",
                predictCustomerName,
                ",predictCustomerRemark:",
                predictCustomerRemark
            }));
            this.mCallType = Call_Type.COMMON_CALL_IN;
            this.mCallStatus = CallStatus.CALL_IN;
            this.m_agent_current_call_uuid = agent_call_uuid;
            if (this.CallInEvent != null)
            {
                makeStr = string.Concat(new string[]
                {
                    makeStr,
                    "customer_num=",
                    callerID,
                    "#callcenter_num=",
                    calledID,
                    "#access_num_name=",
                    accessNumName,
                    "#agent_num=",
                    agentID,
                    "#relation_uuid=",
                    relation_uuid,
                    "#area_id=",
                    areaID,
                    "#area_name=",
                    areaName,
                    "#cust_grade=",
                    custGrade,
                    "#customer_num_format_local=",
                    customer_num_format_local,
                    "#customer_num_format_national=",
                    customer_num_format_national,
                    "#customer_num_format_e164=",
                    customer_num_format_e164,
                    "#customer_num_phone_type=",
                    customer_num_phone_type,
                    "#agent_current_call_uuid=",
                    agent_call_uuid,
                    "#predictCustomerForeignId=",
                    predictCustomerForeignId,
                    "#predictCustomerName=",
                    predictCustomerName,
                    "#predictCustomerRemark=",
                    predictCustomerRemark,
                    "#"
                });
                this.CallInEvent(callerID, makeStr, callType, true, ComFunc.PLDESDecrypt(outExtraParamsFromIvr, this.SaltKey, todayDate));
            }
        }

        private void OnEvent_PredictCallOutBridgeRing(string callerID, string calledID, string accessNumName, string makeStr, string callType, string areaID, string areaName, string outExtraParamsFromIvr, string todayDate, string customer_num_format_local, string customer_num_format_national, string customer_num_format_e164, string customer_num_phone_type)
        {
            Log.Debug(string.Concat(new string[]
            {
                "enter OnEvent_PredictCallOutBridgeRing. callerID",
                callerID,
                ",calledID:",
                calledID,
                ",accessNumName:",
                accessNumName,
                ",makeStr:",
                makeStr,
                ",callType:",
                callType,
                ",areaID:",
                areaID,
                ",areaName:",
                areaName
            }));
            this.mCallType = Call_Type.PREDICT_CALL_OUT;
            this.mCallStatus = CallStatus.CALL_OUT;
            this.update_Toolbar_UI(Event_Type.CALLIN_PREDICT_CALL, "");
            if (this.PredictCallOutBridgeRingEvent != null)
            {
                makeStr = string.Concat(new string[]
                {
                    makeStr,
                    "area_id=",
                    areaID,
                    "#area_name=",
                    areaName,
                    "#customer_num_format_local=",
                    customer_num_format_local,
                    "#customer_num_format_national=",
                    customer_num_format_national,
                    "#customer_num_format_e164=",
                    customer_num_format_e164,
                    "#customer_num_phone_type=",
                    customer_num_phone_type,
                    "#"
                });
                this.PredictCallOutBridgeRingEvent(callerID, makeStr, callType, true, ComFunc.PLDESDecrypt(outExtraParamsFromIvr, this.SaltKey, todayDate));
            }
        }

        private void OnEvent_Internal_Call_CallIn(string agentID, string agent_call_uuid, string calledID, string callType, string relation_uuid)
        {
            Log.Debug(string.Concat(new string[]
            {
                "enter OnEvent_Internal_Call_CallIn. agentID",
                agentID,
                ",agent_call_uuid:",
                agent_call_uuid,
                ",calledID:",
                calledID,
                ",callType:",
                callType,
                ",relation_uuid:",
                relation_uuid
            }));
            this.mCallType = Call_Type.AGENT_INTERNAL_CALL;
            this.mCallStatus = CallStatus.CALL_IN;
            this.m_agent_current_call_uuid = agent_call_uuid;
            this.update_Toolbar_UI(Event_Type.CALLIN_INTERNAL_MYSELF, "内部呼叫");
            string makeStr = string.Empty;
            makeStr = string.Concat(new string[]
            {
                "#agent_current_call_uuid=",
                agent_call_uuid,
                "#relation_uuid=",
                relation_uuid,
                "#"
            });
        }

        private void OnEvent_Consult(string agentID, int retCode, string reason)
        {
            Log.Debug(string.Concat(new object[]
            {
                "enter OnEvent_Consult. agentID",
                agentID,
                ",retCode:",
                retCode,
                ",reason:",
                reason
            }));
            if (retCode == 0)
            {
                this.update_Toolbar_UI(Event_Type.CONSULT_SUCCESS, "");
            }
            else
            {
                Log.Debug("OnEvent_Consult has error!reason=" + reason);
                this.update_Toolbar_UI(Event_Type.CONSULT_FAIL, "");
            }
            this.Consultcheck = false;
        }

        private void OnEvent_hold_Result(string agentID, int retCode, string reason)
        {
            Log.Debug(string.Concat(new object[]
            {
                "enter OnEvent_hold_Result .agentID:",
                agentID,
                ",retCode:",
                retCode,
                ",reason:",
                reason
            }));
        }

        private void OnEvent_Unhold_Result(string agentID, int retCode, string reason)
        {
            Log.Debug(string.Concat(new object[]
            {
                "enter OnEvent_Unhold_Result .agentID:",
                agentID,
                ",retCode:",
                retCode,
                ",reason:",
                reason
            }));
            
        }

        private void OnEvent_Mute_Result(string agentID, int retCode, string reason)
        {
            Log.Debug(string.Concat(new object[]
            {
                "enter OnEvent_mute_Result .agentID:",
                agentID,
                ",retCode:",
                retCode,
                ",reason:",
                reason
            }));
        }

        private void OnEvent_Unmute_Result(string agentID, int retCode, string reason)
        {
            
        }

        private void OnEvent_Consult_Cancel(string agentID, int retCode, string reason)
        {
            
        }

        private void OnEvent_Consult_Transfer(string agentID, int retCode, string reason)
        {
            
        }

        private void OnEvent_Consult_Callin(string agentID, string agent_call_uuid, 
            string callerID, string calledID, string accessNumName, string consulterAgentNum, 
            string callType, string areaID, string areaName, string custGrade, string outExtraParamsFromIvr, 
            string todayDate, string customer_num_format_local, string customer_num_format_national, 
            string customer_num_format_e164, string customer_num_phone_type, string customerForeignId, 
            string predictCustomerForeignId, string predictCustomerName, string predictCustomerRemark, string relation_uuid)
        {
            
            this.mCallType = Call_Type.CONSULT_CALL_IN;
            this.mCallStatus = CallStatus.CALL_IN;
            this.m_agent_current_call_uuid = agent_call_uuid;

            string makeStr = string.Empty;
            makeStr = string.Concat(new string[]
            {
                "#customer_num_format_local=",
                customer_num_format_local,
                "#customer_num_format_national=",
                customer_num_format_national,
                "#customer_num_format_e164=",
                customer_num_format_e164,
                "#customer_num_phone_type=",
                customer_num_phone_type,
                "#customerForeignId=",
                customerForeignId,
                "#agent_current_call_uuid=",
                agent_call_uuid,
                "#predictCustomerForeignId=",
                predictCustomerForeignId,
                "#predictCustomerName=",
                predictCustomerName,
                "#predictCustomerRemark=",
                predictCustomerRemark,
                "#relation_uuid=",
                relation_uuid,
                "#"
            });
            if (this.ConsultCallInEvent != null)
            {
                this.ConsultCallInEvent(agentID, callerID, calledID, accessNumName, 
                    consulterAgentNum, callType, areaID, areaName, custGrade, 
                    ComFunc.PLDESDecrypt(outExtraParamsFromIvr, this.SaltKey, todayDate), makeStr);
            }
        }

        private void OnEvent_Transfer_Blind_Call_In(string agentID, string agent_call_uuid, string callerID, string calledID, string accessNumName, string callerAgentNum, string callType, string areaID, string areaName, string cust_grade, string outExtraParamsFromIvr, string todayDate, string customer_num_format_local, string customer_num_format_national, string customer_num_format_e164, string customer_num_phone_type, string customerForeignId, string predictCustomerForeignId, string predictCustomerName, string predictCustomerRemark, string relation_uuid)
        {
            Log.Debug(string.Concat(new string[]
            {
                "enter OnEvent_Transfer_Blind_Call_In.agentID:",
                agentID,
                ",agent_call_uuid:",
                agent_call_uuid,
                ",callerID",
                callerID,
                ",calledID:",
                calledID,
                ",accessNumName:",
                accessNumName,
                ",callerAgentNum:",
                callerAgentNum,
                ",callType:",
                callType,
                ",areaID:",
                areaID,
                ",areaName:",
                areaName,
                ",cust_grade:",
                cust_grade,
                ",outExtraParamsFromIvr:",
                outExtraParamsFromIvr,
                ",todayDate:",
                todayDate,
                ",customer_num_format_local:",
                customer_num_format_local,
                ",customer_num_format_national:",
                customer_num_format_national,
                ",customer_num_format_e164:",
                customer_num_format_e164,
                ",customer_num_phone_type:",
                customer_num_phone_type,
                ",customerForeignId:",
                customerForeignId,
                ",predictCustomerForeignId:",
                predictCustomerForeignId,
                ",predictCustomerName:",
                predictCustomerName,
                ",predictCustomerRemark:",
                predictCustomerRemark,
                ",relation_uuid:",
                relation_uuid
            }));
            this.mCallType = Call_Type.COMMON_CALL_IN;
            this.mCallStatus = CallStatus.CALL_IN;
            this.m_agent_current_call_uuid = agent_call_uuid;

            string makeStr = string.Empty;
            makeStr = string.Concat(new string[]
            {
                "#customer_num_format_local=",
                customer_num_format_local,
                "#customer_num_format_national=",
                customer_num_format_national,
                "#customer_num_format_e164=",
                customer_num_format_e164,
                "#customer_num_phone_type=",
                customer_num_phone_type,
                "#customerForeignId=",
                customerForeignId,
                "#agent_current_call_uuid=",
                agent_call_uuid,
                "#predictCustomerForeignId=",
                predictCustomerForeignId,
                "#predictCustomerName=",
                predictCustomerName,
                "#predictCustomerRemark=",
                predictCustomerRemark,
                "#relation_uuid=",
                relation_uuid,
                "#"
            });
            if (this.TransferBlindCallInEvent != null)
            {
                this.TransferBlindCallInEvent(agentID, callerID, calledID, accessNumName, 
                    callerAgentNum, callType, areaID, areaName, cust_grade, 
                    ComFunc.PLDESDecrypt(outExtraParamsFromIvr, this.SaltKey, todayDate), makeStr);
            }
        }

        private void OnEvent_Transfer_Agent(string agentID, int retCode, string reason)
        {
            Log.Debug(string.Concat(new object[]
            {
                "enter OnEvent_Transfer_Agent.agentID:",
                agentID,
                ",retCode:",
                retCode,
                ",reason:",
                reason
            }));
            
            this.transfercheck = false;
        }

        private void OnEvent_Transfer_Ivr(string agentID, int retCode, string reason)
        {
            Log.Debug(string.Concat(new object[]
            {
                "enter OnEvent_Transfer_Ivr.agentID:",
                agentID,
                ",retCode:",
                retCode,
                ",reason:",
                reason
            }));
            
        }

        private void OnEvent_Transfer_Queue(string agentID, int retCode, string reason)
        {
            Log.Debug(string.Concat(new object[]
            {
                "enter OnEvent_Transfer_Queue.agentID:",
                agentID,
                ",retCode:",
                retCode,
                ",reason:",
                reason
            }));
            
        }

        private void OnEvent_Transfer_Ivr_Profile(string agentID, int retCode, string reason)
        {
            Log.Debug(string.Concat(new object[]
            {
                "enter OnEvent_Transfer_Ivr_Profile.agentID:",
                agentID,
                ",retCode:",
                retCode,
                ",reason:",
                reason
            }));
            
        }

        private void OnEvent_Get_Access_Number(string agentID, string reason, int retCode, string[] accessNumbers)
        {
            Log.Debug(string.Concat(new object[]
            {
                "enter OnEvent_Get_Access_Number.agentID:",
                agentID,
                ",reason:",
                reason,
                ",retCode:",
                retCode
            }));
            if (retCode == 0)
            {
                if (this.GetAccessNumberEvent != null)
                {
                    this.GetAccessNumberEvent(accessNumbers, this.mDefaultAccessNum, this.mCalloutHistory);
                }
                if (this.JSGetAccessNumberEvent != null)
                {
                    string[] strArgsLst = new string[]
                    {
                        "accessNumbers",
                        "mDefaultAccessNum",
                        "mCalloutHistory"
                    };
                    this.ProcessAgentBarJsonDelegate(new object[]
                    {
                        this.JSGetAccessNumberEvent,
                        strArgsLst,
                        accessNumbers,
                        this.mDefaultAccessNum,
                        this.mCalloutHistory
                    });
                }
            }
        }

        private void OnEvent_Three_Way_Call(string agentID, string reason, int retCode)
        {
            
        }

        private void OnEvent_Three_Way_Cancel(string agentID, string reason, int retCode)
        {
            
        }

        private void OnEvent_Three_Way_Call_In(string agentID, string agent_call_uuid, string callerID, string calledID, string accessNumName, string callerAgentNum, string callType, string areaID, string areaName, string custGrade, string outExtraParamsFromIvr, string todayDate, string customer_num_format_local, string customer_num_format_national, string customer_num_format_e164, string customer_num_phone_type, string customerForeignId, string predictCustomerForeignId, string predictCustomerName, string predictCustomerRemark, string relation_uuid)
        {
            
            this.mCallType = Call_Type.THREEWAY_CALL_IN;
            this.mCallStatus = CallStatus.CALL_IN;
            this.m_agent_current_call_uuid = agent_call_uuid;
            string makeStr = string.Empty;
            makeStr = string.Concat(new string[]
            {
                "#customer_num_format_local=",
                customer_num_format_local,
                "#customer_num_format_national=",
                customer_num_format_national,
                "#customer_num_format_e164=",
                customer_num_format_e164,
                "#customer_num_phone_type=",
                customer_num_phone_type,
                "#customerForeignId=",
                customerForeignId,
                "#agent_current_call_uuid=",
                agent_call_uuid,
                "#predictCustomerForeignId=",
                predictCustomerForeignId,
                "#predictCustomerName=",
                predictCustomerName,
                "#predictCustomerRemark=",
                predictCustomerRemark,
                "#relation_uuid=",
                relation_uuid,
                "#"
            });
            if (this.ThreeWayCallRingEvent != null)
            {
                this.ThreeWayCallRingEvent(agentID, callerID, calledID, accessNumName, callerAgentNum, callType, areaID, areaName, custGrade, ComFunc.PLDESDecrypt(outExtraParamsFromIvr, this.SaltKey, todayDate), makeStr);
            }
        }

        private void OnEvent_Eavesdrop(string agentID, string reason, int retCode)
        {
            if (this.EavesdropEvent != null)
            {
                this.EavesdropEvent(agentID, reason, retCode);
            }
        }

        private void OnEvent_Whisper(string agentID, string reason, int retCode)
        {
            if (this.WhisperEvent != null)
            {
                this.WhisperEvent(agentID, reason, retCode);
            }
        }

        private void OnEvent_Force_Change_Status(string agentID, string reason, int retCode)
        {
            if (this.ForceChangeStatusEvent != null)
            {
                this.ForceChangeStatusEvent(agentID, reason, retCode);
            }
        }

        private void OnEvent_Bargein(string agentID, string reason, int retCode)
        {
            if (this.BargeinEvent != null)
            {
                this.BargeinEvent(agentID, reason, retCode);
            }
        }

        private void OnEvent_Eavesdrop_Ring(string agentID, string agent_call_uuid, string callerID, string calledID, string accessNumName, string desAgentID, string callType, string areaID, string areaName, string custGrade, string outExtraParamsFromIvr, string todayDate, string customer_num_format_local, string customer_num_format_national, string customer_num_format_e164, string customer_num_phone_type, string customerForeignId, string relation_uuid)
        {
            
            this.mCallType = AgentBar.Call_Type.EAVESDROP_CALL_IN;
            this.mCallStatus = AgentBar.CallStatus.CALL_OUT;
            this.mEavesdropAgent = desAgentID;
            this.m_agent_current_call_uuid = agent_call_uuid;
            this.update_Toolbar_UI(AgentBar.Event_Type.EAVESDROP_RING_MYSELF, "");
            string makeStr = string.Empty;
            makeStr = string.Concat(new string[]
            {
                "#customer_num_format_local=",
                customer_num_format_local,
                "#customer_num_format_national=",
                customer_num_format_national,
                "#customer_num_format_e164=",
                customer_num_format_e164,
                "#customer_num_phone_type=",
                customer_num_phone_type,
                "#customerForeignId=",
                customerForeignId,
                "#agent_current_call_uuid=",
                agent_call_uuid,
                "#relation_uuid=",
                relation_uuid,
                "#"
            });
            if (this.EavesdropCallRingEvent != null)
            {
                this.EavesdropCallRingEvent(agentID, callerID, calledID, accessNumName, desAgentID, callType, areaID, areaName, custGrade, ComFunc.PLDESDecrypt(outExtraParamsFromIvr, this.SaltKey, todayDate), makeStr);
            }
        }

        private void OnEvent_Whisper_Ring(string agentID, string agent_call_uuid, string callerID, string calledID, string accessNumName, string desAgentID, string callType, string areaID, string areaName, string custGrade, string outExtraParamsFromIvr, string todayDate, string customer_num_format_local, string customer_num_format_national, string customer_num_format_e164, string customer_num_phone_type, string customerForeignId, string relation_uuid)
        {
            
            this.mCallType = Call_Type.WHISPER_CALL_IN;
            this.mCallStatus = CallStatus.CALL_OUT;
            this.mEavesdropAgent = desAgentID;
            this.m_agent_current_call_uuid = agent_call_uuid;

            string makeStr = string.Empty;
            makeStr = string.Concat(new string[]
            {
                "#customer_num_format_local=",
                customer_num_format_local,
                "#customer_num_format_national=",
                customer_num_format_national,
                "#customer_num_format_e164=",
                customer_num_format_e164,
                "#customer_num_phone_type=",
                customer_num_phone_type,
                "#customerForeignId=",
                customerForeignId,
                "#agent_current_call_uuid=",
                agent_call_uuid,
                "#relation_uuid=",
                relation_uuid,
                "#"
            });
            if (this.WhisperCallRingEvent != null)
            {
                this.WhisperCallRingEvent(agentID, callerID, calledID, accessNumName, desAgentID, callType, areaID, areaName, custGrade, ComFunc.PLDESDecrypt(outExtraParamsFromIvr, this.SaltKey, todayDate), makeStr);
            }
        }

        private void OnEvent_Bargein_Ring(string agentID, string agent_call_uuid, string callerID, string calledID, string accessNumName, string desAgentID, string callType, string areaID, string areaName, string custGrade, string outExtraParamsFromIvr, string todayDate, string customer_num_format_local, string customer_num_format_national, string customer_num_format_e164, string customer_num_phone_type, string customerForeignId, string relation_uuid)
        {
            
            this.mCallType = Call_Type.BARGEIN_CALL_IN;
            this.mCallStatus = CallStatus.CALL_OUT;
            this.mEavesdropAgent = desAgentID;
            this.m_agent_current_call_uuid = agent_call_uuid;
            string makeStr = string.Empty;
            makeStr = string.Concat(new string[]
            {
                "#customer_num_format_local=",
                customer_num_format_local,
                "#customer_num_format_national=",
                customer_num_format_national,
                "#customer_num_format_e164=",
                customer_num_format_e164,
                "#customer_num_phone_type=",
                customer_num_phone_type,
                "#customerForeignId=",
                customerForeignId,
                "#agent_current_call_uuid=",
                agent_call_uuid,
                "#relation_uuid=",
                relation_uuid,
                "#"
            });
            if (this.BargeinCallRingEvent != null)
            {
                this.BargeinCallRingEvent(agentID, callerID, calledID, accessNumName, desAgentID, callType, areaID, areaName, custGrade, ComFunc.PLDESDecrypt(outExtraParamsFromIvr, this.SaltKey, todayDate), makeStr);
            }
        }

        private void OnEvent_Get_Online_Agent(string agentID, List<Agent_Online_Struct> agent_online, string reason, int retCode)
        {
            if (retCode == 0)
            {
                if (this.GetAgentOnlineEvent != null)
                {
                    this.GetAgentOnlineEvent(agent_online);
                }
                if (this.JSGetAgentOnlineEvent != null)
                {
                    string[] strArgsLst = new string[]
                    {
                        "agent_online"
                    };
                    this.ProcessAgentBarJsonDelegate(new object[]
                    {
                        this.JSGetAgentOnlineEvent,
                        strArgsLst,
                        agent_online
                    });
                }
            }
            else
            {
                Log.Debug("OnEvent_Get_Online_Agent has error!reason=" + reason);
            }
        }

        private void OnEvent_Get_Ivr_List(string agentID, Dictionary<string, string> ivr_list, string reason, int retCode)
        {
            
            if (retCode == 0)
            {
                if (this.GetIvrListEvent != null)
                {
                    this.GetIvrListEvent(ivr_list);
                }
                if (this.JSGetIvrListEvent != null)
                {
                    string[] strArgsLst = new string[]
                    {
                        "ivr_list"
                    };
                    this.ProcessAgentBarJsonDelegate(new object[]
                    {
                        this.JSGetIvrListEvent,
                        strArgsLst,
                        ivr_list
                    });
                }
            }
            else
            {
                Log.Debug("OnEvent_Get_Ivr_List has error!reason=" + reason);
            }
        }

        private void OnEvent_Get_Queue_List(string agentID, Dictionary<string, string> queue_list, string reason, int retCode)
        {
            if (retCode == 0)
            {
                if (this.GetQueueListEvent != null)
                {
                    this.GetQueueListEvent(queue_list);
                }
                if (this.JSGetQueueListEvent != null)
                {
                    string[] strArgsLst = new string[]
                    {
                        "queue_list"
                    };
                    this.ProcessAgentBarJsonDelegate(new object[]
                    {
                        this.JSGetQueueListEvent,
                        strArgsLst,
                        queue_list
                    });
                }
            }
        }

        private void OnEvent_Get_Ivr_Profile_List(string agentID, Dictionary<string, string> ivr_profile_list, string reason, int retCode)
        { 
            if (retCode == 0)
            {
                if (this.GetIvrProfileListEvent != null)
                {
                    this.GetIvrProfileListEvent(ivr_profile_list);
                }
                if (this.JSGetIvrProfileListEvent != null)
                {
                    string[] strArgsLst = new string[]
                    {
                        "ivr_profile_list"
                    };
                    this.ProcessAgentBarJsonDelegate(new object[]
                    {
                        this.JSGetIvrProfileListEvent,
                        strArgsLst,
                        ivr_profile_list
                    });
                }
            }
            else
            {
                Log.Debug("OnEvent_Get_Ivr_Profile_List has error!reason=" + reason);
            }
        }

        private void OnEvent_Get_Defined_Role_Rights(string agentID, List<Agent_Role_And_Right_Struct> agent_role_and_right, string reason, int retCode)
        {
            
            if (retCode == 0)
            {
                this.mRoleMap.Clear();
                this.mAgentRoleAndRight = agent_role_and_right;
                int i;
                for (i = 0; i < agent_role_and_right.Count; i++)
                {
                    this.mRoleMap.Add(agent_role_and_right[i].role_id, agent_role_and_right[i].role_name);
                    if (agent_role_and_right[i].role_id == this.mRoleID)
                    {
                        this.mRoleName = agent_role_and_right[i].role_name;
                        this.mMyRoleAndRight = agent_role_and_right[i];
                        //this.show_agent_bar_by_right(ref this.mMyRoleAndRight);
                        if (this.GetRoleNameEvent != null)
                        {
                            this.GetRoleNameEvent(this.mRoleName);
                        }
                    }
                }
                this.load_default_status();
            }
            else
            {
                Log.Debug("OnEvent_Get_Defined_Role_Rights has error!reason=" + reason);
            }
        }

        private void OnEvent_Agent_Group_List(string agentID, Dictionary<string, string> agent_group_list, string reason, int retCode)
        {
            
            if (retCode == 0)
            {
                this.mGroupMap.Clear();
                this.mGroupMap = agent_group_list;
                if (this.GetAllAgentGroupListEvent != null)
                {
                    this.GetAllAgentGroupListEvent(agent_group_list);
                }
                if (this.JSGetAllAgentGroupListEvent != null)
                {
                    string[] strArgsLst = new string[]
                    {
                        "agent_group_list"
                    };
                    this.ProcessAgentBarJsonDelegate(new object[]
                    {
                        this.JSGetAllAgentGroupListEvent,
                        strArgsLst,
                        agent_group_list
                    });
                }
                if (this.mMyRoleAndRight.controled_agent_group_lst != null && !(this.mMyRoleAndRight.controled_agent_group_lst == ""))
                {
                    string[] sArray = this.mMyRoleAndRight.controled_agent_group_lst.Split(new char[]
                    {
                        ','
                    });
                    Dictionary<string, string> new_agent_group_list = new Dictionary<string, string>();
                    string agentGroupNameLstrStr = "";
                    string[] array = sArray;
                    for (int j = 0; j < array.Length; j++)
                    {
                        string i = array[j];
                        if (agent_group_list.ContainsKey(i))
                        {
                            new_agent_group_list.Add(i, agent_group_list[i]);
                            if (agentGroupNameLstrStr == "")
                            {
                                agentGroupNameLstrStr = agent_group_list[i];
                            }
                            else
                            {
                                agentGroupNameLstrStr = agentGroupNameLstrStr + "," + agent_group_list[i];
                            }
                        }
                    }
                    this.mMyRoleAndRight.controled_agent_group_name_lst = agentGroupNameLstrStr;
                    if (this.GetAgentGroupListEvent != null)
                    {
                        this.GetAgentGroupListEvent(new_agent_group_list);
                    }
                    if (this.JSGetAgentGroupListEvent != null)
                    {
                        string[] strArgsLst = new string[]
                        {
                            "new_agent_group_list"
                        };
                        this.ProcessAgentBarJsonDelegate(new object[]
                        {
                            this.JSGetAgentGroupListEvent,
                            strArgsLst,
                            new_agent_group_list
                        });
                    }
                }
            }
            else
            {
                Log.Debug("OnEvent_Agent_Group_List has error!reason=" + reason);
            }
        }

        private void OnEvent_Get_Agents_Of_Queue(string agentID, string agent_list, string queue_num, string reason, int retCode)
        {
            
            if (retCode == 0)
            {
                if (this.GetAgentsOfQueueEvent != null)
                {
                    this.GetAgentsOfQueueEvent(agent_list, queue_num);
                }
                if (!this.DoGetAgentsMonitorInfo(agent_list))
                {
                    Log.Debug("DoGetAgentsMonitorInfo has error! agent_list=" + agent_list);
                }
            }
            else
            {
                Log.Debug("OnEvent_Get_Agents_Of_Queue has error!reason=" + reason);
            }
        }

        private void OnEvent_Get_Agents_Of_AgentGroup(string agentID, string agents_str, string agent_group_num, string reason, int retCode)
        {
            
            if (retCode == 0)
            {
                if (this.GetAgentsOfAgentGroupEvent != null)
                {
                    this.GetAgentsOfAgentGroupEvent(agents_str, agent_group_num);
                }
                if (!this.DoGetAgentsMonitorInfo(agents_str))
                {
                    Log.Debug("DoGetAgentsMonitorInfo has error! agents_str=" + agents_str);
                }
            }
            else
            {
                Log.Debug("OnEvent_Get_Agents_Of_AgentGroup has error!reason=" + reason);
            }
        }

        private void OnEvent_Get_Agents_Monitor_info(string agentID, List<Agent_Online_Struct> agent_monitor_info, string current_time, string reason, int retCode)
        {
            
            if (retCode == 0)
            {
                if (this.GetAgentsMonitorInfoEvent != null)
                {
                    this.GetAgentsMonitorInfoEvent(agent_monitor_info, current_time, this.mGroupMap, this.mRoleMap);
                }
                if (this.JSGetAgentsMonitorInfoEvent != null)
                {
                    string[] strArgsLst = new string[]
                    {
                        "agent_monitor_info",
                        "current_time",
                        "mGroupMap",
                        "mRoleMap"
                    };
                    this.ProcessAgentBarJsonDelegate(new object[]
                    {
                        this.JSGetAgentsMonitorInfoEvent,
                        strArgsLst,
                        agent_monitor_info,
                        current_time,
                        this.mGroupMap,
                        this.mRoleMap
                    });
                }
            }
            else
            {
                Log.Debug("OnEvent_Get_Agents_Monitor_info has error!reason=" + reason);
            }
        }

        private void OnEvent_Get_Detail_Call_info(string agentID, string targetAgentNum, string callType, List<Leg_Info_Struct> leg_info, List<Relation_Info_Struct> relation_info, string reason, int retCode)
        {
            
            if (retCode == 0)
            {
                if (this.GetDetailCallInfoEvent != null)
                {
                    this.GetDetailCallInfoEvent(targetAgentNum, callType, leg_info, relation_info);
                }
                if (this.JSGetDetailCallInfoEvent != null)
                {
                    string[] strArgsLst = new string[]
                    {
                        "targetAgentNum",
                        "callType",
                        "leg_info",
                        "relation_info"
                    };
                    this.ProcessAgentBarJsonDelegate(new object[]
                    {
                        this.JSGetDetailCallInfoEvent,
                        strArgsLst,
                        targetAgentNum,
                        callType,
                        leg_info,
                        relation_info
                    });
                }
            }
        }

        private void OnEvent_Get_Customer_Of_Queue(string agentID, string queueNumLstStr, string current_time, List<Customer_Info_Struct> customer_list, string reason, int retCode)
        {
           
            if (retCode == 0)
            {
                if (this.GetCustomerOfQueueEvent != null)
                {
                    this.GetCustomerOfQueueEvent(queueNumLstStr, current_time, customer_list);
                }
                if (this.JSGetCustomerOfQueueEvent != null)
                {
                    string[] strArgsLst = new string[]
                    {
                        "queueNumLstStr",
                        "current_time",
                        "customer_list"
                    };
                    this.ProcessAgentBarJsonDelegate(new object[]
                    {
                        this.JSGetCustomerOfQueueEvent,
                        strArgsLst,
                        queueNumLstStr,
                        current_time,
                        customer_list
                    });
                }
            }
            else
            {
                Log.Debug("OnEvent_Get_Customer_Of_Queue has error!reason=" + reason);
            }
        }

        private void OnEvent_Get_Customer_Of_My_Queue(string agentID, string current_time, string queueNumLstStr, List<Customer_Info_Struct> customer_list, string reason, int retCode)
        {
            
            if (retCode == 0)
            {
                if (this.GetCustomerOfMyQueueEvent != null)
                {
                    this.GetCustomerOfMyQueueEvent(queueNumLstStr, current_time, customer_list);
                }
                if (this.JSGetCustomerOfMyQueueEvent != null)
                {
                    string[] strArgsLst = new string[]
                    {
                        "queueNumLstStr",
                        "current_time",
                        "customer_list"
                    };
                    this.ProcessAgentBarJsonDelegate(new object[]
                    {
                        this.JSGetCustomerOfMyQueueEvent,
                        strArgsLst,
                        queueNumLstStr,
                        current_time,
                        customer_list
                    });
                }
            }
            else
            {
                Log.Debug("OnEvent_Get_Customer_Of_My_Queue has error!reason=" + reason);
            }
        }

        private void OnEvent_Get_Queue_Statis_Info(string agentID, string current_time, string queueNumLstStr, List<Queue_Statis_Struct> queue_statis_list, string reason, int retCode)
        {
           
            if (retCode == 0)
            {
                if (this.GetQueueStatisLstEvent != null)
                {
                    this.GetQueueStatisLstEvent(queueNumLstStr, current_time, queue_statis_list);
                }
                if (this.JSGetQueueStatisLstEvent != null)
                {
                    string[] strArgsLst = new string[]
                    {
                        "queueNumLstStr",
                        "current_time",
                        "queue_statis_list"
                    };
                    this.ProcessAgentBarJsonDelegate(new object[]
                    {
                        this.JSGetQueueStatisLstEvent,
                        strArgsLst,
                        queueNumLstStr,
                        current_time,
                        queue_statis_list
                    });
                }
            }
            else
            {
                Log.Debug("OnEvent_Get_Queue_Statis_Info has error!reason=" + reason);
            }
        }

        private void OnEvent_Get_All_Queue_Statis_Info(string agentID, string current_time, List<Queue_Statis_Struct> queue_statis_list, string reason, int retCode)
        {
            
            if (retCode == 0)
            {
                if (this.GetAllQueueStatisEvent != null)
                {
                    this.GetAllQueueStatisEvent(current_time, queue_statis_list);
                }
                if (this.JSGetAllQueueStatisEvent != null)
                {
                    string[] strArgsLst = new string[]
                    {
                        "current_time",
                        "queue_statis_list"
                    };
                    this.ProcessAgentBarJsonDelegate(new object[]
                    {
                        this.JSGetAllQueueStatisEvent,
                        strArgsLst,
                        current_time,
                        queue_statis_list
                    });
                }
            }
            else
            {
                Log.Debug("OnEvent_Get_All_Queue_Statis_Info has error!reason=" + reason);
            }
        }

        private void OnEvent_Add_Customer_To_Queue(string agentID, string callType, string callcenter_num, string customer_num, string customer_status, string customer_type, string customer_uuid, string enter_queue_time, string exclusive_agent_num, string exclusive_queue_num, string queue_num, string queue_name, string queue_customer_amount, string early_queue_enter_time, string early_queue_enter_time_all, string customer_enter_channel)
        {
            if (this.AddCustomerToQueueEvent != null)
            {
                this.AddCustomerToQueueEvent(callType, callcenter_num, customer_num, customer_status, customer_type, customer_uuid, enter_queue_time, exclusive_agent_num, exclusive_queue_num, queue_num, enter_queue_time, queue_name, queue_customer_amount, early_queue_enter_time, early_queue_enter_time_all, customer_enter_channel);
            }
        }

        private void OnEvent_Update_Customer_Of_Queue(string agentID, string callType, string callcenter_num, string customer_num, string customer_status, string customer_type, string customer_uuid, string enter_queue_time, string exclusive_agent_num, string exclusive_queue_num, string queue_num)
        {
            if (this.UpdateCustomerOfQueueEvent != null)
            {
                this.UpdateCustomerOfQueueEvent(callType, callcenter_num, customer_num, customer_status, customer_type, customer_uuid, enter_queue_time, exclusive_agent_num, exclusive_queue_num, queue_num);
            }
        }

        private void OnEvent_Del_Customer_From_Queue(string agentID, string customer_uuid, string queue_num, string current_time, string early_queue_enter_time, string early_queue_enter_time_all, string reason)
        {
            if (this.DelCustomerFromQueueEvent != null)
            {
                this.DelCustomerFromQueueEvent(customer_uuid, queue_num, current_time, early_queue_enter_time, early_queue_enter_time_all, reason);
            }
        }

        private void OnEvent_Threewayee_Hangup(string agentID, string hangupReason)
        {
            string HangupReason = this.ThreeWayHangupReason2Chinese(hangupReason);
            
            if (this.ThreeWayeeHangupEvent != null)
            {
                this.ThreeWayeeHangupEvent(agentID, hangupReason);
            }
            if ("ALL_GATEWAYS_FULL" == hangupReason && this.AllGetwaysFullEvent != null)
            {
                this.AllGetwaysFullEvent();
            }
        }

        private void OnEvent_Consultee_Hangup(string agentID, string hangupReason)
        {
            
            string HangupReason = this.ConsulteeHangupReason2Chinese(hangupReason);
            
            if (this.ConsulteeHangupEvent != null)
            {
                this.ConsulteeHangupEvent(agentID, hangupReason);
            }
            if ("ALL_GATEWAYS_FULL" == hangupReason && this.AllGetwaysFullEvent != null)
            {
                this.AllGetwaysFullEvent();
            }
        }

        private void OnEvent_Warn_Agent_Resignin(string agentID)
        {
            if (this.WarnAgentResigninEvent != null)
            {
                this.WarnAgentResigninEvent();
            }
        }

        private void OnEvent_Warn_Agent_Force_Change_Status(string agentID, string executorAgentID)
        {
           
            if (this.WarnAgentForceChangeStatusEvent != null)
            {
                this.WarnAgentForceChangeStatusEvent(executorAgentID);
            }
        }

        private void OnEvent_Blind_Transfer_Outbound_Failed(string agentID, string hangupReason)
        {
            
            string failedReason = "转接失败";
            if ("ALL_GATEWAYS_FULL" == hangupReason)
            {
                failedReason += "：外线满线";
                if (this.AllGetwaysFullEvent != null)
                {
                    this.AllGetwaysFullEvent();
                }
            }
        }

        private void OnEvent_Force_Hangup_Ring(string agentID, string agent_call_uuid, string callerID, string calledID, string accessNumName, string desAgentID, string callType, string areaID, string areaName, string custGrade, string outExtraParamsFromIvr, string todayDate, string customer_num_format_local, string customer_num_format_national, string customer_num_format_e164, string customer_num_phone_type, string customerForeignId, string relation_uuid)
        {
            
            this.mCallType = AgentBar.Call_Type.COMMON_CALL_IN;
            this.mCallStatus = AgentBar.CallStatus.CALL_IN;
            this.m_agent_current_call_uuid = agent_call_uuid;
            this.update_Toolbar_UI(AgentBar.Event_Type.FORCE_HANGUP_RING_MYSELF, "");
            string makeStr = string.Empty;
            makeStr = string.Concat(new string[]
            {
                "#customer_num_format_local=",
                customer_num_format_local,
                "#customer_num_format_national=",
                customer_num_format_national,
                "#customer_num_format_e164=",
                customer_num_format_e164,
                "#customer_num_phone_type=",
                customer_num_phone_type,
                "#customerForeignId=",
                customerForeignId,
                "#agent_current_call_uuid=",
                agent_call_uuid,
                "#relation_uuid=",
                relation_uuid,
                "#"
            });
            if (this.ForceHangupCallRingEvent != null)
            {
                this.ForceHangupCallRingEvent(agentID, callerID, calledID, accessNumName, desAgentID, callType, areaID, areaName, custGrade, ComFunc.PLDESDecrypt(outExtraParamsFromIvr, this.SaltKey, todayDate), makeStr);
            }
        }

        private void OnEvent_ForceHangup(string agentID, string reason, int retCode)
        {
            AgentBar.Log.Debug(string.Concat(new object[]
            {
                "enter OnEvent_ForceHangup .agentID:",
                agentID,
                ",reason:",
                reason,
                ",retCode",
                retCode
            }));
            if (retCode == 0)
            {
                this.update_Toolbar_UI(AgentBar.Event_Type.FORCE_HANGUP_SUCCESS, "");
            }
            else
            {
                this.update_Toolbar_UI(AgentBar.Event_Type.FORCE_HANGUP_FAIL, "");
            }
            if (this.ForceHangupEvent != null)
            {
                this.ForceHangupEvent(agentID, reason, retCode);
            }
        }

        private void OnEvent_Internal_Caller_Ring(string agentID, string agent_call_uuid, string callerID, string callType, string relation_uuid)
        {
            AgentBar.Log.Debug(string.Concat(new string[]
            {
                "enter OnEvent_Internal_Caller_Ring .agentID:",
                agentID,
                ",agent_call_uuid:",
                agent_call_uuid,
                ",callerID:",
                callerID,
                ",callType",
                callType,
                ",relation_uuid",
                relation_uuid
            }));
            this.mCallType = AgentBar.Call_Type.AGENT_INTERNAL_CALL;
            this.mCallStatus = AgentBar.CallStatus.CALL_OUT;
            this.m_agent_current_call_uuid = agent_call_uuid;
            this.update_Toolbar_UI(AgentBar.Event_Type.CALLIN_INTERNAL, "内部呼叫");
            string makeStr = string.Empty;
            makeStr = string.Concat(new string[]
            {
                "#agent_current_call_uuid=",
                agent_call_uuid,
                "#relation_uuid=",
                relation_uuid,
                "#"
            });
            if (this.InternalCallerRingEvent != null)
            {
                this.InternalCallerRingEvent(agentID, callerID, callType, makeStr);
            }
            if ((this.SoftPhoneEnable2 || this.bindSoftPhoneLogin) && this.mSoftphoneAutoAnswer)
            {
                if (!this.DoAnswer())
                {
                    MessageBox.Show("接听失败！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                }
            }
        }

        private void OnEvent_Hangup(string hangupActiveFlag, string no_answered_alram_flag, string hangupReason, int retCode, string isEvaluated, string evaluateStatus, string evaluateDefaultResult, string agent_call_uuid, string relation_uuid)
        {
            AgentBar.Log.Debug("enter OnEvent_Hangup .hangupReason:" + hangupReason);
            this.mEavesdropAgent = "";
            this.m_agent_current_call_uuid = string.Empty;
            this.m_customer_current_call_uuid = string.Empty;
            if (retCode == 0)
            {
                if (this.bindSoftPhoneLogin)
                {
                    if (hangupActiveFlag == "false" && this.CallSuccess)
                    {
                        if (File.Exists(this.hangupRingFileName))
                        {
                            this.sndPlayer.SoundLocation = this.hangupRingFileName;
                            this.sndPlayer.Load();
                            this.sndPlayer.Play();
                        }
                        else
                        {
                            AgentBar.Log.Error("来电铃声文件 dududu.wav 不存在！");
                        }
                    }
                }
                this.CallSuccess = false;
                //this.update_Toolbar_UI(AgentBar.Event_Type.HANGUP_CALL_SUCCESS, this.HangupReason2Chinese(hangupReason));
            }
            else
            {
                this.update_Toolbar_UI(AgentBar.Event_Type.HANGUP_CALL_FAIL, "挂断失败");
            }
            //if ("0" == no_answered_alram_flag)
            //{
            //    if (this.tsbNoAnswerCalls.Image != Resources.NoAnswerCall_alarm)
            //    {
            //        this.tsbNoAnswerCalls.Image = Resources.NoAnswerCall_alarm;
            //        this.NoAnswerCallNormal = false;
            //        if (this.other != null && this.other.DropDownItems.Contains(this.tsbNoAnswerCalls) && this.other.Image != Resources.other1)
            //        {
            //            this.other.Image = Resources.other1;
            //            this.otherNormal = false;
            //        }
            //    }
            //    if (this.NoAnswerCallAlarmlEvent != null)
            //    {
            //        this.NoAnswerCallAlarmlEvent();
            //    }
            //}
            this.mCallStatus = AgentBar.CallStatus.NO_CALL;
            string makeStr = string.Empty;
            makeStr = string.Concat(new string[]
            {
                "#isEvaluated=",
                isEvaluated,
                "#evaluateStatus=",
                evaluateStatus,
                "#evaluateDefaultResult=",
                evaluateDefaultResult,
                "#agent_call_uuid=",
                agent_call_uuid,
                "#relation_uuid=",
                relation_uuid,
                "#"
            });
            if (this.HangupEvent != null)
            {
                this.HangupEvent(hangupReason, retCode, makeStr);
            }
            this.trmCall.Stop();
            this.timeCount = 1;
            this.tsbState.Size = new Size(130, 36);
            this.needCleanUpTime = false;
        }

        private void OnEvent_HangupResult(string agentID, string reason, int retCode)
        {
            if (retCode != 0)
            {
                string Reason = this.HangupFailReason2Chinese(reason);
            }
        }

        private void OnHeartBeat()
        {
            this.HeartBeatTimes = 0;
        }

        ~PLAgent()
        {
            this.agentDll.Dispose();
        }

        private void OnEvent_Answer(string agent_call_uuid, string relation_uuid)
        {
            string makeStr = string.Empty;
            makeStr = string.Concat(new string[]
            {
                "#agent_current_call_uuid=",
                agent_call_uuid,
                "#relation_uuid=",
                relation_uuid,
                "#"
            });
            if (this.AnswerEvent != null)
            {
                this.AnswerEvent(makeStr);
            }
            this.CallSuccess = true;
        }

        private void OnEvent_Callee_Answer(string agent_call_uuid, string relaiton_uuid)
        {
            string makeStr = string.Empty;
            makeStr = string.Concat(new string[]
            {
                "#agent_current_call_uuid=",
                agent_call_uuid,
                "#relation_uuid=",
                relaiton_uuid,
                "#"
            });
            if (this.CalleeAnswerEvent != null)
            {
                this.CalleeAnswerEvent(makeStr);
            }
        }

        private void OnEvent_Bridge(string bridge_uuidA, string bridge_uuidB)
        {
            if (bridge_uuidA == this.m_agent_current_call_uuid)
            {
                this.m_customer_current_call_uuid = bridge_uuidB;
            }
            else
            {
                this.m_customer_current_call_uuid = bridge_uuidA;
            }
            if (this.BridgeEvent != null)
            {
                this.BridgeEvent();
            }
        }

        private void OnEvent_Get_Agent_Personal_info(string AgentID, string agent_mobile, string agent_email)
        {
            if (this.GetAgentPersonalInfoEvent != null)
            {
                this.GetAgentPersonalInfoEvent(AgentID, agent_mobile, agent_email);
            }
        }

        private void OnEvent_Set_Agent_Personal_info(string AgentID, int retCode, string reason)
        {
            if (this.SetAgentPersonalInfoEvent != null)
            {
                this.SetAgentPersonalInfoEvent(AgentID, retCode, reason);
            }
        }

        private void OnEvent_Change_Pswd(string AgentID, int retCode, string reason)
        {
            if (this.ChangePswdEvent != null)
            {
                this.ChangePswdEvent(AgentID, retCode, reason);
            }
        }

        private void OnEvent_Get_Report_Statis_Info(string AgentID, int retCode, string reason, Dictionary<string, string> reportStatisInfo)
        {
            AgentBar.Log.Debug("enter OnEvent_Get_Report_Statis_Info .");
            if (this.GetReportStatisInfoEvent != null)
            {
                this.GetReportStatisInfoEvent(AgentID, retCode, reason, reportStatisInfo);
            }
            if (this.JSGetReportStatisInfoEvent != null)
            {
                string[] strArgsLst = new string[]
                {
                    "AgentID",
                    "retCode",
                    "reason",
                    "reportStatisInfo"
                };
                this.ProcessAgentBarJsonDelegate(new object[]
                {
                    this.JSGetReportStatisInfoEvent,
                    strArgsLst,
                    AgentID,
                    retCode,
                    reason,
                    reportStatisInfo
                });
            }
        }

        private void OnEvent_Apply_Change_Status(string AgentID, int retCode, string reason)
        {
            AgentBar.Log.Debug("enter OnEvent_Apply_Change_Status .");
            if (retCode == 0)
            {
                this.update_Toolbar_UI(AgentBar.Event_Type.APPLY_CHANGE_STATUS_APPLY_SUCCESS, "离开申请中");
            }
            else
            {
                this.update_Toolbar_UI(AgentBar.Event_Type.APPLY_CHANGE_STATUS_APPLY_FAILED, "申请离开失败");
            }
            if (this.ApplyChangeStatusEvent != null)
            {
                this.ApplyChangeStatusEvent(AgentID, retCode, reason);
            }
        }

        private void OnEvent_Apply_Change_Status_Distribute(string AgentID, string apply_agentid, string targetStatus, string apply_agentName, string apply_agent_groupID, string apply_agent_groupName, string apply_time, string applyType, int retCode, string reason)
        {
            bool isFound = false;
            if (retCode == 0)
            {
                Apply_Change_Status tmp_apply_change_status = default(Apply_Change_Status);
                tmp_apply_change_status.applyAgentID = apply_agentid;
                tmp_apply_change_status.agentName = apply_agentName;
                tmp_apply_change_status.applyAgentGroupID = apply_agent_groupID;
                tmp_apply_change_status.applyAgentGroupName = apply_agent_groupName;
                tmp_apply_change_status.applyTime = ComFunc.TotalSecondToDateTime(apply_time);
                tmp_apply_change_status.applyState = Apply_State.Apply_State_Applying;
                tmp_apply_change_status.targetStatus = targetStatus;
                tmp_apply_change_status.isFinished = false;
                tmp_apply_change_status.applyType = AgentBar.str2ApplyType(applyType);
                if (AgentID == apply_agentid)
                {
                    this.my_apply_change_status = tmp_apply_change_status;
                    this.update_Toolbar_UI(AgentBar.Event_Type.APPLY_CHANGE_STATUS_APPLY_SUCCESS, "离开申请中");
                }
                if (this.mMyRoleAndRight.rights_of_view_agent_group_info && this.mMyRoleAndRight.controled_agent_group_lst != null)
                {
                    if (this.mMyRoleAndRight.controled_agent_group_lst.IndexOf(apply_agent_groupID) >= 0)
                    {
                        foreach (Apply_Change_Status applyItem in this.apply_change_status_to_approval_lst)
                        {
                            if (applyItem.applyAgentID == apply_agentid)
                            {
                                isFound = true;
                                break;
                            }
                        }
                        if (!isFound)
                        {
                            this.apply_change_status_to_approval_lst.Add(tmp_apply_change_status);
                            if (this.tsbApprove.Image != Resources.approval)
                            {
                                this.tsbApprove.Image = Resources.approval;
                                this.ApproveNormal = false;
                                if (this.other != null && this.other.DropDownItems.Contains(this.tsbApprove) && this.other.Image != Resources.other1)
                                {
                                    this.other.Image = Resources.other1;
                                    this.otherNormal = false;
                                }
                            }
                        }
                    }
                }
            }
            if (this.ApplyChangeStatusDistributeEvent != null)
            {
                this.ApplyChangeStatusDistributeEvent(AgentID, apply_agentid, targetStatus, apply_agentName, apply_agent_groupID, apply_agent_groupName, apply_time, applyType, retCode, reason);
            }
        }

        private void OnEvent_Approve_Change_Status_Distribute(string AgentID, string apply_agentid, string apply_agent_groupID, string targetStatus, string approveResult, string approve_time, int retCode, string reason)
        {
            if (retCode == 0)
            {
                if (AgentID == apply_agentid)
                {
                    if (this.my_apply_change_status.applyState == Apply_State.Apply_State_Applying && this.my_apply_change_status.targetStatus == targetStatus)
                    {
                        this.my_apply_change_status.approveTime = ComFunc.TotalSecondToDateTime(approve_time);
                        if (approveResult == "1")
                        {
                            this.my_apply_change_status.applyState = Apply_State.Apply_State_Exeute;
                            this.update_Toolbar_UI(AgentBar.Event_Type.APPLY_CHANGE_STATUS_APPLY_SUCCESS, "申请离开审批通过");
                        }
                        else
                        {
                            this.update_Toolbar_UI(AgentBar.Event_Type.APPLY_CHANGE_STATUS_APPLY_FAILED, "离开审批未通过");
                            this.initMyApplyChangeStatus();
                        }
                    }
                }
                if (this.mMyRoleAndRight.rights_of_view_agent_group_info && this.mMyRoleAndRight.controled_agent_group_lst != null)
                {
                    if (this.mMyRoleAndRight.controled_agent_group_lst.IndexOf(apply_agent_groupID) >= 0)
                    {
                        int FindIndex = -1;
                        Apply_Change_Status new_item = default(Apply_Change_Status);
                        for (int i = 0; i < this.apply_change_status_to_approval_lst.Count; i++)
                        {
                            new_item = this.apply_change_status_to_approval_lst[i];
                            if (new_item.applyAgentID == apply_agentid && new_item.targetStatus == targetStatus)
                            {
                                new_item.approveTime = ComFunc.TotalSecondToDateTime(approve_time);
                                if (approveResult == "1")
                                {
                                    new_item.applyState = Apply_State.Apply_State_Exeute;
                                }
                                else
                                {
                                    new_item.applyState = Apply_State.Apply_State_Approval_NoPass;
                                    new_item.reason = reason;
                                }
                                FindIndex = i;
                                break;
                            }
                        }
                        if (FindIndex != -1)
                        {
                            this.apply_change_status_to_approval_lst.RemoveAt(FindIndex);
                            this.apply_change_status_approval_history.Insert(0, new_item);
                        }
                        this.ChkHaveAnyToApprove();
                    }
                }
            }
            if (this.ApproveChangeStatusDistributeEvent != null)
            {
                this.ApproveChangeStatusDistributeEvent(AgentID, apply_agentid, apply_agent_groupID, targetStatus, approveResult, approve_time, retCode, reason);
            }
        }

        private void ChkHaveAnyToApprove()
        {
            int i;
            for (i = 0; i < this.apply_change_status_to_approval_lst.Count; i++)
            {
                if (this.apply_change_status_to_approval_lst[i].applyState == Apply_State.Apply_State_Applying)
                {
                    break;
                }
            }
            if (i >= this.apply_change_status_to_approval_lst.Count)
            {
                this.update_Toolbar_UI(AgentBar.Event_Type.APPLY_CHANGE_STATUS_NO_ANY_APPROVAL, "");
            }
            else
            {
                this.update_Toolbar_UI(AgentBar.Event_Type.APPLY_CHANGE_STATUS_SOME_APPLY, "");
            }
        }

        private void OnEvent_Apply_Change_Status_Cancel_Distribute(string AgentID, string apply_agentid, string targetStatus, int retCode, string reason)
        {
            if (retCode == 0)
            {
                int FindIndex = -1;
                Apply_Change_Status new_item = default(Apply_Change_Status);
                for (int i = 0; i < this.apply_change_status_to_approval_lst.Count; i++)
                {
                    new_item = this.apply_change_status_to_approval_lst[i];
                    if (new_item.applyAgentID == apply_agentid && new_item.targetStatus == targetStatus)
                    {
                        new_item.applyState = Apply_State.Apply_State_Cancel;
                        new_item.reason = "申请被取消";
                        FindIndex = i;
                        break;
                    }
                }
                if (FindIndex != -1)
                {
                    this.apply_change_status_to_approval_lst.RemoveAt(FindIndex);
                    this.apply_change_status_approval_history.Insert(0, new_item);
                }
                FindIndex = -1;
                for (int i = 0; i < this.apply_change_status_approval_history.Count; i++)
                {
                    new_item = this.apply_change_status_approval_history[i];
                    if (new_item.applyAgentID == apply_agentid && new_item.targetStatus == targetStatus && new_item.applyState != Apply_State.Apply_State_Execute_Success && new_item.applyState != Apply_State.Apply_State_Approval_NoPass && new_item.applyState != Apply_State.Apply_State_Execute_Failed && new_item.applyState != Apply_State.Apply_State_Cancel)
                    {
                        new_item.applyState = Apply_State.Apply_State_Cancel;
                        new_item.reason = "申请被取消";
                        FindIndex = i;
                        break;
                    }
                }
                if (FindIndex != -1)
                {
                    this.apply_change_status_approval_history.RemoveAt(FindIndex);
                    this.apply_change_status_approval_history.Insert(0, new_item);
                }
                this.ChkHaveAnyToApprove();
                if (this.ApplyChangeStatusCancelEvent != null)
                {
                    this.ApplyChangeStatusCancelEvent(AgentID, apply_agentid, targetStatus, retCode, reason);
                }
            }
        }

        private void OnEvent_Get_Change_Status_Apply_List(string agentID, List<Apply_Change_Status> apply_change_status_list, int retCode, string reason)
        {
            if (this.apply_change_status_to_approval_lst == null)
            {
                this.apply_change_status_to_approval_lst = new List<Apply_Change_Status>();
            }
            this.apply_change_status_to_approval_lst.Clear();
            for (int i = 0; i < apply_change_status_list.Count; i++)
            {
                Apply_Change_Status new_apply_agent = default(Apply_Change_Status);
                new_apply_agent = apply_change_status_list[i];
                new_apply_agent.applyState = AgentBar.IntStr2ApplyState(new_apply_agent.applyStateStr);
                new_apply_agent.applyTime = ComFunc.TotalSecondToDateTime(new_apply_agent.applyTime);
                new_apply_agent.applyType = AgentBar.str2ApplyType(new_apply_agent.applyType);
                this.apply_change_status_to_approval_lst.Add(new_apply_agent);
            }
            if (apply_change_status_list.Count == 0)
            {
                this.update_Toolbar_UI(AgentBar.Event_Type.APPLY_CHANGE_STATUS_NO_ANY_APPROVAL, "");
            }
            else
            {
                this.update_Toolbar_UI(AgentBar.Event_Type.APPLY_CHANGE_STATUS_SOME_APPLY, "");
            }
            if (this.GetChangeStatusApplyListEvent != null)
            {
                this.GetChangeStatusApplyListEvent(this.AgentID, apply_change_status_list, retCode, reason);
            }
            if (this.JSGetChangeStatusApplyListEvent != null)
            {
                string[] strArgsLst = new string[]
                {
                    "AgentID",
                    "apply_change_status_list",
                    "retCode",
                    "reason"
                };
                this.ProcessAgentBarJsonDelegate(new object[]
                {
                    this.JSGetChangeStatusApplyListEvent,
                    strArgsLst,
                    this.AgentID,
                    apply_change_status_list,
                    retCode,
                    reason
                });
            }
        }

        private void OnEvent_Queue_Transfer_Outbound(string agentID, string customer_num, string queue_num, string transfee_num, string access_num, string tranfer_time)
        {
            if (this.QueueTransferBoundEvent != null)
            {
                this.QueueTransferBoundEvent(agentID, customer_num, queue_num, transfee_num, access_num, tranfer_time);
            }
        }

        private void OnEvent_RecordStart(string AgentExten, string AgentID, string Agent_call_uuid, string CalleeNum, string CallerNum, string FilePath, string relation_uuid)
        {
           
            string makeStr = string.Empty;
            makeStr = "#relation_uuid=" + relation_uuid + "#";
            if (this.RecordStart != null)
            {
                this.RecordStart(AgentExten, AgentID, Agent_call_uuid, CalleeNum, CallerNum, FilePath, makeStr);
            }
        }

        private void OnEvent_RecordStop(string AgentID, string Agent_call_uuid, string FilePath, string relation_uuid)
        {
            
            string makeStr = string.Empty;
            makeStr = "#relation_uuid=" + relation_uuid + "#";
            if (this.RecordStop != null)
            {
                this.RecordStop(AgentID, Agent_call_uuid, FilePath, makeStr);
            }
        }

        private void OnEvent_Evaluate_Result(string agentID, string agent_call_uuid, string agentExten, string customerUuid, string agent_group_name, string callerNum, string evaluateScore, string evaluateStatus, string queue_num)
        {
            string makeStr = string.Empty;
            makeStr = string.Concat(new string[]
            {
                "#agent_group_name=",
                agent_group_name,
                "#callerNum=",
                callerNum,
                "#queue_num=",
                queue_num,
                "#"
            });
            if (this.Cust_Evaluate_Result != null)
            {
                this.Cust_Evaluate_Result(this.AgentID, agent_call_uuid, agentExten, customerUuid, evaluateScore, evaluateStatus, makeStr);
            }
        }

        private void OnEvent_Get_Agentgroup_Status_Max_Num(string AgentID, string agentGroupNameLstStr, Dictionary<string, string> dicAgentGroupStatusMaxNum)
        {
            if (this.GetAgentGroupStatusMaxNumEvent != null)
            {
                this.GetAgentGroupStatusMaxNumEvent(AgentID, agentGroupNameLstStr, dicAgentGroupStatusMaxNum);
            }
            if (this.JSGetAgentGroupStatusMaxNumEvent != null)
            {
                string[] strArgsLst = new string[]
                {
                    "AgentID",
                    "agentGroupNameLstStr",
                    "dicAgentGroupStatusMaxNum"
                };
                this.ProcessAgentBarJsonDelegate(new object[]
                {
                    this.JSGetAgentGroupStatusMaxNumEvent,
                    strArgsLst,
                    AgentID,
                    agentGroupNameLstStr,
                    dicAgentGroupStatusMaxNum
                });
            }
        }

        private void OnEvent_Apply_Change_Status_Cancel(string agentID, string targetStatus, int retCode, string reason)
        {
            if (retCode == 0)
            {
                this.update_Toolbar_UI(AgentBar.Event_Type.APPLY_CHANGE_STATUS_CANCEL_SUCCESS, "取消申请成功");
                this.initMyApplyChangeStatus();
            }
            else
            {
                this.update_Toolbar_UI(AgentBar.Event_Type.APPLY_CHANGE_STATUS_CANCEL_FAILED, "取消申请失败");
            }
        }

        private void OnEvent_Apply_or_Approve_Change_Status_Timeout_Distribute(string agentID, string apply_agentid, string apply_agent_groupID, string targetStatus, string timeoutType)
        {
            AgentBar.Log.Debug("enter OnEvent_Apply_or_Approve_Change_Status_Timeout_Distribute .");
            if (this.AgentID == apply_agentid)
            {
                if (timeoutType == "1")
                {
                    this.update_Toolbar_UI(AgentBar.Event_Type.APPLY_CHANGE_STATUS_APPLY_FAILED, "申请离开超时");
                }
                else
                {
                    if (!(timeoutType == "2"))
                    {
                        AgentBar.Log.Error("超时类型错误！timeoutType=" + timeoutType);
                        return;
                    }
                    this.update_Toolbar_UI(AgentBar.Event_Type.APPLY_CHANGE_STATUS_APPLY_FAILED, "审批超时");
                }
                this.update_Toolbar_UI(AgentBar.Event_Type.APPLY_CHANGE_STATUS_APPLY_OR_APPROVE_TIMEOUT, "申请或审批超时");
                this.initMyApplyChangeStatus();
            }
            if (this.mMyRoleAndRight.rights_of_view_agent_group_info && this.mMyRoleAndRight.controled_agent_group_lst != null)
            {
                if (this.mMyRoleAndRight.controled_agent_group_lst.IndexOf(apply_agent_groupID) >= 0)
                {
                    if (timeoutType == "1")
                    {
                        int FindIndex = -1;
                        Apply_Change_Status new_item = default(Apply_Change_Status);
                        for (int i = 0; i < this.apply_change_status_to_approval_lst.Count; i++)
                        {
                            new_item = this.apply_change_status_to_approval_lst[i];
                            if (new_item.applyAgentID == apply_agentid && new_item.targetStatus == targetStatus && new_item.applyState == Apply_State.Apply_State_Applying)
                            {
                                new_item.applyState = Apply_State.Apply_State_Approval_NoPass;
                                new_item.reason = "申请超时";
                                FindIndex = i;
                                break;
                            }
                        }
                        if (FindIndex != -1)
                        {
                            this.apply_change_status_to_approval_lst.RemoveAt(FindIndex);
                            this.apply_change_status_approval_history.Insert(0, new_item);
                        }
                        this.ChkHaveAnyToApprove();
                    }
                    if (timeoutType == "2")
                    {
                        int FindIndex = -1;
                        Apply_Change_Status new_item = default(Apply_Change_Status);
                        for (int i = 0; i < this.apply_change_status_approval_history.Count; i++)
                        {
                            new_item = this.apply_change_status_approval_history[i];
                            if (new_item.applyAgentID == apply_agentid && new_item.targetStatus == targetStatus && new_item.applyState != Apply_State.Apply_State_Execute_Success && new_item.applyState != Apply_State.Apply_State_Approval_NoPass && new_item.applyState != Apply_State.Apply_State_Execute_Failed && new_item.applyState != Apply_State.Apply_State_Cancel)
                            {
                                new_item.applyState = Apply_State.Apply_State_Execute_Failed;
                                new_item.reason = "审批超时";
                                FindIndex = i;
                                break;
                            }
                        }
                        if (FindIndex != -1)
                        {
                            this.apply_change_status_approval_history.RemoveAt(FindIndex);
                            this.apply_change_status_approval_history.Insert(0, new_item);
                        }
                    }
                }
            }
            if (this.ApproveChangeStatusTimeoutDistributeEvent != null)
            {
                this.ApproveChangeStatusTimeoutDistributeEvent(this.AgentID, apply_agentid, apply_agent_groupID, targetStatus, timeoutType);
            }
        }

        private void OnEvent_CheckExten(string agentID, string agent_call_uuid, string relation_uuid)
        {
            
            this.mCallType = AgentBar.Call_Type.ECHO_TEST;
            this.mCallStatus = AgentBar.CallStatus.CALL_OUT;
            this.m_agent_current_call_uuid = agent_call_uuid;
            string makeStr = string.Empty;
            makeStr = string.Concat(new string[]
            {
                "#agent_current_call_uuid=",
                agent_call_uuid,
                "#relation_uuid=",
                relation_uuid,
                "#"
            });

            if (this.CheckExtenEvent != null)
            {
                this.CheckExtenEvent(agentID, makeStr);
            }
        }

        private void OnResponse(string EventType, string AgentID, int retCode, string strReason)
        {
           
            if (0 != retCode)
            {
                if (retCode == -69)
                {
                    this.chkIfHaveApplyChangeStatus();
                }
                AgentBar.Log.Debug("response error:" + strReason);
                if (this.ServerResponse != null)
                {
                    this.ServerResponse(AgentID, retCode, strReason);
                }
            }
            string text = EventType.ToLower();
            switch (text)
            {
                case "signin":
                    if (0 != retCode)
                    {
                        this.update_Toolbar_UI(AgentBar.Event_Type.SIGNIN_FAIL, "");
                    }
                    if (this.SignInResponse != null)
                    {
                        this.SignInResponse(AgentID, retCode, strReason);
                    }
                    break;
                case "signout":
                    if (0 != retCode)
                    {
                        this.update_Toolbar_UI(AgentBar.Event_Type.SIGNOUT_FAIL, "");
                    }
                    if (this.SignOutResponse != null)
                    {
                        this.SignOutResponse(AgentID, retCode, strReason);
                    }
                    break;
                case "calloutring":
                    if (0 != retCode)
                    {
                        this.update_Toolbar_UI(AgentBar.Event_Type.CALLOUT_RING_MYSELF, "");
                    }
                    break;
                case "hangup":
                    if (0 != retCode)
                    {
                        this.update_Toolbar_UI(AgentBar.Event_Type.HANGUP_CALL_FAIL, "挂断失败");
                    }
                    break;
                case "check":
                    if (0 != retCode)
                    {
                        this.update_Toolbar_UI(AgentBar.Event_Type.ECHO_TEST_FAIL, "");
                    }
                    break;
                case "heartbeat":
                    this.OnHeartBeat();
                    break;
                case "hold":
                    if (0 != retCode)
                    {
                        this.update_Toolbar_UI(AgentBar.Event_Type.HOLD_FAIL, "");
                    }
                    break;
                case "stophold":
                    if (0 != retCode)
                    {
                        this.update_Toolbar_UI(AgentBar.Event_Type.UNHOLD_FAIL, "");
                    }
                    break;
                case "consult":
                    if (0 != retCode)
                    {
                        this.update_Toolbar_UI(AgentBar.Event_Type.CONSULT_FAIL, "");
                    }
                    break;
                case "consult_cancel":
                    if (0 != retCode)
                    {
                        this.update_Toolbar_UI(AgentBar.Event_Type.CONSULT_CANCEL_FAIL, "");
                    }
                    break;
                case "consult_transfer":
                    if (0 != retCode)
                    {
                        this.update_Toolbar_UI(AgentBar.Event_Type.CONSULT_TRANSFER_FAIL, "");
                    }
                    break;
                case "eavesdrop":
                    if (0 != retCode)
                    {
                        this.update_Toolbar_UI(AgentBar.Event_Type.EAVESDROP_FAIL, "");
                    }
                    break;
                case "whisper":
                    if (0 != retCode)
                    {
                        this.update_Toolbar_UI(AgentBar.Event_Type.WHISPER_FAIL, "");
                    }
                    break;
                case "bargein":
                    if (0 != retCode)
                    {
                        this.update_Toolbar_UI(AgentBar.Event_Type.BARGE_IN_FAIL, "");
                    }
                    break;
                case "force_hangup":
                    if (0 != retCode)
                    {
                        this.update_Toolbar_UI(AgentBar.Event_Type.FORCE_HANGUP_FAIL, "");
                    }
                    break;
                case "transfer_agent":
                    if (0 != retCode)
                    {
                        this.update_Toolbar_UI(AgentBar.Event_Type.TRANSFER_AGENT_FAIL, "");
                    }
                    break;
                case "transfer_ivr":
                    if (0 != retCode)
                    {
                        this.update_Toolbar_UI(AgentBar.Event_Type.TRANSFER_IVR_FAIL, "");
                    }
                    break;
                case "transfer_queue":
                    if (0 != retCode)
                    {
                        this.update_Toolbar_UI(AgentBar.Event_Type.TRANSFER_QUEUE_FAIL, "");
                    }
                    break;
                case "transfer_ivr_profile":
                    if (0 != retCode)
                    {
                        this.update_Toolbar_UI(AgentBar.Event_Type.TRANSFER_IVR_PROFILE_FAIL, "");
                    }
                    break;
                case "interrupt":
                    if (0 != retCode)
                    {
                        this.update_Toolbar_UI(AgentBar.Event_Type.BARGE_IN_FAIL, "");
                    }
                    break;
                case "forcedisconnect":
                    if (0 != retCode)
                    {
                        this.update_Toolbar_UI(AgentBar.Event_Type.FORCE_HANGUP_FAIL, "");
                    }
                    break;
                case "internal_call":
                    if (0 != retCode)
                    {
                        this.update_Toolbar_UI(AgentBar.Event_Type.INTERNAL_CALL_AGENT_FAIL, "");
                    }
                    break;
                case "evaluate":
                    if (0 != retCode)
                    {
                        this.update_Toolbar_UI(AgentBar.Event_Type.GRADE_FAIL, "");
                    }
                    break;
                case "get_access_numbers":
                    if (0 != retCode)
                    {
                        this.update_Toolbar_UI(AgentBar.Event_Type.GET_ACCESS_NUMBERS_FAIL, "");
                    }
                    break;
                case "three_way_call":
                    if (0 != retCode)
                    {
                        this.update_Toolbar_UI(AgentBar.Event_Type.THREE_WAY_FAIL, "");
                    }
                    break;
                case "three_way_call_cancel":
                    if (0 != retCode)
                    {
                        this.update_Toolbar_UI(AgentBar.Event_Type.THREE_WAY_CANCEL_FAIL, "");
                    }
                    break;
                case "get_online_agent":
                    if (0 != retCode)
                    {
                        this.update_Toolbar_UI(AgentBar.Event_Type.GET_ONLINE_AGENT_FAIL, "");
                    }
                    break;
                case "get_ivr_list":
                    if (0 != retCode)
                    {
                        this.update_Toolbar_UI(AgentBar.Event_Type.GET_IVR_LIST_FAIL, "");
                    }
                    break;
                case "get_queue_list":
                    if (0 != retCode)
                    {
                        this.update_Toolbar_UI(AgentBar.Event_Type.GET_QUEUE_LIST_FAIL, "");
                    }
                    break;
                case "get_agent_group_list":
                    if (0 != retCode)
                    {
                        this.update_Toolbar_UI(AgentBar.Event_Type.GET_AGENT_GROUP_LIST_FAIL, "");
                    }
                    break;
                case "get_agents_of_queue":
                    if (0 != retCode)
                    {
                    }
                    break;
                case "get_agents_of_agent_group":
                    if (0 != retCode)
                    {
                    }
                    break;
                case "get_agents_monitor_info":
                    if (0 != retCode)
                    {
                    }
                    break;
                case "get_detail_call_info":
                    if (0 != retCode)
                    {
                    }
                    break;
                case "get_customer_of_queue":
                    if (0 != retCode)
                    {
                    }
                    break;
                case "get_customer_of_my_queue":
                    if (0 != retCode)
                    {
                    }
                    break;
                case "get_report_statis_info":
                    if (0 != retCode)
                    {
                        if (this.newFrmMonitorScreen != null)
                        {
                            this.newFrmMonitorScreen.Close();
                        }
                    }
                    break;
            }
        }


        private void OnEvent_GetDefineStatus(string AgentID, int retCode, string strReason, Dictionary<string, string> define_status_dic)
        {
            if (0 == retCode)
            {
                this.mAgentDefineStatus = new Dictionary<string, string>();
                this.mAgentDefineStatus.Clear();
                this.mAgentDefineStatus = define_status_dic;
            }
        }

       
        private void refreshApplyChangeStatusHistoryList(string AgentID, string status_change_agentid, string status_change_before, string status_change_after, bool is_bind_exten)
        {
            if (status_change_agentid == this.my_apply_change_status.applyAgentID && this.my_apply_change_status.targetStatus == status_change_after)
            {
                this.update_Toolbar_UI(AgentBar.Event_Type.APPLY_CHANGE_STATUS_FINISHED, "申请离开成功");
                this.initMyApplyChangeStatus();
            }
            if (this.mMyRoleAndRight.rights_of_view_agent_group_info)
            {
                int FindIndex = -1;
                Apply_Change_Status new_item = default(Apply_Change_Status);
                for (int i = 0; i < this.apply_change_status_to_approval_lst.Count; i++)
                {
                    new_item = this.apply_change_status_to_approval_lst[i];
                    if (new_item.applyAgentID == status_change_agentid)
                    {
                        FindIndex = i;
                        break;
                    }
                }
                if (FindIndex != -1)
                {
                    if (status_change_after == "-1")
                    {
                        this.apply_change_status_to_approval_lst.RemoveAt(FindIndex);
                        new_item.applyState = Apply_State.Apply_State_Approval_NoPass;
                        new_item.reason = "离线";
                        this.apply_change_status_approval_history.Insert(0, new_item);
                    }
                    else if (new_item.targetStatus == status_change_after)
                    {
                        new_item.applyState = Apply_State.Apply_State_Execute_Success;
                        new_item.reason = "";
                        this.apply_change_status_to_approval_lst.RemoveAt(FindIndex);
                        this.apply_change_status_approval_history.Insert(0, new_item);
                    }
                    this.ChkHaveAnyToApprove();
                }
                FindIndex = -1;
                new_item = default(Apply_Change_Status);
                for (int i = 0; i < this.apply_change_status_approval_history.Count; i++)
                {
                    new_item = this.apply_change_status_approval_history[i];
                    if (new_item.applyAgentID == status_change_agentid && new_item.applyState != Apply_State.Apply_State_Execute_Success && new_item.applyState != Apply_State.Apply_State_Approval_NoPass && new_item.applyState != Apply_State.Apply_State_Execute_Failed && new_item.applyState != Apply_State.Apply_State_Cancel)
                    {
                        FindIndex = i;
                        break;
                    }
                }
                if (FindIndex != -1)
                {
                    if (status_change_after == "-1")
                    {
                        new_item.applyState = Apply_State.Apply_State_Approval_NoPass;
                        new_item.reason = "离线";
                        this.apply_change_status_approval_history.RemoveAt(FindIndex);
                        this.apply_change_status_approval_history.Insert(0, new_item);
                    }
                    else if (new_item.targetStatus == status_change_after)
                    {
                        new_item.applyState = Apply_State.Apply_State_Execute_Success;
                        new_item.reason = "";
                        this.apply_change_status_approval_history.RemoveAt(FindIndex);
                        this.apply_change_status_approval_history.Insert(0, new_item);
                    }
                }
            }
        }

        private void OnEvent_AgentStatusChange(string AgentID, string status_change_agentid, string status_change_before, string status_change_after, bool is_bind_exten, string customer_enter_channel, string current_time, string start_talking_time)
        {
            
            this.refreshApplyChangeStatusHistoryList(AgentID, status_change_agentid, status_change_before, status_change_after, is_bind_exten);
            if (this.AgentStatusChangeEvent != null)
            {
                this.AgentStatusChangeEvent(status_change_agentid, status_change_before, status_change_after, is_bind_exten, customer_enter_channel, current_time, start_talking_time);
            }
            if (!(AgentID != status_change_agentid))
            {
                this.mAgentStatus = status_change_after;
                if (AgentBar.Str2AgentStatus(this.mAgentStatus) == AgentBar.Agent_Status.AGENT_STATUS_CAMP_ON)
                {
                    this.mAgentStateBeforeCallinOrCallout = AgentBar.Str2AgentStatus(status_change_before);
                }
                if (AgentBar.Str2AgentStatus(this.mAgentStatus) == AgentBar.Agent_Status.AGENT_STATUS_IDLE)
                {
                    if (this.mAgentState == AgentBar.Agent_State.AGENT_STATUS_CAMP_ON)
                    {
                        this.update_Toolbar_UI(AgentBar.Event_Type.AGENT_STATUS_CHANGE_TO_IDLE, "");
                    }
                }
                int i;
                for (i = 0; i < this.tsbState.DropDownItems.Count; i++)
                {
                    if (this.tsbState.DropDownItems[i].AccessibleName == status_change_after)
                    {
                        this.tsbState.Enabled = true;
                        this.tsbState.Image = this.tsbState.DropDownItems[i].Image;
                        string tempStatus = this.tsbState.DropDownItems[i].Text.Trim();
                        try
                        {
                            if (tempStatus.IndexOf(' ') >= 0)
                            {
                                tempStatus = tempStatus.Substring(0, this.tsbState.DropDownItems[i].Text.IndexOf(' '));
                            }
                            if (tempStatus.Length > 10)
                            {
                                this.tsbState.Text = tempStatus.Substring(0, 9) + "..";
                                this.tsbState.ToolTipText = tempStatus;
                            }
                            else
                            {
                                this.tsbState.Text = tempStatus;
                            }
                            this.tsbState.AccessibleName = this.tsbState.DropDownItems[i].AccessibleName;
                            this.update_Toolbar_UI(AgentBar.Event_Type.AGENT_STATUS_RESTORE, "");
                            break;
                        }
                        catch (Exception ex)
                        {
                            AgentBar.Log.Error(string.Concat(new string[]
                            {
                                ex.Source,
                                ",信息:",
                                ex.Message,
                                ",堆栈:",
                                ex.StackTrace
                            }));
                        }
                    }
                }
            }
        }

        private void OnEvent_ThrowException(string ex)
        {
            AgentBar.Log.Debug("enter OnEvent_ThrowException .ex:");
            if (this.transfercheck)
            {
                this.tsbTransfer.Enabled = true;
            }
            if (this.Consultcheck)
            {
                this.tsbConsult.Enabled = true;
            }
            MessageBox.Show(ex, "系统错误", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            AgentBar.Log.Fatal("发生严重错误:" + ex);
        }

        private void OnEvent_CallOutRing(string agent_call_uuid, string callerID, string calledID, string accessNumName, string callType, string areaID, string areaName, string customer_num_format_local, string customer_num_format_national, string customer_num_format_e164, string customer_num_phone_type, string customerForeignId, string relation_uuid)
        {
            
            this.mCallType = Call_Type.MANUAL_CALL_OUT;
            this.mCallStatus = CallStatus.CALL_OUT;
            this.m_agent_current_call_uuid = agent_call_uuid;
            string makeStr = string.Empty;
            makeStr = string.Concat(new string[]
            {
                "#customer_num_format_local=",
                customer_num_format_local,
                "#customer_num_format_national=",
                customer_num_format_national,
                "#customer_num_format_e164=",
                customer_num_format_e164,
                "#customer_num_phone_type=",
                customer_num_phone_type,
                "#customerForeignId=",
                customerForeignId,
                "#agent_current_call_uuid=",
                agent_call_uuid,
                "#relation_uuid=",
                relation_uuid,
                "#"
            });

            if (this.CallOutRingEvent != null)
            {
                this.CallOutRingEvent(callerID, calledID, accessNumName, callType, areaID, areaName, makeStr);
            }
            if ((this.SoftPhoneEnable2 || this.bindSoftPhoneLogin) && this.mSoftphoneAutoAnswer)
            {
                if (!this.DoAnswer())
                {
                    //MessageBox.Show("接听失败！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                }
            }
        }

        private void OnEvent_Manual_Callout(string agentID, string reason, int retCode)
        {
           
            if (retCode == 0)
            {
                
            }
            else
            {
                string failedReason = "呼出失败";
                if ("ALL_GATEWAYS_FULL" == reason)
                {
                    failedReason += "：外线满线";
                    if (this.AllGetwaysFullEvent != null)
                    {
                        this.AllGetwaysFullEvent();
                    }
                }
            }
        }

        private void OnEvent_SignIn(string AgentID, string AgentName, string AgentExten, string AgentGroupID, string AgentGroupName, bool AutoPostTreatment, bool BindExten, bool GradeSwitch, string InitStatus, string RoleID, int retCode, string strReason, int heartbeat_timeout, string SaltKey, string DID_Num)
        {
            if (0 == retCode)
            {
                this.mAgentState = Agent_State.AGENT_STATUS_SIGNIN;
                this.mAgentID = AgentID;
                this.mAgentName = AgentName;
                this.mAgentExten = AgentExten;
                this.mAgentGroupID = AgentGroupID;
                this.mAgentGroupName = AgentGroupName;
                this.mBindExten = BindExten;
                this.mGradeSwitch = GradeSwitch;
                this.mAgentStatus = InitStatus;
                this.mRoleID = RoleID;
                this.mSaltKey = SaltKey;
                this.mDID_Num = DID_Num;
                ComFunc.mTodayDate = "";
                ComFunc.mDESKey = "";
                this.mInitStatusWhenLogin = InitStatus;
                if (heartbeat_timeout > 0)
                {
                    this.mHeartBeatTimeout = heartbeat_timeout;
                }
                else
                {
                    Log.Error("heartbeat is invalid,we will use default value!value=" + this.HeartBeatTimeout);
                }
                
                this.BeginTheTimer();
                
                Thread.Sleep(100);
                if (!this.DoGetDefinedRoleRights())
                {
                    AgentBar.Log.Debug("获取坐席角色和权限失败！");
                }
                if (!this.DoGetCustomerOfMyQueue())
                {
                    AgentBar.Log.Debug("获取坐席所属队列的客户信息失败！");
                }
                Thread thr = new Thread(new ThreadStart(this.doSomeAsyncThing));
                thr.Start();
                this.initMyApplyChangeStatus();
            }
            else
            {
                AgentBar.Log.Debug("签入失败！正在断开连接............");
                this.DoDisconnect();
            }
            if (this.SignInEvent != null)
            {
                this.SignInEvent(AgentID, retCode, strReason);
            }
        }

        private void initMyApplyChangeStatus()
        {
            this.my_apply_change_status.isFinished = true;
            this.my_apply_change_status.applyAgentID = "";
            this.my_apply_change_status.agentName = "";
            this.my_apply_change_status.applyAgentGroupID = "";
            this.my_apply_change_status.applyAgentGroupName = "";
            this.my_apply_change_status.applyState = Apply_State.Unknow;
            this.my_apply_change_status.applyStateStr = "";
            this.my_apply_change_status.applyTime = "";
            this.my_apply_change_status.approveTime = "";
            this.my_apply_change_status.executeTime = "";
            this.my_apply_change_status.targetStatus = "";
        }

        private void doSomeAsyncThing()
        {
        }

        private void OnEvent_SignOut(string agentID, int retCode, string reason)
        {
            
            this.mAgentState = Agent_State.AGENT_STATUS_OFFLINE;
            this.mCallStatus = CallStatus.NO_CALL;

            if (this.mSipAutoSignIn)
            {
                this.softPhone_close();
            }
            if (this.SignOutEvent != null)
            {
                this.SignOutEvent(this.AgentID, retCode, reason);
            }
        }

        private void OnEvent_KickOut(string agentID, int retCode, string reason)
        {
            if (this.mSipAutoSignIn)
            {
                this.softPhone_close();
            }
            if (this.KickOutEvent != null)
            {
                this.KickOutEvent(this.AgentID, retCode, reason);
            }
        }

        private void OnEventResultEvent(string EventType, string agentID, int retCode, string strReason, string hangupReason)
        {
            string text = EventType.ToLower();
            switch (text)
            {
                case "callout":
                    if (0 == retCode)
                    {
                        
                    }
                    else
                    {
                        
                    }
                    break;
                case "offline":
                    if (0 == retCode)
                    {
                        
                    }
                    else
                    {
                       
                    }
                    break;
                case "answer":
                    if (0 == retCode)
                    {
                        
                    }
                    else
                    {
                       
                    }
                    break;
                case "bridge":
                    if (0 == retCode)
                    {
                       
                    }
                    else
                    {
                        
                    }
                    break;
                case "hold":
                    if (0 == retCode)
                    {
                        
                    }
                    else
                    {
                        
                    }
                    break;
                case "stophold":
                    if (0 == retCode)
                    {
                        
                    }
                    else
                    {
                        
                    }
                    break;
                case "listen":
                    if (0 == retCode)
                    {
                        
                    }
                    else
                    {
                        
                    }
                    break;
                case "consult":
                    if (0 == retCode)
                    {
                        
                    }
                    else
                    {
                        
                    }
                    break;
                case "stopconsult":
                    if (0 == retCode)
                    {
                        
                    }
                    else
                    {
                        
                    }
                    break;
                case "intercept":
                    if (0 == retCode)
                    {
                        
                    }
                    else
                    {
                        
                    }
                    break;
                case "interrupt":
                    if (0 == retCode)
                    {
                        
                    }
                    else
                    {
                        
                    }
                    break;
                case "forcedisconnect":
                    if (0 == retCode)
                    {
                        
                    }
                    else
                    {
                        
                    }
                    break;
                case "transfer":
                    if (0 == retCode)
                    {
                        
                    }
                    else
                    {
                        
                    }
                    break;
                case "grade":
                    if (0 == retCode)
                    {
                        
                    }
                    else
                    {
                        
                    }
                    break;
                case "manual_callout":
                    if (0 == retCode)
                    {
                        
                    }
                    else
                    {
                        
                    }
                    break;
                case "get_report_statis_info":
                    if (0 != retCode)
                    {
                        
                    }
                    break;
                case "apply_change_status":
                    if (0 == retCode)
                    {
                        
                    }
                    else
                    {
                        
                    }
                    break;
            }
        }

        private void OnSignOutEvent(string agentID, int retCode, string strReason)
        {
        }

        public bool DoSignIn()
        {
            bool result;
            if (this.mAgentID == "" || this.mAgentID == null || this.mAgentStatus == "")
            {
                result = false;
            }
            else
            {
                if (this.mAgentExten != "" && this.mAgentExten != null)
                {
                    this.mBindExten = true;
                }
                else
                {
                    this.mBindExten = false;
                }
                string strBindExten;
                if (this.mBindExten)
                {
                    strBindExten = "yes";
                }
                else
                {
                    strBindExten = "no";
                }
                string strExtenIsOutbound;
                if (this.mExtenIsOutbound)
                {
                    strExtenIsOutbound = "yes";
                }
                else
                {
                    strExtenIsOutbound = "no";
                }
                if ((this.SoftPhoneEnable2 || this.bindSoftPhoneLogin) && this.mSipAutoSignIn)
                {
                    if (this.start_softphone_app() != 0)
                    {
                        result = false;
                        return result;
                    }
                    Thread.Sleep(this.SigninIntervalAfterSoftPhoneRegisted);
                }
                long rt = this.agentDll.PL_SignIn(this.mAgentID, this.mAgentPwd, this.mAgentExten, strBindExten, this.mAgentStatus, this.mWebUrl, strExtenIsOutbound);
                result = (0L == rt);
            }
            return result;
        }

        public bool DoSignInWithConnect()
        {
            bool result;
            if (this.mServerIP == "" || this.mPort == 0 || this.mServerIP == null)
            {
                result = false;
            }
            else if (this.mAgentID == "" || this.mAgentID == null || this.mAgentStatus == "")
            {
                result = false;
            }
            else
            {
                if (this.mAgentExten != "" && this.mAgentExten != null)
                {
                    this.mBindExten = true;
                }
                else
                {
                    this.mBindExten = false;
                }
                if ((this.SoftPhoneEnable2 || this.bindSoftPhoneLogin) && this.mSipAutoSignIn)
                {
                    if (this.start_softphone_app() != 0)
                    {
                        result = false;
                        return result;
                    }
                    Thread.Sleep(this.SigninIntervalAfterSoftPhoneRegisted);
                }
                long rt = this.agentDll.PL_ConnectToCti(this.mServerIP, this.mPort);
                if (0L != rt)
                {
                    result = false;
                }
                else
                {
                    this.blnConnect = true;
                    string strBindExten;
                    if (this.mBindExten)
                    {
                        strBindExten = "yes";
                    }
                    else
                    {
                        strBindExten = "no";
                    }
                    string strExtenIsOutbound;
                    if (this.mExtenIsOutbound)
                    {
                        strExtenIsOutbound = "yes";
                    }
                    else
                    {
                        strExtenIsOutbound = "no";
                    }
                    rt = this.agentDll.PL_SignIn(this.mAgentID, this.mAgentPwd, this.mAgentExten, strBindExten, this.mAgentStatus, this.mWebUrl, strExtenIsOutbound);
                    if (0L != rt)
                    {
                        this.agentDll.PL_DisConnectToCti();
                        result = false;
                    }
                    else
                    {
                        result = true;
                    }
                }
            }
            return result;
        }

        public bool softPhone_close()
        {
            bool result;
            if (!string.IsNullOrEmpty(this.SoftPhoneAppName) && !string.IsNullOrEmpty(this.SoftPhoneAppClassName) && (this.SoftPhoneEnable2 || this.bindSoftPhoneLogin))
            {
                if (this.IsInUse)
                {
                    result = false;
                    return result;
                }
                IntPtr hwnd = AgentBar.GetProcessWindowHandle(this.SoftPhoneAppClassName, this.SoftPhoneAppName);
                if (hwnd != IntPtr.Zero)
                {
                    if (!AgentBar.PostMessageApi(hwnd, this.SoftPhoneMsgValue, this.SoftPhoneLogoffCmd, this.SoftPhoneLogoffCmd))
                    {
                        MessageBox.Show("软电话签出失败！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    }
                    Thread.Sleep(3000);
                    result = true;
                    return result;
                }
            }
            result = false;
            return result;
        }

        private int start_softphone_app()
        {
            int first_login_delay_time = 0;
            Process[] soft_phone_app_process = Process.GetProcessesByName(this.SoftPhoneAppProcessName);
            int result;
            if (0 == soft_phone_app_process.Count<Process>())
            {
                string arg = string.Concat(new object[]
                {
                    "-u",
                    this.SipNum,
                    " -p",
                    this.SipPwd,
                    " -s",
                    this.SipServer,
                    ":",
                    this.SipPort
                });
                string process_path = ComFunc.APPDATA_PATH + "\\CTIClient\\视频通话\\VideoTelephone.exe";
                string work_dir = ComFunc.APPDATA_PATH + "\\CTIClient\\视频通话\\";
                if (!Directory.Exists(ComFunc.APPDATA_PATH + "\\wonderUsers\\" + this.SipNum))
                {
                    first_login_delay_time = 8000;
                }
                if (!this.start_process(work_dir, process_path, arg))
                {
                    result = -1;
                    return result;
                }
                Thread.Sleep(3000 + first_login_delay_time);
            }
            else
            {
                if (this.mSipPhoneOnLineWarning)
                {
                    if (MessageBox.Show("内置软电话程序正在运行中，如果正在通话则电话将被挂断，您是否确定要关闭它？", "退出内置软电话", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                    {
                        result = -2;
                        return result;
                    }
                }
                try
                {
                    Process[] array = soft_phone_app_process;
                    for (int i = 0; i < array.Length; i++)
                    {
                        Process p = array[i];
                        p.Kill();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                Thread.Sleep(3000);
                this.start_softphone_app();
            }
            result = 0;
            return result;
        }

        public static IntPtr GetProcessWindowHandle(string className, string strProcTitle)
        {
            IntPtr hwnd = IntPtr.Zero;
            return AgentBar.FindWindow(className, strProcTitle);
        }

        public bool DoSignOut()
        {
            bool result;
            if (this.mAgentID == null)
            {
                result = false;
            }
            else if (this.mAgentID == "")
            {
                result = false;
            }
            else
            {
                long rt = this.agentDll.PL_SignOut(this.mAgentID);
                if (0L != rt)
                {
                    result = false;
                }
                else
                {
                    this.mCallStatus = CallStatus.NO_CALL;
                    result = true;
                }
            }
            return result;
        }

        public int DoConnect()
        {
            int result;
            if (this.mServerIP == null || this.mPort == 0)
            {
                result = -1;
            }
            else if (this.mServerIP == "" || this.mPort == 0)
            {
                result = -1;
            }
            else
            {
                long rt = this.agentDll.PL_ConnectToCti(this.mServerIP, this.mPort);
                if (0L != rt)
                {
                    result = -3;
                }
                else
                {
                    this.blnConnect = true;
                    result = 0;
                }
            }
            return result;
        }

        public bool DoDisconnect()
        {
            long rt = this.agentDll.PL_DisConnectToCti();
            if (0L != rt)
            {
                PLAgent.Log.Error("PL_DisConnectToCti is error!maybe it has disconnect already! retCode=" + rt);
            }
            this.blnConnect = false;
            this.mCallStatus = CallStatus.NO_CALL;
            return true;
        }

        public bool DoCallOut(string strCallID, string strDisplayNum)
        {
            bool result;
            if (this.mAgentID == null)
            {
                result = false;
            }
            else if (this.mAgentID == "" || strCallID == "")
            {
                result = false;
            }
            else
            {
                long rt = this.agentDll.PL_CallOut(this.mAgentID, strCallID, strDisplayNum, 1, "", "0", "", "");
                result = (0L == rt);
            }
            return result;
        }

        public bool DoCallOutForeignId(string strCallID, string strDisplayNum, string customerForeignId)
        {
           
            bool result;
            if (this.mAgentID == null)
            {
                result = false;
            }
            else if (this.mAgentID == "" || strCallID == "")
            {
                result = false;
            }
            else
            {
                long rt = this.agentDll.PL_CallOut(this.mAgentID, strCallID, strDisplayNum, 1, "", "0", "", customerForeignId);
                result = (0L == rt);
            }
            return result;
        }

        public bool DoCallAgent(string strAgentNum)
        {
            bool result;
            if (this.mAgentID == null)
            {
                result = false;
            }
            else if (this.mAgentID == "" || strAgentNum == "")
            {
                result = false;
            }
            else
            {
                long rt = this.agentDll.PL_CallAgent(this.mAgentID, strAgentNum);
                result = (0L == rt);
            }
            return result;
        }

        public bool DoAnswer()
        {
            bool result;
            if (this.mSoftPhoneEnable)
            {
                if (!this.mSoftPhone.PL_Answer())
                {
                    result = false;
                    return result;
                }
            }
            else
            {
                int rt =PLAgent.PostMsgToSoftPhone(this.mSoftPhoneWindowHandle, this.mSoftPhone_app_className, this.mSoftPhone_app_name, this.mSoftPhone_msg_value, this.mSoftPhone_answer_cmd, this.mSoftPhone_answer_cmd);
                if (rt != 0)
                {
                    result = false;
                    return result;
                }
                OperatingSystem os = Environment.OSVersion;
                if (os.Version.Major < 6)
                {
                    Thread.Sleep(100);
                    rt = PLAgent.PostMsgToSoftPhone(this.mSoftPhoneWindowHandle, this.mSoftPhone_app_className, this.mSoftPhone_app_name, this.mSoftPhone_msg_value, this.mSoftPhone_answer_cmd, this.mSoftPhone_answer_cmd);
                }
            }
            result = true;
            return result;
        }

        public bool DoHangUp()
        {
            bool result;
            if (this.mAgentID == null)
            {
                result = false;
            }
            else if (this.mAgentID == "")
            {
                result = false;
            }
            else
            {
                long rt = this.agentDll.PL_HangUp(this.mAgentID, this.m_agent_current_call_uuid);
                result = (0L == rt);
            }
            return result;
        }

        public bool DoGetAgentDefineStatus()
        {
            bool result;
            if (this.mAgentID == null)
            {
                result = false;
            }
            else if (this.mAgentID == "")
            {
                result = false;
            }
            else
            {
                long rt = this.agentDll.PL_GetAgentDefineStatus(this.mAgentID);
                result = (0L == rt);
            }
            return result;
        }

        public bool DoGetAgentWebSiteInfo()
        {
            bool result;
            if (this.mAgentID == null)
            {
                result = false;
            }
            else if (this.mAgentID == "")
            {
                result = false;
            }
            else
            {
                long rt = this.agentDll.PL_GetAgentWebSiteInfo(this.mAgentID);
                result = (0L == rt);
            }
            return result;
        }

        public bool DoHold()
        {
            bool result;
            if (this.mAgentID == null || this.mAgentExten == null)
            {
                result = false;
            }
            else if (this.mAgentID == "" || this.mAgentExten == "")
            {
                result = false;
            }
            else
            {
                long rt = this.agentDll.PL_Hold(this.mAgentID, this.m_agent_current_call_uuid);
                result = (0L == rt);
            }
            return result;
        }

        public bool DoStopHold()
        {
            bool result;
            if (this.mAgentID == null || this.mAgentExten == null)
            {
                result = false;
            }
            else if (this.mAgentID == "" || this.mAgentExten == "")
            {
                result = false;
            }
            else
            {
                long rt = this.agentDll.PL_StopHold(this.mAgentID, this.m_agent_current_call_uuid);
                result = (0L == rt);
            }
            return result;
        }

        public bool DoMute()
        {
            bool result;
            if (string.IsNullOrEmpty(this.mAgentID))
            {
                result = false;
            }
            else if (string.IsNullOrEmpty(this.mAgentExten))
            {
                result = false;
            }
            else
            {
                long rt = this.agentDll.PL_Mute(this.mAgentID, this.m_agent_current_call_uuid);
                result = (0L == rt);
            }
            return result;
        }

        public bool DoStopMute()
        {
            bool result;
            if (string.IsNullOrEmpty(this.mAgentID))
            {
                result = false;
            }
            else if (string.IsNullOrEmpty(this.mAgentExten))
            {
                result = false;
            }
            else
            {
                long rt = this.agentDll.PL_StopMute(this.mAgentID, this.m_agent_current_call_uuid);
                result = (0L == rt);
            }
            return result;
        }

        public bool DoConsult(string destAgentID, bool trdIsOutbound)
        {
            bool result;
            if (this.mAgentID == null || this.mAgentExten == null || destAgentID == null)
            {
                result = false;
            }
            else if (this.mAgentID == "" || this.mAgentExten == "" || destAgentID == "")
            {
                result = false;
            }
            else
            {
                long rt = this.agentDll.PL_Consult(this.mAgentID, destAgentID, this.m_agent_current_call_uuid, trdIsOutbound);
                result = (0L == rt);
            }
            return result;
        }

        public bool DoStopConsult()
        {
            bool result;
            if (this.mAgentID == null || this.mAgentExten == null)
            {
                result = false;
            }
            else if (this.mAgentID == "" || this.mAgentExten == "")
            {
                result = false;
            }
            else
            {
                long rt = this.agentDll.PL_ConsultCancel(this.mAgentID, this.m_agent_current_call_uuid);
                result = (0L == rt);
            }
            return result;
        }

        public bool DoConsultTransfer()
        {
            bool result;
            if (this.mAgentID == null || this.mAgentExten == null)
            {
                result = false;
            }
            else if (this.mAgentID == "" || this.mAgentExten == "")
            {
                result = false;
            }
            else
            {
                long rt = this.agentDll.PL_ConsultTransfer(this.mAgentID, this.m_agent_current_call_uuid);
                result = (0L == rt);
            }
            return result;
        }

        public bool DoListen(string strAgentID)
        {
            bool result;
            if (this.mAgentID == null || this.mAgentExten == null || strAgentID == null)
            {
                result = false;
            }
            else if (this.mAgentID == "" || this.mAgentExten == "" || strAgentID == "")
            {
                result = false;
            }
            else
            {
                long rt = this.agentDll.PL_Listen(this.mAgentID, strAgentID);
                result = (0L == rt);
            }
            return result;
        }

        public bool DoWhisper(string strAgentID)
        {
            bool result;
            if (this.mAgentID == null || this.mAgentExten == null || strAgentID == null)
            {
                result = false;
            }
            else if (this.mAgentID == "" || this.mAgentExten == "" || strAgentID == "")
            {
                result = false;
            }
            else
            {
                long rt = this.agentDll.PL_Whisper(this.mAgentID, strAgentID);
                result = (0L == rt);
            }
            return result;
        }

        public bool DoForceChangeStatusEvent(string strAgentID, string status)
        {
            bool result;
            if (this.mAgentID == null || strAgentID == null)
            {
                result = false;
            }
            else if (this.mAgentID == "" || strAgentID == "")
            {
                result = false;
            }
            else if (status == "" || status == "")
            {
                result = false;
            }
            else
            {
                long rt = this.agentDll.PL_OnForceChangeStatus(this.mAgentID, strAgentID, status);
                result = (0L == rt);
            }
            return result;
        }

        public bool DoTransferAgent(string destAgentID, string strOutBoundFlag)
        {
            bool result;
            if (this.mAgentID == null || this.mAgentExten == null)
            {
                result = false;
            }
            else if (this.mAgentID == "" || this.mAgentExten == "" || destAgentID == "" || strOutBoundFlag == "")
            {
                result = false;
            }
            else
            {
                long rt = this.agentDll.PL_TransferAgent(this.mAgentID, destAgentID, this.m_customer_current_call_uuid, strOutBoundFlag);
                result = (0L == rt);
            }
            return result;
        }

        public bool DoTransferIvr(string ivrNum)
        {
            bool result;
            if (this.mAgentID == null || this.mAgentExten == null)
            {
                result = false;
            }
            else if (this.mAgentID == "" || this.mAgentExten == "" || ivrNum == "")
            {
                result = false;
            }
            else
            {
                long rt = this.agentDll.PL_TransferIvr(this.mAgentID, ivrNum, this.m_customer_current_call_uuid);
                result = (0L == rt);
            }
            return result;
        }

        public bool DoTransferQueue(string queueNum)
        {
            bool result;
            if (this.mAgentID == null || this.mAgentExten == null)
            {
                result = false;
            }
            else if (this.mAgentID == "" || this.mAgentExten == "" || queueNum == "")
            {
                result = false;
            }
            else
            {
                long rt = this.agentDll.PL_TransferQueue(this.mAgentID, queueNum, this.m_customer_current_call_uuid);
                result = (0L == rt);
            }
            return result;
        }

        public bool DoTransferIvrProfile(string ivrProfileNum)
        {
            bool result;
            if (this.mAgentID == null || null == this.mAgentExten)
            {
                result = false;
            }
            else if (string.Empty == this.mAgentID || string.Empty == this.mAgentExten || string.Empty == ivrProfileNum)
            {
                result = false;
            }
            else
            {
                long rt = this.agentDll.PL_TransferIvrProfile(this.mAgentID, ivrProfileNum, this.m_customer_current_call_uuid);
                result = (0L == rt);
            }
            return result;
        }

        public bool DoSetAgentDefineStatus(int targetStatus, int IsNeedApproval)
        {
            bool result;
            if (this.mAgentID == null)
            {
                result = false;
            }
            else if (this.mAgentID == "")
            {
                result = false;
            }
            else
            {
                long rt = this.agentDll.PL_SetAgentDefineStatus(this.mAgentID, targetStatus, IsNeedApproval);
                result = (0L == rt);
            }
            return result;
        }

        public bool DoIntercept(string strCuid)
        {
            bool result;
            if (this.mAgentID == null || this.mAgentExten == null || strCuid == null)
            {
                result = false;
            }
            else if (this.mAgentID == "" || this.mAgentExten == "" || strCuid == "")
            {
                result = false;
            }
            else
            {
                long rt = this.agentDll.PL_Intercept(this.mAgentID, this.mAgentExten, strCuid);
                result = (0L == rt);
            }
            return result;
        }

        public bool DoBargein(string strAgentID)
        {
            bool result;
            if (this.mAgentID == null || this.mAgentExten == null || strAgentID == null)
            {
                result = false;
            }
            else if (this.mAgentID == "" || this.mAgentExten == "" || strAgentID == "")
            {
                result = false;
            }
            else
            {
                long rt = this.agentDll.PL_Bargein(this.mAgentID, strAgentID);
                result = (0L == rt);
            }
            return result;
        }

        public bool DoForceHangup(string strAgentID)
        {
            bool result;
            if (this.mAgentID == null || this.mAgentExten == null || strAgentID == null)
            {
                result = false;
            }
            else if (this.mAgentID == "" || this.mAgentExten == "" || strAgentID == "")
            {
                result = false;
            }
            else
            {
                long rt = this.agentDll.PL_ForceHangup(this.mAgentID, strAgentID);
                result = (0L == rt);
            }
            return result;
        }

        private bool DoHeartBeat()
        {
            bool result;
            if (this.mAgentID == null || this.mAgentID == "")
            {
                result = false;
            }
            else
            {
                long rt = this.agentDll.PL_HeartBeat(this.mAgentID);
                result = (0L == rt);
            }
            return result;
        }

        public bool DoSignInAck()
        {
            bool result;
            if (this.mAgentID == null || this.mAgentExten == null)
            {
                result = false;
            }
            else if (this.mAgentID == "" || this.mAgentExten == "")
            {
                result = false;
            }
            else
            {
                long rt = this.agentDll.PL_SignAck(this.mAgentID, this.mAgentExten);
                result = (0L == rt);
            }
            return result;
        }

        public bool DoCheck()
        {
            bool result;
            if (this.mAgentID == null || this.mAgentExten == null)
            {
                result = false;
            }
            else if (this.mAgentID == "" || this.mAgentExten == "")
            {
                result = false;
            }
            else
            {
                long rt = this.agentDll.PL_Check(this.mAgentID);
                result = (0L == rt);
            }
            return result;
        }

        public bool DoSignInFin()
        {
            bool result;
            if (this.mAgentID == null || this.mAgentExten == null)
            {
                result = false;
            }
            else if (this.mAgentID == "" || this.mAgentExten == "")
            {
                result = false;
            }
            else
            {
                long rt = this.agentDll.PL_SignFin(this.mAgentID, this.mAgentExten);
                result = (0L == rt);
            }
            return result;
        }

        public bool DoGrade(string language)
        {
            bool result;
            if (this.mAgentID == null || this.mAgentExten == null)
            {
                result = false;
            }
            else if (this.mAgentID == "" || this.mAgentExten == "")
            {
                result = false;
            }
            else
            {
                long rt = this.agentDll.PL_Grade(this.mAgentID, language, this.m_customer_current_call_uuid);
                result = (0L == rt);
            }
            return result;
        }

        public bool DoGetAccessNumbers()
        {
            bool result;
            if (this.mAgentID == null)
            {
                result = false;
            }
            else if (this.mAgentID == "")
            {
                result = false;
            }
            else
            {
                long rt = this.agentDll.PL_GetAccessNumbers(this.mAgentID);
                result = (0L == rt);
            }
            return result;
        }

        public bool DoThreeWay(string trdAgentNum, bool trdIsOutbound)
        {
            bool result;
            if (this.mAgentID == null || this.mAgentExten == null)
            {
                result = false;
            }
            else if (this.mAgentID == "" || this.mAgentExten == "" || trdAgentNum == "")
            {
                result = false;
            }
            else
            {
                long rt = this.agentDll.PL_ThreeWay(this.mAgentID, trdAgentNum, this.m_agent_current_call_uuid, trdIsOutbound);
                result = (0L == rt);
            }
            return result;
        }

        public bool DoThreeWayCancel()
        {
            bool result;
            if (this.mAgentID == null || this.mAgentExten == null)
            {
                result = false;
            }
            else if (this.mAgentID == "" || this.mAgentExten == "")
            {
                result = false;
            }
            else
            {
                long rt = this.agentDll.PL_ThreeWayCancel(this.mAgentID, this.m_agent_current_call_uuid);
                result = (0L == rt);
            }
            return result;
        }

        public bool DoGetOnlineAgent(string specificNum, int numType)
        {
            bool result;
            if (numType != 1 && numType != 2 && numType != 3)
            {
                result = false;
            }
            else
            {
                if (numType == 1 || numType == 2)
                {
                    if (string.IsNullOrEmpty(specificNum))
                    {
                        result = false;
                        return result;
                    }
                }
                if (this.mAgentID == null || this.mAgentExten == null)
                {
                    result = false;
                }
                else if (this.mAgentID == "" || this.mAgentExten == "")
                {
                    result = false;
                }
                else
                {
                    long rt = this.agentDll.PL_GetAgentOnline(this.mAgentID, specificNum, numType);
                    result = (0L == rt);
                }
            }
            return result;
        }

        public bool DoGetIvrList()
        {
            bool result;
            if (this.mAgentID == null || this.mAgentExten == null)
            {
                result = false;
            }
            else if (this.mAgentID == "" || this.mAgentExten == "")
            {
                result = false;
            }
            else
            {
                long rt = this.agentDll.PL_GetIvrList(this.mAgentID);
                result = (0L == rt);
            }
            return result;
        }

        public bool DoGetQueueList()
        {
            bool result;
            if (this.mAgentID == null || this.mAgentID == "")
            {
                result = false;
            }
            else
            {
                long rt = this.agentDll.PL_GetQueueList(this.mAgentID);
                result = (0L == rt);
            }
            return result;
        }

        public bool DoGetIvrProfileList()
        {
            bool result;
            if (this.mAgentID == null || null == this.mAgentExten)
            {
                result = false;
            }
            else if (string.Empty == this.mAgentID || string.Empty == this.mAgentExten)
            {
                result = false;
            }
            else
            {
                long rt = this.agentDll.PL_GetIvrProfileList(this.mAgentID);
                result = (0L == rt);
            }
            return result;
        }

        public bool DoGetDefinedRoleRights()
        {
            bool result;
            if (this.mAgentID == null)
            {
                result = false;
            }
            else if (this.mAgentID == "")
            {
                result = false;
            }
            else
            {
                long rt = this.agentDll.PL_GetDefinedRoleAndRight(this.mAgentID);
                result = (0L == rt);
            }
            return result;
        }

        public bool DoGetAgentGroupList(string agentGroupRange)
        {
            bool result;
            if (this.mAgentID == null)
            {
                result = false;
            }
            else if (this.mAgentID == "")
            {
                result = false;
            }
            else
            {
                long rt = this.agentDll.PL_GetAgentGroupList(this.mAgentID, agentGroupRange);
                result = (0L == rt);
            }
            return result;
        }

        public bool DoGetAgentsOfQueue(string strQueueNum)
        {
            bool result;
            if (this.mAgentID == null || strQueueNum == null)
            {
                result = false;
            }
            else if (this.mAgentID == "" || strQueueNum == "")
            {
                result = false;
            }
            else
            {
                long rt = this.agentDll.PL_GetAgentsOfQueue(this.mAgentID, strQueueNum);
                result = (0L == rt);
            }
            return result;
        }

        public bool DoGetAgentsOfAgentGroup(string strAgentGroupNum)
        {
            bool result;
            if (this.mAgentID == null || strAgentGroupNum == null)
            {
                result = false;
            }
            else if (this.mAgentID == "" || strAgentGroupNum == "")
            {
                result = false;
            }
            else
            {
                long rt = this.agentDll.PL_GetAgentsOfAgentGroup(this.mAgentID, strAgentGroupNum);
                result = (0L == rt);
            }
            return result;
        }

        public bool DoGetAgentsMonitorInfo(string agentsStr)
        {
            bool result;
            if (this.mAgentID == null || agentsStr == null)
            {
                result = false;
            }
            else if (this.mAgentID == "" || agentsStr == "")
            {
                result = false;
            }
            else
            {
                long rt = this.agentDll.PL_GetAgentsMonitorInfo(this.mAgentID, agentsStr);
                result = (0L == rt);
            }
            return result;
        }

        public bool DoGetDetailCallInfo(string targetAgentNum)
        {
            bool result;
            if (this.mAgentID == null || targetAgentNum == null)
            {
                result = false;
            }
            else if (this.mAgentID == "" || targetAgentNum == "")
            {
                result = false;
            }
            else
            {
                long rt = this.agentDll.PL_GetDetailCallInfo(this.mAgentID, targetAgentNum);
                result = (0L == rt);
            }
            return result;
        }

        public bool DoGetCustomerOfQueue(string queue_num)
        {
            bool result;
            if (this.mAgentID == null || queue_num == null)
            {
                result = false;
            }
            else if (this.mAgentID == "" || queue_num == "")
            {
                result = false;
            }
            else
            {
                long rt = this.agentDll.PL_GetCustomerOfQueue(this.mAgentID, queue_num);
                result = (0L == rt);
            }
            return result;
        }

        public bool DoGetCustomerOfMyQueue()
        {
            bool result;
            if (this.mAgentID == null)
            {
                result = false;
            }
            else if (this.mAgentID == "")
            {
                result = false;
            }
            else
            {
                long rt = this.agentDll.PL_GetCustomerOfMyQueue(this.mAgentID);
                result = (0L == rt);
            }
            return result;
        }

        public bool DoGetQueueStatis(string queue_num)
        {
            bool result;
            if (this.mAgentID == null)
            {
                result = false;
            }
            else if (this.mAgentID == "")
            {
                result = false;
            }
            else if (string.IsNullOrEmpty(queue_num))
            {
                result = false;
            }
            else
            {
                long rt = this.agentDll.PL_GetQueueStatis(this.mAgentID, queue_num);
                result = (0L == rt);
            }
            return result;
        }

        public bool DoGetAllQueueStatis()
        {
            bool result;
            if (this.mAgentID == null)
            {
                result = false;
            }
            else if (this.mAgentID == "")
            {
                result = false;
            }
            else
            {
                long rt = this.agentDll.PL_GetAllQueueStatis(this.mAgentID);
                result = (0L == rt);
            }
            return result;
        }

        public bool DoApplyChangeStatus(string targetStatus)
        {
            bool result;
            if (string.IsNullOrEmpty(this.mAgentID))
            {
                result = false;
            }
            else
            {
                long rt = this.agentDll.PL_ApplyChangeStatus(this.mAgentID, targetStatus);
                result = (0L == rt);
            }
            return result;
        }

        public bool DoApplyApproval(string applyAgentID, string targetStatus, string passFlag)
        {
            
            bool result;
            if (string.IsNullOrEmpty(this.mAgentID) || string.IsNullOrEmpty(applyAgentID) || string.IsNullOrEmpty(targetStatus) || string.IsNullOrEmpty(passFlag))
            {
                result = false;
            }
            else
            {
                long rt = this.agentDll.PL_ApplyApproval(this.mAgentID, applyAgentID, targetStatus, passFlag);
                result = (0L == rt);
            }
            return result;
        }

        public bool DoApplyCancel(string targetStatus)
        {
            bool result;
            if (string.IsNullOrEmpty(this.mAgentID) || string.IsNullOrEmpty(targetStatus))
            {
                result = false;
            }
            else
            {
                long rt = this.agentDll.PL_ApplyCancel(this.mAgentID, targetStatus);
                result = (0L == rt);
            }
            return result;
        }

        public bool DoGetAgentGroupStatusMaxNum(string targetAgentGroupNameLstStr, string targetAgentGroupIdLstStr, string targetStatus)
        {
           
            bool result;
            if (string.IsNullOrEmpty(this.mAgentID) || string.IsNullOrEmpty(targetAgentGroupIdLstStr) || string.IsNullOrEmpty(targetStatus))
            {
                result = false;
            }
            else
            {
                long rt = this.agentDll.PL_getAgentGroupStatusMaxNum(this.mAgentID, targetAgentGroupNameLstStr, targetAgentGroupIdLstStr, targetStatus);
                result = (0L == rt);
            }
            return result;
        }

        public bool DoSetAgentGroupStatusMaxNum(string targetAgentGroupNameLstStr, string targetAgentGroupIdLstStr, string targetStatus, string maxStatusNum)
        {
            
            bool result;
            if (string.IsNullOrEmpty(this.mAgentID) || string.IsNullOrEmpty(targetAgentGroupIdLstStr) || string.IsNullOrEmpty(targetStatus) || string.IsNullOrEmpty(maxStatusNum))
            {
                result = false;
            }
            else
            {
                long rt = this.agentDll.PL_setAgentGroupStatusMaxNum(this.mAgentID, targetAgentGroupNameLstStr, targetAgentGroupIdLstStr, targetStatus, maxStatusNum);
                result = (0L == rt);
            }
            return result;
        }

        public bool DoGetChangeStatusApplyList(string targetStatus)
        {
            bool result;
            if (string.IsNullOrEmpty(this.mAgentID))
            {
                result = false;
            }
            else if (string.IsNullOrEmpty(targetStatus))
            {
                result = false;
            }
            else
            {
                long rt = this.agentDll.PL_GetChangeStatusApplyList(this.mAgentID, targetStatus);
                result = (0L == rt);
            }
            return result;
        }

        public int DoClickAgentBarButtonByName(string btnName)
        {
            int result;
            if (string.IsNullOrEmpty(btnName))
            {
                result = -1;
            }
            else if (!this.agentTool.Items.ContainsKey(btnName) && !this.other.DropDownItems.Contains(this.tsbApprove))
            {
                result = -2;
            }
            else
            {
                if (this.agentTool.Items.ContainsKey(btnName))
                {
                    int btnIndex = this.agentTool.Items.IndexOfKey(btnName);
                    this.agentTool.Items[btnIndex].PerformClick();
                }
                if (this.other.DropDownItems.Contains(this.tsbApprove))
                {
                    int btnIndex = this.other.DropDownItems.IndexOfKey(btnName);
                    this.other.DropDownItems[btnIndex].PerformClick();
                }
                result = 0;
            }
            return result;
        }


        private void ReceiveAgentEvents(AgentEvent agent_event)
        {
            AgentBar.Log.Debug("enter ReceiveAgentEvents");
            try
            {
                if (base.InvokeRequired)
                {
                    AgentBar.ReceiveEventHandler delegateEvtReceive = new AgentBar.ReceiveEventHandler(this.RaiseAllEvents);
                    base.Invoke(delegateEvtReceive, new object[]
                    {
                        agent_event
                    });
                }
                else
                {
                    this.RaiseAllEvents(agent_event);
                }
            }
            catch (Exception e)
            {
                AgentBar.Log.Debug("enter ReceiveAgentEvents e: " + e.ToString());
                AgentBar.Log.Debug("enter ReceiveAgentEvents e.Message: " + e.Message);
                AgentBar.Log.Debug("enter ReceiveAgentEvents e.Source: " + e.Source);
                AgentBar.Log.Debug("enter ReceiveAgentEvents e.StackTrace: " + e.StackTrace);
            }
        }

        private void ReceiveAgentHeartBeatEvents(AgentEvent agent_event)
        {
            AgentBar.Log.Debug("enter ReceiveAgentHeartBeatEvents");
            try
            {
                if (base.InvokeRequired)
                {
                    AgentBar.ReceiveHeartBeatEventHandler delegateHeartBeatEvtReceive = new AgentBar.ReceiveHeartBeatEventHandler(this.RaiseHeartBeatEvents);
                    base.Invoke(delegateHeartBeatEvtReceive, new object[]
                    {
                        agent_event
                    });
                }
                else
                {
                    this.RaiseHeartBeatEvents(agent_event);
                }
            }
            catch (Exception e)
            {
                AgentBar.Log.Debug("enter ReceiveAgentHeartBeatEvents e: " + e.ToString());
                AgentBar.Log.Debug("enter ReceiveAgentHeartBeatEvents e.Message: " + e.Message);
                AgentBar.Log.Debug("enter ReceiveAgentHeartBeatEvents e.Source: " + e.Source);
                AgentBar.Log.Debug("enter ReceiveAgentHeartBeatEvents e.StackTrace: " + e.StackTrace);
            }
        }

        private void RaiseAllEvents(AgentEvent agent_event)
        {

            switch (agent_event.deAgentEventType)
            {
                case AgentEventType.AGENT_EVENT_RESPONSE:
                    {
                        EventQualifier eEventQualifier = agent_event.eEventQualifier;
                        if (eEventQualifier <= EventQualifier.SignOut_Status)
                        {
                            switch (eEventQualifier)
                            {
                                case EventQualifier.CallOut_NormalCall:
                                    this.ResponseEvent("callout", agent_event.agentID, agent_event.retCode, agent_event.reason);
                                    break;
                                case EventQualifier.CallIn_NormalCall:
                                case EventQualifier.Disconnect_NormalCall:
                                case EventQualifier.Bridge_NormalCall:
                                case EventQualifier.SignIn_Ring_NormalCall:
                                case EventQualifier.SignIn_Play_NormalCall:
                                case EventQualifier.Consult_Call_In:
                                case EventQualifier.Internal_Caller_Ring:
                                case EventQualifier.Internal_Call_CallIn:
                                case EventQualifier.Predict_CallOut_Bridge_Ring:
                                case EventQualifier.Three_Way_Call_In:
                                case EventQualifier.Eavesdrop_Ring_Myself:
                                case EventQualifier.Whisper_Ring_Myself:
                                case EventQualifier.Bargein_Ring_Myself:
                                case EventQualifier.Force_Hangup_Ring_Myself:
                                case EventQualifier.Transfer_Blind_Call_In:
                                case EventQualifier.Get_Defined_Role_Rights:
                                case (EventQualifier)157:
                                case EventQualifier.Add_Customer_To_Queue:
                                case EventQualifier.Del_Customer_From_Queue:
                                case EventQualifier.Update_Customer_Of_Queue:
                                case EventQualifier.Consultee_Hangup_Call:
                                case EventQualifier.Threewayee_Hangup_Call:
                                case EventQualifier.Get_Website_Info:
                                case EventQualifier.Callee_Answered:
                                case EventQualifier.Get_Agent_Personal_info:
                                case EventQualifier.Set_Agent_Personal_info:
                                case EventQualifier.Change_Pswd:
                                case EventQualifier.Get_All_Queue_Statis:
                                case EventQualifier.Apply_Change_Status_Distribute:
                                case EventQualifier.Approve_Change_Status_Distribute:
                                case EventQualifier.Get_Agentgroup_Status_Max_Num:
                                case EventQualifier.Set_Agentgroup_Status_Max_Num:
                                    break;
                                case EventQualifier.HangUp_NormalCall:
                                    this.ResponseEvent("hangup", agent_event.agentID, agent_event.retCode, agent_event.reason);
                                    break;
                                case EventQualifier.CallOut_Ring:
                                    this.ResponseEvent("callOutRing", agent_event.agentID, agent_event.retCode, agent_event.reason);
                                    break;
                                case EventQualifier.Check_ExtenStatus:
                                    this.ResponseEvent("check", agent_event.agentID, agent_event.retCode, agent_event.reason);
                                    break;
                                case EventQualifier.Hold_NormalCall:
                                    this.ResponseEvent("hold", agent_event.agentID, agent_event.retCode, agent_event.reason);
                                    break;
                                case EventQualifier.StopHold_NormalCall:
                                    this.ResponseEvent("stophold", agent_event.agentID, agent_event.retCode, agent_event.reason);
                                    break;
                                case EventQualifier.Mute_NormalCall:
                                    this.ResponseEvent("mute", agent_event.agentID, agent_event.retCode, agent_event.reason);
                                    break;
                                case EventQualifier.StopMute_NormalCall:
                                    this.ResponseEvent("unmute", agent_event.agentID, agent_event.retCode, agent_event.reason);
                                    break;
                                case EventQualifier.Consult_Cancel_Result:
                                    this.ResponseEvent("consult_cancel", agent_event.agentID, agent_event.retCode, agent_event.reason);
                                    break;
                                case EventQualifier.Eavesdrop_Result:
                                    this.ResponseEvent("eavesdrop", agent_event.agentID, agent_event.retCode, agent_event.reason);
                                    break;
                                case EventQualifier.StopListen_NormalCall:
                                    this.ResponseEvent("stoplisten", agent_event.agentID, agent_event.retCode, agent_event.reason);
                                    break;
                                case EventQualifier.Transfer_Blind_Agent_NormalCall:
                                    this.ResponseEvent("transfer_agent", agent_event.agentID, agent_event.retCode, agent_event.reason);
                                    break;
                                case EventQualifier.InterceptCall_NormalCall:
                                    this.ResponseEvent("intercept", agent_event.agentID, agent_event.retCode, agent_event.reason);
                                    break;
                                case EventQualifier.Interrupt_NormalCall:
                                    this.ResponseEvent("interrupt", agent_event.agentID, agent_event.retCode, agent_event.reason);
                                    break;
                                case EventQualifier.ForceDisconnect_NormalCall:
                                    this.ResponseEvent("forcedisconnect", agent_event.agentID, agent_event.retCode, agent_event.reason);
                                    break;
                                case EventQualifier.Internal_Call:
                                    this.ResponseEvent("internal_call", agent_event.agentID, agent_event.retCode, agent_event.reason);
                                    break;
                                case EventQualifier.Grade_NormalCall:
                                    this.ResponseEvent("evaluate", agent_event.agentID, agent_event.retCode, agent_event.reason);
                                    break;
                                case EventQualifier.Get_Access_Numbers:
                                    this.ResponseEvent("get_access_numbers", agent_event.agentID, agent_event.retCode, agent_event.reason);
                                    break;
                                case EventQualifier.Three_Way_Call_Result:
                                    this.ResponseEvent("three_way_call", agent_event.agentID, agent_event.retCode, agent_event.reason);
                                    break;
                                case EventQualifier.Three_Way_Call_Cancel_Result:
                                    this.ResponseEvent("three_way_call_cancel", agent_event.agentID, agent_event.retCode, agent_event.reason);
                                    break;
                                case EventQualifier.Consult_Call_Result:
                                    this.ResponseEvent("consult", agent_event.agentID, agent_event.retCode, agent_event.reason);
                                    break;
                                case EventQualifier.Consult_Transfer_Result:
                                    this.ResponseEvent("consult_transfer", agent_event.agentID, agent_event.retCode, agent_event.reason);
                                    break;
                                case EventQualifier.Whisper_Result:
                                    this.ResponseEvent("whisper", agent_event.agentID, agent_event.retCode, agent_event.reason);
                                    break;
                                case EventQualifier.Bargein_Result:
                                    this.ResponseEvent("bargein", agent_event.agentID, agent_event.retCode, agent_event.reason);
                                    break;
                                case EventQualifier.Force_Hangup_Result:
                                    this.ResponseEvent("force_hangup", agent_event.agentID, agent_event.retCode, agent_event.reason);
                                    break;
                                case EventQualifier.Transfer_Queue_NormalCall:
                                    this.ResponseEvent("transfer_queue", agent_event.agentID, agent_event.retCode, agent_event.reason);
                                    break;
                                case EventQualifier.Transfer_IVR_NormalCall:
                                    this.ResponseEvent("transfer_ivr", agent_event.agentID, agent_event.retCode, agent_event.reason);
                                    break;
                                case EventQualifier.Get_Online_Agents:
                                    this.ResponseEvent("get_online_agent", agent_event.agentID, agent_event.retCode, agent_event.reason);
                                    break;
                                case EventQualifier.Get_Ivr_List:
                                    this.ResponseEvent("get_ivr_list", agent_event.agentID, agent_event.retCode, agent_event.reason);
                                    break;
                                case EventQualifier.Get_Queue_List:
                                    this.ResponseEvent("get_queue_list", agent_event.agentID, agent_event.retCode, agent_event.reason);
                                    break;
                                case EventQualifier.Manual_Callout:
                                    this.ResponseEvent("manual_callout", agent_event.agentID, agent_event.retCode, agent_event.reason);
                                    break;
                                case EventQualifier.Get_Agent_Group_List:
                                    this.ResponseEvent("get_agent_group_list", agent_event.agentID, agent_event.retCode, agent_event.reason);
                                    break;
                                case EventQualifier.Get_Agents_Of_Queue:
                                    this.ResponseEvent("get_agents_of_queue", agent_event.agentID, agent_event.retCode, agent_event.reason);
                                    break;
                                case EventQualifier.Get_Agents_Of_Agent_Group:
                                    this.ResponseEvent("get_agents_of_agent_group", agent_event.agentID, agent_event.retCode, agent_event.reason);
                                    break;
                                case EventQualifier.Get_Agents_Monitor_Info:
                                    this.ResponseEvent("get_agents_monitor_info", agent_event.agentID, agent_event.retCode, agent_event.reason);
                                    break;
                                case EventQualifier.Get_Detail_Call_Info:
                                    this.ResponseEvent("get_detail_call_info", agent_event.agentID, agent_event.retCode, agent_event.reason);
                                    break;
                                case EventQualifier.Get_Customer_Of_Queue:
                                    this.ResponseEvent("get_customer_of_queue", agent_event.agentID, agent_event.retCode, agent_event.reason);
                                    break;
                                case EventQualifier.Get_Ivr_Profile_List:
                                    this.ResponseEvent("get_ivr_profile_list", agent_event.agentID, agent_event.retCode, agent_event.reason);
                                    break;
                                case EventQualifier.Transfer_IVR_Profile_NormalCall:
                                    this.ResponseEvent("transfer_ivr_profile", agent_event.agentID, agent_event.retCode, agent_event.reason);
                                    break;
                                case EventQualifier.Force_Change_Status_Result:
                                    this.OnEvent_Force_Change_Status(agent_event.agentID, agent_event.reason, agent_event.retCode);
                                    break;
                                case EventQualifier.Get_Queue_Statis:
                                    this.ResponseEvent("get_queue_statis", agent_event.agentID, agent_event.retCode, agent_event.reason);
                                    break;
                                case EventQualifier.Get_Customer_Of_My_Queue:
                                    this.ResponseEvent("get_customer_of_my_queue", agent_event.agentID, agent_event.retCode, agent_event.reason);
                                    break;
                                case EventQualifier.Get_Report_Statis_Info:
                                    this.ResponseEvent("get_report_statis_info", agent_event.agentID, agent_event.retCode, agent_event.reason);
                                    break;
                                case EventQualifier.Apply_Change_Status:
                                    this.ResponseEvent("apply_change_status", agent_event.agentID, agent_event.retCode, agent_event.reason);
                                    break;
                                case EventQualifier.Approve_Change_Status_Result:
                                    this.ResponseEvent("approve_change_status_result", agent_event.agentID, agent_event.retCode, agent_event.reason);
                                    break;
                                default:
                                    switch (eEventQualifier)
                                    {
                                        case EventQualifier.SignIn_Status:
                                            if (agent_event.retCode != 0)
                                            {
                                                AgentBar.Log.Debug("签入响应失败！正在断开连接........");
                                                bool blnRt = this.DoDisconnect();
                                                if (blnRt)
                                                {
                                                    AgentBar.Log.Debug("执行DoDisconnect()成功！");
                                                }
                                                else
                                                {
                                                    AgentBar.Log.Debug("执行DoDisconnect()失败！");
                                                }
                                            }
                                            this.ResponseEvent("signin", agent_event.agentID, agent_event.retCode, agent_event.reason);
                                            break;
                                        case EventQualifier.SignOut_Status:
                                            this.ResponseEvent("signout", agent_event.agentID, agent_event.retCode, agent_event.reason);
                                            break;
                                    }
                                    break;
                            }
                        }
                        else if (eEventQualifier != EventQualifier.Socket_HeartBeat)
                        {
                            if (eEventQualifier != EventQualifier.Sys_AgentStatusChange)
                            {
                                switch (eEventQualifier)
                                {
                                }
                            }
                            else
                            {
                                this.ResponseEvent("sys_agent_status_change", agent_event.agentID, agent_event.retCode, agent_event.reason);
                            }
                        }
                        else
                        {
                            this.ResponseEvent("heartbeat", agent_event.agentID, agent_event.retCode, agent_event.reason);
                        }
                        break;
                    }
                case AgentEventType.AGENT_EVENT_EVENT:
                    {
                        if (agent_event.cuID != "" && agent_event.cuID != null)
                        {
                            this.mCuID = agent_event.cuID;
                        }
                        EventQualifier eEventQualifier = agent_event.eEventQualifier;
                        switch (eEventQualifier)
                        {
                            case EventQualifier.Answered_NormalCall:
                                this.OnEvent_Answer(agent_event.agent_call_uuid, agent_event.relation_uuid);
                                break;
                            case EventQualifier.CallOut_NormalCall:
                                this.EventResultEvent("callout", agent_event.agentID, agent_event.retCode, agent_event.reason, agent_event.hangupReason);
                                break;
                            case EventQualifier.CallIn_NormalCall:
                                this.OnEvent_CommonCallIn(agent_event.agentID, agent_event.agent_call_uuid, agent_event.callerID, agent_event.calledID, agent_event.access_num_name, agent_event.makeStr, agent_event.call_type, agent_event.relation_uuid, agent_event.area_id, agent_event.area_name, agent_event.cust_grade, agent_event.outExtraParamsFromIvr, agent_event.todayDate, agent_event.customer_num_format_local, agent_event.customer_num_format_national, agent_event.customer_num_format_e164, agent_event.customer_num_phone_type, agent_event.predictCustomerForeignId, agent_event.predictCustomerName, agent_event.predictCustomerRemark);
                                break;
                            case EventQualifier.HangUp_NormalCall:
                                this.OnEvent_Hangup(agent_event.HangupActiveFlag, agent_event.no_answered_alram_flag, agent_event.hangupReason, agent_event.retCode, agent_event.isEvaluated, agent_event.evaluateStatus, agent_event.evaluate_default_result, agent_event.agent_call_uuid, agent_event.relation_uuid);
                                break;
                            case EventQualifier.Disconnect_NormalCall:
                            case EventQualifier.StopListen_NormalCall:
                            case EventQualifier.Internal_Call:
                            case (EventQualifier)157:
                            case EventQualifier.Force_Change_Status_Result:
                            case EventQualifier.Approve_Change_Status_Result:
                            case (EventQualifier)189:
                            case (EventQualifier)190:
                            case (EventQualifier)191:
                            case (EventQualifier)192:
                            case (EventQualifier)193:
                            case (EventQualifier)194:
                            case (EventQualifier)195:
                            case (EventQualifier)196:
                            case (EventQualifier)197:
                            case (EventQualifier)198:
                            case (EventQualifier)199:
                            case EventQualifier.Online_Status:
                            case EventQualifier.Login_Status:
                            case EventQualifier.LogOut_Status:
                                break;
                            case EventQualifier.Bridge_NormalCall:
                                this.OnEvent_Bridge(agent_event.bridge_uuidA, agent_event.bridge_uuidB);
                                break;
                            case EventQualifier.SignIn_Ring_NormalCall:
                                this.SignInRingEvent();
                                break;
                            case EventQualifier.SignIn_Play_NormalCall:
                                this.SignInPlayEvent();
                                break;
                            case EventQualifier.CallOut_Ring:
                                this.OnEvent_CallOutRing(agent_event.agent_call_uuid, agent_event.callerID, agent_event.calledID, agent_event.access_num_name, agent_event.call_type, agent_event.area_id, agent_event.area_name, agent_event.customer_num_format_local, agent_event.customer_num_format_national, agent_event.customer_num_format_e164, agent_event.customer_num_phone_type, agent_event.customer_foreign_id, agent_event.relation_uuid);
                                break;
                            case EventQualifier.Check_ExtenStatus:
                                this.OnEvent_CheckExten(agent_event.agentID, agent_event.agent_call_uuid, agent_event.relation_uuid);
                                break;
                            case EventQualifier.Hold_NormalCall:
                                this.OnEvent_hold_Result(agent_event.agentID, agent_event.retCode, agent_event.reason);
                                break;
                            case EventQualifier.StopHold_NormalCall:
                                this.OnEvent_Unhold_Result(agent_event.agentID, agent_event.retCode, agent_event.reason);
                                break;
                            case EventQualifier.Mute_NormalCall:
                                this.OnEvent_Mute_Result(agent_event.agentID, agent_event.retCode, agent_event.reason);
                                break;
                            case EventQualifier.StopMute_NormalCall:
                                this.OnEvent_Unmute_Result(agent_event.agentID, agent_event.retCode, agent_event.reason);
                                break;
                            case EventQualifier.Consult_Call_In:
                                this.OnEvent_Consult_Callin(agent_event.agentID, agent_event.agent_call_uuid, agent_event.callerID, agent_event.calledID, agent_event.access_num_name, agent_event.consulterAgentNum, agent_event.call_type, agent_event.area_id, agent_event.area_name, agent_event.cust_grade, agent_event.outExtraParamsFromIvr, agent_event.todayDate, agent_event.customer_num_format_local, agent_event.customer_num_format_national, agent_event.customer_num_format_e164, agent_event.customer_num_phone_type, agent_event.customer_foreign_id, agent_event.predictCustomerForeignId, agent_event.predictCustomerName, agent_event.predictCustomerRemark, agent_event.relation_uuid);
                                break;
                            case EventQualifier.Consult_Cancel_Result:
                                this.OnEvent_Consult_Cancel(agent_event.agentID, agent_event.retCode, agent_event.reason);
                                break;
                            case EventQualifier.Eavesdrop_Result:
                                this.OnEvent_Eavesdrop(agent_event.agentID, agent_event.reason, agent_event.retCode);
                                break;
                            case EventQualifier.Transfer_Blind_Agent_NormalCall:
                                this.OnEvent_Transfer_Agent(agent_event.agentID, agent_event.retCode, agent_event.reason);
                                break;
                            case EventQualifier.InterceptCall_NormalCall:
                                this.EventResultEvent("intercept", agent_event.agentID, agent_event.retCode, agent_event.reason, agent_event.hangupReason);
                                break;
                            case EventQualifier.Interrupt_NormalCall:
                                this.EventResultEvent("interrupt", agent_event.agentID, agent_event.retCode, agent_event.reason, agent_event.hangupReason);
                                break;
                            case EventQualifier.ForceDisconnect_NormalCall:
                                this.EventResultEvent("forcedisconnect", agent_event.agentID, agent_event.retCode, agent_event.reason, agent_event.hangupReason);
                                break;
                            case EventQualifier.Internal_Caller_Ring:
                                this.OnEvent_Internal_Caller_Ring(agent_event.agentID, agent_event.agent_call_uuid, agent_event.calledAgentNum, agent_event.call_type, agent_event.relation_uuid);
                                break;
                            case EventQualifier.Internal_Call_CallIn:
                                this.OnEvent_Internal_Call_CallIn(agent_event.agentID, agent_event.agent_call_uuid, agent_event.callerAgentNum, agent_event.call_type, agent_event.relation_uuid);
                                break;
                            case EventQualifier.Grade_NormalCall:
                                this.EventResultEvent("grade", agent_event.agentID, agent_event.retCode, agent_event.reason, agent_event.hangupReason);
                                break;
                            case EventQualifier.Predict_CallOut_Bridge_Ring:
                                this.OnEvent_PredictCallOutBridgeRing(agent_event.callerID, agent_event.calledID, agent_event.access_num_name, agent_event.makeStr, agent_event.call_type, agent_event.area_id, agent_event.area_name, agent_event.outExtraParamsFromIvr, agent_event.todayDate, agent_event.customer_num_format_local, agent_event.customer_num_format_national, agent_event.customer_num_format_e164, agent_event.customer_num_phone_type);
                                break;
                            case EventQualifier.Get_Access_Numbers:
                                this.OnEvent_Get_Access_Number(agent_event.agentID, agent_event.reason, agent_event.retCode, agent_event.accessNumbers);
                                break;
                            case EventQualifier.Three_Way_Call_Result:
                                this.OnEvent_Three_Way_Call(agent_event.agentID, agent_event.reason, agent_event.retCode);
                                break;
                            case EventQualifier.Three_Way_Call_In:
                                this.OnEvent_Three_Way_Call_In(agent_event.agentID, agent_event.agent_call_uuid, agent_event.callerID, agent_event.calledID, agent_event.access_num_name, agent_event.callerAgentNum, agent_event.call_type, agent_event.area_id, agent_event.area_name, agent_event.cust_grade, agent_event.outExtraParamsFromIvr, agent_event.todayDate, agent_event.customer_num_format_local, agent_event.customer_num_format_national, agent_event.customer_num_format_e164, agent_event.customer_num_phone_type, agent_event.customer_foreign_id, agent_event.predictCustomerForeignId, agent_event.predictCustomerName, agent_event.predictCustomerRemark, agent_event.relation_uuid);
                                break;
                            case EventQualifier.Three_Way_Call_Cancel_Result:
                                this.OnEvent_Three_Way_Cancel(agent_event.agentID, agent_event.reason, agent_event.retCode);
                                break;
                            case EventQualifier.Consult_Call_Result:
                                this.OnEvent_Consult(agent_event.agentID, agent_event.retCode, agent_event.reason);
                                break;
                            case EventQualifier.Consult_Transfer_Result:
                                this.OnEvent_Consult_Transfer(agent_event.agentID, agent_event.retCode, agent_event.reason);
                                break;
                            case EventQualifier.Whisper_Result:
                                this.OnEvent_Whisper(agent_event.agentID, agent_event.reason, agent_event.retCode);
                                break;
                            case EventQualifier.Bargein_Result:
                                this.OnEvent_Bargein(agent_event.agentID, agent_event.reason, agent_event.retCode);
                                break;
                            case EventQualifier.Force_Hangup_Result:
                                this.OnEvent_ForceHangup(agent_event.agentID, agent_event.reason, agent_event.retCode);
                                break;
                            case EventQualifier.Eavesdrop_Ring_Myself:
                                this.OnEvent_Eavesdrop_Ring(agent_event.agentID, agent_event.agent_call_uuid, agent_event.callerID, agent_event.calledID, agent_event.access_num_name, agent_event.destAgentID, agent_event.call_type, agent_event.area_id, agent_event.area_name, agent_event.cust_grade, agent_event.outExtraParamsFromIvr, agent_event.todayDate, agent_event.customer_num_format_local, agent_event.customer_num_format_national, agent_event.customer_num_format_e164, agent_event.customer_num_phone_type, agent_event.customer_foreign_id, agent_event.relation_uuid);
                                break;
                            case EventQualifier.Whisper_Ring_Myself:
                                this.OnEvent_Whisper_Ring(agent_event.agentID, agent_event.agent_call_uuid, agent_event.callerID, agent_event.calledID, agent_event.access_num_name, agent_event.destAgentID, agent_event.call_type, agent_event.area_id, agent_event.area_name, agent_event.cust_grade, agent_event.outExtraParamsFromIvr, agent_event.todayDate, agent_event.customer_num_format_local, agent_event.customer_num_format_national, agent_event.customer_num_format_e164, agent_event.customer_num_phone_type, agent_event.customer_foreign_id, agent_event.relation_uuid);
                                break;
                            case EventQualifier.Bargein_Ring_Myself:
                                this.OnEvent_Bargein_Ring(agent_event.agentID, agent_event.agent_call_uuid, agent_event.callerID, agent_event.calledID, agent_event.access_num_name, agent_event.destAgentID, agent_event.call_type, agent_event.area_id, agent_event.area_name, agent_event.cust_grade, agent_event.outExtraParamsFromIvr, agent_event.todayDate, agent_event.customer_num_format_local, agent_event.customer_num_format_national, agent_event.customer_num_format_e164, agent_event.customer_num_phone_type, agent_event.customer_foreign_id, agent_event.relation_uuid);
                                break;
                            case EventQualifier.Force_Hangup_Ring_Myself:
                                this.OnEvent_Force_Hangup_Ring(agent_event.agentID, agent_event.agent_call_uuid, agent_event.callerID, agent_event.calledID, agent_event.access_num_name, agent_event.destAgentID, agent_event.call_type, agent_event.area_id, agent_event.area_name, agent_event.cust_grade, agent_event.outExtraParamsFromIvr, agent_event.todayDate, agent_event.customer_num_format_local, agent_event.customer_num_format_national, agent_event.customer_num_format_e164, agent_event.customer_num_phone_type, agent_event.customer_foreign_id, agent_event.relation_uuid);
                                break;
                            case EventQualifier.Transfer_Queue_NormalCall:
                                this.OnEvent_Transfer_Queue(agent_event.agentID, agent_event.retCode, agent_event.reason);
                                break;
                            case EventQualifier.Transfer_IVR_NormalCall:
                                this.OnEvent_Transfer_Ivr(agent_event.agentID, agent_event.retCode, agent_event.reason);
                                break;
                            case EventQualifier.Transfer_Blind_Call_In:
                                this.OnEvent_Transfer_Blind_Call_In(agent_event.agentID, agent_event.agent_call_uuid, agent_event.callerID, agent_event.calledID, agent_event.access_num_name, agent_event.callerAgentNum, agent_event.call_type, agent_event.area_id, agent_event.area_name, agent_event.cust_grade, agent_event.outExtraParamsFromIvr, agent_event.todayDate, agent_event.customer_num_format_local, agent_event.customer_num_format_national, agent_event.customer_num_format_e164, agent_event.customer_num_phone_type, agent_event.customer_foreign_id, agent_event.predictCustomerForeignId, agent_event.predictCustomerName, agent_event.predictCustomerRemark, agent_event.relation_uuid);
                                break;
                            case EventQualifier.Get_Online_Agents:
                                this.OnEvent_Get_Online_Agent(agent_event.agentID, agent_event.agent_online, agent_event.reason, agent_event.retCode);
                                break;
                            case EventQualifier.Get_Ivr_List:
                                this.OnEvent_Get_Ivr_List(agent_event.agentID, agent_event.ivr_list, agent_event.reason, agent_event.retCode);
                                break;
                            case EventQualifier.Get_Queue_List:
                                this.OnEvent_Get_Queue_List(agent_event.agentID, agent_event.queue_list, agent_event.reason, agent_event.retCode);
                                break;
                            case EventQualifier.Manual_Callout:
                                this.OnEvent_Manual_Callout(agent_event.agentID, agent_event.reason, agent_event.retCode);
                                break;
                            case EventQualifier.Get_Defined_Role_Rights:
                                this.OnEvent_Get_Defined_Role_Rights(agent_event.agentID, agent_event.agent_role_and_right, agent_event.reason, agent_event.retCode);
                                break;
                            case EventQualifier.Get_Agent_Group_List:
                                this.OnEvent_Agent_Group_List(agent_event.agentID, agent_event.group_list, agent_event.reason, agent_event.retCode);
                                break;
                            case EventQualifier.Get_Agents_Of_Queue:
                                this.OnEvent_Get_Agents_Of_Queue(agent_event.agentID, agent_event.agent_list, agent_event.queue_num, agent_event.reason, agent_event.retCode);
                                break;
                            case EventQualifier.Get_Agents_Of_Agent_Group:
                                this.OnEvent_Get_Agents_Of_AgentGroup(agent_event.agentID, agent_event.agent_list, agent_event.agent_group_num, agent_event.reason, agent_event.retCode);
                                break;
                            case EventQualifier.Get_Agents_Monitor_Info:
                                this.OnEvent_Get_Agents_Monitor_info(agent_event.agentID, agent_event.agent_online, agent_event.current_time, agent_event.reason, agent_event.retCode);
                                break;
                            case EventQualifier.Get_Detail_Call_Info:
                                this.OnEvent_Get_Detail_Call_info(agent_event.agentID, agent_event.destAgentID, agent_event.call_type, agent_event.leg_info_list, agent_event.relation_info_list, agent_event.reason, agent_event.retCode);
                                break;
                            case EventQualifier.Get_Customer_Of_Queue:
                                this.OnEvent_Get_Customer_Of_Queue(agent_event.agentID, agent_event.queueNumLstStr, agent_event.current_time, agent_event.customer_info_list, agent_event.reason, agent_event.retCode);
                                break;
                            case EventQualifier.Add_Customer_To_Queue:
                                this.OnEvent_Add_Customer_To_Queue(agent_event.agentID, agent_event.call_type, agent_event.calledID, agent_event.callerID, agent_event.customer_status, agent_event.customer_type, agent_event.customer_uuid, agent_event.enter_queue_time, agent_event.exclusive_agent_num, agent_event.exclusive_queue_num, agent_event.queue_num, agent_event.queue_name, agent_event.queue_customer_amount, agent_event.early_queue_enter_time, agent_event.early_queue_enter_time_all, agent_event.customer_enter_channel);
                                break;
                            case EventQualifier.Del_Customer_From_Queue:
                                this.OnEvent_Del_Customer_From_Queue(agent_event.agentID, agent_event.customer_uuid, agent_event.queue_num, agent_event.current_time, agent_event.early_queue_enter_time, agent_event.early_queue_enter_time_all, agent_event.reason);
                                break;
                            case EventQualifier.Update_Customer_Of_Queue:
                                this.OnEvent_Update_Customer_Of_Queue(agent_event.agentID, agent_event.call_type, agent_event.calledID, agent_event.callerID, agent_event.customer_status, agent_event.customer_type, agent_event.customer_uuid, agent_event.enter_queue_time, agent_event.exclusive_agent_num, agent_event.exclusive_queue_num, agent_event.queue_num);
                                break;
                            case EventQualifier.Consultee_Hangup_Call:
                                this.OnEvent_Consultee_Hangup(agent_event.agentID, agent_event.hangupReason);
                                break;
                            case EventQualifier.Threewayee_Hangup_Call:
                                this.OnEvent_Threewayee_Hangup(agent_event.agentID, agent_event.hangupReason);
                                break;
                            case EventQualifier.Get_Website_Info:
                                this.OnEvent_GetWebSiteInfo(agent_event.agentID, agent_event.retCode, agent_event.reason, agent_event.agentWebSiteInfo);
                                break;
                            case EventQualifier.Get_Ivr_Profile_List:
                                this.OnEvent_Get_Ivr_Profile_List(agent_event.agentID, agent_event.ivr_profile_list, agent_event.reason, agent_event.retCode);
                                break;
                            case EventQualifier.Transfer_IVR_Profile_NormalCall:
                                this.OnEvent_Transfer_Ivr_Profile(agent_event.agentID, agent_event.retCode, agent_event.reason);
                                break;
                            case EventQualifier.Get_Queue_Statis:
                                this.OnEvent_Get_Queue_Statis_Info(agent_event.agentID, agent_event.current_time, agent_event.queueNumLstStr, agent_event.queue_statis_list, agent_event.reason, agent_event.retCode);
                                break;
                            case EventQualifier.Get_Customer_Of_My_Queue:
                                this.OnEvent_Get_Customer_Of_My_Queue(agent_event.agentID, agent_event.current_time, agent_event.queueNumLstStr, agent_event.customer_info_list, agent_event.reason, agent_event.retCode);
                                break;
                            case EventQualifier.Callee_Answered:
                                this.OnEvent_Callee_Answer(agent_event.agent_call_uuid, agent_event.relation_uuid);
                                break;
                            case EventQualifier.Get_Agent_Personal_info:
                                this.OnEvent_Get_Agent_Personal_info(agent_event.agentID, agent_event.agent_mobile, agent_event.agent_email);
                                break;
                            case EventQualifier.Set_Agent_Personal_info:
                                this.OnEvent_Set_Agent_Personal_info(agent_event.agentID, agent_event.retCode, agent_event.reason);
                                break;
                            case EventQualifier.Change_Pswd:
                                this.OnEvent_Change_Pswd(agent_event.agentID, agent_event.retCode, agent_event.reason);
                                break;
                            case EventQualifier.Get_Report_Statis_Info:
                                this.OnEvent_Get_Report_Statis_Info(agent_event.agentID, agent_event.retCode, agent_event.reason, agent_event.report_statis_info_map);
                                break;
                            case EventQualifier.Get_All_Queue_Statis:
                                this.OnEvent_Get_All_Queue_Statis_Info(agent_event.agentID, agent_event.current_time, agent_event.queue_statis_list, agent_event.reason, agent_event.retCode);
                                break;
                            case EventQualifier.Apply_Change_Status:
                                this.OnEvent_Apply_Change_Status(agent_event.agentID, agent_event.retCode, agent_event.reason);
                                break;
                            case EventQualifier.Apply_Change_Status_Distribute:
                                this.OnEvent_Apply_Change_Status_Distribute(agent_event.agentID, agent_event.apply_agent_id, agent_event.targetStatus, agent_event.apply_agent_name, agent_event.agentGroupID, agent_event.agent_group_name, agent_event.applyTime, agent_event.applyType, agent_event.retCode, agent_event.reason);
                                break;
                            case EventQualifier.Approve_Change_Status_Distribute:
                                this.OnEvent_Approve_Change_Status_Distribute(agent_event.agentID, agent_event.apply_agent_id, agent_event.agentGroupID, agent_event.targetStatus, agent_event.approveResult, agent_event.approveTime, agent_event.retCode, agent_event.reason);
                                break;
                            case EventQualifier.Get_Agentgroup_Status_Max_Num:
                                this.OnEvent_Get_Agentgroup_Status_Max_Num(agent_event.agentID, agent_event.agentGroupNameLstStr, agent_event.status_max_num_list);
                                break;
                            case EventQualifier.Set_Agentgroup_Status_Max_Num:
                                this.OnEvent_Set_Agentgroup_Status_Max_Num(agent_event.agentID, agent_event.agent_group_name, agent_event.targetStatus, agent_event.retCode, agent_event.reason);
                                break;
                            case EventQualifier.Apply_or_Approve_Change_Status_Timeout_Distribute:
                                this.OnEvent_Apply_or_Approve_Change_Status_Timeout_Distribute(agent_event.agentID, agent_event.apply_agent_id, agent_event.agentGroupID, agent_event.targetStatus, agent_event.timeoutType);
                                break;
                            case EventQualifier.Apply_Change_Status_Cancel:
                                this.OnEvent_Apply_Change_Status_Cancel(agent_event.agentID, agent_event.targetStatus, agent_event.retCode, agent_event.reason);
                                break;
                            case EventQualifier.Apply_Change_Status_Cancel_Distribute:
                                this.OnEvent_Apply_Change_Status_Cancel_Distribute(agent_event.agentID, agent_event.apply_agent_id, agent_event.targetStatus, agent_event.retCode, agent_event.reason);
                                break;
                            case EventQualifier.Get_Change_Status_Apply_List:
                                this.OnEvent_Get_Change_Status_Apply_List(agent_event.agentID, agent_event.apply_change_status_list, agent_event.retCode, agent_event.reason);
                                break;
                            case EventQualifier.HangUp_NormalCallResult:
                                this.OnEvent_HangupResult(agent_event.agentID, agent_event.reason, agent_event.retCode);
                                break;
                            case EventQualifier.Record_Start:
                                this.OnEvent_RecordStart(agent_event.agentExten, agent_event.agentID, agent_event.agent_call_uuid, agent_event.calleeNum, agent_event.callerNum, agent_event.filePath, agent_event.relation_uuid);
                                break;
                            case EventQualifier.Record_Stop:
                                this.OnEvent_RecordStop(agent_event.agentID, agent_event.agent_call_uuid, agent_event.filePath, agent_event.relation_uuid);
                                break;
                            case EventQualifier.Evaluate_Result_Event:
                                this.OnEvent_Evaluate_Result(agent_event.agentID, agent_event.agent_call_uuid, agent_event.agentExten, agent_event.customerUuid, agent_event.agent_group_name, agent_event.callerNum, agent_event.evaluateScore, agent_event.evaluateStatus, agent_event.queue_num);
                                break;
                            case EventQualifier.Offline_Status:
                                this.EventResultEvent("offline", agent_event.agentID, agent_event.retCode, agent_event.reason, agent_event.hangupReason);
                                break;
                            case EventQualifier.SignIn_Status:
                                this.OnEvent_SignIn(agent_event.agentID, agent_event.agentName, agent_event.agentExten, agent_event.agentGroupID, agent_event.agent_group_name, agent_event.autoPostTreatment, agent_event.bindExten, agent_event.gradeSwitch, agent_event.initStatus, agent_event.roleID, agent_event.retCode, agent_event.reason, agent_event.heartbeat_timeout, agent_event.salt_key, agent_event.DID_Num);
                                break;
                            case EventQualifier.SignOut_Status:
                                this.OnEvent_SignOut(agent_event.agentID, agent_event.retCode, agent_event.reason);
                                break;
                            case EventQualifier.Warn_Agent_Resignin:
                                this.OnEvent_Warn_Agent_Resignin(agent_event.agentID);
                                break;
                            case EventQualifier.Warn_Agent_Force_Change_Status:
                                this.OnEvent_Warn_Agent_Force_Change_Status(agent_event.agentID, agent_event.executorAgentID);
                                break;
                            case EventQualifier.Blind_Transfer_Outbound_Failed:
                                this.OnEvent_Blind_Transfer_Outbound_Failed(agent_event.agentID, agent_event.hangupReason);
                                break;
                            case EventQualifier.Queue_Transfer_Outbound:
                                this.OnEvent_Queue_Transfer_Outbound(agent_event.agentID, agent_event.callerID, agent_event.queue_num, agent_event.transfee_num, agent_event.access_num, agent_event.transfer_time);
                                break;
                            default:
                                if (eEventQualifier != EventQualifier.Socket_Disconnect)
                                {
                                    switch (eEventQualifier)
                                    {
                                        case EventQualifier.Sys_KickOut:
                                            this.OnEvent_KickOut(agent_event.agentID, agent_event.retCode, agent_event.reason);
                                            break;
                                        case EventQualifier.Sys_GetAgentDefineStatus:
                                            this.OnEvent_GetDefineStatus(agent_event.agentID, agent_event.retCode, agent_event.reason, agent_event.agentDefineStatus);
                                            break;
                                        case EventQualifier.Sys_AgentStatusChange:
                                            this.OnEvent_AgentStatusChange(agent_event.agentID, agent_event.status_change_agentid, agent_event.status_before, agent_event.status_after, agent_event.bindExten, agent_event.customer_enter_channel, agent_event.current_time, agent_event.start_talking_time);
                                            break;
                                        case EventQualifier.Sys_ThrowException:
                                            this.OnEvent_ThrowException(agent_event.exception_reason);
                                            break;
                                    }
                                }
                                else
                                {
                                    this.SockDisconnectEvent(agent_event.reason, agent_event.retCode);
                                }
                                break;
                        }
                        break;
                    }
                case AgentEventType.AGENT_EVENT_MYDEFINE:
                    {
                        EventQualifier eEventQualifier = agent_event.eEventQualifier;
                        if (eEventQualifier != EventQualifier.Socket_Disconnect)
                        {
                            if (eEventQualifier != EventQualifier.Sys_ResponseTimeOut)
                            {
                                if (eEventQualifier == EventQualifier.Sys_ThrowException)
                                {
                                    if (this.SysThrowExceptionEvent != null)
                                    {
                                        this.SysThrowExceptionEvent(agent_event.reason, agent_event.retCode);
                                    }
                                }
                            }
                            else if (this.ResponseTimeOutEvent != null)
                            {
                                this.ResponseTimeOutEvent(agent_event.reason, agent_event.retCode);
                            }
                        }
                        else if (this.SockDisconnectEvent != null)
                        {
                            this.SockDisconnectEvent(agent_event.reason, agent_event.retCode);
                        }
                        break;
                    }
            }
        }

        private void RaiseHeartBeatEvents(AgentEvent agent_event)
        {
            AgentBar.Log.Debug(string.Concat(new object[]
            {
                "enter RaiseHeartBeatEvents.eventType=",
                agent_event.deAgentEventType,
                ",qualifier=",
                agent_event.eEventQualifier
            }));
            if (agent_event.deAgentEventType == AgentEventType.AGENT_EVENT_RESPONSE)
            {
                if (agent_event.eEventQualifier == EventQualifier.Socket_HeartBeat)
                {
                    this.ResponseEvent("heartbeat", agent_event.agentID, agent_event.retCode, agent_event.reason);
                }
            }
        }


        private void BeginTheTimer()
        {
            AgentBar.Log.Debug("enter BeginTheTimer .");
            object myobject = 7;
            this.HeartBeatTimes = 0;
            this.tmrHeartBeat = new System.Threading.Timer(new TimerCallback(this.testTheNet), myobject, this.mHeartBeatTimeout * 1000, this.mHeartBeatTimeout * 1000);
        }

        private void testTheNet(object myobject)
        {
            AgentBar.Log.Debug("enter testTheNet .");
            try
            {
                if (!this.blnConnect)
                {
                    this.tmrHeartBeat.Dispose();
                }
                else
                {
                    Thread sendMyPulseThPro = new Thread(new ThreadStart(this.delegateSendMyPulse));
                    sendMyPulseThPro.Start();
                }
            }
            catch (Exception e)
            {
                AgentBar.Log.Error(string.Concat(new string[]
                {
                    e.Source,
                    ",信息:",
                    e.Message,
                    ",堆栈:",
                    e.StackTrace
                }));
            }
        }

        private void delegateSendMyPulse()
        {
            AgentBar.Log.Debug("enter delegateSendMyPulse .");
            try
            {
                this.HeartBeatTimes++;
                if (this.HeartBeatTimes >= 3)
                {
                    AgentBar.Log.Debug("心跳超时！开始断开连接......");
                    this.DoDisconnect();
                    this.SockDisconnectEvent("heartbeat timeout!", -1001);
                    AgentBar.Log.Debug("心跳检测超时，断开连接！");
                    this.tmrHeartBeat.Dispose();
                }
                else if (!this.DoHeartBeat())
                {
                    AgentBar.Log.Error("DoHeartBeat 调用失败");
                }
            }
            catch (Exception e)
            {
                AgentBar.Log.Error(string.Concat(new string[]
                {
                    e.Source,
                    ",信息:",
                    e.Message,
                    ",堆栈:",
                    e.StackTrace
                }));
            }
        }

        public int GetInterfaceSafetyOptions(ref Guid riid, ref int pdwSupportedOptions, ref int pdwEnabledOptions)
        {
            string strGUID = riid.ToString("B");
            pdwSupportedOptions = 3;
            string text = strGUID;
            int Rslt;
            if (text != null)
            {
                if (text == "{00020400-0000-0000-C000-000000000046}" || text == "{a6ef9860-c720-11d0-9337-00a0c90dcaa9}")
                {
                    Rslt = 0;
                    pdwSupportedOptions = 0;
                    if (this._fSafeForScripting)
                    {
                        pdwEnabledOptions = 1;
                    }
                    return Rslt;
                }
                if (text == "{0000010A-0000-0000-C000-000000000046}" || text == "{00000109-0000-0000-C000-000000000046}" || text == "{37D84F60-42CB-11CE-8135-00AA004BB851}")
                {
                    Rslt = 0;
                    pdwEnabledOptions = 0;
                    if (this._fSafeForInitializing)
                    {
                        pdwEnabledOptions = 2;
                    }
                    return Rslt;
                }
            }
            Rslt = -2147467262;
            return Rslt;
        }

        public int SetInterfaceSafetyOptions(ref Guid riid, int dwOptionSetMask, int dwEnabledOptions)
        {
            int Rslt = -2147467259;
            string strGUID = riid.ToString("B");
            string text = strGUID;
            if (text != null)
            {
                if (text == "{00020400-0000-0000-C000-000000000046}" || text == "{a6ef9860-c720-11d0-9337-00a0c90dcaa9}")
                {
                    if ((dwEnabledOptions & dwOptionSetMask) == 1 && this._fSafeForScripting)
                    {
                        Rslt = 0;
                    }
                    return Rslt;
                }
                if (text == "{0000010A-0000-0000-C000-000000000046}" || text == "{00000109-0000-0000-C000-000000000046}" || text == "{37D84F60-42CB-11CE-8135-00AA004BB851}")
                {
                    if ((dwEnabledOptions & dwOptionSetMask) == 2 && this._fSafeForInitializing)
                    {
                        Rslt = 0;
                    }
                    return Rslt;
                }
            }
            Rslt = -2147467262;
            return Rslt;
        }

        private void addToCallHistory(string tel)
        {
            if (this.mCalloutHistory != null)
            {
                if (this.mCalloutHistory.Contains(tel))
                {
                    int index = this.mCalloutHistory.IndexOf(tel);
                    this.mCalloutHistory.RemoveAt(index);
                }
                this.mCalloutHistory.Insert(0, tel);
                if (this.mCalloutHistory.Count > 10)
                {
                    this.mCalloutHistory.RemoveAt(10);
                }
            }
        }

        public static int PostMsgToSoftPhone(IntPtr _mSoftPhoneWindowHandle, 
            string softPhone_app_className, string softPhone_app_name, 
            int softPhone_msg_value, int softPhone_cmd, int softPhone_cmd2)
        {
            int result;
            if (softPhone_app_name != "")
            {
                if (_mSoftPhoneWindowHandle == IntPtr.Zero)
                {
                    _mSoftPhoneWindowHandle = GetProcessWindowHandle(softPhone_app_className, softPhone_app_name);
                }
                if (_mSoftPhoneWindowHandle != IntPtr.Zero)
                {
                    if (!PLAgent.PostMessageApi(_mSoftPhoneWindowHandle, softPhone_msg_value, softPhone_cmd, softPhone_cmd))
                    {
                        PLAgent.Log.Error(string.Concat(new object[]
                        {
                            "PostMessageApi failed!handle:",
                            _mSoftPhoneWindowHandle,
                            ",msg_value:",
                            softPhone_msg_value,
                            ",wp:",
                            softPhone_cmd,
                            ",lp:",
                            softPhone_cmd
                        }));
                        result = -1;
                    }
                    else
                    {
                        PLAgent.Log.Error(string.Concat(new object[]
                        {
                            "PostMessageApi success!handle:",
                            _mSoftPhoneWindowHandle,
                            ",msg_value:",
                            softPhone_msg_value,
                            ",wp:",
                            softPhone_cmd,
                            ",lp:",
                            softPhone_cmd
                        }));
                        result = 0;
                    }
                }
                else
                {
                    PLAgent.Log.Error("Answer failed!!reason:softphone handle is Not Exist!");
                    result = -2;
                }
            }
            else
            {
                PLAgent.Log.Error("Answer failed!!reason:softphone app name is empty!!");
                result = -3;
            }
            return result;
        }

        public static bool PostMessageApi(IntPtr hwnd, int msg_value, int wP, int lP)
        {
            bool result;
            try
            {
                if (hwnd == IntPtr.Zero || msg_value == 0 || wP == 0 || lP == 0)
                {
                    Log.Error("PostMessageApi is failed!!one of parameter is zero!");
                    result = false;
                }
                else
                {
                    PostMessage(hwnd, msg_value, (IntPtr)wP, (IntPtr)lP);
                    result = true;
                }
            }
            catch (Exception e)
            {
                Log.Error(string.Concat(new string[]
                {
                    "PostMessageApi is failed!!reason:",
                    e.Source,
                    ",信息:",
                    e.Message,
                    ",堆栈:",
                    e.StackTrace
                }));
                result = false;
            }
            return result;
        }

   

        public bool DoGetPersonalInfo(string strAgentNum)
        {
            bool result;
            if (string.IsNullOrEmpty(strAgentNum))
            {
                result = false;
            }
            else
            {
                long rt = this.agentDll.PL_GetPersonalInfo(strAgentNum);
                result = (0L == rt);
            }
            return result;
        }

        public bool DoSetPersonalInfo(string strAgentNum, string strMobile, string strEmail)
        {
            bool result;
            if (string.IsNullOrEmpty(strAgentNum) || string.IsNullOrEmpty(strMobile) || string.IsNullOrEmpty(strEmail))
            {
                result = false;
            }
            else
            {
                long rt = this.agentDll.PL_SetPersonalInfo(strAgentNum, strMobile, strEmail);
                result = (0L == rt);
            }
            return result;
        }

        public bool DoChangePswd(string strAgentNum, string strOldPswd, string strNewPswd)
        {
            bool result;
            if (string.IsNullOrEmpty(strAgentNum) || string.IsNullOrEmpty(strOldPswd) || string.IsNullOrEmpty(strNewPswd))
            {
                result = false;
            }
            else
            {
                long rt = this.agentDll.PL_ChangePswd(strAgentNum, strOldPswd, strNewPswd);
                result = (0L == rt);
            }
            return result;
        }

        public bool DoGetReportStatisInfo()
        {
            bool result;
            if (string.IsNullOrEmpty(this.mAgentID))
            {
                result = false;
            }
            else
            {
                long rt = this.agentDll.PL_GetReportStatisInfo(this.mAgentID);
                result = (0L == rt);
            }
            return result;
        }

        public void reCheckPrompt()
        {
            if (!this.NoAnswerCallNormal && this.other.DropDownItems.Contains(this.tsbNoAnswerCalls))
            {
                if (this.otherNormal)
                {
                    this.other.Image = Resources.other1;
                }
            }
            if (!this.ApproveNormal && this.other.DropDownItems.Contains(this.tsbApprove))
            {
                if (this.otherNormal)
                {
                    this.other.Image = Resources.other1;
                }
            }
        }

        public void DoShowKeyPad()
        {
            if (this.newKeyPad == null || this.newKeyPad.IsDisposed)
            {
                this.newKeyPad = new FrmKeyPad();
                this.newKeyPad.Show();
                this.newKeyPad.Activate();
                this.newKeyPad.agentBar1 = this;
            }
            else
            {
                this.newKeyPad.Show();
                this.newKeyPad.Activate();
            }
        }
    }
}
