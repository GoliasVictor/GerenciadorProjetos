using System;

namespace GP.CLI
{
	static class VarsAmbiente{
		public static string DiretorioDeDesenvolvimento {
			get{
				#if TEST
					return Environment.GetEnvironmentVariable("TEST_DEV_DIR");
				#else
					return Environment.GetEnvironmentVariable("DEV_DIR");
				#endif
			}
		}
	}
}