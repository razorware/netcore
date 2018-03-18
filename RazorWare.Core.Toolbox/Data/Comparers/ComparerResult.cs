namespace RazorWare.Data.Comparers {
   public class ComparerResult {

      public bool Pass { get; }
      public int Index { get; }

      public ComparerResult(bool result, int index) {
         Pass = result;
         Index = index;
      }

      public static implicit operator ComparerResult(bool result) {
         return new ComparerResult(result, result ? 0 : -1);
      }

      public static implicit operator bool(ComparerResult result) {
         return result.Pass;
      }
   }
}
