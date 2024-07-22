using System.Collections.Generic;
using System;
using System.Collections;

namespace Twita.Common.Registry
{

	public class IdentityRegister<T> where T : Identifiable
	{

		public List<T> IdList = new List<T>();
		public Dictionary<Identity, T> IdMap = new Dictionary<Identity, T>();

		public int NextId = 1;

		private Queue<Runnable> delayedRegistry = new Queue<Runnable>();

		public T RegisterDefaultValue(Identity idt, T o)
		{
			o.Registry = new Identity(idt.Namespace, idt.Key, 0);
			IdList.Add(o);
			IdMap[o.Registry] = o;

			delayedRegistry.Enqueue(() =>
			{
				o.OnRegistry();
			});

			return o;
		}

		public T Register(Identity idt, T o)
		{
			o.Registry = new Identity(idt.Namespace, idt.Key, NextId);
			IdList.Add(o);
			IdMap[o.Registry] = o;
			NextId++;

			delayedRegistry.Enqueue(() =>
			{
				o.OnRegistry();
			});

			return o;
		}

		public void DoRegistry()
		{
			while(delayedRegistry.Count != 0)
			{
				delayedRegistry.Dequeue().Invoke();
			}
		}

		public T this[int index] => Get(index);
		public T this[Identity idt] => Get(idt);
		public T this[string idt] => Get(idt);
		public T DefaultValue => IdList[0];

		public T Get(int id)
		{
			return IdList[id];
		}

		public T Get(Identity idt)
		{
			return IdMap[idt];
		}

		public T Get(ref Identity idt)
		{
			return IdMap[idt];
		}

		public T Get(string key)
		{
			return IdMap[new Identity(key)];
		}

		public List<T> List()
		{
			return IdList;
		}

		public Dictionary<Identity, T> Mapped()
		{
			return IdMap;
		}

	}

}