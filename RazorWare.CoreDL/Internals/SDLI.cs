using System;

namespace RazorWare.CoreDL.Internals {
   using System.Runtime.InteropServices;

   internal static class SDLI {
      #region Constants
      const string SDLib = @".\Binaries\SDL2.dll";

      internal const int SCANCODE_MASK = (1 << 30);

      /* General keyboard/mouse state definitions. */
      internal const byte SDL_PRESSED = 1;
      internal const byte SDL_RELEASED = 0;

      /* Default size is according to SDL2 default. */
      internal const int SDL_TEXTEDITINGEVENT_TEXT_SIZE = 32;
      internal const int SDL_TEXTINPUTEVENT_TEXT_SIZE = 32;

      /* These are for SDL_EventState(). */
      internal const int SDL_QUERY = -1;
      internal const int SDL_IGNORE = 0;
      internal const int SDL_DISABLE = 0;
      internal const int SDL_ENABLE = 1;

      internal const int SDL_LOG_CATEGORY_APPLICATION = 0;
      internal const int SDL_LOG_CATEGORY_ERROR = 1;
      internal const int SDL_LOG_CATEGORY_ASSERT = 2;
      internal const int SDL_LOG_CATEGORY_SYSTEM = 3;
      internal const int SDL_LOG_CATEGORY_AUDIO = 4;
      internal const int SDL_LOG_CATEGORY_VIDEO = 5;
      internal const int SDL_LOG_CATEGORY_RENDER = 6;
      internal const int SDL_LOG_CATEGORY_INPUT = 7;
      internal const int SDL_LOG_CATEGORY_TEST = 8;

      /* Reserved for future SDL library use */
      internal const int SDL_LOG_CATEGORY_RESERVED1 = 9;
      internal const int SDL_LOG_CATEGORY_RESERVED2 = 10;
      internal const int SDL_LOG_CATEGORY_RESERVED3 = 11;
      internal const int SDL_LOG_CATEGORY_RESERVED4 = 12;
      internal const int SDL_LOG_CATEGORY_RESERVED5 = 13;
      internal const int SDL_LOG_CATEGORY_RESERVED6 = 14;
      internal const int SDL_LOG_CATEGORY_RESERVED7 = 15;
      internal const int SDL_LOG_CATEGORY_RESERVED8 = 16;
      internal const int SDL_LOG_CATEGORY_RESERVED9 = 17;
      internal const int SDL_LOG_CATEGORY_RESERVED10 = 18;
      internal const int SDL_LOG_CATEGORY_CUSTOM = 19;
      #endregion

      #region SDL initializations, et al
      [DllImport(SDLib, CallingConvention = CallingConvention.Cdecl)]
      internal static extern int SDL_Init(uint flags);

      [DllImport(SDLib, CallingConvention = CallingConvention.Cdecl)]
      internal static extern void SDL_Quit( );

      [DllImport(SDLib, CallingConvention = CallingConvention.Cdecl)]
      internal static extern int SDL_InitSubSystem(uint flags);

      [DllImport(SDLib, CallingConvention = CallingConvention.Cdecl)]
      internal static extern void SDL_QuitSubSystem(uint flags);

      [DllImport(SDLib, CallingConvention = CallingConvention.Cdecl)]
      internal static extern uint SDL_WasInit(uint flags);
      #endregion

      #region Platform
      [DllImport(SDLib, EntryPoint = "SDL_GetPlatform", CallingConvention = CallingConvention.Cdecl)]
      private static extern IntPtr PRIVATE_SDL_GetPlatform( );
      internal static Func<IntPtr> SDL_GetPlatform = ( ) => {
         return PRIVATE_SDL_GetPlatform();
      };
      #endregion

      #region Memory
      [DllImport(SDLib, EntryPoint = "SDL_malloc", CallingConvention = CallingConvention.Cdecl)]
      internal static extern IntPtr SDL_Malloc(IntPtr size);

      [DllImport(SDLib, EntryPoint = "SDL_free", CallingConvention = CallingConvention.Cdecl)]
      internal static extern void SDL_Free(IntPtr memblock);
      #endregion

      #region SDL Window
      [DllImport(SDLib, EntryPoint = "SDL_CreateWindow", CallingConvention = CallingConvention.Cdecl)]
      internal static extern IntPtr SDL_CreateWindow(byte[] title, int x, int y, int w, int h, SDL_WINDOW flags);

