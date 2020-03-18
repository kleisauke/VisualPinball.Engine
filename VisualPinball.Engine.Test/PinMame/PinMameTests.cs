using System.Threading;
using VisualPinball.Engine.PinMame;
using VisualPinball.Engine.Test.Test;
using Xunit;
using Xunit.Abstractions;

namespace VisualPinball.Engine.Test.PinMame
{
	public class PinMameTests : BaseTests
	{

		private const string VpmPath = @"D:\Pinball\Visual Pinball\VPinMAME";
		public PinMameTests(ITestOutputHelper output) : base(output) { }

		[Fact]
		public void ShouldStartPinMame()
		{
			PinMAME.SetSampleRate(48000);
			PinMAME.SetVPMPath(VpmPath);
			var romIndex = PinMAME.StartThreadedGame("mm_109c", false);
			Logger.Info("Started PinMAME: " + romIndex);
			var i = 0;
			while (!PinMAME.IsGameReady() && i++ < 20) {
				Logger.Info("Waiting ({0})...", i);
				Thread.Sleep(500);
			}
			Logger.Info("Ready: " + PinMAME.IsGameReady());
			Logger.Info("Max lamps: {0}", PinMAME.GetMaxLamps());
			Logger.Info("DMD: {0}x{1}", PinMAME.GetRawDMDWidth(), PinMAME.GetRawDMDHeight());
		}
	}
}
