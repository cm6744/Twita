using System.Collections.Generic;
using OpenTK.Graphics.OpenGL;
using Twita.Codec;
using Twita.Draw;
using Twita.Maths.Structs;

namespace Twita.Native.OpenGL
{

	public class GLDeviceSettings
	{

		public vec2 Size = new vec2(128, 128);
		
		public string Title = "Twita OpenGL";
		public bool Floating = false;
		public bool Resizable = true;
		public bool Decorated = true;
		public bool AutoIconify = true;
		public bool Maximized = false;
		public FileHandler Icon;
		public FileHandler Cursor;
		public vec2 Hotspot = new vec2(0, 0);

		public static int MipmapLevel = 0;
		public static TextureMagFilter FilterMag = TextureMagFilter.Nearest;
		public static TextureMinFilter FilterMin = TextureMinFilter.Nearest;
		public static TextureWrapMode Wrap = TextureWrapMode.Repeat;

		public vec4 ClearColor = new vec4(0, 0, 0, 1);

	}

}