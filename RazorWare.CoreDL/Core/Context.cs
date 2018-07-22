using System;

namespace RazorWare.CoreDL.Core {
   using RazorWare.CoreDL.Internals;

   public delegate void OnContextStart( );
   public delegate void OnContextClosing( );
   public delegate void OnContextQuit( );

   public abstract class Context : IDisposable {
      private static readonly EventPump eventPump = EventPump.Instance;
      private EventPumpState currentState;

      public event OnContextStart Start;
      public event OnContextClosing Closing;
      public event OnContextQuit Exit;
      
      public string Name { get; }
      public Keyboard Keyboard { get; }

      protected Context(string name) {
         Name = name;
         currentState = eventPump.IsRunning;
         Keyboard = Keyboard.Instance;

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
         eventPump.Start(((INativeWindow)window).GetNativeHandle());
      }

      public void Quit( ) {
         // TODO: make cancellable
         OnContextStopping();
         Closing?.Invoke();

         eventPump.Stop();
      }

      protected virtual void OnContextStart( ) { }

      protected virtual void OnContextStopping( ) { }

      protected virtual void OnContextQuit( ) { }

      private void RegisterEventDevices( ) {
         eventPump.RegisterEventListener(Keyboard.Device);
      }

      private void OnEventPumpStateChanged(DispatchState state) {
         if (currentState == state) {
            return;
         }

         // state change is occuring
         switch (state) {
            case DispatchState.Running:
               currentState = state;
               StartContext();

               break;
            case DispatchState.Idle:
               currentState = state;
               StopContext();

               break;
         }
      }

      private void StartContext( ) {
         OnContextStart();
         Start?.Invoke();
      }

      private void StopContext( ) {
         OnContextQuit();
         Exit?.Invoke();
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
