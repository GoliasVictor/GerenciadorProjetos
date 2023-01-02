using System;
using System.IO;

namespace GP
{
	static class BuscadorAmbientes{
		public static Projeto BuscarProjetoAtual(DirectoryInfo Raiz,string Nome){
			if(string.IsNullOrEmpty(Nome))
				return Mapeador.EncontrarAmbientePai(new DirectoryInfo(Environment.CurrentDirectory)) as Projeto;
			else 
				return Mapeador.EncontrarAmbiente(Raiz, Nome) as Projeto;

		}
	 }

}