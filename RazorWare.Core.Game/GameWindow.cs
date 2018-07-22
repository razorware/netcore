using System.Drawing;

namespace RazorWare.Core.Media {
   using RazorWare.CoreDL.Core;
   using RazorWare.CoreDL.Internals;

   public abstract class GameWindow : BaseWindow {
      public Color Background { get; set; }

      protected GameWindow(int width, int height, string title = "GameWindow", int left = SDL_WINDOWPOS.CENTERED, int top = SDL_WINDOWPOS.CENTERED)
         : base(title, width, height, left, top) {
         OnError += OnWindowError;
      }

      protected override void OnInitialized( ) {
         NativeWindow.SetBackground(Background.R, Background.G, Background.B, Background.A);
      }

      protected virtual void OnWindowError(string Message) { }

      protected override void OnInitializing( ) { }

      protected override void OnResized( ) { }

      public void Show( ) {
         EventPump.Instance.Start(((INativeWindow)this).GetNativeHandle());
      }

      public void Close() {
         EventPump.Instance.Stop();
         EventPump.Instance.Dispose();
      }
   }
}
