using System.Text;

namespace Twita.Codec
{

	public delegate T Decode<out T>(BinaryCompound compound);
	public delegate void Encode<in T>(T t, BinaryCompound compound);

	public class Codec<T>
	{

		private Decode<T> decode;
		private Encode<T> encode;

		public Codec(Decode<T> decode, Encode<T> encode)
		{
			this.decode = decode;
			this.encode = encode;
		}

		public T Decode(BinaryCompound compound)
		{
			return decode.Invoke(compound);
		}

		public void Encode(T t, BinaryCompound compound)
		{
			encode.Invoke(t, compound);
		}

		public T Decode(BinaryCompound compound, string obj)
		{
			return Decode(compound.Get<BinaryCompound>(obj));
		}

		public void Encode(T t, BinaryCompound compound, string obj)
		{
			BinaryCompound c1 = new BinaryCompound();
			Encode(t, c1);
			compound.Set(obj, compound);
		}

	}

}