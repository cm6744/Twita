using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twita.Common;
using Twita.Maths;

namespace Twita.Draw.Extended
{

	public class Animation : Icon
	{

		TexturePart[] stream;
		int timeDelay;
		int maxIndex;
		int index;
		int clock;
		int lastClock;

		public Animation(Texture tex, int count, int w, int h, int u, int v)
		{
			maxIndex = count;
			stream = new TexturePart[maxIndex];

			for(int i = 0; i < maxIndex; i++)
			{
				stream[i] = TexturePart.BySize(tex, i * w + u, v, w, h);
			}
		}

		public Animation(params TexturePart[] parts)
		{
			stream = parts;
		}

		public Animation Seconds(float time)
		{
			timeDelay = Mth.Floor(time * Platform.DTps);
			return this;
		}

		public void SetClock(int clock)
		{
			this.clock = clock;
		}

		public void Draw(DrawBatch batch, float x, float y, float w, float h)
		{
			SetClock(Platform.Ticks);

			//to next frame.
			if(clock % timeDelay == 0 && clock != lastClock)
			{
				index++;
				if(index >= maxIndex)
				{
					index = 0;
				}
				lastClock = clock;
			}

			batch.Draw(stream[index], x, y, w, h);
		}

		public void Reset()
		{
			index = 0;
		}

		public int GetTimePerCycle()
		{
			return timeDelay * (maxIndex - 1);
		}

	}

}
