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

		static Stopwatch sw = new Stopwatch();
		static int Main(string[] args)
		{
            #if TEST
				args = args.Length > 0 ? args : "l -t 1 -r ../AmbienteTest/".Split();
            #endif

			var app = new CommandApp();
			app.Configure(config =>
			{
				config.SetApplicationName("gp");

				config.AddCommand<ComandoAbrir>("abrir"  ).WithAlias("a");
				config.AddCommand<ComandoListar>("listar").WithAlias("l");
				config.AddCommand<ComandoCriar>("criar"  ).WithAlias("c");
			});
			return app.Run(args);

		}
	}
}
