using System;
using System.Text;
using Twita.Codec;

namespace Twita.Draw
{

	public delegate int LocatePage(char ch);

	public class Font
	{

		public static string ASCII;

		static Font()
		{
			StringBuilder builder = new StringBuilder();
			for(int i = 32; i < 256 && i != 127; i++)
			{
				builder.Append((char) i);
			}
			ASCII = builder.ToString();
		}

		public static Font load(Texture[] textures, LocatePage locator, BinaryCompound compound)
		{
			Font font = new Font();
			font.texture = textures;
			font.Locate = locator;
			font.GlyphX = compound.Get<int[]>("ArrayX");
			font.GlyphY = compound.Get<int[]>("ArrayY");
			font.GlyphWidth = compound.Get<int[]>("ArrayWidth");
			font.YSize = compound.Get<int>("Height");

			return font;
		}

		public int[] GlyphWidth = new int[65536];
		public int[] GlyphX = new int[65536];
		public int[] GlyphY = new int[65536];

		public Texture[] texture;
		public int YSize;

		public LocatePage Locate;
		public float Scale = 1f;

		public GlyphBounds GetBounds(string text, float maxWidth)
		{
			if(string.IsNullOrWhiteSpace(text))
			{
				return new GlyphBounds("", 0, 16);
			}

			int width = 0;
			int lineWidth = 0;
			int height = 0;
			int lineHeight = (int) (YSize * Scale);
			bool needNewLine = false;

			for(int i = 0; i < text.Length; i++)
			{
				char c = text[i];

				if(c == '\n' || needNewLine)
				{
					height += lineHeight;
					width = Math.Max(lineWidth, width);
					lineWidth = 0;
					needNewLine = false;
					continue;
				}

				if(lineWidth + GlyphWidth[c] * Scale >= maxWidth)
				{
					needNewLine = true;
					i -= 2; //correct index
					continue;
				}

				lineWidth += (int) (GlyphWidth[c] * Scale);
			}

			height += lineHeight;
			width = Math.Max(lineWidth, width);

			return new GlyphBounds(text, width, height);
		}

		public GlyphBounds GetBounds(string text)
		{
			return GetBounds(text, int.MaxValue);
		}

	}

	public struct GlyphBounds
	{

		public string Sequence;
		public int Width;
		public int Height;

		public GlyphBounds(string sequence, int w, int h)
		{
			Sequence = sequence;
			Width = w;
			Height = h;
		}

	}

	public enum Align
	{

		LEFT,
		CENTER,
		RIGHT

	}

}