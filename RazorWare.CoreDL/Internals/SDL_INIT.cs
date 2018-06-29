namespace RazorWare.CoreDL.Internals {
   internal static class SDL_INIT {
      internal const uint TIMER = 0x00000001;
      internal const uint AUDIO = 0x00000010;
      internal const uint VIDEO = 0x00000020;
      internal const uint JOYSTICK = 0x00000200;
      internal const uint HAPTIC = 0x00001000;
      internal const uint GAMECONTROLLER = 0x00002000;
      internal const uint NOPARACHUTE = 0x00100000;
      internal const uint EVERYTHING = (TIMER | AUDIO | VIDEO | JOYSTICK | HAPTIC | GAMECONTROLLER
      );
   }
}
