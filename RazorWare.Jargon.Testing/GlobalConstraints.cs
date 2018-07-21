using System;
using System.Text;

namespace RazorWare.Jargon.Testing {
   internal static class GlobalConstraints {
      public static string RootDirectory => @"..\..\..\..\jargon_samples\";
      public static Encoding Encoder => Encoding.UTF8;
   }
}
