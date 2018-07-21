using System;

namespace RazorWare.CoreDL.Internals {
   using RazorWare.CoreDL.Core;

   internal class Renderer : IRenderer {
      private readonly IntPtr renderer;
      private readonly IntPtr surface;
      private readonly IntPtr texture;

      internal Renderer(IntPtr windowPointer, int width, int height) {
         renderer = SDLI.SDL_CreateRenderer(windowPointer, -1, SDL_RENDERERMODES.ACCELERATED);
         surface = IntPtr.Zero; // SDLI.SDL_CreateRGBSurface(0, width, height, 32, 255, 0, 0, 0);
         texture = SDLI.SDL_CreateTextureFromSurface(renderer, surface);
      }

      public void Update( ) {
         SDLI.SDL_RenderClear(renderer);
         SDLI.SDL_RenderCopy(renderer, texture, IntPtr.Zero, IntPtr.Zero);
         SDLI.SDL_RenderPresent(renderer);
      }

      public static implicit operator IntPtr(Renderer r) {
         return r.renderer;
      }

   }
}
