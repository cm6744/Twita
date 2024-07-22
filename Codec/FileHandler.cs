using System.Collections.Generic;
using System.IO;

namespace Twita.Codec
{

	//Control files and folders without "/" contained.
	//You should consider the file type when using because it has no handler for error.
	public interface FileHandler
	{

		public string Path { get; }

		bool IsDirectory();

		bool IsFile();

		string Format();

		string Name();

		FileHandler Goto(string name);

		FileHandler Exit();

		FileHandler[] Directories();

		FileHandler[] Files();

		bool Exists();

		void Mkdirs();

		void Mkfile();

		void Delete();

	}

	public class FileHandlerImpl : FileHandler
	{

		readonly string File;
		readonly bool Doc, Fol;

		public FileHandlerImpl(string file)
		{
			File = file.Replace('\\', '/');

			if(File.EndsWith('/'))
			{
				File = File.Substring(0, File.Length - 1);
			}

			Doc = System.IO.File.Exists(file);
			Fol = Directory.Exists(file);
		}

		public void Delete()
		{
			if(Doc)
			{
				System.IO.File.Delete(File);
			}
			else if(Fol)
			{
				Directory.Delete(File, true);
			}
		}

		public FileHandler[] Files()
		{
			List<FileHandler> files = new List<FileHandler>();

			foreach(FileInfo info in new DirectoryInfo(File).GetFiles())
			{
				files.Add(new FileHandlerImpl(info.FullName));
			}

			return files.ToArray();
		}

		public FileHandler Goto(string name)
		{
			string call;

			if(File.EndsWith("/"))
			{
				call = File + name;
			}
			else
			{
				call = File + "/" + name;
			}

			return new FileHandlerImpl(call);
		}

		public bool Exists()
		{
			return Doc || Fol;
		}

		public FileHandler Exit()
		{
			int idx = File.LastIndexOf('/');
			string call = File.Substring(0, idx);
			return new FileHandlerImpl(call);
		}

		public FileHandler[] Directories()
		{
			List<FileHandler> files = new List<FileHandler>();

			foreach(DirectoryInfo info in new DirectoryInfo(File).GetDirectories())
			{
				files.Add(new FileHandlerImpl(info.FullName));
			}

			return files.ToArray();
		}

		public string Format()
		{
			int idx = File.LastIndexOf('.');
			return File.Substring(idx + 1);
		}

		public bool IsFile()
		{
			return Doc;
		}

		public string Path => File;

		public bool IsDirectory()
		{
			return Fol;
		}

		public void Mkdirs()
		{
			DirectoryInfo info = new DirectoryInfo(File);
			if(!info.Exists)
			{
				info.Create();
			}
		}

		public void Mkfile()
		{
			Exit().Mkdirs();
			FileInfo info = new FileInfo(File);
			if(!info.Exists)
			{
				info.Create().Close();
			}
		}

		public string Name()
		{
			int idx = File.LastIndexOf('.');
			int idx1 = File.LastIndexOf('/') + 1;
			return File.Substring(idx1, idx - idx1);
		}

	}

}