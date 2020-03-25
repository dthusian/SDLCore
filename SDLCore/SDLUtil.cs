using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using SDL2;

namespace SDLCore
{
  internal class SDLUtil
  {
    public static string NullTerminatedUTF8String(IntPtr ptr)
    {
      unsafe
      {
        int i = 0;
        byte* rawPtr = (byte*)ptr.ToPointer();
        while (rawPtr[i] != 0) i++;
        byte[] buf = new byte[i];
        Marshal.Copy(new IntPtr(rawPtr), buf, 0, i);
        return Encoding.UTF8.GetString(buf);
      }
    }
    public static bool ToBool(SDL.SDL_bool v)
    {
      return v == SDL.SDL_bool.SDL_TRUE;
    }
  }
}
