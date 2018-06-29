using System;
using System.Runtime.InteropServices;

namespace RazorWare.CoreDL.Internals
{
   // Ignore private members used for padding in this struct
   //#pragma warning disable 0169
   /* Window state change event data (event.window.*) */
   [StructLayout(LayoutKind.Sequential)]
   internal struct SDL_WindowEvent {
      public SDL_EVENTTYPE type;
      public UInt32 timestamp;
      public UInt32 windowID;
      public SDL_Window.EVENT windowEvent; // event, lolC#
      private byte padding1;
      private byte padding2;
      private byte padding3;
      public Int32 data1;
      public Int32 data2;
   }
   //#pragma warning restore 0169
}
