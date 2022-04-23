using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace GP
{
	class NPMManger : IManager
	{
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

			var jsonObject = JsonSerializer.Deserialize<JsonElement>(json,JsonHelper.Options).ToCollection();
			var meta = new Meta()
			{
				Nome = jsonObject.GetPropString("name"),
				Descricao = jsonObject.GetPropString("description"),
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