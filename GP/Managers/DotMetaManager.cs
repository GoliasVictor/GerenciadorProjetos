using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.IO;

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
			var Options = new JsonSerializerOptions{
				WriteIndented=true,
				DefaultIgnoreCondition=JsonIgnoreCondition.WhenWritingDefault,
				Converters =
				{
					new JsonStringEnumConverter(JsonNamingPolicy.CamelCase)
				}
			};
			return JsonSerializer.Serialize<Meta>(meta,Options);
		}
		public static Meta JsonToMeta(string json)
		{
			if(string.IsNullOrWhiteSpace(json))
				json = "{}";
			JsonElement JsonElement = JsonSerializer.Deserialize<JsonElement>(json,JsonHelper.Options);
			return JsonToMeta(JsonElement);
		}


		public static Meta JsonToMeta(JsonElement json)
		{
			var jsonObject = json.EnumerateObject().Select(p => (JsonProperty?)p);
			var meta = new Meta()
			{
				Nome = jsonObject.GetPropString("Nome"),
				Descricao = jsonObject.GetPropString("Descricao"),
				ComandoAbrir = jsonObject.GetPropString("ComandoAbrir"),
				Caminho = jsonObject.GetPropString("Caminho"),
				Linguagem = jsonObject.GetPropString("Linguagem")
			};


			var strTipo = jsonObject.GetPropString("Tipo");
			try
			{
				meta.Tipo = Enum.Parse<TipoAmbiente>(strTipo, true);
			}
			catch
			{
				meta.Tipo = null;
			}

			var Aux = jsonObject.GetProp("SubProjetos");
			if (Aux is JsonElement jsonArray && jsonArray.ValueKind == JsonValueKind.Array)
			{
				
				meta.SubProjetos = jsonArray.EnumerateArray()
											.Select(JsonToMeta)
											.ToArray();
			}

			return meta;
		}
	}

}