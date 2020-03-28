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
  /// <summary>
  /// Represents a window created with SDL.
  /// </summary>
  /// <remarks>
  /// To use this class, you should inherit it, and then
  /// construct an instance of your inherited window, and
  /// register it with an SDLApp.
  /// </remarks>
  public abstract class SDLWindow : ISDLHandle
  {
    // Fields/Properties
    public int Width { get; private set; }
    public int Height { get; private set; }
    IntPtr sdlWindowPtr;
    protected SDLRenderer Renderer;
    public const int WindowPosCentered = SDL.SDL_WINDOWPOS_CENTERED;
    public const int WindowPosArbitrary = SDL.SDL_WINDOWPOS_UNDEFINED;

    /// <summary>
    /// To be used in a derived class.
    /// </summary>
    /// <param name="title">
    /// The title of the window.
    /// </param>
    /// <param name="x">
    /// The X position where to place the window. Pass in WindowPosCentered to center the window.
    /// </param>
    /// <param name="y">
    /// The Y position where to place the window. Pass in WindowPosCentered to center the window.
    /// </param>
    /// <param name="w">
    /// The width of the window. Graphics and and rendering will be set to these dimensions.
    /// </param>
    /// <param name="h">
    /// The height of the window. Graphics and and rendering will be set to these dimensions.
    /// </param>
    /// <param name="flags">
    /// Options about a window.
    /// </param>
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

    /// <summary>
    /// Fired when the application is about to quit.
    /// </summary>
    public virtual void OnQuit() { }
    /// <summary>
    /// Fired when the window needs to be painted.
    /// </summary>
    /// <remarks>
    /// Usually, you should call Renderer.Draw() in this function.
    /// </remarks>
    public virtual void OnPaint() { Renderer.Draw(); }
    /// <summary>
    /// Fired once when the mouse is clicked downwards.
    /// </summary>
    /// <param name="action">
    /// Information about the mouse click.
    /// </param>
    public virtual void OnMouseDown(MouseAction action) { }
    /// <summary>
    /// Fired once when the mouse is released from a clicked state.
    /// </summary>
    /// <param name="action">
    /// Information about the mouse click.
    /// </param>
    public virtual void OnMouseUp(MouseAction action) { }
    /// <summary>
    /// Fired when the mouse moves. It may be fired many times
    /// across the motion of the mouse.
    /// </summary>
    /// <param name="x">
    /// The new X-position of the mouse (relative to the upper-left corner of the window)
    /// </param>
    /// <param name="y">
    /// The new Y-position of the mouse (relative to the upper-left corner of the window)
    /// </param>
    /// <param name="xv">
    /// The X-position of the mouse relative to the last message (how much the mouse moved)
    /// </param>
    /// <param name="yv">
    /// The Y-position of the mouse relative to the last message (how much the mouse moved)
    /// </param>
    public virtual void OnMouseMove(int x, int y, int xv, int yv) { }
    /// <summary>
    /// Fired once when the key is pressed, and fires mulitple times
    /// while the key is held.
    /// </summary>
    /// <param name="key"></param>
    public virtual void OnKeyDown(KeyAction key) { }
    /// <summary>
    /// Fired once when the key is released.
    /// </summary>
    /// <param name="key"></param>
    public virtual void OnKeyUp(KeyAction key) { }
    /// <summary>
    /// Fired when a user has begun to hover over the window with
    /// content that can be dropped.
    /// </summary>
    public virtual void OnBeginDrop() { }
    /// <summary>
    /// Fired when a user stops hovering over the window with
    /// content.
    /// </summary>
    public virtual void OnEndDrop() { }
    /// <summary>
    /// Fired when a user drops content onto the window.
    /// </summary>
    /// <param name="data"></param>
    public virtual void OnDataDrop(DataDrop data) { }
    /// <summary>
    /// Fired during a text editing action.
    /// </summary>
    /// <param name="edit"></param>
    public virtual void OnTextEdit(TextEditingAction edit) { }
    /// <summary>
    /// Fired when the text editing is committed.
    /// </summary>
    /// <param name="data"></param>
    public virtual void OnTextInput(string data) { EndTextInput(); }
    /// <summary>
    /// Starts text input. This function can be called anytime.
    /// </summary>
    public void StartTextInput()
    {
      SDL.SDL_StartTextInput();
    }
    /// <summary>
    /// Ends text input.
    /// </summary>
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
