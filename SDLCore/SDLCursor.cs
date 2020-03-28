using System;
using System.Collections.Generic;
using System.Text;
using SDL2;

namespace SDLCore
{
  /// <summary>
  /// Represents a system cursor icon.
  /// </summary>
  /// <remarks>
  /// This is used in SDLCursor's constructor for loading
  /// a system-defined cursor.
  /// </remarks>
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
  /// <summary>
  /// Represents an cursor.
  /// </summary>
  public class SDLCursor : ISDLHandle
  {
    IntPtr sdlCursorPtr;
    /// <summary>
    /// This function constructs a cursor with a custom cursor icon.
    /// </summary>
    /// <param name="data">
    /// This byte array will be interpreted as a sequence of bits.
    /// See remarks.
    /// </param>
    /// <param name="mask">
    /// This byte array will be interpreted as a sequence of bits.
    /// </param>
    /// <param name="w">
    /// The width (in pixels/bits) of the icon. Must be a multiple of 8.
    /// </param>
    /// <param name="h">
    /// The height (in pixels/bits) of the icon.
    /// </param>
    /// <param name="hotx">
    /// The x location of the actual mouse position relative to the icon.
    /// </param>
    /// <param name="hoty">
    /// The y location of the actual mouse position relative to the icon.
    /// </param>
    /// <remarks>
    /// This function interprets the data and mask arrays as sequences of bits.
    /// Each bit corresponds to one pixel in the output icon. The color of the pixel
    /// depends on the values for data and mask. Check the SDL docs (https://wiki.libsdl.org/SDL_CreateCursor)
    /// for specific documentation on this behavior.
    /// Hot-x and hot-y set the actual cursor position
    /// in comparison to where the icon is drawn.
    /// </remarks>
    public SDLCursor(byte[] data, byte[] mask, int w, int h, int hotx, int hoty) {
      unsafe {
        fixed(byte* pData = data, pMask = mask)
        {
          sdlCursorPtr = SDL.SDL_CreateCursor(
            new IntPtr((void*)pData), new IntPtr((void*)pMask), w, h, hotx, hoty);
        }
      }
    }
    /// <summary>
    /// Constructs a cursor from a system-defined icon.
    /// </summary>
    /// <param name="cursor">
    /// The cursor to construct
    /// </param>
    public SDLCursor(SystemCursor cursor)
    {
      sdlCursorPtr = SDL.SDL_CreateSystemCursor((SDL.SDL_SystemCursor)cursor);
    }
    /// <summary>
    /// Sets the current cursor to this cursor.
    /// </summary>
    public void SetCursor()
    {
      SDL.SDL_SetCursor(sdlCursorPtr);
    }
    /// <summary>
    /// ISDLHandle implementation.
    /// </summary>
    /// <returns></returns>
    public IntPtr GetPointer()
    {
      return sdlCursorPtr;
    }
    /// <summary>
    /// Disposes the cursor.
    /// </summary>
    public void Dispose()
    {
      SDL.SDL_FreeCursor(sdlCursorPtr);
    }
  }
}
