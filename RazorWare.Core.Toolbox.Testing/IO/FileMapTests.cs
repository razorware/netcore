using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RazorWare.IO;

namespace RazorWare.Toolbox.Testing.IO {
   /// <summary>
   /// The FileMap handles file requests - Get, Read, etc. - and monitors file status (open/close)
   /// </summary>
   [TestClass]
   [DeploymentItem(@"\_items\source_text.txt", @".\_items\")]
   public class FileMapTests {
      private const string TextFile = @".\_items\source_text.txt";

      private readonly FileInfo mFile = new FileInfo(TextFile);
      private readonly FileMap fMap = FileMap.Instance;

      [TestMethod]
      public void GetFile( ) {
         var fileExists = mFile.Exists;
         Assert.IsTrue(fileExists, $"{mFile.Name} !Exist");

         var file = fMap.Get(TextFile);

         Assert.AreEqual(mFile.Name, file.Name);
         Assert.AreEqual(fileExists, file.Exists);
      }

      [TestMethod]
      public void FileDirectory( ) {
         var directory = string.Empty;
         var path = string.Empty;

         using (var file = fMap.Get(TextFile)) {
            directory = file.Directory;
            path = file.Path;
         }

         Assert.AreEqual(mFile.Directory.Name, directory);
         Assert.AreEqual(mFile.DirectoryName, path);
      }

      [TestMethod]
      public void FileIsOpen( ) {
         IFile file;

         using (file = fMap.Get(TextFile)) {
            Assert.IsFalse(file.IsOpen);
         }

         //  test re-opening
         using (file = fMap.Get(TextFile)) {
            Assert.IsFalse(file.IsOpen);
         }

         Assert.IsFalse(fMap.VerifyIsOpen(file));
      }

      [TestMethod]
      public void ReadFileBytes( ) {
         IFile file;
         var buffer = default(byte[]);
         var bytes = default(byte);

         using (file = fMap.Get(TextFile)) {
            file.Position = 0;
            buffer = file.Read(10);
         }

         Assert.AreEqual(10, buffer.Length);
         Assert.AreEqual(10, file.Position);

         --file.Position;
         using (file) {
            bytes = file.Read();
         }

         Assert.AreEqual(buffer[buffer.Length - 1], bytes);
         Assert.AreEqual(10, file.Position);
      }

      [TestMethod]
      public void ReadFileLength( ) {
         IFile file;
         var buffer = default(byte[]);
         var bytes = default(byte);
         long length;

         using (file = fMap.Get(TextFile)) {
            file.Position = 0;
            length = file.Length;
            buffer = file.Read(length);
         }

         Assert.AreEqual(length, buffer.Length);
         Assert.AreEqual(length, file.Position);

         --file.Position;
         using (file) {
            bytes = file.Read();
         }

         Assert.AreEqual(buffer[buffer.Length - 1], bytes);
      }

      [TestMethod]
      public void GetFileEvents( ) {
         IFile file;
         var buffer = default(byte[]);
         var bytes = default(byte);
         long length;

         IFileEvents fileEvents = null;

         using (file = fMap.Get(TextFile)) {
            fileEvents = FileMap.GetFileEvents(file);

            file.Position = 0;
            length = file.Length;
            buffer = file.Read(length);
         }

         int eventCount = 0;
         fileEvents.Opened += (f) => {
            ++eventCount;
         };
         fileEvents.Closed += (f) => {
            ++eventCount;
         };

         --file.Position;
         using (file) {
            bytes = file.Read();
         }

         Assert.AreEqual(2, eventCount);
      }
   }
}
