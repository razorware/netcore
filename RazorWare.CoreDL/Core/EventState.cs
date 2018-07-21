using System;

namespace RazorWare.CoreDL.Core {
   using RazorWare.CoreDL.Internals;

   public class EventState {

      public EventType Type { get; }

      internal EventState(ref SDL_Event sdlEvent) {
         Type = (EventType)sdlEvent.type;
      }
   }
}
