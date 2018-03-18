using System;
using System.Linq;
using System.Collections.Generic;

namespace RazorWare.Linq {
   public static class LinqExtensions {

      public static TResult[] ToArray<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TResult> func) {
         return source
            .Select(i => func(i))
            .ToArray();
      }

      public static List<TResult> ToList<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TResult> func) {
         return source
            .Select(i => func(i))
            .ToList();
      }

      public static IEnumerable<TSource> Iterate<TSource>(this IEnumerable<TSource> source, Action<TSource> action) {
         var iterator = source.GetEnumerator();
         while (iterator.MoveNext()) {
            action(iterator.Current);

            yield return iterator.Current;
         }
      }

      public static void Go<TSource>(this IEnumerable<TSource> source) {
         var iterator = source.GetEnumerator();
         while (iterator.MoveNext()) { }
      }
   }
}
