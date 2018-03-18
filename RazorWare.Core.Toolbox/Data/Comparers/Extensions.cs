using System;
using System.Collections;
using System.Collections.Generic;

namespace RazorWare.Data.Comparers {
   public static class Extensions {

      public static IComparer GetComparer(this TypeCode typeCode) {
         switch (typeCode) {
            case TypeCode.Boolean:
               return Comparer<bool>.Default;
            case TypeCode.SByte:
               return Comparer<sbyte>.Default;
            case TypeCode.Byte:
               return Comparer<byte>.Default;
            case TypeCode.Char:
               return Comparer<char>.Default;
            case TypeCode.DateTime:
               return Comparer<DateTime>.Default;
            case TypeCode.Decimal:
               return Comparer<decimal>.Default;
            case TypeCode.Double:
               return Comparer<double>.Default;
            case TypeCode.Int16:
               return Comparer<short>.Default;
            case TypeCode.Int32:
               return Comparer<int>.Default;
            case TypeCode.Int64:
               return Comparer<long>.Default;
            case TypeCode.Single:
               return Comparer<float>.Default;
            case TypeCode.String:
               return Comparer<string>.Default;
            case TypeCode.UInt16:
               return Comparer<ushort>.Default;
            case TypeCode.UInt32:
               return Comparer<uint>.Default;
            case TypeCode.UInt64:
               return Comparer<ulong>.Default;
            case TypeCode.DBNull:
            case TypeCode.Empty:
            case TypeCode.Object:
            default:
               return Comparer.Default;
         }
      }
   }
}
