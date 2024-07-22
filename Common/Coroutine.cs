using System;
using System.Collections.Generic;
using System.Threading;

namespace Twita.Common
{

	public class Coroutine
	{

		public bool IsCompleted;
		public bool IsStarted;

		public Runnable Task;
		private Timer timer;

		public Coroutine(Runnable runnable)
		{
			Task = runnable;
		}

		public void Start()
		{
			ThreadPool.QueueUserWorkItem(s =>
			{
				IsStarted = true;
				Task.Invoke();
				IsCompleted = true;
				Dispose();
			});
		}

		public void Start(TimeSpan time)
		{
			timer = new Timer((o) =>
			{
				Start();
			}, this, time, Timeout.InfiniteTimeSpan);
		}

		public void Dispose()
		{
			timer?.Dispose();
		}

	}

}