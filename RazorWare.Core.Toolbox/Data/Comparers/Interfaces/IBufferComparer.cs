namespace RazorWare.Data.Comparers {
   public interface IBufferComparer {
        int Index { get; }

        ComparerResult Equals(byte[] buff1, byte[] buff2);
    }
}