using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RazorWare.Jargon.Testing {
   using RazorWare.Jargon.Infrastructure;

   [TestClass]
   public class NamedValueTests {

      [TestMethod]
      public void ConstructNamedValue( ) {
         var expInt = 25;

         var nv = new NamedValue<int>("Name");
         nv.Assign(expInt);

         int actInt = nv;

         Assert.AreEqual(expInt, actInt);
      }

      [TestMethod]
      public void FromValueTuple( ) {
         var valTuple = (name:"Foo", value:25);
         NamedValue<int> actValue = valTuple;

         Assert.AreEqual(valTuple.name, actValue.Name);
         Assert.AreEqual(valTuple.value, actValue);
      }

      [TestMethod]
      public void ToValueTuple( ) {
         var expInt = 25;

         var nv = new NamedValue<int>("Name");
         nv.Assign(expInt);

         (string name, int value) actTuple = nv;

         Assert.AreEqual(nv.Name, actTuple.name);
         Assert.AreEqual(nv.Value, actTuple.value);
      }

   }
}
