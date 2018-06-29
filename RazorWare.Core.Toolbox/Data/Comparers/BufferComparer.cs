using System.Collections.Generic;

namespace RazorWare.Data.Comparers {
   public class BufferComparer : IBufferComparer {

      public int Index { get; private set; }

      bool IEqualityComparer<byte[]>.Equals(byte[] buff1, byte[] buff2) {
         return Equals(buff1, buff2).Pass;
      }

      public ComparerResult Equals(byte[] buff1, byte[] buff2) {
         Index = -1;
         var pass = buff1.Length == buff2.Length;

         var idx = 0;
         while (pass && (idx < buff1.Length)) {
            pass &= buff1[idx] == buff2[idx];

            if (pass) {
               Index = idx;
            }

            ++idx;
         }

         return new ComparerResult(pass, Index);
      }

      public unsafe bool Equals(byte* buff1, byte* buff2, int length) {
         var idx = 0;
         var pass = true;
         while(pass && (idx < length)) {
            pass &= buff1[idx] == buff2[idx];

            ++idx;
         }

         return pass;
      }

      internal void Reset( ) {
         Index = -1;
      }

      public int GetHashCode(byte[] obj) {
         var hash = -1;

         unchecked {
            hash = 0;
            for (var i = 0; i < obj.Length; i++)
               hash += obj[i].GetHashCode() * (i + 1);
         }

         return hash;
      }
   }
}
