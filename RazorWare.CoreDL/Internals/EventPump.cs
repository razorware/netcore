using System;
using System.Threading;
using System.Threading.Tasks;

namespace RazorWare.CoreDL.Internals {
   using RazorWare.CoreDL.Core;

   /// <summary>
   /// Loops on a threaded task
   /// </summary>
   internal sealed class EventPump : IDisposable{
      private static readonly Lazy<EventPump> _eventPump = new Lazy<EventPump>(( ) => new EventPump());

      private readonly ManualResetEventSlim waitSignal;

      private CancellationTokenSource taskWatcher;

      public static EventPump Instance => _eventPump.Value;

      public bool IsInitialized { get; private set; }
      public bool IsRunning { get; private set; }

      private EventPump() {
         // initialize SDL Timer so Events will publish
         IsInitialized = SDLI.SDL_Init(SDL_INIT.TIMER) == 0;
         waitSignal = new ManualResetEventSlim(false);
      }

      internal async void Start() {
         if (!IsRunning) {
            IsRunning = await Start(GetEventLoop());
         }
      }

      internal void Stop() {
         var eventState = EventPumpCommand.Quit.Execute();
         waitSignal.Wait();
      }

      private Task GetEventLoop() {
         taskWatcher = new CancellationTokenSource();

         return new Task(() => {
            PollEvents();
         }, taskWatcher.Token);         
      }

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
      private async Task<bool> Start(Task startTask) {
         startTask.Start();

         return true;
      }
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously

      private void PollEvents() {
         waitSignal.Reset();

         // main loop keeping pump alive
         while(IsRunning) {

            // event polling loop
            while(SDLI.SDL_PollEvent(out SDL_Event sdlEvent) != 0) {
               if (sdlEvent.type == SDL_EVENTTYPE.QUIT) {
                  taskWatcher.Cancel();
                  IsRunning = false;
               }
            }
         }

         waitSignal.Set();
      }

      public void Dispose( ) {
         SDLI.SDL_QuitSubSystem(SDL_INIT.TIMER);

         taskWatcher?.Dispose();
         waitSignal.Dispose();
      }
   }
}
