using System;
using System.Text;

namespace RazorWare.CoreDL.Internals {
   using RazorWare.Geometry;
   using RazorWare.CoreDL.Core;

   internal sealed class NativeWindow : ISDLHwnd, IDisposable {
      private readonly byte[] title;
      private readonly SDL_WINDOW winFlags;
      private readonly uint sdlSystem = SDL_INIT.VIDEO;

      private IntPtr nativePointer;
      private Location<int> location;
      private Size<int> size;

      internal static Encoding Encoder => Constants.Encoder;

      uint ISDLHwnd.SdlSystem => sdlSystem;

      internal NativeWindow(string windowTitle, int locX=SDL_WINDOWPOS.CENTERED, int locY=SDL_WINDOWPOS.CENTERED, int width=800, int height=600, SDL_WINDOW windowFlags=SDL_WINDOW.OPENGL) {
         title = Encoder.GetBytes(windowTitle);
         winFlags = windowFlags;
         location = new Location<int>(locX, locY);
         size = new Size<int>(width, height);
      }

      void ISDLHwnd.Start( ) {
         if (SDLI.SDL_WasInit(sdlSystem) == sdlSystem) {
            nativePointer = SDLI.SDL_CreateWindow(title, location.X, location.Y, size.Width, size.Height, winFlags);
         }
      }

      #region IDisposable Support
      private bool isDisposed = false;

      private void Dispose(bool disposing) {
         if (!isDisposed) {
            if (disposing) {
               // TODO: dispose managed state (managed objects).
            }

            // free unmanaged resources (unmanaged objects) and override a finalizer below.
            SDLI.SDL_DestroyWindow(nativePointer);

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
