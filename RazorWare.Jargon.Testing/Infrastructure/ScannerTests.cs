using System;
using System.IO;
using System.Linq;
using System.Reflection;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RazorWare.Jargon.Testing {
   using RazorWare.Jargon.Domain;
   using RazorWare.Jargon.Interfaces;
   using RazorWare.Jargon.Infrastructure;

   [TestClass]
   public class ScannerTests {
      private static string rootDir = GlobalConstraints.RootDirectory;
      private static System.Text.Encoding encoder = GlobalConstraints.Encoder;

      [TestMethod]
      public void InitializeViaReflection( ) {
         var file = "jargon_sample.jss";
         var scanner = CreateRandomAccessScanner(file);

         Assert.IsNotNull(scanner, "Scanner is null");
      }

      [TestMethod]
      public void ScanNext( ) {
         var file = "jargon_0.jss";
         var scanner = CreateRandomAccessScanner(file);
         var expBuffer = encoder.GetBytes("Window {}");
         var actBuffer = new byte[expBuffer.Length];

         var i = 0;
         while (scanner.Next()) {
            actBuffer[i] = scanner.Current;

            ++i;
         }

         CollectionAssert.AreEqual(expBuffer, actBuffer);
      }

      [TestMethod]
      public void ScanPrevious( ) {
         var file = "jargon_0.jss";
         var scanner = CreateRandomAccessScanner(file);
         var expBuffer = encoder.GetBytes("Window {}");
         expBuffer = expBuffer.Reverse().ToArray();

         var actBuffer = new byte[expBuffer.Length];

         while (scanner.Next()) { }

         Assert.AreEqual(expBuffer.Length, scanner.Length);

         var i = 0;
         // already at the last position
         actBuffer[i] = scanner.Current;
         ++i;
         while (scanner.Previous()) {
            actBuffer[i] = scanner.Current;

            ++i;
         }

         CollectionAssert.AreEqual(expBuffer, actBuffer);
      }

      [TestMethod]
      public void ScanPeekWithBuffer( ) {
         var file = "jargon_0.jss";
         var scanner = CreateRandomAccessScanner(file);
         var skip = 3;
         var length = 2;
         var expBuffer = encoder.GetBytes("Window {}")
            .Skip(skip)
            .Take(length)
            .ToArray();
         var actBuffer = new byte[length];

         Assert.IsTrue(scanner.Next());

         var start = scanner.Position;
         // ensure scanner position is 0
         Assert.AreEqual(0, start);

         scanner.Peek(actBuffer, skip);

         // ensure Peek has not moved position
         Assert.AreEqual(start, scanner.Position);
         CollectionAssert.AreEqual(expBuffer, actBuffer);
      }

      [TestMethod]
      public void ScanPeekSingleByte( ) {
         var file = "jargon_0.jss";
         var scanner = CreateRandomAccessScanner(file);
         var skip = 3;
         var expByte = encoder.GetBytes("Window {}")
            .Skip(skip)
            .Take(1)
            .First();
         var actByte = default(byte);

         Assert.IsTrue(scanner.Next());

         var start = scanner.Position;
         // ensure scanner position is 0
         Assert.AreEqual(0, start);

         actByte = scanner.Peek(skip);

         // ensure Peek has not moved position
         Assert.AreEqual(start, scanner.Position);
         Assert.AreEqual(expByte, actByte);
      }

      private static IRandomAccessScanner CreateRandomAccessScanner(string file) {
         var filePath = Path.Combine(rootDir, file);
         FileObject fo = new FileObject(filePath);

         var parserType = typeof(Parser);
         var nested = parserType.GetNestedType("Scanner", BindingFlags.NonPublic);

         Assert.IsNotNull(nested, "nested (Scanner) type is null");

         var ctor = nested.GetConstructor(BindingFlags.NonPublic | BindingFlags.Instance, null, new Type[] { typeof(IFileObject) }, null);

         Assert.IsNotNull(ctor, "Scanner ctor is null");

         return (IRandomAccessScanner)ctor.Invoke(new[] { fo });
      }
   }
}
