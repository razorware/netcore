   using System;

namespace RazorWare.CoreGL.Input {
   public struct Key : IEquatable<Key> {
      internal const int NumKeys = 105;

      public static Key Unknown = (0, "Unknown", false);
      public static Key LShift = (1, "LShift", true);
      public static Key RShift = (2, "RShift", true);
      public static Key LControl = (3, "LControl", true);
      public static Key RControl = (4, "RControl", true);
      public static Key LAlt = (5, "LAlt", true);
      public static Key RAlt = (6, "RAlt", true);
      public static Key LWin = (7, "LWin", false);
      public static Key RWin = (8, "RWin", false);
      public static Key Menu = (9, "Menu", false);
      public static Key F1 = (10, "F1", false);
      public static Key F2 = (11, "F2", false);
      public static Key F3 = (12, "F3", false);
      public static Key F4 = (13, "F4", false);
      public static Key F5 = (14, "F5", false);
      public static Key F6 = (15, "F6", false);
      public static Key F7 = (16, "F7", false);
      public static Key F8 = (17, "F8", false);
      public static Key F9 = (18, "F9", false);
      public static Key F10 = (19, "F10", false);
      public static Key F11 = (20, "F11", false);
      public static Key F12 = (21, "F12", false);
      public static Key Up = (22, "Up", false);
      public static Key Down = (23, "Down", false);
      public static Key Left = (24, "Left", false);
      public static Key Right = (25, "Right", false);
      public static Key Enter = (26, "Enter", false);
      public static Key Escape = (27, "Escape", false);
      public static Key Space = (28, "Space", false);
      public static Key Tab = (29, "Tab", false);
      public static Key BackSpace = (30, "BackSpace", false);
      public static Key Insert = (31, "Insert", false);
      public static Key Delete = (32, "Delete", false);
      public static Key PageUp = (33, "PageUp", false);
      public static Key PageDown = (34, "PageDown", false);
      public static Key Home = (35, "Home", false);
      public static Key End = (36, "End", false);
      public static Key CapsLock = (37, "CapsLock", false);
      public static Key ScrollLock = (38, "ScrollLock", false);
      public static Key PrintScreen = (39, "PrintScreen", false);
      public static Key Pause = (40, "Pause", false);
      public static Key NumLock = (41, "NumLock", false);
      public static Key Keypad0 = (42, "Keypad0", false);
      public static Key Keypad1 = (43, "Keypad1", false);
      public static Key Keypad2 = (44, "Keypad2", false);
      public static Key Keypad3 = (45, "Keypad3", false);
      public static Key Keypad4 = (46, "Keypad4", false);
      public static Key Keypad5 = (47, "Keypad5", false);
      public static Key Keypad6 = (48, "Keypad6", false);
      public static Key Keypad7 = (49, "Keypad7", false);
      public static Key Keypad8 = (50, "Keypad8", false);
      public static Key Keypad9 = (51, "Keypad9", false);
      public static Key KeypadDivide = (52, "KeypadDivide", false);
      public static Key KeypadMultiply = (53, "KeypadMultiply", false);
      public static Key KeypadMinus = (54, "KeypadMinus", false);
      public static Key KeypadPlus = (55, "KeypadPlus", false);
      public static Key KeypadPeriod = (56, "KeypadPeriod", false);
      public static Key KeypadEnter = (57, "KeypadEnter", false);
      public static Key A = (58, "A", false);
      public static Key B = (59, "B", false);
      public static Key C = (60, "C", false);
      public static Key D = (61, "D", false);
      public static Key E = (62, "E", false);
      public static Key F = (63, "F", false);
      public static Key G = (64, "G", false);
      public static Key H = (65, "H", false);
      public static Key I = (66, "I", false);
      public static Key J = (67, "J", false);
      public static Key K = (68, "K", false);
      public static Key L = (69, "L", false);
      public static Key M = (70, "M", false);
      public static Key N = (71, "N", false);
      public static Key O = (72, "O", false);
      public static Key P = (73, "P", false);
      public static Key Q = (74, "Q", false);
      public static Key R = (75, "R", false);
      public static Key S = (76, "S", false);
      public static Key T = (77, "T", false);
      public static Key U = (78, "U", false);
      public static Key V = (79, "V", false);
      public static Key W = (80, "W", false);
      public static Key X = (81, "X", false);
      public static Key Y = (82, "Y", false);
      public static Key Z = (83, "Z", false);
      public static Key Number0 = (84, "Number0", false);
      public static Key Number1 = (85, "Number1", false);
      public static Key Number2 = (86, "Number2", false);
      public static Key Number3 = (87, "Number3", false);
      public static Key Number4 = (88, "Number4", false);
      public static Key Number5 = (89, "Number5", false);
      public static Key Number6 = (90, "Number6", false);
      public static Key Number7 = (91, "Number7", false);
      public static Key Number8 = (92, "Number8", false);
      public static Key Number9 = (93, "Number9", false);
      public static Key Tilde = (94, "Tilde", false);
      public static Key Minus = (95, "Minus", false);
      public static Key Plus = (96, "Plus", false);
      public static Key LBracket = (97, "LBracket", false);
      public static Key RBracket = (98, "RBracket", false);
      public static Key Semicolon = (99, "Semicolon", false);
      public static Key Quote = (100, "Quote", false);
      public static Key Comma = (101, "Comma", false);
      public static Key Period = (102, "Period", false);
      public static Key Slash = (103, "Slash", false);
      public static Key BackSlash = (104, "BackSlash", false);

      private readonly byte mask;
      private readonly bool isMod;
      private readonly string name;

      public bool IsKeyModifier => isMod;

      private Key(byte keyMask, string keyName, bool isKeyMod) {
         mask = keyMask;
         isMod = isKeyMod;
         name = keyName;
      }

      public static implicit operator Key((byte mask, string name, bool isMod) keyInfo) {
         return new Key(keyInfo.mask, keyInfo.name, keyInfo.isMod);
      }

      public static implicit operator byte(Key key) {
         return key.mask;
      }

      public static bool operator ==(Key k1, Key k2) {
         return k1.mask == k2.mask;
      }

      public static bool operator !=(Key k1, Key k2) {
         return k1.mask != k2.mask;
      }

      public override bool Equals(object obj) {
         if (!(obj is Key)) {
            return false;
         }

         return Equals((Key)obj);
      }

      public bool Equals(Key other) {
         return mask == other.mask;
      }

      public override int GetHashCode( ) {
         return mask.GetHashCode();
      }

      public override string ToString( ) {
         return name;
      }
   }
}
