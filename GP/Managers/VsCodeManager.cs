using System;
using System.Linq;
using System.IO;
using System.Text.Json;

namespace GP
{
	class VsCodeManager : IManager
	{
		record MetaVsCode(
			(string path, string name)[] folders
		);
		VsCodeManager()
		{

		}

		public static VsCodeManager Default => new VsCodeManager();
		static string PatternMetaFile => "*.code-workspace";
		static FileInfo EncontrarArquivoMetadados(DirectoryInfo dir)
		{
			FileInfo[] files = dir.GetFiles(PatternMetaFile);
			if (files.Length > 1)
				throw new MutiplosMetadadosException();
			if (files.Length == 0)
				return null;
			return files[0];
		}
		public bool EhAmbiente(DirectoryInfo dir)
		{
			FileInfo[] metaFiles = dir.GetFiles(PatternMetaFile);
			return metaFiles.Length > 0;
		}
		public Meta GetMeta(DirectoryInfo dir)
		{
			FileInfo file = EncontrarArquivoMetadados(dir);
			if (file is null)
				return null;
			string json = file.ReadAllText();
			if (string.IsNullOrWhiteSpace(json))
				throw new MetadadosInvalidosException();
			MetaVsCode metaVsCode;
			try {
				metaVsCode = JsonSerializer.Deserialize<MetaVsCode>(json,JsonHelper.Options);
			}
			catch(Exception e){
				throw new MetadadosInvalidosException(null, e);
			}
			var SubProjetos = metaVsCode.folders?.Select((folder)=>
			{
				if (folder.path is null || Path.IsPathRooted(folder.path))
					throw new MetadadosInvalidosException();

				var SubProjDir = dir.GetDirectory(folder.path);
				
				if(!SubProjDir.Exists)
					throw new MetadadosInvalidosException();

				return new Meta()
				{
					Nome = folder.name ?? SubProjDir.Name,
					Caminho = SubProjDir.FullName
				};
			});
			var meta = new Meta()
			{
				Nome = Path.GetFileNameWithoutExtension(file.Name),
				ComandoAbrir = $"code {file.FullName}",
				Tipo = TipoAmbiente.Projeto,
				SubProjetos = SubProjetos.DefaultIfEmpty()?.ToArray()
			};
			return meta;
		}
	}

}