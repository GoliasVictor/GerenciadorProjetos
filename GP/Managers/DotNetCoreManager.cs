using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace GP
{
	class DotNetCoreManager : IManager
	{
		public static DotNetCoreManager Default => new DotNetCoreManager();
		DotNetCoreManager(){

		}
		public bool EhAmbiente(DirectoryInfo dir)
		{
			int projs = dir.GetFiles("*.csproj").Length;
			int slns = dir.GetFiles("*.sln").Length;
			return slns > 0 || projs > 0;
		}
		public Meta GetMeta(DirectoryInfo dir)
		{
			return GetMetaSolucao(dir) ?? GetMetaProjeto(dir);
		}
		public static FileInfo EncontrarArquivoMetadados(DirectoryInfo dir,string PadraoProcura){
			FileInfo[] files = dir.GetFiles(PadraoProcura);
			if(files.Length > 1)
				throw new MutiplosMetadadosException();
			if(files.Length == 0)
				return null;
			return files[0];
		}
		public static Meta GetMetaProjeto(DirectoryInfo dir){
			FileInfo file =  EncontrarArquivoMetadados(dir, "*.csproj");
			
			if(file is null)
				return null;
			var meta = new Meta(){
				Nome =  Path.GetFileNameWithoutExtension(file.Name),
				Tipo =  TipoAmbiente.Projeto,
				Linguagem = ".Net"
			};
			return meta;

		}
		public static Meta GetMetaSolucao(DirectoryInfo dir){
			FileInfo file =  EncontrarArquivoMetadados(dir, "*.sln");
			if(file is null)
				return null;

			var SubProjetos =  new List<Meta>();
	
			foreach (var subDir in dir.GetDirectories())
			{
				Meta subProjeto = GetMetaProjeto(subDir);
				if(subProjeto is not null){
					 subProjeto.Caminho = Path.GetRelativePath(dir.FullName,subDir.FullName) ;
					SubProjetos.Add(subProjeto);
				}
			}

			var meta = new Meta(){
				Nome =  Path.GetFileNameWithoutExtension(file.Name),
				Tipo =  TipoAmbiente.Projeto,
				Linguagem = ".Net",
  				SubProjetos = SubProjetos.ToArray()
			};

			return meta;
		}


	}
}