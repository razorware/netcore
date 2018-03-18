using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RazorWare.IO;
using RazorWare.Logging;

namespace RazorWare.Toolbox.Testing {
   [TestClass]
   public class LoggerTests {
      private static readonly DirectoryInfo mDirectory = new DirectoryInfo(Directory.GetCurrentDirectory());
      private static readonly FileMap fMap = FileMap.Instance;

      private ILogger mLogger;
      private string mLogFile;

      [TestInitialize]
      public void InitializeTest( ) {
         var fileName = "testLog";
         var filePath = Path.Combine(mDirectory.FullName, @"_items\logs", fileName);
         mLogger = Logger.CreateLog("LGTST", filePath);
         mLogFile = mLogger.File;
      }

      [TestCleanup]
      public void CleanupTest( ) {
         Logger.DeleteLog(mLogger);
      }

      [TestMethod]
      public void InitializeLoggerWithStringInfo( ) {
         var fileName = "log1";
         var filePath = Path.Combine(mDirectory.FullName, @"_items\logs", fileName);

         var logger = Logger.CreateLog(filePath);

         Assert.IsNotNull(logger, "logger == null");
         Assert.AreEqual(LogStatus.Ready, logger.Status);

         Logger.DeleteLog(logger);
      }

      [TestMethod]
      public void WriteLogEntry( ) {
         var entry = "Now is the time...";
         var expEntry = string.Format(LogFormatter.Default, "{0:D}{1:L}{2:E}", DateTime.Now, LogLevel.Forced, entry);

         mLogger.Write(entry);
         var file = fMap.Get(mLogFile);

         Assert.AreEqual(expEntry.Length, file.Length);
      }

      [TestMethod]
      public void TestLogLevel( ) {
         var entry = "This is info";
         var expEntry = string.Format(LogFormatter.Default, "{0:D}{1:L}{2:E}", DateTime.Now, LogLevel.Forced, entry);

         Assert.AreEqual(LogLevel.Warning, mLogger.Level);

         mLogger.WriteLine(LogLevel.Info, entry);
         var file = fMap.Get(mLogFile);

         Assert.AreEqual(0, file.Length);

         mLogger.WriteLine(LogLevel.Warning, entry);
         file = fMap.Get(mLogFile);

         Assert.AreEqual(expEntry.Length + Environment.NewLine.Length, file.Length);

         mLogger.WriteLine("Another line of info");
         mLogger.WriteLine("Yet another line - the last - of info ...");
      }

      [TestMethod]
      public void LoggingSource( ) {
         var entry = "Test Log Entry Source";
         mLogger.Formatter = new LogFormatter("D:[MMddyyyyHHmmssffff] S:[{0, -5} L:{0, -10}] :: E");
         mLogger.Formatter.ToFormat = "{0:D}{1:S L}{2:E}";

         Assert.AreEqual(LogLevel.Warning, mLogger.Level);

         mLogger.WriteLine(LogLevel.Warning, entry);
      }
   }
}
