namespace RazorWare.Collections.Specialized {
   public interface IBufferStack {
      sbyte State { get; }
      byte[] Current { get; }
      int Index { get; }
      int Element { get; }

      bool MoveNext( );
      bool NextBuffer( );
      bool Peek(out byte value);
      bool Peek(out byte[] buffer);
   }
}