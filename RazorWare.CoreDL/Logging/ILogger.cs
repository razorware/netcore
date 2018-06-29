
namespace RazorWare.CoreDL.Logging {
   public interface ILogger {
      LogCategory Category { get; }

      void Log(LogPriority logPriority, string entry);
   }
}
