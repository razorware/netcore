using System;

namespace RazorWare.CoreDL.Core {
   using RazorWare.CoreDL.Internals;

   public class Platform {
      private static readonly Lazy<Platform> _platform = new Lazy<Platform>(( ) => new Platform());

      private readonly CharPointer platform;

      public static Platform Instance => _platform.Value;

      public string OperatingSystem => platform;

      private Platform( ) {
         platform = SDLI.SDL_GetPlatform;
      }
   }
}
