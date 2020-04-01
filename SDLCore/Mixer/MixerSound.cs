using System;
using System.Collections.Generic;
using System.Text;
using SDL2;

namespace SDLCore.Mixer
{
  public class MixerSound : IMixerSound
  {
    IntPtr mixerChunkPtr;
    private int _volume = 64;
    public int Volume {
      get {
        return _volume;
      } set {
        _volume = value;
        SDL_mixer.Mix_VolumeChunk(mixerChunkPtr, value);
      } 
    }
    public MixerSound(string wavFile) {
      mixerChunkPtr = SDL_mixer.Mix_LoadWAV(wavFile);
      if(mixerChunkPtr == IntPtr.Zero)
      {
        throw new MixerException();
      }
    }
    public IntPtr GetPointer()
    {
      return mixerChunkPtr;
    }
    public void Dispose()
    {
      SDL_mixer.Mix_FreeChunk(mixerChunkPtr);
    }
  }
}
