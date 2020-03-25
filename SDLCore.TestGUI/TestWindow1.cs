using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using SDLCore.EventArgs;

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
      Renderer.Draw();
      NumPaints++;
      if(NumPaints == 600)
      {
        DateTime ProfileEnd = DateTime.Now;
        NumPaints = 0;
        double mspframe = ((ProfileEnd - ProfileStart) / 600).TotalMilliseconds;
        Console.WriteLine("{0} ms - {1} fps", mspframe, Math.Floor(1000 / mspframe));
        ProfileStart = DateTime.Now;
      }
    }

    public override void OnKeyDown(KeyAction key)
    {
      Console.WriteLine("KeyDown: {0} {1} {2}", key.Key, key.Flags, key.Repeat);
    }
    public override void OnKeyUp(KeyAction key)
    {
      Console.WriteLine("KeyUp: {0} {1}", key.Key, key.Flags);
    }
    public override void OnMouseMove(int x, int y, int xv, int yv)
    {
      Console.WriteLine("MouseMove: {0} {1} {2} {3}", x, y, xv, yv);
    }
    public override void OnMouseDown(MouseAction action)
    {
      Console.WriteLine("MouseDown: {0} {1} {2} {3}", action.MouseX, action.MouseY, action.Button, action.NumClicks);
    }
    public override void OnMouseUp(MouseAction action)
    {
      Console.WriteLine("MouseUp: {0} {1} {2} {3}", action.MouseX, action.MouseY, action.Button, action.NumClicks);
    }
    public override void OnBeginDrop()
    {
      Console.WriteLine("BeginDrop");
    }
    public override void OnEndDrop()
    {
      Console.WriteLine("EndDrop");
    }
    public override void OnDataDrop(DataDrop data)
    {
      Console.WriteLine("DataDrop: {0} {1}", data.IsFile, data.Data);
    }
  }
}
