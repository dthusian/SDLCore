using System;
using System.Collections.Generic;
using System.Text;
using SDL2;

namespace SDLCore.Mixer
{

  [Serializable]
  public class MixerException : SDLException
  {
    public MixerException() : base() { }
    public MixerException(string message) : base(message) { }
    public MixerException(string message, Exception inner) : base(message, inner) { }
    protected MixerException(
    System.Runtime.Serialization.SerializationInfo info,
    System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
  }
}
