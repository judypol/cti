using System;
using System.Runtime.InteropServices;

namespace PLAgentDll
{
	[ComVisible(true), Guid("4B691098-55D6-43f6-991E-3CB68D0C086B"), InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
	public interface IAgentEvents
	{
		void AgentEvents(AgentEvent agent_event);
	}
}
