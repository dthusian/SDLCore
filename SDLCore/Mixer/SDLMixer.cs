using System;
using System.Collections.Generic;
using System.Text;
using SDL2;

namespace SDLCore.Mixer
{
  [Flags]
  public enum MixerFormats
  {
    Flac = SDL_mixer.MIX_InitFlags.MIX_INIT_FLAC,
    MikiMod = SDL_mixer.MIX_InitFlags.MIX_INIT_MOD,
    OggVorbis = SDL_mixer.MIX_InitFlags.MIX_INIT_OGG,
    Mp3 = SDL_mixer.MIX_InitFlags.MIX_INIT_MP3
  }
  public class SDLMixer : IDisposable
  {
    public const int MusicChannel = -2;
    internal int sampleRate = 44100;
    internal int nChannels;
    internal MixerFormats formats;
    public SDLMixer(MixerFormats flags, int numChannels)
    {
      if (SDL.SDL_Init(SDL.SDL_INIT_AUDIO) != 0) {
        throw new SDLException();
      }
      if((SDL_mixer.MIX_InitFlags)SDL_mixer.Mix_Init((SDL_mixer.MIX_InitFlags)flags) != (SDL_mixer.MIX_InitFlags)flags)
      {
        throw new MixerException();
      }
      if (SDL_mixer.Mix_OpenAudio(44100, SDL.AUDIO_S16, 2, 4096) != 0) {
        throw new MixerException();
      }
      if(SDL_mixer.Mix_AllocateChannels(numChannels) != numChannels)
      {
        throw new MixerException();
      }
      formats = flags;
      nChannels = numChannels;
    }
    // General methods
    public MixerInfo GetInfo()
    {
      return new MixerInfo(this);
    }
    // Sound methods (for general sounds)
    public void PlaySound(IMixerSound sound, int channel = -1, bool propagateErrors = false)
    {
      int ret = -1;
      if(sound is MixerSound)
      {
        ret = SDL_mixer.Mix_PlayChannel(channel, sound.GetPointer(), 0);
      }
      else if(sound is MixerMusic) {
        ret = SDL_mixer.Mix_PlayMusic(sound.GetPointer(), 1);
      }
      else
      {
        throw new MixerException("Cannot play unknown object");
      }
      if(propagateErrors && ret == -1)
      {
        throw new MixerException();
      }
    }
    public int LoopSound(IMixerSound sound, int channel = -1)
    {
      int ret = -1;
      if(sound is MixerSound)
      {
        ret = SDL_mixer.Mix_PlayChannel(channel, sound.GetPointer(), -1);
      }
      else if(sound is MixerMusic)
      {
        ret = SDL_mixer.Mix_PlayMusic(sound.GetPointer(), -1);
      }
      else
      {
        throw new MixerException("Cannot play unknown object");
      }
      if (ret == -1)
      {
        throw new MixerException();
      }
      return ret;
    }
    public void Pause(int channel = -1)
    {
      if(channel == MusicChannel)
      {
        SDL_mixer.Mix_PauseMusic();
      }
      else
      {
        SDL_mixer.Mix_Pause(channel);
      }
    }
    public void Resume(int channel = -1)
    {
      if(channel == MusicChannel)
      {
        SDL_mixer.Mix_ResumeMusic();
      }
      else
      {
        SDL_mixer.Mix_Resume(channel);
      }
    }
    public void Stop(int channel = -1)
    {
      if(channel == MusicChannel)
      {
        SDL_mixer.Mix_HaltMusic();
      }
      else
      {
        SDL_mixer.Mix_HaltChannel(channel);
      }
    }
    public bool IsPlaying(int channel = -1)
    {
      if (channel == MusicChannel)
      {
        return SDL_mixer.Mix_PlayingMusic() == 1;
      }
      else
      {
        return SDL_mixer.Mix_Playing(channel) == 1;
      }
    }
    public int NumChannelsPlaying() {
      return SDL_mixer.Mix_Playing(-1);
    }
    public bool IsPaused(int channel = -1)
    {
      if(channel == MusicChannel)
      {
        return SDL_mixer.Mix_PausedMusic() == 1;
      }
      else
      {
        return SDL_mixer.Mix_Paused(channel) == 1;
      }
    }
    // Only for music
    public void SeekMusic(double position)
    {
      SDL_mixer.Mix_RewindMusic();
      SDL_mixer.Mix_SetMusicPosition(position);
    }
    public void Dispose()
    {
      SDL_mixer.Mix_CloseAudio();
    }
  }
}
