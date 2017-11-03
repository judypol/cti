using System;

namespace PLAgentDll
{
	public struct Agent_Role_And_Right_Struct
	{
		public string role_id;

		public string role_name;

		public bool rights_of_whisper;

		public bool rights_of_view_recording;

		public bool rights_of_view_queue_message;

		public bool rights_of_view_queue_info;

		public bool rights_of_view_cdr;

		public bool rights_of_view_agent_group_info;

		public bool rights_of_forcehangup;

		public bool rights_of_eavesdrop;

		public bool rights_of_callout;

		public bool rights_of_bargein;

		public string controled_queue_lst;

		public string controled_agent_group_lst;

		public string controled_agent_group_name_lst;

		public bool rights_of_force_change_status_idle;

		public bool rights_of_force_change_status_busy;

		public bool rights_of_force_change_status_leave;

		public bool role_right1;

		public bool role_right2;

		public bool role_right3;

		public bool role_right4;

		public bool role_right5;

		public bool group_role_right1;

		public bool group_role_right2;

		public bool group_role_right3;

		public bool group_role_right4;

		public bool group_role_right5;

		public bool queue_role_right1;

		public bool queue_role_right2;

		public bool queue_role_right3;

		public bool queue_role_right4;

		public bool queue_role_right5;
	}
}
