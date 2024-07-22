using System.Collections.Generic;
using Twita.Codec;
using Twita.Common.Registry;

namespace Twita.Common.Resource
{

	public abstract class CommonLoader
	{

		public float Progress;
		public float Total;
		public float Run;
		protected bool DoneBasically;
		public int AsyncCount;
		public virtual bool Done => DoneBasically && AsyncCount <= 0;

		protected Queue<Runnable> PreLoadTasks = new Queue<Runnable>();
		protected Queue<Runnable> Tasks = new Queue<Runnable>();

		protected List<CommonLoader> Children = new List<CommonLoader>();

		public string Namespace;
		public readonly IdentityAtlas Mapper;
		public readonly Localer Localer;

		public FileHandler Filebase = FileSystem.GetAbsolute("");

		protected CommonLoader(string namespc, IdentityAtlas mapper, Localer localer)
		{
			Namespace = namespc;
			Mapper = mapper;
			Localer = localer;
		}

		public void SubLoader(CommonLoader loader)
		{
			if(loader == null)
			{
				return;
			}

			Children.Add(loader);

			Total += loader.Total;

			FlushProgress();
		}

		void Enqueue0(Runnable task, bool preLoad)
		{
			if(!preLoad)
			{
				Tasks.Enqueue(task);
				++Total;
				DoneBasically = false;
			}
			else
			{
				PreLoadTasks.Enqueue(task);
			}
		}

		public void DoPreLoad()
		{
			while(PreLoadTasks.Count != 0)
			{
				PreLoadTasks.Dequeue().Invoke();
			}

			foreach(CommonLoader c in Children)
			{
				c.DoPreLoad();
			}
		}

		public void Next()
		{
			if(Tasks.Count == 0)
			{
				if(Children.Count == 0)
				{
					DoneBasically = true;
					Progress = 1;
					return;
				}
				CommonLoader loader1 = Children[0];

				loader1.Next();
				++Run;
				Progress = Run / Total;

				if(loader1.Done)
				{
					Children.RemoveAt(0);
					return;
				}
			}
			else
			{
				Tasks.Dequeue().Invoke();
				++Run;
				Progress = Run / Total;
			}
		}

		public void FlushProgress()
		{
			Total = Tasks.Count;
			Run = 0;
			Progress = 0;
			DoneBasically = false;
		}

		public void Enqueue(Runnable task, bool preLoad)
		{
			Enqueue0(task, preLoad);
		}

		public void Enqueue(Runnable task)
		{
			Enqueue0(task, false);
		}

		public void PreEnqueue(Runnable task)
		{
			Enqueue0(task, true);
		}

		public void AsyncEnqueue(Runnable task)
		{
			AsyncCount++;
			Enqueue0(() =>
			{
				new Coroutine(() =>
				{
					task.Invoke();
					AsyncCount--;
				}).Start();
			}, true);
		}

		public void Load(FileHandler file, bool preload)
		{
			string resName = file.Path.Replace(Filebase.Path + "/", "");
			AddLoadTask(resName, file, preload);
		}

		public void Load(FileHandler file)
		{
			Load(file, false);
		}

		public void PreLoad(FileHandler file)
		{
			Load(file, true);
		}

		public void Scan(bool preload)
		{
			Scan(Filebase, preload);
		}

		public void Scan(FileHandler startPos, bool preload)
		{
			FileHandler[] files = startPos.Directories();

			foreach(FileHandler file in files)
			{
				Scan(file, preload);
			}

			files = startPos.Files();

			foreach(FileHandler file in files)
			{
				Load(file, preload);
			}
		}

		private void AddLoadTask(string resource, FileHandler file, bool preLoad)
		{
			ExpandTask(new Identity(Namespace, resource), file, preLoad);
		}

		protected abstract void ExpandTask(Identity resource, FileHandler file, bool preLoad);

	}

}