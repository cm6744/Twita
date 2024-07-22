using System.Collections.Generic;
using Twita.Common.Registry;
using Twita.Common.Toolkit;

namespace Twita.Common.Manage
{

	public class Finalization
	{

		public static Finalization FREE = new Finalization();

		private List<Runnable> ToRelease = new List<Runnable>();

		public void OnHoldReferred(Runnable release)
		{
			ToRelease.Add(release);
		}

		public void OnFinalized()
		{
			ToRelease.ForEach(r => r.Invoke());
			ToRelease.Clear();

			if(this == FREE)
			{
				Log.Info("Final disposing ended. GLOBAL-FREE was cleared.");
				Log.TryEndStreamWriting();
			}
		}


		private static Dictionary<Identity, Finalization> ShortNatives = new Dictionary<Identity, Finalization>();

		//Provides a short-time native manager, and you're supposed to free it by yourself when its usage ends.
		public static Finalization GetFree(Identity key)
		{
			if(!ShortNatives.ContainsKey(key))
			{
				ShortNatives[key] = new Finalization();
			}

			return ShortNatives[key];
		}

		public static void Free(Identity key)
		{
			ShortNatives[key]?.OnFinalized();
			ShortNatives.Remove(key);
		}

	}

}