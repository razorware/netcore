using System;
using System.IO;
using System.Collections.Generic;

namespace RazorWare.CoreDL.Logging {
   public class Logger {
      private static readonly Lazy<Logger> _logger = new Lazy<Logger>(( ) => new Logger());

      public static Logger Instance => _logger.Value;



      private Logger() {

      }
   }
}
