using System;
using System.Collections.Generic;
using System.Text;

namespace SDLCore
{

  [Serializable]
  public class SDLException : ApplicationException
  {
    public SDLException() { }
    public SDLException(string message) : base(message) { }
    public SDLException(string message, Exception inner) : base(message, inner) { }
    protected SDLException(
    System.Runtime.Serialization.SerializationInfo info,
    System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
  }
}
