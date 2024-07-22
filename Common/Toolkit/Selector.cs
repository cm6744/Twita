using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twita.Maths;

namespace Twita.Common.Toolkit
{

	public class Selector<E>
	{

		List<E> objects = new();

		public Selector<E> Put(E obj, int weight)
		{
			for(int i = 0; i < weight; i++)
			{
				objects.Add(obj);
			}
			return this;
		}

		public E Get(int index)
		{
			return objects[index];
		}

		public E Get(RandomGenerator gen)
		{
			return gen.Select<E>(objects);
		}

		public int Count => objects.Count;

	}

}
