using System;

namespace RazorWare.Reflection {
   public class TypeData : IEquatable<Type>, IEquatable<TypeCode> {

      #region primitive types
      public static TypeData Null = new TypeData(typeof(DBNull));
      public static TypeData Byte = new TypeData(typeof(byte));
      public static TypeData SByte = new TypeData(typeof(sbyte));
      public static TypeData Char = new TypeData(typeof(char));
      public static TypeData Bool = new TypeData(typeof(bool));
      public static TypeData Int16 = new TypeData(typeof(short));
      public static TypeData Uint16 = new TypeData(typeof(ushort));
      public static TypeData Float = new TypeData(typeof(float));
      public static TypeData Int32 = new TypeData(typeof(int));
      public static TypeData UInt32 = new TypeData(typeof(uint));
      public static TypeData Int64 = new TypeData(typeof(long));
      public static TypeData UInt64 = new TypeData(typeof(ulong));
      public static TypeData Decimal = new TypeData(typeof(decimal));
      public static TypeData Double = new TypeData(typeof(double));
      public static TypeData DateTime = new TypeData(typeof(DateTime));
      public static TypeData Object = new TypeData(typeof(object));
      public static TypeData String = new TypeData(typeof(string));
      #endregion

      public Type Type { get; }
      public TypeCode TypeCode { get; }
      public byte TypeByte => (byte)TypeCode;
      public byte Size { get; }

      internal TypeData(Type type) {
         Type = type;
         TypeCode = Type.GetTypeCode(type);
         Size = TypeCode == TypeCode.Object ? default(byte) : TypeCode.Size();
      }

      public override bool Equals(object obj) {
         if (!(obj is TypeData)) {
            return false;
         }

         return Equals(((TypeData)obj).TypeCode);
      }

      public bool Equals(Type other) {
         return Type.Equals(other);
      }

      public bool Equals(TypeCode other) {
         return TypeCode == other;
      }

      public override int GetHashCode( ) {
         return TypeCode.GetHashCode();
      }

      public override string ToString( ) {
         return Type.ToString();
      }
   }
}
