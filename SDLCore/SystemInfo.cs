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
    HasSSE3 = 0x100,
    HasSSE41 = 0x200,
    HasSSE42 = 0x400
  }
  /// <summary>
  /// Information about the system.
  /// </summary>
  /// <remarks>
  /// This class only contains information not viewable in System.Environment.
  /// </remarks>
  public class SystemInfo
  {
    /// <summary>
    /// The size, in bytes of the CPU's L1 cache
    /// </summary>
    public int CPUCacheSize { get; private set; }
    /// <summary>
    /// The size, in megabytes, of the RAM (random access memory)
    /// </summary>
    public int RAMSizeMB { get; private set; }
    /// <summary>
    /// The CPU features supported.
    /// </summary>
    public CPUFeatures ProcessorFeatures { get; private set; }
    /// <summary>
    /// The amount of power left in the system battery, as a percentage
    /// May return -1 if there is no system battery.
    /// </summary>
    public int PowerLeftPercentage { get; private set; }
    /// <summary>
    /// The amount of power left in the system battery, as an estimated
    /// time until empty. May return -1 if the system does not report
    /// this value
    /// </summary>
    public int PowerLeftSeconds { get; private set; }
    /// <summary>
    /// The DPI (dots per inch) horizontally
    /// </summary>
    public double HorizontalDPI { get; private set; }
    /// <summary>
    /// The DPI (dots per inch) vertically
    /// </summary>
    public double VerticalDPI { get; private set; }
    /// <summary>
    /// The DPI (dots per inch) diagonally
    /// </summary>
    public double DiagonalDPI { get; private set; }
    /// <summary>
    /// [Internal] The index of video driver currently used
    /// </summary>
    internal int VideoDriverIndex { get; private set; }
    /// <summary>
    /// The name of the current video driver.
    /// </summary>
    public string CurrentVideoDriver { get; private set; }
    /// <summary>
    /// A list of names of possible video drivers that are installed.
    /// </summary>
    public string[] InstalledVideoDrivers { get; private set; }
    /// <summary>
    /// Retrieves system info and stores it in this class
    /// </summary>
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
    /// <summary>
    /// Updates the power info, as that may likely change over
    /// program execution.
    /// </summary>
    public void UpdatePowerInfo()
    {
      int powerLeft, powerSeconds;
      SDL.SDL_GetPowerInfo(out powerSeconds, out powerLeft);
      PowerLeftPercentage = powerLeft;
      PowerLeftSeconds = powerSeconds;
    }
  }
}
