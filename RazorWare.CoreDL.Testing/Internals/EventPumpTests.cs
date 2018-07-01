using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RazorWare.CoreDL.Testing {
   using System.Threading;
   using System.Threading.Tasks;
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
         Assert.IsFalse(eventPump.IsRunning);
      }

      [TestMethod]
      public void StartStopEventPump( ) {
         ManualResetEventSlim signal = new ManualResetEventSlim(false);
         eventPump.EventPumpStateChanged += (arg) => {
            signal.Set();
         };

         Task task = Task.Factory.StartNew(() => eventPump.Start());
         signal.Wait();
         signal.Reset();

         Assert.IsTrue(eventPump.IsRunning);

         // let it run
         Thread.Sleep(500);

         eventPump.Stop();
         signal.Wait();

         Assert.IsFalse(eventPump.IsRunning);

         signal.Dispose();
         task.Dispose();
      }
   }
}
