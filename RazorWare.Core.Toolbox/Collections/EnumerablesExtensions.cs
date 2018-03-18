using System.Linq;
using System.Collections.Generic;
using System;

namespace RazorWare.Collections {
   public static class EnumerablesExtensions {

      public static (bool, TResult) FromDictionary<TKey, TValue, TResult>(this Dictionary<TKey, TValue> source, TKey key, Func<TValue, TResult> getFunc) {
         var result = default(TResult);
         var hasResult = false;

         if(source.TryGetValue(key, out TValue value)) {
            result = getFunc(value);
            hasResult = true;
         }

         return (hasResult, result);
      }
   }
}
