using System.IO;
using OpenTK.Graphics.OpenGL;
using StbImageSharp;
using Twita.Codec;
using Twita.Common.Manage;
using Texture = Twita.Draw.Texture;

namespace Twita.Native.OpenGL
{

	public class GLTexture : Texture
	{

		static GLTexture()
		{
			GL.Enable(EnableCap.Texture2D);
		}

		public int Id;
		public bool IsFB;

		public GLTexture(int id, int w, int h)
		{
			Id = id;
			Width = w;
			Height = h;
		}

		public GLTexture(FileHandler handler)
		{
			Id = GL.GenTexture();

			GL.ActiveTexture(TextureUnit.Texture0);
			GL.BindTexture(TextureTarget.Texture2D, Id);

			ImageResult result =
				ImageResult.FromMemory(File.ReadAllBytes(handler.Path), ColorComponents.RedGreenBlueAlpha);

			Width = result.Width;
			Height = result.Height;

			GL.TexImage2D(
				TextureTarget.Texture2D,
				0,
				PixelInternalFormat.Rgba,
				Width,
				Height,
				0,
				PixelFormat.Rgba,
				PixelType.UnsignedByte,
				result.Data
			);

			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter,
				(int) GLDeviceSettings.FilterMag);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter,
				(int) GLDeviceSettings.FilterMin);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS,
				(int) GLDeviceSettings.Wrap);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT,
				(int) GLDeviceSettings.Wrap);

			if(GLDeviceSettings.MipmapLevel > 0)
			{
				GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureBaseLevel, 0);
				GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMaxLevel,
					GLDeviceSettings.MipmapLevel);
				GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
			}

			Finalization.FREE.OnHoldReferred(() => GL.DeleteTexture(Id));
		}

	}

}