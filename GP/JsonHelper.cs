using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace GP
{
	static class JsonHelper
	{
		public static readonly JsonSerializerOptions Options = new JsonSerializerOptions()
		{
			ReadCommentHandling = JsonCommentHandling.Skip,
			AllowTrailingCommas = true,
			PropertyNameCaseInsensitive = true,
			PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
			WriteIndented=true,
			DefaultIgnoreCondition=JsonIgnoreCondition.WhenWritingDefault,
			Converters = {
				new JsonStringEnumConverter()
			}
		};
	}
}