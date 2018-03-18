using System;

namespace RazorWare.Collections {
   public class Sortable<TType> {
      public interface ISortable {
         Comparison<TType> Comparer { get; set; }

         void Sort( );
      }
   }
}
