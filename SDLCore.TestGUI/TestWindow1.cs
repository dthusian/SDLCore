using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace SDLCore.TestGUI
{
  class TestWindow1 : SDLWindow
  {
    public int NumPaints = 0;
    public DateTime ProfileStart;
    public TestWindow1() : base("SDLCore Test Window 1", WindowPosCentered, WindowPosCentered, 800, 450, 0)
    {
      ProfileStart = DateTime.Now;
    }

    public override void OnPaint() {
      Graphics graphics = Renderer.GetGDIRenderer();
      graphics.Clear(Color.FromArgb(255, 255, 255));
      SolidBrush brush = new SolidBrush(Color.FromArgb(255, 0, 0));
      graphics.FillEllipse(brush, 25, 25, 25, 25);
      graphics.Flush();
      Renderer.Draw();
      NumPaints++;
      if(NumPaints == 600)
      {
        DateTime ProfileEnd = DateTime.Now;
        NumPaints = 0;
        Console.WriteLine(ProfileEnd - ProfileStart);
        ProfileStart = DateTime.Now;
      }
    }
  }
}
