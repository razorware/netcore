namespace RazorWare.CoreDL.Core {
   using RazorWare.CoreDL.Internals;

   public abstract class BaseWindow : INativeWindow {
      protected delegate void Error(string Message);

      private WindowStyles styles;

      internal NativeWindow NativeWindow { get; }

      protected event Error OnError;
      
      public int Top => NativeWindow.Location.Y;
      public int Left => NativeWindow.Location.X;
      public int Width => NativeWindow.Size.Width;
      public int Height => NativeWindow.Size.Height;
      public string Title => NativeWindow.Name;
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
         NativeWindow = new NativeWindow(title, left, top, width, height);

         NativeWindow.OnConfigure(Initializing);
         NativeWindow.OnInitialized(Initialized);
         NativeWindow.OnResize(Resize);
      }

      protected void SetWindowState(WindowState state) {
         if (!NativeWindow.TrySetWindowState(state, out CharPointer msgPointer)) {
            OnError?.Invoke(msgPointer);
         }
      }

      protected virtual void OnInitializing( ) { }
      protected virtual void OnInitialized( ) { }
      protected virtual void OnResized( ) { }
      
      private void Initializing( ) {
         // Initializing handler
         OnInitializing();

         NativeWindow.Style = Style;

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

      ISDLNative INativeWindow.GetNativeHandle( ) {
         return NativeWindow;
      }

      public void Dispose( ) {
         NativeWindow.Dispose();
      }
   }
}
