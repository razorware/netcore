using System;
using System.Runtime.InteropServices;

namespace RazorWare.CoreDL.Internals {
#pragma warning disable 0169
   /* Game controller axis motion event (event.caxis.*) */
   [StructLayout(LayoutKind.Sequential)]
   internal struct SDL_ControllerAxisEvent {
      public SDL_EVENTTYPE type;
      public UInt32 timestamp;
      public Int32 which; /* SDL_JoystickID */
      public byte axis;
      private byte padding1;
      private byte padding2;
      private byte padding3;
      public Int16 axisValue; /* value, lolC# */
      private UInt16 padding4;
   }
#pragma warning restore 0169
}
