namespace RazorWare.Collections.Specialized {
   public interface IByteStack : IStack<byte> {
      sbyte State { get; }
      int Index { get; }
   }
}