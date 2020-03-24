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
    Bitmap bitmap;
    Graphics gdiRendering;
    internal SDLRenderer(IntPtr windowPtr, int width = -1, int height = -1)
    {
      sdlRendererPtr = SDL.SDL_CreateRenderer(windowPtr, -1, 0);
      if (sdlRendererPtr == IntPtr.Zero)
      {
        throw new SDLException(string.Format("Failed to create renderer. Error: {0}", SDL.SDL_GetError()));
      }
      int w = 0, h = 0;
      if(width == -1 || height == -1)
      {
        if(SDL.SDL_GetRendererOutputSize(sdlRendererPtr, out w, out h) != 0)
        {
          throw new SDLException(string.Format("Failed to initialize render output size. Error: {0}", SDL.SDL_GetError()));
        }
      }
      else
      {
        w = width;
        h = height;
        if(SDL.SDL_RenderSetLogicalSize(sdlRendererPtr, w, h) != 0)
        {
          throw new SDLException(string.Format("Failed to initialize render output size. Error: {0}", SDL.SDL_GetError()));
        }
      }
      bitmap = new Bitmap(w, h);
      Width = w;
      Height = h;
      gdiRendering = Graphics.FromImage(bitmap);
    }
    public void Draw()
    {
      // Graphics already renders to bitmap
      // Bitmap -> SDL_Surface
      BitmapData dat = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
      IntPtr surfacePtr = SDL.SDL_CreateRGBSurfaceFrom(dat.Scan0, bitmap.Width, bitmap.Height, 32, dat.Stride, 0x00ff0000, 0x0000ff00, 0x000000ff, 0xff000000);
      if(surfacePtr == IntPtr.Zero)
      {
        throw new SDLException(string.Format("Failed to create SDL surface. Error: {0}", SDL.SDL_GetError()));
      }
      // SDL_Surface -> SDL_Texture
      IntPtr texturePtr = SDL.SDL_CreateTextureFromSurface(sdlRendererPtr, surfacePtr);
      if(surfacePtr == IntPtr.Zero)
      {
        throw new SDLException(string.Format("Failed to create SDL texture. Error: {0}", SDL.SDL_GetError()));
      }
      // SDL_Texture -> SDL_Renderer
      if(SDL.SDL_RenderCopy(sdlRendererPtr, texturePtr, IntPtr.Zero, IntPtr.Zero) != 0)
      {
        throw new SDLException(string.Format("Failed to render. Error: {0}", SDL.SDL_GetError()));
      }
      // Render the renderer
      SDL.SDL_RenderPresent(sdlRendererPtr);
      // Unlock bitmap to avoid cryptic GDI+ errors
      bitmap.UnlockBits(dat);
      // Free stuff
      SDL.SDL_FreeSurface(surfacePtr);
      SDL.SDL_DestroyTexture(texturePtr);
    }
    public IntPtr GetPointer()
    {
      return sdlRendererPtr;
    }
    public void Dispose() {
      SDL.SDL_DestroyRenderer(sdlRendererPtr);
    }
    public Graphics GetGDIRenderer()
    {
      return gdiRendering;
    }
  }
}
