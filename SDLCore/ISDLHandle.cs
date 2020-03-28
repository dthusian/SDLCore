using System;
using System.Collections.Generic;
using System.Text;

namespace SDLCore
{
  public interface ISDLHandle : IDisposable
  {
    IntPtr GetPointer();
  }
}
