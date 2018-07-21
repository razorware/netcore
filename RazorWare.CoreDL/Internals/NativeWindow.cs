using System;
using System.Text;
using System.Drawing;

namespace RazorWare.CoreDL.Internals {
   using RazorWare.Geometry;
   using RazorWare.CoreDL.Core;

   internal sealed class NativeWindow : ISDLHwnd, IEventListener, IDisposable {
      internal delegate void Configure( );
      internal delegate void Initialized(bool isInitialized);
      internal delegate void Resize( );

      private readonly byte[] title;
      private readonly uint sdlSystem = SDL_INIT.VIDEO;

      private Renderer renderer;
      private IntPtr nativePointer;
      private bool isInitialized;
      private Location<int> location;
      private Size<int> size;
      private Configure configure;
      private Initialized initialized;
      private Resize resize;

      internal static Encoding Encoder => Constants.Encoder;

      uint ISDLHwnd.SdlSystem => sdlSystem;
      public Point Location => location.ToPoint();
      public Size Size => size.ToSize();
      public EventSourceType Type => EventSourceType.Window;
      public string Name => "Window";

      public EventFilter Events { get; }
      public WindowStyles Style { get; internal set; }

      internal NativeWindow(int locX, int locY, int width, int height) {
         Events = new EventFilter(this);

         EventPump.Instance.RegisterEventListener(this);

         location = new Location<int>(locX, locY);
         size = new Size<int>(width, height);
      }

      internal NativeWindow(string windowTitle, int locX = SDL_WINDOWPOS.CENTERED, int locY = SDL_WINDOWPOS.CENTERED, int width = 800, int height = 600, SDL_WINDOW windowFlags = SDL_WINDOW.OPENGL) {
         Events = new EventFilter(this);
         Events.Add(EventType.Window);
         Events.Add(EventType.Quit);

         EventPump.Instance.RegisterEventListener(this);

         title = Encoder.GetBytes(windowTitle);
         location = new Location<int>(locX, locY);
         size = new Size<int>(width, height);

         Style = (WindowStyles)windowFlags;
      }

      void ISDLHwnd.Start(EventPump eventPump) {
         if (SDLI.SDL_WasInit(sdlSystem) != sdlSystem) {
            // TODO: set error condition
         }

         eventPump.EventPumpStateChanged += EventPumpStateChanged;

         configure?.Invoke();
         nativePointer = SDLI.SDL_CreateWindow(title, location.X, location.Y, size.Width, size.Height, (SDL_WINDOW)Style);
         renderer = new Renderer(nativePointer, size.Width, size.Height);
         eventPump.RendererUpdate(renderer.Update);

         isInitialized = true;
         initialized?.Invoke(isInitialized);
      }

      internal static IntPtr Instantiate(NativeWindow window) {
         window.nativePointer = SDLI.SDL_CreateWindow(window.title, window.Location.X, window.Location.Y, window.Size.Width, window.Size.Height, (SDL_WINDOW)window.Style);
         window.renderer = new Renderer(window.nativePointer, window.Size.Width, window.Size.Height);
         EventPump.Instance.RendererUpdate(window.renderer.Update);

         return window.nativePointer;
      }

      public void OnConfigure(Configure onConfigure) {
         configure = onConfigure;
      }

      public void OnInitialized(Initialized onInitialized) {
         initialized = onInitialized;
      }

      public void OnResize(Resize onResize) {
         resize = onResize;
      }

      public void Process(SourceWindowEvent windowEvent) {
         Console.WriteLine($"[{Name}] Handle event: {windowEvent.Type}");

         if (windowEvent.Resize) {
            resize?.Invoke();
         }
      }

      internal void SetBackground(byte red, byte green, byte blue, byte alpha) {
         SDLI.SDL_SetRenderDrawColor(renderer, red, green, blue, alpha);
      }

      private void EventPumpStateChanged(DispatchState state) {
         if (state == DispatchState.Idle) {
            EventPump.Instance.EventPumpStateChanged -= EventPumpStateChanged;
         }
      }

      #region IDisposable Support
      private bool isDisposed = false;

      private void Dispose(bool disposing) {
         if (!isDisposed) {
            if (disposing) {
               // TODO: dispose managed state (managed objects).
            }

            // free unmanaged resources (unmanaged objects) and override a finalizer below.
            SDLI.SDL_DestroyWindow(nativePointer);

            // TODO: set large fields to null.

            isDisposed = true;
         }
      }

      // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
      ~NativeWindow( ) {
         // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
         Dispose(false);
      }

      // This code added to correctly implement the disposable pattern.
      public void Dispose( ) {
         // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
         Dispose(true);
         // TODO: uncomment the following line if the finalizer is overridden above.
         GC.SuppressFinalize(this);
      }
      #endregion
   }
}
