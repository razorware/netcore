using System;

namespace RazorWare.CoreDL.Internals {
   [Flags]
   internal enum SDL_WINDOW : uint {
      FULLSCREEN = 0x00000001,
      OPENGL = 0x00000002,
      SHOWN = 0x00000004,
      HIDDEN = 0x00000008,
      BORDERLESS = 0x00000010,
      RESIZABLE = 0x00000020,
      MINIMIZED = 0x00000040,
      MAXIMIZED = 0x00000080,
      INPUT_GRABBED = 0x00000100,
      INPUT_FOCUS = 0x00000200,
      MOUSE_FOCUS = 0x00000400,
      FULLSCREEN_DESKTOP = FULLSCREEN | 0x00001000,
      FOREIGN = 0x00000800,
      ALLOW_HIGHDPI = 0x00002000, // 2.0.1
      MOUSE_CAPTURE = 0x00004000, // 2.0.4
      ALWAYS_ON_TOP = 0x00008000, // 2.0.5
      SKIP_TASKBAR = 0x00010000,  // 2.0.5
      UTILITY = 0x00020000,       // 2.0.5
      TOOLTIP = 0x00040000,       // 2.0.5
      POPUP_MENU = 0x00080000,    // 2.0.5
      VULKAN = 0x10000000,        // 2.0.6
   }

   internal class SDL_Window {
      internal enum EVENT : byte {
         NONE,
         SHOWN,
         HIDDEN,
         EXPOSED,
         MOVED,
         RESIZED,
         SIZE_CHANGED,
         MINIMIZED,
         MAXIMIZED,
         RESTORED,
         ENTER,
         LEAVE,
         FOCUS_GAINED,
         FOCUS_LOST,
         CLOSE,
         /* Available in 2.0.5 or higher */
         TAKE_FOCUS,
         HIT_TEST
      }
   }
}
