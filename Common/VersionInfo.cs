using System;

namespace Twita.Common
{

	public class VersionInfo : IComparable<VersionInfo>
	{

		public string FullName;
		public string Prefix;
		public bool Snapshot;
		public bool Stable;
		public int Iteration;

		public VersionInfo(string code, int iteration)
		{
			FullName = code;
			Prefix = code[code.LastIndexOf('-')..];
			Snapshot = Prefix.Contains("snapshot", StringComparison.OrdinalIgnoreCase);
			Stable = !Snapshot;

			Iteration = iteration;
		}

		public int CompareTo(VersionInfo other)
		{
			if(other.Iteration < Iteration) return 1;
			if(other.Iteration == Iteration) return 0;
			return -1;
		}

	}

}