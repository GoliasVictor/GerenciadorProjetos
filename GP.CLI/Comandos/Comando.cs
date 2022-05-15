using System.IO;
using Spectre.Console.Cli;

namespace GP.CLI
{
	public class RootSettings : CommandSettings
	{

		[CommandOption("-r|--raiz")]
		[CaminhoDiretorio(true, "Diretorio não existe FAMILIA")]
		public string strRaiz
		{
			get => Raiz.FullName;
			init => Raiz = new DirectoryInfo(value ?? VarsAmbiente.DiretorioDeDesenvolvimento);
		}
		public DirectoryInfo Raiz;

	}

}