using System;
using System.Collections.Generic;
using System.Text;
using SDL2;

namespace SDLCore.EventArgs
{
  public enum MouseButton : uint
  {
    LeftButton = SDL.SDL_BUTTON_LEFT,
    RightButton = SDL.SDL_BUTTON_RIGHT,
    MiddleClick = SDL.SDL_BUTTON_MIDDLE,
    X1 = SDL.SDL_BUTTON_X1,
    X2 = SDL.SDL_BUTTON_X2
  }
  public struct MouseAction
  {
    public int MouseX { get; private set; }
    public int MouseY { get; private set; }
    public int NumClicks { get; private set; }
    public MouseButton Button { get; private set; }
    public MouseAction(SDL.SDL_MouseButtonEvent ev)
    {
      MouseX = ev.x;
      MouseY = ev.y;
      NumClicks = ev.clicks;
      Button = (MouseButton)ev.button;
    }
  }
}
