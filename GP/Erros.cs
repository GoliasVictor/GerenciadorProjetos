using System;
using System.Runtime.Serialization;
namespace GP
{

	
	[System.Serializable]
	public class MetaJaExisteException : System.Exception
	{
		public MetaJaExisteException() { }
		public MetaJaExisteException(string message) : base(message) { }
		public MetaJaExisteException(string message, System.Exception inner) : base(message, inner) { }
		protected MetaJaExisteException(SerializationInfo info, StreamingContext context) : base(info, context) { }
	}
	[System.Serializable]
	public class AmbienteFisicoInexistenteException : System.Exception
	{
		public AmbienteFisicoInexistenteException() { }
		public AmbienteFisicoInexistenteException(string message) : base(message) { }
		public AmbienteFisicoInexistenteException(string message, System.Exception inner) : base(message, inner) { }
		protected AmbienteFisicoInexistenteException(SerializationInfo info, StreamingContext context) : base(info, context) { }
	}

	[System.Serializable]
	public class MutiplosMetadadosException : System.Exception
	{
		public MutiplosMetadadosException() { }
		public MutiplosMetadadosException(string message) : base(message) { }
		public MutiplosMetadadosException(string message, System.Exception inner) : base(message, inner) { }
		protected MutiplosMetadadosException(SerializationInfo info,StreamingContext context) : base(info, context) { }
	}
	[System.Serializable]
	public class MetadadosInvalidosException : System.Exception
	{
		public MetadadosInvalidosException() { }
		public MetadadosInvalidosException(string message) : base(message) { }
		public MetadadosInvalidosException(string message, System.Exception inner) : base(message, inner) { }
		protected MetadadosInvalidosException(SerializationInfo info,StreamingContext context) : base(info, context) { }
	}
}