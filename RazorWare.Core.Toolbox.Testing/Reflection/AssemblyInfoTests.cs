using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using RazorWare.IO;
using RazorWare.Reflection;

namespace RazorWare.Toolbox.Testing {
   [TestClass]
   public class AssemblyInfoTests {
      private Assembly testAssembly;

      [TestInitialize]
      public void InitializeTest( ) {
         testAssembly = Assembly.LoadFile(Path.Combine(Directory.GetCurrentDirectory(), @"_items\RazorWare.NetDb.dll"));
      }

      [TestMethod]
      public void InitializeAssemblyInfo( ) {
         var expAsm = "RzWare.NetDb";
         var asmInfo = new AssemblyInfo(expAsm);

         Assert.AreEqual(expAsm, asmInfo.Name);
      }

      [TestMethod]
      public void GetTypesAssignableFrom( ) {
         var asmInfo = new AssemblyInfo("RazorWare.Core");
         var fileTypes = asmInfo.TypesDescendingFrom(typeof(IFile));

         // there is at least 1 (File)
         Assert.IsTrue(fileTypes.Length == 1);
         Assert.AreEqual("File", fileTypes[0].Name);
      }
   }
}
