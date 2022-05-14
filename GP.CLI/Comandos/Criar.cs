using System;
using Spectre.Console.Cli;

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
			var Contexto =  (DadosContexto)context.Data;
			Console.WriteLine(settings.Nome);
			return 0;
		}
	} 

}