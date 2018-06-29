using System;
using System.Runtime.InteropServices;

namespace RazorWare.CoreDL.Internals {
#pragma warning disable 0169
   /* Game controller button event (event.cbutton.*) */
   [StructLayout(LayoutKind.Sequential)]
   internal struct SDL_ControllerButtonEvent {
      public SDL_EVENTTYPE type;
      public UInt32 timestamp;
      public Int32 which; /* SDL_JoystickID */
      public byte button;
      public byte state;
      private byte padding1;
      private byte padding2;
   }
#pragma warning restore 0169
}
