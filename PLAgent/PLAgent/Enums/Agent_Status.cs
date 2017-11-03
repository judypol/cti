using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLAgent
{
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
}
