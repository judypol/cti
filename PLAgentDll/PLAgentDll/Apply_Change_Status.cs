using System;

namespace PLAgentDll
{
	[Serializable]
	public struct Apply_Change_Status
	{
		public string applyAgentID;

		public string agentName;

		public string targetStatus;

		public string applyTime;

		public string applyAgentGroupID;

		public string applyAgentGroupName;

		public Apply_State applyState;

		public string applyStateStr;

		public string approveTime;

		public string executeTime;

		public string reason;

		public bool isFinished;

		public string applyType;
	}
}
