using System.IO;

namespace GP
{
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
