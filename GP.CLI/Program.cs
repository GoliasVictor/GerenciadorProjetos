using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using Spectre.Console;
using Spectre.Console.Cli;

namespace GP.CLI
{

	static partial class Program
	{

		static int Main(string[] args)
		{
			#if TEST
				args = args.Length > 0 ? args : "r dev -p Joguim".Split();
			#endif
			
			var app = new CommandApp();
			app.Configure(config =>
			{
				config.SetApplicationName("gp");
				//config.SetExceptionHandler((ex) => AnsiConsole.WriteException(ex));
				config.AddCommand<ComandoAbrir>("abrir"   ).WithAlias("a");
				config.AddCommand<ComandoCriar>("criar"   ).WithAlias("c");
				config.AddCommand<ComandoDir  >("dir"     ).WithAlias("d").WithAlias("diretorio");
				config.AddCommand<ComandoListar>("listar" ).WithAlias("l");
				config.AddCommand<ComandoRodar>("rodar").WithAlias("r").WithAlias("run");
				
			});
			return app.Run(args);
		}
	}
}
