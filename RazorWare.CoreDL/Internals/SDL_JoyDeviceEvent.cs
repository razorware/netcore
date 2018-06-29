using System;
using System.Runtime.InteropServices;

namespace RazorWare.CoreDL.Internals {
   /* Joystick device event structure (event.jdevice.*) */
   [StructLayout(LayoutKind.Sequential)]
   internal struct SDL_JoyDeviceEvent {
      public SDL_EVENTTYPE type;
      public UInt32 timestamp;
      public Int32 which; /* SDL_JoystickID */
   }
}
