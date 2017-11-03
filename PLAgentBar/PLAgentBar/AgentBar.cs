using log4net;
using log4net.Config;
using PLAgentBar.Properties;
using PLAgentDll;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Media;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using System.Xml;

namespace PLAgentBar
{
    [ClassInterface(ClassInterfaceType.AutoDispatch), ComSourceInterfaces(typeof(IActiveXEvents)), ComVisible(true), Guid("ED49E1D4-306F-4ec5-8A25-BCE8849BAF23"), ProgId("PLAgentBar.AgentBar")]
    public class AgentBar : UserControl, IObjectSafety, IDisposable
    {
        public delegate void AgentBarEventDelegate(string jsonArgsStr);

        private enum Call_Type
        {
            COMMON_CALL_IN,
            MANUAL_CALL_OUT,
            PREDICT_CALL_OUT,
            AGENT_INTERNAL_CALL,
            ECHO_TEST,
            CONSULT_CALL_IN,
            EAVESDROP_CALL_IN,
            WHISPER_CALL_IN,
            BARGEIN_CALL_IN,
            THREEWAY_CALL_IN,
            FORCE_HANGUP_CALL_IN
        }

        public enum Agent_Status
        {
            AGENT_STATUS_UNKNOWN = -2,
            AGENT_STATUS_OFFLINE,
            AGENT_STATUS_IDLE,
            AGENT_STATUS_RING,
            AGENT_STATUS_TALKING,
            AGENT_STATUS_HOLD,
            AGENT_STATUS_ACW,
            AGENT_STATUS_CAMP_ON,
            AGENT_STATUS_BUSY,
            AGENT_STATUS_LEAVE,
            AGENT_STATUS_CALL_OUT,
            AGENT_STATUS_MONITOR,
            AGENT_STATUS_CALLING_OUT,
            AGENT_STATUS_MUTE,
            AGENT_STATUS_RESTORE = 999
        }

        public enum Agent_State
        {
            AGENT_STATUS_UNKNOWN,
            AGENT_STATUS_SIGNIN,
            AGENT_STATUS_IDLE,
            AGENT_STATUS_OFFLINE,
            AGENT_STATUS_CONSULTING,
            AGENT_STATUS_THREEWAYTING,
            AGENT_STATUS_TALKING_CUSTOMER,
            AGENT_STATUS_TALKING_CONSULT,
            AGENT_STATUS_ANSWER_CONSULT,
            AGENT_STATUS_TALKING_THREEWAY,
            AGENT_STATUS_TALKING_INTERNAL,
            AGENT_STATUS_TALKING_EAVESDROP,
            AGENT_STATUS_TALKING_WHISPER,
            AGENT_STATUS_TALKING_BARGEIN,
            AGENT_STATUS_CAMP_ON,
            AGENT_STATUS_ANSWER_CALLEE,
            AGENT_STATUS_BRIDGE_CALLEE
        }

        public enum Event_Type
        {
            INITE_TOOLBAR,
            RESPONSE_TIMEOUT,
            KICK_OUT,
            EXTERNAL_CALL_OUT_SUCCESS,
            EXTERNAL_CALL_OUT_FAIL,
            INTERNAL_CALL_AGENT_SUCCESS,
            INTERNAL_CALL_AGENT_FAIL,
            ANSWER_CALL_PEER,
            ANSWER_CALL_CALLEE,
            BRIDGE_CALL_PEER,
            HANGUP_CALL_SUCCESS,
            HANGUP_CALL_FAIL,
            CALLIN_COMMON,
            CALLIN_PREDICT_CALL,
            CALLIN_INTERNAL,
            CALLIN_INTERNAL_MYSELF,
            CALLIN_THREE_WAY,
            CALLOUT_RING_MYSELF,
            MANUAL_CALLOUT_SUCCESS,
            MANUAL_CALLOUT_FAIL,
            DISCONNECT_SOCKET,
            SIGNIN_SUCCESS,
            SIGNIN_FAIL,
            SIGNOUT_SUCCESS,
            SIGNOUT_FAIL,
            GRADE_SUCCESS,
            GRADE_FAIL,
            HOLD_SUCCESS,
            HOLD_FAIL,
            UNHOLD_SUCCESS,
            UNHOLD_FAIL,
            MUTE_SUCCESS,
            MUTE_FAIL,
            UNMUTE_SUCCESS,
            UNMUTE_FAIL,
            TRANSFER_AGENT_SUCCESS,
            TRANSFER_AGENT_FAIL,
            TRANSFER_IVR_SUCCESS,
            TRANSFER_IVR_FAIL,
            TRANSFER_QUEUE_SUCCESS,
            TRANSFER_QUEUE_FAIL,
            TRANSFER_IVR_PROFILE_SUCCESS,
            TRANSFER_IVR_PROFILE_FAIL,
            TRANSFER_BLIND_CALL_IN,
            ECHO_TEST_SUCCESS,
            ECHO_TEST_FAIL,
            CONSULT_SUCCESS,
            CONSULT_FAIL,
            CONSULT_CANCEL_SUCCESS,
            CONSULT_CANCEL_FAIL,
            CONSULT_TRANSFER_SUCCESS,
            CONSULT_TRANSFER_FAIL,
            CONSULT_CALL_IN,
            CONSULTEE_HANGUP,
            THREE_WAY_SUCCESS,
            THREE_WAY_FAIL,
            THREE_WAY_CANCEL_SUCCESS,
            THREE_WAY_CANCEL_FAIL,
            THREEWAYEE_HANGUP,
            EAVESDROP_SUCCESS,
            EAVESDROP_FAIL,
            EAVESDROP_CANCEL_SUCCESS,
            EAVESDROP_CANCEL_FAIL,
            EAVESDROP_RING_MYSELF,
            WHISPER_RING_MYSELF,
            BARGEIN_RING_MYSELF,
            FORCE_HANGUP_RING_MYSELF,
            WHISPER_SUCCESS,
            WHISPER_FAIL,
            BARGE_IN_SUCCESS,
            BARGE_IN_FAIL,
            FORCE_HANGUP_SUCCESS,
            FORCE_HANGUP_FAIL,
            BLIND_TRANSFER_OUTBOUND_FAILED,
            GET_ACCESS_NUMBERS_SUCCESS,
            GET_ACCESS_NUMBERS_FAIL,
            GET_ONLINE_AGENT_FAIL,
            GET_ONLINE_AGENT_SUCCESS,
            GET_IVR_LIST_SUCCESS,
            GET_IVR_LIST_FAIL,
            GET_QUEUE_LIST_SUCCESS,
            GET_QUEUE_LIST_FAIL,
            GET_AGENT_GROUP_LIST_SUCCESS,
            GET_AGENT_GROUP_LIST_FAIL,
            AGENT_STATUS_CHANGE_TO_IDLE,
            AGENT_STATUS_CHANGE_TO_RING,
            AGENT_STATUS_CHANGE_TO_TALKING,
            AGENT_STATUS_CHANGE_TO_HOLD,
            AGENT_STATUS_CHANGE_TO_MUTE,
            AGENT_STATUS_CHANGE_TO_ACW,
            AGENT_STATUS_CHANGE_TO_CAMP_ON,
            AGENT_STATUS_CHANGE_TO_BUSY,
            AGENT_STATUS_CHANGE_TO_LEAVE,
            AGENT_STATUS_CHANGE_TO_MANUAL_CALL_OUT,
            AGENT_STATUS_CHANGE_TO_CALLING_OUT,
            SOFTPHONE_REGIST_RESULT,
            SOFTPHONE_CALL_IN,
            SOFTPHONE_HANGUP,
            SOFTPHONE_ANSWER,
            AGENT_STATUS_RESTORE,
            APPLY_CHANGE_STATUS_APPLY_SUCCESS,
            APPLY_CHANGE_STATUS_APPLY_FAILED,
            APPLY_CHANGE_STATUS_CANCEL_SUCCESS,
            APPLY_CHANGE_STATUS_CANCEL_FAILED,
            APPLY_CHANGE_STATUS_APPROVAL_PASS,
            APPLY_CHANGE_STATUS_NO_ANY_APPROVAL,
            APPLY_CHANGE_STATUS_SOME_APPLY,
            APPLY_CHANGE_STATUS_APPLY_OR_APPROVE_TIMEOUT,
            APPLY_CHANGE_STATUS_FINISHED
        }

        public enum Agent_Or_AgentGroup
        {
            AGENTNUM = 1,
            AGENTGROUPNUM,
            ALL,
            QUEUENUM
        }

        public enum CallStatus
        {
            NO_CALL,
            CALL_IN,
            CALL_OUT
        }

        public delegate void JsonEventHandler(string jsonArgsStr);

        public delegate void InternalCallerRingEventHandler(string agentID, string calledID, string callType, string makeStr);

        public delegate void AgentBarUIChangedEventHandler(AgentBar.Event_Type event_type, string statusInfo);

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

        public delegate void SendAgentStatusEventHandler(AgentBar.Agent_State agentState, string targetAgentNum);

        public delegate void ReceiveEventHandler(AgentEvent agent_event);

        public delegate void ReceiveHeartBeatEventHandler(AgentEvent agent_event);

        private delegate void DelegateUpdateUI(AgentBar.Event_Type eventType, string info);

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

        public struct ControlsVisible
        {
            public bool ListenVisible;

            public bool WhisperVisible;

            public bool BargeinVisible;

            public bool ForceHangupVisible;

            public bool CallOutVisible;

            public bool MonitorVisible;

            public bool ApproveVisible;
        }

        public struct ControlsInfo
        {
            public bool chkHoldInfo;

            public bool chkMuteInfo;

            public bool chkThreeWayInfo;

            public bool chkConsultInfo;

            public bool chkStopConsultInfo;

            public bool chkConsultTransferInfo;

            public bool chkTransferInfo;

            public bool chkGradeInfo;

            public bool chkCallAgentInfo;

            public bool chkCallOutInfo;

            public bool chkListenInfo;

            public bool chkWhisperInfo;

            public bool chkBargeinInfo;

            public bool chkForceHangupInfo;

            public bool chkCheckInfo;

            public bool chkMonitorInfo;

            public bool chkCancelApplicationInfo;

            public bool chkdbAfterHangupInfo;

            public bool chkApproveInfo;

            public bool chkNoAnswerCallsInfo;
        }

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

        private IContainer components = null;

        private ToolStripButton tsbHangUp;

        private ToolStripButton tsbCallAgent;

        private ToolStripButton tsbCheck;

        private ToolStripButton tsbHold;

        private ToolStripLabel tslAgentInfo;

        private ToolStripLabel tslStatus;

        private ToolStrip agentTool;

        private ToolStripButton tsbThreeWay;

        private ToolStripButton tsbConsult;

        private ToolStripButton tsbStopConsult;

        private ToolStripButton tsbConsultTransfer;

        private ToolStripButton tsbCallOut;

        private ToolStripDropDownButton tsbTransfer;

        private ToolStripMenuItem tsmi_transferAgent;

        private ToolStripMenuItem tsmi_transferIvr;

        private ToolStripMenuItem tsmi_transferQueue;

        private ToolStripDropDownButton tsbState;

        private ToolStripButton tsbAnswer;

        private Button button1;

        private ToolStripDropDownButton tsddbAfterHangup;

        private ToolStripSplitButton tsbGrade;

        private ToolStripMenuItem 中文ToolStripMenuItem;

        private ToolStripMenuItem 英文ToolStripMenuItem;

        private ToolStripMenuItem tsmi_transferIvr_Profile;

        private ToolStripSplitButton tsbMonitor;

        private ToolStripMenuItem tsmiScreen;

        private ToolStripMenuItem tsmiMonitor;

        public ToolStripButton tsbListen;

        public ToolStripButton tsbBargein;

        public ToolStripButton tsbForceHangup;

        public ToolStripButton tsbWhisper;

        private ToolStripButton tsbKeyPad;

        private ToolStripButton tsbCancelApplication;

        private ToolStripButton tsbApprove;

        private ToolStripButton tsbMute;

        private ToolStripButton tsbNoAnswerCalls;

        private ToolTip toolTip1;

        private ToolStripSeparator tsSeparator1;

        private ToolStripSeparator toolStripSeparator1;

        private ToolStripMenuItem tsmiIdle;

        public ToolStripMenuItem tsmiAcw;

        private ToolStripMenuItem tsmiRestore;

        private System.Windows.Forms.Timer trmCall;

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

        private AgentBar.Agent_Status mAgentStateAfterHangup = AgentBar.Agent_Status.AGENT_STATUS_ACW;

        private AgentBar.Agent_Status mAgentStateBeforeCallinOrCallout;

        private bool mBindExten;

        private string mAgentStatus;

        private string mExtenMode;

        private string mWebUrl;

        private string mServerIP;

        private AgentBar.Call_Type mCallType;

        private AgentBar.CallStatus mCallStatus = AgentBar.CallStatus.NO_CALL;

        private AgentBar.Agent_State mAgentState;

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

        public AgentBar.ControlsInfo controls_info = default(AgentBar.ControlsInfo);

        public AgentBar.ControlsVisible controlsVisible = default(AgentBar.ControlsVisible);

        public ToolStripDropDownButton basic = new ToolStripDropDownButton();

        public ToolStripDropDownButton advanced = new ToolStripDropDownButton();

        public ToolStripDropDownButton other = new ToolStripDropDownButton();

        public bool ApproveNormal = true;

        public bool NoAnswerCallNormal = true;

        public bool otherNormal = true;

        private Form InputForm;

        public static ILog Log;

        private int mPort;

        private FrmTransfer newFrmTransfer;

        private FrmMonitor newFrmMonitor;

        private frmCallOut newFrmCallOut;

        private FrmMonitorScreen newFrmMonitorScreen;

        private FrmApplication newFrmApplication;

        private FrmKeyPad newKeyPad;

        private FrmMessageBox newFrmMessageBox;

        private FrmNoAnswerCalls frmNoAnswerCalls;

        private SoundPlayer sndPlayer;

        private ProcessStartInfo process_info;

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

        public string AssemblyVersion
        {
            get
            {
                return Assembly.GetExecutingAssembly().GetName().Version.ToString();
            }
        }

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

        public AgentBar.Agent_State AgentState
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

        public AgentBar.Agent_Status AgentStateAfterHangup
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

