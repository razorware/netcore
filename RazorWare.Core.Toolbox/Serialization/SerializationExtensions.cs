using System;
using RazorWare.Data;
using RazorWare.Data.Annotations;

namespace RazorWare.Serialization {
   public static class SerializationExtensions {

      public static byte[] Encode<TValue>(this TValue value) {
         return Type.GetTypeCode(typeof(TValue)).Encode(value);
      }

      public static byte[] Encode(this TypeCode typeCode, object value) {
         var buffer = default(byte[]);

         switch (typeCode) {
            case TypeCode.Boolean:
               buffer = BitConverter.GetBytes((bool)value);

               break;
            case TypeCode.Byte:
               buffer = BitConverter.GetBytes((byte)value);

               break;
            case TypeCode.Char:
               buffer = BitConverter.GetBytes((char)value);

               break;
            case TypeCode.DateTime:
               buffer = BitConverter.GetBytes(((DateTime)value).Ticks);

               break;
            case TypeCode.Decimal:
            case TypeCode.Double:
               buffer = BitConverter.GetBytes((double)value);

               break;
            case TypeCode.Int16:
               buffer = BitConverter.GetBytes((short)value);

               break;
            case TypeCode.Int32:
               buffer = BitConverter.GetBytes((int)value);

               break;
            case TypeCode.Int64:
               buffer = BitConverter.GetBytes((long)value);

               break;
            case TypeCode.SByte:
               buffer = BitConverter.GetBytes((sbyte)value);

               break;
            case TypeCode.Single:
               buffer = BitConverter.GetBytes((float)value);

               break;
            case TypeCode.String:
               buffer = System.Text.Encoding.UTF8.GetBytes((string)value);

               break;
            case TypeCode.UInt16:
               buffer = BitConverter.GetBytes((ushort)value);

               break;
            case TypeCode.UInt32:
               buffer = BitConverter.GetBytes((uint)value);

               break;
            case TypeCode.UInt64:
               buffer = BitConverter.GetBytes((ulong)value);

               break;
            case TypeCode.DBNull:
            case TypeCode.Empty:
            case TypeCode.Object:
            default:
               break;
         }

         return buffer;
      }

      public static byte[] Encode(this IField field, object value) {
         var dataType = field.Type;
         var buffer = default(byte[]);

         if (dataType == DataType.Boolean) {
            buffer = BitConverter.GetBytes((bool)value);
         }
         else if (dataType == DataType.Byte) {
            buffer = BitConverter.GetBytes((byte)value);
         }
         else if (dataType == DataType.SByte) {
            buffer = BitConverter.GetBytes((sbyte)value);
         }
         else if (dataType == DataType.Char) {
            buffer = BitConverter.GetBytes((char)value);
         }
         else if (dataType == DataType.TimeStamp) {
            buffer = BitConverter.GetBytes(((DateTime)value).Ticks);
         }
         else if (dataType == DataType.Double) {
            buffer = BitConverter.GetBytes((double)value);
         }
         else if (dataType == DataType.Short) {
            buffer = BitConverter.GetBytes((short)value);
         }
         else if (dataType == DataType.UShort) {
            buffer = BitConverter.GetBytes((ushort)value);
         }
         else if (dataType == DataType.Int) {
            buffer = BitConverter.GetBytes((int)value);
         }
         else if (dataType == DataType.UInt) {
            buffer = BitConverter.GetBytes((uint)value);
         }
         else if (dataType == DataType.Long) {
            buffer = BitConverter.GetBytes((long)value);
         }
         else if (dataType == DataType.ULong) {
            buffer = BitConverter.GetBytes((ulong)value);
         }
         else if (dataType == DataType.Float) {
            buffer = BitConverter.GetBytes((float)value);
         }
         else if (dataType == DataType.String) {
            //  - because string field is resizable, we have a 2-step process
            //  - if value is null and we make it this far, assume null value is allowed
            var bytes = System.Text.Encoding.UTF8.GetBytes((string)value ?? string.Empty);
            buffer = new byte[field.Size];
            Buffer.BlockCopy(bytes, 0, buffer, 0, bytes.Length);
         }
         else {
            // TODO: handle the following: 
            //       DataType.None
            //       DataType.Null
            //       DataType.Blob
            //       DataType.Text
            throw new NotSupportedException($"handling {dataType} is not yet supported");
         }

         return buffer;
      }

