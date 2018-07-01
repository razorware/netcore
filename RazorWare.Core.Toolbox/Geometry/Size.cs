namespace RazorWare.Geometry {
   public struct Size<TLength> {

      public TLength Width { get; }
      public TLength Height { get; }

      public Size(TLength width, TLength height) {
         Width = width;
         Height = height;
      }
   }
}
