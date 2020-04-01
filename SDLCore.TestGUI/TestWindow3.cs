using SDLCore.EventArgs;
using SDLCore.Mixer;
using System;
using System.Collections.Generic;
using System.Text;

namespace SDLCore.TestGUI
{
  class TestWindow3 : SDLWindow
  {
    SDLMixer mixer;
    MixerSound beat1, beat2;
    public TestWindow3() : base("SDLCore Mixer Debug Window", WindowPosCentered, WindowPosCentered, 300, 300, 0) {
      mixer = new SDLMixer(MixerFormats.Mp3, 8);
      beat1 = new MixerSound("./Kick-Drum-1.wav");
      beat2 = new MixerSound("./Snare-Drum-1.wav");
    }
    public override void OnKeyDown(KeyAction key)
    {
      switch (key.Key)
      {
        case "q":
          {
            if (key.Flags.HasFlag(KeyFlags.Shift))
            {
              mixer.LoopSound(beat1);
            }
            else
            {
              mixer.PlaySound(beat1);
            }
            break;
          }
        case "w":
          {
            if (key.Flags.HasFlag(KeyFlags.Shift))
            {
              mixer.LoopSound(beat2);
            }
            else
            {
              mixer.PlaySound(beat2);
            }
            break;
          }
      }
    }
    public new void Dispose()
    {
      base.Dispose();
      mixer.Dispose();
    }
  }
}
