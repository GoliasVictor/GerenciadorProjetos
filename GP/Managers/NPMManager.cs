using System;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.IO;
using System.Linq;

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
			var meta = new Meta(){ 
				Linguagem = "js"
			};
			try {
				var jmeta = JsonSerializer.Deserialize<JsonObject>(json,JsonHelper.Options);
				meta.Nome = (string)jmeta["name"];
				meta.Descricao =  (string)jmeta["description"];
				var jnScripts = jmeta["scripts"]; 
				if( jnScripts is JsonObject jScripts){
					meta.Scripts = jScripts.ToDictionary(
						jScript => jScript.Key, 
						jScript => (string)jScript.Value 
					);
				}
			}
			catch (Exception e) {
				throw new MetadadosInvalidosException(null, e);
			}
		
			return meta;
		}

	}

}