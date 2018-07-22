namespace RazorWare.CoreDL.Core {
   using RazorWare.CoreDL.Internals;

   public enum WindowState :uint {
      Windowed = 0,                             // default
      FullScreen = SDL_WINDOW.FULLSCREEN,
      Desktop = SDL_WINDOW.FULLSCREEN_DESKTOP,
      Maximized = SDL_WINDOW.MAXIMIZED
   }
}
