using System;
using System.IO;
using Spectre.Console;
using Spectre.Console.Cli;

namespace GP.CLI
{

	sealed class ComandoAbrir : Command<ComandoAbrir.Settings>
	{
		public sealed class Settings : RootSettings
		{
			[CommandArgument(0, "<PROJETO>")]
			public string Nome { get; set; }
		}
		public override int Execute(CommandContext context, Settings settings)
		{
			Ambiente ambiente = Mapeador.EncontrarAmbiente(settings.Raiz, settings.Nome);
			if (ambiente is null)			
				throw new Exception("Projeto não encontrado");
			
			ambiente.Abrir();
			return 0;
		}
	}
}