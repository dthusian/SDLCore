using System;
using System.Collections.Generic;
using System.Text;

namespace SDLCore.TestGUI
{
  class TestWindow2 : SDLWindow
  {
    public int NumPaints = 0;
    public DateTime ProfileStart;
    public TestWindow2() : base("SDLCore FPS Profiler", WindowPosCentered, WindowPosCentered, 800, 450, 0) {
      ProfileStart = DateTime.Now;
    }

    public override void OnPaint()
    {
      Renderer.Draw();
      NumPaints++;
      if (NumPaints == 600)
      {
        DateTime ProfileEnd = DateTime.Now;
        NumPaints = 0;
        double mspframe = ((ProfileEnd - ProfileStart) / 600).TotalMilliseconds;
        Console.WriteLine("{0} ms - {1} fps", mspframe, Math.Floor(1000 / mspframe));
        ProfileStart = DateTime.Now;
      }
    }
  }
}
