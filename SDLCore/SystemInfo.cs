using System;
using System.Collections.Generic;
using System.Text;
using SDL2;

namespace SDLCore
{
  [Flags]
  public enum CPUFeatures
  {
    Null = 0x0,
    Has3DNow = 0x1,
    HasAVX = 0x2,
    HasAVX2 = 0x4,
    HasAltiVec = 0x8,
    HasMMX = 0x10,
    HasRDTSC = 0x20,
    HasSSE = 0x40,
    HasSSE2 = 0x80,
    HasSSE3 = 0x10,
    HasSSE41 = 0x20,
    HasSSE42 = 0x40
  }
  // Note: This class will only contain information not obtainable in
  // System.Environment class, and obtainable via SDL methods
  public class SystemInfo
  {
    public int CPUCacheSize { get; private set; }
    public int RAMSizeMB { get; private set; }
    public CPUFeatures ProcessorFeatures { get; private set; }
    public int PowerLeftPercentage { get; private set; }
    public int PowerLeftSeconds { get; private set; }
    public double HorizontalDPI { get; private set; }
    public double VerticalDPI { get; private set; }
    public double DiagonalDPI { get; private set; }
    internal int VideoDriverIndex { get; private set; }
    public string CurrentVideoDriver { get; private set; }
    public string[] InstalledVideoDrivers { get; private set; }
    public SystemInfo()
    {
      CPUCacheSize = SDL.SDL_GetCPUCacheLineSize();
      RAMSizeMB = SDL.SDL_GetSystemRAM();
      CPUFeatures feat = CPUFeatures.Null;
      if (SDLUtil.ToBool(SDL.SDL_Has3DNow())) feat |= CPUFeatures.Has3DNow;
      if (SDLUtil.ToBool(SDL.SDL_HasAVX())) feat |= CPUFeatures.HasAVX;
      if (SDLUtil.ToBool(SDL.SDL_HasAVX2())) feat |= CPUFeatures.HasAVX2;
      if (SDLUtil.ToBool(SDL.SDL_HasAltiVec())) feat |= CPUFeatures.HasAltiVec;
      if (SDLUtil.ToBool(SDL.SDL_HasMMX())) feat |= CPUFeatures.HasMMX;
      if (SDLUtil.ToBool(SDL.SDL_HasRDTSC())) feat |= CPUFeatures.HasRDTSC;
      if (SDLUtil.ToBool(SDL.SDL_HasSSE())) feat |= CPUFeatures.HasSSE;
      if (SDLUtil.ToBool(SDL.SDL_HasSSE2())) feat |= CPUFeatures.HasSSE2;
      if (SDLUtil.ToBool(SDL.SDL_HasSSE3())) feat |= CPUFeatures.HasSSE3;
      if (SDLUtil.ToBool(SDL.SDL_HasSSE41())) feat |= CPUFeatures.HasSSE41;
      if (SDLUtil.ToBool(SDL.SDL_HasSSE42())) feat |= CPUFeatures.HasSSE42;
      ProcessorFeatures = feat;
      UpdatePowerInfo();
      // Find current display driver index
      VideoDriverIndex = -1;
      CurrentVideoDriver = SDL.SDL_GetCurrentVideoDriver();
      List<string> tmpVideoDrivers = new List<string>();
      for(int i = 0; i < SDL.SDL_GetNumVideoDrivers(); i++)
      {
        string cVidDri = SDL.SDL_GetVideoDriver(i);
        if (cVidDri == CurrentVideoDriver)
        {
          VideoDriverIndex = i;
        }
        tmpVideoDrivers.Add(cVidDri);
      }
      InstalledVideoDrivers = tmpVideoDrivers.ToArray();
      // DPI
      if (SDL.SDL_GetDisplayDPI(VideoDriverIndex, out float ddpi, out float hdpi, out float vdpi) == 0)
      {
        HorizontalDPI = hdpi;
        VerticalDPI = vdpi;
        DiagonalDPI = ddpi;
      }
    }
    public void UpdatePowerInfo()
    {
      int powerLeft, powerSeconds;
      SDL.SDL_GetPowerInfo(out powerSeconds, out powerLeft);
      PowerLeftPercentage = powerLeft;
      PowerLeftSeconds = powerSeconds;
    }
  }
}
