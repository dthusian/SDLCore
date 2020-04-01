using System;
using System.Collections.Generic;
using System.Text;
using SDL2;
using SDLCore.EventArgs;

namespace SDLCore
{
  /// <summary>
  /// The basic object representing an application that uses SDL.
  /// </summary>
  /// <remarks>
  /// Its constructors run SDL_Init and similar functions to initialize SDL.
  /// Its Dispose() method runs SDL_Quit, so you should only create
  /// one of this object for the lifetime of your application.
  /// </remarks>
  public class SDLApp
  {
    private Dictionary<uint, SDLWindow> windows;
    /// <summary>
    /// Constructor. Runs SDL_Init to initialize SDL.
    /// SDL_Init is called with flags to initialize video and audio.
    /// </summary>
    public SDLApp(bool debug = false)
    {
      if (debug)
      {
        SDL.SDL_SetHint(SDL.SDL_HINT_WINDOWS_DISABLE_THREAD_NAMING, "1");
        SDL.SDL_LogSetAllPriority(SDL.SDL_LogPriority.SDL_LOG_PRIORITY_WARN);
      }
      if (SDL.SDL_Init(SDL.SDL_INIT_VIDEO) != 0)
      {
        throw new SDLException();
      }
      windows = new Dictionary<uint, SDLWindow>();
    }
    /// <summary>
    /// Registers a window to recieve window events from the main application queue.
    /// </summary>
    /// <param name="window">
    /// The window to register.
    /// </param>
    public void RegisterWindow(SDLWindow window)
    {
      if (window == null) throw new ArgumentNullException(nameof(window));
      uint windowID = SDL.SDL_GetWindowID(window.GetPointer());
      if (windowID == 0) throw new SDLException();
      windows.Add(windowID, window);
    }
    /// <summary>
    /// Runs the main event loop.
    /// </summary>
    /// <remarks>
    /// Note that this function does not return
    /// until the SDL_QUIT message is recieved. If you want to run
    /// a game tick loop, it should run on a separate thread.
    /// </remarks>
    public void RunWindowLoop()
    {
      try
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
                  // Or have the binding return a string here
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
      }catch(Exception ex)
      {
        MessageBox.ShowMessageBox(MessageBoxFlags.Error, "Unhandled Exception", ex.ToString());
        throw;
      }
    }
    /// <summary>
    /// Disposes the SDLApp object.
    /// </summary>
    /// <remarks>
    /// This function runs SDL_Quit as well as Dispose()-ing all
    /// windows registered. You do not have to Dispose() the
    /// windows yourself.
    /// </remarks>
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
