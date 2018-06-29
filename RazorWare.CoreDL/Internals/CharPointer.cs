using System;

namespace RazorWare.CoreDL.Internals {
   using RazorWare.CoreDL.Core;

   internal unsafe struct CharPointer {
      private readonly IntPtr pointer;

      private CharPointer(IntPtr charPointer) {
         pointer = charPointer;
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

         return new string((sbyte*)cp.pointer, 0, (int)(ptr - (byte*)cp.pointer), Constants.Encoder);
      }

      public void Free( ) {
         SDLI.SDL_Free(pointer);
      }
   }
}
