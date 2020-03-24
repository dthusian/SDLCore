using System;
using System.Drawing;

namespace SDLCore.TestGUI
{
  class Program
  {
    static void Main(string[] args)
    {
      SDLApp app = new SDLApp();
      app.RegisterWindow(new TestWindow1());
      app.RegisterWindow(new TestWindow1());
      app.RunWindowLoop();
      app.Dispose();
    }
  }
}
