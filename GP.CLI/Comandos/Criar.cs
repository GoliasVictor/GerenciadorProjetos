using System;
using Spectre.Console.Cli;
using Spectre.Console;
namespace GP.CLI
{
	
	class ComandoCriar : Command<ComandoCriar.Settings>{
		public sealed class Settings : RootSettings
		{
        	[CommandArgument(0, "<PROJETO>")]
			public string Nome { get; set; }
		}
		public override int Execute(CommandContext context, Settings settings)
		{
			AnsiConsole.MarkupLine(settings.Nome.EscapeMarkup());
			return 0;
		}
	} 

}