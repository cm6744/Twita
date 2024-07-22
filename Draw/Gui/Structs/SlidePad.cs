using System;
using System.Globalization;
using Twita.Common;
using Twita.Input;
using Twita.Maths.Structs;

namespace Twita.Draw.Gui.Structs
{

	public delegate string TextConvert(float value);

	public class SlidePad : Component
	{

		public float Value, MaxValue, MinValue, StepValue;

		public Icon Icon;

		public string Display = "";
		public TextConvert TextRelinker = (f) => f.ToString("%.2f", CultureInfo.InvariantCulture);

		Button decrease, increase;
		box4 decRect, incRect;
		int scrollBuf;

		public SlidePad(Button dec, Button inc)
		{
			decrease = dec;
			increase = inc;
			decrease.OnLeftFired += () =>
			{
				Value -= StepValue;
				Check();
			};
			increase.OnLeftFired += () =>
			{
				Value += StepValue;
				Check();
			};
			decRect = decrease.Bound;
			incRect = increase.Bound;
		}

		public void Correct()
		{
			decRect.Locate(Bound.x, Bound.y);
			incRect.Locate(Bound.xprom - incRect.w, Bound.y);
		}

		private void Check()
		{
			//EC BUG 2024/1/3 Value steps have float inaccuracy.
			if(Value < MinValue - 0.0001f)
			{
				Value = MaxValue;
			}
			if(Value > MaxValue + 0.0001f)
			{
				Value = MinValue;
			}
		}

		public override void Input(InputState input, mutvec2 cursor)
		{
			float scr = input.GetCursorScroll();

			if(Bound.Contains(cursor) && scrollBuf < 0 && scr != 0)
			{
				switch(input.GetScrollDirection())
				{
					case ScrollDirection.UP:
						Value += StepValue;
						Check();
						break;
					case ScrollDirection.DOWN:
						Value -= StepValue;
						Check();
						break;
				}
				scrollBuf = 2;
				input.ConsumeCursorScroll();
			}
			scrollBuf--;

			decrease.Input(input, cursor);
			increase.Input(input, cursor);
		}

		public override void Update(TickSchedule schedule)
		{
			decrease.Update(schedule);
			increase.Update(schedule);
		}

		public override void SaveData()
		{
			PersistentData.Set("Value", Value);
		}

		public override void ReloadData()
		{
			if(PersistentData.Try("Value", out float val))
			{
				Value = val;
			}
		}

		public override void Draw(DrawBatch batch)
		{
			batch.Draw(Icon, Bound);

			string special = TextRelinker.Invoke(Value);
			string drw = Display + (string.IsNullOrWhiteSpace(Display) ? "" : ": ") + special;

			batch.Draw(drw, Bound.xcentral, Bound.y + 4, Align.CENTER);

			decrease.Draw(batch);
			increase.Draw(batch);
		}

	}

}