using System.Collections.Generic;
using Twita.Codec;

namespace Twita.Common.Resource
{

	public class Localer
	{

		public Dictionary<string, BinaryCompound> Langs = new Dictionary<string, BinaryCompound>();
		public string LangKey = "EN_US";

		public void Load(string key, BinaryCompound compound)
		{
			Langs[key] = compound;
		}

		public string Get(string key)
		{
			if(!Langs.ContainsKey(LangKey))
			{
				return key;
			}

			BinaryCompound compound = Langs[LangKey];
			return compound.Has(key) ? compound.Get<string>(key) : key;
		}

	}

}