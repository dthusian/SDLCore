using System;
using System.Collections.Generic;
using System.Text;
using SDL2;

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
    public int Width { get; private set; }
    public int Height { get; private set; }
    IntPtr sdlWindowPtr;
    protected SDLRenderer Renderer;
    public const int WindowPosCentered = SDL.SDL_WINDOWPOS_CENTERED;
    public const int WindowPosArbitrary = SDL.SDL_WINDOWPOS_UNDEFINED;
    protected SDLWindow(string title, int x, int y, int w, int h, WindowFlags flags) {
      sdlWindowPtr = SDL.SDL_CreateWindow(title, x, y, w, h, (SDL.SDL_WindowFlags)flags);
      if(sdlWindowPtr == IntPtr.Zero)
      {
        throw new SDLException(string.Format("Failed to create window. Error: {0}", SDL.SDL_GetError()));
      }
      SDL.SDL_ShowWindow(sdlWindowPtr);
      Width = w;
      Height = h;
      Renderer = new SDLRenderer(sdlWindowPtr);
    }
    public virtual void OnQuit() { }
    public virtual void OnPaint() { Renderer.Draw(); }
    public virtual void OnMouseDown(int x, int y) { }
    public virtual void OnMouseUp(int x, int y) { }
    public virtual void OnKeyDown(KeyAction key) { }
    public IntPtr GetPointer()
    {
      return sdlWindowPtr;
    }
    public void Dispose()
    {
      SDL.SDL_DestroyWindow(sdlWindowPtr);
    }
  }
}