      public static TValue Decode<TValue>(this byte[] srcBuffer) {
         var value = default(object);

         switch (Type.GetTypeCode(typeof(TValue))) {
            case TypeCode.Boolean:
               value = BitConverter.ToBoolean(srcBuffer, 0);

               break;
            case TypeCode.SByte:
            case TypeCode.Byte:
               value = srcBuffer[0];

               break;
            case TypeCode.Char:
               value = BitConverter.ToChar(srcBuffer, 0);

               break;
            case TypeCode.DateTime:
               value = new DateTime(BitConverter.ToInt64(srcBuffer, 0));

               break;
            case TypeCode.Decimal:
            case TypeCode.Double:
               value = BitConverter.ToDouble(srcBuffer, 0);

               break;
            case TypeCode.Int16:
               value = BitConverter.ToInt16(srcBuffer, 0);

               break;
            case TypeCode.Int32:
               value = BitConverter.ToInt32(srcBuffer, 0);

               break;
            case TypeCode.Int64:
               value = BitConverter.ToInt64(srcBuffer, 0);

               break;
            case TypeCode.Single:
               value = BitConverter.ToSingle(srcBuffer, 0);

               break;
            case TypeCode.String:
               value = Token.Encoder.GetString(srcBuffer);

               break;
            case TypeCode.UInt16:
               value = BitConverter.ToUInt16(srcBuffer, 0);

               break;
            case TypeCode.UInt32:
               value = BitConverter.ToUInt32(srcBuffer, 0);

               break;
            case TypeCode.UInt64:
               value = BitConverter.ToUInt64(srcBuffer, 0);

               break;
            case TypeCode.DBNull:
            case TypeCode.Empty:
            case TypeCode.Object:
            default:
               break;
         }

         return (TValue)value;
      }

      public static TType AsType<TType>(this Token token) {
         return (TType)AsType(token, Type.GetTypeCode(typeof(TType)));
      }

      public static object AsType(this Token token, TypeCode typeCode) {
         var value = default(object);

         switch (typeCode) {
            case TypeCode.Boolean:
               value = BitConverter.ToBoolean(token, 0);

               break;
            case TypeCode.SByte:
            case TypeCode.Byte:
               value = token[0];

               break;
            case TypeCode.Char:
               value = BitConverter.ToChar(token, 0);

               break;
            case TypeCode.DateTime:
               value = new DateTime(BitConverter.ToInt64(token, 0));

               break;
            case TypeCode.Decimal:
            case TypeCode.Double:
               value = BitConverter.ToDouble(token, 0);

               break;
            case TypeCode.Int16:
               value = BitConverter.ToInt16(token, 0);

               break;
            case TypeCode.Int32:
               value = BitConverter.ToInt32(token, 0);

               break;
            case TypeCode.Int64:
               value = BitConverter.ToInt64(token, 0);

               break;
            case TypeCode.Single:
               value = BitConverter.ToSingle(token, 0);

               break;
            case TypeCode.String:
               value = Token.Encoder.GetString(token);

               break;
            case TypeCode.UInt16:
               value = BitConverter.ToUInt16(token, 0);

               break;
            case TypeCode.UInt32:
               value = BitConverter.ToUInt32(token, 0);

               break;
            case TypeCode.UInt64:
               value = BitConverter.ToUInt64(token, 0);

               break;
            case TypeCode.DBNull:
            case TypeCode.Empty:
            case TypeCode.Object:
            default:
               break;
         }

         return value;
      }

      public static Token Serialize(this ISerializable serializable) {
         var buffer = default(byte[]);


         return buffer;
      }
   }
}
