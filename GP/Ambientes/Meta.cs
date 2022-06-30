using System.IO;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using System;
using System.Linq;
namespace GP
{
	//To-do Adicionar Scripts
	public class Meta
	{
		public string Nome { get; set; }
		public TipoAmbiente? Tipo { get; set; }
		public string Descricao { get; set; }
		public string Linguagem { get; set; }
		public string ComandoAbrir { get; set; }
		public Meta[] SubProjetos { get; set; }
		public string Caminho { get; set; }

		[JsonIgnore]
		public DirectoryInfo Origem {get;set;}
		public Ambiente ToAmbiente(){
			Ambiente Ambiente;
			switch(Tipo){
				case TipoAmbiente.Pasta:
					Ambiente =  new Pasta(this);
				break;
				case TipoAmbiente.Projeto:
				default:
					Ambiente =  new Projeto(this);
				break;
			}
			return Ambiente;
		}

		public static Meta fromJson(JsonObject jmeta){
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
				var SubProjs = jSubProjs.Select( subproj => fromJson(subproj.AsObject()));
				meta.SubProjetos = SubProjs.ToArray();
			}			


		
			return meta;
		}
	}

}
