using System.Windows.Forms;
using System.Management;
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Text;

namespace Audio_Switcher {
	public partial class frmAudioSwitcher : Form {

		const int BM_CLICK = 0x00F5;

		List<SoundDevice> soundDevices = new List<SoundDevice>();

		[DllImport("user32.dll")]
		public static extern IntPtr FindWindow(string sClassName, string sAppName);

		[DllImport("user32.dll")]
		public static extern bool SetForegroundWindow(IntPtr hWnd);

		[DllImport("user32.dll")]
		public static extern int SendMessage(IntPtr hWnd, int wMsg, IntPtr wParam, IntPtr lParam);

		[DllImport("user32.dll")]
		public static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, String type, String name);

		[DllImport("user32.dll")]
		static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int count);

		public frmAudioSwitcher() {
			InitializeComponent();
			GetOutputDevices();
			lstBoxOutputDevices.Items.AddRange(soundDevices.ToArray());
		}

		private void GetOutputDevices() {
			soundDevices.Clear();
			ManagementObjectSearcher mos = new ManagementObjectSearcher("select * from Win32_SoundDevice");
			foreach (ManagementObject soundObject in mos.Get())
				soundDevices.Insert(0, new SoundDevice(soundObject));
		}

		private void lstBoxOutputDevices_SelectedIndexChanged(object sender, System.EventArgs e) {
			string deviceName = soundDevices[lstBoxOutputDevices.SelectedIndex].name;

			Process process = new Process();
			process.StartInfo.FileName = "rundll32.exe";
			process.StartInfo.Arguments = "shell32.dll,Control_RunDLL mmsys.cpl,,0";
			process.Start();

			IntPtr windowID;
			do {
				windowID = FindWindow(null, "Sound");
			} while (windowID.ToInt32() == 0);

			SetForegroundWindow(windowID);

			for (int i = -1; i < lstBoxOutputDevices.SelectedIndex; i++)
				SendKey("^+{DOWN}");

			//SendMessage(0, 0, 0, 0);
			IntPtr pointer = FindWindowEx(windowID, IntPtr.Zero, "BUTTON", null);

			List<IntPtr> children = GetChildWindows(windowID);

			string title;
			foreach (IntPtr ptr in children) {
				title = GetWindowTitle(ptr);
				if (title == null)
					continue;

				title = title.Trim();

				if (title.Equals("&Set Default"))
					SendMessage(ptr, BM_CLICK, IntPtr.Zero, IntPtr.Zero);
				else if (title.Equals("OK"))
					SendMessage(ptr, BM_CLICK, IntPtr.Zero, IntPtr.Zero);
			}

			Close();
		}

		private static void SendKey(string keys) {
			SendKeys.SendWait(keys);
			Thread.Sleep(50);
		}

		private class SoundDevice {
			public string deviceID;
			public string name;

			public SoundDevice(ManagementObject soundDevice) {
				deviceID = soundDevice.GetPropertyValue("DeviceID").ToString();
				name = soundDevice.GetPropertyValue("Name").ToString();
			}

			public override string ToString() {
				return name;
			}
		}

		public static string GetWindowTitle(IntPtr hWnd) {
			const int nChars = 256;
			StringBuilder Buff = new StringBuilder(nChars);

			if (GetWindowText(hWnd, Buff, nChars) > 0)
				return Buff.ToString();

			return null;
		}

		[DllImport("user32")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool EnumChildWindows(IntPtr window, EnumWindowProc callback, IntPtr i);

		/// <summary>
		/// Returns a list of child windows
		/// </summary>
		/// <param name="parent">Parent of the windows to return</param>
		/// <returns>List of child windows</returns>
		public static List<IntPtr> GetChildWindows(IntPtr parent) {
			List<IntPtr> result = new List<IntPtr>();
			GCHandle listHandle = GCHandle.Alloc(result);
			try {
				EnumWindowProc childProc = new EnumWindowProc(EnumWindow);
				EnumChildWindows(parent, childProc, GCHandle.ToIntPtr(listHandle));
			}
			finally {
				if (listHandle.IsAllocated)
					listHandle.Free();
			}
			return result;
		}

		/// <summary>
		/// Callback method to be used when enumerating windows.
		/// </summary>
		/// <param name="handle">Handle of the next window</param>
		/// <param name="pointer">Pointer to a GCHandle that holds a reference to the list to fill</param>
		/// <returns>True to continue the enumeration, false to bail</returns>
		private static bool EnumWindow(IntPtr handle, IntPtr pointer) {
			GCHandle gch = GCHandle.FromIntPtr(pointer);
			List<IntPtr> list = gch.Target as List<IntPtr>;
			if (list == null) {
				throw new InvalidCastException("GCHandle Target could not be cast as List<IntPtr>");
			}
			list.Add(handle);
			//  You can modify this to check to see if you want to cancel the operation, then return a null here
			return true;
		}

		/// <summary>
		/// Delegate for the EnumChildWindows method
		/// </summary>
		/// <param name="hWnd">Window handle</param>
		/// <param name="parameter">Caller-defined variable; we use it for a pointer to our list</param>
		/// <returns>True to continue enumerating, false to bail.</returns>
		public delegate bool EnumWindowProc(IntPtr hWnd, IntPtr parameter);

	}
}
