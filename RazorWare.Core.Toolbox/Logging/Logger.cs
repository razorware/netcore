using System;
using System.IO;
using RazorWare.IO;

namespace RazorWare.Logging {
   /// <summary>
   /// Asynchronous logger
   /// </summary>
   public class Logger : ILogger {
      private static readonly FileMap mFileMap = FileMap.Instance;

      private readonly string mSource;
      private IFile mLog;
      private LogFormatter logFormatter;

      public const string DefaultExt = "log";
      public const LogLevel DefaultLevel = LogLevel.Warning;

      public string Directory => mLog.Directory;
      public string File => mLog.Name;
      public LogFormatter Formatter {
         get { return logFormatter; }
         set {
            logFormatter = value;
            logFormatter.LogSource = mSource;
         }
      }
      public LogLevel Level { get; set; } = DefaultLevel;
      public LogStatus Status => mLog.Exists ? LogStatus.Ready : LogStatus.Unknown;

      private Logger(FileInfo logFile) : this(logFile, string.Empty) { }
      private Logger(FileInfo logFile, string logSource) {
         mLog = FileMap.Instance.Get(logFile.FullName);
         mSource = logSource;
         Formatter = LogFormatter.Default;
      }

      public static ILogger CreateLog(string filePath) {
         if (!FileMap.HasExtension(filePath)) {
            filePath = $"{filePath}.{DefaultExt}";
         }

         return new Logger(FileMap.CreateFile(filePath));
      }

      public static ILogger CreateLog(string logSource, string filePath) {
         if (!FileMap.HasExtension(filePath)) {
            filePath = $"{filePath}.{DefaultExt}";
         }

         return new Logger(FileMap.CreateFile(filePath), logSource);
      }

      public static void DeleteLog(ILogger logger) {
         ((Logger)logger).mLog.Delete(out ((Logger)logger).mLog);
      }

      public void Write(string entry) {
         Write(LogLevel.Forced, entry);
      }

      public void Write(LogLevel logLevel, string entry) {
         if (logLevel >= Level || logLevel == LogLevel.Forced) {
            using (mLog) {
               entry = string.Format(Formatter, Formatter.ToFormat, DateTime.Now, logLevel, entry);
               var buffer = System.Text.Encoding.UTF8.GetBytes(entry);
               mLog.Write(buffer);
            }
         }
      }

      public void WriteLine(string entry) {
         Write(LogLevel.Forced, $"{entry}{Environment.NewLine}");
      }

      public void WriteLine(LogLevel logLevel, string entry) {
         Write(logLevel, $"{entry}{Environment.NewLine}");
      }
   }
}
