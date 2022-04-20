using System;
using System.IO;
using System.Linq;
using System.Text;
using CommandLine;
using CommandLine.Text;

namespace GP.CLI
{
	[Verb("listar", aliases: new[] { "l" })]
	class ListarOptions
	{
		[Value(0, MetaName = "tipo")]
		public OptionTipoAmbiente Tipo { get; set; }
		[Option('p', "pasta")]
		public string NomePasta { get; set; }

		[Option('r', "raiz")]
		public string Raiz { get; set; }
	}
	partial class Program
	{
		static void Listar(ListarOptions op)
		{

			DirectoryInfo raiz = Raiz;

			if (op.Raiz is not null)
			{
				raiz = new DirectoryInfo(op.Raiz);
			}
			if (raiz.Exists)
			{
				Console.Error.WriteLine("Raiz não existe");
				return;
			}
			if (op.NomePasta is not null)
			{
				var pasta = Mapeador.EncontrarAmbiente(raiz, op.NomePasta);
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
					Console.Error.WriteLine(Erro);
					return;
				}

			}

			Pasta Pasta = Mapeador.MapearPastaRaiz(raiz);

			string Resultado;
			if (op.Tipo == OptionTipoAmbiente.pas)
				Resultado = ListarPastas(Pasta, 0);
			else
				Resultado = Listar(Pasta, 0);

			if (string.IsNullOrWhiteSpace(Resultado))
				Console.WriteLine("Nada encontrado");
			else
				Console.WriteLine(Resultado);
		}

		static string ListarPastas(Pasta pasta, int nivel)
		{
			var SB = new StringBuilder();
			foreach (var ambiente in pasta.Ambientes.OfType<Pasta>())
			{
				SB.Append(new string(' ', nivel));
				SB.Append(ambiente.Nome);
				SB.Append("\n");
				if (ambiente is Pasta PastaFilha)
					SB.Append(ListarPastas(PastaFilha, nivel + 1));
			}
			return SB.ToString();
		}
		static string Listar(Pasta pasta, int nivel)
		{
			var SB = new StringBuilder();
			foreach (var ambiente in pasta.Ambientes)
			{
				SB.Append(new string(' ', nivel));
				SB.Append(ambiente.Nome);
				SB.Append("\n");
				if (ambiente is Pasta PastaFilha)
					SB.Append(Listar(PastaFilha, nivel + 1));
			}
			return SB.ToString();
		}

	}
}