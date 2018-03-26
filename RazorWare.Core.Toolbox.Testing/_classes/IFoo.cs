using System;

namespace RazorWare.Testing._classes {
   public interface IFoo {
      int Id { get; }
      string Name { get; set; }

      int GetNum( );
      DayOfWeek GetDay( );
   }
}
