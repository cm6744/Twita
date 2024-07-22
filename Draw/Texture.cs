namespace Twita.Draw
{

	public interface Icon
	{

		void Draw(DrawBatch batch, float x, float y, float w, float h);

	}

	public interface DimesionalIcon : Icon
	{

		public int Width { get; } 
		public int Height { get; }

	}

	public struct IconNoBehavior : Icon
	{

		public void Draw(DrawBatch batch, float x, float y, float w, float h)
		{
		}

	}

	public abstract class Texture : DimesionalIcon
	{

		public int Width { get; set; }
		public int Height { get; set; }

		public void Draw(DrawBatch batch, float x, float y, float w, float h)
		{
			batch.Draw(this, x, y, w, h);
		}

	}

}