using System;

namespace RazorWare.CoreDL.Internals {
   internal interface ISDLHwnd : IDisposable {
      uint SdlSystem { get; }

      void Start( );
   }
}
