using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Concurrent;

namespace GP
{
	public class Mapeador
	{
		private readonly ILogger logger;

		public Mapeador(ILogger logger)
		{
			this.logger = logger;
		}

		public Meta ObterMetadados(string Caminho)
		{


				return ObterMetadados(new DirectoryInfo(Caminho));

		}
		public Meta ObterMetadados(DirectoryInfo dir)
		{
			try
			{
				Meta meta = AllManager.GetMeta(dir);
				if (meta is not null)
				{
					meta.Origem = dir;
					meta.Nome ??= dir.Name;
					return meta;
				}
				return null;
			}
			catch (MetadadosInvalidosException ex)
			{
#if TEST
				throw;
#else
				logger.LogAviso($"Metadados invalidos em {dir.FullName}, Motivo: {ex.Message} ");
				return null;
#endif
			}
		}
		public List<Ambiente> MapearDiretorio(string Raiz)
		{
			return MapearDiretorio(new DirectoryInfo(Raiz));
		}
		public List<Ambiente> MapearDiretorio(DirectoryInfo Raiz)
		{
			DirectoryInfo[] Dirs = Raiz.GetDirectories();
			var Ambientes = new ConcurrentBag<Ambiente>();
			var Pastas = new ConcurrentBag<Pasta>();
			foreach (var Dir in Dirs)
			{
				Meta meta = ObterMetadados(Dir);

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
		public Ambiente EncontrarAmbiente(DirectoryInfo Raiz, string Nome)
		{
			DirectoryInfo[] dirs = Raiz.GetDirectories();
			var pastas = new List<DirectoryInfo>();
			foreach (var dir in dirs)
			{
				Meta meta = ObterMetadados(dir);
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
		public Ambiente EncontrarAmbientePai(DirectoryInfo Pasta)
		{
			var pastas = new List<DirectoryInfo>();
			DirectoryInfo DiretorioAtual = Pasta;
			Stack<DirectoryInfo> DiretoriosPais =  new();
			while (DiretorioAtual != null){
				DiretoriosPais.Push(DiretorioAtual);
				DiretorioAtual = DiretorioAtual.Parent;
			}
			while(DiretoriosPais.TryPop(out var dir)){
				Meta meta =  ObterMetadados(dir);
				if(meta is not null && meta.Tipo == TipoAmbiente.Projeto)
					return meta.ToAmbiente();
			}
			return null;
		}
		public Pasta MapearPastaRaiz(string Raiz)
		{
			return MapearPastaRaiz(new DirectoryInfo(Raiz));
		}
		public Pasta MapearPastaRaiz(DirectoryInfo Raiz)
		{
			Pasta pasta = new Pasta(Raiz.Name);
			pasta.Diretorio = Raiz;
			pasta.Ambientes = MapearDiretorio(Raiz);
			return pasta;
		}

	}
}