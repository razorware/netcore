using System;

namespace RazorWare.CoreGL {
   public class FrameEventArgs : EventArgs {
      public FrameEventArgs( ) { }
      public FrameEventArgs(double elapsed) {
         Time = elapsed;
      }

      public double Time { get; }
   }
}
