using System;
using System.Runtime.InteropServices;

namespace RazorWare.CoreDL.Internals {
#pragma warning disable 0169
   /* Audio device event (event.adevice.*) */
   [StructLayout(LayoutKind.Sequential)]
   internal struct SDL_AudioDeviceEvent {
      public UInt32 type;
      public UInt32 timestamp;
      public UInt32 which;
      public byte iscapture;
      private byte padding1;
      private byte padding2;
      private byte padding3;
   }
#pragma warning restore 0169
}
