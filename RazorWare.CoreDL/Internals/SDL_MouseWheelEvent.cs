using System;
using System.Runtime.InteropServices;

namespace RazorWare.CoreDL.Internals {
   /* Mouse wheel event structure (event.wheel.*) */
   [StructLayout(LayoutKind.Sequential)]
   internal struct SDL_MouseWheelEvent {
      public SDL_EVENTTYPE type;
      public UInt32 timestamp;
      public UInt32 windowID;
      public UInt32 which;
      public Int32 x; /* amount scrolled horizontally */
      public Int32 y; /* amount scrolled vertically */
      public UInt32 direction; /* Set to one of the SDL_MOUSEWHEEL_* defines */
   }
}
