using System;
using Twita.Maths;

namespace Twita.Draw.Extended
{

	//This is the very light ninepatch with limited functions.
	//But it's too difficult to write a more complicated one.
	public class NinePatch : Icon
	{

		TexturePart LT, T, RT;
		TexturePart L, C, R;
		TexturePart LB, B, RB;
		float tw, th;
		float scale;

		public NinePatch(Texture texture) : this(texture, 1)
		{
		}

		public NinePatch(Texture tex, float scale)
		{
			this.scale = scale;

			const float p13 = 1f / 3;
			const float p23 = 2f / 3;

			tw = p13 * tex.Width * scale;
			th = p13 * tex.Height * scale;

			LT = TexturePart.ByPercentSize(tex, 0, 0, p13, p13);
			T = TexturePart.ByPercentSize(tex, p13, 0, p13, p13);
			RT = TexturePart.ByPercentSize(tex, p23, 0, p13, p13);
			L = TexturePart.ByPercentSize(tex, 0, p13, p13, p13);
			C = TexturePart.ByPercentSize(tex, p13, p13, p13, p13);
			R = TexturePart.ByPercentSize(tex, p23, p13, p13, p13);
			LB = TexturePart.ByPercentSize(tex, 0, p23, p13, p13);
			B = TexturePart.ByPercentSize(tex, p13, p23, p13, p13);
			RB = TexturePart.ByPercentSize(tex, p23, p23, p13, p13);
		}

		public float CeilW(float mw)
		{
			return (float) Math.Ceiling(mw / tw);
		}

		public float CeilH(float mh)
		{
			return (float) Math.Ceiling(mh / th);
		}

		public float ActualW(float mw)
		{
			return tw * (CeilW(mw) - 1);
		}

		public float ActualH(float mh)
		{
			return th * (CeilH(mh) - 1);
		}

		public void Draw(DrawBatch batch, float x, float y, float w, float h)
		{
			float nw = CeilW(w);
			float nh = CeilH(h);
			float x2 = x + ActualW(w);
			float y2 = y + ActualH(h);

			LB.Draw(batch, x, y, tw, th);
			for(int i = 1; i < nh - 1; i++)
			{
				L.Draw(batch, x, y + i * th, tw, th);//left draw
			}
			LT.Draw(batch, x, y2, tw, th);
			for(int i = 1; i < nw - 1; i++)
			{
				B.Draw(batch, x + i * tw, y, tw, th);//top draw
			}
			RB.Draw(batch, x2, y, tw, th);
			for(int i = 1; i < nh - 1; i++)
			{
				R.Draw(batch, x2, y + i * th, tw, th);//right draw
			}
			RT.Draw(batch, x2, y2, tw, th);
			for(int i = 1; i < nw - 1; i++)
			{
				T.Draw(batch, x + i * tw, y2, tw, th);//bottom draw
			}

			for(int i = 1; i < nw - 1; i++)
			{
				for(int j = 1; j < nh - 1; j++)
				{
					C.Draw(batch, x + i * tw, y + j * th, tw, th);//center draw
				}
			}
		}

	}

}