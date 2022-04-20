using System.IO;
using System.Text.Json.Serialization;

namespace GP
{
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
	}

}
