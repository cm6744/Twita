using System;
using System.Threading;
using Twita.Common.Manage;
using Twita.Common.Toolkit;
using Twita.Draw;
using Twita.Input;

namespace Twita.Common
{

	public class Platform
	{

		public static readonly VersionInfo Version = new VersionInfo("stable-1.0.0", 1);

		//Backends to implement
		public static GraphicEnvironment Graph;
		public static InputState InputState;
		public static DrawBatch Batch;

		public static readonly Lifecycle Lifecycle = new Lifecycle();

		public static bool IsExited;
		public static int Ticks;
		public static int Tps, Fps, Dpt;
		public static int DTps { get; private set; }
		public static string DFpsData = null;

		public static void RunStandard(int tps, int chaseback = 5, bool syncRender = false)
		{
			DFpsData = syncRender ? tps.ToString() : "No lim";

			Thread.CurrentThread.Name = "MAIN";

			double renderPartialTicks = 0f;
			double lastSyncSysClock = Graph.Nanotime;
			double tickLength = 1_000_000_000.0 / tps;
			int framesT = 0, framesR = 0;
			long lastCalcClock = Graph.Millitime;

			DTps = tps;

			Lifecycle.TaskLoad.Invoke();

			TickSchedule schedule = new TickSchedule();

			try
			{
				while(!IsExited)
				{
					double i = Graph.Nanotime;
					double elapsedPartialTicks = (i - lastSyncSysClock) / tickLength;
					lastSyncSysClock = i;
					renderPartialTicks += elapsedPartialTicks;
					int elapsedTicks = (int) renderPartialTicks;
					renderPartialTicks -= elapsedTicks;

					for(int j = 0; j < Math.Min(chaseback, elapsedTicks); j++)
					{
						framesT++;
						Ticks++;
						schedule.DeltaSecond = 1f / tps;
						schedule.Ticks = Ticks;

						InputState.StartRoll();
						Lifecycle.TaskTick.Invoke(schedule);
						InputState.EndRoll();

						if(!syncRender)
						{
							continue;
						}

						DrawAndSwap();
					}

					if(!syncRender)
					{
						DrawAndSwap();
					}

					if(Graph.Millitime - lastCalcClock < 1000)
					{
						continue;
					}

					lastCalcClock = Graph.Millitime;
					Tps = framesT;
					Fps = framesR;
					framesT = framesR = 0;
				}
			}
			catch(Exception e)
			{
				Log.Fatal(e);
			}
			finally
			{
				Finalization.FREE.OnFinalized();
			}

			return;

			void DrawAndSwap()
			{
				Graph.Prepare();
				Lifecycle.PartialTicks = (float) renderPartialTicks;
				Lifecycle.TaskRender.Invoke((float) renderPartialTicks);
				Graph.Swap();
				Dpt = Batch._DrawCalls;
				Batch._DrawCalls = 0;
				framesR++;
			}
		}

	}

}