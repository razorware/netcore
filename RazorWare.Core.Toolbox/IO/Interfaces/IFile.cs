using System;

namespace RazorWare.IO {
   public interface IFile : IDisposable {
      string Directory { get; }
      bool Exists { get; }
      string Extension { get; }
      string FullName { get; }
      bool IsOpen { get; }
      long Length { get; }
      string Name { get; }
      string Path { get; }
      long Position { get; set; }

      byte Read( );
      byte[] Read(long count);
      void Write(byte[] buffer);
      void Truncate( );
   }
}
