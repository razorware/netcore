using System;
using RazorWare.Data;

namespace RazorWare.Reflection {
   public static class ReflectionExtensions {
      internal const byte SZBYTE = sizeof(byte);
      internal const byte SZCHAR = sizeof(char);
      internal const byte SZSHORT = sizeof(short);
      internal const byte SZINT = sizeof(int);
      internal const byte SZFLOAT = sizeof(float);
      internal const byte SZLONG = sizeof(long);
      internal const byte SZDOUBLE = sizeof(double);

      public static byte Size(this TypeCode typeCode) {
         var size = default(byte);

         switch (typeCode) {
            case TypeCode.Boolean:
            case TypeCode.Byte:
            case TypeCode.SByte:
               size = SZBYTE;

               break;
            case TypeCode.Char:
               size = SZCHAR;

               break;
            case TypeCode.DateTime:
            case TypeCode.Int64:
            case TypeCode.UInt64:
               size = SZLONG;

               break;
            case TypeCode.Decimal:
            case TypeCode.Double:
               size = SZDOUBLE;

               break;
            case TypeCode.Int16:
            case TypeCode.UInt16:
               size = SZSHORT;

               break;
            case TypeCode.Int32:
            case TypeCode.UInt32:
               size = SZINT;

               break;
            case TypeCode.Single:
               size = SZFLOAT;

               break;
            case TypeCode.String:
            case TypeCode.DBNull:
            case TypeCode.Empty:
            case TypeCode.Object:
            default:
               break;
         }

         return size;
      }

      public static TypeData GetTypeData<TType>(this TType obj) {
         return GetTypeData(typeof(TType));
      }

      public static TypeData GetTypeData(this Type type) {
         switch (type.Name) {
            case "DBNull":
               return TypeData.Null;
            case "Byte":
               return TypeData.Byte;
            case "SByte":
               return TypeData.SByte;
            case "Boolean":
               return TypeData.Bool;
            case "Int16":
               return TypeData.Int16;
            case "UInt16":
               return TypeData.Uint16;
            case "Int32":
               return TypeData.Int32;
            case "UInt32":
               return TypeData.UInt32;
            case "Int64":
               return TypeData.Int64;
            case "UInt64":
               return TypeData.UInt64;
            case "Single":
               return TypeData.Float;
            case "Decimal":
               return TypeData.Decimal;
            case "Double":
               return TypeData.Double;
            case "DateTime":
               return TypeData.DateTime;
            case "String":
               return TypeData.String;
            case "Object":
            default:
               return new TypeData(type);
         }
      }
   }
}
