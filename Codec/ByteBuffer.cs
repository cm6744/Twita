using System;
using System.Collections.Generic;

namespace Twita.Codec
{

	public abstract class LowByteBuffer
	{

		public byte[] Buf;
		protected int ReadIndex;
		protected int WriteIndex;
		protected int MarkReadIndex;
		protected int MarkWirteIndex;
		public int Capacity;

		public int ReadableBytes => WriteIndex - ReadIndex;

		protected LowByteBuffer(int cap)
		{
			Buf = new byte[cap];
			Capacity = cap;
			ReadIndex = 0;
			WriteIndex = 0;
			MarkReadIndex = MarkWirteIndex = 0;
		}

		protected LowByteBuffer(byte[] bytes)
		{
			Buf = new byte[bytes.Length];
			Array.Copy(bytes, 0, Buf, 0, Buf.Length);
			Capacity = Buf.Length;
			ReadIndex = 0;
			WriteIndex = bytes.Length + 1;
			MarkReadIndex = MarkWirteIndex = 0;
		}

		protected static int GetPower2Len(int value)
		{
			if(value == 0)
			{
				return 1;
			}

			value--;
			value |= value >> 1;
			value |= value >> 2;
			value |= value >> 4;
			value |= value >> 8;
			value |= value >> 16;
			return value + 1;
		}

		protected static byte[] Flip(byte[] bytes)
		{
			if(BitConverter.IsLittleEndian)
			{
				Array.Reverse(bytes);
			}

			return bytes;
		}

		protected int FixSizeAndReset(int currLen, int futureLen)
		{
			if(futureLen > currLen)
			{
				int size = GetPower2Len(currLen) * 2;
				if(futureLen > size)
				{
					size = GetPower2Len(futureLen) * 2;
				}

				byte[] newbuf = new byte[size];
				Array.Copy(Buf, 0, newbuf, 0, currLen);
				Buf = newbuf;
				Capacity = size;
			}

			return futureLen;
		}

		public void WriteBytes(byte[] bytes, int startIndex, int length)
		{
			int offset = length - startIndex;
			if(offset <= 0) return;
			int total = offset + WriteIndex;
			int len = Buf.Length;
			FixSizeAndReset(len, total);
			for(int i = WriteIndex, j = startIndex; i < total; i++, j++)
			{
				Buf[i] = bytes[j];
			}

			WriteIndex = total;
		}

		public void MarkReaderIndex()
		{
			MarkReadIndex = ReadIndex;
		}

		public void MarkWriterIndex()
		{
			MarkWirteIndex = WriteIndex;
		}

		public void ResetReaderIndex()
		{
			ReadIndex = MarkReadIndex;
		}

		public void ResetWriterIndex()
		{
			WriteIndex = MarkWirteIndex;
		}

		public ByteBufferOutChunk Output()
		{
			return new ByteBufferOutChunk(Buf, 0, WriteIndex);
		}

		public void Clear()
		{
			for(int i = 0; i < Buf.Length; i++)
			{
				Buf[i] = 0;
			}

			ReadIndex = 0;
			WriteIndex = 0;
			MarkReadIndex = 0;
			MarkWirteIndex = 0;
			Capacity = Buf.Length;
		}

		public void Dispose()
		{
			ReadIndex = 0;
			WriteIndex = 0;
			MarkReadIndex = 0;
			MarkWirteIndex = 0;
			Capacity = 0;
			Buf = null;
		}

		public byte ReadByte()
		{
			byte b = Buf[ReadIndex];
			ReadIndex++;
			return b;
		}

		public const int TempSize = 64;

		//The Temp arrays for #Get.
		private static List<byte[]> TmpBytes = new List<byte[]>();

		static LowByteBuffer()
		{
			for(int i = 0; i <= TempSize; i++)
			{
				TmpBytes.Add(new byte[i]);
			}
		}

		private byte[] Get(int index, int len)
		{
			byte[] bytes = len <= TempSize ? TmpBytes[len] : new byte[len];
			Array.Copy(Buf, index, bytes, 0, len);
			return Flip(bytes);
		}

		protected byte[] Read(int len)
		{
			byte[] bytes = Get(ReadIndex, len);
			ReadIndex += len;
			return bytes;
		}

	}

	public class ByteBuffer : LowByteBuffer
	{

		//Write

		public void WriteBytes(byte[] bytes, int length)
		{
			WriteBytes(bytes, 0, length);
		}

		public void WriteBytes(byte[] bytes)
		{
			WriteBytes(bytes, bytes.Length);
		}

		public void Write(ByteBuffer buffer)
		{
			if(buffer.ReadableBytes <= 0) return;
			ByteBufferOutChunk chk = buffer.Output();
			WriteBytes(chk.Bytes, chk.Offset, chk.Len);
		}

		public void WriteShort(short value)
		{
			WriteBytes(Flip(BitConverter.GetBytes(value)));
		}

		public void WriteInt(int value)
		{
			WriteBytes(Flip(BitConverter.GetBytes(value)));
		}

		public void WriteLong(long value)
		{
			WriteBytes(Flip(BitConverter.GetBytes(value)));
		}

		public void WriteFloat(float value)
		{
			WriteBytes(Flip(BitConverter.GetBytes(value)));
		}

		public void WriteByte(byte value)
		{
			int afterLen = WriteIndex + 1;
			int len = Buf.Length;
			FixSizeAndReset(len, afterLen);
			Buf[WriteIndex] = value;
			WriteIndex = afterLen;
		}

		public void WriteByte(int value)
		{
			byte b = (byte) value;
			WriteByte(b);
		}

		public void WriteDouble(double value)
		{
			WriteBytes(Flip(BitConverter.GetBytes(value)));
		}

		public void WriteChar(char value)
		{
			WriteBytes(Flip(BitConverter.GetBytes(value)));
		}

		public void WriteString(string value)
		{
			WriteInt(value.Length);

			for(int i = 0; i < value.Length; i++)
			{
				WriteChar(value[i]);
			}
		}

		public void WriteBoolean(bool value)
		{
			WriteBytes(Flip(BitConverter.GetBytes(value)));
		}

		//Read

		public short ReadShort()
		{
			return BitConverter.ToInt16(Read(2), 0);
		}

		public int ReadInt()
		{
			return BitConverter.ToInt32(Read(4), 0);
		}

		public long ReadLong()
		{
			return BitConverter.ToInt64(Read(8), 0);
		}

		public float ReadFloat()
		{
			return BitConverter.ToSingle(Read(4), 0);
		}

		public double ReadDouble()
		{
			return BitConverter.ToDouble(Read(8), 0);
		}

		public char ReadChar()
		{
			return BitConverter.ToChar(Read(2), 0);
		}

		public bool ReadBoolean()
		{
			return BitConverter.ToBoolean(Read(1), 0);
		}

		public string ReadString()
		{
			int len = ReadInt();
			char[] chars = new char[len];

			for(int i = 0; i < len; i++)
			{
				chars[i] = ReadChar();
			}

			return new string(chars);
		}

		public void ReadBytes(byte[] bytes, int len)
		{
			for(int i = 0; i < len; i++)
			{
				bytes[i] = ReadByte();
			}
		}

		//Constructors

		public static int DefaultCapacity = 64;

		public ByteBuffer() : base(DefaultCapacity)
		{
		}

		public ByteBuffer(int cap) : base(cap)
		{
		}

		public ByteBuffer(byte[] bytes) : base(bytes)
		{
		}

	}

}