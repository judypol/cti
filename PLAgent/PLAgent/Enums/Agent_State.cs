﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLAgent
{
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
}
