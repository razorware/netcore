using System;
using System.Text;
using System.Drawing;
using System.Collections.Generic;

namespace RazorWare.Geometry {
   public static class GeometryExtensionMethods {

      public static Point ToPoint(this Location<int> location) {
         return new Point(location.X, location.Y);
      }

      public static Size ToSize(this Size<int> size) {
         return new Size(size.Width, size.Height);
      }
   }
}
