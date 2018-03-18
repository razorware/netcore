using System.Collections.Generic;
using Attribs = RazorWare.Data.Annotations.DataAttributes;
using static RazorWare.Data.Annotations.DataAttributes;

namespace RazorWare.Data {
   internal partial class Internals {

      private class Index : IIndex {
         private readonly Dictionary<DataValue, List<int>> idxValMap = new Dictionary<DataValue, List<int>>();

         private readonly int mOrdinal;
         private readonly IField mField;

         public Attribs Attributes => mField.Attributes;
         public string Name => mField.Name;
         public int Ordinal => mOrdinal;

         internal Index(int ordinal, IField field) {
            mOrdinal = ordinal;
            mField = field;
         }

         /// <summary>
         /// Adds a data value to the index row collection if constraints are not violated.
         /// </summary>
         /// <param name="rowId"></param>
         /// <param name="value"></param>
         /// <returns>TRUE if added (value is does not violate constraints); else FALSE</returns>
         public bool Add(int rowId, DataValue value) {
            if(!idxValMap.TryGetValue(value, out List<int> idxList)) {
               idxValMap.Add(value, new List<int> { rowId });

               return true;
            }

            if (mField.Is(Unique)) {
               // value cannot be duplicated
               return false;
            }

            idxList.Add(rowId);

            return true;
         }
      }

      internal interface IIndex {
         Attribs Attributes { get; }
         string Name { get; }
         int Ordinal { get; }

         bool Add(int rowId, DataValue value);
      }
   }
}
