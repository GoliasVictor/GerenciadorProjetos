using System.IO;
using System.Linq;

namespace GP
{
	static class AllManager 
	{

		public static readonly IManager[] Managers = new IManager[]{
			DotMetaManager.Default,
			VsCodeManager.Default,
			DotNetCoreManager.Default,
			NPMManger.Default,
			GitManger.Default,
		};
		public static IManager GetManager(DirectoryInfo dir){
			return null;
		}
		public static bool EhAmbiente(DirectoryInfo dir)
		{
			return Managers.Any((m) =>  m.EhAmbiente(dir));
		}

		public static Meta GetMeta(DirectoryInfo dir)
		{
			IManager manager = Managers.FirstOrDefault( (m)=> m.EhAmbiente(dir));
			Meta meta = manager?.GetMeta(dir);
			if(meta?.SubProjetos is not null){
				foreach(var subproj in meta?.SubProjetos){	
					if(string.IsNullOrEmpty(subproj?.Caminho))
						throw new MetadadosInvalidosException("Caminho do projeto não definido");
				}
			}
			if(meta is not null)
				meta.Manager = manager;
			return meta;
		}
	}

}