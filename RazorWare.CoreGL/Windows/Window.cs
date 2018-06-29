using System;
using System.Drawing;
using System.ComponentModel;
using System.Drawing.Imaging;

namespace RazorWare.CoreGL.Windows {
   using RazorWare.CoreGL.Devices;

   public class Window : IWindow {
      private readonly CoreWindow window;
      private readonly double mFrameRate;

      public event OnWindowLoad Load;
      public event OnWindowResize Resize;
      public event OnWindowFrameUpdate FrameUpdate;
      public event OnWindowFrameRender FrameRender;
      public event OnWindowClosing Closing;
      public event OnInputKeyDown KeyDown;
      public event OnInputKeyUp KeyUp;

      public const double DefaultFrameRate = 30;

      public double FrameRate => mFrameRate;
      public Keyboard Keyboard => Keyboard.Device;
      public int Width => window.Width;
      public int Height => window.Height;

      protected Window( ) : this("New Window", DefaultFrameRate) { }

      protected Window(string caption, double frameRate) : this(frameRate, caption) { }

      private Window(double frameRate, string caption) {
         mFrameRate = frameRate;
         window = string.IsNullOrEmpty(caption) ? new CoreWindow(800, 600) : new CoreWindow(800, 600, title: caption);
         //Keyboard.InputKeyboard = window.Keyboard;

         //window.Load += (sender, args) => { OnWindowLoad(args); };
         //window.Resize += (sender, args) => { OnWindowResize(args); };
         //window.UpdateFrame += (sender, args) => { OnWindowFrameUpdate(new Domain.FrameEventArgs(args.Time)); };
         //window.RenderFrame += (sender, args) => {
         //   OnWindowFrameRender(new Domain.FrameEventArgs(args.Time));
         //   window.SwapBuffers();
         //};
         //window.Closing += (sender, args) => { OnWindowClosing(args); };

         //window.KeyDown += (sender, args) => { OnWindowInputKeyDown(new Domain.KeyboardEventArgs(args, Keyboard.GetState())); };
         //window.KeyUp += (sender, args) => { OnWindowInputKeyUp(new Domain.KeyboardEventArgs(args, Keyboard.GetState())); };
      }

      public static IWindow Create( ) {
         return Create(null);
      }

      public static IWindow Create(string caption) {
         return new Window(caption, DefaultFrameRate);
      }

      public void Dispose( ) {
         window.Dispose();
      }

      public void Run( ) {
         window.Run(mFrameRate);
      }

      public void Exit( ) {
         window.Close();
      }

      public int LoadTexture(string file) {
         Bitmap bitmap = new Bitmap(file);

         int tex = 0;
         //GL.Hint(HintTarget.PerspectiveCorrectionHint, HintMode.Nicest);

         //GL.GenTextures(1, out tex);
         //GL.BindTexture(TextureTarget.Texture2D, tex);

         BitmapData data = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height),
             ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);

         //GL.TexImage2D(TextureTarget.Texture2D, 0, 
         //   PixelInternalFormat.Rgba, data.Width, data.Height, 0,
         //    PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);
         //bitmap.UnlockBits(data);

         //GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
         //GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
         //GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
         //GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);

         return tex;
      }

      //protected virtual void OnWindowLoad(EventArgs args) {
      //   Load?.Invoke();
      //}

      //protected virtual void OnWindowResize(EventArgs args) {
      //   Resize?.Invoke(window.ClientRectangle.X, window.ClientRectangle.Y, window.ClientRectangle.Width, window.ClientRectangle.Height);
      //}

      //protected virtual void OnWindowFrameUpdate(Domain.FrameEventArgs args) {
      //   FrameUpdate?.Invoke(args);
      //}

      //protected virtual void OnWindowFrameRender(Domain.FrameEventArgs args) {
      //   FrameRender?.Invoke(args);
      //}

      //protected virtual void OnWindowClosing(CancelEventArgs args) {
      //   Closing?.Invoke(args);
      //}

      //protected virtual void OnWindowInputKeyDown(Domain.KeyboardEventArgs args) {
      //   KeyDown?.Invoke(args);
      //}

      //protected virtual void OnWindowInputKeyUp(Domain.KeyboardEventArgs args) {
      //   KeyUp?.Invoke(args);
      //}
   }
}
