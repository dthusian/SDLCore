using System;
using System.Collections.Generic;
using System.Text;

namespace SDLCore.Mixer
{
  public class MixerInfo
  {
    public MixerFormats SupportedFormats { get; private set; }
    public int SampleRate { get; private set; }
    public int NumChannels { get; private set; }
    internal MixerInfo(SDLMixer mixer)
    {
      SampleRate = mixer.sampleRate;
      NumChannels = mixer.nChannels;
      SupportedFormats = mixer.formats;
    }
  }
}
