using System;
using System.Runtime.InteropServices;

namespace RazorWare.CoreDL.Internals {
#pragma warning disable 0169
   /* Joystick axis motion event structure (event.jaxis.*) */
   [StructLayout(LayoutKind.Sequential)]
   internal struct SDL_JoyAxisEvent {
      public SDL_EVENTTYPE type;
      public UInt32 timestamp;
      public Int32 which; /* SDL_JoystickID */
      public byte axis;
      private byte padding1;
      private byte padding2;
      private byte padding3;
      public Int16 axisValue; /* value, lolC# */
      public UInt16 padding4;
   }
#pragma warning restore 0169
}
