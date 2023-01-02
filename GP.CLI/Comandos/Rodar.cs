using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Spectre.Console;
using Spectre.Console.Cli;
using System.Threading.Tasks;

namespace GP.CLI
{

	sealed class ComandoRodar : AsyncCommand<ComandoRodar.Settings>
	{
		public sealed class Settings : RootSettings
		{

			[CommandArgument(0,"[SCRIPT]")]
			public string Script { get; set; }

			[CommandOption("-p|--projeto")]
			public string Projeto { get; set; }
		}
		public override async Task<int> ExecuteAsync(CommandContext context, Settings settings)
		{
			var projeto = BuscadorAmbientes.BuscarProjetoAtual(settings.Raiz, settings.Projeto);
			_ = projeto ?? throw new Exception("Projeto não encontrado");
			
			if(projeto.Scripts?.DefaultIfEmpty() is null)
				throw new Exception("O projeto não possue nenhum script definido");
			
			if(string.IsNullOrWhiteSpace(settings.Script))
				MostrarScripts(projeto.Scripts);
			else 
				await projeto.Rodar(settings.Script);
			return 0;
		}
		void MostrarScripts(Dictionary<string, string> scripts){
			var table = new Table();
			table.Border(TableBorder.Simple);

			table.AddColumn(new TableColumn(new Text("Nome", new Style(Color.Yellow))));
			table.AddColumn(new TableColumn(new Text("Comando", new Style(Color.Green))));

			foreach(var script in scripts)
				table.AddRow(script.Key, script.Value);
			AnsiConsole.Write(table);
		}
	}
}