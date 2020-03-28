using System;
using System.Drawing;
using System.Drawing.Imaging;
using SDL2;

namespace SDLCore
{
  /// <summary>
  /// Encapsulates an SDL renderer.
  /// </summary>
  /// <remarks>
  /// An SDL renderer renders drawing commands to the screen,
  /// but due to the lack of 2D primitive support, and the
  /// fact the SDL_gfx is a software renderer, drawing commands
  /// are actually rendered with System.Drawing.Graphics.
  /// GetGDIRenderer() returns a Graphics object that you can
  /// use to draw 2D primitives to the screen. For exact details
  /// about how the rendering works, check the remarks section
  /// of the Draw() method.
  /// </remarks>
  public class SDLRenderer : ISDLHandle
  {
    public int Width { get; private set; }
    public int Height { get; private set; }

    // Rendering components
    IntPtr sdlRendererPtr;
    IntPtr sdlTexturePtr;
    Bitmap bitmap;
    Graphics gdiRendering;
    /// <summary>
    /// [Internal] Constructs an SDLRenderer from an SDL window.
    /// This constructor should only be called from an SDLWindow.
    /// </summary>
    /// <param name="windowPtr"></param>
    internal SDLRenderer(IntPtr windowPtr)
    {
      sdlRendererPtr = SDL.SDL_CreateRenderer(windowPtr, -1, 0);
      if (sdlRendererPtr == IntPtr.Zero)
      {
        throw new SDLException();
      }
      int w, h;
      if (SDL.SDL_GetRendererOutputSize(sdlRendererPtr, out w, out h) != 0)
      {
        throw new SDLException();
      }
      bitmap = new Bitmap(w, h);
      Width = w;
      Height = h;
      gdiRendering = Graphics.FromImage(bitmap);
      sdlTexturePtr = SDL.SDL_CreateTexture(sdlRendererPtr, SDL.SDL_PIXELFORMAT_ARGB8888, (int)SDL.SDL_TextureAccess.SDL_TEXTUREACCESS_STREAMING, w, h);
    }
    /// <summary>
    /// Resizes the renderer, if its size has changed.
    /// </summary>
    public void Resize()
    {
      // We need to realloc the bitmaps, graphics, and texture
      // First get the render output size
      int w, h;
      if (SDL.SDL_GetRendererOutputSize(sdlRendererPtr, out w, out h) != 0)
      {
        throw new SDLException();
      }
      SDL.SDL_DestroyTexture(sdlTexturePtr);
      gdiRendering.Dispose();
      bitmap.Dispose();
      bitmap = new Bitmap(w, h);
      gdiRendering = Graphics.FromImage(bitmap);
      sdlTexturePtr = SDL.SDL_CreateTexture(sdlRendererPtr, SDL.SDL_PIXELFORMAT_ABGR8888, (int)SDL.SDL_TextureAccess.SDL_TEXTUREACCESS_STREAMING, w, h);
    }
    /// <summary>
    /// Draws the rendering to the screen.
    /// </summary>
    /// <remarks>
    /// SDLRenderers internally manage a Bitmap, a Graphics, and two IntPtrs
    /// (one SDL_Texture, and one SDL_Renderer). The graphics object is returned
    /// for the user to draw on. The Bitmap is the target of the Graphics returned.
    /// When Draw() is called, LockBits() is called on the Bitmap to access the raw
    /// pixel data. This pixel data is passed into SDL_UpdateTexture to copy the
    /// data into the texture. Then the texture is rendered. The pixels are copied
    /// about 3 times each frame, but with block-copy implementations, it shouldn't
    /// be too much of a performance issue.
    /// 
    /// Because the Bitmap is copied into a texture before rendering, flicker
    /// should not be an issue either.
    /// </remarks>
    public void Draw()
    {
      // Graphics already renders to bitmap
      // Bitmap -> SDL_Texture
      BitmapData dat = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
      if (SDL.SDL_UpdateTexture(sdlTexturePtr, IntPtr.Zero, dat.Scan0, dat.Stride) != 0)
      {
        throw new SDLException();
      }
      // SDL_Texture -> SDL_Renderer
      if (SDL.SDL_RenderCopy(sdlRendererPtr, sdlTexturePtr, IntPtr.Zero, IntPtr.Zero) != 0)
      {
        throw new SDLException();
      }
      // Render the renderer
      SDL.SDL_RenderPresent(sdlRendererPtr);
      // Unlock bitmap to avoid cryptic GDI+ errors
      bitmap.UnlockBits(dat);
    }
    public IntPtr GetPointer()
    {
      return sdlRendererPtr;
    }
    public void Dispose()
    {
      SDL.SDL_DestroyRenderer(sdlRendererPtr);
      SDL.SDL_DestroyTexture(sdlTexturePtr);
      bitmap.Dispose();
      gdiRendering.Dispose();
    }
    /// <summary>
    /// Gets a graphics object that can be used to render 2D primives.
    /// </summary>
    /// <returns></returns>
    public Graphics GetGDIRenderer()
    {
      return gdiRendering;
    }
  }
}
