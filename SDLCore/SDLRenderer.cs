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
        SDL.SDL_GetRendererOutputSize(sdlRendererPtr, out w, out h);
      }
      else
      {
        w = width;
        h = height;
        SDL.SDL_RenderSetLogicalSize(sdlRendererPtr, w, h);
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
      IntPtr surfacePtr = SDL.SDL_CreateRGBSurfaceFrom(dat.Scan0, bitmap.Width, bitmap.Height, 32, 4, 0x00ff0000, 0x0000ff00, 0x000000ff, 0xff00000);
      // SDL_Surface -> SDL_Texture
      IntPtr texturePtr = SDL.SDL_CreateTextureFromSurface(sdlRendererPtr, surfacePtr);
      SDL.SDL_FreeSurface(surfacePtr);
      // SDL_Texture -> SDL_Renderer
      SDL.SDL_RenderCopy(sdlRendererPtr, texturePtr, IntPtr.Zero, IntPtr.Zero);
      SDL.SDL_DestroyTexture(texturePtr);
      // Render the renderer
      SDL.SDL_RenderPresent(sdlRendererPtr);
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
