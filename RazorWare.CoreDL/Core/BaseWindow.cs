namespace RazorWare.CoreDL.Core {
   using System;
   using RazorWare.CoreDL.Internals;

   public abstract class BaseWindow : INativeWindow {
      private WindowStyles styles;

      internal NativeWindow Native { get; }

      public int Top => Native.Location.Y;
      public int Left => Native.Location.X;
      public int Width => Native.Size.Width;
      public int Height => Native.Size.Height;
      public string Title => Native.Name;
      public bool IsInitialized { get; private set; }
      public WindowStyles Style {
         get {
            return styles;
         }
         set {
            if (!IsInitialized) {
               styles = value;
            }
         }
      }

      protected BaseWindow(string title, int width, int height, int left, int top) {
         Native = new NativeWindow(title, left, top, width, height);

         Native.OnConfigure(Initializing);
         Native.OnInitialized(Initialized);
         Native.OnResize(Resize);
      }

      protected virtual void OnInitializing( ) { }
      protected virtual void OnInitialized( ) { }
      protected virtual void OnResized( ) { }
      
      private void Initializing( ) {
         // Initializing handler
         OnInitializing();

         Native.Style = Style;

         // Initializing event
      }

      private void Initialized(bool isInitialized) {
         IsInitialized = isInitialized;

         // Initialized handler
         OnInitialized();

         // Initialized event
      }

      private void Resize( ) {
         // Resized handler
         OnResized();

         // Resized event
      }

      ISDLHwnd INativeWindow.GetHwndDevice( ) {
         return Native;
      }

      public void Dispose( ) {
         Native.Dispose();
      }
   }
}
