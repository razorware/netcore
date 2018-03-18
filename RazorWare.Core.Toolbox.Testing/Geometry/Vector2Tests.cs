using Microsoft.VisualStudio.TestTools.UnitTesting;
using RazorWare.Geometry;

namespace RazorWare.Toolbox.Testing {
   [TestClass]
   public class Vector2Tests {

      [TestMethod]
      public void InitializeVector2( ) {
         var expX = 1D;
         var expY = 2D;
         var v2 = new Vector2(expX, expY);

         Assert.AreEqual(expX, v2.X);
         Assert.AreEqual(expY, v2.Y);
      }

      [TestMethod]
      public void InitializeFromTuple( ) {
         var expX = 1;
         var expY = 2;
         Vector2 v2 = (1, 2);

         Assert.AreEqual(expX, v2.X);
         Assert.AreEqual(expY, v2.Y);
      }

      [TestMethod]
      public void SumVector2s( ) {
         double origX1 = 1D, 
                origX2 = 2D, 
                origY1 = 1D, 
                origY2 = 2D;
         double expX = origX1 + origX2, 
                expY = origY1 + origY2;
         Vector2 v1 = (origX1, origY1);
         Vector2 v2 = (origX2, origY2);

         var v3 = v1 + v2;

         Assert.AreEqual(expX, v3.X);
         Assert.AreEqual(expY, v3.Y);
      }
   }
}
