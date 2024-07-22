using Twita.Common;

namespace Twita.Draw.Gui
{

	public class Resolution
	{

		public float Xsize;
		public float Ysize;
		public float Factor;

		public Resolution(Screen curScreen, float scaleForced = 0, bool limitInt = false)
		{
			Xsize = Platform.Graph.Size.x;
			Ysize = Platform.Graph.Size.y;

			if(scaleForced <= 0)
			{
				Factor = 0.5f;

				while(Factor < 1.5f && Xsize / (Factor + 1) >= 320
				      && Ysize / (Factor + 1) >= 240)
				{
					Factor += 0.5f;
				}

				if(limitInt && (Factor * 2) % 2 != 0 && (Factor - 0.5f) > 0)
				{
					Factor -= 0.5f;
				}
			}
			else
			{
				Factor = scaleForced;
			}

			if(curScreen != null)
			{
				Factor *= curScreen.ScaleMul;

				float locked = curScreen.ScaleLocked;

				if(locked > 0)
				{
					Factor = locked;
				}
			}

			Xsize /= Factor;
			Ysize /= Factor;

			curScreen?.Resolve(this);
		}

	}

}