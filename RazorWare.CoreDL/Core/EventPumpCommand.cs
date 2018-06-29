using System;

namespace RazorWare.CoreDL.Core {
   using RazorWare.CoreDL.Internals;

   public class EventPumpCommand : SdlCommand {
      public static EventPumpCommand Quit = new EventPumpCommand(e => {
         e.type = SDL_EVENTTYPE.QUIT;

         return e;
      });

      private SDL_Event _event;

      public EventState Event => new EventState(ref _event);

      private EventPumpCommand(Func<SDL_Event, SDL_Event> buildEvent) {
         _event = buildEvent(_event);
      }

      public override EventState Execute( ) {
         var retVal = SDLI.SDL_PushEvent(ref _event);

         return new EventState(ref _event);
      }
   }
}
