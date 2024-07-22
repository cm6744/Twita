using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twita.Common.Registry;
using Twita.Common.Toolkit;

namespace Twita.Grid
{

	public class SortableTileMap<T> : TileMap<T> where T : IPalettable
	{

		public SortableTileMap(Palette<T> palette, T defval, TileMapScale suggestion = null) 
		: base(palette, defval, suggestion)
		{
			Sorter = new Dictionary<int, List<TilePos>>[suggestion.SizeZ];
			for(int i = 0; i < Sorter.Length; i++) Sorter[i] = new();
		}

		public Dictionary<int, List<TilePos>>[] Sorter;

		public void Sort()
		{
			for(int x = 0; x < Scale.SizeX; x++)
				for(int y = 0; y < Scale.SizeY; y++)
					for(int z = 0; z < Scale.SizeZ; z++)
					{
						int id = Idat(x, y, z);
						if(!Sorter[z].ContainsKey(id))
						{
							Sorter[z][id] = new List<TilePos>();
						}
						Sorter[z][id].Add(new TilePos(x, y, z));
					}
		}

		public override T Set(int x, int y, int z, T obj)
		{
			int idx = Index(x, y, z);
			if(idx < 0 || idx + 3 >= Bytes.Length)
			{
				return Default;
			}
			T old = Palette[ReadBytes(idx)];
			WriteBytes(idx, obj.PaletteId);

			if(old.PaletteId != obj.PaletteId)
			{
				AddSort(x, y, z, obj.PaletteId);
				RemoveSort(x, y, z, old.PaletteId);
			}

			return old;
		}

		public void AddSort(int x, int y, int z, int id)
		{
			if(!Sorter[z].ContainsKey(id))
			{
				Sorter[z][id] = new List<TilePos>();
			}
			Sorter[z][id].Add(new TilePos(x, y, z));
		}

		public void RemoveSort(int x, int y, int z, int id)
		{
			if(!Sorter[z].ContainsKey(id))
			{
				return;
			}
			Sorter[z][id].Remove(new TilePos(x, y, z));
			if(Sorter[z][id].Count == 0)
			{
				Sorter[z].Remove(id);
			}
		}

	}

}
