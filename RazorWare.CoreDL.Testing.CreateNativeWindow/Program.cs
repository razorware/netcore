using System;
using System.Text;

namespace RazorWare.CoreDL.Testing.CreateNativeWindow {
   using RazorWare.CoreDL.Internals;

   class Program {
      private static bool quit;
      private static Encoding Encoder = Encoding.UTF8;
      private static int locX = SDL_WINDOWPOS.CENTERED;
      private static int locY = SDL_WINDOWPOS.CENTERED;
      private static int width = 800;
      private static int height = 600;
      private static SDL_WINDOW windowFlags = SDL_WINDOW.OPENGL;

      static void Main(string[] args) {
         Console.WriteLine("Hello NativeWindow!");
         WriteOptions();

         while (!quit) {
            var input = Console.ReadKey();
            Console.WriteLine();

            if (input.Key == ConsoleKey.Q) {
               quit = true;
               continue;
            }

            if (input.Key == ConsoleKey.D1) {
               CreateDLLNativeWindow();
            }
            else if (input.Key == ConsoleKey.D2) {
               CreateCoreDLNativeWindow();
            }
            else {
               WriteOptions();
            }
         }
      }

      private static void WriteOptions( ) {
         Console.WriteLine();
         Console.WriteLine("[1] via SDL DLL call");
         Console.WriteLine("[2] via NativeWindow");
         Console.WriteLine("___");
         Console.WriteLine("[Q] Quit");
      }

      private static void CreateDLLNativeWindow( ) {
         var title = "DLL Native Window";
         var window = SDLI.SDL_CreateWindow(Encoder.GetBytes(title), locX, locY, width, height, windowFlags);
         var running = true;

         while (running) {
            while (SDLI.SDL_PollEvent(out SDL_Event sdlEvent) != 0) {
               if (sdlEvent.type == SDL_EVENTTYPE.QUIT) {
                  running = false;
               }
            }
         }

         SDLI.SDL_DestroyWindow(window);
      }

      private static void CreateCoreDLNativeWindow( ) {
         var window = new NativeWindow("CoreDL Native Window");

         using (EventPump eventPump = EventPump.Instance) {
            eventPump.Start(window);

            /*
             * because EventPump was started with window, it will dispose
             * of window when it disposes
             * ***/
         }
      }
   }
}
