using System;
using System.Collections.Generic;
using System.Text;
using SDL2;
using SDLCore.EventArgs;

namespace SDLCore
{
  public class SDLApp
  {
    private Dictionary<uint, SDLWindow> windows;
    public SDLApp()
    {
      if (SDL.SDL_Init(SDL.SDL_INIT_VIDEO | SDL.SDL_INIT_AUDIO) != 0)
      {
        throw new SDLException();
      }
      SDL.SDL_SetHint(SDL.SDL_HINT_WINDOWS_DISABLE_THREAD_NAMING, "1");
      windows = new Dictionary<uint, SDLWindow>();
    }

    public void RegisterWindow(SDLWindow window)
    {
      uint windowID = SDL.SDL_GetWindowID(window.GetPointer());
      if (windowID == 0) throw new SDLException();
      windows.Add(windowID, window);
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
            // Quit
            case SDL.SDL_EventType.SDL_QUIT:
              {
                foreach (var wnd in windows)
                {
                  wnd.Value.OnQuit();
                }
                quit = true;
                break;
              }
            // File dropping
            case SDL.SDL_EventType.SDL_DROPFILE:
            case SDL.SDL_EventType.SDL_DROPTEXT:
              {
                string data = SDLUtil.NullTerminatedUTF8String(e.drop.file);
                DataDrop drop = new DataDrop(e.type == SDL.SDL_EventType.SDL_DROPFILE, data);
                windows[e.drop.windowID].OnDataDrop(drop);
                ExtraSDLBindings.SDL_free(e.drop.file);
                // Note to SDL devs (or SDL2# dev): This is not ok
                // Either give SDL_free in SDL2# or have SDL free the pointer itself
                break;
              }
            case SDL.SDL_EventType.SDL_DROPBEGIN:
              {
                windows[e.drop.windowID].OnBeginDrop();
                break;
              }
            case SDL.SDL_EventType.SDL_DROPCOMPLETE:
              {
                windows[e.drop.windowID].OnEndDrop();
                break;
              }
            // Key actions
            case SDL.SDL_EventType.SDL_KEYDOWN:
              {
                KeyAction action = new KeyAction(e.key);
                windows[e.key.windowID].OnKeyDown(action);
                break;
              }
            case SDL.SDL_EventType.SDL_KEYUP:
              {
                KeyAction action = new KeyAction(e.key);
                windows[e.key.windowID].OnKeyUp(action);
                break;
              }
            // Mouse Actions
            case SDL.SDL_EventType.SDL_MOUSEMOTION:
              {
                windows[e.motion.windowID].OnMouseMove(e.motion.x, e.motion.y, e.motion.xrel, e.motion.yrel);
                break;
              }
            case SDL.SDL_EventType.SDL_MOUSEBUTTONDOWN:
              {
                windows[e.button.windowID].OnMouseDown(new MouseAction(e.button));
                break;
              }
            case SDL.SDL_EventType.SDL_MOUSEBUTTONUP:
              {
                windows[e.button.windowID].OnMouseUp(new MouseAction(e.button));
                break;
              }
            case SDL.SDL_EventType.SDL_TEXTEDITING:
              {
                windows[e.edit.windowID].OnTextEdit(new TextEditingAction(e.edit));
                break;
              }
            case SDL.SDL_EventType.SDL_TEXTINPUT:
              {
                unsafe
                {
                  windows[e.text.windowID].OnTextInput(SDLUtil.NullTerminatedUTF8String(new IntPtr(e.text.text)));
                }
                break;
              }
          }
        }
        foreach (var wnd in windows)
        {
          wnd.Value.OnPaint();
        }
      }
    }
    public void Dispose()
    {
      SDL.SDL_Quit();
      foreach (var wnd in windows)
      {
        wnd.Value.Dispose();
      }
    }
  }
}
