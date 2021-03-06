﻿using System;
using System.Runtime.InteropServices;

namespace RazorWare.CoreDL.Internals {
#pragma warning disable 0169
   /* Joystick hat position change event struct (event.jhat.*) */
   [StructLayout(LayoutKind.Sequential)]
   internal struct SDL_JoyHatEvent {
      public SDL_EVENTTYPE type;
      public UInt32 timestamp;
      public Int32 which; /* SDL_JoystickID */
      public byte hat; /* index of the hat */
      public byte hatValue; /* value, lolC# */
      private byte padding1;
      private byte padding2;
   }
#pragma warning restore 0169
}
