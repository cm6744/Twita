using System.Collections.Generic;

namespace Twita.Maths.Structs
{

	public class mutvec2
	{

		public float x, y;

		public int xi => (int) x;
		public int yi => (int) y;

		public float Len => Mth.Sqrt(x * x + y * y);

		public mutvec2()
		{
		}

		public mutvec2(float x, float y)
		{
			this.x = x;
			this.y = y;
		}

		public mutvec2(IReadOnlyList<float> rvec)
		{
			x = rvec[0];
			y = rvec[1];
		}

		public void Copy(mutvec2 vec)
		{
			x = vec.x;
			y = vec.y;
		}

		public static mutvec2 operator +(mutvec2 rvec1, mutvec2 rvec2)
		{
			rvec1.x += rvec2.x;
			rvec1.y += rvec2.y;
			return rvec1;
		}

		public static mutvec2 operator +(mutvec2 rvec1, vec2 vec2)
		{
			rvec1.x += vec2.x;
			rvec1.y += vec2.y;
			return rvec1;
		}

		public static mutvec2 operator -(mutvec2 rvec1, mutvec2 rvec2)
		{
			rvec1.x -= rvec2.x;
			rvec1.y -= rvec2.y;
			return rvec1;
		}

		public static mutvec2 operator -(mutvec2 rvec1, vec2 vec2)
		{
			rvec1.x -= vec2.x;
			rvec1.y -= vec2.y;
			return rvec1;
		}

		public static mutvec2 operator *(mutvec2 rvec1, mutvec2 rvec2)
		{
			rvec1.x *= rvec2.x;
			rvec1.y *= rvec2.y;
			return rvec1;
		}

		public static mutvec2 operator *(mutvec2 rvec1, vec2 vec2)
		{
			rvec1.x *= vec2.x;
			rvec1.y *= vec2.y;
			return rvec1;
		}

		public static mutvec2 operator /(mutvec2 rvec1, mutvec2 rvec2)
		{
			rvec1.x /= rvec2.x;
			rvec1.y /= rvec2.y;
			return rvec1;
		}

		public static mutvec2 operator /(mutvec2 rvec1, vec2 vec2)
		{
			rvec1.x /= vec2.x;
			rvec1.y /= vec2.y;
			return rvec1;
		}

		public static mutvec2 operator -(mutvec2 rvec)
		{
			rvec.x = -rvec.x;
			rvec.y = -rvec.y;
			return rvec;
		}

		//Dot Operation.
		public static float operator ^(mutvec2 rvec1, mutvec2 rvec2)
		{
			return rvec1.x * rvec2.x + rvec1.y * rvec2.y;
		}

	}

}