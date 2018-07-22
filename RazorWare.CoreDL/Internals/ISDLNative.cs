using System;

namespace RazorWare.CoreDL.Internals {
   internal interface ISDLNative : IDisposable {
      uint SdlSystem { get; }

      void Start(EventPump eventPump);
   }
}