        public AgentBar.CallStatus GetCallStatus
        {
            get
            {
                return this.mCallStatus;
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && this.components != null)
            {
                this.components.Dispose();
            }
            if (this.agentDll != null)
            {
                this.agentDll.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new Container();
            ComponentResourceManager resources = new ComponentResourceManager(typeof(AgentBar));
            this.agentTool = new ToolStrip();
            this.tslAgentInfo = new ToolStripLabel();
            this.tslStatus = new ToolStripLabel();
            this.tsbState = new ToolStripDropDownButton();
            this.tsbAnswer = new ToolStripButton();
            this.tsbHangUp = new ToolStripButton();
            this.tsbKeyPad = new ToolStripButton();
            this.tsbCancelApplication = new ToolStripButton();
            this.tsbCallAgent = new ToolStripButton();
            this.tsbCallOut = new ToolStripButton();
            this.tsbTransfer = new ToolStripDropDownButton();
            this.tsmi_transferAgent = new ToolStripMenuItem();
            this.tsmi_transferIvr = new ToolStripMenuItem();
            this.tsmi_transferQueue = new ToolStripMenuItem();
            this.tsmi_transferIvr_Profile = new ToolStripMenuItem();
            this.tsbGrade = new ToolStripSplitButton();
            this.中文ToolStripMenuItem = new ToolStripMenuItem();
            this.英文ToolStripMenuItem = new ToolStripMenuItem();
            this.tsbHold = new ToolStripButton();
            this.tsbMute = new ToolStripButton();
            this.tsbThreeWay = new ToolStripButton();
            this.tsbConsult = new ToolStripButton();
            this.tsbStopConsult = new ToolStripButton();
            this.tsbConsultTransfer = new ToolStripButton();
            this.tsSeparator1 = new ToolStripSeparator();
            this.tsbListen = new ToolStripButton();
            this.tsbWhisper = new ToolStripButton();
            this.tsbBargein = new ToolStripButton();
            this.tsbForceHangup = new ToolStripButton();
            this.toolStripSeparator1 = new ToolStripSeparator();
            this.tsddbAfterHangup = new ToolStripDropDownButton();
            this.tsmiIdle = new ToolStripMenuItem();
            this.tsmiAcw = new ToolStripMenuItem();
            this.tsmiRestore = new ToolStripMenuItem();
            this.tsbCheck = new ToolStripButton();
            this.tsbMonitor = new ToolStripSplitButton();
            this.tsmiMonitor = new ToolStripMenuItem();
            this.tsmiScreen = new ToolStripMenuItem();
            this.tsbApprove = new ToolStripButton();
            this.tsbNoAnswerCalls = new ToolStripButton();
            this.button1 = new Button();
            this.toolTip1 = new ToolTip(this.components);
            this.trmCall = new System.Windows.Forms.Timer(this.components);
            this.agentTool.SuspendLayout();
            base.SuspendLayout();
            this.agentTool.GripStyle = ToolStripGripStyle.Hidden;
            this.agentTool.ImageScalingSize = new Size(32, 32);
            this.agentTool.Items.AddRange(new ToolStripItem[]
            {
                this.tslAgentInfo,
                this.tslStatus,
                this.tsbState,
                this.tsbAnswer,
                this.tsbHangUp,
                this.tsbKeyPad,
                this.tsbCancelApplication,
                this.tsbCallAgent,
                this.tsbCallOut,
                this.tsbTransfer,
                this.tsbGrade,
                this.tsbHold,
                this.tsbMute,
                this.tsbThreeWay,
                this.tsbConsult,
                this.tsbStopConsult,
                this.tsbConsultTransfer,
                this.tsSeparator1,
                this.tsbListen,
                this.tsbWhisper,
                this.tsbBargein,
                this.tsbForceHangup,
                this.toolStripSeparator1,
                this.tsddbAfterHangup,
                this.tsbCheck,
                this.tsbMonitor,
                this.tsbApprove,
                this.tsbNoAnswerCalls
            });
            this.agentTool.Location = new Point(0, 0);
            this.agentTool.Name = "agentTool";
            this.agentTool.Size = new Size(2690, 39);
            this.agentTool.TabIndex = 0;
            this.agentTool.Text = "toolStrip1";
            this.tslAgentInfo.Image = Resources.agent;
            this.tslAgentInfo.Name = "tslAgentInfo";
            this.tslAgentInfo.Size = new Size(192, 36);
            this.tslAgentInfo.Text = "坐席号:100100 分机号:1001";
            this.tslStatus.AutoSize = false;
            this.tslStatus.Image = Resources.info;
            this.tslStatus.ImageAlign = ContentAlignment.MiddleLeft;
            this.tslStatus.Name = "tslStatus";
            this.tslStatus.Size = new Size(140, 36);
            this.tslStatus.Text = "状态:正在通话";
            this.tslStatus.TextAlign = ContentAlignment.MiddleLeft;
            this.tsbState.AutoSize = false;
            this.tsbState.Image = Resources.idle;
            this.tsbState.ImageAlign = ContentAlignment.MiddleLeft;
            this.tsbState.ImageScaling = ToolStripItemImageScaling.None;
            this.tsbState.ImageTransparentColor = Color.Magenta;
            this.tsbState.Name = "tsbState";
            this.tsbState.Size = new Size(130, 36);
            this.tsbState.Text = "状态";
            this.tsbState.TextAlign = ContentAlignment.MiddleLeft;
            this.tsbState.DropDownItemClicked += new ToolStripItemClickedEventHandler(this.tsbState_DropDownItemClicked);
            this.tsbState.Click += new EventHandler(this.tsbState_Click);
            this.tsbAnswer.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.tsbAnswer.Image = Resources.callin;
            this.tsbAnswer.ImageTransparentColor = Color.Magenta;
            this.tsbAnswer.Name = "tsbAnswer";
            this.tsbAnswer.Size = new Size(36, 36);
            this.tsbAnswer.Text = "接听";
            this.tsbAnswer.Click += new EventHandler(this.tsbAnswer_Click);
            this.tsbHangUp.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.tsbHangUp.Image = Resources.hangup;
            this.tsbHangUp.ImageTransparentColor = Color.Magenta;
            this.tsbHangUp.Name = "tsbHangUp";
            this.tsbHangUp.Size = new Size(36, 36);
            this.tsbHangUp.Text = "挂断";
            this.tsbHangUp.Click += new EventHandler(this.tsbHangUp_Click);
            this.tsbKeyPad.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.tsbKeyPad.Enabled = false;
            this.tsbKeyPad.Image = (Image)resources.GetObject("tsbKeyPad.Image");
            this.tsbKeyPad.ImageTransparentColor = Color.Magenta;
            this.tsbKeyPad.Name = "tsbKeyPad";
            this.tsbKeyPad.Size = new Size(36, 36);
            this.tsbKeyPad.Text = "拨号盘";
            this.tsbKeyPad.Click += new EventHandler(this.tsbKeyPad_Click);
            this.tsbCancelApplication.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.tsbCancelApplication.Enabled = false;
            this.tsbCancelApplication.Image = (Image)resources.GetObject("tsbCancelApplication.Image");
            this.tsbCancelApplication.ImageTransparentColor = Color.Magenta;
            this.tsbCancelApplication.Name = "tsbCancelApplication";
            this.tsbCancelApplication.Size = new Size(36, 36);
            this.tsbCancelApplication.Tag = true;
            this.tsbCancelApplication.Text = "取消申请";
            this.tsbCancelApplication.Click += new EventHandler(this.tsbCancelApplication_Click);
            this.tsbCallAgent.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.tsbCallAgent.Image = Resources.callAgent;
            this.tsbCallAgent.ImageTransparentColor = Color.Magenta;
            this.tsbCallAgent.Name = "tsbCallAgent";
            this.tsbCallAgent.Size = new Size(36, 36);
            this.tsbCallAgent.Tag = true;
            this.tsbCallAgent.Text = "呼叫坐席";
            this.tsbCallAgent.TextImageRelation = TextImageRelation.ImageAboveText;
            this.tsbCallAgent.Click += new EventHandler(this.tsbCallAgent_Click);
            this.tsbCallOut.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.tsbCallOut.Image = Resources._0012;
            this.tsbCallOut.ImageTransparentColor = Color.Magenta;
            this.tsbCallOut.Name = "tsbCallOut";
            this.tsbCallOut.Size = new Size(36, 36);
            this.tsbCallOut.Text = "拨外线";
            this.tsbCallOut.Click += new EventHandler(this.tsbCallOut_Click);
            this.tsbTransfer.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.tsbTransfer.DropDownItems.AddRange(new ToolStripItem[]
            {
                this.tsmi_transferAgent,
                this.tsmi_transferIvr,
                this.tsmi_transferQueue,
                this.tsmi_transferIvr_Profile
            });
            this.tsbTransfer.Image = Resources._0003;
            this.tsbTransfer.ImageTransparentColor = Color.Magenta;
            this.tsbTransfer.Name = "tsbTransfer";
            this.tsbTransfer.Size = new Size(45, 36);
            this.tsbTransfer.Tag = true;
            this.tsbTransfer.Text = "转接";
            this.tsbTransfer.TextAlign = ContentAlignment.MiddleLeft;
            this.tsbTransfer.TextImageRelation = TextImageRelation.ImageAboveText;
            this.tsmi_transferAgent.Name = "tsmi_transferAgent";
            this.tsmi_transferAgent.Size = new Size(161, 22);
            this.tsmi_transferAgent.Tag = true;
            this.tsmi_transferAgent.Text = "转接电话";
            this.tsmi_transferAgent.Click += new EventHandler(this.tsmi_transferAgent_Click);
            this.tsmi_transferIvr.Name = "tsmi_transferIvr";
            this.tsmi_transferIvr.Size = new Size(161, 22);
            this.tsmi_transferIvr.Tag = true;
            this.tsmi_transferIvr.Text = "转接IVR";
            this.tsmi_transferIvr.Click += new EventHandler(this.tsmi_transferIvr_Click);
            this.tsmi_transferQueue.Name = "tsmi_transferQueue";
            this.tsmi_transferQueue.Size = new Size(161, 22);
            this.tsmi_transferQueue.Tag = true;
            this.tsmi_transferQueue.Text = "转接队列";
            this.tsmi_transferQueue.Click += new EventHandler(this.tsmi_transferQueue_Click);
            this.tsmi_transferIvr_Profile.Name = "tsmi_transferIvr_Profile";
            this.tsmi_transferIvr_Profile.Size = new Size(161, 22);
            this.tsmi_transferIvr_Profile.Tag = true;
            this.tsmi_transferIvr_Profile.Text = "转接IVR Profile";
            this.tsmi_transferIvr_Profile.Click += new EventHandler(this.tsmi_transferIvr_Profile_Click);
            this.tsbGrade.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.tsbGrade.DropDownItems.AddRange(new ToolStripItem[]
            {
                this.中文ToolStripMenuItem,
                this.英文ToolStripMenuItem
            });
            this.tsbGrade.Image = Resources.grade;
            this.tsbGrade.ImageTransparentColor = Color.Magenta;
            this.tsbGrade.Name = "tsbGrade";
            this.tsbGrade.Size = new Size(48, 36);
            this.tsbGrade.Tag = true;
            this.tsbGrade.Text = "评分";
            this.tsbGrade.ToolTipText = "评分";
            this.tsbGrade.ButtonClick += new EventHandler(this.tsbGrade_ButtonClick);
            this.中文ToolStripMenuItem.Name = "中文ToolStripMenuItem";
            this.中文ToolStripMenuItem.Size = new Size(100, 22);
            this.中文ToolStripMenuItem.Text = "中文";
            this.中文ToolStripMenuItem.Click += new EventHandler(this.中文ToolStripMenuItem_Click);
            this.英文ToolStripMenuItem.Name = "英文ToolStripMenuItem";
            this.英文ToolStripMenuItem.Size = new Size(100, 22);
            this.英文ToolStripMenuItem.Text = "英文";
            this.英文ToolStripMenuItem.Click += new EventHandler(this.英文ToolStripMenuItem_Click);
            this.tsbHold.AccessibleName = "hold";
            this.tsbHold.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.tsbHold.Image = Resources._0033;
            this.tsbHold.ImageTransparentColor = Color.Magenta;
            this.tsbHold.Name = "tsbHold";
            this.tsbHold.Size = new Size(36, 36);
            this.tsbHold.Tag = true;
            this.tsbHold.Text = "保持";
            this.tsbHold.TextAlign = ContentAlignment.MiddleLeft;
            this.tsbHold.TextImageRelation = TextImageRelation.TextAboveImage;
            this.tsbHold.Click += new EventHandler(this.tsbHold_Click);
            this.tsbMute.AccessibleName = "mute";
            this.tsbMute.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.tsbMute.Image = Resources.mute;
            this.tsbMute.ImageTransparentColor = Color.Magenta;
            this.tsbMute.Name = "tsbMute";
            this.tsbMute.Size = new Size(36, 36);
            this.tsbMute.Tag = true;
            this.tsbMute.Text = "静音";
            this.tsbMute.Click += new EventHandler(this.tsbMute_Click);
            this.tsbThreeWay.AccessibleName = "threeway";
            this.tsbThreeWay.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.tsbThreeWay.Image = Resources._0001;
            this.tsbThreeWay.ImageTransparentColor = Color.Magenta;
            this.tsbThreeWay.Name = "tsbThreeWay";
            this.tsbThreeWay.Size = new Size(36, 36);
            this.tsbThreeWay.Tag = true;
            this.tsbThreeWay.Text = "三方";
            this.tsbThreeWay.Click += new EventHandler(this.tsbThreeWay_Click);
            this.tsbConsult.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.tsbConsult.Image = Resources._0013;
            this.tsbConsult.ImageTransparentColor = Color.Magenta;
            this.tsbConsult.Name = "tsbConsult";
            this.tsbConsult.Size = new Size(36, 36);
            this.tsbConsult.Tag = true;
            this.tsbConsult.Text = "询问";
            this.tsbConsult.Click += new EventHandler(this.tsbConsult_Click);
            this.tsbStopConsult.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.tsbStopConsult.Image = Resources._0008;
            this.tsbStopConsult.ImageTransparentColor = Color.Magenta;
            this.tsbStopConsult.Name = "tsbStopConsult";
            this.tsbStopConsult.Size = new Size(36, 36);
            this.tsbStopConsult.Tag = true;
            this.tsbStopConsult.Text = "取消询问";
            this.tsbStopConsult.Click += new EventHandler(this.tsbStopConsult_Click);
            this.tsbConsultTransfer.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.tsbConsultTransfer.Image = Resources._0006;
            this.tsbConsultTransfer.ImageTransparentColor = Color.Magenta;
            this.tsbConsultTransfer.Name = "tsbConsultTransfer";
            this.tsbConsultTransfer.Size = new Size(36, 36);
            this.tsbConsultTransfer.Tag = true;
            this.tsbConsultTransfer.Text = "询问转";
            this.tsbConsultTransfer.Click += new EventHandler(this.tsbConsultTransfer_Click);
            this.tsSeparator1.Name = "tsSeparator1";
            this.tsSeparator1.Size = new Size(6, 39);
            this.tsbListen.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.tsbListen.Image = Resources.eavesdrop1;
            this.tsbListen.ImageTransparentColor = Color.Magenta;
            this.tsbListen.Name = "tsbListen";
            this.tsbListen.Size = new Size(36, 36);
            this.tsbListen.Text = "监听";
            this.tsbListen.Click += new EventHandler(this.tsbListen_Click);
            this.tsbWhisper.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.tsbWhisper.Image = Resources.whisper;
            this.tsbWhisper.ImageTransparentColor = Color.Magenta;
            this.tsbWhisper.Name = "tsbWhisper";
            this.tsbWhisper.Size = new Size(36, 36);
            this.tsbWhisper.Text = "密语";
            this.tsbWhisper.Click += new EventHandler(this.tsbWhisper_Click);
            this.tsbBargein.BackColor = Color.Transparent;
            this.tsbBargein.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.tsbBargein.Image = Resources._0004;
            this.tsbBargein.ImageTransparentColor = Color.Magenta;
            this.tsbBargein.Name = "tsbBargein";
            this.tsbBargein.Size = new Size(36, 36);
            this.tsbBargein.Text = "强插";
            this.tsbBargein.Click += new EventHandler(this.tsbBargein_Click);
            this.tsbForceHangup.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.tsbForceHangup.Image = Resources._0005;
            this.tsbForceHangup.ImageTransparentColor = Color.Magenta;
            this.tsbForceHangup.Name = "tsbForceHangup";
            this.tsbForceHangup.Size = new Size(36, 36);
            this.tsbForceHangup.Text = "强拆";
            this.tsbForceHangup.Click += new EventHandler(this.tsbForceHangup_Click);
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new Size(6, 39);
            this.tsddbAfterHangup.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.tsddbAfterHangup.DoubleClickEnabled = true;
            this.tsddbAfterHangup.DropDownItems.AddRange(new ToolStripItem[]
            {
                this.tsmiIdle,
                this.tsmiAcw,
                this.tsmiRestore
            });
            this.tsddbAfterHangup.Image = Resources.acw;
            this.tsddbAfterHangup.ImageAlign = ContentAlignment.TopCenter;
            this.tsddbAfterHangup.ImageTransparentColor = Color.Magenta;
            this.tsddbAfterHangup.MergeAction = MergeAction.Replace;
            this.tsddbAfterHangup.Name = "tsddbAfterHangup";
            this.tsddbAfterHangup.Size = new Size(45, 36);
            this.tsddbAfterHangup.Text = "挂断后状态设置";
            this.tsddbAfterHangup.TextAlign = ContentAlignment.MiddleLeft;
            this.tsddbAfterHangup.ToolTipText = "挂断后是否转为后处理";
            this.tsmiIdle.AutoToolTip = true;
            this.tsmiIdle.Image = Resources.idle;
            this.tsmiIdle.ImageAlign = ContentAlignment.TopLeft;
            this.tsmiIdle.ImageScaling = ToolStripItemImageScaling.None;
            this.tsmiIdle.Name = "tsmiIdle";
            this.tsmiIdle.Size = new Size(188, 38);
            this.tsmiIdle.Text = "挂断后置为空闲";
            this.tsmiIdle.ToolTipText = "挂断后置为空闲";
            this.tsmiIdle.Click += new EventHandler(this.tsmiIdle_Click);
            this.tsmiAcw.AutoSize = false;
            this.tsmiAcw.AutoToolTip = true;
            this.tsmiAcw.Image = Resources.acw;
            this.tsmiAcw.ImageAlign = ContentAlignment.BottomLeft;
            this.tsmiAcw.ImageScaling = ToolStripItemImageScaling.None;
            this.tsmiAcw.MergeAction = MergeAction.Insert;
            this.tsmiAcw.Name = "tsmiAcw";
            this.tsmiAcw.Padding = new Padding(10, 0, 5, 3);
            this.tsmiAcw.Size = new Size(188, 38);
            this.tsmiAcw.Text = "挂断后置为后处理";
            this.tsmiAcw.ToolTipText = "挂断后置为后处理";
            this.tsmiAcw.Click += new EventHandler(this.tsmiAcw_Click);
            this.tsmiRestore.Image = Resources.restore;
            this.tsmiRestore.Name = "tsmiRestore";
            this.tsmiRestore.Size = new Size(188, 38);
            this.tsmiRestore.Text = "挂断后恢复原状态";
            this.tsmiRestore.ToolTipText = "挂断后恢复原状态";
            this.tsmiRestore.Click += new EventHandler(this.tsmiRestore_Click);
            this.tsbCheck.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.tsbCheck.Image = Resources.test;
            this.tsbCheck.ImageTransparentColor = Color.Magenta;
            this.tsbCheck.Name = "tsbCheck";
            this.tsbCheck.Size = new Size(36, 36);
            this.tsbCheck.Tag = true;
            this.tsbCheck.Text = "环回测试";
            this.tsbCheck.Click += new EventHandler(this.tsbCheck_Click);
            this.tsbMonitor.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.tsbMonitor.DropDownItems.AddRange(new ToolStripItem[]
            {
                this.tsmiMonitor,
                this.tsmiScreen
            });
            this.tsbMonitor.Image = Resources.monitor;
            this.tsbMonitor.ImageTransparentColor = Color.Magenta;
            this.tsbMonitor.Name = "tsbMonitor";
            this.tsbMonitor.Size = new Size(48, 36);
            this.tsbMonitor.Text = "监控";
            this.tsbMonitor.ButtonClick += new EventHandler(this.tsbMonitor_Click);
            this.tsmiMonitor.Name = "tsmiMonitor";
            this.tsmiMonitor.Size = new Size(136, 22);
            this.tsmiMonitor.Text = "话务监控";
            this.tsmiMonitor.Click += new EventHandler(this.tsmiMonitor_Click);
            this.tsmiScreen.Name = "tsmiScreen";
            this.tsmiScreen.Size = new Size(136, 22);
            this.tsmiScreen.Text = "现场监控屏";
            this.tsmiScreen.Click += new EventHandler(this.tsmiScreen_Click);
            this.tsbApprove.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.tsbApprove.Image = Resources.appoval2;
            this.tsbApprove.ImageTransparentColor = Color.Magenta;
            this.tsbApprove.Name = "tsbApprove";
            this.tsbApprove.Size = new Size(36, 36);
            this.tsbApprove.Text = "离开审批";
            this.tsbApprove.Click += new EventHandler(this.tsbApprove_Click);
            this.tsbNoAnswerCalls.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.tsbNoAnswerCalls.Image = Resources.NoAnswerCall_normal;
            this.tsbNoAnswerCalls.ImageTransparentColor = Color.Magenta;
            this.tsbNoAnswerCalls.Name = "tsbNoAnswerCalls";
            this.tsbNoAnswerCalls.Size = new Size(36, 36);
            this.tsbNoAnswerCalls.Tag = true;
            this.tsbNoAnswerCalls.Text = "未接来电";
            this.tsbNoAnswerCalls.Click += new EventHandler(this.tsbNoAnswerCalls_Click);
            this.button1.Location = new Point(1409, 3);
            this.button1.Name = "button1";
            this.button1.Size = new Size(37, 39);
            this.button1.TabIndex = 1;
            this.button1.Text = "签入";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Visible = false;
            this.button1.Click += new EventHandler(this.button1_Click);
            this.trmCall.Interval = 1000;
            this.trmCall.Tick += new EventHandler(this.trmCall_Tick);
            base.AutoScaleDimensions = new SizeF(6f, 12f);
            base.AutoScaleMode = AutoScaleMode.Font;
            this.AutoSize = true;
            this.BackColor = Color.Transparent;
            base.Controls.Add(this.button1);
            base.Controls.Add(this.agentTool);
            base.Name = "AgentBar";
            base.Size = new Size(2690, 64);
            base.Load += new EventHandler(this.AgentBar_Load);
            this.agentTool.ResumeLayout(false);
            this.agentTool.PerformLayout();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        [DllImport("user32.dll")]
        public static extern IntPtr FindWindow(string class_name, string app_name);

        [DllImport("User32.dll", EntryPoint = "SendMessage")]
        private static extern int PostMessage(IntPtr wnd, int msg, IntPtr wP, IntPtr lP);

        public void ProcessDelegate(AgentBar.AgentBarEventDelegate myDelegate, string jsonArgsStr)
        {
            myDelegate(jsonArgsStr);
        }

        public static AgentBar.Agent_Status Str2AgentStatus(string strAgentStatus)
        {
            AgentBar.Agent_Status result;
            try
            {
                if ("" != strAgentStatus)
                {
                    result = (AgentBar.Agent_Status)Enum.Parse(typeof(AgentBar.Agent_Status), strAgentStatus);
                }
                else
                {
                    result = AgentBar.Agent_Status.AGENT_STATUS_UNKNOWN;
                }
            }
            catch (Exception ex_33)
            {
                result = AgentBar.Agent_Status.AGENT_STATUS_UNKNOWN;
            }
            return result;
        }

        public static string AgentStatus2Str(AgentBar.Agent_Status agentStatus)
        {
            string agentName;
            switch (agentStatus)
            {
                case AgentBar.Agent_Status.AGENT_STATUS_UNKNOWN:
                    agentName = "未知";
                    break;
                case AgentBar.Agent_Status.AGENT_STATUS_OFFLINE:
                    agentName = "离线";
                    break;
                case AgentBar.Agent_Status.AGENT_STATUS_IDLE:
                    agentName = "空闲";
                    break;
                case AgentBar.Agent_Status.AGENT_STATUS_RING:
                    agentName = "响铃";
                    break;
                case AgentBar.Agent_Status.AGENT_STATUS_TALKING:
                    agentName = "通话";
                    break;
                case AgentBar.Agent_Status.AGENT_STATUS_HOLD:
                    agentName = "保持";
                    break;
                case AgentBar.Agent_Status.AGENT_STATUS_ACW:
                    agentName = "后处理";
                    break;
                case AgentBar.Agent_Status.AGENT_STATUS_CAMP_ON:
                    agentName = "预占";
                    break;
                case AgentBar.Agent_Status.AGENT_STATUS_BUSY:
                    agentName = "忙碌";
                    break;
                case AgentBar.Agent_Status.AGENT_STATUS_LEAVE:
                    agentName = "离开";
                    break;
                case AgentBar.Agent_Status.AGENT_STATUS_CALL_OUT:
                    agentName = "手动外呼中";
                    break;
                case AgentBar.Agent_Status.AGENT_STATUS_MONITOR:
                    agentName = "监控";
                    break;
                case AgentBar.Agent_Status.AGENT_STATUS_CALLING_OUT:
                    agentName = "呼出中";
                    break;
                case AgentBar.Agent_Status.AGENT_STATUS_MUTE:
                    agentName = "静音";
                    break;
                default:
                    agentName = "";
                    break;
            }
            return agentName;
        }

        public static int str2int(string agent_status)
        {
            int result;
            if (agent_status == "离线")
            {
                result = -1;
            }
            else
            {
                result = 0;
            }
            return result;
        }

        public static string CustomerStatus2Chinese(string customerStatus)
        {
            string result;
            if (customerStatus == "Waiting")
            {
                result = "等待";
            }
            else if (customerStatus == "Trying")
            {
                result = "振铃";
            }
            else
            {
                result = "未知";
            }
            return result;
        }

        public static string IcoName_CustomerStatus2Str(string customerStatus)
        {
            string result;
            if (customerStatus == "Waiting")
            {
                result = "wait";
            }
            else if (customerStatus == "Trying")
            {
                result = "try";
            }
            else
            {
                result = "unknow";
            }
            return result;
        }

        public static string IcoName_AgentStatus2Str(AgentBar.Agent_Status agentStatus)
        {
            string iconame = "";
            switch (agentStatus)
            {
                case AgentBar.Agent_Status.AGENT_STATUS_UNKNOWN:
                    iconame = "unknow";
                    break;
                case AgentBar.Agent_Status.AGENT_STATUS_OFFLINE:
                    iconame = "offline";
                    break;
                case AgentBar.Agent_Status.AGENT_STATUS_IDLE:
                    iconame = "idle";
                    break;
                case AgentBar.Agent_Status.AGENT_STATUS_RING:
                    iconame = "ring";
                    break;
                case AgentBar.Agent_Status.AGENT_STATUS_TALKING:
                    iconame = "talk";
                    break;
                case AgentBar.Agent_Status.AGENT_STATUS_HOLD:
                    iconame = "hold";
                    break;
                case AgentBar.Agent_Status.AGENT_STATUS_ACW:
                    iconame = "acw";
                    break;
                case AgentBar.Agent_Status.AGENT_STATUS_CAMP_ON:
                    iconame = "occupy";
                    break;
                case AgentBar.Agent_Status.AGENT_STATUS_BUSY:
                    iconame = "busy";
                    break;
                case AgentBar.Agent_Status.AGENT_STATUS_LEAVE:
                    iconame = "leave";
                    break;
                case AgentBar.Agent_Status.AGENT_STATUS_CALL_OUT:
                    iconame = "talk";
                    break;
                case AgentBar.Agent_Status.AGENT_STATUS_MONITOR:
                    iconame = "manage";
                    break;
                case AgentBar.Agent_Status.AGENT_STATUS_CALLING_OUT:
                    iconame = "calling";
                    break;
                case AgentBar.Agent_Status.AGENT_STATUS_MUTE:
                    iconame = "mute";
                    break;
            }
            return iconame;
        }

        public static string ApplyStatus2Str(Apply_State applyStatus)
        {
            string applyStatusStr = "";
            switch (applyStatus)
            {
                case Apply_State.Apply_State_Applying:
                    applyStatusStr = "申请中";
                    break;
                case Apply_State.Apply_State_Cancel:
                    applyStatusStr = "取消申请";
                    break;
                case Apply_State.Apply_State_Approval_Pass:
                    applyStatusStr = "审批通过";
                    break;
                case Apply_State.Apply_State_Approval_NoPass:
                    applyStatusStr = "审批不通过";
                    break;
                case Apply_State.Apply_State_Execute_Success:
                    applyStatusStr = "执行成功";
                    break;
                case Apply_State.Apply_State_Exeute:
                    applyStatusStr = "执行中";
                    break;
                case Apply_State.Apply_State_Execute_Failed:
                    applyStatusStr = "执行失败";
                    break;
            }
            return applyStatusStr;
        }

        public static Apply_State Str2ApplyState(string strApplyStateStr)
        {
            Apply_State result;
            try
            {
                switch (strApplyStateStr)
                {
                    case "申请中":
                        result = Apply_State.Apply_State_Applying;
                        return result;
                    case "取消申请":
                        result = Apply_State.Apply_State_Cancel;
                        return result;
                    case "审批通过":
                        result = Apply_State.Apply_State_Approval_Pass;
                        return result;
                    case "审批不通过":
                        result = Apply_State.Apply_State_Approval_NoPass;
                        return result;
                    case "执行中":
                        result = Apply_State.Apply_State_Exeute;
                        return result;
                    case "执行成功":
                        result = Apply_State.Apply_State_Execute_Success;
                        return result;
                    case "执行失败":
                        result = Apply_State.Apply_State_Execute_Failed;
                        return result;
                }
                result = Apply_State.Unknow;
            }
            catch (Exception ex_C9)
            {
                result = Apply_State.Unknow;
            }
            return result;
        }

        private string HangupReason2Chinese(string hangupReason)
        {
            string result;
            if (hangupReason == null)
            {
                result = "";
            }
            else
            {
                AgentBar.Log.Debug("enter HangupReason2Chinese .hangupReason:" + hangupReason);
                string text = hangupReason.ToUpper();
                string strReason;
                if (text != null)
                {
                    if (text == "NO_ANSWER")
                    {
                        strReason = "未接";
                        goto IL_B0;
                    }
                    if (text == "ORIGINATOR_CANCEL")
                    {
                        strReason = "对方挂断";
                        goto IL_B0;
                    }
                    if (text == "NORMAL_CLEARING")
                    {
                        strReason = "已挂断";
                        goto IL_B0;
                    }
                    if (text == "CALLOUT_FAILED")
                    {
                        strReason = "呼叫失败";
                        goto IL_B0;
                    }
                    if (text == "ALL_GATEWAYS_FULL")
                    {
                        strReason = "外线满线";
                        goto IL_B0;
                    }
                }
                strReason = "拒接";
                IL_B0:
                result = strReason;
            }
            return result;
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
                AgentBar.Log.Debug("enter HangupFailReason2Chinese .hangupReason:" + hangupFailedReason);
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
                AgentBar.Log.Debug("enter ThreeWayHangupReason2Chinese .hangupReason:" + hangupReason);
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
                AgentBar.Log.Debug("enter ConsulteeHangupReason2Chinese .hangupReason:" + hangupReason);
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

        public static string str2ApplyType(string apply_type)
        {
            string result;
            if (apply_type != null)
            {
                if (apply_type == "1")
                {
                    result = "坐席";
                    return result;
                }
                if (apply_type == "2")
                {
                    result = "系统";
                    return result;
                }
            }
            result = "未知";
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

        public AgentBar()
        {
            base.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            this.InitializeComponent();
            XmlConfigurator.Configure(new FileInfo(ComFunc.APPDATA_PATH + "\\CTIClient\\log4net.config"));
            AgentBar.Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
            AgentBar.Log.Info("AgentBar init success！");
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

        private void initAgentTool()
        {
            this.update_Toolbar_UI(AgentBar.Event_Type.INITE_TOOLBAR, "");
        }

        public static int ChkIsTalking(string agent_status)
        {
            int result;
            if (string.IsNullOrEmpty(agent_status))
            {
                result = -1;
            }
            else if (AgentBar.Str2AgentStatus(agent_status) == AgentBar.Agent_Status.AGENT_STATUS_HOLD || AgentBar.Str2AgentStatus(agent_status) == AgentBar.Agent_Status.AGENT_STATUS_MUTE || AgentBar.Str2AgentStatus(agent_status) == AgentBar.Agent_Status.AGENT_STATUS_TALKING)
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
            else if (AgentBar.Str2AgentStatus(agent_status) == AgentBar.Agent_Status.AGENT_STATUS_TALKING)
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
            else if (AgentBar.Str2AgentStatus(agent_status) == AgentBar.Agent_Status.AGENT_STATUS_HOLD || AgentBar.Str2AgentStatus(agent_status) == AgentBar.Agent_Status.AGENT_STATUS_MUTE)
            {
                result = 0;
            }
            else
            {
                result = -2;
            }
            return result;
        }

        public int GetAgentBarItemEnableAndVisible(int itemIndex)
        {
            int itemVisible = 0;
            int itemEnable = 0;
            int result;
            if (itemIndex >= 0 && itemIndex < this.agentTool.Items.Count)
            {
                if (this.agentTool.Items[itemIndex].Visible)
                {
                    itemVisible = 1;
                }
                if (this.agentTool.Items[itemIndex].Enabled)
                {
                    itemEnable = 1;
                }
                result = (itemVisible << 1 | itemEnable);
            }
            else
            {
                result = -1;
            }
            return result;
        }

        public int GetAgentBarItemEnableAndVisible(string itemKey)
        {
            int itemVisible = 0;
            int itemEnable = 0;
            int result;
            if (this.agentTool.Items.ContainsKey(itemKey))
            {
                if (this.agentTool.Items[itemKey].Visible)
                {
                    itemVisible = 1;
                }
                if (this.agentTool.Items[itemKey].Enabled)
                {
                    itemEnable = 1;
                }
                result = (itemVisible << 1 | itemEnable);
            }
            else if (this.other.DropDownItems.ContainsKey(itemKey))
            {
                if ((bool)this.other.DropDownItems[itemKey].Tag)
                {
                    itemVisible = 1;
                }
                if (this.other.DropDownItems[itemKey].Enabled)
                {
                    itemEnable = 1;
                }
                result = (itemVisible << 1 | itemEnable);
            }
            else if (this.basic.DropDownItems.ContainsKey(itemKey))
            {
                if ((bool)this.basic.DropDownItems[itemKey].Tag)
                {
                    itemVisible = 1;
                }
                if (this.basic.DropDownItems[itemKey].Enabled)
                {
                    itemEnable = 1;
                }
                result = (itemVisible << 1 | itemEnable);
            }
            else if (this.advanced.DropDownItems.ContainsKey(itemKey))
            {
                if ((bool)this.advanced.DropDownItems[itemKey].Tag)
                {
                    itemVisible = 1;
                }
                if (this.advanced.DropDownItems[itemKey].Enabled)
                {
                    itemEnable = 1;
                }
                result = (itemVisible << 1 | itemEnable);
            }
            else
            {
                result = -1;
            }
            return result;
        }

        public int GetAgentBarItemVisible(int itemIndex)
        {
            int result;
            if (itemIndex >= 0 && itemIndex < this.agentTool.Items.Count)
            {
                if (this.agentTool.Items[itemIndex].Visible)
                {
                    result = 1;
                }
                else
                {
                    result = 0;
                }
            }
            else
            {
                result = -1;
            }
            return result;
        }

        public int GetAgentBarItemVisible(string itemKey)
        {
            int result;
            if (this.agentTool.Items.ContainsKey(itemKey))
            {
                if (this.agentTool.Items[itemKey].Visible)
                {
                    result = 1;
                }
                else
                {
                    result = 0;
                }
            }
            else if (this.other.DropDownItems.ContainsKey(itemKey))
            {
                if ((bool)this.other.DropDownItems[itemKey].Tag)
                {
                    result = 1;
                }
                else
                {
                    result = 0;
                }
            }
            else
            {
                result = -1;
            }
            return result;
        }

        public string GetAgentBarStatusInfo()
        {
            return this.tslStatus.Text;
        }

        public void Evt_SetAgentStatusHotKeyString(string hotKeyIdle, string hotKeyBusy, string hotKeyLeave, string hotKeyCallOut, string hotKeyMonitor)
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
                AgentBar.Log.Debug(string.Concat(new string[]
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
                AgentBar.Log.Error(e.Source + e.Message + e.StackTrace);
                result = false;
                return result;
            }
            result = true;
            return result;
        }

        private void OnEvent_CommonCallIn(string agentID, string agent_call_uuid, string callerID, string calledID, string accessNumName, string makeStr, string callType, string relation_uuid, string areaID, string areaName, string custGrade, string outExtraParamsFromIvr, string todayDate, string customer_num_format_local, string customer_num_format_national, string customer_num_format_e164, string customer_num_phone_type, string predictCustomerForeignId, string predictCustomerName, string predictCustomerRemark)
        {
            AgentBar.Log.Debug(string.Concat(new string[]
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
            this.mCallType = AgentBar.Call_Type.COMMON_CALL_IN;
            this.mCallStatus = AgentBar.CallStatus.CALL_IN;
            this.m_agent_current_call_uuid = agent_call_uuid;
            this.update_Toolbar_UI(AgentBar.Event_Type.CALLIN_COMMON, "");
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
            AgentBar.Log.Debug(string.Concat(new string[]
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
            this.mCallType = AgentBar.Call_Type.PREDICT_CALL_OUT;
            this.mCallStatus = AgentBar.CallStatus.CALL_OUT;
            this.update_Toolbar_UI(AgentBar.Event_Type.CALLIN_PREDICT_CALL, "");
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
            AgentBar.Log.Debug(string.Concat(new string[]
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
            this.mCallType = AgentBar.Call_Type.AGENT_INTERNAL_CALL;
            this.mCallStatus = AgentBar.CallStatus.CALL_IN;
            this.m_agent_current_call_uuid = agent_call_uuid;
            this.update_Toolbar_UI(AgentBar.Event_Type.CALLIN_INTERNAL_MYSELF, "内部呼叫");
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
            AgentBar.Log.Debug(string.Concat(new object[]
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
                this.update_Toolbar_UI(AgentBar.Event_Type.CONSULT_SUCCESS, "");
            }
            else
            {
                AgentBar.Log.Debug("OnEvent_Consult has error!reason=" + reason);
                this.update_Toolbar_UI(AgentBar.Event_Type.CONSULT_FAIL, "");
            }
            this.Consultcheck = false;
        }

        private void OnEvent_hold_Result(string agentID, int retCode, string reason)
        {
            AgentBar.Log.Debug(string.Concat(new object[]
            {
                "enter OnEvent_hold_Result .agentID:",
                agentID,
                ",retCode:",
                retCode,
                ",reason:",
                reason
            }));
            if (retCode == 0)
            {
                this.update_Toolbar_UI(AgentBar.Event_Type.HOLD_SUCCESS, "");
            }
            else
            {
                this.update_Toolbar_UI(AgentBar.Event_Type.HOLD_FAIL, "");
            }
        }

        private void OnEvent_Unhold_Result(string agentID, int retCode, string reason)
        {
            AgentBar.Log.Debug(string.Concat(new object[]
            {
                "enter OnEvent_Unhold_Result .agentID:",
                agentID,
                ",retCode:",
                retCode,
                ",reason:",
                reason
            }));
            if (retCode == 0)
            {
                this.update_Toolbar_UI(AgentBar.Event_Type.UNHOLD_SUCCESS, "");
            }
            else
            {
                AgentBar.Log.Debug("OnEvent_Unhold_Result has error!reason=" + reason);
                this.update_Toolbar_UI(AgentBar.Event_Type.UNHOLD_FAIL, "");
            }
        }

        private void OnEvent_Mute_Result(string agentID, int retCode, string reason)
        {
            AgentBar.Log.Debug(string.Concat(new object[]
            {
                "enter OnEvent_mute_Result .agentID:",
                agentID,
                ",retCode:",
                retCode,
                ",reason:",
                reason
            }));
            if (retCode == 0)
            {
                this.update_Toolbar_UI(AgentBar.Event_Type.MUTE_SUCCESS, "");
            }
            else
            {
                this.update_Toolbar_UI(AgentBar.Event_Type.MUTE_FAIL, "");
            }
        }

        private void OnEvent_Unmute_Result(string agentID, int retCode, string reason)
        {
            AgentBar.Log.Debug(string.Concat(new object[]
            {
                "enter OnEvent_Unmute_Result .agentID:",
                agentID,
                ",retCode:",
                retCode,
                ",reason:",
                reason
            }));
            if (retCode == 0)
            {
                this.update_Toolbar_UI(AgentBar.Event_Type.UNMUTE_SUCCESS, "");
            }
            else
            {
                AgentBar.Log.Debug("OnEvent_Unmute_Result has error!reason=" + reason);
                this.update_Toolbar_UI(AgentBar.Event_Type.UNMUTE_FAIL, "");
            }
        }

        private void OnEvent_Consult_Cancel(string agentID, int retCode, string reason)
        {
            AgentBar.Log.Debug(string.Concat(new object[]
            {
                "enter OnEvent_Consult_Cancel .agentID:",
                agentID,
                ",retCode:",
                retCode,
                ",reason:",
                reason
            }));
            if (retCode == 0)
            {
                this.update_Toolbar_UI(AgentBar.Event_Type.CONSULT_CANCEL_SUCCESS, "");
            }
            else
            {
                this.update_Toolbar_UI(AgentBar.Event_Type.CONSULT_CANCEL_FAIL, "");
            }
        }

        private void OnEvent_Consult_Transfer(string agentID, int retCode, string reason)
        {
            AgentBar.Log.Debug(string.Concat(new object[]
            {
                "enter OnEvent_Consult_Cancel .agentID:",
                agentID,
                ",retCode:",
                retCode,
                ",reason:",
                reason
            }));
            if (retCode == 0)
            {
                this.update_Toolbar_UI(AgentBar.Event_Type.CONSULT_TRANSFER_SUCCESS, "");
            }
            else
            {
                AgentBar.Log.Debug("OnEvent_Consult_Transfer has error!reason=" + reason);
                this.update_Toolbar_UI(AgentBar.Event_Type.CONSULT_TRANSFER_FAIL, "");
            }
        }

        private void OnEvent_Consult_Callin(string agentID, string agent_call_uuid, string callerID, string calledID, string accessNumName, string consulterAgentNum, string callType, string areaID, string areaName, string custGrade, string outExtraParamsFromIvr, string todayDate, string customer_num_format_local, string customer_num_format_national, string customer_num_format_e164, string customer_num_phone_type, string customerForeignId, string predictCustomerForeignId, string predictCustomerName, string predictCustomerRemark, string relation_uuid)
        {
            AgentBar.Log.Debug(string.Concat(new string[]
            {
                "enter OnEvent_Consult_Callin.agentID:",
                agentID,
                ",agent_call_uuid:",
                agent_call_uuid,
                ",callerID:",
                callerID,
                ",calledID:",
                calledID,
                ",accessNumName:",
                accessNumName,
                ",consulterAgentNum:",
                consulterAgentNum,
                ",callType:",
                callType,
                ",areaID:",
                areaID,
                ",areaName:",
                areaName,
                ",custGrade:",
                custGrade,
                ",outExtraParamsFromIvr",
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
            this.mCallType = AgentBar.Call_Type.CONSULT_CALL_IN;
            this.mCallStatus = AgentBar.CallStatus.CALL_IN;
            this.m_agent_current_call_uuid = agent_call_uuid;
            this.update_Toolbar_UI(AgentBar.Event_Type.CONSULT_CALL_IN, "");
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
                this.ConsultCallInEvent(agentID, callerID, calledID, accessNumName, consulterAgentNum, callType, areaID, areaName, custGrade, ComFunc.PLDESDecrypt(outExtraParamsFromIvr, this.SaltKey, todayDate), makeStr);
            }
        }

        private void OnEvent_Transfer_Blind_Call_In(string agentID, string agent_call_uuid, string callerID, string calledID, string accessNumName, string callerAgentNum, string callType, string areaID, string areaName, string cust_grade, string outExtraParamsFromIvr, string todayDate, string customer_num_format_local, string customer_num_format_national, string customer_num_format_e164, string customer_num_phone_type, string customerForeignId, string predictCustomerForeignId, string predictCustomerName, string predictCustomerRemark, string relation_uuid)
        {
            AgentBar.Log.Debug(string.Concat(new string[]
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
            this.mCallType = AgentBar.Call_Type.COMMON_CALL_IN;
            this.mCallStatus = AgentBar.CallStatus.CALL_IN;
            this.m_agent_current_call_uuid = agent_call_uuid;
            this.update_Toolbar_UI(AgentBar.Event_Type.TRANSFER_BLIND_CALL_IN, "");
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
                this.TransferBlindCallInEvent(agentID, callerID, calledID, accessNumName, callerAgentNum, callType, areaID, areaName, cust_grade, ComFunc.PLDESDecrypt(outExtraParamsFromIvr, this.SaltKey, todayDate), makeStr);
            }
        }

        private void OnEvent_Transfer_Agent(string agentID, int retCode, string reason)
        {
            AgentBar.Log.Debug(string.Concat(new object[]
            {
                "enter OnEvent_Transfer_Agent.agentID:",
                agentID,
                ",retCode:",
                retCode,
                ",reason:",
                reason
            }));
            if (retCode == 0)
            {
                this.update_Toolbar_UI(AgentBar.Event_Type.TRANSFER_AGENT_SUCCESS, "");
            }
            else
            {
                AgentBar.Log.Debug("OnEvent_Transfer_Agent has error!reason=" + reason);
                this.update_Toolbar_UI(AgentBar.Event_Type.TRANSFER_AGENT_FAIL, "");
            }
            this.transfercheck = false;
        }

        private void OnEvent_Transfer_Ivr(string agentID, int retCode, string reason)
        {
            AgentBar.Log.Debug(string.Concat(new object[]
            {
                "enter OnEvent_Transfer_Ivr.agentID:",
                agentID,
                ",retCode:",
                retCode,
                ",reason:",
                reason
            }));
            if (retCode == 0)
            {
                this.update_Toolbar_UI(AgentBar.Event_Type.TRANSFER_IVR_SUCCESS, "");
            }
            else
            {
                AgentBar.Log.Debug("OnEvent_Transfer_Ivr has error!reason=" + reason);
                this.update_Toolbar_UI(AgentBar.Event_Type.TRANSFER_IVR_FAIL, "");
            }
        }

        private void OnEvent_Transfer_Queue(string agentID, int retCode, string reason)
        {
            AgentBar.Log.Debug(string.Concat(new object[]
            {
                "enter OnEvent_Transfer_Queue.agentID:",
                agentID,
                ",retCode:",
                retCode,
                ",reason:",
                reason
            }));
            if (retCode == 0)
            {
                this.update_Toolbar_UI(AgentBar.Event_Type.TRANSFER_QUEUE_SUCCESS, "");
            }
            else
            {
                AgentBar.Log.Debug("OnEvent_Transfer_Queue has error!reason=" + reason);
                this.update_Toolbar_UI(AgentBar.Event_Type.TRANSFER_QUEUE_FAIL, "");
            }
        }

        private void OnEvent_Transfer_Ivr_Profile(string agentID, int retCode, string reason)
        {
            AgentBar.Log.Debug(string.Concat(new object[]
            {
                "enter OnEvent_Transfer_Ivr_Profile.agentID:",
                agentID,
                ",retCode:",
                retCode,
                ",reason:",
                reason
            }));
            if (retCode == 0)
            {
                this.update_Toolbar_UI(AgentBar.Event_Type.TRANSFER_IVR_PROFILE_SUCCESS, "");
            }
            else
            {
                AgentBar.Log.Debug("OnEvent_Transfer_Ivr has error!reason=" + reason);
                this.update_Toolbar_UI(AgentBar.Event_Type.TRANSFER_IVR_PROFILE_FAIL, "");
            }
        }

        private void OnEvent_Get_Access_Number(string agentID, string reason, int retCode, string[] accessNumbers)
        {
            AgentBar.Log.Debug(string.Concat(new object[]
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
                    AgentBar.Log.Debug("before call GetAccessNumberEvent.");
                    AgentBar.Log.Debug("call GetAccessNumberEvent. mDefaultAccessNum=" + this.mDefaultAccessNum);
                    AgentBar.Log.Debug("call GetAccessNumberEvent. mCalloutHistory=" + this.mCalloutHistory);
                    this.GetAccessNumberEvent(accessNumbers, this.mDefaultAccessNum, this.mCalloutHistory);
                    AgentBar.Log.Debug("after call GetAccessNumberEvent.");
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
                this.update_Toolbar_UI(AgentBar.Event_Type.GET_ACCESS_NUMBERS_SUCCESS, "");
            }
            else
            {
                AgentBar.Log.Debug("OnEvent_Get_Access_Number has error!reason=" + reason);
                this.update_Toolbar_UI(AgentBar.Event_Type.GET_ACCESS_NUMBERS_FAIL, "");
            }
        }

        private void OnEvent_Three_Way_Call(string agentID, string reason, int retCode)
        {
            AgentBar.Log.Debug(string.Concat(new object[]
            {
                "enter OnEvent_Three_Way_Call.agentID:",
                agentID,
                ",reason:",
                reason,
                ",retCode:",
                retCode
            }));
            if (retCode == 0)
            {
                this.update_Toolbar_UI(AgentBar.Event_Type.THREE_WAY_SUCCESS, "");
            }
            else
            {
                AgentBar.Log.Debug("OnEvent_Three_Way_Call has error!reason=" + reason);
                this.update_Toolbar_UI(AgentBar.Event_Type.THREE_WAY_FAIL, "");
            }
        }

        private void OnEvent_Three_Way_Cancel(string agentID, string reason, int retCode)
        {
            AgentBar.Log.Debug(string.Concat(new object[]
            {
                "enter OnEvent_Three_Way_Cancel.agentID:",
                agentID,
                ",reason:",
                reason,
                ",retCode:",
                retCode
            }));
            if (retCode == 0)
            {
                this.update_Toolbar_UI(AgentBar.Event_Type.THREE_WAY_CANCEL_SUCCESS, "");
            }
            else
            {
                AgentBar.Log.Debug("OnEvent_Three_Way_Cancel has error!reason=" + reason);
                this.update_Toolbar_UI(AgentBar.Event_Type.THREE_WAY_CANCEL_FAIL, "");
            }
        }

        private void OnEvent_Three_Way_Call_In(string agentID, string agent_call_uuid, string callerID, string calledID, string accessNumName, string callerAgentNum, string callType, string areaID, string areaName, string custGrade, string outExtraParamsFromIvr, string todayDate, string customer_num_format_local, string customer_num_format_national, string customer_num_format_e164, string customer_num_phone_type, string customerForeignId, string predictCustomerForeignId, string predictCustomerName, string predictCustomerRemark, string relation_uuid)
        {
            AgentBar.Log.Debug(string.Concat(new string[]
            {
                "enter OnEvent_Three_Way_Call_In.agentID:",
                agentID,
                ",agent_call_uuid:",
                agent_call_uuid,
                ",callerID:",
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
                ",custGrade:",
                custGrade,
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
            this.mCallType = AgentBar.Call_Type.THREEWAY_CALL_IN;
            this.mCallStatus = AgentBar.CallStatus.CALL_IN;
            this.m_agent_current_call_uuid = agent_call_uuid;
            this.update_Toolbar_UI(AgentBar.Event_Type.CALLIN_THREE_WAY, "三方通话邀请");
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
            AgentBar.Log.Debug(string.Concat(new object[]
            {
                "enter OnEvent_Eavesdrop.agentID:",
                agentID,
                ",reason:",
                reason,
                ",retCode:",
                retCode
            }));
            if (retCode == 0)
            {
                this.update_Toolbar_UI(AgentBar.Event_Type.EAVESDROP_SUCCESS, "");
            }
            else
            {
                AgentBar.Log.Debug("OnEvent_Eavesdrop has error!reason=" + reason);
                this.update_Toolbar_UI(AgentBar.Event_Type.EAVESDROP_FAIL, "");
            }
            if (this.EavesdropEvent != null)
            {
                this.EavesdropEvent(agentID, reason, retCode);
            }
        }

        private void OnEvent_Whisper(string agentID, string reason, int retCode)
        {
            AgentBar.Log.Debug(string.Concat(new object[]
            {
                "enter OnEvent_Whisper.agentID:",
                agentID,
                ",reason:",
                reason,
                ",retCode:",
                retCode
            }));
            if (retCode == 0)
            {
                this.update_Toolbar_UI(AgentBar.Event_Type.WHISPER_SUCCESS, "");
            }
            else
            {
                AgentBar.Log.Debug("OnEvent_Whisper has error!reason=" + reason);
                this.update_Toolbar_UI(AgentBar.Event_Type.WHISPER_FAIL, "");
            }
            if (this.WhisperEvent != null)
            {
                this.WhisperEvent(agentID, reason, retCode);
            }
        }

        private void OnEvent_Force_Change_Status(string agentID, string reason, int retCode)
        {
            AgentBar.Log.Debug(string.Concat(new object[]
            {
                "enter OnEvent_Force_Change_Status.agentID:",
                agentID,
                ",reason:",
                reason,
                ",retCode:",
                retCode
            }));
            if (retCode != 0)
            {
                AgentBar.Log.Debug("OnEvent_Force_Change_Status has error!reason=" + reason);
            }
            if (this.ForceChangeStatusEvent != null)
            {
                this.ForceChangeStatusEvent(agentID, reason, retCode);
            }
        }

        private void OnEvent_Bargein(string agentID, string reason, int retCode)
        {
            AgentBar.Log.Debug(string.Concat(new object[]
            {
                "enter OnEvent_Bargein.agentID:",
                agentID,
                ",reason:",
                reason,
                ",retCode:",
                retCode
            }));
            if (retCode == 0)
            {
                this.update_Toolbar_UI(AgentBar.Event_Type.BARGE_IN_SUCCESS, "");
            }
            else
            {
                AgentBar.Log.Debug("OnEvent_Bargein has error!reason=" + reason);
                this.update_Toolbar_UI(AgentBar.Event_Type.BARGE_IN_FAIL, "");
            }
            if (this.BargeinEvent != null)
            {
                this.BargeinEvent(agentID, reason, retCode);
            }
        }

        private void OnEvent_Eavesdrop_Ring(string agentID, string agent_call_uuid, string callerID, string calledID, string accessNumName, string desAgentID, string callType, string areaID, string areaName, string custGrade, string outExtraParamsFromIvr, string todayDate, string customer_num_format_local, string customer_num_format_national, string customer_num_format_e164, string customer_num_phone_type, string customerForeignId, string relation_uuid)
        {
            AgentBar.Log.Debug(string.Concat(new string[]
            {
                "enter OnEvent_Eavesdrop_Ring.agentID:",
                agentID,
                ",callerID:",
                callerID,
                ",agent_call_uuid:",
                agent_call_uuid,
                ",calledID:",
                calledID,
                ",accessNumName:",
                accessNumName,
                ",desAgentID:",
                desAgentID,
                ",callType:",
                callType,
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
                ",relation_uuid:",
                relation_uuid
            }));
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
            AgentBar.Log.Debug(string.Concat(new string[]
            {
                "enter OnEvent_Whisper_Ring.agentID:",
                agentID,
                ",callerID:",
                callerID,
                ",agent_call_uuid:",
                agent_call_uuid,
                ",calledID:",
                calledID,
                ",accessNumName:",
                accessNumName,
                ",desAgentID:",
                desAgentID,
                ",callType:",
                callType,
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
                ",relation_uuid:",
                relation_uuid
            }));
            this.mCallType = AgentBar.Call_Type.WHISPER_CALL_IN;
            this.mCallStatus = AgentBar.CallStatus.CALL_OUT;
            this.mEavesdropAgent = desAgentID;
            this.m_agent_current_call_uuid = agent_call_uuid;
            this.update_Toolbar_UI(AgentBar.Event_Type.WHISPER_RING_MYSELF, "请摘机");
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
            AgentBar.Log.Debug(string.Concat(new string[]
            {
                "enter OnEvent_Bargein_Ring.agentID:",
                agentID,
                ",callerID:",
                callerID,
                ",agent_call_uuid:",
                agent_call_uuid,
                ",calledID:",
                calledID,
                ",accessNumName:",
                accessNumName,
                ",desAgentID:",
                desAgentID,
                ",callType:",
                callType,
                ",areaID:",
                areaID,
                ",areaName:",
                areaName,
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
                ",relation_uuid:",
                relation_uuid
            }));
            this.mCallType = AgentBar.Call_Type.BARGEIN_CALL_IN;
            this.mCallStatus = AgentBar.CallStatus.CALL_OUT;
            this.mEavesdropAgent = desAgentID;
            this.m_agent_current_call_uuid = agent_call_uuid;
            this.update_Toolbar_UI(AgentBar.Event_Type.BARGEIN_RING_MYSELF, "");
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
            AgentBar.Log.Debug(string.Concat(new object[]
            {
                "enter OnEvent_Get_Online_Agent.agentID:",
                agentID,
                ",reason:",
                reason,
                ",retCode:",
                retCode
            }));
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
                AgentBar.Log.Debug("OnEvent_Get_Online_Agent has error!reason=" + reason);
            }
        }

        private void OnEvent_Get_Ivr_List(string agentID, Dictionary<string, string> ivr_list, string reason, int retCode)
        {
            AgentBar.Log.Debug(string.Concat(new object[]
            {
                "enter OnEvent_Get_Ivr_List.agentID:",
                agentID,
                ",reason:",
                reason,
                ",retCode:",
                retCode
            }));
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
                AgentBar.Log.Debug("OnEvent_Get_Ivr_List has error!reason=" + reason);
            }
        }

        private void OnEvent_Get_Queue_List(string agentID, Dictionary<string, string> queue_list, string reason, int retCode)
        {
            AgentBar.Log.Debug(string.Concat(new object[]
            {
                "enter OnEvent_Get_Queue_List.agentID:",
                agentID,
                ",reason:",
                reason,
                ",retCode:",
                retCode
            }));
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
            AgentBar.Log.Debug(string.Concat(new object[]
            {
                "enter OnEvent_Get_Ivr_Profile_List.agentID:",
                agentID,
                ",reason:",
                reason,
                ",retCode:",
                retCode
            }));
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
                AgentBar.Log.Debug("OnEvent_Get_Ivr_Profile_List has error!reason=" + reason);
            }
        }

        private void OnEvent_Get_Defined_Role_Rights(string agentID, List<Agent_Role_And_Right_Struct> agent_role_and_right, string reason, int retCode)
        {
            AgentBar.Log.Debug(string.Concat(new object[]
            {
                "enter OnEvent_Get_Defined_Role_Rights.agentID:",
                agentID,
                ",reason:",
                reason,
                ",retCode:",
                retCode
            }));
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
                        this.show_agent_bar_by_right(ref this.mMyRoleAndRight);
                        if (this.GetRoleNameEvent != null)
                        {
                            this.GetRoleNameEvent(this.mRoleName);
                        }
                    }
                }
                this.load_default_status();
                for (i = 0; i < this.tsbState.DropDownItems.Count; i++)
                {
                    if (this.tsbState.DropDownItems[i].AccessibleName == this.mInitStatusWhenLogin)
                    {
                        this.tsbState.Image = this.tsbState.DropDownItems[i].Image;
                        int startPosOfSpace = this.tsbState.DropDownItems[i].Text.IndexOf(" ");
                        if (startPosOfSpace != -1)
                        {
                            this.tsbState.Text = this.tsbState.DropDownItems[i].Text.Substring(0, startPosOfSpace);
                        }
                        else
                        {
                            this.tsbState.Text = this.tsbState.DropDownItems[i].Text;
                        }
                        this.tsbState.AccessibleName = this.tsbState.DropDownItems[i].AccessibleName;
                        break;
                    }
                }
                if (i >= this.tsbState.DropDownItems.Count)
                {
                    AgentBar.Log.Error("此坐席初始状态值不存在！initStatus=" + this.mInitStatusWhenLogin);
                }
                if (this.mMyRoleAndRight.rights_of_view_agent_group_info)
                {
                    if (!this.DoGetChangeStatusApplyList(7.ToString()))
                    {
                        AgentBar.Log.Debug("获取坐席所管理的坐席组信息失败！");
                    }
                }
                if (!this.DoGetAgentDefineStatus())
                {
                    AgentBar.Log.Debug("获取坐席自定义状态失败！");
                }
                if (!this.DoGetAgentGroupList(""))
                {
                    AgentBar.Log.Debug("获取坐席组列表失败！");
                }
            }
            else
            {
                AgentBar.Log.Debug("OnEvent_Get_Defined_Role_Rights has error!reason=" + reason);
            }
        }

        private void show_agent_bar_by_right(ref Agent_Role_And_Right_Struct myRoleAndRight)
        {
            AgentBar.Log.Debug("enter show_agent_bar_by_right.");
            this.tsbListen.Visible = myRoleAndRight.rights_of_eavesdrop;
            this.tsbListen.Tag = myRoleAndRight.rights_of_eavesdrop;
            this.tsbWhisper.Visible = myRoleAndRight.rights_of_whisper;
            this.tsbWhisper.Tag = myRoleAndRight.rights_of_whisper;
            this.tsbBargein.Visible = myRoleAndRight.rights_of_bargein;
            this.tsbBargein.Tag = myRoleAndRight.rights_of_bargein;
            this.tsbForceHangup.Visible = myRoleAndRight.rights_of_forcehangup;
            this.tsbForceHangup.Tag = myRoleAndRight.rights_of_forcehangup;
            this.tsbCallOut.Visible = myRoleAndRight.rights_of_callout;
            this.tsbCallOut.Tag = myRoleAndRight.rights_of_callout;
            this.tsbMonitor.Visible = (myRoleAndRight.rights_of_view_agent_group_info || myRoleAndRight.rights_of_view_queue_info);
            this.tsbMonitor.Tag = (myRoleAndRight.rights_of_view_agent_group_info || myRoleAndRight.rights_of_view_queue_info);
            this.tsbApprove.Visible = myRoleAndRight.rights_of_view_agent_group_info;
            this.tsbApprove.Tag = myRoleAndRight.rights_of_view_agent_group_info;
            if (!this.tsbListen.Visible && !this.tsbWhisper.Visible && !this.tsbBargein.Visible && !this.tsbForceHangup.Visible)
            {
            }
            if (myRoleAndRight.rights_of_view_agent_group_info || myRoleAndRight.rights_of_view_queue_info || myRoleAndRight.role_right1)
            {
                this.tsbMonitor.Visible = true;
                this.tsbMonitor.Tag = true;
                this.tsbMonitor.Enabled = true;
                if (myRoleAndRight.role_right1)
                {
                    this.tsbMonitor.DropDownItems["tsmiScreen"].Visible = false;
                    this.tsbMonitor.DropDownItems["tsmiScreen"].Tag = false;
                }
                else
                {
                    this.tsbMonitor.DropDownItems["tsmiScreen"].Visible = false;
                    this.tsbMonitor.DropDownItems["tsmiScreen"].Tag = false;
                }
                if (myRoleAndRight.rights_of_view_agent_group_info || myRoleAndRight.rights_of_view_queue_info)
                {
                    this.tsbMonitor.DropDownItems["tsmiMonitor"].Visible = true;
                    this.tsbMonitor.DropDownItems["tsmiMonitor"].Tag = true;
                }
                else
                {
                    this.tsbMonitor.DropDownItems["tsmiMonitor"].Visible = false;
                    this.tsbMonitor.DropDownItems["tsmiMonitor"].Tag = false;
                }
            }
            else
            {
                this.tsbMonitor.Visible = false;
                this.tsbMonitor.Tag = false;
            }
            this.controlsVisible.ListenVisible = (bool)this.tsbListen.Tag;
            this.controlsVisible.WhisperVisible = (bool)this.tsbWhisper.Tag;
            this.controlsVisible.BargeinVisible = (bool)this.tsbBargein.Tag;
            this.controlsVisible.ForceHangupVisible = (bool)this.tsbForceHangup.Tag;
            this.controlsVisible.CallOutVisible = (bool)this.tsbCallOut.Tag;
            this.controlsVisible.MonitorVisible = (bool)this.tsbMonitor.Tag;
            this.controlsVisible.ApproveVisible = (bool)this.tsbApprove.Tag;
            if (this.transferControlsVisible != null)
            {
                this.transferControlsVisible();
            }
            this.aboControlVisible(this.advanced);
            this.aboControlVisible(this.other);
        }

        private void OnEvent_Agent_Group_List(string agentID, Dictionary<string, string> agent_group_list, string reason, int retCode)
        {
            AgentBar.Log.Debug(string.Concat(new object[]
            {
                "enter OnEvent_Agent_Group_List.agentID:",
                agentID,
                ",reason:",
                reason,
                ",retCode:",
                retCode
            }));
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
                AgentBar.Log.Debug("OnEvent_Agent_Group_List has error!reason=" + reason);
            }
        }

        private void OnEvent_Get_Agents_Of_Queue(string agentID, string agent_list, string queue_num, string reason, int retCode)
        {
            AgentBar.Log.Debug(string.Concat(new object[]
            {
                "enter OnEvent_Get_Agents_Of_Queue.agentID:",
                agentID,
                ",agent_list:",
                agent_list,
                ",queue_num:",
                queue_num,
                ",reason:",
                reason,
                ",retCode:",
                retCode
            }));
            if (retCode == 0)
            {
                if (this.GetAgentsOfQueueEvent != null)
                {
                    this.GetAgentsOfQueueEvent(agent_list, queue_num);
                }
                if (!this.DoGetAgentsMonitorInfo(agent_list))
                {
                    AgentBar.Log.Debug("DoGetAgentsMonitorInfo has error! agent_list=" + agent_list);
                }
            }
            else
            {
                AgentBar.Log.Debug("OnEvent_Get_Agents_Of_Queue has error!reason=" + reason);
            }
        }

        private void OnEvent_Get_Agents_Of_AgentGroup(string agentID, string agents_str, string agent_group_num, string reason, int retCode)
        {
            AgentBar.Log.Debug(string.Concat(new object[]
            {
                "enter OnEvent_Get_Agents_Of_AgentGroup.agentID:",
                agentID,
                ",agents_str:",
                agents_str,
                ",agent_group_num:",
                agent_group_num,
                ",reason:",
                reason,
                ",retCode:",
                retCode
            }));
            if (retCode == 0)
            {
                if (this.GetAgentsOfAgentGroupEvent != null)
                {
                    this.GetAgentsOfAgentGroupEvent(agents_str, agent_group_num);
                }
                if (!this.DoGetAgentsMonitorInfo(agents_str))
                {
                    AgentBar.Log.Debug("DoGetAgentsMonitorInfo has error! agents_str=" + agents_str);
                }
            }
            else
            {
                AgentBar.Log.Debug("OnEvent_Get_Agents_Of_AgentGroup has error!reason=" + reason);
            }
        }

        private void OnEvent_Get_Agents_Monitor_info(string agentID, List<Agent_Online_Struct> agent_monitor_info, string current_time, string reason, int retCode)
        {
            AgentBar.Log.Debug(string.Concat(new object[]
            {
                "enter OnEvent_Get_Agents_Monitor_info.agentID:",
                agentID,
                ",current_time:",
                current_time,
                ",reason:",
                reason,
                ",retCode:",
                retCode
            }));
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
                AgentBar.Log.Debug("OnEvent_Get_Agents_Monitor_info has error!reason=" + reason);
            }
        }

        private void OnEvent_Get_Detail_Call_info(string agentID, string targetAgentNum, string callType, List<Leg_Info_Struct> leg_info, List<Relation_Info_Struct> relation_info, string reason, int retCode)
        {
            AgentBar.Log.Debug(string.Concat(new object[]
            {
                "enter OnEvent_Get_Detail_Call_info .agentID:",
                agentID,
                ",targetAgentNum:",
                targetAgentNum,
                ",callType:",
                targetAgentNum,
                ",reason:",
                reason,
                ",retCode:",
                retCode
            }));
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
            AgentBar.Log.Debug(string.Concat(new object[]
            {
                "enter OnEvent_Get_Customer_Of_Queue .agentID:",
                agentID,
                ",queueNumLstStr:",
                queueNumLstStr,
                ",current_time:",
                current_time,
                ",reason:",
                reason,
                ",retCode:",
                retCode
            }));
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
                AgentBar.Log.Debug("OnEvent_Get_Customer_Of_Queue has error!reason=" + reason);
            }
        }

        private void OnEvent_Get_Customer_Of_My_Queue(string agentID, string current_time, string queueNumLstStr, List<Customer_Info_Struct> customer_list, string reason, int retCode)
        {
            AgentBar.Log.Debug(string.Concat(new object[]
            {
                "enter OnEvent_Get_Customer_Of_My_Queue .agentID:",
                agentID,
                ",current_time:",
                current_time,
                ",reason:",
                reason,
                ",retCode:",
                retCode
            }));
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
                AgentBar.Log.Debug("OnEvent_Get_Customer_Of_My_Queue has error!reason=" + reason);
            }
        }

        private void OnEvent_Get_Queue_Statis_Info(string agentID, string current_time, string queueNumLstStr, List<Queue_Statis_Struct> queue_statis_list, string reason, int retCode)
        {
            AgentBar.Log.Debug(string.Concat(new object[]
            {
                "enter OnEvent_Get_Queue_Statis_Info .agentID:",
                agentID,
                ",queueNumLstStr:",
                queueNumLstStr,
                ",reason:",
                reason,
                ",retCode:",
                retCode
            }));
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
                AgentBar.Log.Debug("OnEvent_Get_Queue_Statis_Info has error!reason=" + reason);
            }
        }

        private void OnEvent_Get_All_Queue_Statis_Info(string agentID, string current_time, List<Queue_Statis_Struct> queue_statis_list, string reason, int retCode)
        {
            AgentBar.Log.Debug(string.Concat(new object[]
            {
                "enter OnEvent_Get_All_Queue_Statis_Info .agentID:",
                agentID,
                ",reason:",
                reason,
                ",retCode:",
                retCode
            }));
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
                AgentBar.Log.Debug("OnEvent_Get_All_Queue_Statis_Info has error!reason=" + reason);
            }
        }

        private void OnEvent_Add_Customer_To_Queue(string agentID, string callType, string callcenter_num, string customer_num, string customer_status, string customer_type, string customer_uuid, string enter_queue_time, string exclusive_agent_num, string exclusive_queue_num, string queue_num, string queue_name, string queue_customer_amount, string early_queue_enter_time, string early_queue_enter_time_all, string customer_enter_channel)
        {
            AgentBar.Log.Debug(string.Concat(new string[]
            {
                "enter OnEvent_Add_Customer_To_Queue .agentID:",
                agentID,
                ",callType:",
                callType,
                ",callcenter_num:",
                callcenter_num,
                ",customer_num:",
                customer_num,
                ",customer_status:",
                customer_status,
                ",customer_type:",
                customer_type,
                ",customer_uuid:",
                customer_uuid,
                ",enter_queue_time:",
                enter_queue_time,
                ",exclusive_agent_num:",
                exclusive_agent_num,
                ",exclusive_queue_num:",
                exclusive_queue_num,
                ",queue_num:",
                queue_num,
                ",queue_name:",
                queue_name,
                ",queue_customer_amount:",
                queue_customer_amount,
                ",early_queue_enter_time:",
                early_queue_enter_time,
                ",early_queue_enter_time_all:",
                early_queue_enter_time_all,
                ",customer_enter_channel:",
                customer_enter_channel
            }));
            if (this.AddCustomerToQueueEvent != null)
            {
                this.AddCustomerToQueueEvent(callType, callcenter_num, customer_num, customer_status, customer_type, customer_uuid, enter_queue_time, exclusive_agent_num, exclusive_queue_num, queue_num, enter_queue_time, queue_name, queue_customer_amount, early_queue_enter_time, early_queue_enter_time_all, customer_enter_channel);
            }
        }

        private void OnEvent_Update_Customer_Of_Queue(string agentID, string callType, string callcenter_num, string customer_num, string customer_status, string customer_type, string customer_uuid, string enter_queue_time, string exclusive_agent_num, string exclusive_queue_num, string queue_num)
        {
            AgentBar.Log.Debug(string.Concat(new string[]
            {
                "enter OnEvent_Update_Customer_Of_Queue .agentID:",
                agentID,
                ",callType:",
                callType,
                ",callcenter_num:",
                callcenter_num,
                ",customer_num:",
                customer_num,
                ",customer_status:",
                customer_status,
                ",customer_type:",
                customer_type,
                ",customer_uuid:",
                customer_uuid,
                ",enter_queue_time:",
                enter_queue_time,
                ",queue_num:",
                queue_num
            }));
            if (this.UpdateCustomerOfQueueEvent != null)
            {
                this.UpdateCustomerOfQueueEvent(callType, callcenter_num, customer_num, customer_status, customer_type, customer_uuid, enter_queue_time, exclusive_agent_num, exclusive_queue_num, queue_num);
            }
        }

        private void OnEvent_Del_Customer_From_Queue(string agentID, string customer_uuid, string queue_num, string current_time, string early_queue_enter_time, string early_queue_enter_time_all, string reason)
        {
            AgentBar.Log.Debug(string.Concat(new string[]
            {
                "enter OnEvent_Update_Customer_Of_Queue .agentID:",
                agentID,
                ",customer_uuid:",
                customer_uuid,
                ",reason:",
                reason,
                ",queue_num:",
                queue_num
            }));
            if (this.DelCustomerFromQueueEvent != null)
            {
                this.DelCustomerFromQueueEvent(customer_uuid, queue_num, current_time, early_queue_enter_time, early_queue_enter_time_all, reason);
            }
        }

        private void OnEvent_Threewayee_Hangup(string agentID, string hangupReason)
        {
            AgentBar.Log.Debug("enter OnEvent_Threewayee_Hangup .agentID:" + agentID + ",hangupReason:" + hangupReason);
            string HangupReason = this.ThreeWayHangupReason2Chinese(hangupReason);
            if (HangupReason.Length > 8)
            {
                HangupReason = "三方失败: ...";
                this.tslStatus.ToolTipText = this.ThreeWayHangupReason2Chinese(hangupReason);
            }
            else
            {
                this.tslStatus.ToolTipText = "";
            }
            this.update_Toolbar_UI(AgentBar.Event_Type.THREEWAYEE_HANGUP, HangupReason);
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
            AgentBar.Log.Debug("enter OnEvent_Consultee_Hangup .agentID:" + agentID + ",hangupReason:" + hangupReason);
            string HangupReason = this.ConsulteeHangupReason2Chinese(hangupReason);
            if (HangupReason.Length > 8)
            {
                HangupReason = "咨询失败: ...";
                this.tslStatus.ToolTipText = this.ConsulteeHangupReason2Chinese(hangupReason);
            }
            else
            {
                this.tslStatus.ToolTipText = "";
            }
            this.update_Toolbar_UI(AgentBar.Event_Type.CONSULTEE_HANGUP, HangupReason);
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
            AgentBar.Log.Debug("enter OnEvent_Warn_Agent_Resignin .agentID:" + agentID);
            if (this.WarnAgentResigninEvent != null)
            {
                this.WarnAgentResigninEvent();
            }
        }

        private void OnEvent_Warn_Agent_Force_Change_Status(string agentID, string executorAgentID)
        {
            AgentBar.Log.Debug("enter OnEvent_Warn_Agent_Force_Change_Status .agentID:" + agentID + ".executorAgentID" + executorAgentID);
            if (this.WarnAgentForceChangeStatusEvent != null)
            {
                this.WarnAgentForceChangeStatusEvent(executorAgentID);
            }
        }

        private void OnEvent_Blind_Transfer_Outbound_Failed(string agentID, string hangupReason)
        {
            AgentBar.Log.Debug("enter OnEvent_Blind_Transfer_Outbound_Failed .agentID:" + agentID);
            string failedReason = "转接失败";
            if ("ALL_GATEWAYS_FULL" == hangupReason)
            {
                failedReason += "：外线满线";
                if (this.AllGetwaysFullEvent != null)
                {
                    this.AllGetwaysFullEvent();
                }
            }
            this.update_Toolbar_UI(AgentBar.Event_Type.BLIND_TRANSFER_OUTBOUND_FAILED, failedReason);
        }

        private void OnEvent_Force_Hangup_Ring(string agentID, string agent_call_uuid, string callerID, string calledID, string accessNumName, string desAgentID, string callType, string areaID, string areaName, string custGrade, string outExtraParamsFromIvr, string todayDate, string customer_num_format_local, string customer_num_format_national, string customer_num_format_e164, string customer_num_phone_type, string customerForeignId, string relation_uuid)
        {
            AgentBar.Log.Debug(string.Concat(new string[]
            {
                "enter OnEvent_Force_Hangup_Ring .agentID:",
                agentID,
                ",agent_call_uuid:",
                agent_call_uuid,
                ",callerID:",
                callerID,
                ",calledID:",
                calledID,
                ",accessNumName:",
                accessNumName,
                ",desAgentID:",
                desAgentID,
                ",callType:",
                callType,
                ",areaID:",
                areaID,
                ",areaName:",
                areaName,
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
                ",relation_uuid:",
                relation_uuid
            }));
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
            AgentBar.Log.Debug("enter OnEvent_HangupResult .reason:" + reason);
            if (retCode != 0)
            {
                string Reason = this.HangupFailReason2Chinese(reason);
                if (Reason.Length > 8)
                {
                    Reason = "挂断失败: ...";
                    this.tslStatus.ToolTipText = this.HangupFailReason2Chinese(reason);
                }
                else
                {
                    this.tslStatus.ToolTipText = "";
                }
                MessageBox.Show("reason :" + reason);
                this.update_Toolbar_UI(AgentBar.Event_Type.HANGUP_CALL_FAIL, Reason);
            }
        }

        private void OnUserStateChange(string userState, string strReason)
        {
            this.tsbCallAgent.Enabled = false;
            this.tsbHangUp.Enabled = true;
        }

        private void OnHeartBeat()
        {
            this.HeartBeatTimes = 0;
        }

        ~AgentBar()
        {
            this.agentDll.Dispose();
            this.Dispose(false);
        }

        private void OnEvent_Answer(string agent_call_uuid, string relation_uuid)
        {
            AgentBar.Log.Debug("enter OnEvent_Answer .");
            this.update_Toolbar_UI(AgentBar.Event_Type.ANSWER_CALL_PEER, "");
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
            AgentBar.Log.Debug("enter OnEvent_Callee_Answer .");
            this.update_Toolbar_UI(AgentBar.Event_Type.ANSWER_CALL_CALLEE, "");
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
            AgentBar.Log.Debug("enter OnEvent_Bridge .");
            this.update_Toolbar_UI(AgentBar.Event_Type.BRIDGE_CALL_PEER, "");
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
            AgentBar.Log.Debug("enter OnEvent_Get_Agent_Personal_info .");
            if (this.GetAgentPersonalInfoEvent != null)
            {
                this.GetAgentPersonalInfoEvent(AgentID, agent_mobile, agent_email);
            }
        }

        private void OnEvent_Set_Agent_Personal_info(string AgentID, int retCode, string reason)
        {
            AgentBar.Log.Debug("enter OnEvent_Set_Agent_Personal_info .");
            if (this.SetAgentPersonalInfoEvent != null)
            {
                this.SetAgentPersonalInfoEvent(AgentID, retCode, reason);
            }
        }

        private void OnEvent_Change_Pswd(string AgentID, int retCode, string reason)
        {
            AgentBar.Log.Debug("enter OnEvent_Change_Pswd .");
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
            AgentBar.Log.Debug("enter OnEvent_Apply_Change_Status .");
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
            AgentBar.Log.Debug("enter OnEvent_Approve_Change_Status_Distribute .");
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
            AgentBar.Log.Debug("enter OnEvent_Apply_Change_Status_Cancel_Distribute .");
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
            AgentBar.Log.Debug(string.Concat(new string[]
            {
                "enter OnEvent_RecordStart AgentExten:",
                AgentExten,
                ",AgentID:",
                AgentID,
                ",Agent_call_uuid:",
                Agent_call_uuid,
                ",CalleeNum:",
                CalleeNum,
                ",CallerNum:",
                CallerNum,
                ",FilePath:",
                FilePath,
                ",relation_uuid:",
                relation_uuid
            }));
            string makeStr = string.Empty;
            makeStr = "#relation_uuid=" + relation_uuid + "#";
            if (this.RecordStart != null)
            {
                this.RecordStart(AgentExten, AgentID, Agent_call_uuid, CalleeNum, CallerNum, FilePath, makeStr);
            }
        }

        private void OnEvent_RecordStop(string AgentID, string Agent_call_uuid, string FilePath, string relation_uuid)
        {
            AgentBar.Log.Debug(string.Concat(new string[]
            {
                "enter OnEvent_RecordStart AgentID:",
                AgentID,
                ",Agent_call_uuid:",
                Agent_call_uuid,
                ",FilePath:",
                FilePath,
                ",relation_uuid:",
                relation_uuid
            }));
            string makeStr = string.Empty;
            makeStr = "#relation_uuid=" + relation_uuid + "#";
            if (this.RecordStop != null)
            {
                this.RecordStop(AgentID, Agent_call_uuid, FilePath, makeStr);
            }
        }

        private void OnEvent_Evaluate_Result(string agentID, string agent_call_uuid, string agentExten, string customerUuid, string agent_group_name, string callerNum, string evaluateScore, string evaluateStatus, string queue_num)
        {
            AgentBar.Log.Debug(string.Concat(new string[]
            {
                "enter OnEvent_Evaluate_Result AgentID:",
                this.AgentID,
                ",Agent_call_uuid:",
                agent_call_uuid,
                ",agentExten:",
                agentExten,
                ",customerUuid:",
                customerUuid,
                ",agent_group_name:",
                agent_group_name,
                ",callerNum:",
                callerNum,
                ",evaluateScore:",
                evaluateScore,
                ",evaluateStatus:",
                evaluateStatus,
                ",queue_num:",
                queue_num
            }));
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

        private void OnEvent_Set_Agentgroup_Status_Max_Num(string agentID, string agent_group_name, string targetStatus, int retCode, string reason)
        {
            if (retCode == 0)
            {
                MessageBox.Show("设置成功！", "设置坐席组最大阀值", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            else
            {
                MessageBox.Show("设置失败！", "设置坐席组最大阀值", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
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

        private void OnSockDisconnectEvent(string reason, int retCode)
        {
            AgentBar.Log.Debug(string.Concat(new object[]
            {
                "enter OnSockDisconnectEvent .reason:",
                reason,
                ",retCode:",
                retCode
            }));
            if (this.newFrmMonitor != null)
            {
                this.newFrmMonitor.Close();
            }
            if (this.newFrmMonitorScreen != null)
            {
                this.newFrmMonitorScreen.Close();
            }
            this.DoDisconnect();
            this.Sys_LogOff();
            this.update_Toolbar_UI(AgentBar.Event_Type.DISCONNECT_SOCKET, "");
            this.mAgentStatus = Convert.ToString(-1);
        }

        private void OnResponseTimeOutEvent(string reason, int retCode)
        {
            AgentBar.Log.Debug(string.Concat(new object[]
            {
                "enter OnResponseTimeOutEvent .reason:",
                reason,
                ",retCode:",
                retCode
            }));
            this.update_Toolbar_UI(AgentBar.Event_Type.RESPONSE_TIMEOUT, "");
        }

        private void OnKickOutEvent()
        {
            AgentBar.Log.Debug("enter OnKickOutEvent .");
            this.update_Toolbar_UI(AgentBar.Event_Type.KICK_OUT, "");
        }

        private void OnEvent_CheckExten(string agentID, string agent_call_uuid, string relation_uuid)
        {
            AgentBar.Log.Debug(string.Concat(new string[]
            {
                "enter OnEvent_CheckExten .agentID:",
                agentID,
                ",agent_call_uuid:",
                agent_call_uuid,
                ",relation_uuid:",
                relation_uuid
            }));
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
            this.update_Toolbar_UI(AgentBar.Event_Type.ECHO_TEST_SUCCESS, "");
            if (this.CheckExtenEvent != null)
            {
                this.CheckExtenEvent(agentID, makeStr);
            }
        }

        private void OnResponse(string EventType, string AgentID, int retCode, string strReason)
        {
            AgentBar.Log.Debug(string.Concat(new object[]
            {
                "enter OnResponse .AgentID:",
                AgentID,
                ",retCode:",
                retCode,
                ",EventType:",
                EventType,
                ",strReason:",
                strReason
            }));
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

        private void chkIfHaveApplyChangeStatus()
        {
            if (this.my_apply_change_status.isFinished)
            {
                if (this.newFrmMessageBox == null || this.newFrmMessageBox.IsDisposed)
                {
                    this.newFrmMessageBox = new FrmMessageBox();
                }
                this.newFrmMessageBox.agentbar1 = this;
                this.newFrmMessageBox.Text = "申请离开";
                this.newFrmMessageBox.lblMsg.Text = "已达到系统离开阀值，是否要申请离开？";
                this.newFrmMessageBox.Show();
                this.newFrmMessageBox.Activate();
            }
            else
            {
                MessageBox.Show("已申请的数量已达到允许的最大阀值，请稍后再试！", "离开申请", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void load_default_status()
        {
            AgentBar.Log.Debug("enter load_default_status . ");
            this.tsbState.DropDownItems.Clear();
            ToolStripMenuItem newItem = new ToolStripMenuItem();
            newItem.AccessibleName = Convert.ToString(0);
            newItem.Image = Resources.idle;
            newItem.Text = "空闲";
            if (!string.IsNullOrEmpty(this.mHotKeyIdle))
            {
                newItem.Text = newItem.Text + "        " + this.mHotKeyIdle;
            }
            this.tsbState.DropDownItems.Add(newItem);
            newItem = new ToolStripMenuItem();
            newItem.AccessibleName = Convert.ToString(6);
            newItem.Image = Resources.busy;
            newItem.Text = "忙碌";
            if (!string.IsNullOrEmpty(this.mHotKeyBusy))
            {
                newItem.Text = newItem.Text + "        " + this.mHotKeyBusy;
            }
            this.tsbState.DropDownItems.Add(newItem);
            newItem = new ToolStripMenuItem();
            newItem.AccessibleName = Convert.ToString(7);
            newItem.Image = Resources.leave;
            newItem.Text = "离开";
            if (!string.IsNullOrEmpty(this.mHotKeyLeave))
            {
                newItem.Text = newItem.Text + "        " + this.mHotKeyLeave;
            }
            this.tsbState.DropDownItems.Add(newItem);
            newItem = new ToolStripMenuItem();
            newItem.AccessibleName = Convert.ToString(8);
            newItem.Image = Resources.talk;
            newItem.Text = "手动外呼中";
            if (!string.IsNullOrEmpty(this.mHotKeyCallOut))
            {
                newItem.Text = newItem.Text + "  " + this.mHotKeyCallOut;
            }
            this.tsbState.DropDownItems.Add(newItem);
            if (this.mMyRoleAndRight.rights_of_view_agent_group_info || this.mMyRoleAndRight.rights_of_view_queue_info || this.mMyRoleAndRight.role_right1 || this.mMyRoleAndRight.rights_of_eavesdrop || this.mMyRoleAndRight.rights_of_bargein || this.mMyRoleAndRight.rights_of_whisper || this.mMyRoleAndRight.rights_of_forcehangup)
            {
                newItem = new ToolStripMenuItem();
                newItem.AccessibleName = Convert.ToString(9);
                newItem.Image = Resources.manage;
                newItem.Text = "监控";
                if (!string.IsNullOrEmpty(this.mHotKeyMonitor))
                {
                    newItem.Text = newItem.Text + "        " + this.mHotKeyMonitor;
                }
                if (newItem.Text != "")
                {
                    this.tsbState.DropDownItems.Add(newItem);
                }
            }
        }

        private void OnEvent_GetDefineStatus(string AgentID, int retCode, string strReason, Dictionary<string, string> define_status_dic)
        {
            AgentBar.Log.Debug(string.Concat(new object[]
            {
                "enter OnEvent_GetDefineStatus .AgentID:",
                AgentID,
                ",retCode:",
                retCode,
                ",strReason:",
                strReason
            }));
            if (0 == retCode)
            {
                this.mAgentDefineStatus = new Dictionary<string, string>();
                this.mAgentDefineStatus.Clear();
                this.mAgentDefineStatus = define_status_dic;
                int i = this.tsbState.DropDownItems.Count;
                while (i > 6)
                {
                    this.tsbState.DropDownItems.RemoveAt(--i);
                }
                foreach (KeyValuePair<string, string> dic in define_status_dic)
                {
                    ToolStripMenuItem newItem = new ToolStripMenuItem();
                    newItem.AccessibleName = dic.Key;
                    newItem.Image = Resources.busy;
                    newItem.Text = dic.Value;
                    this.tsbState.DropDownItems.Add(newItem);
                }
            }
        }

        private void OnEvent_GetWebSiteInfo(string AgentID, int retCode, string strReason, Dictionary<string, string> website_info_dic)
        {
            AgentBar.Log.Debug(string.Concat(new object[]
            {
                "enter OnEvent_GetWebSiteInfo .AgentID:",
                AgentID,
                ",retCode:",
                retCode,
                ",strReason:",
                strReason
            }));
            if (0 == retCode)
            {
                if (this.GetWebSiteInfoEvent != null)
                {
                    this.GetWebSiteInfoEvent(AgentID, retCode, strReason, website_info_dic);
                }
                if (this.JSGetWebSiteInfoEvent != null)
                {
                    string[] strArgsLst = new string[]
                    {
                        "AgentID",
                        "retCode",
                        "strReason",
                        "website_info_dic"
                    };
                    this.ProcessAgentBarJsonDelegate(new object[]
                    {
                        this.JSGetWebSiteInfoEvent,
                        strArgsLst,
                        AgentID,
                        retCode,
                        strReason,
                        website_info_dic
                    });
                }
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
            AgentBar.Log.Debug(string.Concat(new string[]
            {
                "enter OnEvent_AgentStatusChange .AgentID:",
                AgentID,
                ",status_change_agentid:",
                status_change_agentid,
                ",status_change_before:",
                status_change_before,
                ",status_change_after:",
                status_change_after,
                ",customer_enter_channel:",
                customer_enter_channel
            }));
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
                if (i >= this.tsbState.DropDownItems.Count)
                {
                    switch (AgentBar.Str2AgentStatus(status_change_after))
                    {
                        case AgentBar.Agent_Status.AGENT_STATUS_OFFLINE:
                            this.tsbState.Text = "离线";
                            goto IL_477;
                        case AgentBar.Agent_Status.AGENT_STATUS_RING:
                            this.update_Toolbar_UI(AgentBar.Event_Type.AGENT_STATUS_CHANGE_TO_RING, "");
                            goto IL_477;
                        case AgentBar.Agent_Status.AGENT_STATUS_TALKING:
                            this.update_Toolbar_UI(AgentBar.Event_Type.AGENT_STATUS_CHANGE_TO_TALKING, "");
                            goto IL_477;
                        case AgentBar.Agent_Status.AGENT_STATUS_HOLD:
                            this.update_Toolbar_UI(AgentBar.Event_Type.AGENT_STATUS_CHANGE_TO_HOLD, "");
                            goto IL_477;
                        case AgentBar.Agent_Status.AGENT_STATUS_ACW:
                            if (this.mAgentStateAfterHangup == AgentBar.Agent_Status.AGENT_STATUS_IDLE)
                            {
                                if (!this.DoSetAgentDefineStatus(0, 0))
                                {
                                    this.tslStatus.Text = "置为空闲失败";
                                    this.tslStatus.ToolTipText = "";
                                }
                            }
                            else if (this.mAgentStateAfterHangup == AgentBar.Agent_Status.AGENT_STATUS_RESTORE)
                            {
                                if (!this.DoSetAgentDefineStatus((int)this.mAgentStateBeforeCallinOrCallout, 0))
                                {
                                    this.tslStatus.Text = "恢复原状态失败";
                                    this.tslStatus.ToolTipText = "";
                                }
                            }
                            this.update_Toolbar_UI(AgentBar.Event_Type.AGENT_STATUS_CHANGE_TO_ACW, "");
                            goto IL_477;
                        case AgentBar.Agent_Status.AGENT_STATUS_CAMP_ON:
                            this.update_Toolbar_UI(AgentBar.Event_Type.AGENT_STATUS_CHANGE_TO_CAMP_ON, "");
                            goto IL_477;
                        case AgentBar.Agent_Status.AGENT_STATUS_CALLING_OUT:
                            this.update_Toolbar_UI(AgentBar.Event_Type.AGENT_STATUS_CHANGE_TO_CALLING_OUT, "");
                            goto IL_477;
                        case AgentBar.Agent_Status.AGENT_STATUS_MUTE:
                            this.update_Toolbar_UI(AgentBar.Event_Type.AGENT_STATUS_CHANGE_TO_MUTE, "");
                            goto IL_477;
                    }
                    this.tsbState.Text = "未知状态:" + status_change_after;
                    IL_477:;
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
            AgentBar.Log.Debug(string.Concat(new string[]
            {
                "enter OnEvent_CallOutRing .AgentID:",
                this.AgentID,
                ",agent_call_uuid:",
                agent_call_uuid,
                ",calledID:",
                calledID,
                ",accessNumName:",
                accessNumName,
                ",callType:",
                callType,
                ",areaID:",
                areaID,
                ",areaName:",
                areaName,
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
                ",relation_uuid:",
                relation_uuid
            }));
            this.mCallType = AgentBar.Call_Type.MANUAL_CALL_OUT;
            this.mCallStatus = AgentBar.CallStatus.CALL_OUT;
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
            this.update_Toolbar_UI(AgentBar.Event_Type.CALLOUT_RING_MYSELF, "");
            if (this.CallOutRingEvent != null)
            {
                this.CallOutRingEvent(callerID, calledID, accessNumName, callType, areaID, areaName, makeStr);
            }
            if ((this.SoftPhoneEnable2 || this.bindSoftPhoneLogin) && this.mSoftphoneAutoAnswer)
            {
                if (!this.DoAnswer())
                {
                    MessageBox.Show("接听失败！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                }
            }
        }

        private void OnEvent_Manual_Callout(string agentID, string reason, int retCode)
        {
            AgentBar.Log.Debug(string.Concat(new object[]
            {
                "enter OnEvent_Manual_Callout .AgentID:",
                this.AgentID,
                ",reason:",
                reason,
                ",retCode:",
                retCode
            }));
            if (retCode == 0)
            {
                this.update_Toolbar_UI(AgentBar.Event_Type.MANUAL_CALLOUT_SUCCESS, "");
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
                this.update_Toolbar_UI(AgentBar.Event_Type.MANUAL_CALLOUT_FAIL, failedReason);
            }
        }

        private void OnEvent_SignIn(string AgentID, string AgentName, string AgentExten, string AgentGroupID, string AgentGroupName, bool AutoPostTreatment, bool BindExten, bool GradeSwitch, string InitStatus, string RoleID, int retCode, string strReason, int heartbeat_timeout, string SaltKey, string DID_Num)
        {
            AgentBar.Log.Debug(string.Concat(new object[]
            {
                "enter OnEvent_SignIn .AgentID:",
                AgentID,
                ",AgentName:",
                AgentName,
                ",AgentExten:",
                AgentExten,
                ",AgentGroupID:",
                AgentGroupID,
                ",AgentGroupName:",
                AgentGroupName,
                ",AutoPostTreatment:",
                AutoPostTreatment,
                ",BindExten:",
                BindExten,
                ",GradeSwitch:",
                GradeSwitch,
                ",InitStatus:",
                InitStatus,
                ",RoleID:",
                RoleID,
                ",retCode:",
                retCode,
                ",strReason:",
                strReason,
                ",heartbeat_timeout:",
                heartbeat_timeout,
                ", SaltKey:",
                SaltKey,
                ", DID_Num:",
                DID_Num
            }));
            if (0 == retCode)
            {
                this.mAgentState = AgentBar.Agent_State.AGENT_STATUS_SIGNIN;
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
                    AgentBar.Log.Error("heartbeat is invalid,we will use default value!value=" + this.HeartBeatTimeout);
                }
                this.update_Toolbar_UI(AgentBar.Event_Type.SIGNIN_SUCCESS, "");
                if (AgentExten != "")
                {
                    this.tslAgentInfo.Text = "坐席号:" + AgentID + " 分机号:" + AgentExten;
                }
                else
                {
                    this.tslAgentInfo.Text = "坐席号:" + AgentID;
                }
                this.BeginTheTimer();
                if (!this.DoGetAgentWebSiteInfo())
                {
                    AgentBar.Log.Debug("获取坐席网址失败！");
                }
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
                this.update_Toolbar_UI(AgentBar.Event_Type.SIGNIN_FAIL, "");
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
            AgentBar.Log.Debug(string.Concat(new object[]
            {
                "enter OnEvent_SignOut .AgentID:",
                this.AgentID,
                ",reason:",
                reason,
                ",retCode:",
                retCode
            }));
            this.mAgentState = AgentBar.Agent_State.AGENT_STATUS_OFFLINE;
            this.mCallStatus = AgentBar.CallStatus.NO_CALL;
            this.agentTool.Enabled = false;
            this.update_Toolbar_UI(AgentBar.Event_Type.SIGNOUT_SUCCESS, "");
            this.Sys_LogOff();
            if (this.mSipAutoSignIn)
            {
                this.softPhone_close();
            }
            if (this.SignOutEvent != null)
            {
                this.SignOutEvent(this.AgentID, retCode, reason);
            }
        }

        private void Sys_LogOff()
        {
            this.mSoftPhoneWindowHandle = IntPtr.Zero;
            this.blnWhisper = false;
            this.blnListen = false;
            this.blnBargein = false;
            this.blnConnect = false;
            this.blnHold = false;
            this.blnMute = false;
            this.blnSignIn = false;
            this.blnTalking = false;
            try
            {
                this.tmrHeartBeat.Dispose();
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

        private void OnEvent_KickOut(string agentID, int retCode, string reason)
        {
            AgentBar.Log.Debug(string.Concat(new object[]
            {
                "enter OnEvent_KickOut .AgentID:",
                this.AgentID,
                ",reason:",
                reason,
                ",retCode:",
                retCode
            }));
            this.agentTool.Enabled = false;
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
            AgentBar.Log.Error(string.Concat(new object[]
            {
                "enter OnEventResultEvent.EventType:",
                EventType,
                ",agentID",
                agentID,
                ",retCode:",
                retCode,
                ",strReason",
                strReason,
                ",hangupReason",
                hangupReason
            }));
            string text = EventType.ToLower();
            switch (text)
            {
                case "callout":
                    if (0 == retCode)
                    {
                        this.tsbCallAgent.Enabled = false;
                        this.tsbHangUp.Enabled = true;
                        this.tsbCheck.Enabled = false;
                        this.tslStatus.Text = "呼出成功";
                        this.tslStatus.ToolTipText = "";
                    }
                    else
                    {
                        this.tsbCallAgent.Enabled = true;
                        this.tsbHangUp.Enabled = false;
                        this.tsbCheck.Enabled = true;
                        this.tslStatus.Text = "呼出失败";
                        this.tslStatus.ToolTipText = "";
                    }
                    break;
                case "offline":
                    if (0 == retCode)
                    {
                        MessageBox.Show("检测到您的分机已经下线，请重新签入！", "下线提醒", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                        this.tslStatus.Text = "分机下线";
                        this.tslStatus.ToolTipText = "";
                        this.agentTool.Enabled = false;
                    }
                    else
                    {
                        this.agentTool.Enabled = true;
                        this.tslStatus.Text = "下线失败";
                        this.tslStatus.ToolTipText = "";
                    }
                    break;
                case "answer":
                    if (0 == retCode)
                    {
                        this.tsbCallAgent.Enabled = false;
                        this.tsbHangUp.Enabled = true;
                        this.tsbCheck.Enabled = false;
                        this.tslStatus.Text = "接听电话";
                        this.tslStatus.ToolTipText = "";
                    }
                    else
                    {
                        this.tsbCallAgent.Enabled = true;
                        this.tsbHangUp.Enabled = false;
                        this.tsbCheck.Enabled = true;
                        this.tslStatus.Text = "接听失败";
                        this.tslStatus.ToolTipText = "";
                    }
                    break;
                case "bridge":
                    if (0 == retCode)
                    {
                        this.tsbCallAgent.Enabled = false;
                        this.tsbHangUp.Enabled = true;
                        this.tsbCheck.Enabled = false;
                        this.tslStatus.Text = "接通";
                        this.tslStatus.ToolTipText = "";
                    }
                    else
                    {
                        this.tsbCallAgent.Enabled = true;
                        this.tsbHangUp.Enabled = false;
                        this.tsbCheck.Enabled = true;
                        this.tslStatus.Text = "呼出失败";
                        this.tslStatus.ToolTipText = "";
                    }
                    break;
                case "hold":
                    if (0 == retCode)
                    {
                        this.tsbHold.Enabled = true;
                        this.tslStatus.Text = "保持成功";
                        this.tslStatus.ToolTipText = "";
                    }
                    else
                    {
                        this.tsbHold.Enabled = true;
                        this.tslStatus.Text = "保持失败";
                        this.tslStatus.ToolTipText = "";
                    }
                    break;
                case "stophold":
                    if (0 == retCode)
                    {
                        this.tsbHold.Enabled = true;
                        this.tslStatus.Text = "取消保持";
                        this.tslStatus.ToolTipText = "";
                    }
                    else
                    {
                        this.tsbHold.Enabled = true;
                        this.tslStatus.Text = "取消保持失败";
                        this.tslStatus.ToolTipText = "";
                    }
                    break;
                case "listen":
                    if (0 == retCode)
                    {
                        this.tsbListen.Enabled = false;
                        this.tsbHangUp.Enabled = true;
                        this.tslStatus.Text = "监听成功";
                        this.tslStatus.ToolTipText = "";
                    }
                    else
                    {
                        this.tsbListen.Enabled = true;
                        this.tsbHangUp.Enabled = false;
                        this.tslStatus.Text = "监听失败";
                        this.tslStatus.ToolTipText = "";
                    }
                    break;
                case "consult":
                    if (0 == retCode)
                    {
                        this.tsbConsult.Enabled = false;
                        this.tsbStopConsult.Enabled = true;
                        this.tsbConsult.Text = "咨询成功";
                    }
                    else
                    {
                        this.tsbConsult.Enabled = true;
                        this.tsbStopConsult.Enabled = false;
                        this.tslStatus.Text = "咨询失败";
                        this.tslStatus.ToolTipText = "";
                    }
                    break;
                case "stopconsult":
                    if (0 == retCode)
                    {
                        this.tsbConsult.Enabled = true;
                        this.tsbConsult.Text = "取消咨询成功";
                    }
                    else
                    {
                        this.tsbStopConsult.Enabled = true;
                        this.tslStatus.Text = "取消咨询失败";
                        this.tslStatus.ToolTipText = "";
                    }
                    break;
                case "intercept":
                    if (0 == retCode)
                    {
                        this.tsbThreeWay.Enabled = false;
                        this.tsbCallAgent.Enabled = false;
                        this.tsbHangUp.Enabled = true;
                        this.tsbConsult.Text = "拦截成功";
                    }
                    else
                    {
                        this.tsbThreeWay.Enabled = true;
                        this.tslStatus.Text = "拦截失败";
                        this.tslStatus.ToolTipText = "";
                    }
                    break;
                case "interrupt":
                    if (0 == retCode)
                    {
                        this.tsbCallAgent.Enabled = false;
                        this.tsbHangUp.Enabled = true;
                        this.tsbBargein.Enabled = true;
                        this.tsbConsult.Text = "插话成功";
                    }
                    else
                    {
                        this.tsbBargein.Enabled = true;
                        this.tslStatus.Text = "插话失败";
                        this.tslStatus.ToolTipText = "";
                    }
                    break;
                case "forcedisconnect":
                    if (0 == retCode)
                    {
                        this.tsbForceHangup.Enabled = false;
                        this.tsbCallAgent.Enabled = false;
                        this.tsbHangUp.Enabled = true;
                        this.tsbConsult.Text = "强拆成功";
                    }
                    else
                    {
                        this.tsbForceHangup.Enabled = true;
                        this.tslStatus.Text = "强拆失败";
                        this.tslStatus.ToolTipText = "";
                    }
                    break;
                case "transfer":
                    if (0 == retCode)
                    {
                        this.tsbTransfer.Enabled = true;
                        this.tsbCallAgent.Enabled = true;
                        this.tsbHangUp.Enabled = false;
                        this.tslStatus.Text = "转接成功";
                        this.tslStatus.ToolTipText = "";
                    }
                    else
                    {
                        this.tsbTransfer.Enabled = true;
                        this.tslStatus.Text = "转接失败";
                        this.tslStatus.ToolTipText = "";
                    }
                    break;
                case "grade":
                    if (0 == retCode)
                    {
                        this.tslStatus.Text = "评分成功";
                        this.tslStatus.ToolTipText = "";
                    }
                    else
                    {
                        this.tslStatus.Text = "评分失败";
                        this.tslStatus.ToolTipText = "";
                    }
                    break;
                case "manual_callout":
                    if (0 == retCode)
                    {
                        this.tsbCallOut.Enabled = false;
                        this.tslStatus.Text = "外呼成功";
                        this.tslStatus.ToolTipText = "";
                    }
                    else
                    {
                        this.tsbCallOut.Enabled = true;
                        this.tslStatus.Text = "外呼失败";
                        this.tslStatus.ToolTipText = "";
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
                case "apply_change_status":
                    if (0 == retCode)
                    {
                        this.tslStatus.Text = "申请成功";
                        this.tslStatus.ToolTipText = "";
                    }
                    else
                    {
                        this.tslStatus.Text = "申请失败";
                        this.tslStatus.ToolTipText = "";
                    }
                    break;
            }
        }

        private void OnSignOutEvent(string agentID, int retCode, string strReason)
        {
        }

        private string ForDefault()
        {
            return "此方法用于在前台调用,以表明前台页面可以用JS调用控件类中的方法!!!";
        }

        public bool DoSignIn()
        {
            AgentBar.Log.Debug("enter DoSignIn .");
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
            AgentBar.Log.Debug("enter DoSignInWithConnect .");
            bool result;
            if (this.mServerIP == "" || this.mPort == 0 || this.mServerIP == null)
            {
                AgentBar.Log.Debug("DoSignInWithConnect is failed!reason:mServerIP is empty or null or port is 0 ");
                result = false;
            }
            else if (this.mAgentID == "" || this.mAgentID == null || this.mAgentStatus == "")
            {
                AgentBar.Log.Debug("DoSignInWithConnect is failed!reason:mAgentID or mAgentStatus is empty.");
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
                AgentBar.Log.Debug("call PL_ConnectToCti.....");
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
                    AgentBar.Log.Debug("agentDll.PL_ConnectToCti is failed!!rt=" + rt);
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
                    AgentBar.Log.Debug("开始签入...");
                    rt = this.agentDll.PL_SignIn(this.mAgentID, this.mAgentPwd, this.mAgentExten, strBindExten, this.mAgentStatus, this.mWebUrl, strExtenIsOutbound);
                    if (0L != rt)
                    {
                        AgentBar.Log.Debug("agentDll.PL_SignIn is failed!so we will call agentDll.PL_DisConnectToCti()");
                        this.agentDll.PL_DisConnectToCti();
                        result = false;
                    }
                    else
                    {
                        AgentBar.Log.Debug("DoSignInWithConnect is success!!");
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
                    MessageBox.Show("启动内置软电话失败！  ", "启动内置软电话", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
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

        private bool start_process(string work_dir, string path, string args)
        {
            this.process_info = new ProcessStartInfo();
            this.process_info.WorkingDirectory = work_dir;
            this.process_info.FileName = path;
            this.process_info.Arguments = args;
            this.process_info.CreateNoWindow = true;
            this.process_info.UseShellExecute = true;
            bool result;
            try
            {
                Process.Start(this.process_info);
                result = true;
            }
            catch (Win32Exception we)
            {
                MessageBox.Show(this, we.Message);
                result = false;
            }
            return result;
        }

        public bool DoSignOut()
        {
            AgentBar.Log.Debug("enter DoSignOut .");
            bool result;
            if (this.mAgentID == null)
            {
                AgentBar.Log.Debug("DoSignOut failed!reason:mAgentID is NULL .");
                result = false;
            }
            else if (this.mAgentID == "")
            {
                AgentBar.Log.Debug("DoSignOut failed!reason:mAgentID is empty .");
                result = false;
            }
            else
            {
                long rt = this.agentDll.PL_SignOut(this.mAgentID);
                if (0L != rt)
                {
                    AgentBar.Log.Debug("DoSignOut failed!reason:call agentDll.PL_SignOut is failed .");
                    result = false;
                }
                else
                {
                    AgentBar.Log.Debug("DoSignOut is success!");
                    this.mCallStatus = AgentBar.CallStatus.NO_CALL;
                    result = true;
                }
            }
            return result;
        }

        public int DoConnect()
        {
            AgentBar.Log.Debug("enter DoConnect .");
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
                    AgentBar.Log.Debug("连接CTI服务器失败!");
                    result = -3;
                }
                else
                {
                    AgentBar.Log.Debug("连接CTI服务器成功!");
                    this.blnConnect = true;
                    result = 0;
                }
            }
            return result;
        }

        public bool DoDisconnect()
        {
            AgentBar.Log.Debug("enter DoDisconnect.");
            long rt = this.agentDll.PL_DisConnectToCti();
            if (0L != rt)
            {
                AgentBar.Log.Error("PL_DisConnectToCti is error!maybe it has disconnect already! retCode=" + rt);
            }
            this.blnConnect = false;
            this.mCallStatus = AgentBar.CallStatus.NO_CALL;
            return true;
        }

        public bool DoCallOut(string strCallID, string strDisplayNum)
        {
            AgentBar.Log.Debug("enter DoCallOut .strCallID:" + strCallID + ",strDisplayNum:" + strDisplayNum);
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
            AgentBar.Log.Debug(string.Concat(new string[]
            {
                "enter DoCallOutForeignId .strCallID:",
                strCallID,
                ",strDisplayNum:",
                strDisplayNum,
                ",customerForeignId:",
                customerForeignId
            }));
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
            AgentBar.Log.Debug("enter DoCallAgent .strAgentNum:" + strAgentNum);
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
                    MessageBox.Show("接听失败！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    this.tsbAnswer.Enabled = true;
                    result = false;
                    return result;
                }
                this.tsbAnswer.Enabled = false;
            }
            else
            {
                int rt = AgentBar.PostMsgToSoftPhone(this.mSoftPhoneWindowHandle, this.mSoftPhone_app_className, this.mSoftPhone_app_name, this.mSoftPhone_msg_value, this.mSoftPhone_answer_cmd, this.mSoftPhone_answer_cmd);
                if (rt != 0)
                {
                    AgentBar.Log.Error(string.Concat(new object[]
                    {
                        "PostMessageApi failed!handle:",
                        this.mSoftPhoneWindowHandle,
                        ",msg_value:",
                        this.mSoftPhone_msg_value,
                        ",wp:",
                        this.mSoftPhone_answer_cmd,
                        ",lp:",
                        this.mSoftPhone_answer_cmd
                    }));
                    MessageBox.Show("接听失败！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    this.tsbAnswer.Enabled = true;
                    result = false;
                    return result;
                }
                OperatingSystem os = Environment.OSVersion;
                if (os.Version.Major < 6)
                {
                    Thread.Sleep(100);
                    rt = AgentBar.PostMsgToSoftPhone(this.mSoftPhoneWindowHandle, this.mSoftPhone_app_className, this.mSoftPhone_app_name, this.mSoftPhone_msg_value, this.mSoftPhone_answer_cmd, this.mSoftPhone_answer_cmd);
                    this.tsbAnswer.Enabled = false;
                }
            }
            result = true;
            return result;
        }

        public bool DoHangUp()
        {
            AgentBar.Log.Debug("enter DoHangUp .");
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
            AgentBar.Log.Debug("enter DoGetAgentDefineStatus .");
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
            AgentBar.Log.Debug("enter DoGetAgentWebSiteInfo .");
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
            AgentBar.Log.Debug("enter DoHold .");
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
            AgentBar.Log.Debug("enter DoStopHold .");
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
            AgentBar.Log.Debug("enter DoMute .");
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
            AgentBar.Log.Debug("enter DoStopMute .");
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
            AgentBar.Log.Debug("enter DoConsult .strCuID:" + destAgentID);
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
            AgentBar.Log.Debug("enter DoStopConsult.");
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
            AgentBar.Log.Debug("enter DoConsultTransfer.");
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
            AgentBar.Log.Debug("enter DoListen.strAgentID:" + strAgentID);
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
            AgentBar.Log.Debug("enter DoWhisper.strAgentID:" + strAgentID);
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
            AgentBar.Log.Debug("enter DoForceChangeStatusEvent.strAgentID:" + strAgentID + ".status" + status);
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
            AgentBar.Log.Debug("enter DoTransferAgent.destAgentID:" + destAgentID);
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
            AgentBar.Log.Debug("enter DoTransferIvr.ivrNum:" + ivrNum);
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
            AgentBar.Log.Debug("enter DoTransferQueue.queueNum:" + queueNum);
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
            AgentBar.Log.Debug("enter DoTransferIvrProfile.ivrProfileNum:" + ivrProfileNum);
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
            AgentBar.Log.Debug(string.Concat(new object[]
            {
                "enter DoSetAgentDefineStatus.targetStatus:",
                targetStatus,
                ",needApproval=",
                IsNeedApproval
            }));
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
            AgentBar.Log.Debug("enter DoIntercept.strCuid:" + strCuid);
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
            AgentBar.Log.Debug("enter DoBargein.strAgentID:" + strAgentID);
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
            AgentBar.Log.Debug("enter DoForceHangup.strAgentID:" + strAgentID);
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
            AgentBar.Log.Debug("enter DoHeartBeat.");
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
            AgentBar.Log.Debug("enter DoCheck.");
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
            AgentBar.Log.Debug("enter DoGrade.language:" + language);
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
            AgentBar.Log.Debug("enter DoGetAccessNumbers.");
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
            AgentBar.Log.Debug(string.Concat(new object[]
            {
                "enter DoThreeWay.trdAgentNum:",
                trdAgentNum,
                ",trdIsOutbound:",
                trdIsOutbound
            }));
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
            AgentBar.Log.Debug("enter DoThreeWayCancel.");
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
            AgentBar.Log.Debug("enter DoGetOnlineAgent.");
            bool result;
            if (numType != 1 && numType != 2 && numType != 3)
            {
                AgentBar.Log.Error("DoGetOnlineAgent failed,numType is invalid.numType=" + numType);
                result = false;
            }
            else
            {
                if (numType == 1 || numType == 2)
                {
                    if (string.IsNullOrEmpty(specificNum))
                    {
                        AgentBar.Log.Error("DoGetOnlineAgent failed,specificNum is empty!");
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
            AgentBar.Log.Debug("enter DoGetIvrList.");
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
            AgentBar.Log.Debug("enter DoGetQueueList.");
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
            AgentBar.Log.Debug("enter DoGetIvrProfileList.");
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
            AgentBar.Log.Debug("enter DoGetDefinedRoleRights.");
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
            AgentBar.Log.Debug("enter DoGetAgentGroupList.");
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
            AgentBar.Log.Debug("enter DoGetAgentsOfQueue.strQueueNum:" + strQueueNum);
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
            AgentBar.Log.Debug("enter DoGetAgentsOfAgentGroup.strAgentGroupNum:" + strAgentGroupNum);
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
            AgentBar.Log.Debug("enter DoGetAgentsMonitorInfo.agentsStr:" + agentsStr);
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
            AgentBar.Log.Debug("enter DoGetDetailCallInfo.targetAgentNum:" + targetAgentNum);
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
            AgentBar.Log.Debug("enter DoGetCustomerOfQueue.queue_num:" + queue_num);
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
            AgentBar.Log.Debug("enter DoGetCustomerOfMyQueue.");
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
            AgentBar.Log.Debug("enter DoGetQueueStatis.queue_num:" + queue_num);
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
            AgentBar.Log.Debug("enter DoGetQueueStatis.");
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
            AgentBar.Log.Debug("enter DoApplyChangeStatus.");
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
            AgentBar.Log.Debug(string.Concat(new string[]
            {
                "enter DoApplyApproval.applyAgentID:",
                applyAgentID,
                "passFlag:",
                passFlag,
                ",targetStatus:",
                targetStatus
            }));
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
            AgentBar.Log.Debug("enter DoCancelApply. targetStatus:" + targetStatus);
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
            AgentBar.Log.Debug(string.Concat(new string[]
            {
                "enter DoApplyApproval.targetAgentGroupNameLstStr:",
                targetAgentGroupNameLstStr,
                ",targetAgentGroupIdLstStr:",
                targetAgentGroupIdLstStr,
                ",targetStatus:",
                targetStatus
            }));
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
            AgentBar.Log.Debug(string.Concat(new string[]
            {
                "enter DoApplyApproval.targetAgentGroupNameLstStr:",
                targetAgentGroupNameLstStr,
                ",targetAgentGroupIdLstStr:",
                targetAgentGroupIdLstStr,
                ",targetStatus:",
                targetStatus,
                ",maxStatusNum:",
                maxStatusNum
            }));
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
            AgentBar.Log.Debug("enter DoGetChangeStatusApplyList.");
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

        private void tsbCallAgent_Click(object sender, EventArgs e)
        {
            this.newFrmTransfer = new FrmTransfer();
            this.newFrmTransfer.AgentBar = this;
            this.newFrmTransfer.AgentNum = this.mAgentID;
            this.newFrmTransfer.ControledAgentGroupLstStr = "";
            this.newFrmTransfer.AgentOrAgentGroup = AgentBar.Agent_Or_AgentGroup.ALL;
            this.newFrmTransfer.Text = "呼叫坐席";
            this.newFrmTransfer.AgentDefineStatus = this.mAgentDefineStatus;
            this.newFrmTransfer.DoGetAgentOnlineEvent += new FrmTransfer.DoGetAgentOnlineEventHandler(this.newFrmTransfer_DoGetAgentOnlineEvent);
            this.GetAgentOnlineEvent = (AgentBar.GetOnlineAgentEventHandler)Delegate.Combine(this.GetAgentOnlineEvent, new AgentBar.GetOnlineAgentEventHandler(this.newFrmTransfer.Evt_GetOnlineAgent));
            this.GetAllAgentGroupListEvent = (AgentBar.GetAllAgentGroupListEventHandler)Delegate.Combine(this.GetAllAgentGroupListEvent, new AgentBar.GetAllAgentGroupListEventHandler(this.newFrmTransfer.Evt_Get_Agent_Group_List));
            if (!this.DoGetAgentGroupList(""))
            {
                MessageBox.Show("获得坐席组列表失败！", "获得坐席组列表", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            else
            {
                this.newFrmTransfer.chkTransferAgent.Visible = false;
                this.newFrmTransfer.chkTransferAgent.Tag = false;
                this.newFrmTransfer.chkOutbound.Visible = false;
                this.newFrmTransfer.chkOutbound.Tag = false;
                if (this.newFrmTransfer.ShowDialog() == DialogResult.OK)
                {
                    if (this.newFrmTransfer.txtAgentID.Text != this.mAgentID)
                    {
                        bool blnRet = this.DoCallAgent(this.newFrmTransfer.txtAgentID.Text);
                        if (blnRet)
                        {
                            this.tsbTransfer.Enabled = false;
                        }
                        else
                        {
                            this.tsbTransfer.Enabled = true;
                            MessageBox.Show("呼叫坐席失败！", "呼叫坐席", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                        }
                    }
                    else
                    {
                        MessageBox.Show("不能呼叫自己！", "呼叫坐席", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    }
                }
            }
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
            AgentBar.Log.Debug(string.Concat(new object[]
            {
                "enter RaiseAllEvents.eventType=",
                agent_event.deAgentEventType,
                ",qualifier=",
                agent_event.eEventQualifier
            }));
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

        private string InputBox(string Caption, string Hint, string Default)
        {
            this.InputForm = new Form();
            this.InputForm.MinimizeBox = false;
            this.InputForm.MaximizeBox = false;
            this.InputForm.StartPosition = FormStartPosition.CenterScreen;
            this.InputForm.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.InputForm.Width = 220;
            this.InputForm.Height = 150;
            this.InputForm.Text = Caption;
            Label lbl = new Label();
            lbl.Text = Hint;
            lbl.Left = 10;
            lbl.Top = 20;
            lbl.Parent = this.InputForm;
            lbl.AutoSize = true;
            TextBox tb = new TextBox();
            tb.Left = 30;
            tb.Top = 45;
            tb.Width = 160;
            tb.Parent = this.InputForm;
            tb.Text = Default;
            tb.SelectAll();
            Button btnok = new Button();
            btnok.Left = 30;
            btnok.Top = 80;
            btnok.Parent = this.InputForm;
            btnok.Text = "确定";
            this.InputForm.AcceptButton = btnok;
            btnok.DialogResult = DialogResult.OK;
            Button btncancal = new Button();
            btncancal.Left = 120;
            btncancal.Top = 80;
            btncancal.Parent = this.InputForm;
            btncancal.Text = "取消";
            btncancal.DialogResult = DialogResult.Cancel;
            string result;
            try
            {
                if (this.InputForm.ShowDialog() == DialogResult.OK)
                {
                    result = tb.Text;
                }
                else
                {
                    result = "";
                }
            }
            finally
            {
                this.InputForm.Dispose();
            }
            return result;
        }

        private void tsbHangUp_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("您确定要挂断电话么？", "挂断电话", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if (!string.IsNullOrEmpty(this.m_agent_current_call_uuid))
                {
                    if (!this.DoHangUp())
                    {
                        this.tsbHangUp.Enabled = true;
                        MessageBox.Show("挂断失败！");
                    }
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

        private void tsbCheck_Click(object sender, EventArgs e)
        {
            this.tsbCheck.Enabled = false;
            bool blnRet = this.DoCheck();
            if (blnRet)
            {
                this.tsbCheck.Enabled = false;
                this.tslStatus.Text = "环回测试";
                this.tslStatus.ToolTipText = "";
            }
            else
            {
                this.tsbCheck.Enabled = true;
                this.tslStatus.Text = "环回测试失败";
                this.tslStatus.ToolTipText = "";
            }
            AgentBar.Log.Debug("测试！");
        }

        private void tsbListen_Click(object sender, EventArgs e)
        {
            if (this.mAgentState == AgentBar.Agent_State.AGENT_STATUS_TALKING_BARGEIN || this.mAgentState == AgentBar.Agent_State.AGENT_STATUS_TALKING_WHISPER)
            {
                bool blnRet = this.DoListen(this.mEavesdropAgent);
                if (blnRet)
                {
                    this.tsbListen.Enabled = false;
                }
                else
                {
                    MessageBox.Show("监听失败！", "监听坐席", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    this.tsbListen.Enabled = true;
                }
            }
            else
            {
                if (this.mAgentStatus != Convert.ToString(9))
                {
                    int new_state = 9;
                    if (!this.DoSetAgentDefineStatus(new_state, 1))
                    {
                        MessageBox.Show("更改坐席状态为：监控失败！", "更改坐席状态", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                        return;
                    }
                }
                this.newFrmTransfer = new FrmTransfer();
                this.newFrmTransfer.AgentBar = this;
                this.newFrmTransfer.AgentNum = this.mAgentID;
                this.newFrmTransfer.ControledAgentGroupLstStr = this.mMyRoleAndRight.controled_agent_group_lst;
                this.newFrmTransfer.AgentOrAgentGroup = AgentBar.Agent_Or_AgentGroup.AGENTGROUPNUM;
                this.newFrmTransfer.Text = "监听坐席";
                this.newFrmTransfer.AgentDefineStatus = this.mAgentDefineStatus;
                this.newFrmTransfer.DoGetAgentOnlineEvent += new FrmTransfer.DoGetAgentOnlineEventHandler(this.newFrmTransfer_DoGetAgentOnlineEvent);
                this.GetAgentOnlineEvent = (AgentBar.GetOnlineAgentEventHandler)Delegate.Combine(this.GetAgentOnlineEvent, new AgentBar.GetOnlineAgentEventHandler(this.newFrmTransfer.Evt_GetOnlineAgent));
                this.GetAgentGroupListEvent = (AgentBar.GetAgentGroupListEventHandler)Delegate.Combine(this.GetAgentGroupListEvent, new AgentBar.GetAgentGroupListEventHandler(this.newFrmTransfer.Evt_Get_Agent_Group_List));
                if (!this.DoGetAgentGroupList(""))
                {
                    MessageBox.Show("获得坐席组列表失败！", "获得坐席组列表", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                }
                else
                {
                    this.newFrmTransfer.chkTransferAgent.Visible = false;
                    this.newFrmTransfer.chkTransferAgent.Tag = false;
                    this.newFrmTransfer.chkOutbound.Visible = false;
                    this.newFrmTransfer.chkOutbound.Tag = false;
                    if (this.newFrmTransfer.ShowDialog() == DialogResult.OK)
                    {
                        if (this.newFrmTransfer.txtAgentID.Text != this.mAgentID)
                        {
                            bool blnRet = this.DoListen(this.newFrmTransfer.txtAgentID.Text);
                            if (blnRet)
                            {
                                this.tsbListen.Enabled = false;
                            }
                            else
                            {
                                MessageBox.Show("监听失败！", "监听坐席", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                                this.tsbListen.Enabled = true;
                            }
                        }
                        else
                        {
                            MessageBox.Show("不能呼叫自己！", "监听坐席", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                        }
                    }
                }
            }
        }

        private void newFrmTransfer_DoGetAgentOnlineEvent(string specificNum, int numType)
        {
            AgentBar.Log.Debug("enter newFrmTransfer_DoGetAgentOnlineEvent .");
            if (!this.DoGetOnlineAgent(specificNum, numType))
            {
                MessageBox.Show("获得在线坐席失败！", "获得在线坐席", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
        }

        private void tsbBargein_Click(object sender, EventArgs e)
        {
            if (this.mAgentState == AgentBar.Agent_State.AGENT_STATUS_TALKING_EAVESDROP || this.mAgentState == AgentBar.Agent_State.AGENT_STATUS_TALKING_WHISPER)
            {
                bool blnRet = this.DoBargein(this.mEavesdropAgent);
                if (blnRet)
                {
                    this.tsbBargein.Enabled = false;
                }
                else
                {
                    MessageBox.Show("插话失败！", "插话", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    this.tsbBargein.Enabled = true;
                }
            }
            else
            {
                if (this.mAgentStatus != Convert.ToString(9))
                {
                    int new_state = 9;
                    if (!this.DoSetAgentDefineStatus(new_state, 1))
                    {
                        MessageBox.Show("更改坐席状态为：监控失败！", "更改坐席状态", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                        return;
                    }
                }
                this.newFrmTransfer = new FrmTransfer();
                this.newFrmTransfer.AgentBar = this;
                this.newFrmTransfer.AgentNum = this.mAgentID;
                this.newFrmTransfer.ControledAgentGroupLstStr = this.mMyRoleAndRight.controled_agent_group_lst;
                this.newFrmTransfer.AgentOrAgentGroup = AgentBar.Agent_Or_AgentGroup.AGENTGROUPNUM;
                this.newFrmTransfer.AgentDefineStatus = this.mAgentDefineStatus;
                this.newFrmTransfer.DoGetAgentOnlineEvent += new FrmTransfer.DoGetAgentOnlineEventHandler(this.newFrmTransfer_DoGetAgentOnlineEvent);
                this.GetAgentOnlineEvent = (AgentBar.GetOnlineAgentEventHandler)Delegate.Combine(this.GetAgentOnlineEvent, new AgentBar.GetOnlineAgentEventHandler(this.newFrmTransfer.Evt_GetOnlineAgent));
                this.GetAgentGroupListEvent = (AgentBar.GetAgentGroupListEventHandler)Delegate.Combine(this.GetAgentGroupListEvent, new AgentBar.GetAgentGroupListEventHandler(this.newFrmTransfer.Evt_Get_Agent_Group_List));
                if (!this.DoGetAgentGroupList(""))
                {
                    MessageBox.Show("获得坐席组列表失败！", "获得坐席组列表", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                }
                else
                {
                    this.newFrmTransfer.Text = "强插坐席";
                    this.newFrmTransfer.chkTransferAgent.Visible = false;
                    this.newFrmTransfer.chkTransferAgent.Tag = false;
                    this.newFrmTransfer.chkOutbound.Visible = false;
                    this.newFrmTransfer.chkOutbound.Tag = false;
                    if (this.newFrmTransfer.ShowDialog() == DialogResult.OK)
                    {
                        if (this.newFrmTransfer.txtAgentID.Text != this.mAgentID)
                        {
                            bool blnRet = this.DoBargein(this.newFrmTransfer.txtAgentID.Text);
                            if (blnRet)
                            {
                                this.tsbBargein.Enabled = false;
                            }
                            else
                            {
                                this.tsbBargein.Enabled = true;
                                MessageBox.Show("插话失败！", "插话", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                            }
                        }
                        else
                        {
                            MessageBox.Show("不能插话自己！", "插话", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                        }
                    }
                }
            }
        }

        private void tsbForceHangup_Click(object sender, EventArgs e)
        {
            if (this.mAgentState == AgentBar.Agent_State.AGENT_STATUS_TALKING_BARGEIN || this.mAgentState == AgentBar.Agent_State.AGENT_STATUS_TALKING_WHISPER || this.mAgentState == AgentBar.Agent_State.AGENT_STATUS_TALKING_EAVESDROP)
            {
                bool blnRet = this.DoForceHangup(this.mEavesdropAgent);
                if (blnRet)
                {
                    this.tsbForceHangup.Enabled = false;
                }
                else
                {
                    MessageBox.Show("强拆失败！", "强拆", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    this.tsbForceHangup.Enabled = true;
                }
            }
            else
            {
                if (this.mAgentStatus != Convert.ToString(9))
                {
                    int new_state = 9;
                    if (!this.DoSetAgentDefineStatus(new_state, 1))
                    {
                        MessageBox.Show("更改坐席状态为：监控失败！", "更改坐席状态", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                        return;
                    }
                }
                this.newFrmTransfer = new FrmTransfer();
                this.newFrmTransfer.AgentBar = this;
                this.newFrmTransfer.AgentNum = this.mAgentID;
                this.newFrmTransfer.AgentDefineStatus = this.mAgentDefineStatus;
                this.newFrmTransfer.ControledAgentGroupLstStr = this.mMyRoleAndRight.controled_agent_group_lst;
                this.newFrmTransfer.AgentOrAgentGroup = AgentBar.Agent_Or_AgentGroup.AGENTGROUPNUM;
                this.newFrmTransfer.DoGetAgentOnlineEvent += new FrmTransfer.DoGetAgentOnlineEventHandler(this.newFrmTransfer_DoGetAgentOnlineEvent);
                this.GetAgentOnlineEvent = (AgentBar.GetOnlineAgentEventHandler)Delegate.Combine(this.GetAgentOnlineEvent, new AgentBar.GetOnlineAgentEventHandler(this.newFrmTransfer.Evt_GetOnlineAgent));
                this.GetAgentGroupListEvent = (AgentBar.GetAgentGroupListEventHandler)Delegate.Combine(this.GetAgentGroupListEvent, new AgentBar.GetAgentGroupListEventHandler(this.newFrmTransfer.Evt_Get_Agent_Group_List));
                if (!this.DoGetAgentGroupList(""))
                {
                    MessageBox.Show("获得坐席组列表失败！", "获得坐席组列表", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                }
                else
                {
                    this.newFrmTransfer.Text = "强拆坐席";
                    this.newFrmTransfer.chkTransferAgent.Visible = false;
                    this.newFrmTransfer.chkTransferAgent.Tag = false;
                    this.newFrmTransfer.chkOutbound.Visible = false;
                    this.newFrmTransfer.chkOutbound.Tag = false;
                    if (this.newFrmTransfer.ShowDialog() == DialogResult.OK)
                    {
                        if (this.newFrmTransfer.txtAgentID.Text != this.mAgentID)
                        {
                            bool blnRet = this.DoForceHangup(this.newFrmTransfer.txtAgentID.Text);
                            if (blnRet)
                            {
                                this.tsbForceHangup.Enabled = false;
                            }
                            else
                            {
                                MessageBox.Show("强拆失败！", "强拆", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                                this.tsbForceHangup.Enabled = true;
                            }
                        }
                        else
                        {
                            MessageBox.Show("不能强拆自己！", "强拆", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                        }
                    }
                }
            }
        }

        private void tsbConsult_Click(object sender, EventArgs e)
        {
            bool trdIsOutbound = false;
            FrmTransfer newFrmTransfer = new FrmTransfer();
            newFrmTransfer.AgentBar = this;
            newFrmTransfer.AgentNum = this.mAgentID;
            newFrmTransfer.AgentDefineStatus = this.mAgentDefineStatus;
            newFrmTransfer.ControledAgentGroupLstStr = "";
            newFrmTransfer.AgentOrAgentGroup = AgentBar.Agent_Or_AgentGroup.ALL;
            newFrmTransfer.DoGetAgentOnlineEvent += new FrmTransfer.DoGetAgentOnlineEventHandler(this.newFrmTransfer_DoGetAgentOnlineEvent);
            this.GetAgentOnlineEvent = (AgentBar.GetOnlineAgentEventHandler)Delegate.Combine(this.GetAgentOnlineEvent, new AgentBar.GetOnlineAgentEventHandler(newFrmTransfer.Evt_GetOnlineAgent));
            this.GetAllAgentGroupListEvent = (AgentBar.GetAllAgentGroupListEventHandler)Delegate.Combine(this.GetAllAgentGroupListEvent, new AgentBar.GetAllAgentGroupListEventHandler(newFrmTransfer.Evt_Get_Agent_Group_List));
            if (!this.DoGetAgentGroupList(""))
            {
                MessageBox.Show("获得坐席组列表失败！", "获得坐席组列表", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            else
            {
                newFrmTransfer.Text = "咨询";
                if (newFrmTransfer.ShowDialog() == DialogResult.OK)
                {
                    if (AgentBar.Str2AgentStatus(this.mAgentStatus) != AgentBar.Agent_Status.AGENT_STATUS_TALKING)
                    {
                        MessageBox.Show("只有在通话时才能发起询问！", "询问", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    }
                    else
                    {
                        if (!newFrmTransfer.chkTransferAgent.Checked)
                        {
                            trdIsOutbound = true;
                        }
                        if ((newFrmTransfer.txtAgentID.Text != this.mAgentID && !trdIsOutbound) || trdIsOutbound)
                        {
                            if (!newFrmTransfer.chkTransferAgent.Checked)
                            {
                                trdIsOutbound = true;
                            }
                            bool blnRet = this.DoConsult(newFrmTransfer.txtAgentID.Text, trdIsOutbound);
                            if (blnRet)
                            {
                                this.tsbConsult.Enabled = false;
                                this.Consultcheck = true;
                            }
                            else
                            {
                                MessageBox.Show("询问失败！", "询问", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                                this.tsbConsult.Enabled = true;
                            }
                        }
                        else
                        {
                            MessageBox.Show("不能呼叫自己！", "询问", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                        }
                    }
                }
            }
        }

        private void tsbStopConsult_Click(object sender, EventArgs e)
        {
            bool blnRet = this.DoStopConsult();
            if (blnRet)
            {
                this.tsbStopConsult.Enabled = false;
            }
            else
            {
                MessageBox.Show("取消咨询失败！", "取消咨询", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                this.tsbStopConsult.Enabled = true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.ServerIP = "192.168.133.45";
            this.ServerPort = 12345;
            this.AgentID = "1049";
            this.mAgentPwd = "1234";
            this.AgentExten = "";
            this.AgentStatus = "0";
            this.mSipNum = "1004";
            this.mSipPwd = "1234";
            this.mSipServer = "192.168.133.45";
            this.mSipPort = 5060;
            this.mSoftPhoneEnable = false;
            this.mSipRegistTime = 60;
            this.mSipAutoAnswer = false;
            this.mSipLocalNum = "10086";
            this.ExtenMode = "cold";
            this.BindExten = false;
            if (!this.blnConnect)
            {
                if (this.DoConnect() != 0)
                {
                    this.tslStatus.Text = "连接服务器失败！";
                    this.tslStatus.ToolTipText = "";
                }
            }
            if (this.DoSignIn())
            {
                this.tslStatus.Text = "正在签入,请稍候.....";
                this.tslStatus.ToolTipText = "";
            }
            else
            {
                this.tslStatus.Text = "签入失败！";
                this.tslStatus.ToolTipText = "";
            }
        }

        private void tsbState_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (!(this.tsbState.AccessibleName == e.ClickedItem.AccessibleName))
            {
                int intStatus = 0;
                if (int.TryParse(e.ClickedItem.AccessibleName, out intStatus))
                {
                    this.DoSetAgentDefineStatus(intStatus, 1);
                }
                else
                {
                    AgentBar.Log.Error("DoSetAgentDefineStatus is failed!!the param is not number!!e.ClickedItem.AccessibleName:" + e.ClickedItem.AccessibleName);
                }
            }
        }

        private void 中文ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("您确定要转到评分流程吗？", "评分", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                this.DoGrade("zh");
            }
        }

        private void 英文ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.DoGrade("en");
        }

        private void initTsbMute()
        {
            this.tsbMute.Enabled = false;
            this.tsbMute.Image = Resources.mute;
            this.tsbMute.AccessibleName = "mute";
            this.tsbMute.Text = "静音";
        }

        private void initTsbHold()
        {
            this.tsbHold.Enabled = false;
            this.tsbHold.Image = Resources._0033;
            this.tsbHold.AccessibleName = "hold";
            this.tsbHold.Text = "保持";
        }

        private void update_Toolbar_UI(AgentBar.Event_Type eventType, string info)
        {
            AgentBar.Log.Debug("enter update_Toolbar_UI .eventType:" + eventType.ToString() + ",info:" + info);
            switch (eventType)
            {
                case AgentBar.Event_Type.INITE_TOOLBAR:
                    this.agentTool.Enabled = false;
                    this.tslAgentInfo.Text = "坐席号:     分机号:    ";
                    this.tslStatus.Text = "未签入";
                    this.tslStatus.ToolTipText = "";
                    this.tsddbAfterHangup.Image = Resources.acw;
                    break;
                case AgentBar.Event_Type.RESPONSE_TIMEOUT:
                    this.tslStatus.Text = "响应超时";
                    this.tslStatus.ToolTipText = "";
                    break;
                case AgentBar.Event_Type.KICK_OUT:
                    this.tslStatus.Text = "断开连接";
                    this.tslStatus.ToolTipText = "";
                    this.agentTool.Enabled = false;
                    this.mAgentState = AgentBar.Agent_State.AGENT_STATUS_OFFLINE;
                    break;
                case AgentBar.Event_Type.INTERNAL_CALL_AGENT_FAIL:
                    this.tsbCallAgent.Enabled = true;
                    this.tslStatus.Text = "呼出失败";
                    this.tslStatus.ToolTipText = "";
                    break;
                case AgentBar.Event_Type.ANSWER_CALL_PEER:
                    this.tsbHangUp.Enabled = true;
                    this.tsbAnswer.Enabled = false;
                    switch (this.mCallType)
                    {
                        case AgentBar.Call_Type.CONSULT_CALL_IN:
                            this.mAgentState = AgentBar.Agent_State.AGENT_STATUS_ANSWER_CONSULT;
                            break;
                        case AgentBar.Call_Type.EAVESDROP_CALL_IN:
                            this.tsbWhisper.Enabled = true;
                            this.tsbBargein.Enabled = true;
                            this.tsbForceHangup.Enabled = true;
                            this.blnListen = true;
                            this.blnWhisper = false;
                            this.blnBargein = false;
                            this.mAgentState = AgentBar.Agent_State.AGENT_STATUS_TALKING_EAVESDROP;
                            break;
                        case AgentBar.Call_Type.WHISPER_CALL_IN:
                            this.tsbBargein.Enabled = true;
                            this.tsbForceHangup.Enabled = true;
                            this.tsbListen.Enabled = true;
                            this.tsbHold.Enabled = true;
                            this.tsbMute.Enabled = true;
                            this.tsbKeyPad.Enabled = true;
                            this.blnWhisper = true;
                            this.blnListen = false;
                            this.blnBargein = false;
                            this.mAgentState = AgentBar.Agent_State.AGENT_STATUS_TALKING_WHISPER;
                            break;
                        case AgentBar.Call_Type.BARGEIN_CALL_IN:
                            this.tsbListen.Enabled = true;
                            this.tsbWhisper.Enabled = true;
                            this.tsbForceHangup.Enabled = true;
                            this.tsbHold.Enabled = true;
                            this.tsbMute.Enabled = true;
                            this.tsbKeyPad.Enabled = true;
                            this.blnBargein = true;
                            this.blnListen = false;
                            this.blnWhisper = false;
                            this.mAgentState = AgentBar.Agent_State.AGENT_STATUS_TALKING_BARGEIN;
                            break;
                        case AgentBar.Call_Type.THREEWAY_CALL_IN:
                            this.tsbHold.Enabled = true;
                            this.tsbMute.Enabled = true;
                            this.tsbKeyPad.Enabled = true;
                            this.mAgentState = AgentBar.Agent_State.AGENT_STATUS_TALKING_THREEWAY;
                            break;
                    }
                    this.tslStatus.Text = "已接听";
                    this.tslStatus.ToolTipText = "";
                    break;
                case AgentBar.Event_Type.ANSWER_CALL_CALLEE:
                    {
                        AgentBar.Call_Type call_Type = this.mCallType;
                        if (call_Type == AgentBar.Call_Type.MANUAL_CALL_OUT)
                        {
                            if (this.mAgentState == AgentBar.Agent_State.AGENT_STATUS_BRIDGE_CALLEE)
                            {
                                this.tsbHangUp.Enabled = true;
                                this.tsbGrade.Enabled = true;
                                this.tsbTransfer.Enabled = true;
                                this.tsbHold.Enabled = true;
                                this.tsbMute.Enabled = true;
                                this.tsbThreeWay.Enabled = true;
                                this.tsbConsult.Enabled = true;
                                this.tsbStopConsult.Enabled = false;
                                this.tsbConsultTransfer.Enabled = false;
                                this.mAgentState = AgentBar.Agent_State.AGENT_STATUS_TALKING_CUSTOMER;
                            }
                            else
                            {
                                this.mAgentState = AgentBar.Agent_State.AGENT_STATUS_ANSWER_CALLEE;
                            }
                        }
                        break;
                    }
                case AgentBar.Event_Type.BRIDGE_CALL_PEER:
                    this.tsbAnswer.Enabled = false;
                    switch (this.mCallType)
                    {
                        case AgentBar.Call_Type.COMMON_CALL_IN:
                            if (this.mAgentState == AgentBar.Agent_State.AGENT_STATUS_CONSULTING)
                            {
                                this.tsbConsultTransfer.Enabled = true;
                                this.tsbStopConsult.Enabled = true;
                                this.tsbHold.Enabled = false;
                                this.tsbMute.Enabled = false;
                                this.tsbKeyPad.Enabled = true;
                                this.tslStatus.Text = "询问后通话";
                                this.tslStatus.ToolTipText = "";
                            }
                            else
                            {
                                this.tsbGrade.Enabled = true;
                                this.tsbCallOut.Enabled = false;
                                this.tsbCallAgent.Enabled = false;
                                this.tsbTransfer.Enabled = true;
                                this.tsbHangUp.Enabled = true;
                                this.tsbCheck.Enabled = false;
                                this.tsbThreeWay.Enabled = true;
                                this.tsbConsult.Enabled = true;
                                this.tsbStopConsult.Enabled = false;
                                this.tsbConsultTransfer.Enabled = false;
                                this.tsbHold.Enabled = true;
                                this.tsbMute.Enabled = true;
                                this.tsbKeyPad.Enabled = true;
                                this.tsbListen.Enabled = false;
                                this.tsbWhisper.Enabled = false;
                                this.tsbBargein.Enabled = false;
                                this.tsbForceHangup.Enabled = false;
                                this.mAgentState = AgentBar.Agent_State.AGENT_STATUS_TALKING_CUSTOMER;
                            }
                            break;
                        case AgentBar.Call_Type.MANUAL_CALL_OUT:
                            if (this.mAgentState == AgentBar.Agent_State.AGENT_STATUS_CONSULTING)
                            {
                                this.tsbConsultTransfer.Enabled = true;
                                this.tsbStopConsult.Enabled = true;
                                this.tsbHold.Enabled = false;
                                this.tsbMute.Enabled = false;
                                this.tsbKeyPad.Enabled = true;
                                this.tslStatus.Text = "询问后通话";
                                this.tslStatus.ToolTipText = "";
                            }
                            else if (this.mAgentState == AgentBar.Agent_State.AGENT_STATUS_ANSWER_CALLEE)
                            {
                                this.tsbHangUp.Enabled = true;
                                this.tsbGrade.Enabled = true;
                                this.tsbTransfer.Enabled = true;
                                this.tsbHold.Enabled = true;
                                this.tsbMute.Enabled = true;
                                this.tsbKeyPad.Enabled = true;
                                this.tsbThreeWay.Enabled = true;
                                this.tsbConsult.Enabled = true;
                                this.tsbStopConsult.Enabled = false;
                                this.tsbConsultTransfer.Enabled = false;
                                this.mAgentState = AgentBar.Agent_State.AGENT_STATUS_TALKING_CUSTOMER;
                            }
                            else if (this.mAgentState != AgentBar.Agent_State.AGENT_STATUS_TALKING_CUSTOMER)
                            {
                                this.mAgentState = AgentBar.Agent_State.AGENT_STATUS_BRIDGE_CALLEE;
                            }
                            break;
                        case AgentBar.Call_Type.PREDICT_CALL_OUT:
                            this.mAgentState = AgentBar.Agent_State.AGENT_STATUS_TALKING_CUSTOMER;
                            break;
                        case AgentBar.Call_Type.AGENT_INTERNAL_CALL:
                            this.tsbGrade.Enabled = false;
                            this.tsbCallOut.Enabled = false;
                            this.tsbCallAgent.Enabled = false;
                            this.tsbTransfer.Enabled = false;
                            this.tsbHangUp.Enabled = true;
                            this.tsbCheck.Enabled = false;
                            this.tsbThreeWay.Enabled = false;
                            this.tsbConsult.Enabled = false;
                            this.tsbStopConsult.Enabled = false;
                            this.tsbConsultTransfer.Enabled = false;
                            this.tsbHold.Enabled = true;
                            this.tsbMute.Enabled = true;
                            this.tsbKeyPad.Enabled = true;
                            this.tsbListen.Enabled = false;
                            this.tsbWhisper.Enabled = false;
                            this.tsbBargein.Enabled = false;
                            this.tsbForceHangup.Enabled = false;
                            break;
                        case AgentBar.Call_Type.CONSULT_CALL_IN:
                            if (this.mAgentState == AgentBar.Agent_State.AGENT_STATUS_TALKING_CONSULT)
                            {
                                this.tsbGrade.Enabled = true;
                                this.tsbCallOut.Enabled = false;
                                this.tsbCallAgent.Enabled = false;
                                this.tsbTransfer.Enabled = true;
                                this.tsbHangUp.Enabled = true;
                                this.tsbCheck.Enabled = false;
                                this.tsbThreeWay.Enabled = true;
                                this.tsbConsult.Enabled = true;
                                this.tsbStopConsult.Enabled = false;
                                this.tsbConsultTransfer.Enabled = false;
                                this.tsbHold.Enabled = true;
                                this.tsbMute.Enabled = true;
                                this.tsbListen.Enabled = false;
                                this.tsbWhisper.Enabled = false;
                                this.tsbBargein.Enabled = false;
                                this.tsbForceHangup.Enabled = false;
                                this.mCallType = AgentBar.Call_Type.COMMON_CALL_IN;
                                this.mCallStatus = AgentBar.CallStatus.CALL_IN;
                                this.mAgentState = AgentBar.Agent_State.AGENT_STATUS_TALKING_CUSTOMER;
                                this.tslStatus.Text = "与客户通话";
                                this.tslStatus.ToolTipText = "";
                            }
                            else
                            {
                                this.tsbGrade.Enabled = false;
                                this.tsbCallOut.Enabled = false;
                                this.tsbCallAgent.Enabled = false;
                                this.tsbTransfer.Enabled = false;
                                this.tsbHangUp.Enabled = true;
                                this.tsbCheck.Enabled = false;
                                this.tsbThreeWay.Enabled = false;
                                this.tsbConsult.Enabled = false;
                                this.tsbStopConsult.Enabled = false;
                                this.tsbConsultTransfer.Enabled = false;
                                this.tsbHold.Enabled = false;
                                this.tsbMute.Enabled = false;
                                this.tsbKeyPad.Enabled = true;
                                this.tsbListen.Enabled = false;
                                this.tsbWhisper.Enabled = false;
                                this.tsbBargein.Enabled = false;
                                this.tsbForceHangup.Enabled = false;
                                this.mAgentState = AgentBar.Agent_State.AGENT_STATUS_TALKING_CONSULT;
                            }
                            break;
                        case AgentBar.Call_Type.EAVESDROP_CALL_IN:
                        case AgentBar.Call_Type.WHISPER_CALL_IN:
                        case AgentBar.Call_Type.BARGEIN_CALL_IN:
                            if (this.mAgentState == AgentBar.Agent_State.AGENT_STATUS_CONSULTING)
                            {
                                this.tsbConsultTransfer.Enabled = true;
                            }
                            break;
                    }
                    this.tsbKeyPad.Enabled = true;
                    this.tslStatus.Text = "通话中";
                    this.tslStatus.ToolTipText = "";
                    break;
                case AgentBar.Event_Type.HANGUP_CALL_SUCCESS:
                    this.tsbCallAgent.Enabled = true;
                    this.tsbHangUp.Enabled = false;
                    this.tsbGrade.Enabled = false;
                    this.tsbCheck.Enabled = true;
                    this.tsbCallOut.Enabled = true;
                    this.initTsbHold();
                    this.initTsbMute();
                    this.tsbKeyPad.Enabled = false;
                    if (this.newKeyPad != null)
                    {
                        this.newKeyPad.Close();
                    }
                    this.tsbThreeWay.Enabled = false;
                    this.tsbTransfer.Enabled = false;
                    this.tsbConsult.Enabled = false;
                    this.tsbConsultTransfer.Enabled = false;
                    this.tsbStopConsult.Enabled = false;
                    this.tsbListen.Enabled = true;
                    this.tsbWhisper.Enabled = true;
                    this.tsbBargein.Enabled = true;
                    this.tsbForceHangup.Enabled = true;
                    this.tslStatus.Text = info;
                    this.tslStatus.ToolTipText = "";
                    this.tsbState.Enabled = true;
                    this.tsbAnswer.Enabled = false;
                    this.blnWhisper = false;
                    this.blnListen = false;
                    this.blnBargein = false;
                    this.mAgentState = AgentBar.Agent_State.AGENT_STATUS_IDLE;
                    if (this.tsbThreeWay.AccessibleName != "threeway")
                    {
                        this.tsbThreeWay.Image = Resources._0001;
                        this.tsbThreeWay.AccessibleName = "threeway";
                        this.tsbThreeWay.Text = "三方";
                    }
                    break;
                case AgentBar.Event_Type.HANGUP_CALL_FAIL:
                    if (this.mAgentState != AgentBar.Agent_State.AGENT_STATUS_IDLE)
                    {
                        this.tsbHangUp.Enabled = true;
                    }
                    this.tslStatus.Text = info;
                    break;
                case AgentBar.Event_Type.CALLIN_COMMON:
                    this.tsbCallAgent.Enabled = false;
                    this.tsbCallOut.Enabled = false;
                    this.tsbGrade.Enabled = false;
                    this.tsbHangUp.Enabled = false;
                    this.tsbCheck.Enabled = false;
                    this.tsbTransfer.Enabled = false;
                    this.tsbThreeWay.Enabled = false;
                    this.tsbConsult.Enabled = false;
                    this.tsbStopConsult.Enabled = false;
                    this.tsbConsultTransfer.Enabled = false;
                    this.tsbListen.Enabled = false;
                    this.tsbWhisper.Enabled = false;
                    this.tsbBargein.Enabled = false;
                    this.tsbForceHangup.Enabled = false;
                    this.tslStatus.Text = "来电";
                    this.tslStatus.ToolTipText = "";
                    break;
                case AgentBar.Event_Type.CALLIN_PREDICT_CALL:
                    this.tsbCallAgent.Enabled = false;
                    this.tsbCallOut.Enabled = false;
                    this.tsbGrade.Enabled = false;
                    this.tsbHangUp.Enabled = false;
                    this.tsbCheck.Enabled = false;
                    this.tsbTransfer.Enabled = false;
                    this.tsbThreeWay.Enabled = false;
                    this.tsbConsult.Enabled = false;
                    this.tsbStopConsult.Enabled = false;
                    this.tsbConsultTransfer.Enabled = false;
                    this.tsbListen.Enabled = false;
                    this.tsbWhisper.Enabled = false;
                    this.tsbBargein.Enabled = false;
                    this.tsbForceHangup.Enabled = false;
                    this.tslStatus.Text = "来电";
                    this.tslStatus.ToolTipText = "";
                    break;
                case AgentBar.Event_Type.CALLIN_INTERNAL:
                    this.tsbCallAgent.Enabled = false;
                    this.tsbCallOut.Enabled = false;
                    this.tsbGrade.Enabled = false;
                    this.tsbHangUp.Enabled = false;
                    this.tsbCheck.Enabled = false;
                    this.tsbTransfer.Enabled = false;
                    this.tsbThreeWay.Enabled = false;
                    this.tsbConsult.Enabled = false;
                    this.tsbHold.Enabled = false;
                    this.tsbMute.Enabled = false;
                    this.tsbKeyPad.Enabled = false;
                    this.tsbStopConsult.Enabled = false;
                    this.tsbConsultTransfer.Enabled = false;
                    this.tsbListen.Enabled = false;
                    this.tsbWhisper.Enabled = false;
                    this.tsbBargein.Enabled = false;
                    this.tsbForceHangup.Enabled = false;
                    this.tslStatus.Text = info;
                    this.tslStatus.ToolTipText = "";
                    break;
                case AgentBar.Event_Type.CALLIN_INTERNAL_MYSELF:
                    this.tsbCallAgent.Enabled = false;
                    this.tsbCallOut.Enabled = false;
                    this.tsbGrade.Enabled = false;
                    this.tsbHangUp.Enabled = false;
                    this.tsbCheck.Enabled = false;
                    this.tsbTransfer.Enabled = false;
                    this.tsbThreeWay.Enabled = false;
                    this.tsbConsult.Enabled = false;
                    this.tsbHold.Enabled = false;
                    this.tsbMute.Enabled = false;
                    this.tsbKeyPad.Enabled = false;
                    this.tsbStopConsult.Enabled = false;
                    this.tsbConsultTransfer.Enabled = false;
                    this.tsbListen.Enabled = false;
                    this.tsbWhisper.Enabled = false;
                    this.tsbBargein.Enabled = false;
                    this.tsbForceHangup.Enabled = false;
                    this.tslStatus.Text = "内部呼叫";
                    this.tslStatus.ToolTipText = "";
                    break;
                case AgentBar.Event_Type.CALLIN_THREE_WAY:
                    this.tsbCallAgent.Enabled = false;
                    this.tsbCallOut.Enabled = false;
                    this.tsbGrade.Enabled = false;
                    this.tsbHangUp.Enabled = false;
                    this.tsbCheck.Enabled = false;
                    this.tsbTransfer.Enabled = false;
                    this.tsbThreeWay.Enabled = false;
                    this.tsbConsult.Enabled = false;
                    this.tsbHold.Enabled = false;
                    this.tsbMute.Enabled = false;
                    this.tsbStopConsult.Enabled = false;
                    this.tsbConsultTransfer.Enabled = false;
                    this.tsbListen.Enabled = false;
                    this.tsbWhisper.Enabled = false;
                    this.tsbBargein.Enabled = false;
                    this.tsbForceHangup.Enabled = false;
                    this.tslStatus.Text = "三方邀请来电";
                    this.tslStatus.ToolTipText = "";
                    break;
                case AgentBar.Event_Type.CALLOUT_RING_MYSELF:
                    this.tsbCallOut.Enabled = false;
                    this.tsbCallAgent.Enabled = false;
                    this.tsbTransfer.Enabled = false;
                    this.tsbHangUp.Enabled = false;
                    this.tsbCheck.Enabled = false;
                    this.tsbHold.Enabled = false;
                    this.tsbMute.Enabled = false;
                    this.tsbThreeWay.Enabled = false;
                    this.tsbConsult.Enabled = false;
                    this.tsbStopConsult.Enabled = false;
                    this.tsbConsultTransfer.Enabled = false;
                    this.tsbListen.Enabled = false;
                    this.tsbWhisper.Enabled = false;
                    this.tsbBargein.Enabled = false;
                    this.tsbForceHangup.Enabled = false;
                    this.tslStatus.Text = "请摘机";
                    this.tslStatus.ToolTipText = "";
                    break;
                case AgentBar.Event_Type.MANUAL_CALLOUT_SUCCESS:
                    this.tsbCallOut.Enabled = false;
                    this.tslStatus.Text = "呼出成功";
                    this.tslStatus.ToolTipText = "";
                    break;
                case AgentBar.Event_Type.MANUAL_CALLOUT_FAIL:
                    this.tsbCallOut.Enabled = true;
                    this.tslStatus.Text = info;
                    this.tslStatus.ToolTipText = "";
                    break;
                case AgentBar.Event_Type.DISCONNECT_SOCKET:
                    this.tslStatus.Text = "断开连接";
                    this.tslStatus.ToolTipText = "";
                    this.agentTool.Enabled = false;
                    this.mAgentState = AgentBar.Agent_State.AGENT_STATUS_OFFLINE;
                    break;
                case AgentBar.Event_Type.SIGNIN_SUCCESS:
                    this.agentTool.Enabled = true;
                    if (this.BindExten)
                    {
                        if (this.mSoftPhoneEnable || this.mSoftPhoneEnable2)
                        {
                            this.tsbAnswer.Visible = true;
                            this.tsbAnswer.Tag = true;
                            this.tsbAnswer.Enabled = false;
                            this.tsbKeyPad.Visible = true;
                            this.tsbKeyPad.Tag = true;
                        }
                        else
                        {
                            this.tsbAnswer.Visible = false;
                            this.tsbAnswer.Tag = false;
                            this.tsbKeyPad.Visible = false;
                            this.tsbKeyPad.Tag = false;
                        }
                        this.tsbCallOut.Enabled = true;
                        this.tsbCallAgent.Enabled = true;
                        this.tsbCheck.Enabled = true;
                        this.tsbListen.Enabled = true;
                        this.tsbWhisper.Enabled = true;
                        this.tsbBargein.Enabled = true;
                        this.tsbForceHangup.Enabled = true;
                        this.tsddbAfterHangup.Enabled = true;
                        this.tsbNoAnswerCalls.Enabled = true;
                        if (this.mAgentStateAfterHangup == AgentBar.Agent_Status.AGENT_STATUS_IDLE)
                        {
                            this.tsddbAfterHangup.Image = Resources.idle;
                            this.tsddbAfterHangup.ToolTipText = this.tsmiIdle.ToolTipText;
                        }
                        else if (this.mAgentStateAfterHangup == AgentBar.Agent_Status.AGENT_STATUS_ACW)
                        {
                            this.tsddbAfterHangup.Image = Resources.acw;
                            this.tsddbAfterHangup.ToolTipText = this.tsmiAcw.ToolTipText;
                        }
                        else if (this.mAgentStateAfterHangup == AgentBar.Agent_Status.AGENT_STATUS_RESTORE)
                        {
                            this.tsddbAfterHangup.Image = Resources.restore;
                            this.tsddbAfterHangup.ToolTipText = this.tsmiRestore.ToolTipText;
                        }
                    }
                    else
                    {
                        this.tsddbAfterHangup.Enabled = false;
                        this.tsbAnswer.Enabled = false;
                        this.tsbCallOut.Enabled = false;
                        this.tsbCallAgent.Enabled = false;
                        this.tsbCheck.Enabled = false;
                        this.tsbListen.Enabled = false;
                        this.tsbWhisper.Enabled = false;
                        this.tsbBargein.Enabled = false;
                        this.tsbForceHangup.Enabled = false;
                        this.tsbNoAnswerCalls.Enabled = false;
                    }
                    this.initTsbHold();
                    this.initTsbMute();
                    this.tsbGrade.Enabled = false;
                    this.tsbHangUp.Enabled = false;
                    this.tsbTransfer.Enabled = false;
                    this.tsbKeyPad.Enabled = false;
                    this.tsbThreeWay.Enabled = false;
                    this.tsbConsult.Enabled = false;
                    this.tsbStopConsult.Enabled = false;
                    this.tsbConsultTransfer.Enabled = false;
                    this.tsbCancelApplication.Enabled = false;
                    this.tslStatus.Text = "签入成功";
                    this.tslStatus.ToolTipText = "";
                    this.button1.Visible = false;
                    this.button1.Tag = false;
                    break;
                case AgentBar.Event_Type.SIGNIN_FAIL:
                    this.agentTool.Enabled = false;
                    this.tslStatus.Text = "签入失败";
                    this.tslStatus.ToolTipText = "";
                    break;
                case AgentBar.Event_Type.SIGNOUT_SUCCESS:
                    this.tslStatus.Text = "已签出";
                    this.tslStatus.ToolTipText = "";
                    break;
                case AgentBar.Event_Type.SIGNOUT_FAIL:
                    this.tslStatus.Text = "签出失败";
                    this.tslStatus.ToolTipText = "";
                    break;
                case AgentBar.Event_Type.GRADE_FAIL:
                    this.tsbGrade.Enabled = true;
                    this.tslStatus.Text = "评分失败";
                    this.tslStatus.ToolTipText = "";
                    break;
                case AgentBar.Event_Type.HOLD_SUCCESS:
                    this.tsbHold.Image = Resources._0034;
                    this.tsbHold.Text = "取消保持";
                    this.tsbHold.Enabled = true;
                    this.tsbHold.AccessibleName = "unhold";
                    this.tsbMute.Enabled = false;
                    this.tsbKeyPad.Enabled = false;
                    this.tslStatus.Text = "保持成功";
                    this.tslStatus.ToolTipText = "";
                    break;
                case AgentBar.Event_Type.HOLD_FAIL:
                    this.tsbHold.Enabled = true;
                    this.tsbKeyPad.Enabled = true;
                    this.tslStatus.Text = "保持失败";
                    this.tslStatus.ToolTipText = "";
                    break;
                case AgentBar.Event_Type.UNHOLD_SUCCESS:
                    this.tsbHold.Image = Resources._0033;
                    this.tsbHold.Text = "保持";
                    this.tsbHold.Enabled = true;
                    this.tsbHold.AccessibleName = "hold";
                    this.tsbMute.Enabled = true;
                    this.tsbKeyPad.Enabled = true;
                    this.tsbHangUp.Enabled = true;
                    if (this.mCallType == AgentBar.Call_Type.COMMON_CALL_IN || this.mCallType == AgentBar.Call_Type.MANUAL_CALL_OUT || this.mCallType == AgentBar.Call_Type.PREDICT_CALL_OUT || this.mCallType == AgentBar.Call_Type.WHISPER_CALL_IN || this.mCallType == AgentBar.Call_Type.BARGEIN_CALL_IN || this.mCallType == AgentBar.Call_Type.EAVESDROP_CALL_IN)
                    {
                        switch (this.mAgentState)
                        {
                            case AgentBar.Agent_State.AGENT_STATUS_TALKING_CUSTOMER:
                                this.tsbTransfer.Enabled = true;
                                this.tsbGrade.Enabled = true;
                                this.tsbThreeWay.Enabled = true;
                                this.tsbConsult.Enabled = true;
                                break;
                            case AgentBar.Agent_State.AGENT_STATUS_TALKING_CONSULT:
                                this.tsbThreeWay.Enabled = false;
                                this.tsbTransfer.Enabled = false;
                                this.tsbConsult.Enabled = false;
                                this.tsbStopConsult.Enabled = true;
                                this.tsbConsultTransfer.Enabled = true;
                                this.tsbGrade.Enabled = true;
                                break;
                            case AgentBar.Agent_State.AGENT_STATUS_TALKING_THREEWAY:
                                this.tsbThreeWay.Enabled = true;
                                this.tsbTransfer.Enabled = false;
                                this.tsbConsult.Enabled = false;
                                this.tsbStopConsult.Enabled = false;
                                this.tsbConsultTransfer.Enabled = false;
                                this.tsbGrade.Enabled = true;
                                break;
                            case AgentBar.Agent_State.AGENT_STATUS_TALKING_WHISPER:
                                this.tsbListen.Enabled = true;
                                this.tsbBargein.Enabled = true;
                                this.tsbForceHangup.Enabled = true;
                                break;
                            case AgentBar.Agent_State.AGENT_STATUS_TALKING_BARGEIN:
                                this.tsbListen.Enabled = true;
                                this.tsbWhisper.Enabled = true;
                                this.tsbForceHangup.Enabled = true;
                                break;
                        }
                    }
                    this.tslStatus.Text = "取消保持成功";
                    this.tslStatus.ToolTipText = "";
                    break;
                case AgentBar.Event_Type.UNHOLD_FAIL:
                    this.tsbHold.Enabled = true;
                    this.tsbMute.Enabled = false;
                    this.tsbKeyPad.Enabled = false;
                    this.tslStatus.Text = "取消保持失败";
                    this.tslStatus.ToolTipText = "";
                    break;
                case AgentBar.Event_Type.MUTE_SUCCESS:
                    this.tsbMute.Image = Resources.unMute;
                    this.tsbMute.AccessibleName = "unmute";
                    this.tsbMute.Text = "取消静音";
                    this.tsbKeyPad.Enabled = false;
                    this.tsbHold.Enabled = false;
                    this.tsbMute.Enabled = true;
                    this.tslStatus.Text = "静音成功";
                    this.tslStatus.ToolTipText = "";
                    break;
                case AgentBar.Event_Type.MUTE_FAIL:
                    this.tsbMute.Enabled = true;
                    this.tslStatus.Text = "静音失败";
                    this.tslStatus.ToolTipText = "";
                    break;
                case AgentBar.Event_Type.UNMUTE_SUCCESS:
                    this.tsbHold.Enabled = true;
                    this.tsbMute.Enabled = true;
                    this.tsbMute.Image = Resources.mute;
                    this.tsbMute.AccessibleName = "mute";
                    this.tsbMute.Text = "静音";
                    this.tsbKeyPad.Enabled = true;
                    this.tsbHangUp.Enabled = true;
                    if (this.mCallType == AgentBar.Call_Type.COMMON_CALL_IN || this.mCallType == AgentBar.Call_Type.MANUAL_CALL_OUT || this.mCallType == AgentBar.Call_Type.PREDICT_CALL_OUT || this.mCallType == AgentBar.Call_Type.WHISPER_CALL_IN || this.mCallType == AgentBar.Call_Type.BARGEIN_CALL_IN || this.mCallType == AgentBar.Call_Type.EAVESDROP_CALL_IN)
                    {
                        switch (this.mAgentState)
                        {
                            case AgentBar.Agent_State.AGENT_STATUS_TALKING_CUSTOMER:
                                this.tsbTransfer.Enabled = true;
                                this.tsbGrade.Enabled = true;
                                this.tsbThreeWay.Enabled = true;
                                this.tsbConsult.Enabled = true;
                                break;
                            case AgentBar.Agent_State.AGENT_STATUS_TALKING_CONSULT:
                                this.tsbThreeWay.Enabled = false;
                                this.tsbTransfer.Enabled = false;
                                this.tsbConsult.Enabled = false;
                                this.tsbStopConsult.Enabled = true;
                                this.tsbConsultTransfer.Enabled = true;
                                this.tsbGrade.Enabled = true;
                                break;
                            case AgentBar.Agent_State.AGENT_STATUS_TALKING_THREEWAY:
                                this.tsbThreeWay.Enabled = true;
                                this.tsbTransfer.Enabled = false;
                                this.tsbConsult.Enabled = false;
                                this.tsbStopConsult.Enabled = false;
                                this.tsbConsultTransfer.Enabled = false;
                                this.tsbGrade.Enabled = true;
                                break;
                            case AgentBar.Agent_State.AGENT_STATUS_TALKING_WHISPER:
                                this.tsbListen.Enabled = true;
                                this.tsbBargein.Enabled = true;
                                this.tsbForceHangup.Enabled = true;
                                break;
                            case AgentBar.Agent_State.AGENT_STATUS_TALKING_BARGEIN:
                                this.tsbListen.Enabled = true;
                                this.tsbWhisper.Enabled = true;
                                this.tsbForceHangup.Enabled = true;
                                break;
                        }
                    }
                    this.tslStatus.Text = "取消静音成功";
                    this.tslStatus.ToolTipText = "";
                    break;
                case AgentBar.Event_Type.UNMUTE_FAIL:
                    this.tsbMute.Enabled = true;
                    this.tslStatus.Text = "取消静音失败";
                    this.tslStatus.ToolTipText = "";
                    break;
                case AgentBar.Event_Type.TRANSFER_AGENT_SUCCESS:
                case AgentBar.Event_Type.TRANSFER_IVR_SUCCESS:
                case AgentBar.Event_Type.TRANSFER_QUEUE_SUCCESS:
                case AgentBar.Event_Type.TRANSFER_IVR_PROFILE_SUCCESS:
                    this.tslStatus.Text = "转接成功";
                    this.tslStatus.ToolTipText = "";
                    break;
                case AgentBar.Event_Type.TRANSFER_AGENT_FAIL:
                case AgentBar.Event_Type.TRANSFER_IVR_FAIL:
                case AgentBar.Event_Type.TRANSFER_QUEUE_FAIL:
                case AgentBar.Event_Type.TRANSFER_IVR_PROFILE_FAIL:
                    if (AgentBar.Str2AgentStatus(this.mAgentStatus) == AgentBar.Agent_Status.AGENT_STATUS_TALKING)
                    {
                        this.tsbTransfer.Enabled = true;
                    }
                    this.tslStatus.Text = "转接失败";
                    this.tslStatus.ToolTipText = "";
                    break;
                case AgentBar.Event_Type.TRANSFER_BLIND_CALL_IN:
                    this.tsbCallAgent.Enabled = false;
                    this.tsbCallOut.Enabled = false;
                    this.tsbGrade.Enabled = false;
                    this.tsbHangUp.Enabled = false;
                    this.tsbCheck.Enabled = false;
                    this.tsbTransfer.Enabled = false;
                    this.tsbListen.Enabled = false;
                    this.tsbWhisper.Enabled = false;
                    this.tsbBargein.Enabled = false;
                    this.tsbForceHangup.Enabled = false;
                    this.tsbThreeWay.Enabled = false;
                    this.tsbConsult.Enabled = false;
                    this.tsbStopConsult.Enabled = false;
                    this.tsbConsultTransfer.Enabled = false;
                    this.tsbHold.Enabled = false;
                    this.tsbMute.Enabled = false;
                    this.tsbKeyPad.Enabled = false;
                    this.tslStatus.Text = "盲转来电";
                    this.tslStatus.ToolTipText = "";
                    break;
                case AgentBar.Event_Type.ECHO_TEST_SUCCESS:
                    this.tsbCallAgent.Enabled = false;
                    this.tsbCallOut.Enabled = false;
                    this.tsbGrade.Enabled = false;
                    this.tsbHangUp.Enabled = false;
                    this.tsbCheck.Enabled = false;
                    this.tsbTransfer.Enabled = false;
                    this.tsbThreeWay.Enabled = false;
                    this.tsbConsult.Enabled = false;
                    this.tsbStopConsult.Enabled = false;
                    this.tsbConsultTransfer.Enabled = false;
                    this.tsbListen.Enabled = false;
                    this.tsbWhisper.Enabled = false;
                    this.tsbBargein.Enabled = false;
                    this.tsbForceHangup.Enabled = false;
                    break;
                case AgentBar.Event_Type.ECHO_TEST_FAIL:
                    this.tsbCheck.Enabled = true;
                    this.tslStatus.Text = "环回测试失败";
                    this.tslStatus.ToolTipText = "";
                    break;
                case AgentBar.Event_Type.CONSULT_SUCCESS:
                    this.tsbConsult.Enabled = false;
                    this.tsbStopConsult.Enabled = true;
                    this.tsbConsultTransfer.Enabled = false;
                    this.tsbGrade.Enabled = false;
                    this.tsbTransfer.Enabled = false;
                    this.tsbThreeWay.Enabled = false;
                    this.tsbHold.Enabled = false;
                    this.tsbMute.Enabled = false;
                    this.tsbKeyPad.Enabled = true;
                    this.tsbHangUp.Enabled = false;
                    this.tslStatus.Text = "询问成功";
                    this.tslStatus.ToolTipText = "";
                    this.mAgentState = AgentBar.Agent_State.AGENT_STATUS_CONSULTING;
                    break;
                case AgentBar.Event_Type.CONSULT_FAIL:
                    if (AgentBar.Str2AgentStatus(this.mAgentStatus) == AgentBar.Agent_Status.AGENT_STATUS_TALKING)
                    {
                        this.tsbConsult.Enabled = true;
                        this.tsbStopConsult.Enabled = false;
                        this.tsbConsultTransfer.Enabled = false;
                    }
                    this.tslStatus.Text = "询问失败";
                    this.tslStatus.ToolTipText = "";
                    break;
                case AgentBar.Event_Type.CONSULT_CANCEL_SUCCESS:
                    this.tsbConsult.Enabled = true;
                    this.tsbStopConsult.Enabled = false;
                    this.tsbConsultTransfer.Enabled = false;
                    this.tsbGrade.Enabled = true;
                    this.tsbTransfer.Enabled = true;
                    this.tsbThreeWay.Enabled = true;
                    this.tsbHold.Enabled = true;
                    this.tsbMute.Enabled = true;
                    this.tsbKeyPad.Enabled = true;
                    this.tsbHangUp.Enabled = true;
                    this.tslStatus.Text = "取消询问成功";
                    this.tslStatus.ToolTipText = "";
                    this.mAgentState = AgentBar.Agent_State.AGENT_STATUS_TALKING_CUSTOMER;
                    break;
                case AgentBar.Event_Type.CONSULT_CANCEL_FAIL:
                    this.tsbConsult.Enabled = false;
                    this.tsbStopConsult.Enabled = true;
                    this.tsbConsultTransfer.Enabled = true;
                    this.tslStatus.Text = "取消询问失败";
                    this.tslStatus.ToolTipText = "";
                    break;
                case AgentBar.Event_Type.CONSULT_TRANSFER_SUCCESS:
                    this.tsbConsult.Enabled = false;
                    this.tsbStopConsult.Enabled = false;
                    this.tsbConsultTransfer.Enabled = false;
                    this.tslStatus.Text = "询问转成功";
                    this.tslStatus.ToolTipText = "";
                    break;
                case AgentBar.Event_Type.CONSULT_TRANSFER_FAIL:
                    this.tsbConsult.Enabled = false;
                    this.tsbStopConsult.Enabled = true;
                    this.tsbConsultTransfer.Enabled = true;
                    this.tslStatus.Text = "询问转失败";
                    this.tslStatus.ToolTipText = "";
                    break;
                case AgentBar.Event_Type.CONSULT_CALL_IN:
                    this.tsbCallAgent.Enabled = false;
                    this.tsbCallOut.Enabled = false;
                    this.tsbHangUp.Enabled = false;
                    this.tsbCheck.Enabled = false;
                    this.tsbTransfer.Enabled = false;
                    this.tsbListen.Enabled = false;
                    this.tsbWhisper.Enabled = false;
                    this.tsbBargein.Enabled = false;
                    this.tsbForceHangup.Enabled = false;
                    this.tsbThreeWay.Enabled = false;
                    this.tsbConsult.Enabled = false;
                    this.tsbStopConsult.Enabled = false;
                    this.tsbConsultTransfer.Enabled = false;
                    this.tsbHold.Enabled = false;
                    this.tsbMute.Enabled = false;
                    this.tsbKeyPad.Enabled = false;
                    this.tslStatus.Text = "咨询来电";
                    this.tslStatus.ToolTipText = "";
                    break;
                case AgentBar.Event_Type.CONSULTEE_HANGUP:
                    this.tsbConsult.Enabled = true;
                    this.tsbStopConsult.Enabled = false;
                    this.tsbConsultTransfer.Enabled = false;
                    this.tsbThreeWay.Enabled = true;
                    this.tsbGrade.Enabled = true;
                    this.tsbTransfer.Enabled = true;
                    this.tsbHold.Enabled = true;
                    this.tsbMute.Enabled = true;
                    this.tsbKeyPad.Enabled = true;
                    this.tsbHangUp.Enabled = true;
                    this.tslStatus.Text = info;
                    this.mAgentState = AgentBar.Agent_State.AGENT_STATUS_TALKING_CUSTOMER;
                    break;
                case AgentBar.Event_Type.THREE_WAY_SUCCESS:
                    this.tsbThreeWay.Image = Resources._0002;
                    this.tsbThreeWay.AccessibleName = "three_way_cancel";
                    this.tsbThreeWay.Text = "取消三方";
                    this.tsbThreeWay.Enabled = true;
                    this.tsbConsult.Enabled = false;
                    this.tsbStopConsult.Enabled = false;
                    this.tsbConsultTransfer.Enabled = false;
                    this.tsbGrade.Enabled = false;
                    this.tsbTransfer.Enabled = false;
                    this.tsbHold.Enabled = false;
                    this.tsbMute.Enabled = false;
                    this.tslStatus.Text = "三方通话";
                    this.tslStatus.ToolTipText = "";
                    this.mAgentState = AgentBar.Agent_State.AGENT_STATUS_THREEWAYTING;
                    break;
                case AgentBar.Event_Type.THREE_WAY_FAIL:
                    if (AgentBar.Str2AgentStatus(this.mAgentStatus) == AgentBar.Agent_Status.AGENT_STATUS_TALKING)
                    {
                        this.tsbThreeWay.Enabled = true;
                    }
                    this.tslStatus.Text = "三方失败";
                    this.tslStatus.ToolTipText = "";
                    break;
                case AgentBar.Event_Type.THREE_WAY_CANCEL_SUCCESS:
                    this.tsbThreeWay.Image = Resources._0001;
                    this.tsbThreeWay.AccessibleName = "threeway";
                    this.tsbThreeWay.Text = "三方";
                    this.tsbThreeWay.Enabled = true;
                    this.tsbConsult.Enabled = true;
                    this.tsbStopConsult.Enabled = false;
                    this.tsbConsultTransfer.Enabled = false;
                    this.tsbGrade.Enabled = true;
                    this.tsbTransfer.Enabled = true;
                    this.tsbHold.Enabled = true;
                    this.tsbMute.Enabled = true;
                    this.tsbKeyPad.Enabled = true;
                    this.tsbKeyPad.Enabled = true;
                    this.mAgentState = AgentBar.Agent_State.AGENT_STATUS_TALKING_CUSTOMER;
                    this.tslStatus.Text = "取消三方成功";
                    this.tslStatus.ToolTipText = "";
                    break;
                case AgentBar.Event_Type.THREE_WAY_CANCEL_FAIL:
                    this.tslStatus.Text = "取消三方失败";
                    this.tslStatus.ToolTipText = "";
                    break;
                case AgentBar.Event_Type.THREEWAYEE_HANGUP:
                    this.tsbThreeWay.Enabled = true;
                    this.tsbThreeWay.Image = Resources._0001;
                    this.tsbThreeWay.AccessibleName = "threeway";
                    this.tsbThreeWay.Text = "三方";
                    this.tsbConsult.Enabled = true;
                    this.tsbStopConsult.Enabled = false;
                    this.tsbConsultTransfer.Enabled = false;
                    this.tsbGrade.Enabled = true;
                    this.tsbTransfer.Enabled = true;
                    this.tsbHold.Enabled = true;
                    this.tsbMute.Enabled = true;
                    this.tsbKeyPad.Enabled = true;
                    this.tslStatus.Text = info;
                    this.mAgentState = AgentBar.Agent_State.AGENT_STATUS_TALKING_CUSTOMER;
                    break;
                case AgentBar.Event_Type.EAVESDROP_SUCCESS:
                    switch (this.mAgentState)
                    {
                        case AgentBar.Agent_State.AGENT_STATUS_TALKING_WHISPER:
                            this.tsbWhisper.Enabled = true;
                            this.tsbBargein.Enabled = true;
                            this.tsbForceHangup.Enabled = true;
                            break;
                        case AgentBar.Agent_State.AGENT_STATUS_TALKING_BARGEIN:
                            this.tsbWhisper.Enabled = true;
                            this.tsbBargein.Enabled = true;
                            this.tsbForceHangup.Enabled = true;
                            break;
                    }
                    this.tsbListen.Enabled = false;
                    this.mAgentState = AgentBar.Agent_State.AGENT_STATUS_TALKING_EAVESDROP;
                    this.tsbHold.Enabled = false;
                    this.tsbMute.Enabled = false;
                    this.tslStatus.Text = "监听成功";
                    this.tslStatus.ToolTipText = "";
                    break;
                case AgentBar.Event_Type.EAVESDROP_FAIL:
                    this.tsbListen.Enabled = true;
                    this.tslStatus.Text = "监听失败";
                    this.tslStatus.ToolTipText = "";
                    break;
                case AgentBar.Event_Type.EAVESDROP_RING_MYSELF:
                    this.tsbCallAgent.Enabled = false;
                    this.tsbCallOut.Enabled = false;
                    this.tsbHangUp.Enabled = false;
                    this.tsbCheck.Enabled = false;
                    this.tsbTransfer.Enabled = false;
                    this.tsbListen.Enabled = false;
                    this.tsbWhisper.Enabled = false;
                    this.tsbBargein.Enabled = false;
                    this.tsbForceHangup.Enabled = false;
                    this.tsbThreeWay.Enabled = false;
                    this.tsbConsult.Enabled = false;
                    this.tsbStopConsult.Enabled = false;
                    this.tsbConsultTransfer.Enabled = false;
                    this.tsbHold.Enabled = false;
                    this.tsbMute.Enabled = false;
                    this.tsbKeyPad.Enabled = false;
                    this.tslStatus.Text = "请摘机";
                    this.tslStatus.ToolTipText = "";
                    break;
                case AgentBar.Event_Type.WHISPER_RING_MYSELF:
                    this.tsbCallAgent.Enabled = false;
                    this.tsbCallOut.Enabled = false;
                    this.tsbHangUp.Enabled = false;
                    this.tsbCheck.Enabled = false;
                    this.tsbTransfer.Enabled = false;
                    this.tsbListen.Enabled = false;
                    this.tsbWhisper.Enabled = false;
                    this.tsbBargein.Enabled = false;
                    this.tsbForceHangup.Enabled = false;
                    this.tsbThreeWay.Enabled = false;
                    this.tsbConsult.Enabled = false;
                    this.tsbStopConsult.Enabled = false;
                    this.tsbConsultTransfer.Enabled = false;
                    this.tsbHold.Enabled = false;
                    this.tsbMute.Enabled = false;
                    this.tsbKeyPad.Enabled = false;
                    this.tslStatus.Text = info;
                    this.tslStatus.ToolTipText = "";
                    break;
                case AgentBar.Event_Type.BARGEIN_RING_MYSELF:
                    this.tsbCallAgent.Enabled = false;
                    this.tsbCallOut.Enabled = false;
                    this.tsbHangUp.Enabled = false;
                    this.tsbCheck.Enabled = false;
                    this.tsbTransfer.Enabled = false;
                    this.tsbListen.Enabled = false;
                    this.tsbWhisper.Enabled = false;
                    this.tsbBargein.Enabled = false;
                    this.tsbForceHangup.Enabled = false;
                    this.tsbThreeWay.Enabled = false;
                    this.tsbConsult.Enabled = false;
                    this.tsbStopConsult.Enabled = false;
                    this.tsbConsultTransfer.Enabled = false;
                    this.tsbHold.Enabled = false;
                    this.tsbMute.Enabled = false;
                    this.tsbKeyPad.Enabled = false;
                    this.tslStatus.Text = "请摘机";
                    this.tslStatus.ToolTipText = "";
                    break;
                case AgentBar.Event_Type.FORCE_HANGUP_RING_MYSELF:
                    this.tsbCallAgent.Enabled = false;
                    this.tsbCallOut.Enabled = false;
                    this.tsbHangUp.Enabled = false;
                    this.tsbCheck.Enabled = false;
                    this.tsbTransfer.Enabled = false;
                    this.tsbListen.Enabled = false;
                    this.tsbWhisper.Enabled = false;
                    this.tsbBargein.Enabled = false;
                    this.tsbForceHangup.Enabled = false;
                    this.tsbThreeWay.Enabled = false;
                    this.tsbConsult.Enabled = false;
                    this.tsbStopConsult.Enabled = false;
                    this.tsbConsultTransfer.Enabled = false;
                    this.tsbHold.Enabled = false;
                    this.tsbMute.Enabled = false;
                    this.tsbKeyPad.Enabled = false;
                    this.tsbGrade.Enabled = false;
                    this.tslStatus.Text = "请摘机";
                    this.tslStatus.ToolTipText = "";
                    break;
                case AgentBar.Event_Type.WHISPER_SUCCESS:
                    switch (this.mAgentState)
                    {
                        case AgentBar.Agent_State.AGENT_STATUS_TALKING_EAVESDROP:
                            this.tsbListen.Enabled = true;
                            this.tsbBargein.Enabled = true;
                            this.tsbForceHangup.Enabled = true;
                            this.tsbHold.Enabled = true;
                            this.tsbMute.Enabled = true;
                            break;
                        case AgentBar.Agent_State.AGENT_STATUS_TALKING_BARGEIN:
                            this.tsbListen.Enabled = true;
                            this.tsbBargein.Enabled = true;
                            this.tsbForceHangup.Enabled = true;
                            this.tsbHold.Enabled = true;
                            this.tsbMute.Enabled = true;
                            break;
                    }
                    this.tsbWhisper.Enabled = false;
                    this.mAgentState = AgentBar.Agent_State.AGENT_STATUS_TALKING_WHISPER;
                    this.tslStatus.Text = "密语成功";
                    this.tslStatus.ToolTipText = "";
                    break;
                case AgentBar.Event_Type.WHISPER_FAIL:
                    this.tsbWhisper.Enabled = true;
                    this.tslStatus.Text = "密语失败";
                    this.tslStatus.ToolTipText = "";
                    break;
                case AgentBar.Event_Type.BARGE_IN_SUCCESS:
                    switch (this.mAgentState)
                    {
                        case AgentBar.Agent_State.AGENT_STATUS_TALKING_EAVESDROP:
                            this.tsbWhisper.Enabled = true;
                            this.tsbListen.Enabled = true;
                            this.tsbForceHangup.Enabled = true;
                            this.tsbHold.Enabled = true;
                            this.tsbMute.Enabled = true;
                            break;
                        case AgentBar.Agent_State.AGENT_STATUS_TALKING_WHISPER:
                            this.tsbWhisper.Enabled = true;
                            this.tsbListen.Enabled = true;
                            this.tsbForceHangup.Enabled = true;
                            break;
                    }
                    this.tsbBargein.Enabled = false;
                    this.mAgentState = AgentBar.Agent_State.AGENT_STATUS_TALKING_BARGEIN;
                    this.tslStatus.Text = "插话成功";
                    this.tslStatus.ToolTipText = "";
                    break;
                case AgentBar.Event_Type.BARGE_IN_FAIL:
                    this.tsbBargein.Enabled = true;
                    this.tslStatus.Text = "插话失败";
                    this.tslStatus.ToolTipText = "";
                    break;
                case AgentBar.Event_Type.FORCE_HANGUP_SUCCESS:
                    switch (this.mAgentState)
                    {
                        case AgentBar.Agent_State.AGENT_STATUS_TALKING_EAVESDROP:
                        case AgentBar.Agent_State.AGENT_STATUS_TALKING_WHISPER:
                        case AgentBar.Agent_State.AGENT_STATUS_TALKING_BARGEIN:
                            this.tsbCallOut.Enabled = false;
                            this.tsbCallAgent.Enabled = false;
                            this.tsbTransfer.Enabled = true;
                            this.tsbThreeWay.Enabled = true;
                            this.tsbConsult.Enabled = true;
                            this.tsbStopConsult.Enabled = false;
                            this.tsbConsultTransfer.Enabled = false;
                            this.tsbCheck.Enabled = false;
                            this.tsbHold.Enabled = true;
                            this.tsbMute.Enabled = true;
                            this.tsbHangUp.Enabled = true;
                            this.tsbGrade.Enabled = true;
                            this.tsbBargein.Enabled = false;
                            this.tsbListen.Enabled = false;
                            this.tsbWhisper.Enabled = false;
                            this.tslStatus.Text = "强拆成功";
                            this.tslStatus.ToolTipText = "";
                            break;
                    }
                    this.tsbForceHangup.Enabled = false;
                    this.mAgentState = AgentBar.Agent_State.AGENT_STATUS_TALKING_CUSTOMER;
                    break;
                case AgentBar.Event_Type.FORCE_HANGUP_FAIL:
                    this.tsbForceHangup.Enabled = true;
                    this.tslStatus.Text = "强拆失败";
                    this.tslStatus.ToolTipText = "";
                    break;
                case AgentBar.Event_Type.BLIND_TRANSFER_OUTBOUND_FAILED:
                    AgentBar.Log.Debug("收到盲外线电话失败事件");
                    this.tslStatus.Text = info;
                    this.tslStatus.ToolTipText = "";
                    this.tsbTransfer.Enabled = true;
                    break;
                case AgentBar.Event_Type.GET_ACCESS_NUMBERS_FAIL:
                    this.tsbCallOut.Enabled = true;
                    this.tslStatus.Text = "获取接入号失败";
                    this.tslStatus.ToolTipText = "";
                    break;
                case AgentBar.Event_Type.AGENT_STATUS_CHANGE_TO_IDLE:
                    if (this.mBindExten)
                    {
                        this.tsbCallAgent.Enabled = true;
                        this.tsbHangUp.Enabled = false;
                        this.tsbGrade.Enabled = false;
                        this.tsbCheck.Enabled = true;
                        this.tsbCallOut.Enabled = true;
                        this.tsbHold.Enabled = false;
                        this.tsbMute.Enabled = false;
                        this.tsbKeyPad.Enabled = false;
                        this.tsbThreeWay.Enabled = false;
                        this.tsbTransfer.Enabled = false;
                        this.tsbConsult.Enabled = false;
                        this.tsbConsultTransfer.Enabled = false;
                        this.tsbStopConsult.Enabled = false;
                        this.tsbListen.Enabled = true;
                        this.tsbWhisper.Enabled = true;
                        this.tsbBargein.Enabled = true;
                        this.tsbForceHangup.Enabled = true;
                        this.tsbState.Enabled = true;
                        this.mAgentState = AgentBar.Agent_State.AGENT_STATUS_IDLE;
                    }
                    break;
                case AgentBar.Event_Type.AGENT_STATUS_CHANGE_TO_RING:
                    this.tsbState.Image = Resources.callin;
                    this.tsbStateBack = AgentBar.AgentStatus2Str(AgentBar.Agent_Status.AGENT_STATUS_RING);
                    this.tsbState.AccessibleName = Convert.ToString(1);
                    this.tsbState.Enabled = false;
                    this.tsbAnswer.Enabled = true;
                    this.trmCall.Start();
                    this.tsbState.Size = new Size(180, 36);
                    this.needCleanUpTime = true;
                    break;
                case AgentBar.Event_Type.AGENT_STATUS_CHANGE_TO_TALKING:
                    if (this.needCleanUpTime)
                    {
                        this.trmCall.Stop();
                        this.timeCount = 1;
                    }
                    this.tsbState.Image = Resources.talk;
                    this.tsbStateBack = AgentBar.AgentStatus2Str(AgentBar.Agent_Status.AGENT_STATUS_TALKING);
                    this.tsbState.AccessibleName = Convert.ToString(2);
                    this.tsbState.Enabled = false;
                    this.tsbAnswer.Enabled = false;
                    this.trmCall.Start();
                    this.tsbState.Size = new Size(180, 36);
                    this.needCleanUpTime = false;
                    this.isTalking = true;
                    break;
                case AgentBar.Event_Type.AGENT_STATUS_CHANGE_TO_HOLD:
                    this.tsbState.Image = Resources._0034;
                    this.tsbState.Enabled = false;
                    this.tsbHold.Enabled = true;
                    this.tsbHold.AccessibleName = "unhold";
                    this.tsbMute.Enabled = false;
                    this.tsbKeyPad.Enabled = false;
                    this.tsbTransfer.Enabled = false;
                    this.tsbHangUp.Enabled = false;
                    this.tsbThreeWay.Enabled = false;
                    this.tsbConsult.Enabled = false;
                    this.tsbStopConsult.Enabled = false;
                    this.tsbGrade.Enabled = false;
                    this.tsbConsultTransfer.Enabled = false;
                    this.tsbListen.Enabled = false;
                    this.tsbWhisper.Enabled = false;
                    this.tsbBargein.Enabled = false;
                    this.tsbForceHangup.Enabled = false;
                    this.isTalking = false;
                    break;
                case AgentBar.Event_Type.AGENT_STATUS_CHANGE_TO_MUTE:
                    this.tsbState.Image = Resources.unMute;
                    this.tsbState.Enabled = false;
                    this.tsbHold.Enabled = false;
                    this.tsbMute.Enabled = true;
                    this.tsbKeyPad.Enabled = false;
                    this.tsbTransfer.Enabled = false;
                    this.tsbHangUp.Enabled = false;
                    this.tsbThreeWay.Enabled = false;
                    this.tsbConsult.Enabled = false;
                    this.tsbStopConsult.Enabled = false;
                    this.tsbGrade.Enabled = false;
                    this.tsbConsultTransfer.Enabled = false;
                    this.tsbListen.Enabled = false;
                    this.tsbWhisper.Enabled = false;
                    this.tsbBargein.Enabled = false;
                    this.tsbForceHangup.Enabled = false;
                    this.isTalking = false;
                    break;
                case AgentBar.Event_Type.AGENT_STATUS_CHANGE_TO_ACW:
                    this.tsbState.Image = Resources.acw;
                    this.tsbState.Text = AgentBar.AgentStatus2Str(AgentBar.Agent_Status.AGENT_STATUS_ACW);
                    this.tsbState.AccessibleName = Convert.ToString(4);
                    this.tsbState.Enabled = true;
                    break;
                case AgentBar.Event_Type.AGENT_STATUS_CHANGE_TO_CAMP_ON:
                    this.tsbState.Image = Resources.occupy;
                    this.tsbState.Text = AgentBar.AgentStatus2Str(AgentBar.Agent_Status.AGENT_STATUS_CAMP_ON);
                    this.tsbState.AccessibleName = Convert.ToString(5);
                    this.tsbState.Enabled = false;
                    this.tsbCallOut.Enabled = false;
                    this.tsbCallAgent.Enabled = false;
                    this.tsbTransfer.Enabled = false;
                    this.tsbHangUp.Enabled = false;
                    this.tsbCheck.Enabled = false;
                    this.tsbHold.Enabled = false;
                    this.tsbMute.Enabled = false;
                    this.tsbKeyPad.Enabled = false;
                    this.tsbThreeWay.Enabled = false;
                    this.tsbConsult.Enabled = false;
                    this.tsbStopConsult.Enabled = false;
                    this.tsbConsultTransfer.Enabled = false;
                    this.tsbListen.Enabled = false;
                    this.tsbWhisper.Enabled = false;
                    this.tsbBargein.Enabled = false;
                    this.tsbForceHangup.Enabled = false;
                    this.mAgentState = AgentBar.Agent_State.AGENT_STATUS_CAMP_ON;
                    break;
                case AgentBar.Event_Type.AGENT_STATUS_CHANGE_TO_CALLING_OUT:
                    this.trmCall.Stop();
                    this.timeCount = 1;
                    this.tsbState.Image = Resources.calling;
                    this.tsbStateBack = AgentBar.AgentStatus2Str(AgentBar.Agent_Status.AGENT_STATUS_CALLING_OUT);
                    this.tsbState.AccessibleName = Convert.ToString(10);
                    this.tsbState.Enabled = false;
                    this.tsbAnswer.Enabled = false;
                    this.trmCall.Start();
                    this.tsbState.Size = new Size(180, 36);
                    this.needCleanUpTime = true;
                    break;
                case AgentBar.Event_Type.SOFTPHONE_CALL_IN:
                    AgentBar.Log.Debug("收到内置软电话来电事件");
                    this.tsbAnswer.Enabled = true;
                    if (File.Exists(this.ringFileName))
                    {
                        this.sndPlayer.SoundLocation = this.ringFileName;
                        this.sndPlayer.Load();
                        this.sndPlayer.PlayLooping();
                    }
                    else
                    {
                        AgentBar.Log.Error("来电铃声文件 Ringback.wav 不存在！");
                    }
                    break;
                case AgentBar.Event_Type.SOFTPHONE_HANGUP:
                    AgentBar.Log.Debug("收到内置软电话挂断电事件");
                    if (this.sndPlayer != null)
                    {
                        this.sndPlayer.Stop();
                    }
                    this.tsbAnswer.Enabled = false;
                    break;
                case AgentBar.Event_Type.SOFTPHONE_ANSWER:
                    AgentBar.Log.Debug("收到内置软电话接听事件");
                    if (this.sndPlayer != null)
                    {
                        this.sndPlayer.Stop();
                    }
                    this.tsbAnswer.Enabled = false;
                    break;
                case AgentBar.Event_Type.AGENT_STATUS_RESTORE:
                    if (this.mBindExten)
                    {
                        this.tsbCallAgent.Enabled = true;
                        this.tsbHangUp.Enabled = false;
                        this.tsbGrade.Enabled = false;
                        this.tsbCheck.Enabled = true;
                        this.tsbCallOut.Enabled = true;
                        this.tsbHold.Enabled = false;
                        this.tsbMute.Enabled = false;
                        this.tsbKeyPad.Enabled = false;
                        this.tsbThreeWay.Enabled = false;
                        this.tsbTransfer.Enabled = false;
                        this.tsbConsult.Enabled = false;
                        this.tsbConsultTransfer.Enabled = false;
                        this.tsbStopConsult.Enabled = false;
                        this.tsbListen.Enabled = true;
                        this.tsbWhisper.Enabled = true;
                        this.tsbBargein.Enabled = true;
                        this.tsbForceHangup.Enabled = true;
                        this.tsbState.Enabled = true;
                        this.mAgentState = AgentBar.Agent_State.AGENT_STATUS_IDLE;
                    }
                    break;
                case AgentBar.Event_Type.APPLY_CHANGE_STATUS_APPLY_SUCCESS:
                    this.tsbCancelApplication.Enabled = true;
                    this.tslStatus.Text = info;
                    this.tslStatus.ToolTipText = "";
                    break;
                case AgentBar.Event_Type.APPLY_CHANGE_STATUS_APPLY_FAILED:
                    this.tsbCancelApplication.Enabled = false;
                    this.tslStatus.Text = info;
                    this.tslStatus.ToolTipText = "";
                    break;
                case AgentBar.Event_Type.APPLY_CHANGE_STATUS_CANCEL_SUCCESS:
                    this.tsbCancelApplication.Enabled = false;
                    this.tslStatus.Text = info;
                    this.tslStatus.ToolTipText = "";
                    break;
                case AgentBar.Event_Type.APPLY_CHANGE_STATUS_CANCEL_FAILED:
                    this.tslStatus.Text = info;
                    this.tslStatus.ToolTipText = "";
                    break;
                case AgentBar.Event_Type.APPLY_CHANGE_STATUS_NO_ANY_APPROVAL:
                    if (this.tsbApprove.Image != Resources.appoval2)
                    {
                        this.tsbApprove.Image = Resources.appoval2;
                        this.ApproveNormal = true;
                        if (this.other != null && this.other.DropDownItems.Contains(this.tsbApprove) && this.other.Image != Resources.other)
                        {
                            if (this.other.DropDownItems.Contains(this.tsbNoAnswerCalls) && this.NoAnswerCallNormal)
                            {
                                this.other.Image = Resources.other;
                                this.otherNormal = true;
                            }
                            if (!this.other.DropDownItems.Contains(this.tsbNoAnswerCalls))
                            {
                                this.other.Image = Resources.other;
                                this.otherNormal = true;
                            }
                        }
                    }
                    break;
                case AgentBar.Event_Type.APPLY_CHANGE_STATUS_SOME_APPLY:
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
                    break;
                case AgentBar.Event_Type.APPLY_CHANGE_STATUS_APPLY_OR_APPROVE_TIMEOUT:
                    this.tsbCancelApplication.Enabled = false;
                    this.tslStatus.Text = info;
                    this.tslStatus.ToolTipText = "";
                    break;
                case AgentBar.Event_Type.APPLY_CHANGE_STATUS_FINISHED:
                    this.tsbCancelApplication.Enabled = false;
                    this.tslStatus.Text = info;
                    this.tslStatus.ToolTipText = "";
                    break;
            }
            if (this.AgentBarUIChangedEvent != null)
            {
                this.AgentBarUIChangedEvent(eventType, info);
            }
        }

        private void tsbCallOut_Click(object sender, EventArgs e)
        {
            string routeNum = string.Empty;
            if (this.mAgentStatus != Convert.ToString(8))
            {
                int new_state = 8;
                if (!this.DoSetAgentDefineStatus(new_state, 1))
                {
                    MessageBox.Show("外呼失败！", "外呼失败", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    return;
                }
            }
            this.newFrmCallOut = new frmCallOut(this.mDID_Num);
            this.GetAccessNumberEvent = (AgentBar.GetAccessNumberEventHandler)Delegate.Combine(this.GetAccessNumberEvent, new AgentBar.GetAccessNumberEventHandler(this.newFrmCallOut.Evt_GetAccessNumber));
            if (!this.DoGetAccessNumbers())
            {
                MessageBox.Show("获取接入号失败,请先签入!", "获取接入号失败", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            if (this.newFrmCallOut.ShowDialog() == DialogResult.OK)
            {
                if (this.newFrmCallOut.cboRoute.Text == "不设置")
                {
                    routeNum = "";
                }
                else
                {
                    routeNum = this.newFrmCallOut.cboRoute.Text;
                }
                this.mDefaultAccessNum = routeNum;
                this.addToCallHistory(this.newFrmCallOut.cboCallID.Text);
                this.DoCallOut(this.newFrmCallOut.cboCallID.Text, this.mDefaultAccessNum);
            }
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

        private void tsbWhisper_Click(object sender, EventArgs e)
        {
            if (this.mAgentState == AgentBar.Agent_State.AGENT_STATUS_TALKING_BARGEIN || this.mAgentState == AgentBar.Agent_State.AGENT_STATUS_TALKING_EAVESDROP)
            {
                bool blnRet = this.DoWhisper(this.mEavesdropAgent);
                if (blnRet)
                {
                    this.tsbWhisper.Enabled = false;
                }
                else
                {
                    MessageBox.Show("密语失败！", "密语", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    this.tsbWhisper.Enabled = true;
                }
            }
            else
            {
                if (this.mAgentStatus != Convert.ToString(9))
                {
                    int new_state = 9;
                    if (!this.DoSetAgentDefineStatus(new_state, 1))
                    {
                        MessageBox.Show("更改坐席状态为：监控失败！", "更改坐席状态", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                        return;
                    }
                }
                this.newFrmTransfer = new FrmTransfer();
                this.newFrmTransfer.AgentBar = this;
                this.newFrmTransfer.AgentNum = this.mAgentID;
                this.newFrmTransfer.AgentDefineStatus = this.mAgentDefineStatus;
                this.newFrmTransfer.ControledAgentGroupLstStr = this.mMyRoleAndRight.controled_agent_group_lst;
                this.newFrmTransfer.AgentOrAgentGroup = AgentBar.Agent_Or_AgentGroup.AGENTGROUPNUM;
                this.newFrmTransfer.DoGetAgentOnlineEvent += new FrmTransfer.DoGetAgentOnlineEventHandler(this.newFrmTransfer_DoGetAgentOnlineEvent);
                this.GetAgentOnlineEvent = (AgentBar.GetOnlineAgentEventHandler)Delegate.Combine(this.GetAgentOnlineEvent, new AgentBar.GetOnlineAgentEventHandler(this.newFrmTransfer.Evt_GetOnlineAgent));
                this.GetAgentGroupListEvent = (AgentBar.GetAgentGroupListEventHandler)Delegate.Combine(this.GetAgentGroupListEvent, new AgentBar.GetAgentGroupListEventHandler(this.newFrmTransfer.Evt_Get_Agent_Group_List));
                if (!this.DoGetAgentGroupList(""))
                {
                    MessageBox.Show("获得坐席组列表失败！", "获得坐席组列表", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                }
                else
                {
                    this.newFrmTransfer.Text = "密语坐席";
                    this.newFrmTransfer.chkTransferAgent.Visible = false;
                    this.newFrmTransfer.chkTransferAgent.Tag = false;
                    this.newFrmTransfer.chkOutbound.Visible = false;
                    this.newFrmTransfer.chkOutbound.Tag = false;
                    if (this.newFrmTransfer.ShowDialog() == DialogResult.OK)
                    {
                        if (this.newFrmTransfer.txtAgentID.Text != this.mAgentID)
                        {
                            bool blnRet = this.DoWhisper(this.newFrmTransfer.txtAgentID.Text);
                            if (blnRet)
                            {
                                this.tsbWhisper.Enabled = false;
                            }
                            else
                            {
                                this.tsbWhisper.Enabled = true;
                                MessageBox.Show("密语失败！", "密语", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                            }
                        }
                        else
                        {
                            MessageBox.Show("不能密语自己！", "密语", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                        }
                    }
                }
            }
        }

        private void tsbConsultTransfer_Click(object sender, EventArgs e)
        {
            bool blnRet = this.DoConsultTransfer();
            if (blnRet)
            {
                this.tsbConsultTransfer.Enabled = false;
            }
            else
            {
                MessageBox.Show("询问转失败！", "询问转", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                this.tsbConsultTransfer.Enabled = true;
            }
        }

        private void tsmi_transferAgent_Click(object sender, EventArgs e)
        {
            bool TransfereeIsOutbound = false;
            this.newFrmTransfer = new FrmTransfer();
            this.newFrmTransfer.AgentBar = this;
            this.newFrmTransfer.AgentDefineStatus = this.mAgentDefineStatus;
            this.newFrmTransfer.AgentNum = this.mAgentID;
            this.newFrmTransfer.ControledAgentGroupLstStr = "";
            this.newFrmTransfer.AgentOrAgentGroup = AgentBar.Agent_Or_AgentGroup.ALL;
            this.newFrmTransfer.DoGetAgentOnlineEvent += new FrmTransfer.DoGetAgentOnlineEventHandler(this.newFrmTransfer_DoGetAgentOnlineEvent);
            this.GetAgentOnlineEvent = (AgentBar.GetOnlineAgentEventHandler)Delegate.Combine(this.GetAgentOnlineEvent, new AgentBar.GetOnlineAgentEventHandler(this.newFrmTransfer.Evt_GetOnlineAgent));
            this.GetAllAgentGroupListEvent = (AgentBar.GetAllAgentGroupListEventHandler)Delegate.Combine(this.GetAllAgentGroupListEvent, new AgentBar.GetAllAgentGroupListEventHandler(this.newFrmTransfer.Evt_Get_Agent_Group_List));
            if (!this.DoGetAgentGroupList(""))
            {
                MessageBox.Show("获取坐席组列表失败！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            if (this.newFrmTransfer.ShowDialog() == DialogResult.OK)
            {
                if (AgentBar.Str2AgentStatus(this.mAgentStatus) != AgentBar.Agent_Status.AGENT_STATUS_TALKING)
                {
                    MessageBox.Show("只有在通话时才能转接坐席！", "转接坐席", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                }
                else
                {
                    if (!this.newFrmTransfer.chkTransferAgent.Checked)
                    {
                        TransfereeIsOutbound = true;
                    }
                    if ((this.newFrmTransfer.txtAgentID.Text != this.mAgentID && !TransfereeIsOutbound) || TransfereeIsOutbound)
                    {
                        bool blnRet = this.DoTransferAgent(this.newFrmTransfer.txtAgentID.Text, this.newFrmTransfer.OutboundFlag);
                        if (blnRet)
                        {
                            this.tsbTransfer.Enabled = false;
                            this.transfercheck = true;
                        }
                        else
                        {
                            this.tsbTransfer.Enabled = true;
                            MessageBox.Show("转接坐席失败！", "转接坐席", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                        }
                    }
                    else
                    {
                        MessageBox.Show("不能转接自己！", "转接", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    }
                }
            }
        }

        private void tsmi_transferIvr_Click(object sender, EventArgs e)
        {
            FrmTransferIvr newFrmTransferIvr = new FrmTransferIvr();
            bool blnRet = this.DoGetIvrList();
            if (blnRet)
            {
                this.GetIvrListEvent = (AgentBar.GetIvrListEventHandler)Delegate.Combine(this.GetIvrListEvent, new AgentBar.GetIvrListEventHandler(newFrmTransferIvr.Evt_UpdateLvwIvr));
            }
            else
            {
                MessageBox.Show("获取IVR列表失败！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            if (newFrmTransferIvr.ShowDialog() == DialogResult.OK)
            {
                if (newFrmTransferIvr.txtIvrNum.Text != "")
                {
                    blnRet = this.DoTransferIvr(newFrmTransferIvr.txtIvrNum.Text);
                    if (blnRet)
                    {
                        this.tsbTransfer.Enabled = false;
                    }
                    else
                    {
                        this.tsbTransfer.Enabled = true;
                        MessageBox.Show("转接IVR失败！", "转接IVR", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    }
                }
            }
        }

        private void tsmi_transferQueue_Click(object sender, EventArgs e)
        {
            FrmTransferQueue newFrmTransfer = new FrmTransferQueue();
            bool blnRet = this.DoGetQueueList();
            if (blnRet)
            {
                this.GetQueueListEvent = (AgentBar.GetQueueListEventHandler)Delegate.Combine(this.GetQueueListEvent, new AgentBar.GetQueueListEventHandler(newFrmTransfer.Evt_UpdateLvwQueue));
            }
            else
            {
                MessageBox.Show("获取队列列表失败！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            if (newFrmTransfer.ShowDialog() == DialogResult.OK)
            {
                if (newFrmTransfer.txtQueueNum.Text != "")
                {
                    blnRet = this.DoTransferQueue(newFrmTransfer.txtQueueNum.Text);
                    if (blnRet)
                    {
                        this.tsbTransfer.Enabled = false;
                    }
                    else
                    {
                        this.tsbTransfer.Enabled = true;
                        MessageBox.Show("转接队列失败！", "转接队列", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    }
                }
            }
        }

        private void tsmi_transferIvr_Profile_Click(object sender, EventArgs e)
        {
            FrmTransferIvrProfile newFrmTransferIvrProfile = new FrmTransferIvrProfile();
            bool blnRet = this.DoGetIvrProfileList();
            if (blnRet)
            {
                this.GetIvrProfileListEvent = (AgentBar.GetIvrProfileListEventHandler)Delegate.Combine(this.GetIvrProfileListEvent, new AgentBar.GetIvrProfileListEventHandler(newFrmTransferIvrProfile.Evt_UpdateLvwIvrProfile));
            }
            else
            {
                MessageBox.Show("获取IVR Profile列表失败！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            if (DialogResult.OK == newFrmTransferIvrProfile.ShowDialog())
            {
                if (string.Empty != newFrmTransferIvrProfile.txtIvrProfileNum.Text.Trim())
                {
                    blnRet = this.DoTransferIvrProfile(newFrmTransferIvrProfile.txtIvrProfileNum.Text.Trim());
                    if (blnRet)
                    {
                        this.tsbTransfer.Enabled = false;
                    }
                    else
                    {
                        this.tsbTransfer.Enabled = true;
                        MessageBox.Show("转接IVR Profile失败！", "转接IVR Profile", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    }
                }
            }
        }

        private void tsbMonitor_Click(object sender, EventArgs e)
        {
            if (this.mMyRoleAndRight.rights_of_view_agent_group_info || this.mMyRoleAndRight.rights_of_view_queue_info)
            {
                this.tsmiMonitor_Click(sender, e);
            }
            else if (this.mMyRoleAndRight.role_right1)
            {
                this.tsmiScreen_Click(sender, e);
            }
        }

        private void newFrmMonitor_GetAgentsOfGroupEvent(string agentGroupNum)
        {
            AgentBar.Log.Debug("enter newFrmMonitor_GetAgentsOfGroupEvent .agentGroupNum:" + agentGroupNum);
            if (!this.DoGetAgentsOfAgentGroup(agentGroupNum))
            {
                MessageBox.Show("获取坐席组中的坐席失败！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
        }

        private void newFrmMonitor_GetAgentsOfQueueEvent(string QueueName)
        {
            AgentBar.Log.Debug("enter newFrmMonitor_GetAgentsOfQueueEvent .QueueName:" + QueueName);
            if (!this.DoGetAgentsOfQueue(QueueName))
            {
                MessageBox.Show("获取队列中的坐席失败！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
        }

        private void newFrmMonitor_GetCustomersOfQueueEvent(string QueueName)
        {
            AgentBar.Log.Debug("enter newFrmMonitor_GetCustomersOfQueueEvent .QueueName:" + QueueName);
            if (!this.DoGetCustomerOfQueue(QueueName))
            {
                MessageBox.Show("获取队列中的客户失败！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
        }

        private void newFrmMonitor_GetAgentStateEvent()
        {
            AgentBar.Log.Debug("enter newFrmMonitor_GetAgentStateEvent .");
            if (this.SendAgentStatusEvent != null)
            {
                this.SendAgentStatusEvent(this.mAgentState, this.mEavesdropAgent);
            }
        }

        private void newFrmMonitor_DoEavesDropEvent(string agentNum)
        {
            AgentBar.Log.Debug("enter newFrmMonitor_DoEavesDropEvent .agentNum:" + agentNum);
            bool blnRet = this.DoListen(agentNum);
            if (blnRet)
            {
                this.tsbListen.Enabled = false;
            }
            else
            {
                this.tsbListen.Enabled = true;
                MessageBox.Show("监听失败！", "监听坐席", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
        }

        private void newFrmMonitor_DoWhisperEvent(string agentNum)
        {
            AgentBar.Log.Debug("enter newFrmMonitor_DoWhisperEvent .agentNum:" + agentNum);
            bool blnRet = this.DoWhisper(agentNum);
            if (blnRet)
            {
                this.tsbWhisper.Enabled = false;
            }
            else
            {
                this.tsbWhisper.Enabled = true;
                MessageBox.Show("密语失败！", "密语坐席", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
        }

        private void newFrmMonitor_DoForceChangeStatusEvent(string agentNum, string status)
        {
            AgentBar.Log.Debug("enter newFrmMonitor_DoForceChangeStatusEvent .agentNum:" + agentNum + " status:" + status);
            bool blnRet = this.DoForceChangeStatusEvent(agentNum, status);
            if (!blnRet)
            {
                MessageBox.Show("强制改变坐席状态失败！", "目标坐席", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
        }

        private void newFrmMonitor_DoBargeinEvent(string agentNum)
        {
            AgentBar.Log.Debug("enter newFrmMonitor_DoBargeinEvent .agentNum:" + agentNum);
            bool blnRet = this.DoBargein(agentNum);
            if (blnRet)
            {
                this.tsbBargein.Enabled = false;
            }
            else
            {
                this.tsbBargein.Enabled = true;
                MessageBox.Show("插话失败！", "插话坐席", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
        }

        private void newFrmMonitor_DoForceHangupEvent(string agentNum)
        {
            AgentBar.Log.Debug("enter newFrmMonitor_DoForceHangupEvent .agentNum:" + agentNum);
            bool blnRet = this.DoForceHangup(agentNum);
            if (blnRet)
            {
                this.tsbForceHangup.Enabled = false;
            }
            else
            {
                this.tsbForceHangup.Enabled = true;
                MessageBox.Show("强拆失败！", "强拆坐席", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
        }

        private void newFrmMonitor_DoGetDetailCallInfoEvent(string targetAgentNum)
        {
            AgentBar.Log.Debug("enter newFrmMonitor_DoGetDetailCallInfoEvent .targetAgentNum:" + targetAgentNum);
            if (!this.DoGetDetailCallInfo(targetAgentNum))
            {
                MessageBox.Show("获取坐席的详细通话信息失败！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
        }

        private void tsbAnswer_Click(object sender, EventArgs e)
        {
            AgentBar.Log.Debug("enter tsbAnswer_Click .");
            if (!this.DoAnswer())
            {
                MessageBox.Show("接听失败！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
        }

        public static int PostMsgToSoftPhone(IntPtr _mSoftPhoneWindowHandle, string softPhone_app_className, string softPhone_app_name, int softPhone_msg_value, int softPhone_cmd, int softPhone_cmd2)
        {
            int result;
            if (softPhone_app_name != "")
            {
                if (_mSoftPhoneWindowHandle == IntPtr.Zero)
                {
                    _mSoftPhoneWindowHandle = AgentBar.GetProcessWindowHandle(softPhone_app_className, softPhone_app_name);
                }
                if (_mSoftPhoneWindowHandle != IntPtr.Zero)
                {
                    if (!AgentBar.PostMessageApi(_mSoftPhoneWindowHandle, softPhone_msg_value, softPhone_cmd, softPhone_cmd))
                    {
                        AgentBar.Log.Error(string.Concat(new object[]
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
                        AgentBar.Log.Error(string.Concat(new object[]
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
                    AgentBar.Log.Error("Answer failed!!reason:softphone handle is Not Exist!");
                    result = -2;
                }
            }
            else
            {
                AgentBar.Log.Error("Answer failed!!reason:softphone app name is empty!!");
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
                    AgentBar.Log.Error("PostMessageApi is failed!!one of parameter is zero!");
                    result = false;
                }
                else
                {
                    AgentBar.PostMessage(hwnd, msg_value, (IntPtr)wP, (IntPtr)lP);
                    result = true;
                }
            }
            catch (Exception e)
            {
                AgentBar.Log.Error(string.Concat(new string[]
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

        private void tsmiAcw_Click(object sender, EventArgs e)
        {
            this.tsddbAfterHangup.Image = this.tsmiAcw.Image;
            this.tsddbAfterHangup.ToolTipText = this.tsmiAcw.ToolTipText;
            this.mAgentStateAfterHangup = AgentBar.Agent_Status.AGENT_STATUS_ACW;
            this.tsddbAfterHangup.Size = new Size(125, 36);
        }

        private void tsmiIdle_Click(object sender, EventArgs e)
        {
            this.tsddbAfterHangup.Image = this.tsmiIdle.Image;
            this.tsddbAfterHangup.ToolTipText = this.tsmiIdle.ToolTipText;
            this.mAgentStateAfterHangup = AgentBar.Agent_Status.AGENT_STATUS_IDLE;
            this.tsddbAfterHangup.Size = new Size(125, 36);
        }

        private void tsbGrade_ButtonClick(object sender, EventArgs e)
        {
            if (MessageBox.Show("您确定要转到评分流程吗？", "评分", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                this.DoGrade("zh");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.mSoftPhone.PL_Hold();
        }

        private void tsbState_Click(object sender, EventArgs e)
        {
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

        private void tsmiScreen_Click(object sender, EventArgs e)
        {
            if (this.newFrmMonitorScreen == null || this.newFrmMonitorScreen.IsDisposed)
            {
                this.newFrmMonitorScreen = new FrmMonitorScreen();
                this.newFrmMonitorScreen.MyAgentNum = this.mAgentID;
                this.newFrmMonitorScreen.AgentBar = this;
                this.newFrmMonitorScreen.Show();
                this.GetAllQueueStatisEvent = (AgentBar.GetAllQueueStatisEventHandler)Delegate.Combine(this.GetAllQueueStatisEvent, new AgentBar.GetAllQueueStatisEventHandler(this.newFrmMonitorScreen.Evt_Get_All_Queue_Statis));
                this.AgentStatusChangeEvent = (AgentBar.AgentStatusChangeEventHandler)Delegate.Combine(this.AgentStatusChangeEvent, new AgentBar.AgentStatusChangeEventHandler(this.newFrmMonitorScreen.Evt_Agent_Status_Change));
                this.AddCustomerToQueueEvent = (AgentBar.AddCustomerToQueueEventHandler)Delegate.Combine(this.AddCustomerToQueueEvent, new AgentBar.AddCustomerToQueueEventHandler(this.newFrmMonitorScreen.Evt_Add_Customer_To_Queue));
                this.DelCustomerFromQueueEvent = (AgentBar.DelCustomerFromQueueEventHandler)Delegate.Combine(this.DelCustomerFromQueueEvent, new AgentBar.DelCustomerFromQueueEventHandler(this.newFrmMonitorScreen.Evt_Del_Customer_From_Queue));
                this.GetReportStatisInfoEvent = (AgentBar.GetReportStatisInfoEventHandler)Delegate.Combine(this.GetReportStatisInfoEvent, new AgentBar.GetReportStatisInfoEventHandler(this.newFrmMonitorScreen.Evt_GetReportStatisInfoEvent));
                if (!this.DoGetAllQueueStatis())
                {
                    MessageBox.Show("获取所有队列统计信息失败！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                }
                if (!this.DoGetReportStatisInfo())
                {
                    MessageBox.Show("获取报表统计失败！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                }
            }
            else
            {
                this.newFrmMonitorScreen.Show();
                this.newFrmMonitorScreen.Activate();
            }
        }

        private void tsmiMonitor_Click(object sender, EventArgs e)
        {
            if (this.newFrmMonitor == null || this.newFrmMonitor.IsDisposed)
            {
                this.newFrmMonitor = new FrmMonitor();
                this.newFrmMonitor.AgentDefineStatus = this.mAgentDefineStatus;
                this.newFrmMonitor.MyAgentNum = this.mAgentID;
                this.newFrmMonitor.AgentBar = this;
                this.newFrmMonitor.WindowState = this.newFrmMonitor.LastWindowsState;
                this.newFrmMonitor.Show();
                this.newFrmMonitor.Activate();
                this.newFrmMonitor.GetAgentsOfGroupEvent += new FrmMonitor.GetAgentsOfGroupEventHandler(this.newFrmMonitor_GetAgentsOfGroupEvent);
                this.newFrmMonitor.GetAgentsOfQueueEvent += new FrmMonitor.GetAgentsOfQueueEventHandler(this.newFrmMonitor_GetAgentsOfQueueEvent);
                this.newFrmMonitor.GetCustomersOfQueueEvent += new FrmMonitor.GetCustomersOfQueueEventHandler(this.newFrmMonitor_GetCustomersOfQueueEvent);
                this.newFrmMonitor.DoGetDetailCallInfoEvent += new FrmMonitor.DoGetDetailCallInfoEventHandler(this.newFrmMonitor_DoGetDetailCallInfoEvent);
                this.newFrmMonitor.DoEavesDropEvent += new FrmMonitor.DoEavesDropEventHandler(this.newFrmMonitor_DoEavesDropEvent);
                this.newFrmMonitor.DoWhisperEvent += new FrmMonitor.DoWhisperEventHandler(this.newFrmMonitor_DoWhisperEvent);
                this.newFrmMonitor.DoBargeinEvent += new FrmMonitor.DoBargeinEventHandler(this.newFrmMonitor_DoBargeinEvent);
                this.newFrmMonitor.DoForceHangupEvent += new FrmMonitor.DoForceHangupEventHandler(this.newFrmMonitor_DoForceHangupEvent);
                this.newFrmMonitor.DoForceChangeStatusEvent += new FrmMonitor.DoForceChangeStatusEventHandler(this.newFrmMonitor_DoForceChangeStatusEvent);
                this.GetAgentGroupListEvent = (AgentBar.GetAgentGroupListEventHandler)Delegate.Combine(this.GetAgentGroupListEvent, new AgentBar.GetAgentGroupListEventHandler(this.newFrmMonitor.Evt_Get_Agent_Group_List));
                this.GetQueueListEvent = (AgentBar.GetQueueListEventHandler)Delegate.Combine(this.GetQueueListEvent, new AgentBar.GetQueueListEventHandler(this.newFrmMonitor.Evt_Get_Agent_Queue_List));
                this.GetAgentsOfAgentGroupEvent = (AgentBar.GetAgentsOfAgentGroupEventHandler)Delegate.Combine(this.GetAgentsOfAgentGroupEvent, new AgentBar.GetAgentsOfAgentGroupEventHandler(this.newFrmMonitor.Evt_Get_Agents_Of_AgentGroup));
                this.GetAgentsOfQueueEvent = (AgentBar.GetAgentsOfQueueEventHandler)Delegate.Combine(this.GetAgentsOfQueueEvent, new AgentBar.GetAgentsOfQueueEventHandler(this.newFrmMonitor.Evt_Get_Agents_Of_Queue));
                this.GetAgentsMonitorInfoEvent = (AgentBar.GetAgentsMonitorInfoEventHandler)Delegate.Combine(this.GetAgentsMonitorInfoEvent, new AgentBar.GetAgentsMonitorInfoEventHandler(this.newFrmMonitor.Evt_Get_Agents_Monitor_Info));
                this.GetCustomerOfQueueEvent = (AgentBar.GetCustomerOfQueueEventHandler)Delegate.Combine(this.GetCustomerOfQueueEvent, new AgentBar.GetCustomerOfQueueEventHandler(this.newFrmMonitor.Evt_Get_Customer_Monitor_Info));
                this.GetQueueStatisLstEvent = (AgentBar.GetQueueStatisLstEventHandler)Delegate.Combine(this.GetQueueStatisLstEvent, new AgentBar.GetQueueStatisLstEventHandler(this.newFrmMonitor.Evt_Get_Queue_Statis_List));
                this.AgentStatusChangeEvent = (AgentBar.AgentStatusChangeEventHandler)Delegate.Combine(this.AgentStatusChangeEvent, new AgentBar.AgentStatusChangeEventHandler(this.newFrmMonitor.Evt_Agent_Status_Change));
                this.AddCustomerToQueueEvent = (AgentBar.AddCustomerToQueueEventHandler)Delegate.Combine(this.AddCustomerToQueueEvent, new AgentBar.AddCustomerToQueueEventHandler(this.newFrmMonitor.Evt_Add_Customer_To_Queue));
                this.DelCustomerFromQueueEvent = (AgentBar.DelCustomerFromQueueEventHandler)Delegate.Combine(this.DelCustomerFromQueueEvent, new AgentBar.DelCustomerFromQueueEventHandler(this.newFrmMonitor.Evt_Del_Customer_From_Queue));
                this.UpdateCustomerOfQueueEvent = (AgentBar.UpdateCustomerOfQueueEventHandler)Delegate.Combine(this.UpdateCustomerOfQueueEvent, new AgentBar.UpdateCustomerOfQueueEventHandler(this.newFrmMonitor.Evt_Update_Customer_Of_Queue));
                this.GetDetailCallInfoEvent = (AgentBar.GetDetailCallInfoEventHandler)Delegate.Combine(this.GetDetailCallInfoEvent, new AgentBar.GetDetailCallInfoEventHandler(this.newFrmMonitor.Evt_Get_Detail_Call_Info));
                this.AgentBarUIChangedEvent = (AgentBar.AgentBarUIChangedEventHandler)Delegate.Combine(this.AgentBarUIChangedEvent, new AgentBar.AgentBarUIChangedEventHandler(this.newFrmMonitor.Evt_AgentBar_AgentBarUIChangedEvent));
                if (!this.DoGetAgentGroupList("all"))
                {
                    MessageBox.Show("获取坐席组列表失败！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                }
                if (!this.DoGetQueueList())
                {
                    MessageBox.Show("获取队列失败！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                }
            }
            else
            {
                this.newFrmMonitor.WindowState = this.newFrmMonitor.LastWindowsState;
                this.newFrmMonitor.Show();
                this.newFrmMonitor.Activate();
            }
        }

        private void tsmiApproval_Click(object sender, EventArgs e)
        {
            FrmApplication newFrmApplication = new FrmApplication();
            newFrmApplication.Show();
        }

        private void tsbApprove_Click(object sender, EventArgs e)
        {
            if (this.newFrmApplication == null || this.newFrmApplication.IsDisposed)
            {
                this.newFrmApplication = new FrmApplication();
                this.ApplyChangeStatusDistributeEvent = (AgentBar.ApplyChangeStatusDistributeEventHandler)Delegate.Combine(this.ApplyChangeStatusDistributeEvent, new AgentBar.ApplyChangeStatusDistributeEventHandler(this.newFrmApplication.AgentBar_ApplyChangeStatusDistributeEvent));
                this.GetAgentGroupListEvent = (AgentBar.GetAgentGroupListEventHandler)Delegate.Combine(this.GetAgentGroupListEvent, new AgentBar.GetAgentGroupListEventHandler(this.newFrmApplication.AgentBar_GetAgentGroupListEvent));
                this.GetAgentGroupStatusMaxNumEvent = (AgentBar.GetAgentGroupStatusMaxNumEventHandler)Delegate.Combine(this.GetAgentGroupStatusMaxNumEvent, new AgentBar.GetAgentGroupStatusMaxNumEventHandler(this.newFrmApplication.AgentBar_GetAgentGroupStatusMaxNumEvent));
                this.ApproveChangeStatusDistributeEvent = (AgentBar.ApproveChangeStatusDistributeEventHandler)Delegate.Combine(this.ApproveChangeStatusDistributeEvent, new AgentBar.ApproveChangeStatusDistributeEventHandler(this.newFrmApplication.AgentBar_ApproveChangeStatusDistributeEvent));
                this.ApplyChangeStatusCancelEvent = (AgentBar.ApplyChangeStatusCancelEventHandler)Delegate.Combine(this.ApplyChangeStatusCancelEvent, new AgentBar.ApplyChangeStatusCancelEventHandler(this.newFrmApplication.AgentBar_ApplyChangeStatusCancelEvent));
                this.GetChangeStatusApplyListEvent = (AgentBar.GetChangeStatusApplyListEventHandler)Delegate.Combine(this.GetChangeStatusApplyListEvent, new AgentBar.GetChangeStatusApplyListEventHandler(this.newFrmApplication.AgentBar_GetChangeStatusApplyListEvent));
                this.ApproveChangeStatusTimeoutDistributeEvent = (AgentBar.ApproveChangeStatusTimeoutEventDistributeHandler)Delegate.Combine(this.ApproveChangeStatusTimeoutDistributeEvent, new AgentBar.ApproveChangeStatusTimeoutEventDistributeHandler(this.newFrmApplication.AgentBar_ApproveChangeStatusTimeoutDistributeEvent));
                this.AgentStatusChangeEvent = (AgentBar.AgentStatusChangeEventHandler)Delegate.Combine(this.AgentStatusChangeEvent, new AgentBar.AgentStatusChangeEventHandler(this.newFrmApplication.AgentBar_AgentStatusChangeEvent));
                this.newFrmApplication.agentBar1 = this;
                this.newFrmApplication.WindowState = this.newFrmApplication.LastWindowsState;
                this.newFrmApplication.Show();
                this.newFrmApplication.Activate();
            }
            else
            {
                this.newFrmApplication.WindowState = this.newFrmApplication.LastWindowsState;
                this.newFrmApplication.Show();
                this.newFrmApplication.Activate();
            }
        }

        private void tsbCancelApplication_Click(object sender, EventArgs e)
        {
            if (this.my_apply_change_status.applyState == Apply_State.Apply_State_Applying || this.my_apply_change_status.applyState == Apply_State.Apply_State_Exeute)
            {
                bool rt = this.DoApplyCancel(7.ToString());
                if (rt)
                {
                    return;
                }
            }
            MessageBox.Show("取消申请失败！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }

        private void tsbKeyPad_Click(object sender, EventArgs e)
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

        private void tsbMute_Click(object sender, EventArgs e)
        {
            this.tsbMute.Enabled = false;
            if (this.tsbMute.AccessibleName == "mute")
            {
                if (!this.DoMute())
                {
                    MessageBox.Show("静音失败！", "静音", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    this.tsbMute.Enabled = true;
                }
            }
            else if (!this.DoStopMute())
            {
                MessageBox.Show("取消静音失败！", "取消静音", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                this.tsbMute.Enabled = true;
            }
        }

        private void tsbHold_Click(object sender, EventArgs e)
        {
            if (this.tsbHold.AccessibleName == "hold")
            {
                if (!this.DoHold())
                {
                    MessageBox.Show("保持失败！", "保持", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                }
            }
            else if (!this.DoStopHold())
            {
                MessageBox.Show("取消保持失败！", "取消保持", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
        }

        private void tsbThreeWay_Click(object sender, EventArgs e)
        {
            if ("threeway" == this.tsbThreeWay.AccessibleName)
            {
                bool trdIsOutbound = false;
                this.newFrmTransfer = new FrmTransfer();
                this.newFrmTransfer.AgentBar = this;
                this.newFrmTransfer.AgentDefineStatus = this.mAgentDefineStatus;
                this.newFrmTransfer.AgentNum = this.mAgentID;
                this.newFrmTransfer.ControledAgentGroupLstStr = "";
                this.newFrmTransfer.AgentOrAgentGroup = AgentBar.Agent_Or_AgentGroup.ALL;
                this.newFrmTransfer.DoGetAgentOnlineEvent += new FrmTransfer.DoGetAgentOnlineEventHandler(this.newFrmTransfer_DoGetAgentOnlineEvent);
                this.GetAgentOnlineEvent = (AgentBar.GetOnlineAgentEventHandler)Delegate.Combine(this.GetAgentOnlineEvent, new AgentBar.GetOnlineAgentEventHandler(this.newFrmTransfer.Evt_GetOnlineAgent));
                this.GetAllAgentGroupListEvent = (AgentBar.GetAllAgentGroupListEventHandler)Delegate.Combine(this.GetAllAgentGroupListEvent, new AgentBar.GetAllAgentGroupListEventHandler(this.newFrmTransfer.Evt_Get_Agent_Group_List));
                if (!this.DoGetAgentGroupList(""))
                {
                    MessageBox.Show("获得坐席组列表失败！", "获得坐席组列表", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                }
                else
                {
                    this.newFrmTransfer.Text = "三方通话";
                    if (this.newFrmTransfer.ShowDialog() == DialogResult.OK)
                    {
                        if (AgentBar.Str2AgentStatus(this.mAgentStatus) != AgentBar.Agent_Status.AGENT_STATUS_TALKING)
                        {
                            MessageBox.Show("只有在通话时才能发起三方！", "三方通话", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                        }
                        else
                        {
                            if (!this.newFrmTransfer.chkTransferAgent.Checked)
                            {
                                trdIsOutbound = true;
                            }
                            if ((!trdIsOutbound && this.newFrmTransfer.txtAgentID.Text != this.mAgentID) || trdIsOutbound)
                            {
                                if (!this.DoThreeWay(this.newFrmTransfer.txtAgentID.Text, trdIsOutbound))
                                {
                                    this.tsbThreeWay.Enabled = true;
                                    MessageBox.Show("三方通话失败！", "三方通话", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                                }
                            }
                            else
                            {
                                MessageBox.Show("不能呼叫自己！", "三方通话", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                            }
                        }
                    }
                }
            }
            else if (!this.DoThreeWayCancel())
            {
                MessageBox.Show("取消三方通话失败！", "取消三方通话失败", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
        }

        private void tsmiRestore_Click(object sender, EventArgs e)
        {
            this.tsddbAfterHangup.Image = this.tsmiRestore.Image;
            this.tsddbAfterHangup.ToolTipText = this.tsmiRestore.ToolTipText;
            this.mAgentStateAfterHangup = AgentBar.Agent_Status.AGENT_STATUS_RESTORE;
            this.tsddbAfterHangup.Size = new Size(125, 36);
        }

        public void ProcessAgentBarJsonDelegate(params object[] argsList)
        {
            int i = 0;
            try
            {
                AgentBar.JsonEventHandler jsonEvent = (AgentBar.JsonEventHandler)argsList[0];
                string[] strArgsLst = (string[])argsList[1];
                if (jsonEvent == null && strArgsLst.Length <= 0)
                {
                    AgentBar.Log.Error("jsonEvent is null or strArgsLst.length <=0 !!");
                }
                else
                {
                    Dictionary<string, string> jsonDic = new Dictionary<string, string>();
                    for (int j = 2; j < argsList.Length; j++)
                    {
                        jsonDic.Add(strArgsLst[i], ComFunc.object2Json(argsList[j]));
                        i++;
                    }
                    string argsJson = ComFunc.object2Json(jsonDic);
                    jsonEvent(argsJson);
                }
            }
            catch (Exception ex)
            {
                AgentBar.Log.Error(ex.Message + ex.Source + ex.StackTrace);
            }
        }

        private void tsbNoAnswerCalls_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.mNoAnswerCallsURL))
            {
                MessageBox.Show("未设置获取未接来电数据接口地址！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            else
            {
                if (this.tsbNoAnswerCalls.Image != Resources.NoAnswerCall_normal)
                {
                    this.tsbNoAnswerCalls.Image = Resources.NoAnswerCall_normal;
                    this.NoAnswerCallNormal = true;
                    if (this.other != null && this.other.DropDownItems.Contains(this.tsbNoAnswerCalls) && this.other.Image != Resources.other)
                    {
                        if (this.other.DropDownItems.Contains(this.tsbApprove) && this.ApproveNormal)
                        {
                            this.other.Image = Resources.other;
                            this.otherNormal = true;
                        }
                        if (!this.other.DropDownItems.Contains(this.tsbApprove))
                        {
                            this.other.Image = Resources.other;
                            this.otherNormal = true;
                        }
                    }
                }
                if (this.frmNoAnswerCalls == null || this.frmNoAnswerCalls.IsDisposed)
                {
                    this.frmNoAnswerCalls = new FrmNoAnswerCalls(this.AgentID, this.AgentPwd);
                    this.frmNoAnswerCalls.DoCallBackOuterEvent += new FrmNoAnswerCalls.DoCallBackOuterEventHandler(this.DoCallOut);
                    this.frmNoAnswerCalls.DoCallBackInterEvent += new FrmNoAnswerCalls.DoCallBackInterEventHandler(this.DoCallAgent);
                    this.AgentStatusChangeEvent = (AgentBar.AgentStatusChangeEventHandler)Delegate.Combine(this.AgentStatusChangeEvent, new AgentBar.AgentStatusChangeEventHandler(this.frmNoAnswerCalls.update_agent_status));
                    this.frmNoAnswerCalls.SetHttpServerInfo(this.mNoAnswerCallsURL);
                    this.frmNoAnswerCalls.Agent_status = this.mAgentStatus;
                    this.frmNoAnswerCalls.Show();
                    this.frmNoAnswerCalls.Activate();
                }
                else
                {
                    this.frmNoAnswerCalls.Show();
                    this.frmNoAnswerCalls.Activate();
                }
            }
        }

        public int GetAgentLocation(string agentText)
        {
            int count = 0;
            foreach (KeyValuePair<string, bool> i in this.controls_count_previous)
            {
                if (i.Key == agentText)
                {
                    count++;
                    break;
                }
                if (!i.Value)
                {
                    count++;
                }
            }
            return count;
        }

        private void AgentBar_Load(object sender, EventArgs e)
        {
            this.addAndInit(this.controls_count_previous, true);
            this.addAndInit(this.controls_count_current, false);
        }

        public void addAndInit(Dictionary<string, bool> controls, bool value)
        {
            controls.Add("tsbCancelApplication", value);
            controls.Add("tsbCallAgent", value);
            controls.Add("tsbCallOut", value);
            controls.Add("tsbTransfer", value);
            controls.Add("tsbGrade", value);
            controls.Add("tsbHold", value);
            controls.Add("tsbMute", value);
            controls.Add("tsbThreeWay", value);
            controls.Add("tsbConsult", value);
            controls.Add("tsbStopConsult", value);
            controls.Add("tsbConsultTransfer", value);
            controls.Add("tsSeparator1", false);
            controls.Add("tsbListen", value);
            controls.Add("tsbWhisper", value);
            controls.Add("tsbBargein", value);
            controls.Add("tsbForceHangup", value);
            controls.Add("tsSeparator2", false);
            controls.Add("tsddbAfterHangup", value);
            controls.Add("tsbCheck", value);
            controls.Add("tsbMonitor", value);
            controls.Add("tsbApprove", value);
            controls.Add("tsbNoAnswerCalls", value);
            controls.Add("basic", true);
            controls.Add("advanced", true);
            controls.Add("other", true);
        }

        public void GetAgentLocationInfo(Dictionary<string, bool> controls)
        {
            controls["tsbCancelApplication"] = this.controls_info.chkCancelApplicationInfo;
            controls["tsbCallAgent"] = this.controls_info.chkCallAgentInfo;
            controls["tsbCallOut"] = this.controls_info.chkCallOutInfo;
            controls["tsbTransfer"] = this.controls_info.chkTransferInfo;
            controls["tsbGrade"] = this.controls_info.chkGradeInfo;
            controls["tsbHold"] = this.controls_info.chkHoldInfo;
            controls["tsbMute"] = this.controls_info.chkMuteInfo;
            controls["tsbThreeWay"] = this.controls_info.chkThreeWayInfo;
            controls["tsbConsult"] = this.controls_info.chkConsultInfo;
            controls["tsbStopConsult"] = this.controls_info.chkStopConsultInfo;
            controls["tsbConsultTransfer"] = this.controls_info.chkConsultTransferInfo;
            controls["tsbListen"] = this.controls_info.chkListenInfo;
            controls["tsbWhisper"] = this.controls_info.chkWhisperInfo;
            controls["tsbBargein"] = this.controls_info.chkBargeinInfo;
            controls["tsbForceHangup"] = this.controls_info.chkForceHangupInfo;
            controls["tsddbAfterHangup"] = this.controls_info.chkdbAfterHangupInfo;
            controls["tsbCheck"] = this.controls_info.chkCheckInfo;
            controls["tsbMonitor"] = this.controls_info.chkMonitorInfo;
            controls["tsbApprove"] = this.controls_info.chkApproveInfo;
            controls["tsbNoAnswerCalls"] = this.controls_info.chkNoAnswerCallsInfo;
        }

        public void tsbitem_remove(ToolStripButton item)
        {
            if (!this.controls_count_current[item.Name])
            {
                int ucount = this.GetAgentLocation(item.Name) + 5;
                this.agentTool.Items.Insert(ucount, item);
                this.controls_count_previous[item.Name] = false;
                item.DisplayStyle = ToolStripItemDisplayStyle.Image;
            }
        }

        public void tssbitem_remove(ToolStripSplitButton item)
        {
            if (!this.controls_count_current[item.Name])
            {
                int ucount = this.GetAgentLocation(item.Name) + 5;
                this.agentTool.Items.Insert(ucount, item);
                this.controls_count_previous[item.Name] = false;
                item.DisplayStyle = ToolStripItemDisplayStyle.Image;
            }
        }

        public void tsddbitem_remove(ToolStripDropDownButton item)
        {
            if (!this.controls_count_current[item.Name])
            {
                int ucount = this.GetAgentLocation(item.Name) + 5;
                this.agentTool.Items.Insert(ucount, item);
                this.controls_count_previous[item.Name] = false;
                item.DisplayStyle = ToolStripItemDisplayStyle.Image;
            }
        }

        public void addControls(ToolStripDropDownButton temp, string controlsName)
        {
            if (!this.agentTool.Items.Contains(temp))
            {
                int ucount = this.GetAgentLocation(controlsName) + 5;
                this.agentTool.Items.Add(temp);
                this.controls_count_previous[controlsName] = false;
                temp.ImageAlign = ContentAlignment.MiddleLeft;
                temp.ImageTransparentColor = Color.Magenta;
                temp.MergeAction = MergeAction.Replace;
                temp.DisplayStyle = ToolStripItemDisplayStyle.Image;
                temp.DoubleClickEnabled = true;
                temp.Size = new Size(45, 36);
            }
        }

        public void reInsertControls(ToolStripDropDownButton temp, string controlsName)
        {
            if (this.agentTool.Items.Contains(temp))
            {
                int ucount = this.GetAgentLocation(controlsName) + 5;
                this.agentTool.Items.Insert(ucount, temp);
            }
        }

        public void addButtonToDropDownButton(ToolStripDropDownButton name, bool chkControlInfo, ToolStripButton temp)
        {
            if (temp == this.tsbCallOut || temp == this.tsbListen || temp == this.tsbWhisper || temp == this.tsbBargein || temp == this.tsbForceHangup || temp == this.tsbApprove)
            {
                temp.Tag = temp.Visible;
            }
            if (chkControlInfo && this.agentTool.Items.Contains(temp))
            {
                int ret = name.DropDownItems.Add(temp);
                this.controls_count_previous[temp.Name] = true;
                temp.DisplayStyle = ToolStripItemDisplayStyle.ImageAndText;
                temp.TextImageRelation = TextImageRelation.ImageBeforeText;
            }
        }

        public void addTsddButtonToDropDownButton(ToolStripDropDownButton name, bool chkControlInfo, ToolStripDropDownButton temp)
        {
            temp.Tag = temp.Visible;
            if (chkControlInfo && this.agentTool.Items.Contains(temp))
            {
                int ret = name.DropDownItems.Add(temp);
                this.controls_count_previous[temp.Name] = true;
                temp.DisplayStyle = ToolStripItemDisplayStyle.ImageAndText;
                temp.TextImageRelation = TextImageRelation.ImageBeforeText;
                if (this.tsddbAfterHangup == temp)
                {
                    this.tsddbAfterHangup.Size = new Size(155, 36);
                }
            }
        }

        public void addTssButtonToDropDownButton(ToolStripDropDownButton name, bool chkControlInfo, ToolStripSplitButton temp)
        {
            if (temp == this.tsbMonitor)
            {
                temp.Tag = temp.Visible;
            }
            if (chkControlInfo && this.agentTool.Items.Contains(temp))
            {
                int ret = name.DropDownItems.Add(temp);
                this.controls_count_previous[temp.Name] = true;
                temp.DisplayStyle = ToolStripItemDisplayStyle.ImageAndText;
                temp.TextImageRelation = TextImageRelation.ImageBeforeText;
            }
        }

        public void addBasicFunction()
        {
            this.basic.Image = Resources.basic;
            this.basic.Name = "basic";
            this.basic.Size = new Size(50, 48);
            this.basic.ToolTipText = "基础功能";
            this.basic.Visible = true;
            if (this.agentTool.Items.Contains(this.basic))
            {
                for (int i = this.basic.DropDownItems.Count - 1; i >= 0; i--)
                {
                    ToolStripButton tsbitem = this.basic.DropDownItems[i] as ToolStripButton;
                    if (tsbitem != null)
                    {
                        this.tsbitem_remove(tsbitem);
                    }
                    else
                    {
                        ToolStripSplitButton tssbitem = this.basic.DropDownItems[i] as ToolStripSplitButton;
                        if (tssbitem != null)
                        {
                            this.tssbitem_remove(tssbitem);
                        }
                        else
                        {
                            ToolStripDropDownButton tsddbitem = this.basic.DropDownItems[i] as ToolStripDropDownButton;
                            if (tsddbitem != null)
                            {
                                this.tsddbitem_remove(tsddbitem);
                            }
                        }
                    }
                }
                if (this.basic.DropDownItems.Count == 0)
                {
                    this.agentTool.Items.Remove(this.basic);
                    this.controls_count_previous["basic"] = true;
                }
            }
            if (this.controls_info.chkHoldInfo || this.controls_info.chkMuteInfo || this.controls_info.chkThreeWayInfo || this.controls_info.chkConsultInfo || this.controls_info.chkStopConsultInfo || this.controls_info.chkConsultTransferInfo || this.controls_info.chkTransferInfo || this.controls_info.chkGradeInfo)
            {
                this.addControls(this.basic, "basic");
                this.addButtonToDropDownButton(this.basic, this.controls_info.chkHoldInfo, this.tsbHold);
                this.addButtonToDropDownButton(this.basic, this.controls_info.chkMuteInfo, this.tsbMute);
                this.addButtonToDropDownButton(this.basic, this.controls_info.chkThreeWayInfo, this.tsbThreeWay);
                this.addButtonToDropDownButton(this.basic, this.controls_info.chkConsultInfo, this.tsbConsult);
                this.addButtonToDropDownButton(this.basic, this.controls_info.chkStopConsultInfo, this.tsbStopConsult);
                this.addButtonToDropDownButton(this.basic, this.controls_info.chkConsultTransferInfo, this.tsbConsultTransfer);
                this.addTsddButtonToDropDownButton(this.basic, this.controls_info.chkTransferInfo, this.tsbTransfer);
                this.addTssButtonToDropDownButton(this.basic, this.controls_info.chkGradeInfo, this.tsbGrade);
            }
        }

        public void addAdvancedFunction()
        {
            this.advanced.Image = Resources.advanced;
            this.advanced.Name = "advanced";
            this.advanced.Size = new Size(50, 48);
            this.advanced.ToolTipText = "高级功能";
            this.advanced.Visible = true;
            if (this.agentTool.Items.Contains(this.advanced))
            {
                for (int i = this.advanced.DropDownItems.Count - 1; i >= 0; i--)
                {
                    ToolStripButton tsbitem = this.advanced.DropDownItems[i] as ToolStripButton;
                    if (tsbitem != null)
                    {
                        this.tsbitem_remove(tsbitem);
                    }
                    else
                    {
                        ToolStripSplitButton tssbitem = this.advanced.DropDownItems[i] as ToolStripSplitButton;
                        if (tssbitem != null)
                        {
                            this.tssbitem_remove(tssbitem);
                        }
                        else
                        {
                            ToolStripDropDownButton tsddbitem = this.advanced.DropDownItems[i] as ToolStripDropDownButton;
                            if (tsddbitem != null)
                            {
                                this.tsddbitem_remove(tsddbitem);
                            }
                        }
                    }
                }
                if (0 == this.advanced.DropDownItems.Count)
                {
                    this.agentTool.Items.Remove(this.advanced);
                    this.controls_count_previous["advanced"] = true;
                }
            }
            if (this.controls_info.chkCallAgentInfo || this.controls_info.chkCallOutInfo || this.controls_info.chkListenInfo || this.controls_info.chkWhisperInfo || this.controls_info.chkBargeinInfo || this.controls_info.chkForceHangupInfo || this.controls_info.chkCheckInfo)
            {
                this.addControls(this.advanced, "advanced");
                this.addButtonToDropDownButton(this.advanced, this.controls_info.chkCallAgentInfo, this.tsbCallAgent);
                this.addButtonToDropDownButton(this.advanced, this.controls_info.chkCallOutInfo, this.tsbCallOut);
                this.addButtonToDropDownButton(this.advanced, this.controls_info.chkListenInfo, this.tsbListen);
                this.addButtonToDropDownButton(this.advanced, this.controls_info.chkWhisperInfo, this.tsbWhisper);
                this.addButtonToDropDownButton(this.advanced, this.controls_info.chkBargeinInfo, this.tsbBargein);
                this.addButtonToDropDownButton(this.advanced, this.controls_info.chkForceHangupInfo, this.tsbForceHangup);
                this.addButtonToDropDownButton(this.advanced, this.controls_info.chkCheckInfo, this.tsbCheck);
            }
        }

        public void addOtherFunction()
        {
            this.other.Image = Resources.other;
            this.other.Name = "other";
            this.other.Size = new Size(50, 48);
            this.other.ToolTipText = "其他功能";
            this.other.Visible = true;
            if (this.agentTool.Items.Contains(this.other))
            {
                for (int i = this.other.DropDownItems.Count - 1; i >= 0; i--)
                {
                    ToolStripButton tsbitem = this.other.DropDownItems[i] as ToolStripButton;
                    if (tsbitem != null)
                    {
                        this.tsbitem_remove(tsbitem);
                    }
                    else
                    {
                        ToolStripSplitButton tssbitem = this.other.DropDownItems[i] as ToolStripSplitButton;
                        if (tssbitem != null)
                        {
                            this.tssbitem_remove(tssbitem);
                        }
                        else
                        {
                            ToolStripDropDownButton tsddbitem = this.other.DropDownItems[i] as ToolStripDropDownButton;
                            if (tsddbitem != null)
                            {
                                this.tsddbitem_remove(tsddbitem);
                            }
                        }
                    }
                }
                if (0 == this.other.DropDownItems.Count)
                {
                    this.agentTool.Items.Remove(this.other);
                    this.controls_count_previous["other"] = true;
                }
            }
            if (this.controls_info.chkMonitorInfo || this.controls_info.chkCancelApplicationInfo || this.controls_info.chkdbAfterHangupInfo || this.controls_info.chkApproveInfo || this.controls_info.chkNoAnswerCallsInfo)
            {
                this.addControls(this.other, "other");
                this.addTssButtonToDropDownButton(this.other, this.controls_info.chkMonitorInfo, this.tsbMonitor);
                this.addButtonToDropDownButton(this.other, this.controls_info.chkCancelApplicationInfo, this.tsbCancelApplication);
                this.addTsddButtonToDropDownButton(this.other, this.controls_info.chkdbAfterHangupInfo, this.tsddbAfterHangup);
                this.addButtonToDropDownButton(this.other, this.controls_info.chkApproveInfo, this.tsbApprove);
                this.addButtonToDropDownButton(this.other, this.controls_info.chkNoAnswerCallsInfo, this.tsbNoAnswerCalls);
            }
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

        public void ChangeControls()
        {
            this.addBasicFunction();
            this.addAdvancedFunction();
            this.addOtherFunction();
            this.reInsertControls(this.basic, "basic");
            this.reInsertControls(this.advanced, "advanced");
            this.reInsertControls(this.other, "other");
            this.reCheckPrompt();
            this.aboControlVisible(this.advanced);
            this.aboControlVisible(this.other);
            this.aboControlVisibleIsTrue(this.advanced);
            this.aboControlVisibleIsTrue(this.other);
        }

        public void aboControlVisibleIsTrue(ToolStripDropDownButton control)
        {
            if (!control.Visible)
            {
                if (control == this.advanced)
                {
                    if ((this.agentTool.Items.Contains(this.tsbListen) && this.controlsVisible.ListenVisible) || (this.advanced.DropDownItems.Contains(this.tsbWhisper) && this.controlsVisible.WhisperVisible) || (this.advanced.DropDownItems.Contains(this.tsbBargein) && this.controlsVisible.BargeinVisible) || (this.advanced.DropDownItems.Contains(this.tsbForceHangup) && this.controlsVisible.ForceHangupVisible) || (this.advanced.DropDownItems.Contains(this.tsbForceHangup) && this.controlsVisible.CallOutVisible))
                    {
                        control.Visible = true;
                    }
                }
                if (control == this.other)
                {
                    if ((this.advanced.DropDownItems.Contains(this.tsbForceHangup) && this.controlsVisible.MonitorVisible) || (this.advanced.DropDownItems.Contains(this.tsbForceHangup) && this.controlsVisible.ApproveVisible))
                    {
                        control.Visible = true;
                    }
                }
            }
        }

        public void aboControlVisible(ToolStripDropDownButton control)
        {
            if (this.agentTool.Items.Contains(control))
            {
                for (int i = control.DropDownItems.Count - 1; i >= 0; i--)
                {
                    ToolStripButton tsbitem = control.DropDownItems[i] as ToolStripButton;
                    if (tsbitem != null)
                    {
                        if ((bool)tsbitem.Tag)
                        {
                            control.Visible = true;
                            break;
                        }
                    }
                    else
                    {
                        ToolStripSplitButton tssbitem = control.DropDownItems[i] as ToolStripSplitButton;
                        if (tssbitem != null)
                        {
                            if ((bool)tssbitem.Tag)
                            {
                                control.Visible = true;
                                break;
                            }
                        }
                        else
                        {
                            ToolStripDropDownButton tsddbitem = control.DropDownItems[i] as ToolStripDropDownButton;
                            if (tsddbitem != null)
                            {
                                if ((bool)tsddbitem.Tag)
                                {
                                    control.Visible = true;
                                    break;
                                }
                            }
                        }
                    }
                    control.Visible = false;
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

        public void changetsddbAfterHangupSize()
        {
            if (this.other.DropDownItems.Contains(this.tsddbAfterHangup))
            {
                this.tsddbAfterHangup.Size = new Size(165, 36);
            }
        }

        private void trmCall_Tick(object sender, EventArgs e)
        {
            this.tsbState.Text = this.tsbStateBack + "时长：" + ComFunc.converToTimeLength(this.timeCount.ToString());
            this.timeCount++;
        }
    }
}
