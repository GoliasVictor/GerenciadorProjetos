using System.IO;

namespace GP
{
	public interface IManager{
		bool EhAmbiente(DirectoryInfo dir);
		
		Meta GetMeta(DirectoryInfo dir);
	} 

}