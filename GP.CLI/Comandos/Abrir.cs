using System;
using CommandLine;

namespace GP.CLI
{
	[Verb("abrir",aliases:new[]{"a"})]
	public class AbrirOptions
	{
	
		[Value(0)]
		public string Nome {get;set;}
	}
	partial class Program 
	{
        static void Abrir(AbrirOptions op){
            Ambiente ambiente = Mapeador.EncontrarAmbiente(Raiz,op.Nome);
            if(ambiente is null){
                Console.WriteLine("Projeto não encontrado");
                return;
            }
            ambiente.Abrir();
        }
	}
}