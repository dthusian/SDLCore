using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace SDLCore.TestGUI
{
  class TestWindow4 : SDLWindow
  {
    Color bgBrush = Color.FromArgb(0, 0, 255);
    SolidBrush rectBrush = new SolidBrush(Color.FromArgb(255, 0, 0));
    SolidBrush circleBrush = new SolidBrush(Color.FromArgb(0, 255, 0));
    public TestWindow4() : base("SDLCore Graphics Debug", WindowPosCentered, WindowPosCentered, 400, 400, 0)
    {
      
    }
    public override void OnPaint()
    {
      var gra = Renderer.GetGDIRenderer();
      gra.Clear(bgBrush);
      gra.FillRectangle(rectBrush, new Rectangle(100, 100, 100, 100));
      gra.FillEllipse(circleBrush, new Rectangle(250, 250, 100, 100));
      Renderer.Draw();
    }
  }
}
