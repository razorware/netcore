using System;
using System.ComponentModel;

namespace RazorWare.CoreGL.Windows {
   using RazorWare.CoreGL.Devices;

   public delegate void OnWindowLoad( );
   public delegate void OnWindowResize(int X, int Y, int Width, int Height);
   public delegate void OnWindowFrameUpdate(FrameEventArgs args);
   public delegate void OnWindowFrameRender(FrameEventArgs args);
   public delegate void OnInputKeyDown(KeyboardEventArgs args);
   public delegate void OnInputKeyUp(KeyboardEventArgs args);
   public delegate void OnWindowClosing(CancelEventArgs args);

   public interface IWindow : IDisposable {
      event OnWindowLoad Load;
      event OnWindowResize Resize;
      event OnWindowFrameUpdate FrameUpdate;
      event OnWindowFrameRender FrameRender;
      event OnWindowClosing Closing;
      event OnInputKeyDown KeyDown;
      event OnInputKeyUp KeyUp;

      Keyboard Keyboard { get; }
      double FrameRate { get; }
      int Width { get; }
      int Height { get; }

      void Run( );
      void Exit( );
   }
}
