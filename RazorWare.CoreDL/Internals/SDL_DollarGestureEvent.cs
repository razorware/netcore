using System;
using System.Runtime.InteropServices;

namespace RazorWare.CoreDL.Internals {
   [StructLayout(LayoutKind.Sequential)]
   internal struct SDL_DollarGestureEvent {
      public UInt32 type;
      public UInt32 timestamp;
      public Int64 touchId; // SDL_TouchID
      public Int64 gestureId; // SDL_GestureID
      public UInt32 numFingers;
      public float error;
      public float x;
      public float y;
   }
}
