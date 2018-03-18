using System;
using System.Collections.Generic;
using System.Text;

namespace RazorWare.Geometry
{
    public unsafe struct Degrees
    {
      const double Max = 360;

      private fixed double deg[1];

      private Degrees(double val) {
         fixed(double* d = deg) {
            d[0] = val;
         }
      }

      public double Cos( ) {
         fixed(double* d = deg) {
            return Math.Cos(*d);
         }
      }

      public double Sin( ) {
         fixed (double* d = deg) {
            return Math.Sin(*d);
         }
      }

      public double Tan( ) {
         fixed (double* d = deg) {
            return Math.Tan(*d);
         }
      }

      public static implicit operator double(Degrees d) {
         return *d.deg;
      }

      public static implicit operator Degrees(double d) {
         return new Degrees(d);
      }

      public static Degrees operator +(Degrees d1, Degrees d2) {
         return new Degrees(*d1.deg + *d2.deg);
      }

      public static Degrees operator +(Degrees d1, int i) {
         return new Degrees(*d1.deg + i);
      }

      public static Degrees operator +(Degrees d1, float f) {
         return new Degrees(*d1.deg + f);
      }

      public static Degrees operator +(Degrees d1, double d) {
         return new Degrees(*d1.deg + d);
      }

      public static Degrees operator -(Degrees d1, Degrees d2) {
         return new Degrees(*d1.deg - *d2.deg);
      }

      public static Degrees operator -(Degrees d1, int i) {
         return new Degrees(*d1.deg - i);
      }

      public static Degrees operator -(Degrees d1, float f) {
         return new Degrees(*d1.deg - f);
      }

      public static Degrees operator -(Degrees d1, double d) {
         return new Degrees(*d1.deg - d);
      }

      public static Degrees Difference(Degrees d1, Degrees d2) {
         return Math.Abs(*d1.deg - *d2.deg);
      }
    }
}
