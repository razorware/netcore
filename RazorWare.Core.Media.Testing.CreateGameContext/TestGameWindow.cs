using System.Drawing;

namespace RazorWare.Core.Media.Testing.CreateGameContext {
   using RazorWare.CoreDL.Core;

   public partial class TestGameWindow : GameWindow {

      public TestGameWindow(string title = "TestGameWindow", WindowStyles style = WindowStyles.Default) : base(800, 600, title) {
         Style = style;
         Background = Color.CornflowerBlue;
      }

      public void Dispose(bool withEventPump) {
         if (withEventPump) {
            Close();
         }

         Dispose();
      }
   }
}
