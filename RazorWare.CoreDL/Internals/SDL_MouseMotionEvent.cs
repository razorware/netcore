using System;
using System.Runtime.InteropServices;

namespace RazorWare.CoreDL.Internals {
#pragma warning disable 0169
   /* Mouse motion event structure (event.motion.*) */
   [StructLayout(LayoutKind.Sequential)]
   internal struct SDL_MouseMotionEvent {
      public SDL_EVENTTYPE type;
      public UInt32 timestamp;
      public UInt32 windowID;
      public UInt32 which;
      public byte state; /* bitmask of buttons */
      private byte padding1;
      private byte padding2;
      private byte padding3;
      public Int32 x;
      public Int32 y;
      public Int32 xrel;
      public Int32 yrel;
   }
#pragma warning restore 0169
}
