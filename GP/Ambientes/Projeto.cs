using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace GP
{
	public class Projeto : Ambiente
	{

		public override TipoAmbiente Tipo => TipoAmbiente.Projeto;
		public string Linguagem { get; set; }
		public string ComandoAbrir { get; set; }
		public Dictionary<string, string> Scripts { get; set; }

		public SubProjeto[] SubProjetos { get; set; }
		public Projeto(Meta meta) : base(meta)
		{
			Linguagem = meta.Linguagem;
			ComandoAbrir = meta.ComandoAbrir;
			Scripts = meta.Scripts;
			SubProjetos = meta.SubProjetos?.Select((metaSubProjeto) => new SubProjeto(metaSubProjeto, this)).ToArray();
		}
		
		public override Meta ToMeta(){
			var meta = base.ToMeta();
			meta.Linguagem = Linguagem;
			meta.ComandoAbrir = ComandoAbrir;
			meta.SubProjetos = SubProjetos?.Select( sp => sp.ToMeta()).ToArray();
			return meta;
		}
		public override void Abrir(){
			if(ComandoAbrir is not null){
				_ = Terminal.Rodar(ComandoAbrir);
			} else {
				_ = Terminal.Rodar($"code {Diretorio.FullName}");
			}
		}
		public async Task Rodar(string NomeScript){
			

			if(Scripts.TryGetValue(NomeScript, out string ComandoScript)){
				await Terminal.Executar(ComandoScript,Diretorio);
			}
			else throw new InvalidOperationException("script não existe");

		}

	}
	public class SubProjeto : Projeto
	{
		public override TipoAmbiente Tipo => TipoAmbiente.SubProjeto;
		public Projeto Pai { get; set; }
		public string Caminho {get;set;}

		public override Meta ToMeta()
		{
			var meta = base.ToMeta();
			meta.Caminho = Caminho;
			return meta;
		}
		public SubProjeto(Meta meta, Projeto pai) : base(meta)
		{
			Pai = pai;
			Caminho = meta.Caminho;
			if(meta.Caminho is null)
				throw new MetadadosInvalidosException("Sem caminho definido");
			if(string.IsNullOrWhiteSpace(Nome ))
				Nome =  Path.GetFileName(meta.Caminho);
		}
		public override string ToString()
		{
			return $"{Pai.ToString()}/{Nome}";
		}
	}
}
