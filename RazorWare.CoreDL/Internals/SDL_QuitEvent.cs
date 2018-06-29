using System;
using System.Runtime.InteropServices;

namespace RazorWare.CoreDL.Internals {
   /* The "quit requested" event */
   [StructLayout(LayoutKind.Sequential)]
   internal struct SDL_QuitEvent {
      public SDL_EVENTTYPE type;
      public UInt32 timestamp;
   }
}
