using log4net;
using System;
using System.Reflection;

namespace PLAgentBar
{
	public static class LogHelper
	{
		public static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
	}
}
