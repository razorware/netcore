using System;
using System.IO;

namespace RazorWare.IO {
   public class FileBuffer : FileStream {
      private bool isDisposing;

      public event EventHandler Disposing;

      public FileBuffer(string path, FileMode mode, FileAccess access, FileShare share) :
         base(path, mode, access, share) { }

      protected override void Dispose(bool disposing) {
         if (disposing & !isDisposing) {
            isDisposing = true;
            Disposing?.Invoke(null, EventArgs.Empty);

            base.Dispose(disposing);
         }
      }
   }
}
