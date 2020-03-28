using System;
using System.Collections.Generic;
using System.Text;
using SDL2;

namespace SDLCore.Mixer
{
  // Like mixer sound but only one channel and has extra functionality
  public class MixerMusic : ISDLHandle
  {
    IntPtr mixerMusicPtr;
    public MixerMusic(string filePath) {
      mixerMusicPtr = SDL_mixer.Mix_LoadMUS(filePath);
    }
    public IntPtr GetPointer()
    {
      return mixerMusicPtr;
    }
    public void Dispose()
    {
      SDL_mixer.Mix_FreeMusic(mixerMusicPtr);
    }
  }
}
