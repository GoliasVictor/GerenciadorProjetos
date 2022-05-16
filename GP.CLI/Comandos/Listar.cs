using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using Spectre.Console.Cli;
using Spectre.Console;
using Spectre.Console.Rendering;

namespace GP.CLI
{



	sealed class ComandoListar : Command<ComandoListar.Settings>
	{
		public sealed class Settings : RootSettings
		{
			[CommandArgument(0, "[pasta]")]
			[Description("Pasta que deve ser listada")]
			public string NomePasta { get; set; }

			[Description("tipo maximo da listagem, pas: apenas pastas, prj: pastas e projetos, sprj: pastas, projetos e subprojetos")]
			[CommandOption("-t|--tipo")]
			[DefaultValue(OptionTipoAmbiente.prj)]
			public OptionTipoAmbiente tipoMaximo { get; set; }
			[CommandOption("-p|--planificar")]
			[DefaultValue(false)]
			public bool Planificar { get; set; }

			[CommandOption("-m|--maxima-profundidade")]
			[DefaultValue(int.MaxValue)]
			public int profundidade { get; set; }

		}

		OptionTipoAmbiente tipoMaximo;
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
			tipoMaximo = settings.tipoMaximo;
			Pasta Pasta = Mapeador.MapearPastaRaiz(raiz);
			IRenderable result = null;
			if (settings.Planificar)
			{
				result = new Markup(ListarPastas(Pasta, new StringBuilder(), settings.profundidade).ToString());
			}
			else
			{
				var TreeRoot = new Tree("");
				Arvore(Pasta, TreeRoot, settings.profundidade);
				result = TreeRoot;
			}

			AnsiConsole.Write(result);
			return 0;
		}

		StringBuilder ListarPastas(Pasta pasta, StringBuilder sb, int profundidadeMaxima, string Prefixo = "")
		{
			if (profundidadeMaxima < 0)
				return sb;
			void NovaLinha(string valor)
			{
				sb.AppendLine($"{Prefixo}{valor}");
			}
			foreach (var ambiente in pasta.Ambientes)
			{
				var nome = ambiente.Nome.EscapeMarkup();

				if (ambiente is Pasta subPasta)
				{
					NovaLinha($"[blue]{nome}[/]");
					ListarPastas(subPasta, sb, profundidadeMaxima - 1, Prefixo + $"[blue]{nome}[/]/");
				}
				else if (tipoMaximo >= OptionTipoAmbiente.prj && ambiente is Projeto projeto)
				{
					NovaLinha($"{nome}");

					if (tipoMaximo == OptionTipoAmbiente.sprj && projeto.SubProjetos is not null)
						foreach (var subProjeto in projeto.SubProjetos)
							NovaLinha($"[yellow]+ {subProjeto.Nome.EscapeMarkup()}[/]");
				}
			}
			return sb;
		}

		void Arvore(Pasta pasta, IHasTreeNodes tree, int profundidadeMaxima)
		{
			if (profundidadeMaxima < 0)
				return;
			foreach (var ambiente in pasta.Ambientes)
			{
				if (ambiente is Pasta subPasta)
					Arvore(subPasta, tree.AddNode($"[blue]{subPasta.Nome.EscapeMarkup()}[/]"), profundidadeMaxima - 1);
				else if (tipoMaximo >= OptionTipoAmbiente.prj)
				{
					var NodeProjeto = tree.AddNode(ambiente.Nome.EscapeMarkup());
					if (tipoMaximo == OptionTipoAmbiente.sprj && ambiente is Projeto projeto && projeto.SubProjetos is not null)
						foreach (var subProjeto in projeto.SubProjetos)
							NodeProjeto.AddNode($"[yellow]+ {subProjeto.Nome.EscapeMarkup()}[/]");
				}
			}
		}
	}
}