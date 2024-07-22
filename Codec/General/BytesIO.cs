using System;
using System.IO;
using System.IO.Compression;

namespace Twita.Codec.General
{

	public class BytesIO
	{

		public static void Write(FileHandler handler, byte[] data)
		{
			using FileStream stream = new FileStream(handler.Path, FileMode.OpenOrCreate);
			using BinaryWriter bw = new BinaryWriter(stream);

			bw.Write(data, 0, data.Length);
		}

		public static void Write(FileHandler handler, ByteBufferOutChunk data)
		{
			using FileStream stream = new FileStream(handler.Path, FileMode.OpenOrCreate);
			using BinaryWriter bw = new BinaryWriter(stream);

			bw.Write(data.Bytes, data.Offset, data.Len);
		}

		public static byte[] Read(FileHandler handler)
		{
			using FileStream stream = new FileStream(handler.Path, FileMode.Open);
			using BinaryReader br = new BinaryReader(stream);

			byte[] bytes = new byte[stream.Length];
			br.Read(bytes, 0, bytes.Length);
			return bytes;
		}

		public static void WriteGzip(FileHandler handler, byte[] data)
		{
			using FileStream stream = new FileStream(handler.Path, FileMode.OpenOrCreate);
			using MemoryStream src = new MemoryStream(data);
			using GZipStream comp = new GZipStream(stream, CompressionMode.Compress);

			src.CopyTo(comp);
		}

		public static void WriteGzip(FileHandler handler, ByteBufferOutChunk data)
		{
			using FileStream stream = new FileStream(handler.Path, FileMode.OpenOrCreate);
			using MemoryStream src = new MemoryStream(data.Bytes, data.Offset, data.Len);
			using GZipStream comp = new GZipStream(stream, CompressionMode.Compress);

			src.CopyTo(comp);
		}

		public static byte[] ReadGzipOrFlat(FileHandler handler)
		{
			try
			{
				return ReadGzip(handler);
			}
			catch(Exception)
			{
				return Read(handler);
			}
		}

		public static byte[] ReadGzip(FileHandler handler)
		{
			using FileStream stream = new FileStream(handler.Path, FileMode.Open);
			using MemoryStream outp = new MemoryStream();
			using GZipStream decomp = new GZipStream(stream, CompressionMode.Decompress);

			decomp.CopyTo(outp);
			return outp.ToArray();
		}

	}

}