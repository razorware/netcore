using System;
using System.IO;
using SysFile = System.IO.File;

namespace RazorWare.IO {
   public abstract class FileHandler : IFile {
      private readonly string mFile;

      private FileStream fStream;
      private bool isDisposed;
      private DirectoryInfo fDirectory;
      private bool fExists;
      private string fExtension;
      private string fName;
      private long fLength;
      private long fPos;

      public string Directory => fDirectory.Name;
      public bool Exists => fExists;
      public string Extension => fExtension;
      public string FullName => mFile;
      public bool IsOpen => fStream?.CanSeek ?? false;
      public virtual long Length => fLength;
      public string Name => fName;
      public string Path => fDirectory.FullName;
      public virtual long Position {
         get { return fPos; }
         set { fPos = value; }
      }

      protected Stream FileStream {
         get { return fStream; }
         set { fStream = (FileStream)value; }
      }

      internal virtual event FileClosed Closed;
      internal virtual event FileOpened Opened;

      protected FileHandler(FileInfo file) {
         mFile = file.FullName;
         fName = file.Name;
         fDirectory = file.Directory;
         fExists = file.Exists;
         fExtension = file.Extension;

         // when the file does not exist, size cannot be 0
         fLength = fExists ? file.Length : -1;
      }

      public virtual byte Read( ) {
         var buffer = default(byte);
         using (Open(FileMode.Open, FileAccess.Read)) {
            fStream.Position = fPos;
            buffer = (byte)fStream.ReadByte();
            fPos = fStream.Position;
         }

         Close();

         return buffer;
      }

      public virtual byte[] Read(long count) {
         var buffer = new byte[count];
         using (Open(FileMode.Open, FileAccess.Read)) {
            fStream.Position = fPos;

            #region if file.Length is too large ...
            /* if (count > int.Max) {
             *
             *    int readCount = 0;
             *    while (readCount < count) {
             *       buffer[readCount] = Read();
             *       
             *       ++readCount;
             *    }
             * }
             * else {
             * ***/
            #endregion

            fStream.Read(buffer, (int)fPos, (int)count);
            /* }
             * ***/

            fPos = fStream.Position;
         }

         Close();

         return buffer;
      }

      public virtual void Write(byte[] buffer) {
         using (Open(FileMode.Open, FileAccess.Write)) {
            fStream.Position = fPos;
            fStream.Write(buffer, 0, buffer.Length);

            fPos = fStream.Position;
            fLength = fStream.Length;
         }

         Close();
      }

      internal void DeleteFile(string filePath) {
         SysFile.Delete(filePath);
      }

      internal virtual IDisposable Open(FileMode fileMode, FileAccess fileAccess) {
         if (IsOpen) {
            throw new InvalidOperationException($"File [{fStream?.Name}] is already open.");
         }

         fStream = new FileStream(mFile, fileMode, fileAccess, FileShare.Read);
         fLength = fStream.Length;

         isDisposed = false;
         Opened?.Invoke(this);

         return fStream;
      }

      public virtual void Truncate( ) {
         using (Open(FileMode.Truncate, FileAccess.Write)) {
            fPos = fStream.Position;
            fLength = fStream.Length;
         }
      }

      public virtual void Dispose( ) {
         if (!isDisposed && IsOpen) {
            Close();
         }

         isDisposed = true;
      }

      protected virtual void Close( ) {
         fStream?.Dispose();
         Closed?.Invoke(this);
      }
   }
}
