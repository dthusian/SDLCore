using System;
using System.Collections.Generic;
using System.Text;
using SDL2;

namespace SDLCore
{

  [Serializable]
  public class SDLException : ApplicationException
  {
    public SDLException() : base(SDL.SDL_GetError()) { }
    public SDLException(string message) : base(message) { }
    public SDLException(string message, Exception inner) : base(message, inner) { }
    protected SDLException(
    System.Runtime.Serialization.SerializationInfo info,
    System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
  }
}
