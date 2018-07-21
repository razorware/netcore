using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RazorWare.CoreDL.Testing {
   using RazorWare.CoreDL.Internals;

   [TestClass]
   public class SDLI_Tests {
      const int RESULT = 0;
      const uint UNKNOWN = 0;
      const int BAD_DEVICE = 7;
      static readonly IntPtr BAD_WINDOW = new IntPtr(0);

      [TestCleanup]
      public void CleanupSDL( ) {
         SDLI.SDL_Quit();
      }

      [TestMethod]
      public void GetPlatform( ) {
         CharPointer cp = SDLI.SDL_GetPlatform;

         Assert.AreEqual("Windows", cp);
      }

      [TestMethod]
      public void InitializeSDL( ) {
         Assert.AreEqual(RESULT, SDLI.SDL_Init(SDL_INIT.VIDEO));
         Assert.AreEqual(SDL_INIT.VIDEO, SDLI.SDL_WasInit(SDL_INIT.VIDEO));
      }

      [TestMethod]
      public void InitializeSDL_FAIL( ) {
         // apparently, the only way this fails is going to be if SDL fails
         Assert.AreEqual(RESULT, SDLI.SDL_Init(BAD_DEVICE));
         // but we can assert resource initialization failures like so
         Assert.AreEqual(UNKNOWN, SDLI.SDL_WasInit(SDL_INIT.VIDEO));
      }

      [TestMethod]
      public void InitializeSubSytem( ) {
         Assert.AreEqual(RESULT, SDLI.SDL_Init(SDL_INIT.VIDEO));
         Assert.AreEqual(RESULT, SDLI.SDL_InitSubSystem(SDL_INIT.AUDIO));

         var allInits = SDL_INIT.VIDEO | SDL_INIT.AUDIO;

         // will not return individual initializations from bitwise flag value
         Assert.AreNotEqual(SDL_INIT.AUDIO, SDLI.SDL_WasInit(SDL_INIT.VIDEO | SDL_INIT.AUDIO));
         Assert.AreNotEqual(SDL_INIT.VIDEO, SDLI.SDL_WasInit(SDL_INIT.VIDEO | SDL_INIT.AUDIO));

         // must check for individual initializaitons
         Assert.AreEqual(SDL_INIT.AUDIO, SDLI.SDL_WasInit(SDL_INIT.AUDIO));
         Assert.AreEqual(SDL_INIT.VIDEO, SDLI.SDL_WasInit(SDL_INIT.VIDEO));

         // or all expected
         Assert.AreEqual(allInits, SDLI.SDL_WasInit(SDL_INIT.VIDEO | SDL_INIT.AUDIO));

         // and for sanity - TIMER was not set
         Assert.AreEqual(UNKNOWN, SDLI.SDL_WasInit(SDL_INIT.TIMER));
      }

      [TestMethod]
      public void CreateWindow( ) {
         var title = new byte[0];
         var window = SDLI.SDL_CreateWindow(title, SDL_WINDOWPOS.CENTERED, SDL_WINDOWPOS.CENTERED, 800, 600, SDL_WINDOW.OPENGL);

         Assert.AreNotEqual(BAD_WINDOW, window);
      }
   }
}
