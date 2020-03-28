using System;
using System.Drawing;
using System.Drawing.Imaging;
using SDL2;

namespace SDLCore
{
  public class SDLRenderer : ISDLHandle
  {
    public int Width { get; private set; }
    public int Height { get; private set; }

    // Rendering components
    IntPtr sdlRendererPtr;
    IntPtr sdlTexturePtr;
    Bitmap bitmap;
    Graphics gdiRendering;
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
    }
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
    public Graphics GetGDIRenderer()
    {
      return gdiRendering;
    }
  }
}
