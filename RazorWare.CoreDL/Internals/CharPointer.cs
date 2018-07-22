using System;
using System.Text;

namespace RazorWare.CoreDL.Internals {
   using RazorWare.CoreDL.Core;

   internal unsafe struct CharPointer {
      private readonly IntPtr pointer;

      public static CharPointer Null => new CharPointer(IntPtr.Zero);

      private CharPointer(IntPtr charPointer) {
         pointer = charPointer;
      }

      public static bool operator ==(CharPointer cp1, CharPointer cp2) {
         return cp1.pointer == cp2.pointer;
      }

      public static bool operator !=(CharPointer cp1, CharPointer cp2) {
         return cp1.pointer != cp2.pointer;
      }

      public static implicit operator CharPointer(Func<IntPtr> getCharPointer) {
         return new CharPointer(getCharPointer());
      }

      public static implicit operator string(CharPointer cp) {
         if (cp.pointer == IntPtr.Zero) {
            return null;
         }

         byte* ptr = (byte*)cp.pointer;
         while (*ptr != 0) {
            ptr++;
         }

         return new string((sbyte*)cp.pointer, 0, (int)(ptr - (byte*)cp.pointer), Fixtures.Encoder);
      }

      /// <summary>
      /// Pins a string without using Encoding.UTF8.GetBytes
      /// </summary>
      /// <param name="s">string to pin</param>
      public static implicit operator CharPointer(string s) {
         var i = 0;

         fixed (byte* ptr = new byte[s.Length + 1]) {
            // copy s to ptr
            while (i < s.Length) {
               *(ptr + i) = (byte)s[i];

               ++i;
            }

            // write string termination
            *(ptr + i) = 0;

            return new CharPointer(new IntPtr(ptr));
         }
      }

      public override bool Equals(object obj) {
         if (!(obj is CharPointer)) {
            return false;
         }

         CharPointer other = (CharPointer)obj;

         return pointer == other.pointer;
      }

      public override int GetHashCode( ) {
         return pointer.GetHashCode();
      }

      /// <summary>
      /// Use when SDL resource needs to be freed
      /// </summary>
      public void Free( ) {
         SDLI.SDL_Free(pointer);
      }
   }
}
