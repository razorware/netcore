using System.Collections.Generic;

namespace RazorWare.CoreDL.Core {
   public class EventFilter {
      private readonly HashSet<EventType> events;

      public object Source { get; }

      public EventFilter(object eventSource) {
         events = new HashSet<EventType>();
         Source = eventSource;
      }

      public bool HasEvent(EventType type) {
         return events.Contains(type);
      }

      public void Add(EventType type) {
         events.Add(type);
      }

      public void AddRange(IEnumerable<EventType> types) {
         foreach(var t in types) {
            Add(t);
         }
      }
   }
}
