using System;
using System.Collections.Generic;
using System.Text;

namespace SDLCore
{
  // Code is a port of SDL_gfx to C#
  // SDL_gfx is licensed under the zlib license
  internal class GFXPort
  {
    public struct Color
    {
      public byte R { get; private set; }
      public byte G { get; private set; }
      public byte B { get; private set; }
      public Color(byte r, byte g, byte b)
      {
        R = r;
        G = g;
        B = b;
      }
    }
    public static void RoundedRect(IntPtr renderer, int x1, int y1, int x2, int y2, int radius, Color col) {
      //TODO port
    }

    public static void FillPolygon(IntPtr renderer, int[] xs, int[] ys, Color col)
    {
      //TODO port
    }

    public static void Line(IntPtr renderer, int x1, int y1, int x2, int y2, Color col, double thickness)
    {
      //TODO port
    }

    public static void FillEllipse(IntPtr renderer, int x, int y, int w, int h, Color col)
    {
      //TODO port
    }

    public static void EllipseAA(IntPtr renderer, int x, int y, int w, int h, Color col)
    {
      //TODO port
    } 

    //TODO Arcs and Beziers
  }
}
