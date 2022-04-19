using System;
using System.Linq;
using System.Text;
using CommandLine;

namespace GP.CLI
{
	[Verb("listar",aliases:new[]{"l"})]
	class ListarOptions
	{
		[Value(0, MetaName = "tipo")]
		public OptionTipoAmbiente Tipo {get;set;}
	}
	partial class Program{
		    static void Listar(ListarOptions op){
            var Pasta = Mapeador.MapearPastaRaiz(Raiz);
            
            if(op.Tipo == OptionTipoAmbiente.pas)
                Console.WriteLine(ListarPastas(Pasta,0));
            else
                Console.WriteLine(Listar(Pasta,0));
            
        }

        static string ListarPastas(Pasta pasta,int nivel )
        {
            var SB = new StringBuilder();
            foreach (var ambiente in pasta.Ambientes.OfType<Pasta>())
            {
                SB.Append(new string(' ',nivel));
                SB.Append(ambiente.Nome);
                SB.Append("\n");
                if(ambiente is Pasta PastaFilha)
                    SB.Append(ListarPastas(PastaFilha, nivel+1));
            }
            return SB.ToString();
        }
        static string Listar(Pasta pasta,int nivel ) 
        {
            var SB = new StringBuilder();
            foreach (var ambiente in pasta.Ambientes)
            {
                SB.Append(new string(' ',nivel));
                SB.Append(ambiente.Nome);
                SB.Append("\n");
                if(ambiente is Pasta PastaFilha)
                    SB.Append(Listar(PastaFilha, nivel+1));
            }
            return SB.ToString();
        }

    }
}