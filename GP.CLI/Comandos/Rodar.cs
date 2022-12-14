using System;
using System.IO;
using Spectre.Console;
using Spectre.Console.Cli;

namespace GP.CLI
{

	sealed class ComandoRodar : Command<ComandoRodar.Settings>
	{
		public sealed class Settings : RootSettings
		{

			[CommandArgument(0,"<SCRIPT>")]
			public string Script { get; set; }

			[CommandOption("-p|--projeto")]
			public string Projeto { get; set; }
		}
		public override int Execute(CommandContext context, Settings settings)
		{
			Ambiente ambiente = Mapeador.EncontrarAmbiente(settings.Raiz, settings.Nome);
			if (ambiente is null)			
				throw new Exception("Projeto não encontrado");
			
			ambiente.Rodar(settings.Script);
			return 0;
		}
	}
}