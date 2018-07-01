using System;

namespace RazorWare.CoreDL.Internals {
   using RazorWare.CoreDL.Core;

   /// <summary>
   /// Loops on a threaded task
   /// </summary>
   internal sealed class EventPump : IDisposable{
      internal delegate void OnEventPumpStateChange(EventPumpState state);

      private static readonly Lazy<EventPump> _eventPump = new Lazy<EventPump>(( ) => new EventPump());

      private ISDLHwnd sdlHwnd;

      internal static EventPump Instance => _eventPump.Value;

      internal bool IsRunning { get; private set; }
      internal event OnEventPumpStateChange EventPumpStateChanged;

      private EventPump( ) { }

      /// <summary>
      /// EventPump will only function with one system - TIMER or VIDEO or AUDIO, etc.
      /// Can be stopped and restarted with a different system although the effects of
      /// doing so need to be fully discovered.
      /// </summary>
      /// <param name="system"></param>
      internal void Start(uint system = SDL_INIT.TIMER) {
         if (sdlHwnd == null || SDLI.SDL_WasInit(system) != sdlHwnd.SdlSystem) {
            // changing SDL system initialization
            SDLI.SDL_Quit();

            SDLI.SDL_Init(system);
         }

         if (!IsRunning && system == SDL_INIT.TIMER) {
            PollEvents();
         }
      }

      internal void Start(ISDLHwnd hwnd) {
         Start(hwnd.SdlSystem);

         if (SDLI.SDL_WasInit(hwnd.SdlSystem) == hwnd.SdlSystem) {
            sdlHwnd = hwnd;
            PollEvents(sdlHwnd.Start);
         }
      }

      internal void Stop() {
         var eventState = EventPumpCommand.Quit.Execute();
      }

      private void PollEvents(Action onStart = null) {
         IsRunning = true;
         EventPumpStateChanged?.Invoke(IsRunning);

         // main loop keeping pump alive
         while(IsRunning) {
            onStart?.Invoke();
            onStart = null;

            // event polling loop
            while(SDLI.SDL_PollEvent(out SDL_Event sdlEvent) != 0) {
               if (sdlEvent.type == SDL_EVENTTYPE.QUIT) {
                  IsRunning = false;
                  EventPumpStateChanged?.Invoke(IsRunning);
               }
            }
         }
      }

      public void Dispose( ) {
         SDLI.SDL_QuitSubSystem(sdlHwnd.SdlSystem);
         sdlHwnd.Dispose();
      }
   }
}
