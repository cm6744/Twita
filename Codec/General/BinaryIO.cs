using System;
using System.Collections.Generic;
using Twita.Common.Toolkit;

namespace Twita.Codec.General
{

	public class BinaryIO
	{

		const byte
			COMPOUND = 16,
			LIST = 64,
			BYTE = 128,
			INT = 129,
			LONG = 130,
			FLOAT = 131,
			DOUBLE = 132,
			BOOL = 133,
			STR = 134,
			BYTE_ARR = 200,
			INT_ARR = 201;

		public static byte GetId(object o)
		{
			switch(o)
			{
				case BinaryCompound:
					return COMPOUND;
				case BinaryList:
					return LIST;
				case byte:
					return BYTE;
				case int:
					return INT;
				case long:
					return LONG;
				case float:
					return FLOAT;
				case double:
					return DOUBLE;
				case bool:
					return BOOL;
				case string:
					return STR;
				case byte[]:
					return BYTE_ARR;
				case int[]:
					return INT_ARR;
			}

			return 0;
		}

		public static void Encode(BinaryCompound compound, ByteBuffer output)
		{
			foreach(KeyValuePair<string, object> pair in compound.Map)
			{
				string key = pair.Key;
				object val = pair.Value;

				output.WriteByte(GetId(val));
				output.WriteString(key);
				EncodePrimitive(val, output);
			}

			output.WriteByte(0); //Exit
		}

		public static void EncodePrimitive(object o, ByteBuffer output)
		{
			switch(o)
			{
				case BinaryCompound:
					Encode((BinaryCompound) o, output);
					break;
				case BinaryList:
					BinaryList lst = (BinaryList) o;
					output.WriteByte(lst.Type);
					output.WriteInt(lst.Count);
					foreach(object v in lst.Values)
					{
						EncodePrimitive(v, output);
					}

					break;
				case byte:
					output.WriteByte((byte) o);
					break;
				case int:
					output.WriteInt((int) o);
					break;
				case long:
					output.WriteLong((long) o);
					break;
				case float:
					output.WriteFloat((float) o);
					break;
				case double:
					output.WriteDouble((double) o);
					break;
				case bool:
					output.WriteBoolean((bool) o);
					break;
				case string:
					output.WriteString((string) o);
					break;
				case byte[]:
					byte[] bytes = (byte[]) o;
					output.WriteInt(bytes.Length);
					output.WriteBytes(bytes);
					break;
				case int[]:
					int[] ints = (int[]) o;
					output.WriteInt(ints.Length);
					foreach(int i in ints)
					{
						output.WriteInt(i);
					}
					break;
				default:
					Log.Warn($"{o} with the type: {o.GetType().Name} is not a primitive type in serialization!");
					break;
			}
		}

		public static BinaryCompound Decode(ByteBuffer input)
		{
			BinaryCompound compound = new BinaryCompound();

			while(true)
			{
				byte id = input.ReadByte();

				if(id == 0)
				{
					break;
				}

				string key = input.ReadString();
				object data = DecodePrimitive(input, id);
				compound.Set(key, data);
			}

			return compound;
		}

		public static object DecodePrimitive(ByteBuffer input, byte id)
		{
			switch(id)
			{
				case COMPOUND:
					return Decode(input);
				case LIST:
					BinaryList lst = new BinaryList();
					byte type = input.ReadByte();
					int size = input.ReadInt();

					if(type == 0 && size > 0)
					{
						throw new Exception("Find no type mark in BinaryList! Is the saving broken?");
					}

					for(int i = 0; i < size; i++)
					{
						object data = DecodePrimitive(input, type);
						lst.Add(data);
					}

					return lst;
				case BYTE:
					return input.ReadByte();
				case INT:
					return input.ReadInt();
				case LONG:
					return input.ReadLong();
				case FLOAT:
					return input.ReadFloat();
				case DOUBLE:
					return input.ReadDouble();
				case BOOL:
					return input.ReadBoolean();
				case STR:
					return input.ReadString();
				case BYTE_ARR:
					int len = input.ReadInt();
					byte[] bytes = new byte[len];
					input.ReadBytes(bytes, len);
					return bytes;
				case INT_ARR:
					len = input.ReadInt();
					int[] ints = new int[len];
					for(int i = 0; i < len; i++)
					{
						ints[i] = input.ReadInt();
					}
					return ints;
			}

			Log.Warn("Unknown type in binary found. Input data may be broken.");

			return null;
		}

		//Packed ops:

		public static void Write(BinaryCompound compound, FileHandler file)
		{
			if(!file.Exists()) file.Mkfile();

			ByteBuffer buffer = new ByteBuffer();
			Encode(compound, buffer);
			BytesIO.WriteGzip(file, buffer.Output());
		}

		public static BinaryCompound Read(FileHandler file)
		{
			if(!file.Exists())
			{
				Log.Warn($"Cannot find compound coded file at {file.Path}");
			}

			byte[] bytes = BytesIO.ReadGzip(file);
			ByteBuffer buffer = new ByteBuffer(bytes);
			return Decode(buffer);
		}

	}

}