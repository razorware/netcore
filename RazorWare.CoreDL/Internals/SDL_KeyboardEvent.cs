using System;
using System.Runtime.InteropServices;

namespace RazorWare.CoreDL.Internals
{
   // Ignore private members used for padding in this struct
#pragma warning disable 0169
   /* Keyboard button event structure (event.key.*) */
   [StructLayout(LayoutKind.Sequential)]
   internal struct SDL_KeyboardEvent {
      public SDL_EVENTTYPE type;
      public UInt32 timestamp;
      public UInt32 windowID;
      public byte state;
      public byte repeat; /* non-zero if this is a repeat */
      private byte padding2;
      private byte padding3;
      public SDL_Keysym keysym;
   }
#pragma warning restore 0169
}
