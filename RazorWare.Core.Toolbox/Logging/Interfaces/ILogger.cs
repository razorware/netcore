using RazorWare.IO;

namespace RazorWare.Logging {
   public interface ILogger {
      string Directory { get; }
      string File { get; }
      LogLevel Level { get; set; }
      LogStatus Status { get; }
      LogFormatter Formatter { get; set; }

      void Write(string entry);
      void Write(LogLevel logLevel, string entry);
      void WriteLine(string entry);
      void WriteLine(LogLevel logLevel, string entry);
   }
}
