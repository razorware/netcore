using System.Collections.Generic;

namespace RazorWare.Collections.Specialized {
   public interface IStack<TResult> : IEnumerator<TResult>, IEnumerable<TResult> {
      bool Peek(out TResult value);
   }
}