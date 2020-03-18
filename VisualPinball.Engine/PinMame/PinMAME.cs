using System.Runtime.InteropServices;

namespace VisualPinball.Engine.PinMame
{
	public static class PinMAME
	{

		#region Setup functions
		[DllImport("PinMAME64.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern void SetVPMPath(string path);

		[DllImport("PinMAME64.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern void SetSampleRate(int sampleRate);
		#endregion

		#region Game related functions
		[DllImport("PinMAME64.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern int StartThreadedGame(string gameName, bool showConsole);

		[DllImport("PinMAME64.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern void StopThreadedGame(bool locking);

		[DllImport("PinMAME64.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern bool IsGameReady();
		#endregion

		#region Pause related functions
		[DllImport("PinMAME64.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern void ResetGame();

		[DllImport("PinMAME64.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern void Pause();

		[DllImport("PinMAME64.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern void Continue();

		[DllImport("PinMAME64.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern bool IsPaused();
		#endregion

		#region DMD related functions
		[DllImport("PinMAME64.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern bool NeedsDMDUpdate();

		[DllImport("PinMAME64.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern int GetRawDMDWidth();

		[DllImport("PinMAME64.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern int GetRawDMDHeight();

		[DllImport("PinMAME64.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern int GetRawDMDPixels(out byte[] buffer);
		#endregion

		#region Audio related functions
		[DllImport("PinMAME64.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern int GetAudioChannels();


		[DllImport("PinMAME64.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern int GetPendingAudioSamples(float[] buffer, int outChannels, int maxNumber);
		#endregion

		#region Switch related functions
		[DllImport("PinMAME64.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern bool GetSwitch(int slot);

		[DllImport("PinMAME64.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern void SetSwitch(int slot, bool state);
		#endregion

		#region Lamps related functions
		[DllImport("PinMAME64.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern int GetMaxLamps();

		[DllImport("PinMAME64.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern int GetChangedLamps(int[] changedStates);
		#endregion

		#region Solenoids related functions
		[DllImport("PinMAME64.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern int GetMaxSolenoids();

		[DllImport("PinMAME64.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern int GetChangedSolenoids(int[] changedStates);
		#endregion

		#region GI strings related functions
		[DllImport("PinMAME64.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern int GetMaxGIStrings();

		[DllImport("PinMAME64.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
		public static extern int GetChangedGIs(int[] changedStates);
		#endregion
	}
}
