using System;
using System.ComponentModel;

namespace PLAgentDll
{
	public enum CommandResult
	{
		[Description("连接服务器失败！")]
		RPCERR_INITFAILED = 100,
		[Description("断开服务器失败！")]
		RPCERR_DISCONNECT,
		[Description("服务器连接已存在,请不要重复连接！")]
		RPCERR_CONNECTED_ALREADY,
		[Description("系统初始化失败！")]
		SYSERR_INITFAIL = 200,
		[Description("信息发送失败，请检查网络！")]
		SYSERR_SENDFAIL,
		[Description("发送的数据不能为空！")]
		SYSERR_SENDNULL,
		[Description("请先连接服务器！")]
		SYSERR_NOCONNECT,
		[Description("启动日志失败！")]
		SYSERR_LOGERR,
		[Description("发送的数据长度不能为0！")]
		SYSERR_ZEROLENGTH,
		[Description("发送数据超时！")]
		SYSERR_SENDTIMEOUT,
		[Description("部分数据没有发送成功！")]
		SYSERR_SENDDATALOST,
		[Description("请先签入！")]
		CALL_NOSIGIN = 301,
		[Description("传入参数必填项不能为空！")]
		PARAMETER_NULL = 400,
		[Description("未知错误！")]
		SYSERR_UNKNOWN = 1000
	}
}
