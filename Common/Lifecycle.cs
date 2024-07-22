namespace Twita.Common
{

	public delegate void RunRender(float partial);

	public delegate void RunTick(TickSchedule schedule);

	public delegate void Resize(int ow, int oh, int w, int h);

	public delegate void Load();

	public class Lifecycle
	{

		public static float PartialTicks;

		public Load TaskLoad = () => { };
		public RunRender TaskRender = (partial) => { };
		public RunTick TaskTick = (s) => { };
		public Resize TaskResize = (ow1, oh1, w1, w2) => { };

	}

}