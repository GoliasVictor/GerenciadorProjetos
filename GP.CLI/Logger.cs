
using Spectre.Console;
using Spectre.Console.Cli;
class Logger : ILogger
{
	private Logger()
	{
	}
	public static Logger Default = new();

	public void Log(string mensagem)
	{
		AnsiConsole.WriteLine(mensagem);
	}

	public void LogAviso(string mensagem)
	{
		AnsiConsole.WriteLine("Aviso:" + mensagem);
	}

	public void LogErro(string mensagem)
	{
		AnsiConsole.WriteLine("Erro:" + mensagem);
	}
}