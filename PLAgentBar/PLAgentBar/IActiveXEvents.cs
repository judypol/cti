using System;
using System.Runtime.InteropServices;

namespace PLAgentBar
{
	[ComVisible(true), Guid("C37124B1-8013-44C1-B8B4-9672B2AA175F"), InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
	public interface IActiveXEvents
	{
		[DispId(1610743810)]
		void CallInEvent(string callerID, string make_str, string callType, bool bNeedArea, string outExtraParamsFromIvr);

		[DispId(1610743811)]
		void InternalCallerRingEvent(string agentID, string calledID, string callType, string makeStr);

		[DispId(1610743812)]
		void PredictCallOutBridgeRingEvent(string callerID, string make_str, string callType, bool bNeedArea, string outExtraParamsFromIvr);

		[DispId(1610743813)]
		void ConsultCallInEvent(string agentID, string customerNum, string callcenterNum, string accessNumName, string consulterAgentNum, string callType, string areaID, string areaName, string custGrade, string outExtraParamsFromIvr, string makeStr);

		[DispId(1610743814)]
		void TransferBlindCallInEvent(string agentID, string customerNum, string callcenterNum, string accessNumName, string consulterAgentNum, string callType, string areaID, string areaName, string custGrade, string outExtraParamsFromIvr, string makeStr);

		[DispId(1610743815)]
		void ThreeWayCallRingEvent(string agentID, string customerNum, string callcenterNum, string accessNumName, string callerAgentNum, string callType, string areaID, string areaName, string custGrade, string outExtraParamsFromIvr, string makeStr);

		[DispId(1610743816)]
		void EavesdropCallRingEvent(string agentID, string customerNum, string callcenterNum, string accessNumName, string desAgentID, string callType, string areaID, string areaName, string custGrade, string outExtraParamsFromIvr, string makeStr);

		[DispId(1610743817)]
		void WhisperCallRingEvent(string agentID, string customerNum, string callcenterNum, string accessNumName, string desAgentID, string callType, string areaID, string areaName, string custGrade, string outExtraParamsFromIvr, string makeStr);

		[DispId(1610743818)]
		void BargeinCallRingEvent(string agentID, string customerNum, string callcenterNum, string accessNumName, string desAgentID, string callType, string areaID, string areaName, string custGrade, string outExtraParamsFromIvr, string makeStr);

		[DispId(1610743819)]
		void ForceHangupCallRingEvent(string agentID, string customerNum, string callcenterNum, string accessNumName, string desAgentID, string callType, string areaID, string areaName, string custGrade, string outExtraParamsFromIvr, string makeStr);

		[DispId(1610743820)]
		void EavesdropEvent(string agentID, string reason, int retCode);

		[DispId(1610743821)]
		void WhisperEvent(string agentID, string reason, int retCode);

		[DispId(1610743822)]
		void BargeinEvent(string agentID, string reason, int retCode);

		[DispId(1610743823)]
		void ForceHangupEvent(string agentID, string reason, int retCode);

		[DispId(1610743824)]
		void InternalCall_CallInEvent(string agentID, string callerID, string callType, string makeStr);

		[DispId(1610743826)]
		void UserStateChangeEvent(string userState, string strReason);

		[DispId(1610743827)]
		void AgentStatusChangeEvent(string agent_num, string old_status, string new_status, bool is_bind_exten, string customer_enter_channel, string current_time, string start_talking_time);

		[DispId(1610743828)]
		void ResponseEvent(string EventType, string agentID, int retCode, string strReason);

		[DispId(1610743829)]
		void JSGetAgentOnlineEvent(string jsonArgsStr);

		[DispId(1610743830)]
		void JSGetAccessNumberEvent(string jsonArgsStr);

		[DispId(1610743831)]
		void JSGetIvrListEvent(string jsonArgsStr);

		[DispId(1610743834)]
		void JSGetQueueListEvent(string jsonArgsStr);

		[DispId(1610743835)]
		void JSGetIvrProfileListEvent(string jsonArgsStr);

		[DispId(1610743836)]
		void JSGetAgentGroupListEvent(string jsonArgsStr);

		[DispId(1610743837)]
		void JSGetAllAgentGroupListEvent(string jsonArgsStr);

		[DispId(1610743838)]
		void GetAgentsOfQueueEvent(string agent_list, string queue_num);

		[DispId(1610743839)]
		void GetAgentsOfAgentGroupEvent(string agent_list, string agent_group_num);

		[DispId(1610743840)]
		void JSGetAgentsMonitorInfoEvent(string jsonArgsStr);

		[DispId(1610743841)]
		void JSGetDetailCallInfoEvent(string jsonArgsStr);

		[DispId(1610743842)]
		void JSGetCustomerOfQueueEvent(string jsonArgsStr);

		[DispId(1610743843)]
		void JSGetCustomerOfMyQueueEvent(string jsonArgsStr);

		[DispId(1610743844)]
		void JSGetQueueStatisLstEvent(string jsonArgsStr);

		[DispId(1610743845)]
		void JSGetAllQueueStatisEvent(string jsonArgsStr);

		[DispId(1610743846)]
		void AddCustomerToQueueEvent(string call_type, string callcenter_num, string customer_num, string customer_status, string customer_type, string customer_uuid, string enter_queue_time, string queue_num, string current_time, string exclusive_agent_num, string exclusive_queue_num, string queue_name, string queue_customer_amount, string early_queue_enter_time, string early_queue_enter_time_all, string customer_enter_channel);

		[DispId(1610743847)]
		void UpdateCustomerOfQueueEvent(string call_type, string callcenter_num, string customer_num, string customer_status, string customer_type, string customer_uuid, string enter_queue_time, string queue_num);

		[DispId(1610743848)]
		void DelCustomerFromQueueEvent(string customer_uuid, string queue_num, string current_time, string early_queue_enter_time, string early_queue_enter_time_all, string reason);

		[DispId(1610743849)]
		void ThreeWayeeHangupEvent(string agentID, string hangupReason);

		[DispId(1610743850)]
		void WarnAgentResigninEvent();

		[DispId(1610743851)]
		void WarnAgentForceChangeStatusEvent(string executorAgentID);

		[DispId(1610743852)]
		void ConsulteeHangupEvent(string agentID, string hangupReason);

		[DispId(1610743853)]
		void EventResultEvent(string EventType, string agentID, int retCode, string strReason, string strHangupReason);

		[DispId(1610743854)]
		void CallOutRingEvent(string customerNum, string callcenterNum, string accessNumName, string callType, string areaID, string areaName, string makeStr);

		[DispId(1610743856)]
		void SockDisconnectEvent(string reason, int retCode);

		[DispId(1610743857)]
		void ResponseTimeOutEvent(string reason, int retCode);

		[DispId(1610743858)]
		void SysThrowExceptionEvent(string reason, int retCode);

		[DispId(1610743859)]
		void CheckExtenEvent(string AgentID, string makeStr);

		[DispId(1610743860)]
		void SignInEvent(string AgentID, int retCode, string strReason);

		[DispId(1610743861)]
		void HangupEvent(string strReason, int retCode, string makeStr);

		[DispId(1610743862)]
		void SignOutEvent(string AgentID, int retCode, string strReason);

		[DispId(1610743863)]
		void JSGetWebSiteInfoEvent(string jsonArgsStr);

		[DispId(1610743864)]
		void KickOutEvent(string AgentID, int retCode, string strReason);

		[DispId(1610743865)]
		void ServerResponse(string AgentID, int retCode, string strReason);

		[DispId(1610743872)]
		void AnswerEvent(string makeStr);

		[DispId(1610743873)]
		void CalleeAnswerEvent(string relaiton_uuid);

		[DispId(1610743874)]
		void BridgeEvent();

		[DispId(1610743875)]
		void GetRoleNameEvent(string roleName);

		[DispId(1610743876)]
		void ForceChangeStatusEvent(string agentID, string reason, int retCode);

		[DispId(1610743877)]
		void GetAgentPersonalInfoEvent(string agentID, string agent_mobile, string agnet_email);

		[DispId(1610743878)]
		void SetAgentPersonalInfoEvent(string agentID, int retCode, string reason);

		[DispId(1610743879)]
		void ChangePswdEvent(string agentID, int retCode, string reason);

		[DispId(1610743880)]
		void JSGetReportStatisInfoEvent(string jsonArgsStr);

		[DispId(1610743881)]
		void ApplyChangeStatusEvent(string agentID, int retCode, string reason);

		[DispId(1610743882)]
		void ApplyChangeStatusCancelEvent(string agentID, string apply_agentid, string targetStatus, int retCode, string reason);

		[DispId(1610743883)]
		void ApplyChangeStatusDistributeEvent(string AgentID, string apply_agentid, string targetStatus, string apply_agentName, string apply_agent_groupID, string apply_agent_groupName, string apply_time, string applyType, int retCode, string reason);

		[DispId(1610743884)]
		void ApproveChangeStatusDistributeEvent(string AgentID, string apply_agentid, string apply_agent_groupID, string targetStatus, string approveResult, string approve_time, int retCode, string reason);

		[DispId(1610743885)]
		void ApproveChangeStatusTimeoutDistributeEvent(string AgentID, string apply_agentid, string apply_agent_groupID, string targetStatus, string timeoutType);

		[DispId(1610743886)]
		void JSGetAgentGroupStatusMaxNumEvent(string jsonArgsStr);

		[DispId(1610743887)]
		void JSGetChangeStatusApplyListEvent(string jsonArgsStr);

		[DispId(1610743888)]
		void AllGetwaysFullEvent();

		[DispId(1610743889)]
		void NoAnswerCallAlarmlEvent();

		[DispId(1610743890)]
		void QueueTransferBoundEvent(string agentID, string customer_num, string queue_num, string transfee_num, string access_num, string tranfer_time);

		[DispId(1610743891)]
		void RecordStart(string AgentExten, string AgentID, string Agent_call_uuid, string CalleeNum, string CallerNum, string FilePath, string makeStr);

		[DispId(1610743892)]
		void RecordStop(string AgentID, string Agent_call_uuid, string FilePath, string makeStr);

		[DispId(1610743893)]
		void Cust_Evaluate_Result(string AgentID, string Agent_call_uuid, string agentExten, string customerUuid, string evaluateScore, string evaluateStatus, string makeStr);
	}
}
