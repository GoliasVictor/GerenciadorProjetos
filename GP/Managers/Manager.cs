using System.IO;
using System.Linq;

namespace GP
{
	interface IManager{
		bool EhAmbiente(DirectoryInfo dir);
		
		Meta GetMeta(DirectoryInfo dir);
	} 


	static class AllManager 
	{

		public static readonly IManager[] Managers = new IManager[]{
			DotMetaManager.Default,
			VsCodeManager.Default,
			DotNetCoreManager.Default,
			NPMManger.Default
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
			if (manager is not null)
				return manager.GetMeta(dir);
			else	
				return null;
		}
	}

}