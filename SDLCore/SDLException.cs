using System;
using System.Collections.Generic;
using System.Text;
using SDL2;

namespace SDLCore
{
  /// <summary>
  /// The exception that is thrown when an SDL function fails.
  /// </summary>
  /// <remarks>
  /// Often, the exception message is a string returned by SDL_GetError(),
  /// so it is possible to find documentation for that error message.
  /// </remarks>
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
