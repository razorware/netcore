using System;

namespace RazorWare.CoreGL.Devices {
   using RazorWare.CoreGL.Input;
   using RazorWare.CoreGL.Utility;

   public class Keyboard {
      private static readonly Lazy<Keyboard> mKeyboard = new Lazy<Keyboard>(( ) => new Keyboard());

      //private readonly KeyboardDevice tkKbd;

      private KeyboardState keyboardState = new KeyboardState();

      //public static KeyboardDevice InputKeyboard { get; set; }

      public static Keyboard Device => mKeyboard.Value;

      private Keyboard( ) {
         //tkKbd = InputKeyboard;
         //tkKbd.KeyDown += KeyboardKeyDown;
         //tkKbd.KeyUp += KeyboardKeyUp;
      }

      public bool this[Key key] {
         get {
            return false; //tkKbd[key]; 
         }
      }

      public bool this[uint scancode] {
         get {
            return false; //tkKbd[scancode];
         }
      }

      public int NumberOfKeys { get; }
      public int NumberOfFunctionKeys { get; }
      public int NumberOfLeds { get; }
      public IntPtr DeviceID { get; }
      //public bool KeyRepeat {
      //   get { return tkKbd.KeyRepeat; }
      //   set { tkKbd.KeyRepeat = value; }
      //}
      //public string Description {
      //   get { return tkKbd.Description; }
      //}
      public InputDeviceType DeviceType {
         get { return InputDeviceType.Keyboard; }
      }

      //
      // Summary:
      //     Occurs when a key is pressed.
      public event EventHandler<KeyboardEventArgs> KeyDown;
      //
      // Summary:
      //     Occurs when a key is released.
      public event EventHandler<KeyboardEventArgs> KeyUp;

      public override int GetHashCode( ) {
         return base.GetHashCode();
      }
      //
      // Summary:
      //     Retrieves the combined OpenTK.Input.KeyboardState for all keyboard devices. This
      //     method is equivalent to OpenTK.Input.Keyboard.GetState.
      //
      // Returns:
      //     An OpenTK.Input.KeyboardState structure containing the combined state for all
      //     keyboard devices.
      public KeyboardState GetState( ) {
         return keyboardState;
      }
      //
      // Summary:
      //     Retrieves the OpenTK.Input.KeyboardState for the specified keyboard device. This
      //     method is equivalent to OpenTK.Input.Keyboard.GetState(System.Int32).
      //
      // Parameters:
      //   index:
      //     The index of the keyboard device.
      //
      // Returns:
      //     An OpenTK.Input.KeyboardState structure containing the combined state for all
      //     keyboard devices.
      public KeyboardState GetState(int index) {
         return keyboardState;
      }
      //
      // Summary:
      //     Returns a System.String representing this KeyboardDevice.
      //
      // Returns:
      //     A System.String representing this KeyboardDevice.
      public override string ToString( ) {
         return "Generic Keyboard";
      }

      public void SetLogger(ILogger logger) {
         keyboardState.Logger(logger);
      }

      //private void KeyboardKeyUp(object sender, OpenTK.Input.KeyboardKeyEventArgs args) {
      //   KeyUp?.Invoke(sender, new KeyboardEventArgs(args, GetState()));
      //}

      //private void KeyboardKeyDown(object sender, OpenTK.Input.KeyboardKeyEventArgs args) {
      //   KeyDown?.Invoke(sender, new KeyboardEventArgs(args, GetState()));
      //}
   }
}
