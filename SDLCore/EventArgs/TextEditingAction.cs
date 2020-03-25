using System;
using System.Collections.Generic;
using System.Text;
using SDL2;
using System.Runtime.InteropServices;

namespace SDLCore.EventArgs
{
  public struct TextEditingAction
  {
    public string Text { get; private set; }
    public int CursorPos { get; private set; }
    public int SelectionLen { get; private set; }
    public TextEditingAction(SDL.SDL_TextEditingEvent e)
    {
      unsafe
      {
        Text = SDLUtil.NullTerminatedUTF8String(new IntPtr(e.text));
      }
      CursorPos = e.start;
      SelectionLen = e.length;
    }
  }
}
