using System;
using System.Collections.Generic;
using System.Text;
using SDL2;
using SDLCore.EventArgs;

namespace SDLCore
{
  [Flags]
  public enum WindowFlags : uint
  {
    AllowHighDPI = SDL.SDL_WindowFlags.SDL_WINDOW_ALLOW_HIGHDPI,
    AlwaysOnTop = SDL.SDL_WindowFlags.SDL_WINDOW_ALWAYS_ON_TOP,
    Borderless = SDL.SDL_WindowFlags.SDL_WINDOW_BORDERLESS,
    Foreign = SDL.SDL_WindowFlags.SDL_WINDOW_FOREIGN,
    Fullscreen = SDL.SDL_WindowFlags.SDL_WINDOW_FULLSCREEN,
    FullscreenDesktop = SDL.SDL_WindowFlags.SDL_WINDOW_FULLSCREEN_DESKTOP,
    Hidden = SDL.SDL_WindowFlags.SDL_WINDOW_HIDDEN,
    InputFocus = SDL.SDL_WindowFlags.SDL_WINDOW_INPUT_FOCUS,
    InputGrabbed = SDL.SDL_WindowFlags.SDL_WINDOW_INPUT_GRABBED,
    Maximized = SDL.SDL_WindowFlags.SDL_WINDOW_MAXIMIZED,
    Minimized = SDL.SDL_WindowFlags.SDL_WINDOW_MINIMIZED,
    MouseCapture = SDL.SDL_WindowFlags.SDL_WINDOW_MOUSE_CAPTURE,
    MouseFocus = SDL.SDL_WindowFlags.SDL_WINDOW_MOUSE_FOCUS,
    OpenGL = SDL.SDL_WindowFlags.SDL_WINDOW_OPENGL,
    PopupMenu = SDL.SDL_WindowFlags.SDL_WINDOW_POPUP_MENU,
    Resizable = SDL.SDL_WindowFlags.SDL_WINDOW_RESIZABLE,
    Shown = SDL.SDL_WindowFlags.SDL_WINDOW_SHOWN,
    SkipTaskbar = SDL.SDL_WindowFlags.SDL_WINDOW_SKIP_TASKBAR,
    Tooltip = SDL.SDL_WindowFlags.SDL_WINDOW_TOOLTIP,
    Utility = SDL.SDL_WindowFlags.SDL_WINDOW_UTILITY,
    Vulkan = SDL.SDL_WindowFlags.SDL_WINDOW_VULKAN
  }
  public abstract class SDLWindow : ISDLHandle
  {
    // Fields/Properties
    public int Width { get; private set; }
    public int Height { get; private set; }
    IntPtr sdlWindowPtr;
    protected SDLRenderer Renderer;
    public const int WindowPosCentered = SDL.SDL_WINDOWPOS_CENTERED;
    public const int WindowPosArbitrary = SDL.SDL_WINDOWPOS_UNDEFINED;

    // Constructors
    protected SDLWindow(string title, int x, int y, int w, int h, WindowFlags flags) {
      sdlWindowPtr = SDL.SDL_CreateWindow(title, x, y, w, h, (SDL.SDL_WindowFlags)flags);
      if(sdlWindowPtr == IntPtr.Zero)
      {
        throw new SDLException();
      }
      SDL.SDL_ShowWindow(sdlWindowPtr);
      Width = w;
      Height = h;
      Renderer = new SDLRenderer(sdlWindowPtr);
    }

    // Events
    public virtual void OnQuit() { }
    public virtual void OnPaint() { Renderer.Draw(); }
    public virtual void OnMouseDown(MouseAction action) { }
    public virtual void OnMouseUp(MouseAction action) { }
    public virtual void OnMouseMove(int x, int y, int xv, int yv) { }
    public virtual void OnKeyDown(KeyAction key) { }
    public virtual void OnKeyUp(KeyAction key) { }
    public virtual void OnBeginDrop() { }
    public virtual void OnEndDrop() { }
    public virtual void OnDataDrop(DataDrop data) { }
    public virtual void OnTextEdit(TextEditingAction edit) { }
    public virtual void OnTextInput(string data) { EndTextInput(); }
    // Text Input
    public void StartTextInput()
    {
      SDL.SDL_StartTextInput();
    }
    public void EndTextInput()
    {
      SDL.SDL_StopTextInput();
    }
    public bool IsInputtingText()
    {
      // Note to SDL devs: You really don't have to add your own bool type.
      // Just typedef it to an int or something and declare SDL_FALSE = 0 and SDL_TRUE = 1
      return SDL.SDL_IsTextInputActive() == SDL.SDL_bool.SDL_TRUE;
    }
    // Interface
    public IntPtr GetPointer()
    {
      return sdlWindowPtr;
    }
    public void Dispose()
    {
      SDL.SDL_DestroyWindow(sdlWindowPtr);
      Renderer.Dispose();
    }
  }
}
