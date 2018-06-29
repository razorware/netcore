namespace RazorWare.CoreDL.Internals {
   internal enum SDL_EVENTTYPE : uint {
      FIRST = 0,

      /* Application */
      QUIT = 0x100,

      /* Window */
      WINDOW = 0x200,
      SYSWM,

      /* Keyboard */
      KEYDOWN = 0x300,
      KEYUP,
      TEXTEDITING,
      TEXTINPUT,

      /* Mouse */
      MOUSEMOTION = 0x400,
      MOUSEBUTTONDOWN,
      MOUSEBUTTONUP,
      MOUSEWHEEL,

      /* Joystick */
      JOYAXISMOTION = 0x600,
      JOYBALLMOTION,
      JOYHATMOTION,
      JOYBUTTONDOWN,
      JOYBUTTONUP,
      JOYDEVICEADDED,
      JOYDEVICEREMOVED,

      /* Game controller */
      CONTROLLERAXISMOTION = 0x650,
      CONTROLLERBUTTONDOWN,
      CONTROLLERBUTTONUP,
      CONTROLLERDEVICEADDED,
      CONTROLLERDEVICEREMOVED,
      CONTROLLERDEVICEREMAPPED,

      /* Touch */
      FINGERDOWN = 0x700,
      FINGERUP,
      FINGERMOTION,

      /* Gesture */
      DOLLARGESTURE = 0x800,
      DOLLARRECORD,
      MULTIGESTURE,

      /* Clipboard */
      CLIPBOARDUPDATE = 0x900,

      /* Drag/Drop */
      DROPFILE = 0x1000,
      /* Only available in 2.0.4 or higher */
      DROPTEXT,
      DROPBEGIN,
      DROPCOMPLETE,

      /* Audio hotplug */
      /* Only available in SDL 2.0.4 or higher */
      AUDIODEVICEADDED = 0x1100,
      AUDIODEVICEREMOVED,

      /* Render */
      /* 2.0.2 or higher */
      RENDER_TARGETS_RESET = 0x2000,
      /* 2.0.4 or higher */
      RENDER_DEVICE_RESET,

      /* Events SDL_USEREVENT through SDL_LASTEVENT are for
       * your use, and should be allocated with
       * SDL_RegisterEvents()
       */
      USER = 0x8000,

      /* The last event, used for bounding arrays. */
      LAST = 0xFFFF
   }
}
