using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLAgent
{
    internal class Utils
    {
        public static Agent_Status Str2AgentStatus(string strAgentStatus)
        {
            Agent_Status result;
            try
            {
                if ("" != strAgentStatus)
                {
                    result = (Agent_Status)Enum.Parse(typeof(Agent_Status), strAgentStatus);
                }
                else
                {
                    result = Agent_Status.AGENT_STATUS_UNKNOWN;
                }
            }
            catch (Exception ex_33)
            {
                result = Agent_Status.AGENT_STATUS_UNKNOWN;
            }
            return result;
        }

        public static string AgentStatus2Str(Agent_Status agentStatus)
        {
            string agentName;
            switch (agentStatus)
            {
                case  Agent_Status.AGENT_STATUS_UNKNOWN:
                    agentName = "未知";
                    break;
                case  Agent_Status.AGENT_STATUS_OFFLINE:
                    agentName = "离线";
                    break;
                case  Agent_Status.AGENT_STATUS_IDLE:
                    agentName = "空闲";
                    break;
                case  Agent_Status.AGENT_STATUS_RING:
                    agentName = "响铃";
                    break;
                case  Agent_Status.AGENT_STATUS_TALKING:
                    agentName = "通话";
                    break;
                case  Agent_Status.AGENT_STATUS_HOLD:
                    agentName = "保持";
                    break;
                case  Agent_Status.AGENT_STATUS_ACW:
                    agentName = "后处理";
                    break;
                case  Agent_Status.AGENT_STATUS_CAMP_ON:
                    agentName = "预占";
                    break;
                case  Agent_Status.AGENT_STATUS_BUSY:
                    agentName = "忙碌";
                    break;
                case  Agent_Status.AGENT_STATUS_LEAVE:
                    agentName = "离开";
                    break;
                case  Agent_Status.AGENT_STATUS_CALL_OUT:
                    agentName = "手动外呼中";
                    break;
                case  Agent_Status.AGENT_STATUS_MONITOR:
                    agentName = "监控";
                    break;
                case  Agent_Status.AGENT_STATUS_CALLING_OUT:
                    agentName = "呼出中";
                    break;
                case  Agent_Status.AGENT_STATUS_MUTE:
                    agentName = "静音";
                    break;
                default:
                    agentName = "";
                    break;
            }
            return agentName;
        }

        public static int str2int(string agent_status)
        {
            int result;
            if (agent_status == "离线")
            {
                result = -1;
            }
            else
            {
                result = 0;
            }
            return result;
        }

        public static string CustomerStatus2Chinese(string customerStatus)
        {
            string result;
            if (customerStatus == "Waiting")
            {
                result = "等待";
            }
            else if (customerStatus == "Trying")
            {
                result = "振铃";
            }
            else
            {
                result = "未知";
            }
            return result;
        }

        public static string IcoName_CustomerStatus2Str(string customerStatus)
        {
            string result;
            if (customerStatus == "Waiting")
            {
                result = "wait";
            }
            else if (customerStatus == "Trying")
            {
                result = "try";
            }
            else
            {
                result = "unknow";
            }
            return result;
        }
        public string HangupReason2Chinese(string hangupReason)
        {
            string result;
            if (hangupReason == null)
            {
                result = "";
            }
            else
            {
                //Log.Debug("enter HangupReason2Chinese .hangupReason:" + hangupReason);
                string text = hangupReason.ToUpper();
                string strReason;
                if (text != null)
                {
                    if (text == "NO_ANSWER")
                    {
                        strReason = "未接";
                        goto IL_B0;
                    }
                    if (text == "ORIGINATOR_CANCEL")
                    {
                        strReason = "对方挂断";
                        goto IL_B0;
                    }
                    if (text == "NORMAL_CLEARING")
                    {
                        strReason = "已挂断";
                        goto IL_B0;
                    }
                    if (text == "CALLOUT_FAILED")
                    {
                        strReason = "呼叫失败";
                        goto IL_B0;
                    }
                    if (text == "ALL_GATEWAYS_FULL")
                    {
                        strReason = "外线满线";
                        goto IL_B0;
                    }
                }
                strReason = "拒接";
                IL_B0:
                result = strReason;
            }
            return result;
        }
        public static string str2ApplyType(string apply_type)
        {
            string result;
            if (apply_type != null)
            {
                if (apply_type == "1")
                {
                    result = "坐席";
                    return result;
                }
                if (apply_type == "2")
                {
                    result = "系统";
                    return result;
                }
            }
            result = "未知";
            return result;
        }
    }
}
