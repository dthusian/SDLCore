using System;
using System.Collections.Generic;
using System.Text;
using SDL2;

namespace SDLCore.EventArgs
{
  [Flags]
  public enum KeyFlags : int
  {
    Null = 0x0,
    Shift = 0x1,
    Alt = 0x2,
    Ctrl = 0x4,
    CapsLock = 0x8
  }
  public struct KeyAction
  {
    public string Key { get; private set; }
    public KeyFlags Flags { get; private set; }
    public bool Repeat { get; private set; }
    internal KeyAction(SDL.SDL_KeyboardEvent ev) {
      SDL.SDL_Keymod modifiers = ev.keysym.mod;
      KeyFlags cflags = KeyFlags.Null;
      if ((modifiers & SDL.SDL_Keymod.KMOD_SHIFT) != 0) cflags |= KeyFlags.Shift;
      if ((modifiers & SDL.SDL_Keymod.KMOD_ALT) != 0) cflags |= KeyFlags.Alt;
      if ((modifiers & SDL.SDL_Keymod.KMOD_CTRL) != 0) cflags |= KeyFlags.Ctrl;
      if ((modifiers & SDL.SDL_Keymod.KMOD_CAPS) != 0) cflags |= KeyFlags.CapsLock;
      Flags = cflags;
      Key = MapSDLKeycode(ev.keysym.sym);
      Repeat = ev.repeat != 0;
    }

    internal static string MapSDLKeycode(SDL.SDL_Keycode key) {
      uint asciiRepresentation = (uint)key;
      if (asciiRepresentation < 128)
      {
        return ((char)asciiRepresentation).ToString();
      }
      switch (key)
      {
        case SDL.SDL_Keycode.SDLK_DOWN: return "Down";
        case SDL.SDL_Keycode.SDLK_UP: return "Up";
        case SDL.SDL_Keycode.SDLK_LEFT: return "Left";
        case SDL.SDL_Keycode.SDLK_RIGHT: return "Right";
        default: return "Unknown";
      }
    }
  }
}
