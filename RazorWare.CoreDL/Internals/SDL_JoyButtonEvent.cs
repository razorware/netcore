using System;
using System.Runtime.InteropServices;

namespace RazorWare.CoreDL.Internals {
#pragma warning disable 0169
   /* Joystick button event structure (event.jbutton.*) */
   [StructLayout(LayoutKind.Sequential)]
   internal struct SDL_JoyButtonEvent {
      public SDL_EVENTTYPE type;
      public UInt32 timestamp;
      public Int32 which; /* SDL_JoystickID */
      public byte button;
      public byte state; /* SDL_PRESSED or SDL_RELEASED */
      private byte padding1;
      private byte padding2;
   }
#pragma warning restore 0169
}
