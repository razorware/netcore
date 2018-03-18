using System;
using System.IO;
using System.Linq;
using static RazorWare.Reflection.ReflectionExtensions;

namespace RazorWare.Data.Annotations {
#pragma warning disable CS0660 // Type defines operator == or operator != but does not override Object.Equals(object o)
#pragma warning disable CS0661 // Type defines operator == or operator != but does not override Object.GetHashCode()
   public unsafe struct DataType {
#pragma warning restore CS0661 // Type defines operator == or operator != but does not override Object.GetHashCode()
#pragma warning restore CS0660 // Type defines operator == or operator != but does not override Object.Equals(object o)
      internal const int TYPESIZE = 5;

      private static byte[] validTypes = new byte[] {
         0x00, 0x01, 0x02, 0x03, 0x04,
         0x05, 0x06, 0x07, 0x08, 0x09,
         0x0A, 0x0B, 0x0C, 0x0D, 0x0D,
         0x0E, 0x0F, 0x10,
         0xFF
      };

      fixed byte type[TYPESIZE];

      public static DataType None = new DataType(0xFF, 0);
      public static DataType Null = new DataType(0x00, 0);
      public static DataType Boolean = new DataType(0x01, SZBYTE);
      public static DataType Byte = new DataType(0x02, SZBYTE);
      public static DataType SByte = new DataType(0x03, SZBYTE);
      public static DataType Char = new DataType(0x04, SZCHAR);
      public static DataType Short = new DataType(0x05, SZSHORT);
      public static DataType UShort = new DataType(0x06, SZSHORT);
      public static DataType Int = new DataType(0x07, SZINT);
      public static DataType UInt = new DataType(0x08, SZINT);
      public static DataType Float = new DataType(0x09, SZFLOAT);
      public static DataType Long = new DataType(0x0A, SZLONG);
      public static DataType ULong = new DataType(0x0B, SZLONG);
      public static DataType Double = new DataType(0x0C, SZDOUBLE);
      public static DataType TimeStamp = new DataType(0x0D, SZLONG);
      public static DataType String = new DataType(0x0E, SZINT);
      public static DataType Text = new DataType(0x0F, SZINT);
      public static DataType Blob = new DataType(0x10, SZINT);

      public int Size {
         get {
            fixed (byte* sz = type) {
               return BitConverter.ToInt32(new byte[]{
                  *(sz + 1), *(sz + 2), *(sz + 3), *(sz + 4)
               }, 0);
            }
         }
      }

      private DataType(byte t, int sz) : this(t, BitConverter.GetBytes(sz)) { }

      private DataType(byte t, byte[] szBuff) {
         if (!Array.Exists(validTypes, b => b == t)) {
            throw new ArgumentOutOfRangeException($"[{t}] is not a valid value for DbType");
         }

         fixed (byte* b = type) {
            *(b) = t;               // type byte
            *(b + 1) = szBuff[0];   // size (int)
            *(b + 2) = szBuff[1];   //     |
            *(b + 3) = szBuff[2];   //     |  
            *(b + 4) = szBuff[3];   //     -
         }
      }

      public static implicit operator DataType(Type type) {
         return Type.GetTypeCode(type);
      }

      public static implicit operator DataType(TypeCode typeCode) {
         switch (typeCode) {
            case TypeCode.Boolean:
               return Boolean;
            case TypeCode.SByte:
               return SByte;
            case TypeCode.Byte:
               return Byte;
            case TypeCode.Char:
               return Char;
            case TypeCode.DateTime:
               return TimeStamp;
            case TypeCode.DBNull:
               return Null;
            case TypeCode.Decimal:
            case TypeCode.Double:
               return Double;
            case TypeCode.Int16:
               return Short;
            case TypeCode.UInt16:
               return UShort;
            case TypeCode.Int32:
               return Int;
            case TypeCode.UInt32:
               return UInt;
            case TypeCode.Int64:
               return Long;
            case TypeCode.UInt64:
               return ULong;
            case TypeCode.Single:
               return Float;
            case TypeCode.String:
               return String;
            case TypeCode.Object:
            case TypeCode.Empty:
            default:
               return None;
         }
      }

      public static implicit operator byte(DataType dbType) {
         return *dbType.type;
      }

      public static implicit operator byte[] (DataType dbType) {
         var buffer = new byte[5];

         buffer[0] = *(dbType.type);
         buffer[1] = *(dbType.type + 1);
         buffer[2] = *(dbType.type + 2);
         buffer[3] = *(dbType.type + 3);
         buffer[4] = *(dbType.type + 4);

         return buffer;
      }

      public static implicit operator DataType(byte[] tBuff) {
         if (tBuff.Length != 5) {
            throw new ArgumentException($"Buffer length must be exactly 5 bytes [{tBuff.Length}]");
         }

         var t = tBuff[0];
         var szBuff = tBuff.Skip(1).ToArray();

         return new DataType(t, szBuff);
      }

      public static bool operator ==(DataType dbType, byte b) {
         return (*dbType.type) == b;
      }

      public static bool operator !=(DataType dbType, byte b) {
         return (*dbType.type) != b;
      }

      internal static DataType Resize(DataType dbType, int sz) {
         var t = *dbType.type;

         // can only be resized for String
         if (t != 0x0E) {
            throw new InvalidOperationException($"Cannot resize DbType [{dbType}]");
         }

         return new DataType(t, sz);
      }

      internal static DataType FromBinaryReader(BinaryReader bReader) {
         var buffer = new byte[TYPESIZE];
         bReader.Read(buffer, 0, TYPESIZE);

         return buffer;
      }

      public override string ToString( ) {
         fixed(byte* t = type) {
            return $"{ValueExtensions.Name(this)}[{*t}]";
         }
      }
   }
}
