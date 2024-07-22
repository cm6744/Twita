using System;

namespace Twita.Codec
{

	public struct ByteBufferOutChunk
	{

		public int Offset, Len;
		public byte[] Bytes;

		public ByteBufferOutChunk(byte[] bytes, int o, int l)
		{
			Offset = o;
			Len = l;
			Bytes = bytes;
		}

		public byte[] GetPartialArray()
		{
			byte[] bytes = new byte[Len];
			Array.Copy(Bytes, 0, bytes, 0, bytes.Length);
			return bytes;
		}

	}

}