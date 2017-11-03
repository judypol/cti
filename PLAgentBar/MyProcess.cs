using System;
using System.Runtime.InteropServices;

public class MyProcess
{
	public delegate bool EnumThreadWindowsCallback(IntPtr hWnd, IntPtr lParam);

	private bool haveMainWindow = false;

	private IntPtr mainWindowHandle = IntPtr.Zero;

	private int processId = 0;

	public IntPtr GetMainWindowHandle(int processId)
	{
		if (!this.haveMainWindow)
		{
			this.mainWindowHandle = IntPtr.Zero;
			this.processId = processId;
			MyProcess.EnumThreadWindowsCallback callback = new MyProcess.EnumThreadWindowsCallback(this.EnumWindowsCallback);
			MyProcess.EnumWindows(callback, IntPtr.Zero);
			GC.KeepAlive(callback);
			this.haveMainWindow = true;
		}
		return this.mainWindowHandle;
	}

	private bool EnumWindowsCallback(IntPtr handle, IntPtr extraParameter)
	{
		int num;
		MyProcess.GetWindowThreadProcessId(new HandleRef(this, handle), out num);
		bool result;
		if (num == this.processId && this.IsMainWindow(handle))
		{
			this.mainWindowHandle = handle;
			result = false;
		}
		else
		{
			result = true;
		}
		return result;
	}

	private bool IsMainWindowAndIsVisible(IntPtr handle)
	{
		return !(MyProcess.GetWindow(new HandleRef(this, handle), 4) != IntPtr.Zero) && MyProcess.IsWindowVisible(new HandleRef(this, handle));
	}

	private bool IsMainWindow(IntPtr handle)
	{
		return !(MyProcess.GetWindow(new HandleRef(this, handle), 4) != IntPtr.Zero);
	}

	[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
	public static extern bool EnumWindows(MyProcess.EnumThreadWindowsCallback callback, IntPtr extraData);

	[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
	public static extern int GetWindowThreadProcessId(HandleRef handle, out int processId);

	[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
	public static extern IntPtr GetWindow(HandleRef hWnd, int uCmd);

	[DllImport("user32.dll", CharSet = CharSet.Auto)]
	public static extern bool IsWindowVisible(HandleRef hWnd);
}
