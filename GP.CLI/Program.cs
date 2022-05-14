using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using Spectre.Console;
using Spectre.Console.Cli;

namespace GP.CLI
{
	class DadosContexto
	{
		public DirectoryInfo Raiz;
	}

	static partial class Program
	{

		static Stopwatch sw = new Stopwatch();
		static int Main(string[] args)
		{
#if TEST
			var strRaiz = Environment.GetEnvironmentVariable("TEST_DEV_DIR");
			args = args.Length > 0 ? args : "l -t 1 -r /home/jvsb/Dev/Projetos".Split();
#else
            var strRaiz = Environment.GetEnvironmentVariable("DEV_DIR");
#endif

			if (strRaiz is null)
				return -1;
			var contexto = new DadosContexto
			{
				Raiz = new DirectoryInfo(strRaiz)
			};

			var app = new CommandApp();
			app.Configure(config =>
			{
				config.SetApplicationName("gp");

				config.AddCommand<ComandoAbrir>("abrir")
						  .WithAlias("a")
						  .WithData(contexto);

				config.AddCommand<ComandoListar>("listar")
					  .WithAlias("l")
					  .WithData(contexto);

				config.AddCommand<ComandoCriar>("criar")
					  .WithAlias("c")
					  .WithData(contexto);
			});
			return app.Run(args);

		}
	}
}
