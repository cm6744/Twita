using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Twita.Common.Toolkit
{

	public static class ArrayWriter
	{

		public static int GetXyzwIndex(int w, int h, int d, int x, int y, int z)
		{
			int prx = w - 1;
			int pry = h - 1;
			x &= prx;
			y &= pry;

			return (x * h + y) * d + z;
		}

	}

}
