using System;
using System.Collections.Generic;

namespace Twita.Maths.Structs
{

	public struct vec2
	{

		public static readonly vec2 Nil = new vec2(0, 0);
		public static readonly vec2 One = new vec2(1, 1);

		public float x, y;

		public int xi => (int) x;
		public int yi => (int) y;

		public float Len => Mth.Sqrt(x * x + y * y);

		public vec2(float x, float y)
		{
			this.x = x;
			this.y = y;
		}

		public vec2(IReadOnlyList<float> vec)
		{
			x = vec[0];
			y = vec[1];
		}

		public vec2 FromDeg(float ln, float deg)
		{
			x = Mth.CosDeg(deg) * ln;
			y = Mth.SinDeg(deg) * ln;
			return this;
		}

		public vec2 FromRad(float ln, float rad)
		{
			x = Mth.CosRad(rad) * ln;
			y = Mth.SinRad(rad) * ln;
			return this;
		}

		public static vec2 operator +(vec2 vec1, vec2 vec2)
		{
			return new vec2(vec1.x + vec2.x, vec1.y + vec2.y);
		}

		public static vec2 operator -(vec2 vec1, vec2 vec2)
		{
			return new vec2(vec1.x - vec2.x, vec1.y - vec2.y);
		}

		public static vec2 operator *(vec2 vec1, vec2 vec2)
		{
			return new vec2(vec1.x * vec2.x, vec1.y * vec2.y);
		}

		public static vec2 operator /(vec2 vec1, vec2 vec2)
		{
			return new vec2(vec1.x / vec2.x, vec1.y / vec2.y);
		}

		public static vec2 operator +(vec2 vec1, float v)
		{
			return new vec2(vec1.x + v, vec1.y + v);
		}

		public static vec2 operator -(vec2 vec1, float v)
		{
			return new vec2(vec1.x - v, vec1.y - v);
		}

		public static vec2 operator *(vec2 vec1, float v)
		{
			return new vec2(vec1.x * v, vec1.y * v);
		}

		public static vec2 operator /(vec2 vec1, float v)
		{
			return new vec2(vec1.x / v, vec1.y / v);
		}

		public static vec2 operator -(vec2 vec)
		{
			return new vec2(-vec.x, -vec.y);
		}

		//Dot Operation.
		public static float operator ^(vec2 vec1, vec2 vec2)
		{
			return vec1.x * vec2.x + vec1.y * vec2.y;
		}

	}

	public struct vec3
	{

		public float x, y, z;

		public vec3(float x, float y, float z)
		{
			this.x = x;
			this.y = y;
			this.z = z;
		}

		public vec3(IReadOnlyList<float> list) : this(list[0], list[1], list[2])
		{
		}

		public static vec3 operator +(vec3 vec1, vec3 vec2)
		{
			return new vec3(vec1.x + vec2.x, vec1.y + vec2.y, vec1.z + vec2.z);
		}

		public static vec3 operator -(vec3 vec1, vec3 vec2)
		{
			return new vec3(vec1.x - vec2.x, vec1.y - vec2.y, vec1.z - vec2.z);
		}

		public static vec3 operator *(vec3 vec1, vec3 vec2)
		{
			return new vec3(vec1.x * vec2.x, vec1.y * vec2.y, vec1.z * vec2.z);
		}

		public static vec3 operator /(vec3 vec1, vec3 vec2)
		{
			return new vec3(vec1.x / vec2.x, vec1.y / vec2.y, vec1.z / vec2.z);
		}

		public static vec3 operator +(vec3 vec1, float v)
		{
			return new vec3(vec1.x + v, vec1.y + v, vec1.z + v);
		}

		public static vec3 operator -(vec3 vec1, float v)
		{
			return new vec3(vec1.x - v, vec1.y - v, vec1.z - v);
		}

		public static vec3 operator *(vec3 vec1, float v)
		{
			return new vec3(vec1.x * v, vec1.y * v, vec1.z * v);
		}

		public static vec3 operator /(vec3 vec1, float v)
		{
			return new vec3(vec1.x / v, vec1.y / v, vec1.z / v);
		}

		public static vec3 operator -(vec3 vec)
		{
			return new vec3(-vec.x, -vec.y, -vec.z);
		}

		//Dot Operation.
		public static float operator ^(vec3 vec1, vec3 vec2)
		{
			return vec1.x * vec2.x + vec1.y * vec2.y + vec1.z * vec2.z;
		}

	}

	public struct vec4
	{

		public float x, y, z, w;

		public vec4(float x, float y, float z, float w)
		{
			this.x = x;
			this.y = y;
			this.z = z;
			this.w = w;
		}

		public vec4(IReadOnlyList<float> list) : this(list[0], list[1], list[2], list[3])
		{
		}

		public static vec4 operator +(vec4 vec1, vec4 vec2)
		{
			return new vec4(vec1.x + vec2.x, vec1.y + vec2.y, vec1.z + vec2.z, vec1.w + vec2.w);
		}

		public static vec4 operator -(vec4 vec1, vec4 vec2)
		{
			return new vec4(vec1.x - vec2.x, vec1.y - vec2.y, vec1.z - vec2.z, vec1.w - vec2.w);
		}

		public static vec4 operator *(vec4 vec1, vec4 vec2)
		{
			return new vec4(vec1.x * vec2.x, vec1.y * vec2.y, vec1.z * vec2.z, vec1.w * vec2.w);
		}

		public static vec4 operator /(vec4 vec1, vec4 vec2)
		{
			return new vec4(vec1.x / vec2.x, vec1.y / vec2.y, vec1.z / vec2.z, vec1.w / vec2.w);
		}

		public static vec4 operator +(vec4 vec1, float v)
		{
			return new vec4(vec1.x + v, vec1.y + v, vec1.z + v, vec1.w + v);
		}

		public static vec4 operator -(vec4 vec1, float v)
		{
			return new vec4(vec1.x - v, vec1.y - v, vec1.z - v, vec1.w - v);
		}

		public static vec4 operator *(vec4 vec1, float v)
		{
			return new vec4(vec1.x * v, vec1.y * v, vec1.z * v, vec1.w * v);
		}

		public static vec4 operator /(vec4 vec1, float v)
		{
			return new vec4(vec1.x / v, vec1.y / v, vec1.z / v, vec1.w / v);
		}

		public static vec4 operator -(vec4 vec)
		{
			return new vec4(-vec.x, -vec.y, -vec.z, -vec.w);
		}

		//Dot Operation.
		public static float operator ^(vec4 vec1, vec4 vec2)
		{
			return vec1.x * vec2.x + vec1.y * vec2.y + vec1.z * vec2.z + vec1.w * vec2.w;
		}

		public float ToFloatBits()
		{
			uint color = (uint) (255 * w) << 24 | (uint) (255 * z) << 16 | (uint) (255 * y) << 8 | (uint) (255 * x);
			uint bits = color & 0xfeffffff;
			int s = bits >> 31 == 0 ? 1 : -1;
			uint e = bits >> 23 & 0xff;
			uint m = e == 0 ? (bits & 0x7fffff) >> 1 : bits & 0x7fffff | 0x800000;
			float res = (float) (s * m * Math.Pow(2, e - 150));
			return res;
		}

	}

}