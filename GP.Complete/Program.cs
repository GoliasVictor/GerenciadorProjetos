
namespace GP.Complete 
{
    internal class Program
    {
		static string filtro="";
		static string[] Comandos = new []{"abrir","listar","criar"};

        static void Main(string[] args)
        {
			if(args.Length <= 1){
				Console.WriteLine(string.Join("\n", Comandos));		
				return;
			}
			string Comando = args[1];
			if(args.Length == 2 && !Comandos.Contains(args[1])){
				Console.WriteLine(string.Join("\n",Comandos.Where(c=>  0 == c.ToLower().IndexOf(Comando))));
				return;
			}
			var Args = args.ToList();
			Args.RemoveAt(0);
			Args.RemoveAt(0);
			switch (Comando)
			{
				case "a":
				case "abrir":
				case "l":
				case "listar":
					filtro = string.Join("",Args).ToLower();
					var Pasta = Mapeador.MapearPastaRaiz(Environment.GetEnvironmentVariable("DEV_DIR"));
					ImprimirNomes(Pasta);
				break;
				default: 
				break;
			}
        }
		
		static void ImprimirNomes(Pasta pasta){
			foreach (var ambiente in pasta.Ambientes)
			{	
				if(ambiente.Nome.ToLower().IndexOf(filtro) == 0)
					Console.WriteLine($"{ambiente.Nome}");
				if(ambiente is Pasta subPasta)
					ImprimirNomes(subPasta);
			}
		}
	
    }
}
