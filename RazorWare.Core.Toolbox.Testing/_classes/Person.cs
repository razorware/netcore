using System;

namespace RazorWare.Testing._classes {
   public class Person {
      private static int mIdCount = 0;

      public int Id { get; set; }
      public string Forename { get; set; }
      public string MiddleName { get; set; }
      public string Surname { get; set; }
      public DateTime Birthdate { get; set; }
      public DateTime Created { get; set; }

      public Person( ) {
         Id = mIdCount++;
         Created = DateTime.Now;
      }
      public Person(string fName, string mName, string lName, DateTime birthdate): this() {
         Forename = fName;
         MiddleName = mName;
         Surname = lName;
         Birthdate = birthdate;
      }

      public override bool Equals(object obj) {
         if(!(obj is Person)) {
            return false;
         }

         var other = (Person)obj;

         return Id == other.Id &&
            Forename == other.Forename &&
            MiddleName == other.MiddleName &&
            Surname == other.Surname &&
            Birthdate == other.Birthdate &&
            Created == other.Created;
      }

      public override int GetHashCode( ) {
         return base.GetHashCode();
      }

      public override string ToString( ) {
         return $"{Surname}, {Forename} " + 
            $"{(string.IsNullOrEmpty(MiddleName) ? string.Empty : $"{MiddleName[0]}")}";
      }
   }
}
