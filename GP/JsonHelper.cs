using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace GP
{
	static class JsonHelper 
	{
		public static JsonElement? GetProp(this IEnumerable<JsonProperty?> json, string name)
		{
			var prop = json.FirstOrDefault(p => p?.Name.ToLower() == name.ToLower());
			return prop?.Value;
		}
		public static string GetPropString(this IEnumerable<JsonProperty?> json, string name)
		{

			JsonElement? aux = json.GetProp(name);
			if (aux is JsonElement valor && valor.ValueKind == JsonValueKind.String)
				return valor.GetString();

			return null;
		}
	}
}