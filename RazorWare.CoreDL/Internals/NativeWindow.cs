using System;
using System.Text;

namespace RazorWare.CoreDL.Internals {
   using RazorWare.CoreDL.Core;

   internal sealed class NativeWindow : IDisposable {
      private readonly IntPtr nativeHandle;

      internal static Encoding Encoder => Constants.Encoder;

      internal NativeWindow(string WindowTitle, int locX=SDL_WINDOWPOS.CENTERED, int locY=SDL_WINDOWPOS.CENTERED, int width=800, int height=600, SDL_WINDOW windowFlags=SDL_WINDOW.OPENGL) {
         nativeHandle = SDLI.SDL_CreateWindow(Encoder.GetBytes(WindowTitle), locX, locY, width, height, windowFlags);
      }

      #region IDisposable Support
      private bool isDisposed = false;

      private void Dispose(bool disposing) {
         if (!isDisposed) {
            if (disposing) {
               // TODO: dispose managed state (managed objects).
            }

            // free unmanaged resources (unmanaged objects) and override a finalizer below.
            SDLI.SDL_DestroyWindow(nativeHandle);

            // TODO: set large fields to null.

            isDisposed = true;
         }
      }

      // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
      ~NativeWindow( ) {
         // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
         Dispose(false);
      }

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
