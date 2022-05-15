using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using Spectre.Console.Cli;
using Spectre.Console;

namespace GP.CLI
{



	sealed class ComandoListar : Command<ComandoListar.Settings>
	{
			public sealed class Settings : RootSettings
			{
				//substituir por commandArgument
				[CommandOption("-p|--pasta")]
				public string NomePasta { get; set; }

	
				
				[Description("Profundidade maxima da listagem, pas: apenas pastas, prj: pastas e projetos, sprj: pastas, projetos e subprojetos")]
				[CommandOption("-t|--tipo-maximo")]
				[DefaultValue(OptionTipoAmbiente.prj)]
				public OptionTipoAmbiente profundidade {get;set;}

			}


		public override int Execute(CommandContext context, Settings settings)
		{
			var raiz = settings.Raiz;
			if (settings.NomePasta is not null)
			{
				var pasta = Mapeador.EncontrarAmbiente(raiz, settings.NomePasta);
				if (pasta is Pasta)
				{
					raiz = pasta.Diretorio;
				}
				else
				{
					string Erro;
					if (pasta is Ambiente)
						Erro = "Nome de Ambiente apontado não é uma pasta";
					else
						Erro = "Pasta não existe ou não possui metadados";
					throw new Exception(Erro);
				}

			}

			Pasta Pasta = Mapeador.MapearPastaRaiz(raiz);


			var TreeRoot = new Tree("");


			if (settings.profundidade == OptionTipoAmbiente.pas)
				ListarPastas(Pasta, TreeRoot);
			else
				Listar(Pasta,TreeRoot,settings.profundidade == OptionTipoAmbiente.sprj);

			AnsiConsole.Write(TreeRoot);

			return 0;
		}

		static void ListarPastas(Pasta pasta, IHasTreeNodes tree)
		{
			foreach (var subPasta in pasta.Ambientes.OfType<Pasta>())
				ListarPastas(subPasta, tree.AddNode($"[blue]{subPasta.Nome.EscapeMarkup()}[/]"));
		}
		static void Listar(Pasta pasta, IHasTreeNodes tree, bool IncluirSubProjetos )
		{
			foreach (var ambiente in pasta.Ambientes)
			{
				if (ambiente is Pasta subPasta)
					Listar(subPasta, tree.AddNode($"[blue]{subPasta.Nome.EscapeMarkup()}[/]"), IncluirSubProjetos);
				else {
					var NodeProjeto = tree.AddNode(ambiente.Nome.EscapeMarkup());
					if(IncluirSubProjetos && ambiente is Projeto projeto && projeto.SubProjetos is not null)
						foreach (var subProjeto in projeto.SubProjetos)
							NodeProjeto.AddNode($"[yellow]+{subProjeto.Nome.EscapeMarkup()}[/]");
				}
			}
		}
	}
}