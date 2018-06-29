using System;
using System.Runtime.InteropServices;

namespace RazorWare.CoreDL.Internals {
   [StructLayout(LayoutKind.Sequential)]
   internal unsafe struct SDL_TextInputEvent {
      public SDL_EVENTTYPE type;
      public UInt32 timestamp;
      public UInt32 windowID;
      public fixed byte text[SDLI.SDL_TEXTINPUTEVENT_TEXT_SIZE];
   }
}
