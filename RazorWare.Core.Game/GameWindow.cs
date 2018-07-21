using System.Drawing;

namespace RazorWare.Core.Media {
   using RazorWare.CoreDL.Core;
   using RazorWare.CoreDL.Internals;

   public abstract class GameWindow : BaseWindow {
      public Color Background { get; set; }

      protected GameWindow(int width, int height, string title = "GameWindow", int left = SDL_WINDOWPOS.CENTERED, int top = SDL_WINDOWPOS.CENTERED)
         : base(title, width, height, left, top) {
      }

      protected override void OnInitialized( ) {
         Native.SetBackground(Background.R, Background.G, Background.B, Background.A);
      }

      protected override void OnInitializing( ) {
         base.OnInitializing();
      }

      protected override void OnResized( ) {
         base.OnResized();
      }

      public void Show( ) {
         EventPump.Instance.Start(((INativeWindow)this).GetHwndDevice());
      }

      public void Close() {
         EventPump.Instance.Stop();
         EventPump.Instance.Dispose();
      }
   }
}
