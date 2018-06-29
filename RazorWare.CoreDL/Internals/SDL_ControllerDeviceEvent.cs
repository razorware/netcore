using System;
using System.Runtime.InteropServices;

namespace RazorWare.CoreDL.Internals {
   /* Game controller device event (event.cdevice.*) */
   [StructLayout(LayoutKind.Sequential)]
   internal struct SDL_ControllerDeviceEvent {
      public SDL_EVENTTYPE type;
      public UInt32 timestamp;
      public Int32 id;
   }
}
