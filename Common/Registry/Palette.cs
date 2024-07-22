using System.Collections.Generic;

namespace Twita.Common.Registry
{

	public class Palette<V>
	{

		private Dictionary<int, V> map0 = new Dictionary<int, V>();
		private List<V> list0 = new List<V>();
		private Dictionary<V, int> map1 = new Dictionary<V, int>();
		private int idCount;

		public void Add(V v)
		{
			map0[idCount] = v;
			list0.Add(v);
			map1[v] = idCount;

			if(v is IPalettable p)
			{
				p.PaletteId = idCount;
				//Inject the id.
			}

			idCount++;
		}

		public V Get(int id)
		{
			return list0[id];
		}

		public int IdFor(V v)
		{
			return map1[v];
		}

		public Dictionary<int, V> Mapped()
		{
			return map0;
		}

		public V this[int index]
		{
			get => Get(index);
		}

	}

}