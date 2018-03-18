using System;
using System.Linq;
using RazorWare.Linq;
using static RazorWare.Reflection.ReflectionExtensions;

namespace RazorWare {
   unsafe public struct TimeStamp {
      public static TimeStamp Empty = default(TimeStamp);

      private fixed long mMark[SZLONG];

      private TimeStamp(DateTime dateTime) : this(dateTime.Ticks) { }
      private TimeStamp(long ticks) {
         fixed(long* m = mMark) {
            *m = ticks;
         }
      }

      public static implicit operator TimeStamp(DateTime dateTime) {
         return new TimeStamp(dateTime);
      }

      public static implicit operator byte[](TimeStamp timeStamp) {
         return BitConverter.GetBytes(*timeStamp.mMark);
      }

      public static implicit operator long(TimeStamp timeStamp) {
         return *timeStamp.mMark;
      }

      public static implicit operator DateTime(TimeStamp timeStamp) {
         return new DateTime(*timeStamp.mMark);
      }

      public static bool operator ==(TimeStamp ts1, TimeStamp ts2) {
         return *ts1.mMark == *ts2.mMark;
      }

      public static bool operator !=(TimeStamp ts1, TimeStamp ts2) {
         return *ts1.mMark != *ts2.mMark;
      }

      public static bool TryParse(string value, out TimeStamp timeStamp) {
         timeStamp = default(TimeStamp);
         var parsed = false;

         if (parsed = DateTime.TryParse(value, out DateTime dateTime)) {
            timeStamp = dateTime;
         }

         return parsed;
      }

      public static TimeStamp FromBytes(byte[] buffer) {
         // TODO: error checking - buffer length (long), etc.
         if(buffer.Length != SZLONG) {
            throw new InvalidOperationException($"buffer length [{buffer.Length}] must be [{SZLONG}]");
         }

         return new TimeStamp(BitConverter.ToInt64(buffer, 0));
      }

      public override bool Equals(object obj) {
         if (!(obj is TimeStamp)) {
            return false;
         }

         return this == (TimeStamp)obj;
      }

      public override int GetHashCode( ) {
         return ((long)this).GetHashCode();
      }

      public string ToByteString( ) {
         return $"{string.Join(string.Empty, ((byte[])this).ToArray(b => $"{b:X2}"))}";
      }

      public override string ToString( ) {
         return ((long)this).ToString();
      }

      public string ToString(string format) {
         if(format == "B") {
            return ToByteString();
         }

         DateTime value = this;

         return value.ToString(format);
      }

      public string ToString(IFormatProvider provider) {
         DateTime value = this;

         return value.ToString(provider);
      }

      public static TimeStamp Now {
         get { return new TimeStamp(DateTime.Now); }
      }
   }
}
