using System;
using System.Collections.Generic;
using Twita.Codec;
using Twita.Common.Registry;
using Twita.Common.Toolkit;

namespace Twita.Common.Resource
{

	public class IdentityAtlas
	{
		
		//STATIC

		static readonly Dictionary<string, IdentityAtlas> MappersAssets = new();
		static readonly Dictionary<string, IdentityAtlas> MappersContents = new();

		public static IdentityAtlas From(string nsp, IdentityAtlasCategory cat)
		{
			if(cat == IdentityAtlasCategory.Assets)
			{
				return MappersAssets.GetValueOrDefault(nsp, null);
			}
			return MappersContents.GetValueOrDefault(nsp, null);
		}

		//END

		private Dictionary<Type, object> ResDict = new Dictionary<Type, object>();

		public IdentityAtlas(string nsp, IdentityAtlasCategory cat)
		{
			if(cat == IdentityAtlasCategory.Assets) 
			{
				MappersAssets[nsp] = this;
			}
			else
			{
				MappersContents[nsp] = this;
			}
		}
		
		public void Load<T>(string key, T o)
		{
			GetManager<T>().Load(key, o);
		}

		public IdentityManager<T> GetManager<T>()
		{
			Type type = typeof(T);

			if(!ResDict.ContainsKey(type))
			{
				ResDict[type] = new IdentityManager<T>();
			}

			return ResDict[typeof(T)] as IdentityManager<T>;
		}

	}

}