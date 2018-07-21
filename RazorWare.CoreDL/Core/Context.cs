using System;

namespace RazorWare.CoreDL.Core {
   using RazorWare.CoreDL.Internals;

   public delegate void OnContextStart( );
   public delegate void OnContextClosing( );
   public delegate void OnContextQuit( );

   public abstract class Context : IDisposable {
      private static readonly EventPump eventPump = EventPump.Instance;

      private readonly Keyboard keyboard;

      private EventPumpState currentState;

      public event OnContextStart Start;
      public event OnContextClosing Closing;
      public event OnContextQuit Exit;

      public string Name { get; }

      protected Context(string name) {
         Name = name;
         currentState = eventPump.IsRunning;
         keyboard = Keyboard.Instance;

         eventPump.EventPumpStateChanged += OnEventPumpStateChanged;
      }

      /// <summary>
      /// Windowless
      /// </summary>
      public virtual void Run( ) {
         eventPump.Start();
      }

      public void Run(IWindow window) {
         RegisterEventDevices();
         eventPump.Start(((INativeWindow)window).GetHwndDevice());
      }

      public void Quit( ) {
         OnContextClosing();
         eventPump.Stop();
      }

      protected virtual void OnContextStart( ) {
         Start?.Invoke();
      }

      protected virtual void OnContextClosing() {
         Closing?.Invoke();
      }

      protected virtual void OnContextQuit() {
         Exit?.Invoke();
      }

      private void RegisterEventDevices( ) {
         eventPump.RegisterEventListener(keyboard.Device);
      }

      private void OnEventPumpStateChanged(DispatchState state) {
         if (currentState == state) {
            return;
         }

         // we know that a state change is occuring
         switch (state) {
            case DispatchState.Running:
               currentState = state;
               OnContextStart();

               break;
            case DispatchState.Idle:
               currentState = state;
               OnContextQuit();

               break;
         }
      }

      public void Dispose( ) {
         if (eventPump.IsRunning) {
            eventPump.Stop();
         }
         eventPump.EventPumpStateChanged -= OnEventPumpStateChanged;

         eventPump.Dispose();
      }
   }
}
