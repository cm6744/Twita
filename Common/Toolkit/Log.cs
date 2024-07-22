using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading;
using Twita.Codec;
using Twita.Common.Manage;

namespace Twita.Common.Toolkit
{

	public class Log
	{

		const string
			DEBUG = "DEBUG",
			INFO = "INFO",
			WARN = "WARN",
			FATAL = "FATAL",
			STACKTRACE = "STACKTRACE";

		private static StreamWriter outw;

		public static void StartStreamWriting(FileHandler file)
		{
			if(!file.Exists())
			{
				file.Mkfile();
			}
			else
			{
				file.Delete();
				file.Mkfile();//Reflush
			}
			outw = new StreamWriter(file.Path, true, Encoding.UTF8);
		}

		public static void TryEndStreamWriting()
		{
			outw.Close();
			outw = null;
		}

		static void Print(string level, string info)
		{
			string className = "-";

			StackTrace stacks = new StackTrace(1);
			StackFrame frame = stacks.GetFrame(1);

			if(frame != null)
			{
				MethodBase method = frame.GetMethod();

				if(method != null && method.ReflectedType != null)
				{
					className = method.ReflectedType.Name;

					if(className.StartsWith('<')) className = "-";
				}
			}

			string threadName = Thread.CurrentThread.Name;

			if(string.IsNullOrWhiteSpace(threadName))
			{
				threadName = "-";
			}

			string time = DateTime.Now.ToString("u");

			string outs = $"[{level}] [{threadName}] [{className}] [{time}] {info}";

			switch(level)
			{
				case DEBUG:
					Console.ForegroundColor = ConsoleColor.DarkGray;
					break;
				case INFO:
					Console.ForegroundColor = ConsoleColor.White;
					break;
				case WARN:
					Console.ForegroundColor = ConsoleColor.DarkRed;
					break;
				case FATAL:
					Console.ForegroundColor = ConsoleColor.Red;
					break;
				case STACKTRACE:
					Console.ForegroundColor = ConsoleColor.Yellow;
					break;
			}
			Console.WriteLine(outs);

			if(outw != null)
			{
				outw.WriteLine(outs);
			}
		}

		public static void Debug(string info)
		{
			Print(DEBUG, info);
		}

		public static void Info(string info)
		{
			Print(INFO, info);
		}

		public static void Warn(string info)
		{
			Print(WARN, info);
		}

		public static void Fatal(string info)
		{
			Print(FATAL, info);
		}

		public static void Warn(Exception exc)
		{
			Print(WARN, exc.Message);
			Print(STACKTRACE, exc.StackTrace);
		}

		public static void Fatal(Exception exc)
		{
			Print(FATAL, exc.Message);
			Print(STACKTRACE, exc.StackTrace);
		}

	}

}