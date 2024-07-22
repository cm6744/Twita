namespace Twita.Draw
{

	public interface FontCarver
	{

		public void Draw(DrawBatch batch, Font font, string text, float x, float y, float maxw);

	}

	public class DefaultFontCarver : FontCarver
	{

		public void Draw(DrawBatch batch, Font font, string text, float x, float y, float maxw)
		{
			if(string.IsNullOrWhiteSpace(text))
			{
				return;
			}

			int fontHeight = (int) (font.YSize * font.Scale);

			float drawX = x;
			float drawY = y;

			bool newLine = false;

			byte nextType = 0;

			for(int i = 0; i < text.Length; i++)
			{
				char ch = text[i];

				if(ch == '\n' || newLine)
				{
					drawY -= fontHeight;
					drawX = x;
					newLine = false;
					continue;
				}

				int w = (int) (font.GlyphWidth[ch] * font.Scale);

				if(drawX - x + w >= maxw)
				{
					newLine = true;
					i -= (int) (2 * font.Scale);
					continue;
				}

				Texture map = font.texture[font.Locate(ch)];

				batch.Draw(map, drawX, drawY, w, fontHeight, font.GlyphX[ch], font.GlyphY[ch], font.GlyphWidth[ch], font.YSize);
				drawX += w;
			}
		}

	}

}