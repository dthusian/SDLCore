using System;
using System.Drawing;

namespace SDLCore.TestGUI
{
  class Program
  {
    static void Main(string[] args)
    {
      SDLApp app = new SDLApp();
      SystemInfo info = new SystemInfo();
      Console.WriteLine("CPU Cache size: {0} | Ram Size: {1} | CPU features: {2}", info.CPUCacheSize, info.RAMSizeMB, info.ProcessorFeatures);
      Console.WriteLine("DPI: Horizontal: {0} | Vertical: {1} | Diagonal: {2}", info.HorizontalDPI, info.VerticalDPI, info.DiagonalDPI);
      Console.WriteLine("Current video driver: {0} | All video drivers: {1}", info.CurrentVideoDriver, string.Join(" ", info.InstalledVideoDrivers));
      Console.WriteLine("Power left (percentage): {0} (seconds): {1}", info.PowerLeftPercentage, info.PowerLeftSeconds);
      Console.WriteLine("Current clipboard text: {0}", SDLUtil.GetClipboard());
      Console.WriteLine("Press enter to launch 3 debug messageboxes");
      Console.ReadLine();
      MessageBox.ShowMessageBox(MessageBoxFlags.Info, "Info", "This should be info box");
      int s = MessageBox.ShowMessageBoxEx(MessageBoxFlags.Warning, "Warning", "This should be warning box",
        new string[] {"0", "1", "2"});
      Console.WriteLine("Button pressed: {0}", s);
      MessageBox.ShowMessageBox(MessageBoxFlags.Error, "Error", "This should be error box");
      Console.WriteLine("Press enter to launch debug window");
      Console.ReadLine();
      app.RegisterWindow(new TestWindow1());
      app.RunWindowLoop();
      app.Dispose();
    }
  }
}
