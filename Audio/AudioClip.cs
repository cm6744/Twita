using System.Collections.Generic;

namespace Twita.Audio
{

	public interface AudioClip
	{

		protected static List<AudioClip> ClipsPlaying = new List<AudioClip>();
		protected static List<AudioClip> ClipsStopped = new List<AudioClip>();

		public static void CheckClipStates()
		{
			ClipsPlaying.ForEach((c) =>
			{
				if(!c.IsPlaying() && !c.IsPaused())
				{
					ClipsStopped.Add(c);
					c.Dispose();
				}
			});

			ClipsPlaying.RemoveAll(ClipsStopped.Contains);
			ClipsStopped.Clear();
		}

		AudioData GetData();

		void Play();

		void Loop();

		void Pause();

		void Resume();

		void Stop();

		void Set(ClipController controller, object v);

		object Get(ClipController controller);

		bool IsPlaying();

		bool IsPaused();

		bool IsDestroyed();

		void Dispose();

	}

	public enum ClipController
	{

		Gain,
		Pitch,
		FramePosition,
		SourceLocation

	}

}