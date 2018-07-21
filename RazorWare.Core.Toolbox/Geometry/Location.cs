namespace RazorWare.Geometry {
   public struct Location<TVector> {

      public TVector X { get; }
      public TVector Y { get; }

      public Location(TVector pointX, TVector pointY) {
         X = pointX;
         Y = pointY;
      }

   }
}