      [DllImport(SDLib, CallingConvention = CallingConvention.Cdecl)]
      internal static extern void SDL_DestroyWindow(IntPtr window);

      [DllImport(SDLib, CallingConvention = CallingConvention.Cdecl)]
      internal static extern int SDL_UpdateWindowSurface(IntPtr window);
      #endregion

      #region SDL Events
      [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
      internal delegate int SDL_EventFilter(IntPtr userdata, IntPtr sdlevent);

      /* Pump the event loop, getting events from the input devices*/
      [DllImport(SDLib, CallingConvention = CallingConvention.Cdecl)]
      internal static extern void SDL_PumpEvents( );

      [DllImport(SDLib, CallingConvention = CallingConvention.Cdecl)]
      internal static extern int SDL_PeepEvents([Out()][MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)]SDL_Event[] events,
         int numevents, SDL_EVENTACTION action, SDL_EVENTTYPE minType, SDL_EVENTTYPE maxType);

      /* Checks to see if certain events are in the event queue */
      [DllImport(SDLib, CallingConvention = CallingConvention.Cdecl)]
      internal static extern SDL_BOOL SDL_HasEvent(SDL_EVENTTYPE type);

      [DllImport(SDLib, CallingConvention = CallingConvention.Cdecl)]
      internal static extern SDL_BOOL SDL_HasEvents(SDL_EVENTTYPE minType, SDL_EVENTTYPE maxType);

      /* Clears events from the event queue */
      [DllImport(SDLib, CallingConvention = CallingConvention.Cdecl)]
      internal static extern void SDL_FlushEvent(SDL_EVENTTYPE type);

      [DllImport(SDLib, CallingConvention = CallingConvention.Cdecl)]
      internal static extern void SDL_FlushEvents(SDL_EVENTTYPE min, SDL_EVENTTYPE max);

      /* Polls for currently pending events */
      [DllImport(SDLib, CallingConvention = CallingConvention.Cdecl)]
      internal static extern int SDL_PollEvent(out SDL_Event _event);

      /* Waits indefinitely for the next event */
      [DllImport(SDLib, CallingConvention = CallingConvention.Cdecl)]
      internal static extern int SDL_WaitEvent(out SDL_Event _event);

      /* Waits until the specified timeout (in ms) for the next event
		 */
      [DllImport(SDLib, CallingConvention = CallingConvention.Cdecl)]
      internal static extern int SDL_WaitEventTimeout(out SDL_Event _event, int timeout);

      /* Add an event to the event queue */
      [DllImport(SDLib, CallingConvention = CallingConvention.Cdecl)]
      internal static extern int SDL_PushEvent(ref SDL_Event _event);

      /* userdata refers to a void* */
      [DllImport(SDLib, CallingConvention = CallingConvention.Cdecl)]
      internal static extern void SDL_SetEventFilter(SDL_EventFilter filter, IntPtr userdata);

      /* userdata refers to a void* */
      [DllImport(SDLib, CallingConvention = CallingConvention.Cdecl)]
      internal static extern SDL_BOOL SDL_GetEventFilter(out IntPtr filter, out IntPtr userdata);

      /// <summary>
      /// TODO: Move out of interface
      /// </summary>
      /// <param name="filter"></param>
      /// <param name="userdata"></param>
      /// <returns></returns>
      public static SDL_BOOL SDL_GetEventFilter(out SDL_EventFilter filter, out IntPtr userdata) {
         IntPtr result = IntPtr.Zero;
         SDL_BOOL retval = SDL_GetEventFilter(out result, out userdata);

         if (result != IntPtr.Zero) {
            filter = (SDL_EventFilter)Marshal.GetDelegateForFunctionPointer(
               result,
               typeof(SDL_EventFilter)
            );
         }
         else {
            filter = null;
         }

         return retval;
      }

      [DllImport(SDLib, CallingConvention = CallingConvention.Cdecl)]
      internal static extern void SDL_AddEventWatch(SDL_EventFilter filter, IntPtr userdata);

      [DllImport(SDLib, CallingConvention = CallingConvention.Cdecl)]
      internal static extern void SDL_DelEventWatch(SDL_EventFilter filter, IntPtr userdata);

