using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace SDLCore.TestGUI
{
  class TestWindow1 : SDLWindow
  {
    public TestWindow1() : base("SDLCore Test Window 1", WindowPosCentered, WindowPosCentered, 200, 200, 0)
    {

    }

    public override void OnPaint() {
      Graphics graphics = Renderer.GetGDIRenderer();
      graphics.Clear(Color.FromArgb(255, 255, 255));
      SolidBrush brush = new SolidBrush(Color.FromArgb(255, 0, 0));
      graphics.FillEllipse(brush, 25, 25, 25, 25);
      graphics.Flush();
      Renderer.Draw();
    }
  }
}
