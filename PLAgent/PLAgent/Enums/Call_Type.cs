using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLAgent
{
    public enum Call_Type
    {
        COMMON_CALL_IN,
        MANUAL_CALL_OUT,
        PREDICT_CALL_OUT,
        AGENT_INTERNAL_CALL,
        ECHO_TEST,
        CONSULT_CALL_IN,
        EAVESDROP_CALL_IN,
        WHISPER_CALL_IN,
        BARGEIN_CALL_IN,
        THREEWAY_CALL_IN,
        FORCE_HANGUP_CALL_IN
    }
}
