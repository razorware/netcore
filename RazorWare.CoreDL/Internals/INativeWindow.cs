﻿namespace RazorWare.CoreDL.Internals {
   using RazorWare.CoreDL.Core;

   internal interface INativeWindow : IWindow {
      ISDLNative GetNativeHandle( );
   }
}
