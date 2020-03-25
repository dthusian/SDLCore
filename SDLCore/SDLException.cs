using System;
using System.Collections.Generic;
using System.Text;
using SDL2;

namespace SDLCore
{

  [Serializable]
  public class SDLException : ApplicationException
  {
    public SDLException() { }
    public SDLException(string message) : base(string.Format(message, SDL.SDL_GetError())) { }
    public SDLException(string message, Exception inner) : base(string.Format(message, SDL.SDL_GetError()), inner) { }
    protected SDLException(
    System.Runtime.Serialization.SerializationInfo info,
    System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
  }
}
