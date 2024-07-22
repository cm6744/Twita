using System;
using OpenTK.Windowing.GraphicsLibraryFramework;
using Twita.Draw;
using Twita.Maths.Structs;

namespace Twita.Native.OpenGL
{

	public class GLGraphicEnvironment : GraphicEnvironment
	{

		public unsafe vec2 Size
		{
			get
			{
				GLFW.GetWindowSize(GLDevice.Window, out int x, out int y);
				return new vec2(x, y);
			}
			set => GLFW.SetWindowSize(GLDevice.Window, (int) value.x, (int) value.y);
		}

		public unsafe vec2 Pos 
		{
			get
			{
				GLFW.GetWindowPos(GLDevice.Window, out int x, out int y);
				return new vec2(x, y);
			}
			set => GLFW.SetWindowPos(GLDevice.Window, (int) value.x, (int) value.y);
		}

		public unsafe bool Decorated
		{
			set => GLFW.SetWindowAttrib(GLDevice.Window, WindowAttribute.Decorated, value);
		}

		public double Nanotime => (GLFW.GetTime() * 1000_000_000);

		public unsafe string Title 
		{ 
			set
			{
				GLFW.SetWindowTitle(GLDevice.Window, value);
			}
		}

		public unsafe void Swap()
		{
			GLFW.SwapBuffers(GLDevice.Window);
		}

		public void Prepare()
		{
			//Nothing to do in opengl.
		}

	}

}