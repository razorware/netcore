using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RazorWare.CoreDL.Testing {
   using RazorWare.CoreDL.Core;

   [TestClass]
   public class PlatformTests {

      [TestMethod]
      public void WinPlatform( ) {
         Assert.AreEqual("Windows", Platform.Instance.OperatingSystem);
      }

   }
}
