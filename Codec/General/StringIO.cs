using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Twita.Codec.General
{

	public delegate bool LineLimiter(string line);

	public class StringIO
	{

		public static string GetCompression(ICollection<string> strs, LineLimiter limiter)
		{
			StringBuilder sb = new StringBuilder();

			foreach(string s in strs)
			{
				if(!limiter.Invoke(s))
				{
					continue;
				}

				sb.Append(s);
				sb.AppendLine();
			}

			return sb.ToString();
		}

		public static string[] ReadArray(FileHandler file, LineLimiter limiter = null)
		{
			List<string> list = new List<string>();

			using(StreamReader r = new StreamReader(file.Path))
			{
				string line;

				while((line = r.ReadLine()) != null)
				{
					if(limiter != null && !limiter.Invoke(line)) continue;
					list.Add(line);
				}
			}

			return list.ToArray();
		}

		public static string Read(FileHandler file)
		{
			return File.ReadAllText(file.Path);
		}

		public static void WriteArray(FileHandler file, string[] arr)
		{
			using(StreamWriter r = new StreamWriter(file.Path))
			{
				foreach(string s in arr)
				{
					r.WriteLine(s);
				}
			}
		}

		public static void Write(FileHandler file, string s)
		{
			File.WriteAllText(file.Path, s);
		}

	}

}