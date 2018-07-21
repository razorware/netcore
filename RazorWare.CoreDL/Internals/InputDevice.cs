using System;

namespace RazorWare.CoreDL.Internals {
   using RazorWare.CoreDL.Core;

   internal class InputDevice : IEventListener {
      internal delegate void ProcessWindowEventHandler(SourceWindowEvent windowEvent);

      public ProcessWindowEventHandler ProcessHandler { get; internal set; }
      public EventSourceType Type => EventSourceType.Device;
      public string Name { get; }
      public EventFilter Events { get; }

      internal InputDevice(string deviceName, object device, params EventType[] deviceEvents) {
         Name = deviceName;
         Events = new EventFilter(device);
         Events.AddRange(deviceEvents);
      }

      public void Process(SourceWindowEvent windowEvent) {
         Console.WriteLine($"[{Name}] Handle event: {windowEvent.Type}");

         ProcessHandler?.Invoke(windowEvent);
      }
   }
}
