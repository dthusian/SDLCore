using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace SDLCore
{
  internal class ExtraSDLBindings
  {
    // We need this for some SDL functions
    [DllImport("SDL2-2.0", CallingConvention = CallingConvention.Cdecl)]
    public static extern void SDL_free(IntPtr ptr);

    [DllImport("SDL2_mixer-2.0", EntryPoint = "SDL_GetError", CallingConvention = CallingConvention.Cdecl)]
    internal static extern IntPtr INTERNAL_Mix_GetError();

    public static string Mix_GetError()
    {
      return SDLUtil.NullTerminatedUTF8String(INTERNAL_Mix_GetError());
    }
  }
}
