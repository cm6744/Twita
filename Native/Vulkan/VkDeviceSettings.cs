using Twita.Maths.Structs;

namespace Twita.Native.Vulkan
{

	public class VkDeviceSettings
	{

		public vec2 Size = new vec2(128, 128);
		
		public string Title = "Twita Vulkan";
		public bool Floating = false;
		public bool Resizable = true;
		public bool Decorated = true;
		public bool AutoIconify = true;
		public bool Maximized = false;
		//public Texture Icon;
		//public Texture Cursor;
		public vec2 Hotspot = new vec2(0, 0);

		public vec4 ClearColor = new vec4(0, 0, 0, 1);

	}

}