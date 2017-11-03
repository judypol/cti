using System;
using System.Net.Sockets;
using System.Text;

namespace PLAgentDll
{
	public class StateObject
	{
		public const int BufferSize = 1024;

		public Socket workSocket = null;

		public byte[] buffer = new byte[1024];

		public StringBuilder sb = new StringBuilder();
	}
}
