namespace RazorWare.CoreDL.Core {
   using RazorWare.CoreDL.Internals;

   public enum WindowEvent : byte {
      None = SDL_Window.EVENT.NONE,
      Shown = SDL_Window.EVENT.SHOWN,
      Hidden = SDL_Window.EVENT.HIDDEN,
      Exposed = SDL_Window.EVENT.EXPOSED,
      Moved = SDL_Window.EVENT.MOVED,
      Resized = SDL_Window.EVENT.RESIZED,
      SizeChanged = SDL_Window.EVENT.SIZE_CHANGED,
      Minimized = SDL_Window.EVENT.MINIMIZED,
      Maximized = SDL_Window.EVENT.MAXIMIZED,
      Restored = SDL_Window.EVENT.RESTORED,
      Enter = SDL_Window.EVENT.ENTER,
      Leave = SDL_Window.EVENT.LEAVE,
      FocusGained = SDL_Window.EVENT.FOCUS_GAINED,
      FocusLost = SDL_Window.EVENT.FOCUS_LOST,
      Close = SDL_Window.EVENT.CLOSE,
      TakeFocus = SDL_Window.EVENT.TAKE_FOCUS,
      HitTest = SDL_Window.EVENT.HIT_TEST
   }
}
