using System;

namespace RazorWare.Geometry {
   public unsafe struct Vector {
      private fixed double mMag[1];
      private readonly bool notEmpty;

      public double Magnitude => GetMagnitude();

      public Vector(double magnitude) {
         fixed(double* m = mMag) {
            m[0] = magnitude;
         }

         notEmpty = true;
      }

      public static implicit operator Vector(double d) {
         return new Vector(d);
      }

      private double GetMagnitude( ) {
         var magnitude = 0D;
         fixed (double* m = mMag) {
            magnitude = *m;
         }

         return magnitude;
      }
   }
}
