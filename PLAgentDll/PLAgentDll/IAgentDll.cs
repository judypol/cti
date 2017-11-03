using System;
using System.Runtime.InteropServices;

namespace PLAgentDll
{
	[ComVisible(true), Guid("6E70014F-B160-4a41-8B40-2D30EAEB9280")]
	public interface IAgentDll
	{
		[DispId(5)]
		bool IsConnected
		{
			get;
		}

		[DispId(1)]
		int testadd(int a, int b);

		[DispId(2)]
		void Dispose();

		[DispId(3)]
		long PL_ConnectToCti(string serverIP, int port);

		[DispId(4)]
		long PL_DisConnectToCti();

		[DispId(8)]
		long PL_Login(string AgentID, string AgentPwd);

		[DispId(9)]
		long PL_Logout(string AgentID, string AgentPwd);

		[DispId(10)]
		long PL_SignIn(string AgentID, string AgentPwd, string AgentExten, string BindExten, string InitStatus, string WebUrl, string ExtenIsOutbound);

		[DispId(11)]
		long PL_SignOut(string AgentID);

		[DispId(12)]
		long PL_CallOut(string AgentID, string CalledID, string DisplayNum, int callout_type, string taskid, string is_send_msg, string send_msg_url, string customerForeignId);

		[DispId(13)]
		long PL_HangUp(string AgentID, string AgentCallUuid);

		[DispId(14)]
		void PLCloseAll();

		[DispId(15)]
		long PL_HeartBeat(string AgentID);

		[DispId(16)]
		long PL_Check(string AgentID);

		[DispId(17)]
		long PL_Hold(string AgentID, string AgentCallUuid);

		[DispId(18)]
		long PL_StopHold(string AgentID, string AgentCallUuid);

		[DispId(19)]
		long PL_Mute(string AgentID, string AgentCallUuid);

		[DispId(20)]
		long PL_StopMute(string AgentID, string AgentCallUuid);

		[DispId(21)]
		long PL_Consult(string AgentID, string DestAgentID, string AgentCallUuid, bool trdIsOutbound);

		[DispId(22)]
		long PL_ConsultCancel(string AgentID, string AgentCallUuid);

		[DispId(23)]
		long PL_Listen(string AgentID, string targetAgentNum);

		[DispId(25)]
		long PL_TransferAgent(string AgentID, string destAgentID, string CustomerCallUuid, string strOutBoundFlag);

		[DispId(26)]
		long PL_Intercept(string AgentID, string AgentExten, string CuID);

		[DispId(27)]
		long PL_Interrupt(string AgentID, string AgentExten, string CuID);

		[DispId(31)]
		long PL_GetAgentDefineStatus(string AgentID);

		[DispId(32)]
		long PL_SetAgentDefineStatus(string AgentID, int TargetStatus, int IsNeedApproval);

		[DispId(33)]
		long PL_CallAgent(string AgentID, string destAgentID);

		[DispId(34)]
		long PL_Grade(string AgentID, string Language, string CustomerCallUuid);

		[DispId(35)]
		long PL_GetAccessNumbers(string AgentID);

		[DispId(36)]
		long PL_ThreeWay(string AgentID, string destAgentID, string AgentCallUuid, bool trdIsOutbound);

		[DispId(37)]
		long PL_ThreeWayCancel(string AgentID, string AgentCallUuid);

		[DispId(38)]
		long PL_ConsultTransfer(string AgentID, string AgentCallUuid);

		[DispId(39)]
		long PL_Whisper(string AgentID, string targetAgentNum);

		[DispId(40)]
		long PL_Bargein(string AgentID, string targetAgentNum);

		[DispId(41)]
		long PL_ForceHangup(string AgentID, string targetAgentNum);

		[DispId(42)]
		long PL_TransferIvr(string AgentID, string ivrNum, string CustomerCallUuid);

		[DispId(43)]
		long PL_TransferQueue(string AgentID, string queueNum, string CustomerCallUuid);

		[DispId(44)]
		long PL_GetAgentOnline(string AgentID, string specificNum, int numType);

		[DispId(45)]
		long PL_GetIvrList(string AgentID);

		[DispId(46)]
		long PL_GetQueueList(string AgentID);

		[DispId(47)]
		long PL_GetDefinedRoleAndRight(string AgentID);

		[DispId(48)]
		long PL_GetAgentGroupList(string AgentID, string agentGroupRange);

		[DispId(49)]
		long PL_GetAgentsOfQueue(string AgentID, string QueueNum);

		[DispId(50)]
		long PL_GetAgentsOfAgentGroup(string AgentID, string AgentGroupNum);

		[DispId(51)]
		long PL_GetAgentsMonitorInfo(string AgentID, string AgentsStr);

		[DispId(52)]
		long PL_GetDetailCallInfo(string AgentID, string targetAgentNum);

		[DispId(53)]
		long PL_GetCustomerOfQueue(string AgentID, string queueNumLstStr);

		[DispId(54)]
		long PL_Reload(string agentID, string reloadType);

		[DispId(55)]
		long PL_UpdateAgent(string agentID, string agentNum);

		[DispId(56)]
		long PL_DelAgent(string agentID, string agentNum);

		[DispId(57)]
		long PL_GetAgentWebSiteInfo(string AgentID);

		[DispId(58)]
		long PL_GetIvrProfileList(string AgentID);

		[DispId(59)]
		long PL_TransferIvrProfile(string AgentID, string ivrProfileNum, string CustomerCallUuid);

		[DispId(60)]
		long PL_OnForceChangeStatus(string AgentID, string targetAgentNum, string status);

		[DispId(61)]
		long PL_GetQueueStatis(string AgentID, string queueNumLstStr);

		[DispId(62)]
		long PL_GetCustomerOfMyQueue(string AgentID);

		[DispId(63)]
		long PL_GetPersonalInfo(string AgentID);

		[DispId(64)]
		long PL_SetPersonalInfo(string AgentID, string strMobile, string strEmail);

		[DispId(65)]
		long PL_ChangePswd(string AgentID, string strOldPswd, string strNewPswd);

		[DispId(66)]
		long PL_GetReportStatisInfo(string AgentID);

		[DispId(67)]
		long PL_ApplyChangeStatus(string AgentID, string targetStatus);

		[DispId(68)]
		long PL_ApplyApproval(string AgentID, string applyAgentID, string targetStatus, string passFlag);

		[DispId(69)]
		long PL_getAgentGroupStatusMaxNum(string AgentID, string agentGroupNameLstStr, string agentGroupIdLstStr, string targetStatus);

		[DispId(70)]
		long PL_setAgentGroupStatusMaxNum(string AgentID, string agentGroupNameLstStr, string agentGroupIdLstStr, string targetStatus, string statusMaxNum);

		[DispId(71)]
		long PL_ApplyCancel(string AgentID, string targetStatus);

		[DispId(72)]
		long PL_GetChangeStatusApplyList(string AgentID, string targetStatus);
	}
}
