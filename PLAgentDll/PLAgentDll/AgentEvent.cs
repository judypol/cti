using System;
using System.Collections.Generic;

namespace PLAgentDll
{
	public struct AgentEvent
	{
		public AgentEventType deAgentEventType;

		public EventQualifier eEventQualifier;

		public string callerID;

		public string calledID;

		public string agentID;

		public string extenNum;

		public string eventMsg;

		public string reason;

		public int retCode;

		public string cuID;

		public string hangupReason;

		public string hangupUuid;

		public string destAgentID;

		public List<string[]> agentInfoList;

		public string agentExten;

		public string HangupActiveFlag;

		public string agentName;

		public bool autoPostTreatment;

		public bool bindExten;

		public bool gradeSwitch;

		public string initStatus;

		public string roleID;

		public string salt_key;

		public string DID_Num;

		public Dictionary<string, string> agentDefineStatus;

		public Dictionary<string, string> agentWebSiteInfo;

		public string status_change_agentid;

		public string status_before;

		public string status_after;

		public string exception_reason;

		public string makeStr;

		public string outExtraParamsFromIvr;

		public string todayDate;

		public string agent_group_name;

		public string agentGroupID;

		public string calledAgentNum;

		public string callerAgentNum;

		public string[] accessNumbers;

		public string access_num;

		public string customer_num_format_local;

		public string customer_num_format_national;

		public string customer_num_format_e164;

		public string customer_num_phone_type;

		public string transfer_time;

		public string consulterAgentNum;

		public string call_type;

		public string specificAgentNum;

		public string num_type;

		public string queue_num;

		public string queue_name;

		public string agent_group_num;

		public string agent_list;

		public string current_time;

		public string queueNumLstStr;

		public string customer_status;

		public string customer_type;

		public string customer_uuid;

		public string enter_queue_time;

		public string queue_customer_amount;

		public string early_queue_enter_time;

		public string early_queue_enter_time_all;

		public int heartbeat_timeout;

		public string relation_uuid;

		public string exclusive_agent_num;

		public string exclusive_queue_num;

		public List<Agent_Online_Struct> agent_online;

		public Dictionary<string, string> ivr_list;

		public Dictionary<string, string> queue_list;

		public Dictionary<string, string> ivr_profile_list;

		public Dictionary<string, string> group_list;

		public Dictionary<string, string> status_max_num_list;

		public Dictionary<string, string> report_statis_info_map;

		public List<Agent_Role_And_Right_Struct> agent_role_and_right;

		public List<Leg_Info_Struct> leg_info_list;

		public List<Relation_Info_Struct> relation_info_list;

		public List<Customer_Info_Struct> customer_info_list;

		public List<Queue_Statis_Struct> queue_statis_list;

		public List<Apply_Change_Status> apply_change_status_list;

		public string executorAgentID;

		public string access_num_name;

		public string area_id;

		public string area_name;

		public string cust_grade;

		public string agent_mobile;

		public string agent_email;

		public string apply_agent_id;

		public string apply_agent_name;

		public string targetStatus;

		public string applyTime;

		public string approveResult;

		public string approveTime;

		public string agentGroupNameLstStr;

		public string timeoutType;

		public string applyType;

		public string customer_enter_channel;

		public string transfee_num;

		public string no_answered_alram_flag;

		public string agent_call_uuid;

		public string bridge_uuidA;

		public string bridge_uuidB;

		public string customer_foreign_id;

		public string filePath;

		public string calleeNum;

		public string callerNum;

		public string isEvaluated;

		public string evaluateStatus;

		public string evaluate_default_result;

		public string customerUuid;

		public string evaluateScore;

		public string predictCustomerName;

		public string predictCustomerRemark;

		public string predictCustomerForeignId;

		public string start_talking_time;
	}
}
