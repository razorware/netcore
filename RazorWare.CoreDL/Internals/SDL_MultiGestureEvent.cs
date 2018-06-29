using System;
using System.Runtime.InteropServices;

namespace RazorWare.CoreDL.Internals {
   [StructLayout(LayoutKind.Sequential)]
   internal struct SDL_MultiGestureEvent {
      public UInt32 type;
      public UInt32 timestamp;
      public Int64 touchId; // SDL_TouchID
      public float dTheta;
      public float dDist;
      public float x;
      public float y;
      public UInt16 numFingers;
      public UInt16 padding;
   }
}
