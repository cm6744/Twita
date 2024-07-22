using System;
using Twita.Input;

namespace Twita.Native.OpenGL
{

	public class GLInputObserver : InputObserver
	{

		public static long InputCheckTicks;

		long PressOccur = -1;
		byte Press;

		public void Fire()
		{
			Press = 1;
			PressOccur = InputCheckTicks;
		}

		public void Consume()
		{
			Press = 0;
			PressOccur = -1;
		}

		public int HoldTime()
		{
			if(Press != 1)
			{
				return 0;
			}

			return (int) (InputCheckTicks - PressOccur - 1);
		}

		public bool Pressed()
		{
			//A strange delay. In java these same codes work well...
			return Press == 1 && PressOccur + 1 == InputCheckTicks;
		}

		public bool Holding()
		{
			return Press == 1;
		}

	}

}