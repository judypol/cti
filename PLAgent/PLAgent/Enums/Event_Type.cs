﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLAgent
{
    public enum Event_Type
    {
        INITE_TOOLBAR,
        RESPONSE_TIMEOUT,
        KICK_OUT,
        EXTERNAL_CALL_OUT_SUCCESS,
        EXTERNAL_CALL_OUT_FAIL,
        INTERNAL_CALL_AGENT_SUCCESS,
        INTERNAL_CALL_AGENT_FAIL,
        ANSWER_CALL_PEER,
        ANSWER_CALL_CALLEE,
        BRIDGE_CALL_PEER,
        HANGUP_CALL_SUCCESS,
        HANGUP_CALL_FAIL,
        CALLIN_COMMON,
        CALLIN_PREDICT_CALL,
        CALLIN_INTERNAL,
        CALLIN_INTERNAL_MYSELF,
        CALLIN_THREE_WAY,
        CALLOUT_RING_MYSELF,
        MANUAL_CALLOUT_SUCCESS,
        MANUAL_CALLOUT_FAIL,
        DISCONNECT_SOCKET,
        SIGNIN_SUCCESS,
        SIGNIN_FAIL,
        SIGNOUT_SUCCESS,
        SIGNOUT_FAIL,
        GRADE_SUCCESS,
        GRADE_FAIL,
        HOLD_SUCCESS,
        HOLD_FAIL,
        UNHOLD_SUCCESS,
        UNHOLD_FAIL,
        MUTE_SUCCESS,
        MUTE_FAIL,
        UNMUTE_SUCCESS,
        UNMUTE_FAIL,
        TRANSFER_AGENT_SUCCESS,
        TRANSFER_AGENT_FAIL,
        TRANSFER_IVR_SUCCESS,
        TRANSFER_IVR_FAIL,
        TRANSFER_QUEUE_SUCCESS,
        TRANSFER_QUEUE_FAIL,
        TRANSFER_IVR_PROFILE_SUCCESS,
        TRANSFER_IVR_PROFILE_FAIL,
        TRANSFER_BLIND_CALL_IN,
        ECHO_TEST_SUCCESS,
        ECHO_TEST_FAIL,
        CONSULT_SUCCESS,
        CONSULT_FAIL,
        CONSULT_CANCEL_SUCCESS,
        CONSULT_CANCEL_FAIL,
        CONSULT_TRANSFER_SUCCESS,
        CONSULT_TRANSFER_FAIL,
        CONSULT_CALL_IN,
        CONSULTEE_HANGUP,
        THREE_WAY_SUCCESS,
        THREE_WAY_FAIL,
        THREE_WAY_CANCEL_SUCCESS,
        THREE_WAY_CANCEL_FAIL,
        THREEWAYEE_HANGUP,
        EAVESDROP_SUCCESS,
        EAVESDROP_FAIL,
        EAVESDROP_CANCEL_SUCCESS,
        EAVESDROP_CANCEL_FAIL,
        EAVESDROP_RING_MYSELF,
        WHISPER_RING_MYSELF,
        BARGEIN_RING_MYSELF,
        FORCE_HANGUP_RING_MYSELF,
        WHISPER_SUCCESS,
        WHISPER_FAIL,
        BARGE_IN_SUCCESS,
        BARGE_IN_FAIL,
        FORCE_HANGUP_SUCCESS,
        FORCE_HANGUP_FAIL,
        BLIND_TRANSFER_OUTBOUND_FAILED,
        GET_ACCESS_NUMBERS_SUCCESS,
        GET_ACCESS_NUMBERS_FAIL,
        GET_ONLINE_AGENT_FAIL,
        GET_ONLINE_AGENT_SUCCESS,
        GET_IVR_LIST_SUCCESS,
        GET_IVR_LIST_FAIL,
        GET_QUEUE_LIST_SUCCESS,
        GET_QUEUE_LIST_FAIL,
        GET_AGENT_GROUP_LIST_SUCCESS,
        GET_AGENT_GROUP_LIST_FAIL,
        AGENT_STATUS_CHANGE_TO_IDLE,
        AGENT_STATUS_CHANGE_TO_RING,
        AGENT_STATUS_CHANGE_TO_TALKING,
        AGENT_STATUS_CHANGE_TO_HOLD,
        AGENT_STATUS_CHANGE_TO_MUTE,
        AGENT_STATUS_CHANGE_TO_ACW,
        AGENT_STATUS_CHANGE_TO_CAMP_ON,
        AGENT_STATUS_CHANGE_TO_BUSY,
        AGENT_STATUS_CHANGE_TO_LEAVE,
        AGENT_STATUS_CHANGE_TO_MANUAL_CALL_OUT,
        AGENT_STATUS_CHANGE_TO_CALLING_OUT,
        SOFTPHONE_REGIST_RESULT,
        SOFTPHONE_CALL_IN,
        SOFTPHONE_HANGUP,
        SOFTPHONE_ANSWER,
        AGENT_STATUS_RESTORE,
        APPLY_CHANGE_STATUS_APPLY_SUCCESS,
        APPLY_CHANGE_STATUS_APPLY_FAILED,
        APPLY_CHANGE_STATUS_CANCEL_SUCCESS,
        APPLY_CHANGE_STATUS_CANCEL_FAILED,
        APPLY_CHANGE_STATUS_APPROVAL_PASS,
        APPLY_CHANGE_STATUS_NO_ANY_APPROVAL,
        APPLY_CHANGE_STATUS_SOME_APPLY,
        APPLY_CHANGE_STATUS_APPLY_OR_APPROVE_TIMEOUT,
        APPLY_CHANGE_STATUS_FINISHED
    }
}