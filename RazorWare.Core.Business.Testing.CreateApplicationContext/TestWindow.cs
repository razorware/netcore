using System.Drawing;

namespace RazorWare.Core.Business.Testing.CreateApplicationContext {
   using RazorWare.CoreDL.Core;
   using RazorWare.Core.Business.Forms;

   public partial class TestWindow : Window {

      public TestWindow(string title, int width, int height, int left, int top)
         : base(width, height, title, left, top) { }
      public TestWindow(string title = "DefaultWindow", WindowStyles style = WindowStyles.Default | WindowStyles.Resizable)
         : base(title: title) {
         Style = style;
         Background = Color.CornflowerBlue;
      }

   }
}
