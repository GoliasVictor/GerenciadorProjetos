using System;
using System.IO;
using Spectre.Console;
using Spectre.Console.Cli;

namespace GP.CLI
{
	[AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
	public sealed class CaminhoDiretorio : ParameterValidationAttribute
	{
		public bool Existente;
		public CaminhoDiretorio(bool existente, string errorMessage)
			: base(errorMessage)
		{
			Existente = existente;
		}

		public override ValidationResult Validate(CommandParameterContext context)
		{
			var Error = ValidationResult.Error($"Diretorio {(Existente ? "não" : "já")} existe.");

			switch (context.Value)
			{
				case string Path:
					return Directory.Exists(Path) ^ Existente
						? Error
						: ValidationResult.Success();

				case DirectoryInfo Dir:
					return Dir.Exists ^ Existente
						? Error
						: ValidationResult.Success();

				default:
					throw new InvalidOperationException($"Parametro invalido ({context.Parameter.PropertyName}).");
			}
		}
	}

}