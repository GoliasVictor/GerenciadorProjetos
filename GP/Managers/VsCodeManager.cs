using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text.Json;

namespace GP
{
	class VsCodeManager : IManager
	{
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

			var jsonObject = JsonSerializer.Deserialize<JsonElement>(json,JsonHelper.Options).ToCollection();

			List<Meta> SubProjetos = new List<Meta>();
			JsonElement? foldersOrNull = jsonObject.GetProp("folders");
			if (foldersOrNull is JsonElement folders)
			{
				if (folders.ValueKind != JsonValueKind.Array)
					throw new MetadadosInvalidosException();

				foreach (var folderElement in folders.EnumerateArray())
				{
					if (folderElement.ValueKind != JsonValueKind.Object)
						throw new MetadadosInvalidosException();

					var folder = folderElement.ToCollection();
					string strPath = folder.GetPropString("path");
					if (strPath is null || Path.IsPathRooted(strPath))
						throw new MetadadosInvalidosException();

					var SubProjDir = dir.GetDirectory(strPath);

					if(!SubProjDir.Exists)
						throw new MetadadosInvalidosException();

					string nome = folder.GetPropString("name");
					SubProjetos.Add(new Meta()
					{
						Nome = nome ?? SubProjDir.Name,
						Caminho = SubProjDir.FullName
					});
				}
			}
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