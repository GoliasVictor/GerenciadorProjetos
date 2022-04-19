using System.Collections.Generic;
using System.Linq;
namespace GP
{
	public class Pasta : Ambiente
	{
		public override TipoAmbiente Tipo => TipoAmbiente.Pasta;
		public List<Ambiente> Ambientes;
		public Ambiente this[string nome]{
			get{
				var Ambiente = Ambientes.Find((A) => A.Nome.ToLower() == nome.ToLower());
				if(Ambiente is not null)
					return Ambiente;
				foreach (var pasta in Ambientes.OfType<Pasta>())
				{
					Ambiente = pasta[nome];
					if(Ambiente is not null)
						return Ambiente;
				}
				return null;
			}
		}		
		public override void Abrir(){
			if(Diretorio.Exists)
				_ = Terminal.Rodar($"cd {Diretorio.FullName}");
			else
				throw new AmbienteFisicoInexistenteException();
		}
		public Pasta(Meta meta) : base(meta)
		{
		}
		public Pasta(string Nome){
			this.Nome =  Nome;
		}
	}

}
