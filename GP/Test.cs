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



		public static void Main(string[] args){
			TestMapear();
		}	
	}
}