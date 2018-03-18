using System.IO;

namespace RazorWare.Data.Annotations {
#pragma warning disable CS0660 // Type defines operator == or operator != but does not override Object.Equals(object o)
#pragma warning disable CS0661 // Type defines operator == or operator != but does not override Object.GetHashCode()
   public unsafe struct DataAttributes {
#pragma warning restore CS0661 // Type defines operator == or operator != but does not override Object.GetHashCode()
#pragma warning restore CS0660 // Type defines operator == or operator != but does not override Object.Equals(object o)
      fixed byte flag[1];

      public static DataAttributes Name = new DataAttributes(0x00);
      public static DataAttributes NotNull = new DataAttributes(0x01);
      public static DataAttributes Default = new DataAttributes(0x02);
      public static DataAttributes PKey = new DataAttributes(0x04);
      public static DataAttributes FKey = new DataAttributes(0x08);
      public static DataAttributes Unique = new DataAttributes(0x10);
      public static DataAttributes Index = new DataAttributes(0x20);
      public static DataAttributes AutoIncr = new DataAttributes(0x40);    // !(String | Text | Blob)
      public static DataAttributes VarLen = new DataAttributes(0x80);      // String | Text | Blob

      private DataAttributes(byte b) {
         fixed (byte* f = flag) {
            *f = b;
         }
      }

      public static implicit operator DataAttributes(byte b) {
         return new DataAttributes(b);
      }

      public static implicit operator byte(DataAttributes a) {
         return *a.flag;
      }

      public static implicit operator string(DataAttributes a) {
         return a.ToString();
      }

      public static bool operator ==(DataAttributes a1, DataAttributes a2) {
         return *a1.flag == *a2.flag;
      }

      public static bool operator !=(DataAttributes a1, DataAttributes a2) {
         return *a1.flag != *a2.flag;
      }

      public static DataAttributes operator |(DataAttributes a1, DataAttributes a2) {
         return *a1.flag |= *a2.flag;
      }

      public static DataAttributes operator &(DataAttributes a1, DataAttributes a2) {
         return *a1.flag &= *a2.flag;
      }

      public static DataAttributes operator ~(DataAttributes a) {
         return (byte)~(*a.flag);
      }

      internal static DataAttributes FromBinaryReader(BinaryReader bReader) {
         var buffer = new byte[1];
         bReader.Read(buffer, 0, 1);

         return buffer[0];
      }

      public override string ToString( ) {
         return $"{ValueExtensions.Name(this)}";
      }
   }
}
