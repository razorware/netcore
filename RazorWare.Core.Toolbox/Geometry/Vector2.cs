using System;

namespace RazorWare.Geometry {
   public struct Vector2 {
      private readonly Vector[] coords;

      public double X => coords[0].Magnitude;
      public double Y => coords[1].Magnitude;

      public Vector2(double x, double y) {
         coords = new Vector[] { x, y };
      }

      /// <summary>
      /// Initializes a Vector2 from an int value tuple.
      /// </summary>
      /// <param name="tup">(int x, int y) value tuple</param>
      public static implicit operator Vector2((int x, int y) tup) {
         return new Vector2(tup.x, tup.y);
      }

      /// <summary>
      /// Initializes a Vector2 from a double value tuple.
      /// </summary>
      /// <param name="tup">(double x, double y) value tuple</param>
      public static implicit operator Vector2((double x, double y) tup) {
         return new Vector2(tup.x, tup.y);
      }

      /// <summary>
      /// Initializes a Vector2 from a direction (degrees & magnitude)
      /// </summary>
      /// <param name="dir">(Degrees d, Vector v) directional tuple</param>
      public static implicit operator Vector2((Degrees d, Vector v) dir) {
         var x = dir.v.Magnitude * dir.d.Cos();
         var y = dir.v.Magnitude * dir.d.Sin();

         return new Vector2(x, y);
      }

      public static Vector2 operator +(Vector2 v1, Vector2 v2) {
         return new Vector2(v1.X + v2.X, v1.Y + v2.Y);
      }

      /// <summary>
      /// Sum a Vector2 and a directional vector (degrees & magnitude)
      /// </summary>
      /// <param name="v1">Vector2 origin</param>
      /// <param name="">directional vector (degrees & magnitude)</param>
      /// <returns></returns>
      public static Vector2 operator +(Vector2 v1, (Degrees d, Vector v) dir) {
         Vector2 v2 = dir;

         return new Vector2(v1.X + v2.X, v1.Y + v2.Y);
      }

      public static Vector2 operator -(Vector2 v1, Vector2 v2) {
         return new Vector2(v1.X - v2.X, v1.Y - v2.Y);
      }

   }
}
