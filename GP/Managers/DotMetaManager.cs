using System;
using System.Text.Json;
using System.IO;
using System.Text.Json.Nodes;

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
		public static Meta JsonToMeta(string json)
		{
			if(string.IsNullOrWhiteSpace(json))
				return new Meta();
			try{
				return Meta.fromJson(JsonSerializer.Deserialize<JsonObject>(json,JsonHelper.Options));
			}
			catch (Exception e) {
				throw new MetadadosInvalidosException(null, e);
			}
		}
	}

}