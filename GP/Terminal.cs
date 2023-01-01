using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace GP
{
	public static class Terminal{

		public static async Task<string> Rodar(string Arquivo, string argumentos,DirectoryInfo dir = null){
			var psi = new ProcessStartInfo{
				WorkingDirectory =  dir?.FullName ?? Environment.CurrentDirectory,
				FileName = Arquivo,
				Arguments = argumentos,
				RedirectStandardOutput = true,
				RedirectStandardError =  true,
				RedirectStandardInput =  true,
				UseShellExecute = false,
				CreateNoWindow = true
			};

			using var process = Process.Start(psi);
			await process.WaitForExitAsync();
			return process.StandardOutput.ReadToEnd();
		}
		public static async Task<string> Rodar(string comando, DirectoryInfo dir = null){
			return await Rodar("/bin/bash",@$"-c ""{comando}""");
		}
		public static async Task Executar(string Arquivo, string argumentos,DirectoryInfo dir = null){
			var psi = new ProcessStartInfo{
				WorkingDirectory =  dir?.FullName ?? Environment.CurrentDirectory,
				FileName = Arquivo,
				Arguments = argumentos,
				UseShellExecute = false,
				CreateNoWindow = false
			};

			using var process = Process.Start(psi);
			await process.WaitForExitAsync();
		}
		public static async Task Executar(string comando, DirectoryInfo dir = null){
			await Executar("/bin/bash",@$"-c ""{comando}""",dir);
		}
	}
}