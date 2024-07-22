using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Twita.Common.Toolkit
{

	public static class Strings
	{

		public static int IndexOf(string str, string find)
		{
			return str.IndexOf(find) + find.Length - 1;
		}

		public static int IndexOfLast(string str, string find)
		{
			return str.LastIndexOf(find);
		}

		public static int IndexOf(string str, char find)
		{
			return str.IndexOf(find);
		}

		public static int IndexOfLast(string str, char find)
		{
			return str.LastIndexOf(find);
		}

		public static string SubString(string str, int a, int b)
		{
			return str.Substring(a + 1, b - a - 1);
		}

		public static string NextString(string str, int a)
		{
			return str.Substring(a + 1);
		}

		public static string LastString(string str, int a)
		{
			return str.Substring(0, a);
		}

		public static string SubString(string str, string spikeA, string spikeB)
		{
			int a = IndexOf(str, spikeA);
			int b = IndexOf(str, spikeB);
			return SubString(str, a, b);
		}

		public static string SubString2(string str, string spikeA, string spikeB)
		{
			int a = IndexOf(str, spikeA);
			int b = IndexOfLast(str, spikeB);
			return SubString(str, a, b);
		}

		public static string NextString(string str, string spike)
		{
			return NextString(str, IndexOf(str, spike));
		}

		public static string LastString(string str, string spike)
		{
			return LastString(str, IndexOf(str, spike));
		}

		public static string SubString(string str, char spikeA, char spikeB)
		{
			int a = IndexOf(str, spikeA);
			int b = IndexOf(str, spikeB);
			return SubString(str, a, b);
		}

		public static string SubString2(string str, char spikeA, char spikeB)
		{
			int a = IndexOf(str, spikeA);
			int b = IndexOfLast(str, spikeB);
			return SubString(str, a, b);
		}

		public static string NextString(string str, char spike)
		{
			return NextString(str, IndexOf(str, spike));
		}

		public static string LastString(string str, char spike)
		{
			return LastString(str, IndexOf(str, spike));
		}

		public static string Compress(string str)
		{
			return str.Replace(" ", "");
		}

		public static string[] CutBy(string str, string cut)
		{
			return str.Split(cut);
		}

	}

}
