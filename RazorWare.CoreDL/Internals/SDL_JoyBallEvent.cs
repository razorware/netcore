﻿using System;
using System.Runtime.InteropServices;

namespace RazorWare.CoreDL.Internals {
#pragma warning disable 0169
   /* Joystick trackball motion event structure (event.jball.*) */
   [StructLayout(LayoutKind.Sequential)]
   internal struct SDL_JoyBallEvent {
      public SDL_EVENTTYPE type;
      public UInt32 timestamp;
      public Int32 which; /* SDL_JoystickID */
      public byte ball;
      private byte padding1;
      private byte padding2;
      private byte padding3;
      public Int16 xrel;
      public Int16 yrel;
   }
#pragma warning restore 0169
}
