using System;
using System.Collections.Generic;
using System.Text;

namespace SDLCore.EventArgs
{
  public struct DataDrop
  {
    public bool IsFile { get; private set; }
    public string Data { get; private set; }
    public DataDrop(bool isFile, string data) {
      IsFile = isFile;
      Data = data;
    }
  }
}
