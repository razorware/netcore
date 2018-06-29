using System;
using System.Threading;

namespace RazorWare.CoreDL.Testing.CreateEventPump {
   using RazorWare.CoreDL.Internals;

   class Program {
      const int MAX_LOOPS = 80;
      static readonly TimeSpan SLEEPSPAN = new TimeSpan(1500000L);

      static void Main(string[] args) {
         Console.WriteLine("Hello EventPump!");

         var eventPump = EventPump.Instance;

         Console.WriteLine($"EventPump primed: {eventPump.IsInitialized}");

         eventPump.Start();

         Console.WriteLine($"EventPump running: {eventPump.IsRunning}");

         var loopCount = 0;
         while (eventPump.IsRunning) {
            Console.Write(".");
            Thread.Sleep(SLEEPSPAN);

            ++loopCount;

            if (loopCount == MAX_LOOPS) {
               eventPump.Stop();
               Console.WriteLine();
            }
         }

         if (loopCount == 0) {
            Console.WriteLine("EventPump failed to start");
         }
         else {
            Console.WriteLine($"EventPump stopped: {!eventPump.IsRunning}");
         }

         eventPump.Dispose();

         Console.WriteLine();
         Console.Write("Press any key to exit");
         Console.ReadKey();
      }
   }
}
