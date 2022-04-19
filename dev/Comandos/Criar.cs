using System;
using CommandLine;
namespace GP.CLI
{
	
	[Verb("criar",aliases:new[]{"c"})]
	public class CriarOptions
	{
	
		[Value(0)]
		public string Nome {get;set;}
	}
	static partial class Program{
		static void Criar(CriarOptions op){
			Console.WriteLine(op.Nome);
		}
	} 

}