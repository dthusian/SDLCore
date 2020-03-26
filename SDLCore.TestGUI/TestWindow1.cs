using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using SDLCore.EventArgs;

namespace SDLCore.TestGUI
{
  class TestWindow1 : SDLWindow
  {
    List<SDLCursor> systemcursors;
    int cursorIndex;
    public TestWindow1() : base("SDLCore Debug Window", WindowPosCentered, WindowPosCentered, 800, 450, 0)
    {
      systemcursors = new List<SDLCursor> {
        new SDLCursor(SystemCursor.Arrow),
        new SDLCursor(SystemCursor.Crosshair),
        new SDLCursor(SystemCursor.DoubleArrowHorizontal),
        new SDLCursor(SystemCursor.DoubleArrowHorizontal),
        new SDLCursor(SystemCursor.DoubleArrowNSlope),
        new SDLCursor(SystemCursor.DoubleArrowPSlope),
        new SDLCursor(SystemCursor.DoubleArrowVertical),
        new SDLCursor(SystemCursor.Hand),
        new SDLCursor(SystemCursor.IBeam),
        new SDLCursor(SystemCursor.No),
        new SDLCursor(SystemCursor.QuadArrow),
        new SDLCursor(SystemCursor.Wait),
        new SDLCursor(SystemCursor.WaitArrow)
      };
      cursorIndex = -1;
    }

    public override void OnPaint() {
      Renderer.Draw();
    }

    public override void OnKeyDown(KeyAction key)
    {
      Console.WriteLine("KeyDown: {0} {1} {2}", key.Key, key.Flags, key.Repeat);
      cursorIndex++;
      cursorIndex %= systemcursors.Count;
      systemcursors[cursorIndex].SetCursor();
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
