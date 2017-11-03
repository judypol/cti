using System;

namespace PLAgentDll
{
	public enum Apply_State
	{
		Unknow = -1,
		Apply_State_Applying,
		Apply_State_Cancel,
		Apply_State_Approval_Pass,
		Apply_State_Approval_NoPass,
		Apply_State_Execute_Success,
		Apply_State_Exeute,
		Apply_State_Execute_Failed
	}
}
