using Microsoft.VisualStudio.TestTools.UnitTesting;
using RazorWare.Collections.Specialized;

namespace RazorWare.Toolbox.Testing.Collections.Specialized {
   [TestClass]
   public class BufferStackTests {
      private readonly byte[][] source =
      {
            new byte[] {123, 124, 125, 126},
            new byte[] {127, 128, 129, 130, 131}
        };

      [TestMethod]
      public void ExecuteMoveNext( ) {
         IBufferStack stack = new BufferStack(source);

         //  MoveNext will position the stack at first array, first element
         Assert.IsTrue(stack.MoveNext(), "failed first MoveNext");
         Assert.AreEqual(0, stack.Index, $"failed first Index [0]:[{stack.Index}]");
         Assert.AreEqual(0, stack.Element, $"failed first Element [0]:[{stack.Element}]");

         //  first array, 2nd element
         Assert.IsTrue(stack.MoveNext(), "failed second MoveNext");
         Assert.AreEqual(0, stack.Index, $"failed second Index [0]:[{stack.Index}]");
         Assert.AreEqual(1, stack.Element, $"failed second Element [1]:[{stack.Element}]");
      }

      [TestMethod]
      public void ExecuteNextBuffer( ) {
         IBufferStack stack = new BufferStack(source);

         //  NextBuffer will position the stack at the first array, first element
         Assert.IsTrue(stack.NextBuffer(), "failed first NextBuffer");
         Assert.AreEqual(0, stack.Index, $"failed first Index [0]:[{stack.Index}]");
         Assert.AreEqual(-1, stack.Element, $"failed first Element [-1]:[{stack.Element}]");

         //  second array, 2nd element
         Assert.IsTrue(stack.NextBuffer(), "failed second NextBuffer");
         Assert.AreEqual(1, stack.Index, $"failed second Index [0]:[{stack.Index}]");
         Assert.AreEqual(-1, stack.Element, $"failed second Element [-1]:[{stack.Element}]");
      }

      [TestMethod]
      public void FullTest( ) {
         var array = 0;
         var element = 0;
         IBufferStack stack = new BufferStack(source);

         while (stack.NextBuffer()) {
            Assert.AreEqual(array, stack.Index, $"failed Index [{array}]:[{stack.Index}]");
            Assert.AreEqual(-1, stack.Element,
                $"failed Element [{array},{element}]:[{stack.Index},{stack.Element}]");

            var expectedArray = source[array];
            CollectionAssert.AreEqual(expectedArray, stack.Current, $"failed collection [{array}]");

            while (stack.MoveNext()) {
               Assert.AreEqual(expectedArray[element], ((IByteStack)stack).Current,
                   $"failed Current [{expectedArray[element]}]:[{((IByteStack)stack).Current}]");

               ++element;
            }

            ++array;
            element = 0;
         }
      }
   }
}