using System;

namespace RazorWare.CoreGL {
   using RazorWare.CoreGL.Input;
   //using RazorWare.CoreGL.Domain.Entities;
   //using RazorWare.CoreGL.Domain.ValueObjects;

   public class KeyboardEventArgs : EventArgs {

      internal KeyboardEventArgs(KeyboardState state) {
         //Key = state.Key;
         //ScanCode = state.Key.ScanCode();
         //Alt = args.Alt;
         //Control = args.Control;
         //Shift = args.Shift;
         //KeyboardState = state;
         //IsRepeat = args.IsRepeat;
      }

      public Key Key { get; }
      public uint ScanCode { get; }
      public bool Alt { get; }
      public bool Control { get; }
      public bool Shift { get; }
      //public KeyModifiers Modifiers { get; }
      public KeyboardState KeyboardState { get; }
      public bool IsRepeat { get; }
   }
}
