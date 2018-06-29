using System;
using System.Runtime.InteropServices;

namespace RazorWare.CoreDL.Internals {
   /* A video driver dependent event (event.syswm.*), disabled */
   [StructLayout(LayoutKind.Sequential)]
   internal struct SDL_SysWMEvent {
      public SDL_EVENTTYPE type;
      public UInt32 timestamp;
      public IntPtr msg; /* SDL_SysWMmsg*, system-dependent*/
   }
}
