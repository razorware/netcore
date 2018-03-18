using System;

namespace RazorWare.Data.Serialization {
#pragma warning disable CS0660 // Type defines operator == or operator != but does not override Object.Equals(object o)
#pragma warning disable CS0661 // Type defines operator == or operator != but does not override Object.GetHashCode()
   internal unsafe struct Tag {
#pragma warning restore CS0661 // Type defines operator == or operator != but does not override Object.GetHashCode()
#pragma warning restore CS0660 // Type defines operator == or operator != but does not override Object.Equals(object o)
      internal const int SZTAG = 1;

      private static readonly byte[] validTags = new byte[] {
         0x00, 0x01, 0X02, 0X03, 0x04
      };

      fixed byte tag[1];

      public static readonly Tag TAG_UNKNOWN = new Tag(0x00);
      public static readonly Tag TAG_SCHEMA = new Tag(0x01);
      public static readonly Tag TAG_CATALOG = new Tag(0x02);
      public static readonly Tag TAG_DATA = new Tag(0x03);

      private Tag(byte b) {
         if (!Array.Exists(validTags, v => v == b)) {
            throw new ArgumentOutOfRangeException($"[{b}] is not a valid value for Tag");
         }

         fixed (byte* t = tag) {
            *(t) = b;
         }
      }

      public static implicit operator byte(Tag t) {
         return *t.tag;
      }

      public static implicit operator byte[](Tag t) {
         return new byte[] { *t.tag };
      }

      public static implicit operator Tag(byte b) {
         return new Tag(b);
      }

      public static bool operator ==(Tag t1, Tag t2) {
         return (*t1.tag) == (*t2.tag);
      }

      public static bool operator !=(Tag t1, Tag t2) {
         return (*t1.tag) != (*t2.tag);
      }

      public static bool operator ==(Tag t1, byte b1) {
         return (*t1.tag) == b1;
      }

      public static bool operator ==(byte b1, Tag t1) {
         return t1 == b1;
      }

      public static bool operator !=(Tag t1, byte b1) {
         return (*t1.tag) != b1;
      }

      public static bool operator !=(byte b1, Tag t1) {
         return t1 != b1;
      }

      public static bool IsValid(Tag t) {
         byte b = *t.tag;

         return Array.Exists(validTags, v => v == b);
      }

      public override string ToString( ) {
         fixed (byte* t = tag) {
            return $"{ValueExtensions.Name(this)}[{*t}]";
         }
      }

   }
}
