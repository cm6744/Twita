using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twita.Codec;
using Twita.Common.Resource;

namespace Twita.Draw
{

	public class Atlas
	{

		Ref<Texture> Src;
		BinaryCompound Data;

		public Atlas(Texture src, BinaryCompound data)
		{
			Src = new(src);
			Data = data;
		}

		public Atlas(Ref<Texture> src, BinaryCompound data)
		{
			Src = src;
			Data = data;
		}

		public Icon GetSprite(string key)
		{
			BinaryCompound c1 = Data.Get<BinaryCompound>(key);
			if(c1 == null)
			{
				return new IconNoBehavior();
			}
			BinaryList arr = c1.Get<BinaryList>("Location");
			switch(c1.Get<string>("Coordinate"))
			{
				case "Vertex":
					return TexturePart.ByVerts(Src.Value, 
					(int) arr[0], (int) arr[1], (int) arr[2], (int) arr[3]);
				default:
				case "Size":
					return TexturePart.BySize(Src.Value, 
					(int) arr[0], (int) arr[1], (int) arr[2], (int) arr[3]);
				case "Float-Size":
					return TexturePart.ByPercentSize(Src.Value, 
					(float) arr[0], (float) arr[1], (float) arr[2], (float) arr[3]);
				case "Float-Vertex":
					return TexturePart.ByPercentVerts(Src.Value, 
					(float) arr[0], (float) arr[1], (float) arr[2], (float) arr[3]);
			}
		}

	}

}
