using Microsoft.VisualStudio.TestTools.UnitTesting;

using RazorWare.Testing._classes;

using RazorWare.Dynamics;
using static RazorWare.Dynamics.DynamicsExtensions;

namespace RazorWare.Toolbox.Testing {
   [TestClass]
   public class MutatorTests {

      [TestMethod]
      public void GetMutatorDelegate( ) {
         var mutator = GetMutator<Customer>();

         Assert.IsNotNull(mutator);
         Assert.IsInstanceOfType(mutator, typeof(System.Delegate));
      }

      [TestMethod]
      public void CreateProxy( ) {
         var mutator = GetMutator<Customer>();
         var proxy = mutator.Mutate();

         Assert.IsTrue(proxy.IsCarbonProxy());
      }
   }
}
