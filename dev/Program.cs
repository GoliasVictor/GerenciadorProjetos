using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using CommandLine;

namespace GP.CLI {    
    static partial class Program
    {
        static DirectoryInfo Raiz;  
        static Stopwatch sw =  new Stopwatch();

        static void Main(string[] args)
        {
            var strRaiz = Environment.GetEnvironmentVariable("TEST_DEV_DIR");
            if(strRaiz is null)
                return;
            Raiz = new DirectoryInfo(strRaiz);
            var argumentos = args.Length > 0 ?  args : "l prj".Split();
            Parser.Default.ParseArguments<AbrirOptions,CriarOptions,ListarOptions>(argumentos)
                   .WithParsed<AbrirOptions>(Abrir)
                   .WithParsed<CriarOptions>(Criar)
                   .WithParsed<ListarOptions>(Listar);
        }
    }
}
