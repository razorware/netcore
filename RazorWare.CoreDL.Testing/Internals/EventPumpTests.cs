using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RazorWare.CoreDL.Testing {
   using System.Threading;
   using RazorWare.CoreDL.Internals;

   [TestClass]
   public class EventPumpTests {
      private static EventPump eventPump = EventPump.Instance;

      [TestInitialize]
      public void InitializeTest( ) { }

      [ClassCleanup]
      public static void CleanupTest( ) {
         eventPump.Dispose();
      }

      [TestMethod]
      public void ConstructedEventPump( ) {
         Assert.IsNotNull(eventPump);
         Assert.IsTrue(eventPump.IsInitialized);
         Assert.IsFalse(eventPump.IsRunning);
      }

      [TestMethod]
      public void StartStopEventPump( ) {
         eventPump.Start();

         Assert.IsTrue(eventPump.IsRunning);

         // let it run
         Thread.Sleep(500);

         eventPump.Stop();

         Assert.IsFalse(eventPump.IsRunning);
      }
   }
}
