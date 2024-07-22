using System.Runtime.InteropServices;
using OpenTK.Graphics.OpenGL;
using Twita.Common.Manage;

namespace Twita.Native.OpenGL
{

	public class Bufferobject<T> where T : unmanaged
	{

		public int Id;
		private BufferTarget type;

		public unsafe Bufferobject(T[] data, BufferTarget target, BufferUsageHint hint)
		{
			type = target;
			Id = GL.GenBuffer();

			Bind();

			GL.BufferData(type, data.Length * sizeof(T), data, hint);

			Finalization.FREE.OnHoldReferred(() => GL.DeleteBuffer(Id));
		}

		public void Bind()
		{
			GL.BindBuffer(type, Id);
		}

		public void Unbind()
		{
			GL.BindBuffer(type, 0);
		}

		public unsafe void UpdateBuffer(nint offset, T[] data, int len = -1)
		{
			GL.BufferSubData(type, offset, (len == -1 ? data.Length : len) * sizeof(T), data);
		}

	}

	public class VertArrayobject<T, I>
		where T : unmanaged
		where I : unmanaged
	{

		public int Id;

		public VertArrayobject(Bufferobject<T> vbo, Bufferobject<I> ebo)
		{
			Id = GL.GenVertexArray();

			Bind();
			vbo.Bind();
			ebo?.Bind();

			Finalization.FREE.OnHoldReferred(() => GL.DeleteVertexArray(Id));
		}

		public void Bind()
		{
			GL.BindVertexArray(Id);
		}

		public void Unbind()
		{
			GL.BindVertexArray(0);
		}

	}

}