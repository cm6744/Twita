using System;
using Twita.Common.Toolkit;

namespace Twita.Codec
{

	public class FileSystem
	{

		static string CurPath;

		private FileSystem()
		{
		}

		public static void AsApplicationSource()
		{
			CurPath = Environment.CurrentDirectory;
			//Log.Info($"FileSystem is opened at {CurPath}. Check if resources put correctly.");
		}

		//The test source is easier to manage.
		//When sharing a release version do not invoke this.
		public static void AsTestSource()
		{
			int idx = CurPath.LastIndexOf("bin", StringComparison.Ordinal);
			CurPath = CurPath.Substring(0, idx - 1) + "/.Run";//Get rid of the annoying '/', so - 1
			//Log.Info($"FileSystem is retargeted at {CurPath}. Do not contain \"Debug\" in your game path!");
		}

		public static FileHandler GetAbsolute(string path)
		{
			return new FileHandlerImpl(path);
		}

		public static FileHandler GetLocal(string path)
		{
			return new FileHandlerImpl(CurPath + "/" + path);
		}

	}

}