using System;
using System.Text;
using System.Drawing;
using System.Collections.Generic;

namespace RazorWare.Core.Business.Forms {
   using RazorWare.CoreDL.Core;
   using RazorWare.CoreDL.Internals;

   public class Frame : Window, IWindowFrame {
      private NativeWindow nativeWindow;

      public Size Size { get; private set; }
      public Point Location { get; private set; }
      public bool IsVisible { get; private set; }

      protected Frame(Window winTemplate) {
         Style = WindowStyles.Frameless;
         Background = Color.FromArgb(25, Color.LightGray);
         Size = new Size(winTemplate.Width, winTemplate.Height);
         Location = new Point(winTemplate.Left, winTemplate.Top);

         nativeWindow = new NativeWindow(string.Empty, Location.X, Location.Y, Size.Width, Size.Height, (SDL_WINDOW)Style);
         nativeWindow.Events.Add(EventType.MouseButtonUp);
         nativeWindow.Events.Add(EventType.MouseMotion);
      }

      public void Show( ) {
         Console.WriteLine("Showing Window Frame");
         IsVisible = NativeWindow.Instantiate(nativeWindow) != IntPtr.Zero;
      }

      public void Hide() {
         Console.WriteLine("Hiding Window Frame");
      }
   }
}
