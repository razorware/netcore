using System;

namespace RazorWare.CoreDL.Core {
   using RazorWare.CoreDL.Internals;

   [Flags]
   public enum WindowStyles : uint {
      Default = SDL_WINDOW.OPENGL,
      Borderless = SDL_WINDOW.BORDERLESS,
      Resizable = SDL_WINDOW.RESIZABLE,
      Frameless = Default | Borderless | Resizable,
      MouseCapture = SDL_WINDOW.MOUSE_CAPTURE,
      MouseFocus = SDL_WINDOW.MOUSE_FOCUS
   }
}
