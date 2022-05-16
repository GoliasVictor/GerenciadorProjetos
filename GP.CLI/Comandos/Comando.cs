using System.ComponentModel;
using System.IO;
using Spectre.Console.Cli;

namespace GP.CLI
{
	public class RootSettings : CommandSettings
	{

		[CommandOption("-r|--raiz")]
		[CaminhoDiretorio(true, "Diretorio não existe FAMILIA")]
		public string strRaiz { get; init; }
		public DirectoryInfo Raiz => new DirectoryInfo(strRaiz ?? VarsAmbiente.DiretorioDeDesenvolvimento);
	}

}