      [DllImport(SDLib, CallingConvention = CallingConvention.Cdecl)]
      internal static extern void SDL_FilterEvents(SDL_EventFilter filter, IntPtr userdata);

      [DllImport(SDLib, CallingConvention = CallingConvention.Cdecl)]
      internal static extern byte SDL_EventState(SDL_EVENTTYPE type, int state);

      /// <summary>
      /// TODO: Move out of interface
      /// Get the state of an event
      /// </summary>
      /// <param name="type"></param>
      /// <returns></returns>
      public static byte SDL_GetEventState(SDL_EVENTTYPE type) {
         return SDL_EventState(type, SDL_QUERY);
      }

      /* Allocate a set of user-defined events */
      [DllImport(SDLib, CallingConvention = CallingConvention.Cdecl)]
      internal static extern UInt32 SDL_RegisterEvents(int numevents);
      #endregion

      #region SDL Log
      [DllImport(SDLib, CallingConvention = CallingConvention.Cdecl)]
      internal static extern SDL_LOGPRIORITY SDL_LogGetPriority(int category);

      [DllImport(SDLib, CallingConvention = CallingConvention.Cdecl)]
      internal static extern void SDL_LogSetPriority(int category, SDL_LOGPRIORITY priority);

      [DllImport(SDLib, CallingConvention = CallingConvention.Cdecl)]
      internal static extern void SDL_LogSetAllPriority(SDL_LOGPRIORITY priority);

      [DllImport(SDLib, CallingConvention = CallingConvention.Cdecl)]
      internal static extern void SDL_LogResetPriorities( );

      [DllImport(SDLib, CallingConvention = CallingConvention.Cdecl)]
      internal static extern void SDL_LogGetOutputFunction(out IntPtr callback, out IntPtr userdata);

      /// <summary>
      /// TODO: Move out of interface
      /// </summary>
      /// <param name="callback"></param>
      /// <param name="userdata"></param>
      public static void SDL_LogGetOutputFunction(out SDL_LogOutputFunction callback, out IntPtr userdata) {
         IntPtr result = IntPtr.Zero;
         SDL_LogGetOutputFunction(out result, out userdata);

         if (result != IntPtr.Zero) {
            var funcPointer = Marshal.GetDelegateForFunctionPointer(result, typeof(SDL_LogOutputFunction));
            callback = (SDL_LogOutputFunction)funcPointer;
         }
         else {
            callback = null;
         }
      }

      [DllImport(SDLib, CallingConvention = CallingConvention.Cdecl)]
      internal static extern void SDL_LogSetOutputFunction(SDL_LogOutputFunction callback, IntPtr userdata);

