using System;
using System.Drawing;
using System.Numerics;
using OpenTK.Mathematics;
using Twita.Maths.Structs;
using Vector2 = System.Numerics.Vector2;

namespace Twita.Native.OpenGL
{

	public class GLUtil
	{

		public static Color ToColor(vec4 vec)
		{
			return Color.FromArgb((int) (vec.w * 255), (int) (vec.x * 255), (int) (vec.y * 255), (int) (vec.z * 255));
		}

		public static Matrix4x4 ToM4(matrix4 mat)
		{
			return new Matrix4x4(
				mat.m00, mat.m01, mat.m02, mat.m03,
				mat.m10, mat.m11, mat.m12, mat.m13,
				mat.m20, mat.m21, mat.m22, mat.m23,
				mat.m30, mat.m31, mat.m32, mat.m33
			);
		}

		public static vec2 SysToVec2(Vector2i vec)
		{
			return new vec2(vec.X, vec.Y);
		}

		public static Vector2 ToSysVec2(vec2 vec)
		{
			return new Vector2(vec.x, vec.y);
		}

	}

}