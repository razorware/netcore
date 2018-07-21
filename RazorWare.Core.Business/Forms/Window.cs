using System.Drawing;

namespace RazorWare.Core.Business.Forms {
   using RazorWare.CoreDL.Core;

   public abstract class Window : BaseWindow {
      private WindowFrame frame;

      public Color Background { get; set; }
      public IWindowFrame Frame => GetWindowFrame();

      protected Window(int width = 500, int height = 300, string title = "DefaultWindow", int left = 10, int top = 35)
         : base(title, width, height, left, top) {
         Style = WindowStyles.Default;
      }

      protected override void OnInitialized( ) {
         Native.SetBackground(Background.R, Background.G, Background.B, Background.A);
      }

      protected override void OnResized( ) {
         // show window frame
         GetWindowFrame().Show();
      }

      private WindowFrame GetWindowFrame( ) {
         return (frame ?? (frame = new WindowFrame(this)));
      }
   }
}
