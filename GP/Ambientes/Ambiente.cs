using System;
using System.IO;
namespace GP
{
	public enum TipoAmbiente
	{
		Pasta,
		Projeto,
		SubProjeto
	}
	
	public abstract class Ambiente
	{
		//TODO: Adicinoar palavras chaves, Adicionar Esconder e esconder filhos
		public string Nome { get; set; }
		public string Descricao { get; set; }
		public DirectoryInfo Diretorio { get; set; }
		public abstract TipoAmbiente Tipo { get; }
		
		protected Ambiente(){

		}
		public Ambiente(Meta meta)
		{
			Nome = meta.Nome;
			Descricao = meta.Descricao;
			Diretorio =  meta.Origem;
		}
		public override string ToString()
		{
			return Nome;
		}


		public virtual Meta ToMeta(){
			return new Meta(){
				Nome = Nome,
				Descricao = Descricao,
				Tipo =  Tipo,
			};
		}
		public abstract void Abrir();
		public virtual void Criar(){
			if(DotMetaManager.EhAmbiente(Diretorio))	
				throw new MetaJaExisteException();
			Diretorio.Create();
			string json = DotMetaManager.MetaToYAML(this.ToMeta());	
			File.WriteAllText(DotMetaManager.FileMetadados(Diretorio).FullName,json);
		}
	}
}
