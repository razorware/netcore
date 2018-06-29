using System;

namespace RazorWare.CoreDL.Internals
{
   [Flags]
   internal enum SDL_KEYMOD : ushort {
      NONE = 0x0000,
      LSHIFT = 0x0001,
      RSHIFT = 0x0002,
      LCTRL = 0x0040,
      RCTRL = 0x0080,
      LALT = 0x0100,
      RALT = 0x0200,
      LGUI = 0x0400,
      RGUI = 0x0800,
      NUM = 0x1000,
      CAPS = 0x2000,
      MODE = 0x4000,
      RESERVED = 0x8000,

      /* These are defines in the SDL headers */
      CTRL = (LCTRL | RCTRL),
      SHIFT = (LSHIFT | RSHIFT),
      ALT = (LALT | RALT),
      GUI = (LGUI | RGUI)
   }
}
