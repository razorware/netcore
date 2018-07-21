using System;
using System.Collections.Generic;

namespace RazorWare.CoreDL.Internals {
   using RazorWare.CoreDL.Core;

   /// <summary>
   /// Loops on a threaded task
   /// </summary>
   internal sealed class EventPump : IDisposable {
      internal delegate void OnRendererUpdate( );
      internal delegate void OnEventPumpStateChange(DispatchState state);

      private static readonly Lazy<EventPump> _eventPump = new Lazy<EventPump>(( ) => new EventPump());

      private readonly List<IEventListener> listeners;

      private ISDLHwnd sdlHwnd;
      private OnRendererUpdate rendererUpdate;
      private EventPumpState state;

      internal static EventPump Instance => _eventPump.Value;

      internal bool IsRunning => state;
      internal event OnEventPumpStateChange EventPumpStateChanged;

      private EventPump( ) {
         listeners = new List<IEventListener>();
      }

      /// <summary>
      /// EventPump will only function with one system - TIMER or VIDEO or AUDIO, etc.
      /// Can be stopped and restarted with a different system although the effects of
      /// doing so needs to be fully discovered.
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

      internal void Stop( ) {
         var eventState = EventPumpCommand.Quit.Execute();
      }

      internal void RegisterEventListener(IEventListener eventListener) {
         // needs to be more robust - check for duplicates, etc.
         listeners.Add(eventListener);
      }

      internal void RendererUpdate(OnRendererUpdate onRendererUpdate) {
         rendererUpdate = onRendererUpdate;
      }

      private void PollEvents(Action<EventPump> onStart = null) {
         state = true;
         EventPumpStateChanged?.Invoke(state);

         // main loop keeping pump alive
         while (IsRunning) {
            onStart?.Invoke(this);
            onStart = null;

            // event polling loop
            while (SDLI.SDL_PollEvent(out SDL_Event sdlEvent) != 0) {
               if (!GetEventListener(sdlEvent, out IEventListener listener)) {
                  Console.WriteLine($"No event listener [{sdlEvent.type}] found");
               }
               else {
                  listener.Process(new SourceWindowEvent(listener.Events.Source, sdlEvent));
               }

               if (sdlEvent.type == SDL_EVENTTYPE.QUIT) {
                  state = false;
                  EventPumpStateChanged?.Invoke(state);
               }
            }

            if (IsRunning) {
               rendererUpdate?.Invoke();
            }
         }
      }

      private bool GetEventListener(SDL_Event sdlEvent, out IEventListener listener) {
         listener = listeners.Find(el => el.Events.HasEvent((EventType)sdlEvent.type));

         return listener != null;
      }

      #region IDisposable
      public void Dispose( ) {
         if (sdlHwnd != null) {
            SDLI.SDL_QuitSubSystem(sdlHwnd.SdlSystem);
         }
         else {
            SDLI.SDL_QuitSubSystem(SDL_INIT.TIMER);
         }

         SDLI.SDL_Quit();
         sdlHwnd?.Dispose();
      }
      #endregion
   }
}
