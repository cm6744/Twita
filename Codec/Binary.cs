using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Twita.Codec.General;

namespace Twita.Codec
{

	public interface IBinary
	{
	}

	public class BinaryCompound : IBinary
	{

		//The map might contains some non-primitive values.
		public Dictionary<string, object> Map;
		public readonly bool IsReadOnly;

		public BinaryCompound(Dictionary<string, object> MapCpy, bool isReadOnly = false)
		{
			Map = MapCpy;
			IsReadOnly = isReadOnly;
		}

		public BinaryCompound(bool isReadOnly = false)
		{
			Map = new Dictionary<string, object>();
			IsReadOnly = isReadOnly;
		}

		public T Get<T>(string key)
		{
			return (T) Map.GetValueOrDefault(key, default);
		}

		public T Get<T>(string key, T defv)
		{
			return (T) Map.GetValueOrDefault(key, defv);
		}

		public void Set(string key, object v)
		{
			if(IsReadOnly)
			{
				return;
			}

			Map[key] = v;
		}

		public bool Has(string key)
		{
			return Map.ContainsKey(key);
		}

		public bool Try<T>(string key, out T valout)
		{
			if(Map.ContainsKey(key))
			{
				valout = Get<T>(key);
				return true;
			}

			valout = default;
			return false;
		}

		//Wrapped Operations

		public void Clear()
		{
			if(IsReadOnly)
			{
				return;
			}

			Map.Clear();
		}

		public void Merge(BinaryCompound compound)
		{
			if(IsReadOnly)
			{
				return;
			}

			foreach(var pair in compound.Map)
			{
				if(!Map.ContainsKey(pair.Key))
				{
					Map[pair.Key] = pair.Value;
				}
			}
		}

		public BinaryCompound Copy()
		{
			BinaryCompound compound = new BinaryCompound();

			foreach(KeyValuePair<string, object> pair in Map)
			{
				switch(pair.Value)
				{
					case BinaryCompound c1:
						compound.Set(pair.Key, c1.Copy());
						break;
					case BinaryList l1:
						compound.Set(pair.Key, l1.Copy());
						break;
					default:
						compound.Set(pair.Key, pair.Value);
						break;
				}
			}

			return compound;
		}

		public bool Compare(BinaryCompound compound)
		{
			if(Map.Count != compound.Map.Count)
			{
				return false;
			}

			foreach(var kv in compound.Map)
			{
				object o1 = kv.Value;
				object o2 = Map[kv.Key];

				if(o1.GetType() != o2.GetType())
				{
					return false;
				}

				bool eq;
				switch(o2)
				{
					case BinaryCompound c1:
						eq = c1.Compare((BinaryCompound) o1);
						break;
					case BinaryList l1:
						eq = l1.Compare((BinaryList) o1);
						break;
					default:
						eq = o1.Equals(o2);
						break;
				}

				if(!eq) return false;
			}

			return true;
		}

	}

	public class BinaryList : IBinary
	{

		public List<object> Values;

		public byte Type
		{
			get
			{
				if(Values == null || Values.Count == 0)
				{
					return 0;
				}

				return BinaryIO.GetId(Values[0]);
			}
		}

		public int Count => Values.Count;

		public BinaryList(List<object> ListCpy)
		{
			Values = ListCpy;
		}

		public BinaryList()
		{
			Values = new List<object>();
		}

		public object this[int index]
		{
			get => Get<object>(index);
			set => Set(index, value);
		}

		public BinaryList Copy()
		{
			BinaryList lst = new BinaryList();

			foreach(object o in Values)
			{
				switch(o)
				{
					case BinaryCompound c1:
						lst.Add(c1.Copy());
						break;
					case BinaryList l1:
						lst.Add(l1.Copy());
						break;
					default:
						lst.Add(o);
						break;
				}
			}

			return lst;
		}

		public T Get<T>(int i)
		{
			return (T) Values[i];
		}

		public void Add(object v)
		{
			Values.Add(v);
		}

		public void Set(int i, object v)
		{
			Values[i] = v;
		}

		public bool Compare(BinaryList list)
		{
			if(Values.Count != list.Values.Count)
			{
				return false;
			}

			for(int i = 0; i < list.Count; i++)
			{
				object o1 = this[i];
				object o2 = list[i];

				if(o1.GetType() != o2.GetType())
				{
					return false;
				}

				bool eq;
				switch(o2)
				{
					case BinaryCompound c1:
						eq = c1.Compare((BinaryCompound) o1);
						break;
					case BinaryList l1:
						eq = l1.Compare((BinaryList) o1);
						break;
					default:
						eq = o1.Equals(o2);
						break;
				}

				if(!eq) return false;
			}

			return true;
		}

	}

}