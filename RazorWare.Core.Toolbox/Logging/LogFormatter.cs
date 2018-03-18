using System;
using System.Linq;
using System.Globalization;
using System.Collections.Generic;

namespace RazorWare.Logging {
   public class LogFormatter : ILogFormatter {
      public const char DATE = 'D';
      public const char LOGLEVEL = 'L';
      public const char ENTRY = 'E';
      public const char SOURCE = 'S';
      public const string DefaultFormat = "D:[MMddyyyyHHmmssffff] L:{0, -10}:: E";

      private static readonly char[] KEYS = new[] { DATE, LOGLEVEL, ENTRY, SOURCE };

      private readonly Dictionary<string, string> mFormats;

      public static LogFormatter Default => new LogFormatter(DefaultFormat);

      public string ToFormat { get; set; } = "{0:D}{1:L}{2:E}";

      public string LogSource { get; set; }

      public LogFormatter(string format) {
         var elements = format.Split(" ", StringSplitOptions.RemoveEmptyEntries)
            .ToArray();

         mFormats = new Dictionary<string, string>();

         var i = 0;
         var key = string.Empty;

         while (i < elements.Length) {
            if (KEYS.Contains(elements[i][0])) {
               var kvParts = elements[i].Split(":");
               key = kvParts[0];

               if (kvParts.Length > 2) {
                  // rejoin parts inadvertently split on ':'
                  kvParts[1] = string.Join(':', kvParts.Skip(1).ToArray());
               }

               mFormats[key] = kvParts.Length > 1 ? $"{kvParts[1]} " : string.Empty;

               ++i;
               continue;
            }

            // we know this was split on ' ' so wrap with ' '
            mFormats[key] += $" {elements[i]} ";

            ++i;
         }

      }

      public string Format(string key, object arg, IFormatProvider formatProvider) {
         if (key == null) {
            return string.Empty;
         }

         if (key.Length > 1) {
            var keys = key.Split(" ", StringSplitOptions.RemoveEmptyEntries);
            var strBldr = new System.Text.StringBuilder();

            foreach(var k in keys) {
               if (mFormats.TryGetValue(k, out string partialFormat)) {
                  strBldr.Append(FormatValue(k, arg, partialFormat));
               }
            }

            return strBldr.ToString();
         }

         if (mFormats.TryGetValue(key, out string format)) {
            format = FormatValue(key, arg, format);
         }

         return format;
      }

      private string FormatValue(string key, object arg, string format) {
         if (key[0] == DATE) {
            format = ((DateTime)arg).ToString(format);
         }
         else if (key[0] == LOGLEVEL) {
            format = string.Format(format, (LogLevel)arg);
         }
         else if (key[0] == ENTRY) {
            format = arg.ToString();
         }
         else if (key[0] == SOURCE) {
            format = string.Format(format, LogSource);
         }
         else {
            throw new FormatException($"[{arg}] is unrecognized format parameter");
         }

         return format;
      }

      public object GetFormat(Type formatType) {
         if (formatType == typeof(ICustomFormatter)) {
            return this;
         }
         else {
            return null;
         }
      }

      public override string ToString( ) {
         return string.Join(string.Empty, mFormats.Values.ToArray());
      }

      private string FormatOtherFormats(string format, object arg) {
         if (arg is IFormattable) {
            return ((IFormattable)arg).ToString(format, CultureInfo.CurrentCulture);
         }
         else if (arg != null) {
            return arg.ToString();
         }
         else {
            return String.Empty;
         }
      }
   }
}
