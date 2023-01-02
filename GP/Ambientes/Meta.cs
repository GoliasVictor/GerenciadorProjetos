using System.IO;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using YamlDotNet.Serialization;

namespace GP
{ 
	public class Meta
	{
		[DefaultValue("")]
		public string Nome { get; set; } = "";
		public TipoAmbiente? Tipo { get; set; }
		public string Descricao { get; set; }
		public string Linguagem { get; set; }
		public string ComandoAbrir { get; set; }
		public string ComandoIniciar { get; set; }
		public Meta[] SubProjetos { get; set; }
		[YamlIgnore]
		public string Caminho { get; set; }
		[YamlIgnore]
		public IManager Manager {get;set;}
		public Dictionary<string, string> Scripts { get; set; }

		[YamlIgnore]
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
