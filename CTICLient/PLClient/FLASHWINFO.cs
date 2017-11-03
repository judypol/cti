using System;

namespace PLClient
{
	public struct FLASHWINFO
	{
		public uint cbSize;

		public IntPtr hwnd;

		public uint dwFlags;

		public uint uCount;

		public uint dwTimeout;
	}
}
