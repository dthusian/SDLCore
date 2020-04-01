using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace SDLCore
{
  internal class ExtraSDLBindings
  {
    // We need this for some SDL functions
    [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
    public static extern void SDL_free(IntPtr ptr);
  }
}
