using RazorWare.CoreDL.Internals;

namespace RazorWare.CoreDL.Core {
   public interface IEventListener {
      EventSourceType Type { get; }
      string Name { get; }
      EventFilter Events { get; }

      void Process(SourceWindowEvent windowEvent);
   }
}
