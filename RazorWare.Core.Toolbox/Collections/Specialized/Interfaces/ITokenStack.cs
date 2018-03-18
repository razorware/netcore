using RazorWare.Serialization;

namespace RazorWare.Collections.Specialized {
   public interface ITokenStack : IStack<Token> {
      string Word { get; }
      new byte[] Current { get; }
      Token Token { get; }

      sbyte State { get; }

      string ToString( );
   }
}