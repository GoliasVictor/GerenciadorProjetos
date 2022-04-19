using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace GP
{
	public static class Terminal{
		public static async Task<string> Rodar(string Comando){
			var psi = new ProcessStartInfo{
				WorkingDirectory =  Environment.CurrentDirectory,
				FileName = "/bin/bash",
				Arguments = @$"-c ""{Comando}""",
				RedirectStandardOutput = true,
				UseShellExecute = false,
				CreateNoWindow = true
			};

			using var process = Process.Start(psi);
			await process.WaitForExitAsync();

			var output = process.StandardOutput.ReadToEnd();
			return output;
		}
	}
}