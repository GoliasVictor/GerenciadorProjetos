using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Concurrent;

namespace GP
{
	public static class Mapeador
	{

		public static Meta ObterMetadados(string Caminho)
		{
			return ObterMetadados(new DirectoryInfo(Caminho));
		}
		public static Meta ObterMetadados(DirectoryInfo dir)
		{
			Meta meta = AllManager.GetMeta(dir);
			if (meta is not null)
			{
				meta.Origem = dir;
				meta.Nome ??= dir.Name;
				return meta;
			}
			return null;

			//foreach (var parser in Parser.Parsers)
			//{
			//	if(parser.EhAmbiente(diretorio))
			//		meta =  parser.GetMeta(diretorio);
			//}


		}
		public static List<Ambiente> MapearDiretorio(string Raiz)
		{
			return MapearDiretorio(new DirectoryInfo(Raiz));
		}
		public static List<Ambiente> MapearDiretorio(DirectoryInfo Raiz)
		{
			DirectoryInfo[] Dirs = Raiz.GetDirectories();
			var Ambientes = new ConcurrentBag<Ambiente>();
			var Pastas = new ConcurrentBag<Pasta>();
			foreach (var Dir in Dirs)
			{
				Meta meta;
#if TEST
				meta = ObterMetadados(Dir);
#else
				try
				{
					meta = ObterMetadados(Dir);
				}
				catch (MetadadosInvalidosException)
				{
					continue;
				}
#endif

				if (meta is null)
					continue;
				var ambiente = meta.ToAmbiente();
				Ambientes.Add(ambiente);
				if (ambiente is Pasta pasta)
					Pastas.Add(pasta);
			}
			Parallel.ForEach(Pastas, (pasta) =>
			{
				pasta.Ambientes = MapearDiretorio(pasta.Diretorio);
			});

			return Ambientes.OrderBy((a) => a.Tipo).ThenBy((a) => a.Nome).ToList();
		}
		public static Ambiente EncontrarAmbiente(DirectoryInfo Raiz, string Nome)
		{
			DirectoryInfo[] dirs = Raiz.GetDirectories();
			var pastas = new List<DirectoryInfo>();
			foreach (var dir in dirs)
			{
				Meta meta;
#if TEST
				meta = ObterMetadados(dir);
#else
				try
				{
					meta = ObterMetadados(dir);
				}
				catch (MetadadosInvalidosException)
				{
					continue;
				}
#endif

				if (meta is null)
					continue;
				if (meta.Nome.ToLower() == Nome.ToLower())
					return meta.ToAmbiente();
				if (meta.Tipo is TipoAmbiente.Pasta)
					pastas.Add(dir);
			}
			foreach (var pasta in pastas)
			{
				var ambiente = EncontrarAmbiente(pasta, Nome);
				if (ambiente is not null)
					return ambiente;
			}
			return null;
		}
		public static Pasta MapearPastaRaiz(string Raiz)
		{
			return MapearPastaRaiz(new DirectoryInfo(Raiz));
		}
		public static Pasta MapearPastaRaiz(DirectoryInfo Raiz)
		{
			Pasta pasta = new Pasta(Raiz.Name);
			pasta.Diretorio = Raiz;
			pasta.Ambientes = MapearDiretorio(Raiz);
			return pasta;
		}
		public static string ReadAllText(this FileInfo file)
		{
			return File.ReadAllText(file.FullName);
		}
		public static FileInfo GetFile(this DirectoryInfo dir, string pathToFile)
		{
			return new FileInfo(Path.GetFullPath(Path.Combine(dir.FullName, pathToFile)));
		}
		public static DirectoryInfo GetDirectory(this DirectoryInfo dir, string pathToDirectory)
		{
			return new DirectoryInfo(Path.GetFullPath(Path.Combine(dir.FullName, pathToDirectory)));
		}
	}
}