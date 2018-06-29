using System;
using System.Runtime.InteropServices;

namespace RazorWare.CoreDL.Internals
{
   [StructLayout(LayoutKind.Sequential)]
   internal struct SDL_TouchFingerEvent {
      public UInt32 type;
      public UInt32 timestamp;
      public Int64 touchId; // SDL_TouchID
      public Int64 fingerId; // SDL_GestureID
      public float x;
      public float y;
      public float dx;
      public float dy;
      public float pressure;
   }

}
