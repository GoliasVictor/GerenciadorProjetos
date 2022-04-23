using System;
using System.IO;
using System.Text.Json;
using System.Dynamic;


namespace GP
{
	static class Test{
		static readonly string Raiz = "/home/jvsb/Dev/Projetos/GerenciadorProjetos/AmbienteTest";
		static void TestMapear(){
			  Mapeador.MapearPastaRaiz(Raiz);
		}

		static JsonElement SerializarJson(string json){
			Console.WriteLine(json);
			return JsonSerializer.Deserialize<JsonElement>(json);
		}
		static void TestJsonToMetaCompleto(){
			var meta = DotMetaManager.JsonToMeta(SerializarJson(
			  @"{
					""Nome"": ""FamiliaJoguim"",
					""Tipo"": ""Cavalo"",
					""Descricao"" : ""ValorAleatorio"",
					""Caminho"" : [""ValorAleatorio""],
					""CaminhoMetadados"" : ""ValorAleatorio"",
					""Linguagem"" : ""ValorAleatorio"",
					""ComandoAbrir"" : ""ValorAleatorio"",
					""SubProjetos"" : ""ValorAleatorio""
				}"
			));
			
			var ambiente = meta.ToAmbiente();
			Console.WriteLine(meta);
			Console.WriteLine(DotMetaManager.MetaToJson(meta));
		}
		static void TestDotNetCoreManager(){
			
		}

		public static void Main(string[] args){
			TestJsonToMetaCompleto();
			TestMapear();
			TestDotNetCoreManager();
		}	
	}
}