using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Twita.Maths.Structs
{

	public interface IDimension
	{

		public float x { get; }
		public float xprom { get; }
		public float y { get; }
		public float yprom { get; }
		public float xcentral { get; }
		public float ycentral { get; }
		public float w { get; }
		public float h { get; }

	}

}
