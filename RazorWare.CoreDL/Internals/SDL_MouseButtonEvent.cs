using System;
using System.Runtime.InteropServices;

namespace RazorWare.CoreDL.Internals {
#pragma warning disable 0169
   /* Mouse button event structure (event.button.*) */
   [StructLayout(LayoutKind.Sequential)]
   internal struct SDL_MouseButtonEvent {
      public SDL_EVENTTYPE type;
      public UInt32 timestamp;
      public UInt32 windowID;
      public UInt32 which;
      public byte button; /* button id */
      public byte state; /* SDL_PRESSED or SDL_RELEASED */
      public byte clicks; /* 1 for single-click, 2 for double-click, etc. */
      private byte padding1;
      public Int32 x;
      public Int32 y;
   }
#pragma warning restore 0169
}
