using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using SDL2;

namespace SDLCore
{
  /// <summary>
  /// Contains general utility methods for SDL
  /// </summary>
  public class SDLUtil
  {
    internal static string NullTerminatedUTF8String(IntPtr ptr)
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
    internal static bool ToBool(SDL.SDL_bool v)
    {
      return v == SDL.SDL_bool.SDL_TRUE;
    }
    /// <summary>
    /// Gets the current text on the clipboard.
    /// </summary>
    /// <returns></returns>
    public static string GetClipboard()
    {
      return SDL.SDL_GetClipboardText();
    }
    /// <summary>
    /// Sets the current text on the clipboard.
    /// </summary>
    /// <param name="text"></param>
    public static void SetClipboard(string text)
    {
      if(SDL.SDL_SetClipboardText(text) != 0)
      {
        throw new SDLException();
      }
    }
  }
}
