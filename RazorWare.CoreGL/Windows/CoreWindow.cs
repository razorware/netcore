using System;

namespace RazorWare.CoreGL.Windows {
   using RazorWare.CoreGL.Graphics.Core;

   internal sealed class CoreWindow : IDisposable {

      public int Width { get; internal set; }
      public int Height { get; internal set; }

      internal CoreWindow(int width, int height, string title = "NativeWindow", GraphicsMode grMode = GraphicsMode.Default) { }

      public void Close() { }

      public void Run(double FrameRate) { }


      //protected virtual void OnLoad(EventArgs args) { }

      //protected virtual void OnResize(EventArgs args) { }

      //protected virtual void OnUpdateFrame(FrameEventArgs args) { }

      //protected virtual void OnRenderFrame(FrameEventArgs args) { }
      
      #region IDisposable Support
      private bool disposedValue = false; // To detect redundant calls

      void Dispose(bool disposing) {
         if (!disposedValue) {
            if (disposing) {
               // TODO: dispose managed state (managed objects).
            }

            // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
            // TODO: set large fields to null.

            disposedValue = true;
         }
      }

      // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
      // ~CoreWindow() {
      //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
      //   Dispose(false);
      // }

      // This code added to correctly implement the disposable pattern.
      public void Dispose( ) {
         // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
         Dispose(true);
         // TODO: uncomment the following line if the finalizer is overridden above.
         // GC.SuppressFinalize(this);
      }
      #endregion
   }
}
