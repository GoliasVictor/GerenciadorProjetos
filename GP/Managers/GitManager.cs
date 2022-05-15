using System;
using System.Text.Json;
using System.IO;
using System.Threading.Tasks;

namespace GP
{

	class GitManger : IManager
	{
	
		GitManger()
		{

		}


		public static GitManger Default => new GitManger();

		static string pathDotGit(DirectoryInfo dir) => dir.GetFile(".git").FullName;
		public string RodarGit(DirectoryInfo dir, string args){
			return Terminal.Rodar("/usr/bin/git",$"--git-dir={pathDotGit(dir)} {args}").GetAwaiter().GetResult();
		}
		public  bool EhAmbiente(DirectoryInfo dir)
		{
			string Result = RodarGit(dir,"rev-parse --is-inside-work-tree");
			return Result.Trim() == "true";
		}

		public Meta GetMeta(DirectoryInfo dir)
		{
			string nome= null;
			string url = RodarGit(dir,$"config --get remote.origin.url ");
			if(!string.IsNullOrWhiteSpace(url))
				nome = Path.GetFileNameWithoutExtension(url);
			else
				nome=dir.Name;
			if(string.IsNullOrWhiteSpace(nome))
				throw new MetadadosInvalidosException("Sem Nome");

			var meta = new Meta()
			{
				Nome = nome,
			};
			return meta;
		}

	}

}