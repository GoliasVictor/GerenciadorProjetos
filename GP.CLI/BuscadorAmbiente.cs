using System;
using System.IO;

namespace GP
{
	static class BuscadorAmbientes{
		static Mapeador mapeador = new Mapeador(Logger.Default);
		public static Projeto BuscarProjetoAtual(DirectoryInfo Raiz,string Nome){
			if(string.IsNullOrEmpty(Nome))
				return mapeador.EncontrarAmbientePai(new DirectoryInfo(Environment.CurrentDirectory)) as Projeto;
			else 
				return mapeador.EncontrarAmbiente(Raiz, Nome) as Projeto;

		}
	 }

}