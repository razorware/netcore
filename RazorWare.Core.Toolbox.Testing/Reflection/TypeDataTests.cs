using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RazorWare.Reflection;
using RazorWare.Data.Comparers;

namespace RazorWare.Toolbox.Testing {
   [TestClass]
   public class TypeDataTests {

      [TestMethod]
      public void IntTypeData( ) {
         var value = 128;
         var typeData = value.GetType().GetTypeData();

         Assert.AreEqual(TypeCode.Int32, typeData.TypeCode);
      }

      [TestMethod]
      public void DateTimeComparerFromTypeCode( ) {
         var date1 = new DateTime(1976, 7, 4, 11, 57, 0);
         var date2 = new DateTime(1977, 8, 15, 12, 32, 17);

         var typeData = date1.GetTypeCode();
         var comparer = typeData.GetComparer();

         var expResult = -1;
         var actResult = comparer.Compare(date1, date2);

         Assert.AreEqual(expResult, actResult);
      }
   }
}
