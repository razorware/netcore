using System;

namespace RazorWare.CoreDL.Core {
   using RazorWare.CoreDL.Internals;

   public class EventState {
      private readonly EventType type;

      public EventType Type => type;

      internal EventState(ref SDL_Event sdlEvent) {
         type = (EventType)sdlEvent.type;
      }
   }
}
