using System;
using System.Collections.Generic;
using System.Text;
using SDL2;

namespace SDLCore
{
  public enum MessageBoxFlags : uint
  {
    Warning = SDL.SDL_MessageBoxFlags.SDL_MESSAGEBOX_WARNING,
    Error = SDL.SDL_MessageBoxFlags.SDL_MESSAGEBOX_ERROR,
    Info = SDL.SDL_MessageBoxFlags.SDL_MESSAGEBOX_INFORMATION
  }
  public class MessageBox
  {

    public static int ShowMessageBoxEx(MessageBoxFlags flags, string title, string message, string[] buttons, SDLWindow window = null, int btnEnter = -1, int btnEscape = -1)
    {
      SDL.SDL_MessageBoxData data = new SDL.SDL_MessageBoxData();
      data.title = title;
      data.message = message;
      if (window != null) data.window = window.GetPointer();
      else data.window = IntPtr.Zero;
      data.numbuttons = buttons.Length;
      data.flags = (SDL.SDL_MessageBoxFlags)flags;
      SDL.SDL_MessageBoxButtonData[] buttonStructs = new SDL.SDL_MessageBoxButtonData[buttons.Length];
      for (var i = 0; i < buttons.Length; i++)
      {
        buttonStructs[i].buttonid = i;
        buttonStructs[i].text = buttons[i];
        if (i == btnEnter)
        {
          buttonStructs[i].flags = SDL.SDL_MessageBoxButtonFlags.SDL_MESSAGEBOX_BUTTON_RETURNKEY_DEFAULT;
        }
        else if(i == btnEscape)
        {
          buttonStructs[i].flags = SDL.SDL_MessageBoxButtonFlags.SDL_MESSAGEBOX_BUTTON_ESCAPEKEY_DEFAULT;
        }
      }
      data.buttons = buttonStructs;
      if (SDL.SDL_ShowMessageBox(ref data, out int btnClicked) != 0) {
        throw new SDLException();
      }
      return btnClicked;
    }
    public static void ShowMessageBox(MessageBoxFlags flags, string title, string message) {
      if(SDL.SDL_ShowSimpleMessageBox((SDL.SDL_MessageBoxFlags)flags, title, message, IntPtr.Zero) != 0)
      {
        throw new SDLException();
      }
    }
  }
}