      [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
      public delegate void SDL_LogOutputFunction(IntPtr userdata, int category, SDL_LOGPRIORITY priority, IntPtr message);

      /* Use string.Format for arglists */
      [DllImport(SDLib, CallingConvention = CallingConvention.Cdecl)]
      internal static extern void SDL_Log(byte[] logEntry);

      /* Use string.Format for arglists */
      [DllImport(SDLib, CallingConvention = CallingConvention.Cdecl)]
      internal static extern void SDL_LogVerbose(int category, byte[] logEntry);

      [DllImport(SDLib, CallingConvention = CallingConvention.Cdecl)]
      internal static extern void SDL_LogDebug(int category, byte[] logEntry);

      [DllImport(SDLib, EntryPoint = "SDL_LogInfo", CallingConvention = CallingConvention.Cdecl)]
      internal static extern void SDL_LogInfo(int category, byte[] logEntry);

      [DllImport(SDLib, EntryPoint = "SDL_LogWarn", CallingConvention = CallingConvention.Cdecl)]
      internal static extern void SDL_LogWarn(int category, byte[] logEntry);

      [DllImport(SDLib, EntryPoint = "SDL_LogError", CallingConvention = CallingConvention.Cdecl)]
      internal static extern void SDL_LogError(int category, byte[] logEntry);

      [DllImport(SDLib, EntryPoint = "SDL_LogCritical", CallingConvention = CallingConvention.Cdecl)]
      internal static extern void SDL_LogCritical(int category, byte[] logEntry);

      [DllImport(SDLib, EntryPoint = "SDL_LogMessage", CallingConvention = CallingConvention.Cdecl)]
      internal static extern void SDL_LogMessage(int category, SDL_LOGPRIORITY priority, byte[] logEntry);

      [DllImport(SDLib, EntryPoint = "SDL_LogMessageV", CallingConvention = CallingConvention.Cdecl)]
      internal static extern void SDL_LogMessageV(int category, SDL_LOGPRIORITY priority, byte[] logEntry);
      #endregion

      #region SDL Renderer
      [DllImport(SDLib, CallingConvention = CallingConvention.Cdecl)]
      public static extern int SDL_RenderClear(IntPtr renderer);

      /// <summary>
      /// Create an SDL Renderer given window*, index and flags
      /// </summary>
      /// <param name="window">window*</param>
      /// <param name="index">rendering driver index to initialize; -1 to initialize first driver supporting flags</param>
      /// <param name="flags">0, or one or more SDL_RENDERERMODES</param>
      /// <returns></returns>
      [DllImport(SDLib, CallingConvention = CallingConvention.Cdecl)]
      public static extern IntPtr SDL_CreateRenderer(IntPtr window, int index, SDL_RENDERERMODES flags);

      [DllImport(SDLib, CallingConvention = CallingConvention.Cdecl)]
      public static extern IntPtr SDL_CreateSoftwareRenderer(IntPtr surface);

      [DllImport(SDLib, CallingConvention = CallingConvention.Cdecl)]
      public static extern int SDL_SetRenderDrawColor(IntPtr renderer, byte r, byte g, byte b, byte a);

      /// <summary>
      /// Create an SDL Texture given renderer*, size, format and access
      /// </summary>
      /// <param name="renderer">renderer*</param>
      /// <param name="format">format</param>
      /// <param name="access">access</param>
      /// <param name="w">width</param>
      /// <param name="h">height</param>
      /// <returns>texture*</returns>
      [DllImport(SDLib, CallingConvention = CallingConvention.Cdecl)]
      public static extern IntPtr SDL_CreateTexture(IntPtr renderer, uint format, int access, int w, int h);

      [DllImport(SDLib, CallingConvention = CallingConvention.Cdecl)]
      public static extern IntPtr SDL_CreateTextureFromSurface(IntPtr renderer, IntPtr surface);

      //[DllImport(SDLib, CallingConvention = CallingConvention.Cdecl)]
      //public static extern int SDL_RenderCopy(
      //   IntPtr renderer,
      //   IntPtr texture,
      //   ref SDL_Rect srcrect,
      //   ref SDL_Rect dstrect
      //);

      /* renderer refers to an SDL_Renderer*, texture to an SDL_Texture*.
		 * Internally, this function contains logic to use default values when
		 * source and destination rectangles are passed as NULL.
		 * This overload allows for IntPtr.Zero (null) to be passed for srcrect.
		 */
      //[DllImport(SDLib, CallingConvention = CallingConvention.Cdecl)]
      //public static extern int SDL_RenderCopy(
      //   IntPtr renderer,
      //   IntPtr texture,
      //   IntPtr srcrect,
      //   ref SDL_Rect dstrect
      //);

      /* renderer refers to an SDL_Renderer*, texture to an SDL_Texture*.
		 * Internally, this function contains logic to use default values when
		 * source and destination rectangles are passed as NULL.
		 * This overload allows for IntPtr.Zero (null) to be passed for dstrect.
		 */
      //[DllImport(SDLib, CallingConvention = CallingConvention.Cdecl)]
      //public static extern int SDL_RenderCopy(
      //   IntPtr renderer,
      //   IntPtr texture,
      //   ref SDL_Rect srcrect,
      //   IntPtr dstrect
      //);

      [DllImport(SDLib, CallingConvention = CallingConvention.Cdecl)]
      public static extern int SDL_RenderCopy(IntPtr renderer, IntPtr texture, IntPtr srcrect, IntPtr dstrect);

      [DllImport(SDLib, CallingConvention = CallingConvention.Cdecl)]
      public static extern void SDL_RenderPresent(IntPtr renderer);
      #endregion

      #region SDL Surface
      [DllImport(SDLib, CallingConvention = CallingConvention.Cdecl)]
      public static extern IntPtr SDL_CreateRGBSurface(uint flags, int width, int height, int depth, uint Rmask, uint Gmask, uint Bmask, uint Amask);
      #endregion
   }
}
