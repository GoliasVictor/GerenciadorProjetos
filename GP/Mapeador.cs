using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading.Tasks;

namespace GP
{
	public static class Mapeador
	{
		public static FileInfo FileMetadados(DirectoryInfo Diretorio)
		{
			return new FileInfo(Path.GetFullPath(Path.Combine(Diretorio.FullName, "./.meta")));
		}
		public static Meta ObterMetadados(string Caminho)
		{
			return ObterMetadados(new DirectoryInfo(Caminho));
		}

		public static Meta ObterMetadados(DirectoryInfo Diretorio)
		{

			var FileMetadados = Mapeador.FileMetadados(Diretorio);
			string stringJsonMeta = null;
			try
			{
				stringJsonMeta = File.ReadAllText(FileMetadados.FullName);
			}
			catch (Exception _) when (_ is FileNotFoundException || _ is DirectoryNotFoundException)
			{
				throw new MetaInexistenteException();
			}
			var meta = JsonParser.JsonToMeta(stringJsonMeta);
			meta.Origem = FileMetadados;
			meta.Nome ??= Diretorio.Name;
			
			return meta;
		}
		public static List<Ambiente> MapearDiretorio(string Raiz)
		{
			return MapearDiretorio(new DirectoryInfo(Raiz));
		}
		public static List<Ambiente> MapearDiretorio(DirectoryInfo Raiz)
		{
			DirectoryInfo[] Dirs = Raiz.GetDirectories();
			var Ambientes = new List<Ambiente>();
			var Pastas =  new List<Pasta>();
			foreach (var Dir in Dirs)
			{
				try
				{
					var meta = ObterMetadados(Dir);
					var ambiente = meta.ToAmbiente();
					Ambientes.Add(ambiente);
					if(ambiente is Pasta pasta)
						Pastas.Add(pasta);
				}
				catch (MetaInexistenteException)
				{

				}
			}
			//Parallel.ForEach(Pastas,(pasta)=>{
			//	pasta.Ambientes = MapearDiretorio(pasta.Diretorio);
			//});
			foreach (Pasta pasta in Pastas) {
				pasta.Ambientes = MapearDiretorio(pasta.Diretorio);
			}
			return Ambientes;
		}
		public static Ambiente EncontrarAmbiente(DirectoryInfo Raiz, string Nome){
			DirectoryInfo[] dirs =  Raiz.GetDirectories();
			var pastas = new List<DirectoryInfo>();
			foreach (var dir in dirs)
			{
				var meta = ObterMetadados(dir);
				if(meta.Nome.ToLower() == Nome.ToLower())
					return meta.ToAmbiente();
				if(meta.Tipo is TipoAmbiente.Pasta)
					pastas.Add(dir);
			}
			foreach (var pasta in pastas )
			{
				var ambiente = EncontrarAmbiente(pasta, Nome);
				if(ambiente is not null)
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
	}
}