using log4net;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;

namespace NetDll
{
	public sealed class clsSocket
	{
		public const int TIMEOUT = 1;

		private static ILog Log;

		public NetworkStream _networkStream;

		private clsSocket()
		{
		}

		public static void initSocket()
		{
			clsSocket.Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
			clsSocket.Log.Info("Socket class init success！");
		}

		public static Socket ConnectServer(string ip, int port)
		{
			clsSocket.Log.Debug(string.Concat(new object[]
			{
				"enter ConnectServer.ip:",
				ip,
				",port:",
				port
			}));
			Socket socket = null;
			Socket result;
			try
			{
				IPAddress address = IPAddress.Parse(ip);
				IPEndPoint iPEndPoint = new IPEndPoint(address, port);
				socket = new Socket(iPEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
				socket.Connect(iPEndPoint);
				if (!socket.Connected)
				{
					socket = null;
				}
			}
			catch (Exception message)
			{
				clsSocket.Log.Error(message);
				result = null;
				return result;
			}
			result = socket;
			return result;
		}

		public static int DisConnectServer(Socket socket)
		{
			clsSocket.Log.Debug("enter DisConnectServer.the socket.Connected is:" + socket.Connected);
			int result;
			try
			{
				socket.Shutdown(SocketShutdown.Both);
				socket.Close();
				socket = null;
				clsSocket.Log.Debug("DisConnectServer is Success!!");
				result = 0;
			}
			catch (Exception ex)
			{
				socket.Close();
				socket = null;
				clsSocket.Log.Error("DisConnectServer is failed! reason:" + ex.Message);
				result = -1;
			}
			return result;
		}

		public static Socket ConnectServByHostName(string hostName, int port)
		{
			Socket result = null;
			try
			{
				IPHostEntry hostEntry = Dns.GetHostEntry(hostName);
				IPAddress[] addressList = hostEntry.AddressList;
				for (int i = 0; i < addressList.Length; i++)
				{
					IPAddress address = addressList[i];
					IPEndPoint iPEndPoint = new IPEndPoint(address, port);
					Socket socket = new Socket(iPEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
					socket.Connect(iPEndPoint);
					if (socket.Connected)
					{
						result = socket;
						break;
					}
				}
			}
			catch (Exception message)
			{
				clsSocket.Log.Error(message);
			}
			return result;
		}

		public static int SendData(Socket socket, byte[] buffer, int outTime)
		{
			clsSocket.Log.Debug("enter SendData.");
			int result = 0;
			if (socket == null || !socket.Connected)
			{
				clsSocket.Log.Warn("参数socket 为null，或者未连接到远程计算机");
				result = -4;
			}
			if (buffer == null || buffer.Length == 0)
			{
				clsSocket.Log.Warn("参数buffer 为null ,或者长度为 0");
				result = -5;
			}
			try
			{
				int num = buffer.Length;
				int num2 = 0;
				while (socket.Poll(outTime * 1000000, SelectMode.SelectWrite))
				{
					num2 = socket.Send(buffer, num2, num, SocketFlags.None);
					num -= num2;
					if (num == 0)
					{
						result = 0;
					}
					else
					{
						if (num2 > 0)
						{
							continue;
						}
						result = -2;
					}
					return result;
				}
				result = -1;
			}
			catch (SocketException ex)
			{
				clsSocket.Log.Error(ex.Message);
				result = -3;
			}
			return result;
		}

		public static int SendData(Socket socket, string buffer, int outTime)
		{
			return clsSocket.SendData(socket, Encoding.Default.GetBytes(buffer), outTime);
		}

		public static int SendData(Socket socket, string buffer)
		{
			return clsSocket.SendData(socket, Encoding.Default.GetBytes(buffer), 1);
		}

		public static int RecvData(Socket socket, byte[] buffer, int outTime)
		{
			clsSocket.Log.Debug("enter RecvData.");
			if (socket == null || !socket.Connected)
			{
				clsSocket.Log.Warn("参数socket 为null，或者未连接到远程计算机");
			}
			if (buffer == null || buffer.Length == 0)
			{
				clsSocket.Log.Warn("参数buffer 为null ,或者长度为 0");
			}
			buffer.Initialize();
			int num = buffer.Length;
			int num2 = 0;
			int result = 0;
			try
			{
				while (socket.Poll(outTime * 1000000, SelectMode.SelectRead))
				{
					num2 = socket.Receive(buffer, num2, num, SocketFlags.None);
					num -= num2;
					if (num == 0)
					{
						result = 0;
					}
					else
					{
						if (num2 > 0)
						{
							continue;
						}
						result = -2;
					}
					return result;
				}
				result = -1;
			}
			catch (SocketException message)
			{
				clsSocket.Log.Error(message);
				result = -3;
			}
			return result;
		}

		public static int RecvData(Socket socket, string buffer, int bufferLen, int outTime)
		{
			if (bufferLen <= 0)
			{
				clsSocket.Log.Warn("存储待接收数据的缓冲区长度必须大于0");
			}
			return 0;
		}

		public static int SendFile(Socket socket, string fileName, int maxBufferLength, int outTime)
		{
			if (fileName == null || maxBufferLength <= 0)
			{
				throw new ArgumentException("待发送的文件名称为空或发送缓冲区的大小设置不正确.");
			}
			int result = 0;
			try
			{
				FileStream fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read);
				long length = fileStream.Length;
				long num = length;
				if (length <= (long)maxBufferLength)
				{
					byte[] buffer = new byte[length];
					int num2 = fileStream.Read(buffer, 0, (int)length);
					result = clsSocket.SendData(socket, buffer, outTime);
				}
				else
				{
					byte[] buffer = new byte[maxBufferLength];
					while (num != 0L)
					{
						int num2 = fileStream.Read(buffer, 0, maxBufferLength);
						if ((result = clsSocket.SendData(socket, buffer, outTime)) < 0)
						{
							break;
						}
						num -= (long)num2;
					}
				}
				fileStream.Close();
			}
			catch (IOException message)
			{
				clsSocket.Log.Error(message);
				result = -4;
			}
			return result;
		}

		public static int SendFile(Socket socket, string fileName)
		{
			return clsSocket.SendFile(socket, fileName, 2048, 1);
		}

		public static int RecvFile(Socket socket, string fileName, long fileLength, int maxBufferLength, int outTime)
		{
			if (fileName == null || maxBufferLength <= 0)
			{
				clsSocket.Log.Warn("保存接收数据的文件名称为空或发送缓冲区的大小设置不正确.");
			}
			int result = 0;
			try
			{
				FileStream fileStream = new FileStream(fileName, FileMode.Create);
				if (fileLength <= (long)maxBufferLength)
				{
					byte[] buffer = new byte[fileLength];
					if ((result = clsSocket.RecvData(socket, buffer, outTime)) == 0)
					{
						fileStream.Write(buffer, 0, (int)fileLength);
					}
				}
				else
				{
					int num = maxBufferLength;
					long num2 = fileLength;
					byte[] buffer = new byte[num];
					while (num2 != 0L)
					{
						if ((result = clsSocket.RecvData(socket, buffer, outTime)) < 0)
						{
							break;
						}
						fileStream.Write(buffer, 0, num);
						num2 -= (long)num;
						num = (((long)maxBufferLength < num2) ? maxBufferLength : ((int)num2));
					}
				}
				fileStream.Close();
			}
			catch (IOException message)
			{
				clsSocket.Log.Error(message);
				result = -4;
			}
			return result;
		}

		public static int RecvFile(Socket socket, string fileName, long fileLength)
		{
			return clsSocket.RecvFile(socket, fileName, fileLength, 2048, 1);
		}
	}
}
