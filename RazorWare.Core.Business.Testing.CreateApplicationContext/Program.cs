using System;
using System.Threading;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;

namespace RazorWare.Core.Business.Testing.CreateApplicationContext {
   using RazorWare.CoreDL.Core;

   class Program {
      private delegate void ProcessKey(ConsoleKeyInfo keyInfo);

      private static bool quit;
      private static Program p;
      private static ProcessKey processKey;

      private Application application;
      private Task task;
      private CancellationTokenSource cancel;

      static void Main(string[] args) {
         p = new Program();
         processKey = ProcessMainOptions;

         Console.WriteLine("Hello Application Context!");
         WriteMainOptions();

         while (!quit) {
            var input = Console.ReadKey();
            Console.WriteLine();

            // Q key kills program
            if (input.Key == ConsoleKey.Q) {
               quit = true;
               continue;
            }

            processKey(input);
         }
      }

      private static void ProcessMainOptions(ConsoleKeyInfo input) {
         if (input.Key == ConsoleKey.D1) {
            CreateEmptyApplicationContext();
         }
         else if (input.Key == ConsoleKey.D2) {
            processKey = ProcessWindowedOptions;
            WriteWindowedOptions();
         }
         else if (input.Key == ConsoleKey.X) {
            p.application?.Quit();
         }
         else {
            WriteMainOptions();
         }
      }

      private static void ProcessWindowedOptions(ConsoleKeyInfo input) {
         if (input.Key == ConsoleKey.D1) {
            CreateCustomWindow();
         }
         else if (input.Key == ConsoleKey.D2) {
            CreateResizableWindow();
         }
         else if (input.Key == ConsoleKey.M) {
            processKey = ProcessMainOptions;
            WriteMainOptions();
         }
         else if (input.Key == ConsoleKey.X) {
            p.application?.Quit();
         }
         else {
            WriteWindowedOptions();
         }
      }

      private static void WriteMainOptions( ) {
         Console.WriteLine();
         Console.WriteLine("[1] Start Empty Application Context");
         Console.WriteLine("[2] Windowed Application options");
         Console.WriteLine("___");
         Console.WriteLine("[X] Exit current Application Context");
         Console.WriteLine("___");
         Console.WriteLine("[Q] Quit");
      }

      private static void WriteWindowedOptions() {
         Console.WriteLine();
         Console.WriteLine("[1] Custom Windowed Context");
         Console.WriteLine("[2] Resizable Windowed Context");
         Console.WriteLine("___");
         Console.WriteLine("[M] Main menu");
         Console.WriteLine("___");
         Console.WriteLine("[X] Exit current Application Context");
         Console.WriteLine("___");
         Console.WriteLine("[Q] Quit");
      }

      private static void CreateEmptyApplicationContext( ) {
         if (p.application != null) {
            return;
         }

         SubscribeEvents(p.application = new Application("EmptyApplicationContext"));

         // empty application context
         // TODO: test whether synchronizaiton is necessary (Synchronizer/SyncContext)
         p.cancel = new CancellationTokenSource();
         p.task = Task.Factory.StartNew(( ) => {
            p.application.Run();
            p.cancel.Cancel();
            CleanUpTask(p.task.GetAwaiter());
            p.task.Dispose();
         }, p.cancel.Token);
      }

      private static void CreateCustomWindow( ) {
         if (p.application != null) {
            return;
         }

         SubscribeEvents(p.application = new Application("WindowedApplicationContext"));
         p.application.Run(new TestWindow("TestWindow", 800, 600, 75, 50));
      }

      private static void CreateResizableWindow( ) {
         if (p.application != null) {
            return;
         }

         SubscribeEvents(p.application = new Application());
         p.application.Run(new TestWindow());
      }

      private static void SubscribeEvents(Context context) {
         context.Start += OnApplicationStart;
         context.Closing += OnApplicationClosing;
         context.Exit += OnApplicationExit;
      }

      private static void OnApplicationStart( ) {
         Console.WriteLine($"{p.application.Name} started");
      }

      private static void OnApplicationClosing( ) {
         Console.WriteLine($"{p.application.Name} closing");
      }

      private static void OnApplicationExit( ) {
         Console.WriteLine($"{p.application.Name} quit");

         p.application.Start -= OnApplicationStart;
         p.application.Closing -= OnApplicationClosing;
         p.application.Exit -= OnApplicationExit;

         p.application.Dispose();
         p.application = null;
      }

      private static void CleanUpTask(TaskAwaiter taskAwaiter) {
         taskAwaiter.OnCompleted(( ) => {
            Console.WriteLine();

            var consoleBackground = Console.BackgroundColor;
            var consoleForeground = Console.ForegroundColor;

            Console.BackgroundColor = ConsoleColor.Red;
            if (p.cancel.IsCancellationRequested) {
               Console.WriteLine($"Task [{p.task.Id}] cancellation requested.");
            }
            Console.WriteLine($"Task [{p.task.Id}] canceled/completed: {p.task?.IsCanceled}/{p.task?.IsCompleted}");
            Console.BackgroundColor = consoleBackground;
         });
      }
   }
}
