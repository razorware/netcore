using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RazorWare.Linq;

namespace RazorWare.Toolbox.Testing {
   [TestClass]
   public class TimeStampTests {

      [TestMethod]
      public void InitializeTimeStamp( ) {
         var now = DateTime.Now;
         TimeStamp timeStamp = now;

         var expString = ToByteString(BitConverter.GetBytes(now.Ticks));
         var actString = timeStamp.ToString("B");

         Assert.AreEqual(expString, actString);
      }

      [TestMethod]
      public void ParseFromStringBytes( ) {
         var now = DateTime.Now;
         TimeStamp expTimeStamp = now;

         var actTimeStamp = TimeStamp.FromBytes(BitConverter.GetBytes(now.Ticks));
         
         Assert.AreEqual(expTimeStamp, actTimeStamp);
      }

      private string ToByteString(byte[] buffer) {
         var value = buffer
            .ToArray(b => $"{b:X2}");

         return $"{string.Join(string.Empty, value)}";
      }
   }
}
