using System;

namespace RazorWare.Core.Media.Testing.CreateGameContext {
   class Program {
      private delegate void ProcessKey(ConsoleKeyInfo keyInfo);
      private delegate void WriteOptions( );
      private delegate void ExitHandler( );

      private static Program p;
      private static bool quit;
      private static ProcessKey processKey;
      private static WriteOptions writeOptions;
      private static ExitHandler exitHandler;

      private Game game;
      private TestGameWindow window;

      static void Main(string[] args) {
         p = new Program();
         (writeOptions = WriteMainOptions).Invoke();
         
         Console.WriteLine("Hello Game Context!");

         while (!quit) {
            var input = Console.ReadKey();
            Console.WriteLine();

            // Q key kills program
            if (input.Key == ConsoleKey.Q) {
               quit = true;
            }
            else {
               processKey?.Invoke(input);
            }
         }
      }

      private static void WriteMainOptions( ) {
         processKey = ProcessMainOptions;

         Console.WriteLine();
         Console.WriteLine("[1] Start Default Game Window");
         Console.WriteLine("[2] Start Game context");
         Console.WriteLine("___");
         Console.WriteLine("[X] Exit current Game Context");
         Console.WriteLine("___");
         Console.WriteLine("[Q] Quit");
      }

      private static void ProcessMainOptions(ConsoleKeyInfo input) {
         switch (input.Key) {
            case ConsoleKey.D1:
               p.window = new TestGameWindow();

               p.window.Show();
               p.window.Close();

               break;
            case ConsoleKey.D2:
               p.game = new Game("");
               exitHandler = ( ) => {
                  p.game?.Quit();
               };

               break;
            case ConsoleKey.X:
               exitHandler?.Invoke();

               break;
            default:
               writeOptions = WriteMainOptions;

               break;
         }

         writeOptions?.Invoke();
      }
   }
}
