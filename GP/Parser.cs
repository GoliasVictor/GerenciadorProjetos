using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Collections.Generic;
using System.Linq;
namespace GP
{
	public static class JsonParser
	{
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
			var JsonElement = JsonSerializer.Deserialize<JsonElement>(json);
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


		static JsonElement? GetProp(this IEnumerable<JsonProperty?> json, string name)
		{
			var prop = json.FirstOrDefault(p => p?.Name.ToLower() == name.ToLower());
			return prop?.Value;
		}
		static string GetPropString(this IEnumerable<JsonProperty?> json, string name)
		{

			JsonElement? aux = json.GetProp(name);
			if (aux is JsonElement valor && valor.ValueKind == JsonValueKind.String)
				return valor.GetString();

			return null;
		}

	}

}