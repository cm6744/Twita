using System.Collections.Generic;

namespace Twita.Common.Manage
{

	public delegate object Patch();

	//As a content changer of the whole game, the Patcher class
	//is intended to relink factory object creation to provide highly customized performance.
	public class Patcher
	{

		public static Dictionary<string, Patch> Patches = new Dictionary<string, Patch>();

		public static void SetPatch(string key, Patch patch)
		{
			Patches[key] = patch;
		}

		public static T Fix<T>(string key, Patch normalVal)
		{
			if(!Patches.ContainsKey(key))
			{
				return (T) normalVal.Invoke();
			}

			return (T) Patches[key].Invoke();
		}

	}

}