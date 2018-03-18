using Type = RazorWare.Data.Annotations.DataType;
using Attribs = RazorWare.Data.Annotations.DataAttributes;

namespace RazorWare.Data {
   public struct DataValue {
      private readonly long pid;
      private readonly Type type;
      private readonly Attribs attribs;
      private readonly object value;

      public Type Type => type;
      public Attribs Attributes => attribs;

      public DataValue(Type dataType, Attribs dataAttribs) : this(dataType, dataAttribs, null) { }
      private DataValue(Type dataType, Attribs dataAttribs, object objVal) {
         pid = TimeStamp.Now;
         type = dataType;
         attribs = dataAttribs;
         value = objVal;
      }

      public object Value( ) {
         return value;
      }

      public DataValue Value(object objVal) {
         return new DataValue(type, attribs, objVal);
      }

      public override bool Equals(object obj) {
         if(!(obj is DataValue)) {
            return false;
         }

         var other = (DataValue)obj;
         var valEq = Value()?.Equals(other.Value());

         return valEq.HasValue ? valEq.Value : false;
      }

      public override int GetHashCode( ) {
         var hash = Value().GetHashCode();

         return hash;
      }
   }
}
