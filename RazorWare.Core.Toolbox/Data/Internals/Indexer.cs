using System.Linq;
using System.Collections.Generic;
using Attribs = RazorWare.Data.Annotations.DataAttributes;
using RazorWare.Data.Annotations;

namespace RazorWare.Data {
   internal partial class Internals {

      private class Indexer : IIndexer {
         /* Indexer (and Index(es)) will operate on the premise that there is only 1 PKey. Any
          * other 'keys' are actually configured as Index attributes.
          * 
          * The indexMap keys on IField.
          * ***/
         private readonly Dictionary<int, IIndex> indexMap = new Dictionary<int, IIndex>();
         private readonly Dictionary<string, int> nameIdxMap = new Dictionary<string, int>();

         private readonly ISchema mSchema;

         private DataValue pkeyOrdinal = new DataValue(DataType.Int, Attribs.PKey);

         public IIndex PKey => indexMap[(int)pkeyOrdinal.Value()];
         public IReadOnlyDictionary<string, IIndex> Indexes {
            get {
               return indexMap
                  .Values
                  .ToDictionary(k => k.Name, v => v);
            }
         }

         private Indexer(ISchema schema) {
            mSchema = schema;
            AnnotateIndexes();
         }

         public bool TryGetIndex(DataValue key, out IIndex index) {
            index = default(IIndex);

            if (key.Attributes.HasAttribute(Attribs.PKey)) {
               index = PKey;
            }
            else if (key.Attributes.HasAttribute(Attribs.Index)) {
               // TODO ...test with null value; if NULL then respond with false
               string keyVal = key.Value().ToString() ?? string.Empty;
               int ordinal;

               // first try if key value is an int (ordinal)
               if (int.TryParse(keyVal, out ordinal)) {
                  indexMap.TryGetValue(ordinal, out index);
               }
               // or try if key value is name of index
               else if (!string.IsNullOrEmpty(keyVal)) {
                  if (nameIdxMap.TryGetValue(keyVal, out ordinal)) {
                     indexMap.TryGetValue(ordinal, out index);
                  }
               }
            }

            return index != null;
         }

         private void AnnotateIndexes( ) {
            // if no pkey present, generate a system PKey
            if (!mSchema.Fields.Any(f => f.Attributes.HasAttribute(Attribs.PKey))) {
               // -1 ordinal to signify index is not an actual field (schema member)
               pkeyOrdinal = pkeyOrdinal.Value(-1);
               var field = new Schema.Field(DataType.Int);
               field.Name = "RowKey";
               field.Attributes = Attribs.PKey;

               var indexKey = new Index((int)pkeyOrdinal.Value(), field);
               indexMap.Add(indexKey.Ordinal, indexKey);
               nameIdxMap.Add(indexKey.Name, indexKey.Ordinal);
            }

            // iterate fields now to determine what indexes to build
            var idx = 0;
            while (idx < mSchema.Fields.Count) {
               var field = mSchema.Fields[idx];

               if (field.Is(Attribs.PKey)) {
                  if (pkeyOrdinal.Value() == null) {
                     pkeyOrdinal = pkeyOrdinal.Value(idx);

                     var indexKey = new Index((int)pkeyOrdinal.Value(), field);
                     indexMap.Add(indexKey.Ordinal, indexKey);
                     nameIdxMap.Add(indexKey.Name, indexKey.Ordinal);
                  }
                  else {
                     ++idx;
                     continue;
                  }
               }
               else if (field.Is(Attribs.Index)) {
                  var index = new Index(idx, field);
                  indexMap.Add(idx, index);
                  nameIdxMap.Add(index.Name, index.Ordinal);
               }

               ++idx;
            }
         }
      }

      internal interface IIndexer {
         IIndex PKey { get; }

         bool TryGetIndex(DataValue key, out IIndex index);
      }
   }
}
