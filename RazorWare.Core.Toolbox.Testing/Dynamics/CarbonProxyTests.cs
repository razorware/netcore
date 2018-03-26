using Microsoft.VisualStudio.TestTools.UnitTesting;

using RazorWare.Dynamics;

using RazorWare.Testing._classes;

namespace RazorWare.Toolbox.Testing {
   [TestClass]
   public class CarbonProxyTests {

      [TestMethod]
      public void IsCarbonObjectFalse( ) {
         Assert.IsFalse(new Customer().IsCarbonProxy());
      }
   }
}
