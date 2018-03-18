using System.Collections.Generic;

namespace RazorWare.Data.Comparers {
   public class BufferComparer : IEqualityComparer<byte[]>, IBufferComparer {

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

                if(pass) {
                    Index = idx;
                }

                ++idx;
            }

            return new ComparerResult(pass, Index);
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
