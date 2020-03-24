using System;
using System.Collections.Generic;
using System.Text;
using SDL2;

namespace SDLCore
{
  public class SDLApp
  {
    private List<SDLWindow> windows;
    public SDLApp()
    {
      if(SDL.SDL_Init(SDL.SDL_INIT_VIDEO | SDL.SDL_INIT_AUDIO) != 0)
      {
        throw new SDLException(string.Format("Failed to initialize subsystems. Error: {0}", SDL.SDL_GetError()));
      }
      SDL.SDL_SetHint(SDL.SDL_HINT_WINDOWS_DISABLE_THREAD_NAMING, "1");
      windows = new List<SDLWindow>();
    }

    public void RegisterWindow(SDLWindow window)
    {
      windows.Add(window);
    }

    public void RunWindowLoop()
    {
      bool quit = false;
      while (!quit)
      {
        SDL.SDL_Event e;
        if (SDL.SDL_PollEvent(out e) != 0)
        {
          switch (e.type)
          {
            case SDL.SDL_EventType.SDL_QUIT:
              {
                foreach(SDLWindow wnd in windows)
                {
                  wnd.OnQuit();
                }
                quit = true;
                break;
              }
            case SDL.SDL_EventType.SDL_KEYDOWN:
              {
                KeyAction action = new KeyAction(e.key, KeyFlags.Null);
                foreach(SDLWindow wnd in windows)
                {
                  wnd.OnKeyDown(action);
                }
                break;
              }
          }
        }
        foreach(SDLWindow wnd in windows)
        {
          wnd.OnMsg();
        }
      }
    }

    public void Dispose()
    {
      SDL.SDL_Quit();
    }
  }
}
