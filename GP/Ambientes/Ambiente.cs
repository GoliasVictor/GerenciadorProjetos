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
		public string Nome { get; set; }
		public string Descricao { get; set; }
		public FileInfo FileMetadados => Mapeador.FileMetadados(Diretorio);
		public DirectoryInfo Diretorio { get; set; }
		public abstract TipoAmbiente Tipo { get; }
		protected Ambiente(){

		}
		public Ambiente(Meta meta)
		{
			Nome = meta.Nome;
			Descricao = meta.Descricao;
			Diretorio =  meta.Origem?.Directory;
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
			if(FileMetadados.Exists)	
				throw new MetaJaExisteException();
			Diretorio.Create();
			string json = JsonParser.MetaToJson(this.ToMeta());	
			File.WriteAllText(FileMetadados.FullName,json);
		}
	}
}
