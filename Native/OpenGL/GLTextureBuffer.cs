using OpenTK.Graphics.OpenGL;
using Twita.Codec;
using Twita.Draw;
using Twita.Maths.Structs;

namespace Twita.Native.OpenGL
{

	public class GLTextureBuffer : TextureBuffer
	{

		public Texture Src { get; }
		private vec4 viewport;
		private vec4 tmpVp;
		public int Id;

		public GLTextureBuffer(int w, int h)
		{
			Id = GL.GenFramebuffer();
			int tid = GL.GenTexture();

			GL.BindFramebuffer(FramebufferTarget.Framebuffer, Id);
			GL.BindTexture(TextureTarget.Texture2D, tid);

			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter,
				(int) GLDeviceSettings.FilterMag);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter,
				(int) GLDeviceSettings.FilterMin);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS,
				(int) TextureWrapMode.Repeat);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT,
				(int) TextureWrapMode.Repeat);

			GL.TexImage2D(
				TextureTarget.Texture2D,
				0,
				PixelInternalFormat.Rgba,
				w,
				h,
				0,
				PixelFormat.Rgba,
				PixelType.UnsignedByte,
				(byte[]) null
			);
			GL.FramebufferTexture2D
			(
				FramebufferTarget.Framebuffer,
				FramebufferAttachment.ColorAttachment0,
				TextureTarget.Texture2D,
				tid,
				0
			);

			GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);

			Src = new GLTexture(tid, w, h);
			((GLTexture) Src).IsFB = true;

			viewport = new vec4(0, 0, w, h);
		}

		public void Begin(DrawBatch batch)
		{
			tmpVp = batch.ViewportArray;
			batch.Flush();
			batch.Viewport(viewport);
			GL.BindFramebuffer(FramebufferTarget.Framebuffer, Id);
			GL.ClearColor(GLUtil.ToColor(GLDevice.Settings.ClearColor));
			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
		}

		public void End(DrawBatch batch)
		{
			batch.Flush();
			batch.Viewport(tmpVp);
			GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
		}

		public void Free()
		{
			GL.DeleteFramebuffer(Id);
			GL.DeleteTexture(((GLTexture) Src).Id);
		}

	}

}