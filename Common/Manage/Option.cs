using System.Collections.Generic;
using Twita.Codec;
using Twita.Codec.General;

namespace Twita.Common.Manage
{

	public class Options
	{

		public readonly List<Option> Keys = new List<Option>();
		public BinaryCompound Values = new BinaryCompound();

		public void Join(Option option)
		{
			Keys.Add(option);
			option.Group = this;
		}

		public void WriteToLocal(FileHandler file)
		{
			BinaryIO.Write(Values, file);
		}

		public void ReadFromLocal(FileHandler file)
		{
			if(!file.Exists()) return;

			Values = BinaryIO.Read(file);
		}

	}

	public class Option
	{

		public string Key;
		public Options Group;
		public object Defval;

		public Option(string key, object defval)
		{
			Key = key;
			Defval = defval;
		}

		public void Set(object o)
		{
			Group.Values.Set(Key, o);
		}

		public T Get<T>()
		{
			if(Group.Values.Try<T>(Key, out T val))
			{
				return val;
			}
			return (T) Defval;
		}

	}

}