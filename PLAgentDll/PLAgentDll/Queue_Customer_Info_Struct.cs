using System;
using System.Collections.Generic;

namespace PLAgentDll
{
	public struct Queue_Customer_Info_Struct
	{
		public string queue_num;

		public string queue_name;

		public List<Customer_Info_Struct> customer_info_list;
	}
}
