using System;
using System.Collections.Generic;
using System.Text;
using SDL2;

namespace SDLCore
{
  public enum SystemCursor
  {
    Arrow = SDL.SDL_SystemCursor.SDL_SYSTEM_CURSOR_ARROW,
    IBeam = SDL.SDL_SystemCursor.SDL_SYSTEM_CURSOR_IBEAM,
    Hand = SDL.SDL_SystemCursor.SDL_SYSTEM_CURSOR_HAND,
    Crosshair = SDL.SDL_SystemCursor.SDL_SYSTEM_CURSOR_CROSSHAIR,
    Wait = SDL.SDL_SystemCursor.SDL_SYSTEM_CURSOR_WAIT,
    WaitArrow = SDL.SDL_SystemCursor.SDL_SYSTEM_CURSOR_WAITARROW,
    No = SDL.SDL_SystemCursor.SDL_SYSTEM_CURSOR_NO,
    DoubleArrowVertical = SDL.SDL_SystemCursor.SDL_SYSTEM_CURSOR_SIZENS,
    DoubleArrowHorizontal = SDL.SDL_SystemCursor.SDL_SYSTEM_CURSOR_SIZEWE,
    DoubleArrowPSlope = SDL.SDL_SystemCursor.SDL_SYSTEM_CURSOR_SIZENESW,
    DoubleArrowNSlope = SDL.SDL_SystemCursor.SDL_SYSTEM_CURSOR_SIZENWSE,
    QuadArrow = SDL.SDL_SystemCursor.SDL_SYSTEM_CURSOR_SIZEALL
  }
  public class SDLCursor : IDisposable
  {
    IntPtr sdlCursorPtr;
    public SDLCursor(byte[] data, byte[] mask, int w, int h, int hotx, int hoty) {
      unsafe {
        fixed(byte* pData = data, pMask = mask)
        {
          sdlCursorPtr = SDL.SDL_CreateCursor(
            new IntPtr((void*)pData), new IntPtr((void*)pMask), w, h, hotx, hoty);
        }
      }
    }
    public SDLCursor(SystemCursor cursor)
    {
      sdlCursorPtr = SDL.SDL_CreateSystemCursor((SDL.SDL_SystemCursor)cursor);
    }
    public void SetCursor()
    {
      SDL.SDL_SetCursor(sdlCursorPtr);
    }
    public void Dispose()
    {
      SDL.SDL_FreeCursor(sdlCursorPtr);
    }
  }
}
