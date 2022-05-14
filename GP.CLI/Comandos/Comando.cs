using System;
using System.IO;
using Spectre.Console;
using Spectre.Console.Cli;

namespace GP.CLI
{
	
	public class RootSettings : CommandSettings
	{
		[CommandOption("-r|--raiz")]
		public string Raiz { get; set; }
		 public override ValidationResult Validate(){
			 return new DirectoryInfo(Raiz).Exists 
			 	? ValidationResult.Error("Diretorio Raiz Não Existe")
				: ValidationResult.Success();
		 }
	}

}