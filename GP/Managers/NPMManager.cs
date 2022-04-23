using System;
using System.Text.Json;
using System.IO;

namespace GP
{

	class NPMManger : IManager
	{
	
		private record NPMMeta (
			string name,
			string description
		);
		NPMManger()
		{

		}


		public static NPMManger Default => new NPMManger();

		static string pathMetaFile => "./package.json";

		public bool EhAmbiente(DirectoryInfo dir)
		{
			return dir.GetFile(pathMetaFile).Exists;
		}

		public Meta GetMeta(DirectoryInfo dir)
		{
			string json = dir.GetFile(pathMetaFile).ReadAllText();

			if(string.IsNullOrWhiteSpace(json))
				throw new MetadadosInvalidosException();
			NPMMeta npmMeta;
			try {
				npmMeta = JsonSerializer.Deserialize<NPMMeta>(json,JsonHelper.Options);
			}
			catch (Exception e) {
				throw new MetadadosInvalidosException(null, e);
			}
			var meta = new Meta()
			{
				Nome = npmMeta.name,
				Descricao = npmMeta.description,
				Linguagem = "js"
			};
			return meta;
			//JsonElement? scriptsOrNull = jsonObject.GetProp("scripts");
			//if( scriptsOrNull is JsonElement scripts)
			//	if(scripts.ValueKind == JsonValueKind.Object)
			//		if(scripts.ToCollection().GetProp("start") is not null)
			//			meta.ComandoRodar="npm start"
		}

	}

}