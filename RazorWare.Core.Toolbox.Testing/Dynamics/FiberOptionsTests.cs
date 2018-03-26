using Microsoft.VisualStudio.TestTools.UnitTesting;

using RazorWare.Testing._classes;

using RazorWare.Dynamics;

namespace RazorWare.Toolbox.Testing {
   [TestClass]
   public class FiberOptionsTests {

      [TestMethod]
      public void ConstructFiberOptions( ) {
         var fiberOptions = new FiberOptions();

         Assert.IsNotNull(fiberOptions);
      }

      [TestMethod]
      public void FiberOptionsTarget( ) {
         var expectedType = typeof(Customer);
         var fiberOptions = new FiberOptions();
         fiberOptions.SetTarget(expectedType);

         var target = fiberOptions.Target;

         Assert.IsNotNull(target);
         Assert.IsInstanceOfType(target, expectedType);
      }
   }
}
