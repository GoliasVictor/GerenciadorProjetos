using System;
using System.Runtime.Serialization;
namespace GP
{
	[System.Serializable]
	public class MetaInexistenteException : System.Exception
	{
		public MetaInexistenteException() { }
		public MetaInexistenteException(string message) : base(message) { }
		public MetaInexistenteException(string message, System.Exception inner) : base(message, inner) { }
		protected MetaInexistenteException(SerializationInfo info, StreamingContext context) : base(info, context) { }
	}
	
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
}