using System;
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
			var Raiz =  ((DadosContexto)context.Data).Raiz;
			Ambiente ambiente = Mapeador.EncontrarAmbiente(Raiz, settings.Nome);
			if (ambiente is null){
				AnsiConsole.MarkupLine("Projeto não encontrado");
				return 	-1;
			}
			ambiente.Abrir();
			return 0;
		}
	}
}