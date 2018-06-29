using System;
using System.Runtime.InteropServices;

namespace RazorWare.CoreDL.Internals {
   /* A user defined event (event.user.*) */
   [StructLayout(LayoutKind.Sequential)]
   internal struct SDL_UserEvent {
      public UInt32 type;
      public UInt32 timestamp;
      public UInt32 windowID;
      public Int32 code;
      public IntPtr data1; /* user-defined */
      public IntPtr data2; /* user-defined */
   }
}
