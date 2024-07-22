using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Twita.Common.Toolkit
{

	public struct Bitoy
	{

		public int Src;

		public bool Read(int mask)
		{
			return (Src >> mask & 1) == 1;
		}

		public void Write(int mask, bool v)
		{
			if(v) Src |= (0x1 << mask);
			else Src &= ~(0x1 << mask);
		}

		public bool this[int index]
		{
			get => Read(index);
			set => Write(index, value);
		}

	}

}
