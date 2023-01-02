using System.IO;

namespace GP
{
	static class SystemIOExntensions
	{ 
		public static string ReadAllText(this FileInfo file)
		{
			return File.ReadAllText(file.FullName);
		}
		public static FileInfo GetFile(this DirectoryInfo dir, string pathToFile)
		{
			return new FileInfo(Path.GetFullPath(Path.Combine(dir.FullName, pathToFile)));
		}
		public static DirectoryInfo GetDirectory(this DirectoryInfo dir, string pathToDirectory)
		{
			return new DirectoryInfo(Path.GetFullPath(Path.Combine(dir.FullName, pathToDirectory)));
		}
	}
}