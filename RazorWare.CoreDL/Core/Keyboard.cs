using System;

namespace RazorWare.CoreDL.Core {
   using RazorWare.CoreDL.Internals;

   public delegate void KeyboardKeyDown( );
   public delegate void KeyboardKeyUp( );

   public class Keyboard {
      private static readonly Lazy<Keyboard> _keyboard = new Lazy<Keyboard>(() => new Keyboard());

      private readonly InputDevice device;

      public static Keyboard Instance => _keyboard.Value;

      public IEventListener Device => device;

      private Keyboard( ) {
         device = new InputDevice("Keyboard", this, EventType.KeyDown, EventType.KeyUp, EventType.TestInput);
         device.ProcessHandler = Process;
      }

      private void Process(SourceWindowEvent windowEvent) {
         
      }
   }
}
