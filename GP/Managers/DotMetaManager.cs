using System;
using System.Text.Json;
using System.IO;
using System.Text.Json.Nodes;
using System.Linq;
using System.Collections.Generic;

namespace GP
{
	class DotMetaManager : IManager
	{
		DotMetaManager()
		{

		}

		public static DotMetaManager Default => new DotMetaManager();

		protected static string pathMetaFile => "./.meta";
		public static FileInfo FileMetadados(DirectoryInfo dir){
			return dir.GetFile(pathMetaFile);
		}
		public static bool EhAmbiente(DirectoryInfo dir)
		{
			return dir.GetFile(pathMetaFile).Exists;
		}
		bool IManager.EhAmbiente(DirectoryInfo dir) => EhAmbiente(dir);
		
		public static Meta GetMeta(DirectoryInfo dir)
		{
			var meta = JsonToMeta(dir.GetFile(pathMetaFile).ReadAllText());
			return meta;
		}
		Meta IManager.GetMeta(DirectoryInfo dir) => GetMeta(dir);

		public static string MetaToJson(Meta meta){
			return JsonSerializer.Serialize<Meta>(meta,JsonHelper.Options);
		}
		public static Meta MetaFromJson(JsonObject jmeta){
			var meta =  new Meta();
			meta.Nome		  = (string)jmeta[nameof(Meta.Nome)];
			meta.Descricao	  = (string)jmeta[nameof(Meta.Descricao)];
			meta.Linguagem	  = (string)jmeta[nameof(Meta.Linguagem)];
			meta.ComandoAbrir = (string)jmeta[nameof(Meta.ComandoAbrir)];
			meta.Caminho	  = (string)jmeta[nameof(Meta.Caminho)];
			TipoAmbiente tipo;
			Enum.TryParse<TipoAmbiente>((string)jmeta[nameof(Meta.Tipo)], out tipo );
			meta.Tipo = tipo;
			var jnSubProjs = jmeta[nameof(Meta.SubProjetos)] ;
			if(jnSubProjs is JsonArray jSubProjs){
				var SubProjs = jSubProjs.Select( subproj => MetaFromJson(subproj.AsObject()));
				meta.SubProjetos = SubProjs.ToArray();
			}			
			var jnScripts = jmeta[nameof(Meta.Scripts)]; 
			if( jnScripts is JsonObject jScripts){
				//Verificar depois o que acontece se value não for string
				meta.Scripts = jScripts.ToDictionary(
					jScript => jScript.Key, 
					jScript => (string)jScript.Value 
				);
			};
			return meta;
		}
		public static Meta JsonToMeta(string json)
		{
			if(string.IsNullOrWhiteSpace(json))
				return new Meta();
			try{
				return MetaFromJson(JsonSerializer.Deserialize<JsonObject>(json,JsonHelper.Options));
			}
			catch (Exception e) {
				throw new MetadadosInvalidosException(null, e);
			}
		}
	}

}