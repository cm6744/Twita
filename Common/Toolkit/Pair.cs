using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Twita.Common.Toolkit
{

	public struct Pair<V1, V2>
	{
		
		public readonly V1 Val1;
		public readonly V2 Val2;

		public Pair(V1 v1, V2 v2)
		{
			Val1 = v1;
			Val2 = v2;
		}

	}

}
