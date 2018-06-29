using System;
using System.Runtime.InteropServices;

namespace RazorWare.CoreDL.Internals {
   [StructLayout(LayoutKind.Sequential)]
   internal struct SDL_GenericEvent {
      public SDL_EVENTTYPE type;
      public UInt32 timestamp;
   }
}
