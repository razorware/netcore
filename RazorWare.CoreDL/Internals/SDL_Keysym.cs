using System;
using System.Runtime.InteropServices;

namespace RazorWare.CoreDL.Internals
{
   [StructLayout(LayoutKind.Sequential)]
   internal struct SDL_Keysym {
      public SDL_SCANCODE scancode;
      public SDL_KEYCODE sym;
      public SDL_KEYMOD mod; /* UInt16 */
      private UInt32 unicode; /* Deprecated - kept for proper alignment (???) */
   }
}
