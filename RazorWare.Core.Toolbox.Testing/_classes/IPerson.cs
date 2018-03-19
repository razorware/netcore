using System;

namespace RazorWare.Testing._classes {
   public interface IPerson {
      DateTime Birthdate { get; set; }
      DateTime Created { get; set; }
      string Forename { get; set; }
      int Id { get; set; }
      string MiddleName { get; set; }
      string Surname { get; set; }

      bool Equals(object obj);
      int GetHashCode( );
      string ToString( );
   }
}