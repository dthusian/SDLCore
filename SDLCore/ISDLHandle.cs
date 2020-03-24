using System;
using System.Collections.Generic;
using System.Text;

namespace SDLCore
{
  internal interface ISDLHandle : IDisposable
  {
    IntPtr GetPointer();
  }
}
